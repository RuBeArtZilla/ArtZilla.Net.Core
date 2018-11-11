using System;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions {
	[TestClass]
	public class NumExtTests {
		[TestMethod]
		public void TestMakeLong() {
			var original = 1979205471486323557L;
			var left = (int) (original >> 32);
			var right = (int) (original & 0xffffffffL);

			var correct = NumberUtils.MakeLong(left, right);

			var incorrect1 = (long) (((long) left << 32) | (long) right);
			var incorrect2 = ((long) left << 32) | (long) right;
			var incorrect3 = (long) (left * uint.MaxValue) + right;
			var incorrect4 = (long) (left * 0x100000000) + right;

			Assert.AreEqual(original, correct);
			Assert.AreNotEqual(original, incorrect1);
			Assert.AreNotEqual(original, incorrect2);
			Assert.AreNotEqual(original, incorrect3);
			Assert.AreNotEqual(original, incorrect4);
		}
	}
}