using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.BkgOp {
	[TestClass]
	public class BkgRepeaterTests {
		private static Int64 _counter = 0;

		[TestMethod, Timeout(5000)]
		public void MultipleStart() {
			var bkg = new BackgroundRepeater(SingleMethod);
			bkg.Start();
			bkg.Start();
			bkg.Start();
			bkg.Start();
			Assert.IsTrue(bkg.IsStarted());

			bkg.Stop();
			bkg.Stop();
			bkg.Stop();
			Assert.IsFalse(bkg.IsStarted());

			bkg.Start();
			Assert.IsTrue(bkg.IsStarted());
			bkg.Stop();
			Assert.IsFalse(bkg.IsStarted());
			bkg.Stop();
			Assert.IsFalse(bkg.IsStarted());
			bkg.Start();
			bkg.Start();
			bkg.Stop();
			bkg.Stop();

			Assert.IsFalse(bkg.IsStarted());
		}

		[TestMethod, Timeout(5000)]
		public void SelfCancel() {
			var bkg = new BackgroundRepeater(t => {throw new OperationCanceledException();}) {
				Cooldown = TimeSpan.FromMilliseconds(1D),
				IsCatchExceptions = false,
			};
			bkg.Start();
			Thread.Sleep(TimeSpan.FromSeconds(.5D));
			Assert.IsFalse(bkg.IsStarted());
		}

		[TestMethod, Timeout(5000)]
		public void WrongArguments() {
			Action action = () => { };
			Action nullAction = null;

			Action<CancellationToken> cancellableAction = t => { };
			Action<CancellationToken> nullCancellableAction = null;

			ShouldFailOnCreate(() => new BackgroundRepeater(nullAction));
			ShouldFailOnCreate(() => new BackgroundRepeater(nullCancellableAction));

			ShouldFailOnCreate(() => new BackgroundRepeater(action, TimeSpan.FromSeconds(-1D)));
			ShouldFailOnCreate(() => new BackgroundRepeater(cancellableAction, TimeSpan.FromSeconds(-1D)));

			var shouldBeCreated = new[]{
				new BackgroundRepeater(action, TimeSpan.Zero, true),
				new BackgroundRepeater(action, TimeSpan.Zero, false),

				new BackgroundRepeater(cancellableAction, TimeSpan.Zero, true),
				new BackgroundRepeater(cancellableAction, TimeSpan.Zero, false),
			};
		}

		private void ShouldFailOnCreate(Func<BackgroundRepeater> factory, string message = null){
			var isFailed = false;
			try {
				var repeater = factory();
			} catch (Exception e) {
				isFailed = true;
				Console.WriteLine("Constructor's exception: " + e.Message);
			}

			if (!isFailed)
				Assert.Fail(message ?? "Constructor successfully invoked with wrong arguments");
		}


		[TestMethod, Timeout(5000)]
		public void RepeatingWithExceptionsGood() {
			var bkg = new BackgroundRepeater(SometimesException) {
				Cooldown = TimeSpan.FromMilliseconds(1D),
				IsCatchExceptions = true,
			};

			_counter = 0;
			Console.WriteLine(_counter);

			bkg.Start();
			SpinWait.SpinUntil(() => _counter > 100);
			bkg.Stop();

			Console.WriteLine(_counter);

			var c = _counter;
			Thread.Sleep(500);

			if (_counter > c)
				Assert.Fail("repeating not stopped");

			if (4 > c)
				Assert.Fail("where all repeating?");

			Console.WriteLine(_counter);
			Assert.IsFalse(bkg.IsStarted());
		}

		[TestMethod, Timeout(5000)]
		public void RepeatingWithExceptionsFail() {
			var bkg = new BackgroundRepeater(SometimesException) {
				Cooldown = TimeSpan.FromMilliseconds(1D),
				IsCatchExceptions = false,
			};

			_counter = 0;
			bkg.Start();

			SpinWait.SpinUntil(() => _counter == 4);
			Thread.Sleep(TimeSpan.FromSeconds(1D));

			Assert.IsTrue(_counter == 4, $"Not executed properly: {_counter}");
			Assert.IsFalse(bkg.IsStarted());
		}

		[TestMethod, Timeout(5000)]
		public void EverlastingTest() {
			var bkg = new BackgroundRepeater(EverlastingMethod) {
				Cooldown = TimeSpan.FromMilliseconds(10),
			};

			bkg.Enabled(true);
			Thread.Sleep(TimeSpan.FromSeconds(.5D));
			bkg.Enabled(false);
			
			Assert.IsFalse(bkg.IsStarted());
		}

		[TestMethod, Timeout(5000)]
		public void TestCatchExceptions() {
			var bkg = new BackgroundRepeater(TestCatchExceptionsInner);
			bkg.Start();
			Thread.Sleep(TimeSpan.FromSeconds(.5D));
			Assert.IsTrue(bkg.IsStarted());
			bkg.Stop();
			Assert.IsFalse(bkg.IsStarted());
		}
		
		void TestCatchExceptionsInner() {
			Thread.Sleep(TimeSpan.FromMilliseconds(10D));
			throw new OperationCanceledException(null);
		}

		[TestMethod, Timeout(5000)]
		public void TestCatchExceptions2() {
			var bkg = new BackgroundRepeater(TestCatchExceptionsInner2);
			bkg.Start();
			Thread.Sleep(TimeSpan.FromSeconds(.5D));
			Assert.IsTrue(bkg.IsStarted());
			bkg.Stop();
			Assert.IsFalse(bkg.IsStarted());
		}

		void TestCatchExceptionsInner2() {
			Thread.Sleep(TimeSpan.FromMilliseconds(10D));
			throw new Exception(null);
		}


		private Int32 _selfStep;

		[TestMethod, Timeout(5000)]
		public void SelfStopTest() {
			var selfStop = new BackgroundRepeater(SelfStopMethod) {
				Cooldown = TimeSpan.FromMilliseconds(10D),
				IsCatchExceptions = false,
			};

			_selfStep = 0;
			selfStop.Start();

			while (selfStop.IsStarted()) 
				Thread.Sleep(TimeSpan.FromMilliseconds(50D));
				
			Assert.IsTrue(_selfStep == 16, $"Too much repeats: {_selfStep} != 16");
			Assert.IsFalse(selfStop.IsStarted(), "Not stopped.");

			selfStop.IsCatchExceptions = true;
			_selfStep = 0;
			selfStop.Start();

			while (selfStop.IsStarted())
				Thread.Sleep(TimeSpan.FromMilliseconds(50D));

			Assert.IsTrue(_selfStep == 16, $"Too much repeats: {_selfStep} != 16");
			Assert.IsFalse(selfStop.IsStarted(), "Not stopped.");
		}

		private void SelfStopMethod() {
			if (_selfStep == 16) {
				Console.WriteLine("Test failed");
				Assert.Fail();
			}

			if (_selfStep == 15) {
				_selfStep++;
				BackgroundRepeater.InnerStop();
			}

			_selfStep++;
		}


		static void SometimesException() {
			if (++_counter % 4 == 0) {
				Console.WriteLine("Throw new Exception()");
				throw new Exception();
			}

			Console.Write("Zzz.. ");
			Thread.Sleep(TimeSpan.FromMilliseconds(1D));
		}

		static void EverlastingMethod() {
			while (true) {
				Console.Write("Two week for sleep!");
				Thread.Sleep(TimeSpan.FromDays(14));
			}
		}

		private static readonly Semaphore Sema = new Semaphore(0, 1, nameof(BackgroundRepeater)+"."+nameof(SingleMethod)+"()");

		static void SingleMethod() {
			try {
				if (Sema.WaitOne(1))
					Assert.Fail();
				Thread.Sleep(TimeSpan.FromDays(1));
			} finally {
				Sema.Release();
			}
		}
	}
}