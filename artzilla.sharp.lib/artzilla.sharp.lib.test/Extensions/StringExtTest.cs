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

		// TODO: finish extract tests
		[TestMethod]
		public void ExtractsArgumentsTest() {
			// null border throw exception
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(out var r, null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(out var r, null));

			// empty border throw exception
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract(""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract(out var r, ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast(""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast(out var r, ""));

			// null op throw exception
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(null, "("));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(out var r, null, "("));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(null, "("));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(out var r, null, "("));

			// null ed throw exception
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract("(", null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(out var r, "(", null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast("(", null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(out var r, "(", null));

			// null input throw exception
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(null, null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(out var r, null, null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(null, null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(out var r, null, null));

			// empty op throw exception
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract("", "a"));
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract(out var r, "", "a"));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast("", "a"));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast(out var r, "", "a"));

			// empty ed throw exception
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract("a", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract(out var r, "a", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast("a", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast(out var r, "a", ""));

			// empty input throw exception
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract("", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract(out var r, "", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast("", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast(out var r, "", ""));

			// null input return default string.
			const string def = default;
			string x;

			Assert.AreEqual(def, ((string)null).Extract("("));
			Assert.AreEqual(def, ((string)null).Extract("(", ")"));

			Assert.AreEqual(def, ((string)null).Extract(out x, "("));
			Assert.AreEqual(null, x); // should be same as input

			Assert.AreEqual(def, ((string)null).Extract(out x, "(", ")"));
			Assert.AreEqual(null, x); // should be same as input

			Assert.AreEqual(def, ((string)null).ExtractLast("("));
			Assert.AreEqual(def, ((string)null).ExtractLast("(", ")"));

			Assert.AreEqual(def, ((string)null).ExtractLast(out x, "("));
			Assert.AreEqual(null, x); // should be same as input

			Assert.AreEqual(def, ((string)null).ExtractLast(out x, "(", ")"));
			Assert.AreEqual(null, x); // should be same as input
		}

		[TestMethod]
		public void ExtractTest() {
			const string def = default;

			Assert.AreEqual("yes", "(yes)".Extract("(", ")")); // typical usage
			Assert.AreEqual("yes", ")))(yes)(((a)))(((".Extract("(", ")")); // op ed repeated
			Assert.AreEqual("", ")()(".Extract("(", ")")); // empty substring

			Assert.AreEqual(def, ")(no".Extract("(", ")")); // ed before op at begin
			Assert.AreEqual(def, "(no".Extract("(", ")"));  // only op at begin
			Assert.AreEqual(def, "no)".Extract("(", ")"));  // only op at end
			Assert.AreEqual(def, "(".Extract("(", ")"));     // only op

			Assert.AreEqual(def, "no)(".Extract("(", ")")); // ed before op at end
			Assert.AreEqual(def, "no)".Extract("(", ")"));  // only ed at end
			Assert.AreEqual(def, ")no".Extract("(", ")"));  // only ed at begin
			Assert.AreEqual(def, ")".Extract("(", ")"));     // only ed

			Assert.AreEqual("yes", "ayesc".Extract("a", "c"));     // 
			Assert.AreEqual("yes", "nooyeson".Extract("noo", "on"));  // with op & ed where lenght > 1
			Assert.AreEqual("yes", "nooyesoo".Extract("noo", "oo")); // where op has ed as substring

			Assert.AreEqual("yes", "XxyesXx".Extract("x", "X")); // casing
			Assert.AreEqual("yes", "XyesxXx".Extract("x", "X", StringComparison.OrdinalIgnoreCase)); // casing
		}

		[TestMethod]
		public void ExtractOutTest() {
			const string def = default;
			string x;

			Assert.AreEqual("yes", "☆(yes)★".Extract(out x, "(", ")")); // typical usage
			Assert.AreEqual("☆★", x);

			Assert.AreEqual("yes", ")))(yes)(((a)))(((".Extract(out x, "(", ")")); // op ed repeated
			Assert.AreEqual(")))(((a)))(((", x);

			// remove this ↓

			Assert.AreEqual("", ")()(".Extract(out x, "(", ")")); // empty substring

			Assert.AreEqual(def, ")(no".Extract(out x, "(", ")")); // ed before op at begin
			Assert.AreEqual(def, "(no".Extract(out x, "(", ")"));  // only op at begin
			Assert.AreEqual(def, "no)".Extract(out x, "(", ")"));  // only op at end
			Assert.AreEqual(def, "(".Extract(out x, "(", ")"));     // only op

			Assert.AreEqual(def, "no)(".Extract(out x, "(", ")")); // ed before op at end
			Assert.AreEqual(def, "no)".Extract(out x, "(", ")"));  // only ed at end
			Assert.AreEqual(def, ")no".Extract(out x, "(", ")"));  // only ed at begin
			Assert.AreEqual(def, ")".Extract(out x, "(", ")"));     // only ed

			Assert.AreEqual("yes", "ayesc".Extract(out x, "a", "c"));     // 
			Assert.AreEqual("yes", "nooyeson".Extract(out x, "noo", "on"));  // with op & ed where lenght > 1
			Assert.AreEqual("yes", "nooyesoo".Extract(out x, "noo", "oo")); // where op has ed as substring

			Assert.AreEqual("yes", "XxyesXx".Extract(out x, "x", "X")); // casing
			Assert.AreEqual("yes", "XyesxXx".Extract(out x, "x", "X", StringComparison.OrdinalIgnoreCase)); // casing
		}

		[TestMethod]
		public void ExtractLastTest() {
			const string def = default;

			Assert.AreEqual("yes", "(yes)".ExtractLast("(", ")")); // typical usage
			Assert.AreEqual("yes", ")))(no)(((yes)(((".ExtractLast("(", ")")); // op ed repeated
			Assert.AreEqual("", ")()(".ExtractLast("(", ")")); // empty substring

			Assert.AreEqual(def, ")(no".ExtractLast("(", ")")); // ed before op at begin
			Assert.AreEqual(def, "(no".ExtractLast("(", ")"));  // only op at begin
			Assert.AreEqual(def, "no)".ExtractLast("(", ")"));  // only op at end
			Assert.AreEqual(def, "(".ExtractLast("(", ")"));     // only op

			Assert.AreEqual(def, "no)(".ExtractLast("(", ")")); // ed before op at end
			Assert.AreEqual(def, "no)".ExtractLast("(", ")"));  // only ed at end
			Assert.AreEqual(def, ")no".ExtractLast("(", ")"));  // only ed at begin
			Assert.AreEqual(def, ")".ExtractLast("(", ")"));     // only ed

			Assert.AreEqual("yes", "ayesc".ExtractLast("a", "c"));     // 
			Assert.AreEqual("yes", "nooyeson".ExtractLast("noo", "on"));  // with op & ed where lenght > 1
			Assert.AreEqual("yes", "nooyesoo".ExtractLast("noo", "oo")); // where op has ed as substring

			Assert.AreEqual("yes", "XxyesXx".ExtractLast("x", "X")); // casing
			Assert.AreEqual("yes", "xXxyesX".ExtractLast("x", "X", StringComparison.OrdinalIgnoreCase));
		}

		[TestMethod]
		public void ExtractLastOpEdTest() {
			const string def = default;
			string x;

			Test("☆(yes)★", "(", ")", "yes", "☆★"); // typical usage
			Test(")))(no)(((yes)(((", "(", ")", "yes", ")))(no)((((("); // op ed repeated
			Test(")()(", "(", ")", "", ")("); // empty substring
			Test("()", "(", ")", "", ""); // empty substring and remainder

			Test(")(no", "(", ")", def, ")(no"); // ed before op at begin
			Test("(no", "(", ")", def, "(no"); // only op at begin
			Test("no)", "(", ")", def, "no)"); // only op at end
			Test("(", "(", ")", def, "(");     // only op

			Test("nooyeson", "noo", "on", "yes", ""); // with op & ed where lenght > 1
			Test("nooyesoo", "noo", "oo", "yes", ""); // where op has ed as substring
			Test("ooyesnoo", "oo", "noo", "yes", ""); // where ed has op as substring

			// casing
			Test("XxyesXx", "x", "X", "yes", "Xx"); // where ed has op as substring
			Test("xXxyesX", "x", "X", "yes", "xX", StringComparison.OrdinalIgnoreCase); // where ed has op as substring

			void Test(string source, string op, string ed, string result, string remainder, StringComparison comp = StringComparison.Ordinal) {
				Assert.AreEqual(result, source.ExtractLast(op, ed, comp));
				Assert.AreEqual(result, source.ExtractLast(out var z, op, ed, comp));
				Assert.AreEqual(remainder, z);
			}
		}

		[TestMethod]
		public void ExtractLastBrdTest() {
			const string def = default;
			string x;

			Test("☆''yes''★", "''", "yes", "☆★"); // typical usage
			Test(")''no''((''yes''('", "''", "yes", ")''no''((('"); // br repeated
			Test("★☆☆★", "☆", "", "★★"); // empty substring
			Test("☆☆", "☆", "", ""); // empty substring and remainder

			Test("☆no", "☆", def, "☆no"); // only op at begin
			Test("no☆", "☆", def, "no☆"); // only op at end
			Test("☆", "☆", def, "☆");     // only op

			// casing
			Test("XXyesXx", "X", "yes", "Xx"); // where ed has op as substring
			Test("xXxyesX", "X", "yes", "xX", StringComparison.OrdinalIgnoreCase); // where ed has op as substring

			void Test(string source, string br, string result, string remainder, StringComparison comp = StringComparison.Ordinal) {
				Assert.AreEqual(result, source.ExtractLast(br, comp));
				Assert.AreEqual(result, source.ExtractLast(out var z, br, comp));
				Assert.AreEqual(remainder, z);
			}
		}

		private static void TestParseInt(string s, int t) {
			var r = s.ParseIntEx();
			Console.WriteLine($"Compare [{s}] parsed as [{r}] with [{t}]: {r == t}");
			Assert.AreEqual(t, r);
		}

		private static void TestParseBool(string s, bool t) {
			var r = s.ParseBoolEx();
			Console.WriteLine($"Compare [{s}] parsed as [{r}] with [{t}]: {r == t}");
			Assert.AreEqual(t, r);
		}

		private static void TestParseDouble(string s, double t) {
			var r = s.ParseDoubleEx();
			Console.WriteLine($"Compare [{s}] parsed as [{r}] with [{t}]: {r == t}");
			Assert.AreEqual(t, r, double.Epsilon);
		}
	}
}