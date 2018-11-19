using System;
using ArtZilla.Net.Core.Extensions;
using ArtZilla.Net.Core.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions {
	[TestClass]
	public class StringExtractTests {
		private const string StrNull = null;
		private const string StrEmpty = "";
		private const string StrSpace = " ";

		[TestMethod]
		public void ExtractsArgumentsTest() {
			// null border throw exception
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(out _, null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(out _, null));

			// empty border throw exception
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract(""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract(out _, ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast(""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast(out _, ""));

			// null op throw exception
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(null, "("));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(out _, null, "("));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(null, "("));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(out _, null, "("));

			// null ed throw exception
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract("(", null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(out _, "(", null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast("(", null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(out _, "(", null));

			// null input throw exception
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(null, null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".Extract(out _, null, null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(null, null));
			AssertEx.IsFailWith<ArgumentNullException>(() => "x".ExtractLast(out _, null, null));

			// empty op throw exception
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract("", "a"));
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract(out _, "", "a"));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast("", "a"));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast(out _, "", "a"));

			// empty ed throw exception
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract("a", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract(out _, "a", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast("a", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast(out _, "a", ""));

			// empty input throw exception
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract("", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".Extract(out _, "", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast("", ""));
			AssertEx.IsFailWith<ArgumentException>(() => "x".ExtractLast(out _, "", ""));

			// null input return default string.
			const string def = default;

			Assert.AreEqual(def, ((string)null).Extract("("));
			Assert.AreEqual(def, ((string)null).Extract("(", ")"));

			Assert.AreEqual(def, ((string)null).Extract(out var x, "("));
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

			Assert.AreEqual("yes", "☆(yes)★".Extract(out var x, "(", ")")); // typical usage
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

		[TestMethod]
		public void ExtractFirstWordTest() {
			const string partL = "Homura";
			const string partR = "Akemi";
			const string delimeter = " ";
			const string withDelimeter = partL + delimeter + partR;
			const string lDelimeter = partL + delimeter;
			const string rDelimeter = delimeter + partR;
			const string withoutDelimeter = partL + partR;
			
			const string noResult = "";
			const string noRemainder = "";
			
			Assert.AreEqual(partL, withDelimeter.ExtractFirstWord(out var remainder));
			Assert.AreEqual(partR, remainder);

			Assert.AreEqual(partL, lDelimeter.ExtractFirstWord(out remainder));
			Assert.AreEqual(noRemainder, remainder);

			Assert.AreEqual(noResult, rDelimeter.ExtractFirstWord(out remainder));
			Assert.AreEqual(partR, remainder);

			Assert.AreEqual(noResult, delimeter.ExtractFirstWord(out remainder));
			Assert.AreEqual(noRemainder, remainder);

			Assert.AreEqual(withoutDelimeter, withoutDelimeter.ExtractFirstWord(out remainder));
			Assert.AreEqual(noRemainder, remainder);

			Assert.IsNotNull(((string) null).ExtractFirstWord(out remainder));
			Assert.IsNotNull(remainder);
		}

		[TestMethod]
		public void ExtractFirstWordWithCustomDelimeterTest() {
			const string partL = "Homura";
			const string partR = "Akemi";
			const string delimeter = "＿★＿";
			const string withDelimeter = partL + delimeter + partR;
			const string lDelimeter = partL + delimeter;
			const string rDelimeter = delimeter + partR;
			const string withoutDelimeter = partL + partR;
			
			const string noResult = "";
			const string noRemainder = "";
			
			Assert.AreEqual(partL, withDelimeter.ExtractFirstWord(out var remainder, delimeter));
			Assert.AreEqual(partR, remainder);

			Assert.AreEqual(partL, lDelimeter.ExtractFirstWord(out remainder, delimeter));
			Assert.AreEqual(noRemainder, remainder);

			Assert.AreEqual(noResult, rDelimeter.ExtractFirstWord(out remainder, delimeter));
			Assert.AreEqual(partR, remainder);

			Assert.AreEqual(noResult, delimeter.ExtractFirstWord(out remainder, delimeter));
			Assert.AreEqual(noRemainder, remainder);

			Assert.AreEqual(withoutDelimeter, withoutDelimeter.ExtractFirstWord(out remainder, delimeter));
			Assert.AreEqual(noRemainder, remainder);

			Assert.IsNotNull(StrNull.ExtractFirstWord(out remainder, delimeter));
			Assert.IsNotNull(remainder);

			AssertEx.IsFailWith<ArgumentNullException>(() => withDelimeter.ExtractFirstWord(out _, StrNull));
			AssertEx.IsFailWith<ArgumentNullException>(() => StrNull.ExtractFirstWord(out _, StrNull));
		}

		[TestMethod]
		public void ExtractLastWordTest() {
			const string partL = "Homura";
			const string partR = "Akemi";
			const string delimeter = " ";
			const string withDelimeter = partL + delimeter + partR;
			const string lDelimeter = partL + delimeter;
			const string rDelimeter = delimeter + partR;
			const string withoutDelimeter = partL + partR;
			
			const string noResult = "";
			const string noRemainder = "";
			
			Assert.AreEqual(partR, withDelimeter.ExtractLastWord(out var remainder));
			Assert.AreEqual(partL, remainder);

			Assert.AreEqual(noRemainder, lDelimeter.ExtractLastWord(out remainder));
			Assert.AreEqual(partL, remainder);

			Assert.AreEqual(partR, rDelimeter.ExtractLastWord(out remainder));
			Assert.AreEqual(noResult, remainder);

			Assert.AreEqual(noResult, delimeter.ExtractLastWord(out remainder));
			Assert.AreEqual(noRemainder, remainder);

			Assert.AreEqual(withoutDelimeter, withoutDelimeter.ExtractLastWord(out remainder));
			Assert.AreEqual(noRemainder, remainder);

			Assert.IsNotNull(StrNull.ExtractLastWord(out remainder));
			Assert.IsNotNull(remainder);
		}

		[TestMethod]
		public void ExtractLastWordWithCustomDelimeterTest() {
			const string partL = "Homura";
			const string partR = "Akemi";
			const string delimeter = "＿★＿";
			const string withDelimeter = partL + delimeter + partR;
			const string lDelimeter = partL + delimeter;
			const string rDelimeter = delimeter + partR;
			const string withoutDelimeter = partL + partR;
			
			const string noResult = "";
			const string noRemainder = "";
			
			Assert.AreEqual(partR, withDelimeter.ExtractLastWord(out var remainder, delimeter));
			Assert.AreEqual(partL, remainder);

			Assert.AreEqual(noResult, lDelimeter.ExtractLastWord(out remainder, delimeter));
			Assert.AreEqual(partL, remainder);

			Assert.AreEqual(partR, rDelimeter.ExtractLastWord(out remainder, delimeter));
			Assert.AreEqual(noRemainder, remainder);

			Assert.AreEqual(noResult, delimeter.ExtractLastWord(out remainder, delimeter));
			Assert.AreEqual(noRemainder, remainder);

			Assert.AreEqual(withoutDelimeter, withoutDelimeter.ExtractLastWord(out remainder, delimeter));
			Assert.AreEqual(noRemainder, remainder);

			Assert.IsNotNull(StrNull.ExtractLastWord(out remainder, delimeter));
			Assert.IsNotNull(remainder);

			AssertEx.IsFailWith<ArgumentNullException>(() => withDelimeter.ExtractLastWord(out _, StrNull));
			AssertEx.IsFailWith<ArgumentNullException>(() => StrNull.ExtractLastWord(out _, StrNull));
		}
	}
}