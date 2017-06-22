using System;
using ArtZilla.Net.Core.Patterns;

namespace ArtZilla.Net.Core.Plugins {
	public abstract class APlugin : Disposable, IPlugin {
		public virtual string Name { get; protected set; } = "Unknown";

		public virtual string Description { get; protected set; } = "None";

		public virtual string Author { get; protected set; } = "Unknown";

		public virtual VersionInfo Version { get; protected set; } = new VersionInfo();

		public virtual PluginDependency[] GetDependencies() => new PluginDependency[0];

		public PluginConfig Config {
			get { return _config; }
			set { SetConfig(value); }
		}

		public IPluginService Host => _host;

		public bool Init(IPluginService host) {
			if (host == null)
				return false;

			var deps = GetDependencies();
			if (!host.HasPlugins(deps))
				return false;
			
			_host = host;
			IsInitialized = true;
			return true;
		}

		public bool IsInitialized { get; private set; } = false;

		public virtual PluginConfig GenerateDefaultConfig() => new PluginConfig();

		public virtual Type GetConfigType() => typeof (PluginConfig);
		
		protected virtual void SetConfig(PluginConfig value) {
			if (value == null)
				throw new NullReferenceException();

			if (ReferenceEquals(value, _config))
				return;

			_config = value;
		}
		
		private IPluginService _host;
		private PluginConfig _config;
	}
}