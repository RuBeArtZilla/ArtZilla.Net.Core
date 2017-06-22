namespace ArtZilla.Net.Core.Plugins {
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
}