using ArtZilla.Sharp.Lib.Patterns;

namespace ArtZilla.Sharp.Lib.Plugins {
	public abstract class APlugin : Disposable, IPlugin {
		public virtual string Name { get; protected set; } = "Unknown";
		public virtual string Description { get; protected set; } = "None";
		public virtual string Author { get; protected set; } = "Unknown";
		public virtual VersionInfo Version { get; protected set; } = new VersionInfo();

		public virtual PluginDependency[] GetDependencies() => new PluginDependency[0];

		protected IPluginService Host;

		public bool Init(IPluginService host) {
			if (host == null)
				return false;

			var deps = GetDependencies();
			if (!host.HasPlugins(deps))
				return false;
			
			Host = host;
			IsInitialized = true;
			return true;
		}

		public bool IsInitialized { get; private set; } = false;
	}
}