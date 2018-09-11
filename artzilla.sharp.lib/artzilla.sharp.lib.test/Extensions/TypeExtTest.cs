using System;
using System.Collections.Generic;
using System.Linq;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions {
	[TestClass]
	public class TypeExtTest {
		interface ITest1 { }
		interface ITest2 { }
		class TestClass1 : ITest1, ITest2 {
			public string Property0 { get; set; } // +
			public string Property1 { get; } // +
			public string Property2 { private get; set; } // +
		}

		private struct TestStruct { }
		private enum TestEnum { }

		[TestMethod]
		public void IsNullableTest() {
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

		[TestMethod]
		public void GetReadableNonIndexerPropertiesTest() {
			var props = typeof(TestClass1).GetReadableNonIndexerProperties().ToArray();
			Assert.AreEqual(3, props.Length);
			Assert.IsTrue(props.Select(p => p.Name).SequenceEqual(new[] {
				nameof(TestClass1.Property0),
				nameof(TestClass1.Property1),
				nameof(TestClass1.Property2),
			}));
		}

		[TestMethod]
		public void EnumerableTests() {
			var arr1 = Enumerable.Range(0, 5).ToArray();
			var arr2 = Enumerable.Empty<double?>();

			var type1 = arr1.GetType();
			var type2 = arr2.GetType();
			var type3 = typeof(string);
			var type4 = typeof(int);

			Assert.AreEqual(typeof(IEnumerable<int>), type1.GetIEnumerableInterface());
			Assert.AreEqual(typeof(IEnumerable<double?>), type2.GetIEnumerableInterface());

			Assert.IsTrue(type1.IsImplementIEnumerable());
			Assert.IsTrue(type2.IsImplementIEnumerable());
			Assert.IsTrue(type3.IsImplementIEnumerable());
			Assert.IsFalse(type4.IsImplementIEnumerable());
		}
	}
}
