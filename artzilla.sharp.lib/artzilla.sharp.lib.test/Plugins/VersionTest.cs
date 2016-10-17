using ArtZilla.Sharp.Lib.Experimental;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Sharp.Lib.Test.Plugins {
	[TestClass]
	public class VersionTest {
		[TestMethod]
		public void SimpleTest() {
			var v0 = new VersionInfo(1, 2, 3, 4); 
			var h0 = new VersionInfo(4, 3, 2, 1); 

			Assert.AreNotEqual(v0, h0);
			Assert.AreNotEqual(v0.Value(), h0.Value());

			var v1 = new VersionInfo("1.2.3.4");
			var v2 = new VersionInfo(v0.ToString());

			AssertEx.IsFail(()=> { new VersionInfo(null); });
			AssertEx.IsFail(()=> { new VersionInfo(".1.1.1"); });
			AssertEx.IsFail(()=> { new VersionInfo("1.1.1."); });
			AssertEx.IsFail(()=> { new VersionInfo("1.1.1.1a"); });
			AssertEx.IsFail(()=> { new VersionInfo("1.1.1.100000000"); });
		}
	}
}
