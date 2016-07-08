using System.Collections.Generic;
using ArtZilla.Sharp.Lib.ConsoleExt;

namespace Debug.Con {
	class Program {
		static void Main(string[] args) {
			var conApp = new ConApp {
				MainMenu = new ConMenu("Main menu", new List<ConMenuItem>{
					new ConAction("test 1", () => { }),
					new ConAction("test 2", () => { }),
					new ConAction("test 3", () => { }),
					new ConAction("test 4", () => { }),
					new ConAction("test 5", () => { }),
					new ConAction("test 6", () => { }),
					new ConAction("test 7", () => { }),
					new ConAction("test 8", () => { }),
					new ConAction("test 9", () => { }),
					new ConAction("test 10", () => { }),
					new ConAction("test 11", () => { }),
					new ConAction("test 12", () => { }),
					new ConAction("test 13", () => { }),
					new ConAction("test 14", () => { }),
					new ConAction("test 15", () => { }),

					new ConMenu("Sub menu 1", new List<ConMenuItem>{
						new ConAction("sub menu test 1", () => { }),
						new ConAction("sub menu test 2", () => { }),
						new ConAction("sub menu test 3", () => { }),
						new ConAction("sub menu test 4", () => { }),
					}),

					new ConMenu("Sub menu 2", new List<ConMenuItem>{
						new ConAction("sub menu test 1", () => { }),
						new ConAction("sub menu test 2", () => { }),
						new ConAction("sub menu test 3", () => { }),
						new ConAction("sub menu test 4", () => { }),
						new ConAction("sub menu test 5", () => { }),
					}),
				})
			};

			conApp.Start(new List<string>(args));
		}
	}
}
