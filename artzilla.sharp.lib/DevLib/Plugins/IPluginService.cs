using System;
using System.Collections.Generic;

namespace ArtZilla.Net.Core.Plugins {
	public interface IPluginService : IDisposable {
		IEnumerable<IPlugin> Plugins { get; }

		void LoadPlugins();

		void FindPlugins(string dir);

		bool HasPlugins(PluginDependency[] deps);

		int InitPlugins();
	}
}