using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ArtZilla.Net.Core.Extensions;
using ArtZilla.Net.Core.Patterns;

namespace ArtZilla.Net.Core.Plugins {
	public interface IPlugin {
		bool IsLoaded { get; }
		void Load(IContainer container);
		void Unload(IContainer container);
	}

	public interface IContainer : IDisposable {
		void Init();
		void Load();
		void Unload();
	}

	public interface IContainer<TPlugin> : IContainer where TPlugin : IPlugin {
		ReadOnlyCollection<TPlugin> Plugins { get; }

		void Register(params TPlugin[] plugins);
		void Unregister(params TPlugin[] plugins);
		void Load(params TPlugin[] plugins);
		void Unload(params TPlugin[] plugins);
	}

	public abstract class Container<TPlugin> : Disposable, IContainer<TPlugin> where TPlugin : IPlugin {
		public bool IsInited { get; private set; }
		public bool IsLoaded { get; private set; }

		public ReadOnlyCollection<TPlugin> Plugins => _plugins.AsReadOnly();

		public void Init() {
			if (IsInited)
				throw new Exception("Container already initialized.");

			InitPlugins();
			IsInited = true;
		}

		public void Load() {
			foreach (var plugin in _plugins)
				plugin.Load(this);
		}

		public void Unload() {
			foreach (var plugin in _plugins)
				plugin.Unload(this);
		}

		public void Load(params TPlugin[] plugins) {
			foreach (var plugin in plugins)
				plugin.Load(this);
		}

		public void Unload(params TPlugin[] plugins) {
			foreach (var plugin in plugins)
				plugin.Unload(this);
		}

		public void Register(params TPlugin[] plugins) {
			_plugins.AddRange(plugins);
		}

		public void Unregister(params TPlugin[] plugins) {
			foreach (var plugin in plugins) {
				_plugins.Remove(plugin);

				if (plugin.IsLoaded)
					plugin.Unload(this);
			}
		}

		protected virtual void InitPlugins() {
			_plugins.AddRange(GetPlugins());
		}

		protected override void DisposeManaged() {
			Unload();

			base.DisposeManaged();
		}

		protected virtual IEnumerable<TPlugin> GetPlugins() {
			yield break;
		}

		private List<TPlugin> _plugins = new List<TPlugin>();
	}

	public abstract class Plugin : IPlugin {
		public bool IsLoaded { get; protected set; }

		public abstract void Load(IContainer container);

		public abstract void Unload(IContainer container);
	}

	public abstract class SingleContainerPlugin : Plugin {
		public IContainer Container => _container;

		public override void Load(IContainer container) {
			lock (_sync) {
				if (_container != null)
					throw new Exception("This plugin can exist in only one container at once.");

				BeforeLoad(container);
				_container = container ?? throw new ArgumentNullException(nameof(container));
				IsLoaded = true;
				AfterLoad(container);
			}
		}

		public override void Unload(IContainer container) {
			lock (_sync) {
				if (_container is null)
					return;

				BeforeUnload(container);
				_container = null;
				IsLoaded = false;
				AfterUnload(container);
			}
		}

		protected virtual void BeforeLoad(IContainer container) { }
		protected virtual void AfterLoad(IContainer container) { }
		protected virtual void BeforeUnload(IContainer container) { }
		protected virtual void AfterUnload(IContainer container) { }

		protected IContainer GetContainer => _container ?? throw new Exception("Not connected to any container.");

		private readonly object _sync = new object();
		private IContainer _container;
	}

	public abstract class MultipleContainerPlugin : Plugin {
		public IEnumerable<IContainer> GetContainers => _containers;

		public override void Load(IContainer container) {
			lock (_sync) {
				if (IsLoaded)
					return;
				BeforeLoad(container);
				_containers.Add(container);
				IsLoaded = true;
				AfterLoad(container);
			}
		}

		public override void Unload(IContainer container) {
			lock (_sync) {
				if (!IsLoaded)
					return;
				BeforeUnload(container);
				_containers.Remove(container);
				IsLoaded = false;
				AfterUnload(container);
			}
		}

		protected virtual void BeforeLoad(IContainer container) { }
		protected virtual void AfterLoad(IContainer container) { }
		protected virtual void BeforeUnload(IContainer container) { }
		protected virtual void AfterUnload(IContainer container) { }

		private readonly object _sync = new object();
		private HashSet<IContainer> _containers = new HashSet<IContainer>();
	}
}
