using System.Collections.Generic;
using System.Linq;

namespace ArtZilla.Sharp.Lib.ConsoleExt {
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