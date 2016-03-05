using System;
using ArtZilla.Sharp.Lib.Plugins;

namespace Plugin {
	public class TestType { }

	public class TestType2 : IDisposable {
		#region Implementation of IDisposable

		/// <summary>
		/// Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
		/// </summary>
		public void Dispose() {
			throw new NotImplementedException();
		}

		#endregion
	}

	public class TestPlugin : APlugin {
		public TestPlugin() {
			Name = "TestPlugin";
			Description = "Plugin just fro testing";
			Author = "ArtZilla";
			Version = new VersionInfo(1, 1);
		}
	}
}
