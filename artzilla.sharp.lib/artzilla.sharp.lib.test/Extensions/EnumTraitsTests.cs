using System;
using System.Collections.Generic;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions;

[TestClass]
public class EnumTraitsTests {
	enum EnumLong : long {
		X = -1,
		Y = 1,
		Z = 2,
	}

	[TestMethod]
	public void TestMethod1() {
		Assert.AreEqual(-1, EnumTraits<EnumLong>.MinValue); // -1
		Assert.AreEqual(2, EnumTraits<EnumLong>.MaxValue); // 2
		Assert.IsFalse(EnumTraits<EnumLong>.IsValid(0)); // False
	}
}
