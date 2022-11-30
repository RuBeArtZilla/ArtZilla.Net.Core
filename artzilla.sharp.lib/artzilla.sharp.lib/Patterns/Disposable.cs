using System;

namespace ArtZilla.Net.Core.Patterns;

/// Represent base IDisposable implementation
[Obsolete("Anti-pattern. Keep unmanaged and managed resources separated")]
public abstract class Disposable : IDisposable {
	/// Gets a value indicating whether this <see cref="Disposable"/> is disposed.
	/// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
	public bool Disposed => _disposed;

	/// Invoking Dispose method if needed
	~Disposable() {
		if (_disposed)
			return;

		_disposed = true;
		Dispose(false);
	}

	/// Implementation of <see cref="IDisposable"/>
	public void Dispose() {
		if (_disposed)
			return;

		_disposed = true;
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// Base Dispose method
	/// <param name="disposing"></param>
	protected virtual void Dispose(bool disposing) {
		if (disposing)
			DisposeManaged();

		DisposeUnmanaged();
	}

	/// Disposing managed objects
	protected virtual void DisposeManaged() {
		// nothing to do
	}

	/// Disposing unmanaged objects and resources
	protected virtual void DisposeUnmanaged() {
		// nothing to do
	}

	bool _disposed = false;
}
