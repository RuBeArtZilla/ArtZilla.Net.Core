using System;
using ArtZilla.Sharp.Lib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Sharp.Lib.Test.Extensions {
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
	}
}
