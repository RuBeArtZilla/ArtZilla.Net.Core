using System;
using ArtZilla.Net.Core.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Plugins {
	public class TestPlugin : SingleContainerPlugin {
		public string Id;
		public TestPlugin(string id) => Id = id;

		protected override void AfterLoad(IContainer container) {
			base.AfterLoad(container);

			Console.WriteLine(Id + " loaded.");
		}

		protected override void BeforeUnload(IContainer container) {
			base.BeforeUnload(container);

			Console.WriteLine(Id + " unloaded.");
		}
	}

	public class TestContainer : Container<TestPlugin> {

	}

	[TestClass]
	public class PluginsTest {
		[TestMethod]
		public void SimpleTest() {
			var p1 = new TestPlugin("p1");
			var p2 = new TestPlugin("p2");
			var p3 = new TestPlugin("p3");

			using (var c = new TestContainer()) {
				c.Register(p1, p2, p3);
				Assert.AreEqual(3, c.Plugins.Count, "Not all plugins was registered");

				c.Init();

				Assert.ThrowsException<Exception>(() => c.Init());

				c.Load();
				Assert.IsTrue(p1.IsLoaded, "p1 not loaded");

				c.Unload(p1);
				Assert.IsFalse(p1.IsLoaded, "p1 not unloaded");

				c.Load(p1);
				Assert.IsTrue(p1.IsLoaded, "p1 not loaded");

				c.Unregister(p2);
				Assert.IsFalse(p2.IsLoaded, "p2 not unloaded");
				Assert.AreEqual(2, c.Plugins.Count, "p2 not removed");

				Assert.IsTrue(p3.IsLoaded);
			}

			Assert.IsFalse(p1.IsLoaded);
			Assert.IsFalse(p2.IsLoaded);
			Assert.IsFalse(p3.IsLoaded);
		}
	}
}
