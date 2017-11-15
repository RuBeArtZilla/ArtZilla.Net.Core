using System;
using System.Linq;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Test.Extensions {
	[TestClass]
	public class ObjExtTest {
		[TestMethod]
		public void TestIsNull() {
			Object o = null;
			Assert.IsTrue(o.IsNull());
			o = new { In = 128, Out = 256 };
			Assert.IsFalse(o.IsNull());
		}

		[TestMethod]
		public void TestIsAnyOf() {
			Assert.IsTrue(42.IsAnyOf(4, 8, 15, 16, 23, 42));
			Assert.IsFalse("Luke".IsAnyOf("Jedi", "Sith"));
		}

		[TestMethod]
		public void TestToEnumerable() {
			Assert.IsTrue(42.ToEnumerable().Any());
			Assert.IsTrue(42.ToEnumerable().First() == 42);
			Assert.IsTrue(42.ToEnumerable().ToArray().Length == 1);
		}
	}
}
