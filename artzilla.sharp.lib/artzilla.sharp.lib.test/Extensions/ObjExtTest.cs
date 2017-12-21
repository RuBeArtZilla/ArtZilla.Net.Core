using System;
using System.Collections.Generic;
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
			Assert.IsTrue("Luke".IsAnyOf("Jedi", "Sith", "Luke"));
			Assert.IsFalse("Luke".IsAnyOf("Jedi", "Sith"));

			var comparer = new FirstCharOfStringCustomEqualityComparer();
			Assert.IsTrue("Luke".IsAnyOf(comparer,"Jedi", "Sith", "Luke"));
			Assert.IsTrue("Luke".IsAnyOf(comparer,"Jedi", "Sith", "Leia"));
			Assert.IsFalse("Luke".IsAnyOf(comparer, "Jedi", "Sith"));
			Assert.IsFalse("Luke".IsAnyOf(comparer, "Jedi", "Sith", "leia"));
		}

		[TestMethod]
		public void TestToEnumerable() {
			Assert.IsTrue(42.ToEnumerable().Any());
			Assert.IsTrue(42.ToEnumerable().First() == 42);
			Assert.IsTrue(42.ToEnumerable().ToArray().Length == 1);
		}

		class FirstCharOfStringCustomEqualityComparer: IEqualityComparer<string> {
			public bool Equals(string x, string y) => x[0] == y[0]; // skip null check!
			public int GetHashCode(string obj) => obj[0].GetHashCode();
		}
	}
}
