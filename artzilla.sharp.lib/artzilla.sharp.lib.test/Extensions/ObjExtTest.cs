using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ArtZilla.Net.Core.Extensions;
using ArtZilla.Net.Core.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions {
	[TestClass]
	public class ObjExtTests {
		[TestMethod]
		public void TestIsNull() {
			object o = null;
			Assert.IsTrue(o.IsNull());
			o = new { In = 128, Out = 256 };
			Assert.IsFalse(o.IsNull());
		}

		[TestMethod]
		public void TestIsAnyOf() {
			Assert.IsTrue(42.IsAnyOf(4, 8, 15, 16, 23, 42));
			Assert.IsTrue("Luke".IsAnyOf("Jedi", "Sith", "Luke"));
			Assert.IsFalse("Luke".IsAnyOf("Jedi", "Sith"));

			var comparer = new FirstCharOfStringCustomEqualityComparer();
			Assert.IsTrue("Luke".IsAnyOf(comparer, "Jedi", "Sith", "Luke"));
			Assert.IsTrue("Luke".IsAnyOf(comparer, "Jedi", "Sith", "Leia"));
			Assert.IsFalse("Luke".IsAnyOf(comparer, "Jedi", "Sith"));
			Assert.IsFalse("Luke".IsAnyOf(comparer, "Jedi", "Sith", "leia"));
		}

		[TestMethod]
		public void TestToEnumerable() {
			Assert.IsTrue(42.ToEnumerable().Any());
			Assert.IsTrue(42.ToEnumerable().First() == 42);
			Assert.IsTrue(42.ToEnumerable().ToArray().Length == 1);
		}

		[TestMethod]
		public void TestIsInOpenInterval() {
			Assert.IsTrue(42.IsInOpenInterval(-100, 100));
			Assert.IsTrue(42.IsInOpenInterval(41, 43));
			Assert.IsTrue(42.IsInOpenInterval(43, 41));
			Assert.IsTrue(42.IsInOpenInterval(43));

			Assert.IsFalse(42.IsInOpenInterval(42, 43));
			Assert.IsFalse(42.IsInOpenInterval(41, 42));
			Assert.IsFalse(42.IsInOpenInterval(-43, -41));
			Assert.IsFalse(42.IsInOpenInterval(-41, -43));
			Assert.IsFalse(42.IsInOpenInterval(42, 42));
		}

		[TestMethod]
		public void TestIsInClosedInterval() {
			Assert.IsTrue(42.IsInClosedInterval(-100, 100));
			Assert.IsTrue(42.IsInClosedInterval(41, 43));
			Assert.IsTrue(42.IsInClosedInterval(43, 41));
			Assert.IsTrue(42.IsInClosedInterval(42, 43));
			Assert.IsTrue(42.IsInClosedInterval(41, 42));
			Assert.IsTrue(42.IsInClosedInterval(42, 42));
			Assert.IsTrue(42.IsInClosedInterval(43));
			Assert.IsTrue(42.IsInClosedInterval(42));

			Assert.IsFalse(42.IsInClosedInterval(-43, -41));
			Assert.IsFalse(42.IsInClosedInterval(-41, -43));
			Assert.IsFalse(42.IsInClosedInterval(41));
		}

		[TestMethod]
		public void TestInClosedInterval() {
			Assert.AreEqual(42, 42.InClosedInterval(0, 100));
			Assert.AreEqual(42, 42.InClosedInterval(100, 0));

			Assert.AreEqual(42, 42.InClosedInterval(0, 42));
			Assert.AreEqual(42, 42.InClosedInterval(42, 0));

			Assert.AreEqual(42, 42.InClosedInterval(42, 42));

			Assert.AreEqual(42, 0.InClosedInterval(42, 100));
			Assert.AreEqual(42, 0.InClosedInterval(42, 42));
			Assert.AreEqual(42, 0.InClosedInterval(100, 42));

			Assert.AreEqual(42, 100.InClosedInterval(0, 42));
			Assert.AreEqual(42, 100.InClosedInterval(42, 42));
			Assert.AreEqual(42, 100.InClosedInterval(42, 0));

			Assert.AreEqual(42, 100.InClosedInterval(42));
			Assert.AreEqual(default, default(int).InClosedInterval(default));

			Assert.AreEqual('b', 'b'.InClosedInterval('a', 'c'));
			Assert.AreEqual('b', 'a'.InClosedInterval('b', 'c'));
			Assert.AreEqual('b', 'c'.InClosedInterval('a', 'b'));
		}

		class FirstCharOfStringCustomEqualityComparer: IEqualityComparer<string> {
			public bool Equals(string x, string y) => x[0] == y[0]; // skip null check!
			public int GetHashCode(string obj) => obj[0].GetHashCode();
		}

		private struct ExampleStruct {
			public int Value;
			public bool Flag;

			public ExampleStruct(int value, bool flag) {
				Value = value;
				Flag = flag;
			}
		}

		private class ExampleClass {
			public int Value;
			public bool Flag;

			public ExampleClass(int value, bool flag) {
				Value = value;
				Flag = flag;
			}
		}

		[TestMethod]
		public void TestWithStruct() {
			var x = new ExampleStruct(0, false);

			Assert.IsFalse(x.Flag);
			Assert.AreEqual(0, x.Value);

			x.With((ref ExampleStruct s) => s.Flag = true)
			 .With((ref ExampleStruct s) => s.Value = 42);

			Assert.IsTrue(x.Flag);
			Assert.AreEqual(42, x.Value);
		}

		[TestMethod]
		public void TestWithClass() {
			var x = new ExampleClass(0, false);

			Assert.IsFalse(x.Flag);
			Assert.AreEqual(0, x.Value);

			x.With(s => s.Flag = true)
			 .With(s => s.Value = 42);

			Assert.IsTrue(x.Flag);
			Assert.AreEqual(42, x.Value);
		}

		[TestMethod]
		public void TestElapsedSeconds() {
			AssertEx.IsFailWith<NullReferenceException>(() => default(Stopwatch).ElapsedSeconds());

			var sw = Stopwatch.StartNew();
			Thread.Sleep(TimeSpan.FromMilliseconds(100));
			sw.Stop();
			Assert.IsTrue(sw.ElapsedMilliseconds > 0);
			Assert.IsTrue(sw.ElapsedSeconds() > 0);
			Assert.IsTrue(Math.Abs(sw.ElapsedMilliseconds - sw.ElapsedSeconds() * 1000D) < double.Epsilon);
		}
	}
}
