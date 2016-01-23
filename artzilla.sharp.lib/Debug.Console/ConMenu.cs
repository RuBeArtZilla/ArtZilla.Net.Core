using System;
using System.Collections.Generic;

namespace Debug.Con {
	public class ConMenu : ConMenuItem {
		private const int MaxItemCount = 5;
		private const string NextPage = "Next page";
		private const string PrevPage = "Previous page";

		private List<ConMenuItem> _childs = new List<ConMenuItem>();

		public IEnumerable<ConMenuItem> Childs {
			get { return _childs; }
			set { _childs = new List<ConMenuItem>(value); _childs.ForEach(c => c.Parent = this); }
		}

		public ConMenu() { }

		public ConMenu(string header, IEnumerable<ConMenuItem> menuItems = null) : base(header) {
			_childs = menuItems == null ? new List<ConMenuItem>() : new List<ConMenuItem>(menuItems);
		}

		protected override void OnExecute() {
			var isExit = false;
			var count = _childs.Count;
			var currentScreen = 0;
			var screenCount = (int) Math.Ceiling((double) count / MaxItemCount);
			var header = GetHeader();

			var i = 0;
			var items = new Dictionary<int, ConMenuItem>();
			foreach (var child in _childs) {
				items[i] = child;
				i++;
			}

			do {
				Console.Clear();
				Console.WriteLine(header);

				var pageItemCount = currentScreen == screenCount
															? count - currentScreen * MaxItemCount
															: MaxItemCount;

				for (var j = 0; j < pageItemCount; j++)
					Console.WriteLine($"{j - 1} {items[currentScreen * MaxItemCount + j].Header}");

				if (currentScreen != 0)
					Console.WriteLine("[-] Prev page");

				if (currentScreen != screenCount)
					Console.WriteLine("[+] Next page");
				
				Console.WriteLine(Parent == null ? "[0] Exit" : "[0] Back");
				
				var key = Console.ReadKey();
				
				switch (key.co) {
					case '0':
						isExit = true;
						break;

					case '+':
						currentScreen = Math.Min(++currentScreen, MaxItemCount);
						break;

					case '-':
						currentScreen = Math.Max(--currentScreen, 0);
						break;

					default:
						byte b;
						if (byte.TryParse(key.KeyChar.ToString(), out b) && b > 0 && b < pageItemCount)
							items[currentScreen * MaxItemCount + b].Execute();
						break;
				}
			} while (isExit);
		}

		private string GetHeader() {
			var h = Header;
			ConMenuItem cur = this;

			while (cur.Parent != null) {
				cur = cur.Parent;
				h = cur.Header + " > " + h;
			}

			return h;
		}
	}
}