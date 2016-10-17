using System;
using ArtZilla.Sharp.Lib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Sharp.Lib.Test.Extensions {
	[TestClass]
	public class StringExtTest {
		private static readonly String ExampleEmpty = String.Empty;
		private const string ExampleEnBase = "Hello World!";
		private const string ExampleEnDown = "hello world!";
		private const string ExampleEnUp = "HELLO WORLD!";
		private const string ExampleNull = null;
		private const string ExampleWhitespace = " ";
		private const string ExampleWhitespaces = "     ";

		[TestMethod]
		public void LikeTest() {
			Assert.IsTrue(ExampleEnBase.Like(ExampleEnBase));
			Assert.IsTrue(ExampleEnBase.Like(ExampleEnDown));
			Assert.IsTrue(ExampleEnBase.Like(ExampleEnUp));

			Assert.IsFalse(ExampleEnBase.Like(ExampleNull));
			Assert.IsFalse(ExampleNull.Like(ExampleEnBase));
			Assert.IsFalse(ExampleNull.Like(ExampleNull));
		}

		[TestMethod]
		public void IsBadTest() {
			Assert.IsTrue(ExampleNull.IsBad());
			Assert.IsTrue(ExampleEmpty.IsBad());
			Assert.IsTrue(ExampleWhitespace.IsBad());
			Assert.IsTrue(ExampleWhitespaces.IsBad());
			Assert.IsFalse(ExampleEnBase.IsBad());
		}

		[TestMethod]
		public void ConvertTest() {
			TestParseInt("", Int32.MinValue);
			TestParseInt("-1", -1);
			TestParseInt("0", 0);
			TestParseInt("1", 1);
			TestParseInt(" 1 ", 1);
			TestParseInt("     9777     ", 9777);
			TestParseInt(Int32.MaxValue.ToString(), Int32.MaxValue);
			TestParseInt(Int32.MinValue.ToString(), Int32.MinValue);
		}

		private static void TestParseInt(String s, Int32 t) {
			var r = s.ParseIntEx();
			Console.WriteLine($"Compare [{s}] parsed as [{r}] with [{t}]: {r == t}");
			Assert.IsTrue(r == t);
		}
	}
}
