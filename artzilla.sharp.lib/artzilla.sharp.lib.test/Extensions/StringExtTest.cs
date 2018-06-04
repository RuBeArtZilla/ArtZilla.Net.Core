using System;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Test.Extensions {
	[TestClass]
	public class StringExtTest {
		private static readonly string ExampleEmpty = string.Empty;
		private const string ExampleEnBase = "Hello World!";
		private const string ExampleEnDown = "hello world!";
		private const string ExampleEnUp = "HELLO WORLD!";
		private const string ExampleShort = "Hello";
		private const string ExampleLarge = "Hello World!Hello World!Hello World!";
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
			Assert.IsFalse(ExampleNull.Like(ExampleNull)); // should be false!

			Assert.IsFalse(ExampleEnBase.Like(ExampleShort));
			Assert.IsFalse(ExampleEnBase.Like(ExampleLarge));

			Assert.IsFalse(ExampleShort.Like(ExampleEnBase));
			Assert.IsFalse(ExampleLarge.Like(ExampleEnBase));
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
		public void IsNullOrEmptyTest() {
			Assert.IsTrue(ExampleNull.IsNullOrEmpty());
			Assert.IsTrue(ExampleEmpty.IsNullOrEmpty());
			Assert.IsFalse(ExampleWhitespace.IsNullOrEmpty());
			Assert.IsFalse(ExampleWhitespaces.IsNullOrEmpty());
			Assert.IsFalse(ExampleEnBase.IsNullOrEmpty());
		}

		[TestMethod]
		public void IsNotNullOrEmptyTest() {
			Assert.IsFalse(ExampleNull.IsNotNullOrEmpty());
			Assert.IsFalse(ExampleEmpty.IsNotNullOrEmpty());
			Assert.IsTrue(ExampleWhitespace.IsNotNullOrEmpty());
			Assert.IsTrue(ExampleWhitespaces.IsNotNullOrEmpty());
			Assert.IsTrue(ExampleEnBase.IsNotNullOrEmpty());
		}

		[TestMethod]
		public void IsNullOrWhiteSpaceTest() {
			Assert.IsTrue(ExampleNull.IsNullOrWhiteSpace());
			Assert.IsTrue(ExampleEmpty.IsNullOrWhiteSpace());
			Assert.IsTrue(ExampleWhitespace.IsNullOrWhiteSpace());
			Assert.IsTrue(ExampleWhitespaces.IsNullOrWhiteSpace());
			Assert.IsFalse(ExampleEnBase.IsNullOrWhiteSpace());
		}

		[TestMethod]
		public void IsNotNullOrWhiteSpaceTest() {
			Assert.IsFalse(ExampleNull.IsNotNullOrWhiteSpace());
			Assert.IsFalse(ExampleEmpty.IsNotNullOrWhiteSpace());
			Assert.IsFalse(ExampleWhitespace.IsNotNullOrWhiteSpace());
			Assert.IsFalse(ExampleWhitespaces.IsNotNullOrWhiteSpace());
			Assert.IsTrue(ExampleEnBase.IsNotNullOrWhiteSpace());
		}

		[TestMethod]
		public void ConvertTest() {
			TestParseInt("", int.MinValue);
			TestParseInt("-1", -1);
			TestParseInt("0", 0);
			TestParseInt("1", 1);
			TestParseInt(" 1 ", 1);
			TestParseInt("     9777     ", 9777);
			TestParseInt(int.MaxValue.ToString(), int.MaxValue);
			TestParseInt(int.MinValue.ToString(), int.MinValue);

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

		[TestMethod]
		public void ExtractTest() {
			const string example = "-+zXzxYXYiXI[(abc)][(cba)]*/";

			Assert.AreEqual("abc", example.Extract("(", ")"));
			Assert.AreEqual("abc", example.Extract("[(", ")"));
			Assert.AreEqual("abc", example.Extract("(", ")]"));
			Assert.AreEqual("", example.Extract("-", "+"));
			Assert.AreEqual("", example.Extract("*", "/"));
			Assert.AreEqual(null, example.Extract("hello", ""));
			Assert.AreEqual(null, example.Extract("", "hello"));

			Assert.AreEqual("X", example.Extract("z", "z"));
			Assert.AreEqual("z", example.Extract("X", "x"));
			Assert.AreEqual("z", example.Extract("x", "X", StringComparison.OrdinalIgnoreCase));

			Assert.AreEqual("X", example.Extract("i", "I"));
			Assert.AreEqual("X", example.Extract("i", "i", StringComparison.OrdinalIgnoreCase));

			// todo:...
		}

		[TestMethod]
		public void ExtractLastTest() {
			// todo: ...
		}

		private static void TestParseInt(string s, int t) {
			var r = s.ParseIntEx();
			Console.WriteLine($"Compare [{s}] parsed as [{r}] with [{t}]: {r == t}");
			Assert.IsTrue(r == t);
		}

		private static void TestParseBool(string s, bool t) {
			var r = s.ParseBoolEx();
			Console.WriteLine($"Compare [{s}] parsed as [{r}] with [{t}]: {r == t}");
			Assert.IsTrue(r == t);
		}

		private static void TestParseDouble(string s, double t) {
			var r = s.ParseDoubleEx();
			Console.WriteLine($"Compare [{s}] parsed as [{r}] with [{t}]: {r == t}");
			Assert.IsTrue(r == t);
		}
	}
}