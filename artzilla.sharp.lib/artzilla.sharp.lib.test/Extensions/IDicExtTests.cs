using System.Collections.Generic;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions {
	[TestClass]
	public class IDicExtTests {
		[TestMethod]
		public void TestIsAnyOf() {
			var dict = new Dictionary<int, int>() {
				[4] = 42,
				[8] = 42,
			};

			Assert.AreEqual(42, dict.GetValueOrDefault(4, 0));
			Assert.AreEqual(42, dict.GetValueOrDefault(8, 0));
			Assert.AreEqual(0, dict.GetValueOrDefault(16, 0));
		}
	}
}
