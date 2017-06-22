using System;
using System.Collections.Generic;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Test.Extensions {
	[TestClass]
	public class TypeExtTest {
		private struct TestStruct { }
		private enum TestEnum { }

		[TestMethod]
		public void TestIsNullable() {
			void assertNullable(Type type, bool isNullable) => Assert.IsTrue(type.IsNullable() == isNullable, $"Wrong result on Type {type}");

			assertNullable(typeof(int), false);
			assertNullable(typeof(int?), true);

			assertNullable(typeof(DateTime), false);
			assertNullable(typeof(DateTime?), true);

			assertNullable(typeof(String), true);
			assertNullable(typeof(Object), true);
			assertNullable(typeof(Object[]), true);
			assertNullable(typeof(IEnumerable<Object>), true);
			assertNullable(typeof(TestEnum), false);
			assertNullable(typeof(TestStruct), false);
		}
	}
}
