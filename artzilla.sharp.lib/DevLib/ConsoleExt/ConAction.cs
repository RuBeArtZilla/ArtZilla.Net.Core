using System;

namespace ArtZilla.Net.Core.ConsoleExt {
	public class ConAction : ConMenuItem {
		public Action Action { get; set; }

		protected override void OnExecute() => Action?.Invoke();

		public ConAction() {}

		public ConAction(string header, Action act) : base(header) {
			Action = act;
		}
	}
}