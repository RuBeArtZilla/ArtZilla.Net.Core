using System;
using System.Collections.Generic;

namespace ArtZilla.Sharp.Lib.Plugins {
	public interface IPluginService : IDisposable {
		IEnumerable<IPlugin> Plugins { get; }

		void LoadPlugins();

		void FindPlugins(string dir);

		bool HasPlugins(PluginDependency[] deps);

		int InitPlugins();
	}
}