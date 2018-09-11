using System;
using System.Collections.Generic;
using System.Linq;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions {
	[TestClass]
	public class IEnumerableExtTests {
		[TestMethod]
		public void DoBeforeTest() {
			var arr = new int[] { 4, 8, 15, 16, 23, 42 };
			var i = 0;
			void action(int j) => ++i;
			arr.DoBefore(action).ToArray();
			Assert.IsTrue(arr.Length == i, $"Wanted={arr.Length}, Actual={i}");
		}

		[TestMethod]
		public void AppendTest() {
			var ideal = new int[] { 4, 8, 15, 16, 23, 42 };

			var test1 = new int[] { 4, 8, 15, 16, 23 };
			var test2 = new int[] { 4, 8, 15, 16 };
			var test3 = new int[] { 4, 8, 15 };

			AssertSameEnumerables(ideal, ideal.Append());
			AssertSameEnumerables(ideal, test1.Append(42));
			AssertSameEnumerables(ideal, test2.Append(23, 42));
			AssertSameEnumerables(ideal, test3.Append(16, 23, 42));
		}

		[TestMethod]
		public void CombineTest() {
			Assert.AreEqual("4 8 15 16 23 42", new[] { 4, 8, 15, 16, 23, 42 }.Combine(" "));
			Assert.AreEqual("", new[] { null, "", "   ", null }.Combine());
		}

		public static void AssertSameEnumerables<T>(IEnumerable<T> first, IEnumerable<T> second) {
			var i1 = first.GetEnumerator();
			var i2 = second.GetEnumerator();
			bool hasNext;
			while (true) {
				hasNext = i1.MoveNext();
				Assert.AreEqual(hasNext, i2.MoveNext());
				if (!hasNext) return;
				Assert.AreEqual(i1.Current, i2.Current);
			}
		}
	}
}

