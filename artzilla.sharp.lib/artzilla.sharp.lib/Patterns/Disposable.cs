using System;

namespace ArtZilla.Sharp.Lib.Patterns {
	public abstract class Disposable : IDisposable {
		private bool _disposed = false;

		~Disposable() {
			if (_disposed)
				return;

			_disposed = true;
			Dispose(false);
		}

		public void Dispose() {
			if (_disposed)
				return;

			_disposed = true;
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing)
				DisposeManaged();

			DisposeUnmanaged();
		}

		protected virtual void DisposeManaged() {
			// managed objects
		}

		protected virtual void DisposeUnmanaged() {
			// unmanaged objects and resources
		}
	}
}