using System.Collections.Generic;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions;

[TestClass]
public class IDicExtTests {
	[TestMethod]
	public void TestGetValueOrDefault() {
		var dict = new Dictionary<int, int> { [4] = 42, [8] = 99 };

		Assert.AreEqual(42, dict.GetValueOrDefault(4));
		Assert.AreEqual(0, dict.GetValueOrDefault(16));
		Assert.AreEqual(1, dict.GetValueOrDefault(16, 1));

		// check dictionary state
		Assert.AreEqual(2, dict.Count);
		Assert.AreEqual(42, dict[4]);
		Assert.AreEqual(99, dict[8]);
	}

	[TestMethod]
	public void TestGetValueOrCreate() {
		var dict = new Dictionary<int, int> { [4] = 42, [8] = 99 };

		Assert.AreEqual(42, dict.GetValueOrCreate(4));
		Assert.AreEqual(0, dict.GetValueOrCreate(16));

		Assert.AreEqual(42, dict.GetValueOrCreate(4, () => 1));
		Assert.AreEqual(1, dict.GetValueOrCreate(16, () => 1));

		Assert.AreEqual(42, dict.GetValueOrCreate(4, i => i));
		Assert.AreEqual(1, dict.GetValueOrCreate(1, i => i));

		// check dictionary state
		Assert.AreEqual(2, dict.Count);
		Assert.AreEqual(42, dict[4]);
		Assert.AreEqual(99, dict[8]);
	}

	[TestMethod]
	public void TestGetValueOrAdd() {
		var dict = new Dictionary<int, int> { [4] = 42, [8] = 99 };

		// Exist value test
		Assert.AreEqual(42, dict.GetValueOrAdd(4));
		Assert.AreEqual(42, dict.GetValueOrAdd(4, () => 1));
		Assert.AreEqual(42, dict.GetValueOrAdd(4, i => i));
		Assert.AreEqual(2, dict.Count);

		// New value test
		Assert.AreEqual(0, dict.GetValueOrAdd(0));
		Assert.AreEqual(1, dict.GetValueOrAdd(1, () => 1));
		Assert.AreEqual(2, dict.GetValueOrAdd(2, i => i));

		// check dictionary state
		Assert.AreEqual(5, dict.Count);
		Assert.AreEqual(42, dict[4]);
		Assert.AreEqual(99, dict[8]);
		Assert.AreEqual(0, dict[0]);
		Assert.AreEqual(1, dict[1]);
		Assert.AreEqual(2, dict[2]);
	}

	[TestMethod]
	public void TestGetValueOrAddNew() {
		var dict = new Dictionary<int, int> { [4] = 42, [8] = 99 };

		Assert.AreEqual(42, dict.GetValueOrAddNew(4));
		Assert.AreEqual(0, dict.GetValueOrAddNew(16));

		// check dictionary state
		Assert.AreEqual(3, dict.Count);
		Assert.AreEqual(42, dict[4]);
		Assert.AreEqual(99, dict[8]);
		Assert.AreEqual(0, dict[16]);
	}
}
