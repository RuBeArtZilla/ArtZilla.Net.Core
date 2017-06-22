using System.Linq;
using System.Windows;
using ArtZilla.Net.Core.Plugins;

namespace PlugTest {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		public const string PluginDir = "plugins";

		/// <summary>
		/// Создает событие <see cref="E:System.Windows.Application.Startup"/>.
		/// </summary>
		/// <param name="e">Объект <see cref="T:System.Windows.StartupEventArgs"/>, содержащий данные, которые относятся к событию.</param>
		protected override void OnStartup(StartupEventArgs e) {
			using (var ps = new PluginService()) {
				ps.FindPlugins(PluginDir);
				var c = ps.Plugins.Count();
				ps.InitPlugins();
				c = ps.Plugins.Count();
			}

			base.OnStartup(e);
		}
	}
}
