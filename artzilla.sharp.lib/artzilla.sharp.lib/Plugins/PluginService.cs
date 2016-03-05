using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ArtZilla.Sharp.Lib.Extenstions;
using ArtZilla.Sharp.Lib.Patterns;

namespace ArtZilla.Sharp.Lib.Plugins {
	public class PluginService : Disposable, IPluginService {
		public IEnumerable<IPlugin> Plugins => _plugs.Values;
		
		private readonly Dictionary<string, IPlugin> _plugs = new Dictionary<string, IPlugin>();
		private readonly List<IPlugin> _notInitedPlugins = new List<IPlugin>();

		public void FindPlugins(string dir) {
			if (!Directory.Exists(dir))
				return;

			foreach (var fileOn in Directory.GetFiles(dir)) {
				var file = new FileInfo(fileOn);

				if (file.Extension.Equals(".dll")) {
					TryLoadPlugin(fileOn);
				}
			}
		}

		public bool HasPlugins(PluginDependency[] deps) 
			=> deps == null || deps.All(dep => _plugs.ContainsKey(dep.Name) && dep.IsSuccess(_plugs[dep.Name]));

		private void TryLoadPlugin(string filename) {
			try {
				var pluginAssembly = Assembly.LoadFrom(filename);
				var pluginTypes = pluginAssembly.GetTypes();
				var plugins = pluginTypes.Where(t => t.GetInterface(nameof(IPlugin)) != null).ToList();
				plugins.ForEach(TryLoadPlugin);
			} catch (Exception e) {
				// throw; // todo: ?
			}
		}

		private void TryLoadPlugin(Type plType) {
			try {
				var plCtor = plType.GetConstructor(Type.EmptyTypes);
				if (plCtor == null)
					return;
				var plug = (IPlugin) plCtor.Invoke(null);
				_notInitedPlugins.Add(plug);
			} catch (Exception e) {
				// throw; // todo: ?
			}
		}

		public void InitPlugins() {
			var count = 0;

			do {
				var initedPlugins = _notInitedPlugins.Where(p => p.Init(this)).ToList();
        foreach (var plugin in initedPlugins) {
					if (plugin.Name.IsBad())
						continue;

					if (_plugs.ContainsKey(plugin.Name))
						continue;

					_plugs[plugin.Name] = plugin;
					_notInitedPlugins.Remove(plugin);
					count++;
				}

				count = initedPlugins.Count;
			} while (count > 0);

		}
	}
}