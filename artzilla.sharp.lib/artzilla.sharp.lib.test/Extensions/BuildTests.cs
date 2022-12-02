using System.Collections.Generic;
using System.Linq;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions;

[TestClass]
public class BuildTests {
	[TestMethod]
	public void BuildListTest() {
		var expected = new List<int> { 4, 8, 15, 16, 23, 42 };
		var list = Build.List(4).With(8).With(15, 16).With(new List<int> { 23, 42 }).Create();
		Assert.IsInstanceOfType(list, typeof(List<int>));
		Assert.IsTrue(expected.SequenceEqual(list));

		list = Build.List(4, 8, 15).With(16).With(23, 42).Create();
		Assert.IsInstanceOfType(list, typeof(List<int>));
		Assert.IsTrue(expected.SequenceEqual(list));

		list = 4.BuildList(8, 15, 16, 23).With(42).Create();
		Assert.IsInstanceOfType(list, typeof(List<int>));
		Assert.IsTrue(expected.SequenceEqual(list));

		list = expected.With().With(0).Create();
		Assert.IsInstanceOfType(list, typeof(List<int>));
		Assert.IsFalse(expected.SequenceEqual(list));
		Assert.AreEqual(expected.Count + 1, list.Count);
		Assert.AreEqual(0, list[list.Count - 1]);
	}

	[TestMethod]
	public void BuildArrayTest() {
		var expected = new[] { 4, 8, 15, 16, 23, 42 };
		var array = Build.Array(4).With(8).With(15, 16).With(new List<int> { 23, 42 }).Create();
		Assert.IsInstanceOfType(array, typeof(int[]));
		Assert.IsTrue(expected.SequenceEqual(array));

		array = Build.Array(4, 8, 15).With(16).With(23, 42).Create();
		Assert.IsInstanceOfType(array, typeof(int[]));
		Assert.IsTrue(expected.SequenceEqual(array));

		array = 4.BuildArray(8, 15, 16, 23).With(42).Create();
		Assert.IsInstanceOfType(array, typeof(int[]));
		Assert.IsTrue(expected.SequenceEqual(array));

		array = expected.With().With(0).Create();
		Assert.IsInstanceOfType(array, typeof(int[]));
		Assert.IsFalse(expected.SequenceEqual(array));
		Assert.AreEqual(expected.Length + 1, array.Length);
		Assert.AreEqual(0, array[array.Length - 1]);
	}
}