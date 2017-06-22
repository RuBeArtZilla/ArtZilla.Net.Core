using System;
using System.Management.Instrumentation;

namespace ArtZilla.Net.Core.Plugins {
	public interface IPlugin : IDisposable {
		string Name { get; }

		string Description { get; }

		string Author { get; }

		VersionInfo Version { get; }

		PluginDependency[] GetDependencies();

		bool Init(IPluginService host);

		bool IsInitialized { get; }

		IPluginService Host { get; }
    
		PluginConfig Config { get; set; }

		PluginConfig GenerateDefaultConfig();

		Type GetConfigType();
	}
}