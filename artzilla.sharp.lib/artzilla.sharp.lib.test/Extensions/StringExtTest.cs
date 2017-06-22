using System;
using System.Globalization;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Test.Extensions {
	[TestClass]
	public class StringExtTest {
		private static readonly String ExampleEmpty = String.Empty;
		private const String ExampleEnBase = "Hello World!";
		private const String ExampleEnDown = "hello world!";
		private const String ExampleEnUp = "HELLO WORLD!";
		private const String ExampleNull = null;
		private const String ExampleWhitespace = " ";
		private const String ExampleWhitespaces = "     ";

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
		public void IsGoodTest() {
			Assert.IsFalse(ExampleNull.IsGood());
			Assert.IsFalse(ExampleEmpty.IsGood());
			Assert.IsFalse(ExampleWhitespace.IsGood());
			Assert.IsFalse(ExampleWhitespaces.IsGood());
			Assert.IsTrue(ExampleEnBase.IsGood());
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

			TestParseBool(true.ToString(), true);
			TestParseBool("false", false);

			TestParseDouble("-31", -31D);
			TestParseDouble(0.1D.ToString(), 0.1D);
		}

		private static void TestParseInt(String s, Int32 t) {
			var r = s.ParseIntEx();
			Console.WriteLine($"Compare [{s}] parsed as [{r}] with [{t}]: {r == t}");
			Assert.IsTrue(r == t);
		}

		private static void TestParseBool(String s, Boolean t) {
			var r = s.ParseBoolEx();
			Console.WriteLine($"Compare [{s}] parsed as [{r}] with [{t}]: {r == t}");
			Assert.IsTrue(r == t);
		}

		private static void TestParseDouble(String s, Double t) {
			var r = s.ParseDoubleEx();
			Console.WriteLine($"Compare [{s}] parsed as [{r}] with [{t}]: {r == t}");
			Assert.IsTrue(r == t);
		}
	}
}