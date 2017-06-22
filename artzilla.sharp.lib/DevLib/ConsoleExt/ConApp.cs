using System.Collections.Generic;
using System.Linq;

namespace ArtZilla.Net.Core.ConsoleExt {
	public class ConApp {
		public ConMenu MainMenu = new ConMenu();

		public void Start(List<string> args) {
			if (args.Any()) {}
			else Start();
		}

		private void Start() {
			MainMenu.Execute();
		}
	}
}