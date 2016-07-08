using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Sharp.Lib.Test {
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
			try {
				Action a = null;
				var bkg = new BackgroundRepeater(a);
				Console.WriteLine("Wtf??");
				bkg.Start();
				Console.WriteLine("Wtf??!!!");
				Assert.Fail("Where ArgumentNullException?");
			} catch (ArgumentNullException) {
				Console.WriteLine("OK: Action == null");
			}

			try {
				Action<CancellationToken> a = null;
				var bkg = new BackgroundRepeater(a);
				Console.WriteLine("Wtf?? (2)");
				bkg.Start();
				Console.WriteLine("Wtf??!!! (2)");
				Assert.Fail("Where ArgumentNullException? (2)");
			} catch (ArgumentNullException) {
				Console.WriteLine("OK: Action<CancellationToken> == null");
			}
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

			Thread.Sleep(TimeSpan.FromSeconds(4D));

			Assert.IsTrue(_counter == 4);
			Assert.IsFalse(bkg.IsStarted());
		}

		[TestMethod, Timeout(5000)]
		public void EverlastingTest() {
			var bkg = new BackgroundRepeater(EverlastingMethod) {
				Cooldown = TimeSpan.FromMilliseconds(10),
			};
			bkg.Start();
			Thread.Sleep(TimeSpan.FromSeconds(.5D));
			bkg.Stop();

			Assert.IsFalse(bkg.IsStarted());
		}

		void SometimesException() {
			if (++_counter % 4 == 0) {
				Console.WriteLine("Throw new Exception()");
				throw new Exception();
			}

			Console.Write("Zzz.. ");
			Thread.Sleep(TimeSpan.FromMilliseconds(1D));
		}

		void EverlastingMethod() {
			while (true) {
				Console.Write("Two week for sleep!");
				Thread.Sleep(TimeSpan.FromDays(14));
			}
		}

		private static readonly Semaphore Sema = new Semaphore(0, 1, nameof(BackgroundRepeater)+"."+nameof(SingleMethod)+"()");
		void SingleMethod() {
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