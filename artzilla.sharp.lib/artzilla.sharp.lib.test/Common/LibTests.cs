using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Sharp.Lib.Test {
	[TestClass]
	public class LibTests {
		[TestMethod]
		public void DoAct() {
			var r = Lib.Do(() => { throw new Exception("None"); });
			Assert.IsFalse(r.IsOk);
			Assert.IsTrue(r.Exception.Message == "None");
			r = Lib.Do(null);

			Assert.IsFalse(r.IsOk);
			Assert.IsTrue(Lib.Do(() => { Thread.Sleep(TimeSpan.FromMilliseconds(3D)); }).IsOk);
		}

		[TestMethod]
		public void DoFunc() {
			var r = Lib.Do<Boolean>(() => { throw new Exception("Hello"); });
			Assert.IsFalse(r.IsOk);
			Assert.IsTrue(r.Exception.Message == "Hello");
			Assert.AreEqual(r.Result, default(Boolean));

			var r2 = Lib.Do(() => true);
			Assert.IsTrue(r2.IsOk);
			Assert.AreEqual(r2.Result, true);
		}
	}
}
