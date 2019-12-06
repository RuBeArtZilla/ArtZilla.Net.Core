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

		[TestMethod]
		public void TestHumanReadableByteCount() {
			// check min and max values
			Assert.AreEqual("0 B", ulong.MinValue.HumanReadableByteCount());
			Assert.AreEqual("16.0 EB", ulong.MaxValue.HumanReadableByteCount());

			Assert.AreEqual("27 B", 27UL.HumanReadableByteCount());
			Assert.AreEqual("999 B", 999UL.HumanReadableByteCount());
			Assert.AreEqual("1000 B", 1000UL.HumanReadableByteCount());
			Assert.AreEqual("1023 B", 1023UL.HumanReadableByteCount());
			Assert.AreEqual("1.0 KB", 1024UL.HumanReadableByteCount());
			Assert.AreEqual("1.7 KB", 1728UL.HumanReadableByteCount());
			Assert.AreEqual("108.0 KB", 110592UL.HumanReadableByteCount());
			Assert.AreEqual("6.8 MB", 7077888UL.HumanReadableByteCount());
			Assert.AreEqual("432.0 MB", 452984832UL.HumanReadableByteCount());
			Assert.AreEqual("27.0 GB", 28991029248UL.HumanReadableByteCount());
			Assert.AreEqual("1.7 TB", 1855425871872UL.HumanReadableByteCount());
		}

		[TestMethod]
		public void TestHumanReadableByteCountBin() {
			// check min and max values
			Assert.AreEqual("0 B", ulong.MinValue.HumanReadableByteCountBin());
			Assert.AreEqual("16.0 EiB", ulong.MaxValue.HumanReadableByteCountBin());

			Assert.AreEqual("27 B", 27UL.HumanReadableByteCountBin());
			Assert.AreEqual("999 B", 999UL.HumanReadableByteCountBin());
			Assert.AreEqual("1000 B", 1000UL.HumanReadableByteCountBin());
			Assert.AreEqual("1023 B", 1023UL.HumanReadableByteCountBin());
			Assert.AreEqual("1.0 KiB", 1024UL.HumanReadableByteCountBin());
			Assert.AreEqual("1.7 KiB", 1728UL.HumanReadableByteCountBin());
			Assert.AreEqual("108.0 KiB", 110592UL.HumanReadableByteCountBin());
			Assert.AreEqual("6.8 MiB", 7077888UL.HumanReadableByteCountBin());
			Assert.AreEqual("432.0 MiB", 452984832UL.HumanReadableByteCountBin());
			Assert.AreEqual("27.0 GiB", 28991029248UL.HumanReadableByteCountBin());
			Assert.AreEqual("1.7 TiB", 1855425871872UL.HumanReadableByteCountBin());
		}

		[TestMethod]
		public void TestHumanReadableByteCountSI() {
			// check min and max values
			Assert.AreEqual("0 B", ulong.MinValue.HumanReadableByteCountSI());
			Assert.AreEqual("18.4 EB", ulong.MaxValue.HumanReadableByteCountSI());

			Assert.AreEqual("27 B", 27UL.HumanReadableByteCountSI());
			Assert.AreEqual("999 B", 999UL.HumanReadableByteCountSI());
			Assert.AreEqual("1.0 kB", 1000UL.HumanReadableByteCountSI());
			Assert.AreEqual("1.0 kB", 1023UL.HumanReadableByteCountSI());
			Assert.AreEqual("1.0 kB", 1024UL.HumanReadableByteCountSI());
			Assert.AreEqual("1.7 kB", 1728UL.HumanReadableByteCountSI());
			Assert.AreEqual("110.6 kB", 110592UL.HumanReadableByteCountSI());
			Assert.AreEqual("7.1 MB", 7077888UL.HumanReadableByteCountSI());
			Assert.AreEqual("453.0 MB", 452984832UL.HumanReadableByteCountSI());
			Assert.AreEqual("29.0 GB", 28991029248UL.HumanReadableByteCountSI());
			Assert.AreEqual("1.9 TB", 1855425871872UL.HumanReadableByteCountSI());
		}
	}
}