using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ArtZilla.Sharp.Lib.Extenstions;
using ArtZilla.Sharp.Lib.Patterns;
using ArtZilla.Sharp.Lib.Serialization;

namespace ArtZilla.Sharp.Lib.Plugins {
	public class PluginService : Disposable, IPluginService {
		public const string DefaultConfigPath = "configuration";
		public const string DefaultPluginPath = "plugins";

		public IEnumerable<IPlugin> Plugins => _plugs.Values;

		public PluginService() {
			_executionDir = Assembly.GetExecutingAssembly().Location;
			_pluginsDir = Path.Combine(_executionDir, DefaultPluginPath);
			_configsDir = Path.Combine(_executionDir, DefaultConfigPath);
		}

		public bool IncludePlugin(IPlugin plugin) {
			if (plugin == null)
				throw new NullReferenceException();

			if (plugin.IsInitialized)
				throw new Exception("Can't add already initialized plugin");

			lock (_notInitedPlugins)
				_notInitedPlugins.Add(plugin);

			return true;
		}

		public bool IncludePlugin<T>() where T : IPlugin
			=> TryLoadPlugin(typeof(T));

		public void LoadPlugins() => FindPlugins(_pluginsDir);

		public void FindPlugins(string dir) {
			if (!Directory.Exists(dir))
				return;

			foreach (var fileOn in Directory.GetFiles(dir)) {
				var file = new FileInfo(fileOn);

				if (file.Extension.Equals(".dll"))
					TryLoadPlugin(fileOn);
			}
		}

		public bool HasPlugins(PluginDependency[] deps)
			=> deps == null || deps.All(dep => _plugs.ContainsKey(dep.Name) && dep.IsSuccess(_plugs[dep.Name]));

		private int TryLoadPlugin(string filename) {
			var count = 0;

			try {
				var pluginAssembly = Assembly.LoadFrom(filename);
				var pluginTypes = pluginAssembly.GetTypes();
				var plugins = pluginTypes.Where(t => t.GetInterface(nameof(IPlugin)) != null).ToList();

				plugins.ForEach(p => count += TryLoadPlugin(p) ? 1 : 0);
			} catch (Exception e) {
				// throw; // todo: ?
			}

			return count;
		}

		private bool TryLoadPlugin(Type plType) {
			try {
				var plCtor = plType.GetConstructor(Type.EmptyTypes);
				if (plCtor == null)
					return false;

				var plug = (IPlugin) plCtor.Invoke(null);

				lock (_notInitedPlugins)
					_notInitedPlugins.Add(plug);

				return true;
			} catch (Exception e) {
				// throw; // todo: ?
			}

			return false;
		}

		public int InitPlugins() {
			var count = 0;
			var initedCount = 0;

			lock (_notInitedPlugins) {
				do {
					// var initedPlugins = _notInitedPlugins.Where(p => p.Init(this)).ToList();
					var initedPlugins =
						_notInitedPlugins
							.Where(p => !p.Name.IsBad()											// Plugin name used later
							            && HasPlugins(p.GetDependencies())	// Checking plugin dependencies
							            && TryLoadConfig(p)									// Plugin must have config
							            && p.Init(this))										// Plugin successfully inited
							.ToList();
					
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
					initedCount += count;
				} while (count > 0);
			}

			return initedCount;
		}

		private bool TryLoadConfig(IPlugin plugin) {
			var configFileName = plugin.Name + ".cfg";
			var file = Path.Combine(_configsDir, configFileName);
			var o = SerXml.Load(plugin.GetConfigType(), file) as PluginConfig;

			if (o == null) {
				o = plugin.GenerateDefaultConfig();
				SerXml.Save(file, o);
			}

			plugin.Config = o;
			return true;
		}

		public bool SavePluginConfig(IPlugin plugin) {

			return false;
		}

		private string _executionDir;
		private string _pluginsDir;
		private string _configsDir;
		private readonly Dictionary<string, IPlugin> _plugs = new Dictionary<string, IPlugin>();
		private readonly List<IPlugin> _notInitedPlugins = new List<IPlugin>();
	}
}