using System;

namespace ArtZilla.Sharp.Lib.Plugins {
	public class PluginDependency {
		public readonly string Name;
		public readonly VersionInfo MinVersion = null;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
		/// </summary>
		public PluginDependency(string name, VersionInfo minVersion) {
			Name = name;
			MinVersion = minVersion;
		}

		public bool IsSuccess(IPlugin plugin) {
			return plugin != null
			       && plugin.Name == Name
			       && plugin.Version >= MinVersion;
		}
	}

	public interface IPlugin : IDisposable {
		string Name { get; }

		string Description { get; }

		string Author { get; }

		VersionInfo Version { get; }

		PluginDependency[] GetDependencies();

		bool Init(IPluginService host);

		bool IsInitialized { get; }
	}
}