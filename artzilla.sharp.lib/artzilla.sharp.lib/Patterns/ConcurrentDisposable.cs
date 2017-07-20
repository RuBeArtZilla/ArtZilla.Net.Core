using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtZilla.Net.Core.Patterns {
	// todo: ...
	abstract class ConcurrentDisposable: IDisposable {
		~ConcurrentDisposable() => Dispose(false);

		public void Dispose() {
			if (_disposed)
				return;

			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing)
				DisposeManaged();

			DisposeUnmanaged();
		}

		protected virtual void DisposeManaged() {
			// nothing to do
		}

		protected virtual void DisposeUnmanaged() {
			// nothing to do
		}

		private bool _disposed = false;
		private readonly object _displock = new object();
	}
}
