using System;
using ArtZilla.Sharp.Lib.Extenstions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Sharp.Lib.Test.Extensions {
	[TestClass]
	public class StringExtTest {
		private const string ExampleEnBase = "Hello World!";
		private const string ExampleEnDown = "hello world!";
		private const string ExampleEnUp = "HELLO WORLD!";
		private const string ExampleNull = null;
		private const string ExampleEmpty = "";
		private const string ExampleWhitespace = " ";
		private const string ExampleWhitespaces = "     ";

		[TestMethod]
		public void LikeTest() {
			Assert.IsTrue(ExampleEnBase.Like(ExampleEnBase));
			Assert.IsTrue(ExampleEnBase.Like(ExampleEnDown));
			Assert.IsTrue(ExampleEnBase.Like(ExampleEnUp));
		}

		[TestMethod]
		public void IsBadTest() {
			Assert.IsTrue(ExampleNull.IsBad());
			Assert.IsTrue(ExampleEmpty.IsBad());
			Assert.IsTrue(ExampleWhitespace.IsBad());
			Assert.IsTrue(ExampleWhitespaces.IsBad());
			Assert.IsFalse(ExampleEnBase.IsBad());
		}
	}
}
