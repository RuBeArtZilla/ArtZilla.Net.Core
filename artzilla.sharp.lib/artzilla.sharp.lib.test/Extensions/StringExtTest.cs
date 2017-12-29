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

		[TestMethod]
		public void CombineTests() {
			const string a = "abc";
			const string b = "def";
			const string c = "g";
			const string x = "...";
			const string axb = a + x + b;
			const string axbxc = a + x + b + x + c;
			const string sn = null;
			const string se = "";
			const string sw = "   ";

			Assert.AreEqual(a, a.Combine(x, sn));
			Assert.AreEqual(a, a.Combine(x, se));
			Assert.AreEqual(a, a.Combine(x, sw));

			Assert.AreEqual(a, sn.Combine(x, a));
			Assert.AreEqual(a, se.Combine(x, a));
			Assert.AreEqual(a, sw.Combine(x, a));

			Assert.AreEqual(axb, a.Combine(x, b));
			Assert.AreEqual(axb, a.Combine(x, sn, b));
			Assert.AreEqual(axb, a.Combine(x, b, sn));

			Assert.AreEqual(axb, a.Combine(x, sw, b));
			Assert.AreEqual(axb, a.Combine(x, b, sw));

			Assert.AreEqual(axb, sn.Combine(x, se, a, sw, b, sn));
			Assert.AreEqual(axb, sn.Combine(x, se, a, sw, b, sn));

			Assert.AreEqual(axbxc, a.Combine(x, b, c));
			Assert.AreEqual(axbxc, sn.Combine(x, se, a, sw, b, sn, sw, c));
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