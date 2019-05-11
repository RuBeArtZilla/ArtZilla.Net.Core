using System;
using ArtZilla.Net.Core.Extensions;
using ArtZilla.Net.Core.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions {
	[TestClass]
	public class StringEnframeTests {
		[TestMethod]
		public void EnframeNotNullTest() {
			Assert.AreEqual(string.Empty, default(string).EnframeNotNull());
			Assert.AreEqual("Hello world!", "".EnframeNotNull("Hello ", "world!"));
			Assert.AreEqual("Hello world!", " ".EnframeNotNull("Hello", "world!"));
			Assert.AreEqual(" ", " ".EnframeNotNull(null));
			Assert.AreEqual(" ", " ".EnframeNotNull(null, null));
			Assert.AreEqual(" ", " ".EnframeNotNull("", null));
		}

		[TestMethod]
		public void EnframeNotEmptyTest() {
			Assert.AreEqual(string.Empty, default(string).EnframeNotEmpty());
			Assert.AreEqual(string.Empty, string.Empty.EnframeNotEmpty());
			Assert.AreEqual("Hello world!", " ".EnframeNotEmpty("Hello", "world!"));
			Assert.AreEqual(" ", " ".EnframeNotEmpty(null));
			Assert.AreEqual(" ", " ".EnframeNotEmpty(null, null));
			Assert.AreEqual(" ", " ".EnframeNotEmpty("", null));
		}

		[TestMethod]
		public void EnframeTextTest() {
			Assert.AreEqual(string.Empty, default(string).EnframeText());
			Assert.AreEqual(string.Empty, string.Empty.EnframeText());
			Assert.AreEqual(string.Empty, "          ".EnframeText());
			Assert.AreEqual("Hello world!", " world".EnframeText("Hello", "!"));
			Assert.AreEqual("@", "@".EnframeText(null));
			Assert.AreEqual("@", "@".EnframeText(null, null));
			Assert.AreEqual("@", "@".EnframeText("", null));
		}

		[TestMethod]
		public void EnframeIfTest() {
			Assert.AreEqual(string.Empty, default(string).EnframeIf(startsWithSpace));
			Assert.AreEqual(string.Empty, string.Empty.EnframeIf(startsWithSpace));
			Assert.AreEqual(string.Empty, "bad string".EnframeIf(startsWithSpace));
			Assert.AreEqual("Hello world!", " world".EnframeIf(startsWithSpace, "Hello", "!"));
			Assert.AreEqual(" ", " ".EnframeIf(startsWithSpace, null));
			Assert.AreEqual(" ", " ".EnframeIf(startsWithSpace, null, null));
			Assert.AreEqual(" ", " ".EnframeIf(startsWithSpace, "", null));

			AssertEx.IsFailWith<ArgumentNullException>(() => "".EnframeIf(default));

			bool startsWithSpace(string s) => s?.StartsWith(" ") ?? false;
		}
	}
}