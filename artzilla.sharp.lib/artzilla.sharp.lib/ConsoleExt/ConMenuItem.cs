using System;

namespace ArtZilla.Sharp.Lib.ConsoleExt {
	public abstract class ConMenuItem {
		public ConMenu Parent { get; set; }

		public string Header { get; set; }

		protected ConMenuItem() { }

		protected ConMenuItem(string header) {
			Header = header;
		}

		protected abstract void OnExecute();

		public void Execute() {
			OnBeforeExecute();
			OnExecute();
			OnAfterExecute();
		}

		public event EventHandler<EventArgs> BeforeExecute;
		public event EventHandler<EventArgs> AfterExecute;

		protected virtual void OnBeforeExecute() => BeforeExecute?.Invoke(this, EventArgs.Empty);

		protected virtual void OnAfterExecute() => AfterExecute?.Invoke(this, EventArgs.Empty);
	}
}