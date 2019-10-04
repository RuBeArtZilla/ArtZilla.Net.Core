using System.Diagnostics.CodeAnalysis;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions {
  [TestClass]
  [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
  public class ArrayExtTests {
	  public static readonly int[] NullArray = default;
	  public static readonly int[] EmptyArray = new int[0];
	  public static readonly int[] NotEmptyArray = { 4, 8, 15, 16, 23, 42 };

		[TestMethod]
		public void IsNullOrEmptyTest() {
			Assert.IsTrue(NullArray.IsNullOrEmpty());
			Assert.IsTrue(EmptyArray.IsNullOrEmpty());
			Assert.IsFalse(NotEmptyArray.IsNullOrEmpty());
		}

		[TestMethod]
		public void IsNotNullOrEmptyTest() {
			Assert.IsFalse(NullArray.IsNotNullOrEmpty());
			Assert.IsFalse(EmptyArray.IsNotNullOrEmpty());
			Assert.IsTrue(NotEmptyArray.IsNotNullOrEmpty());
		}
	}
}