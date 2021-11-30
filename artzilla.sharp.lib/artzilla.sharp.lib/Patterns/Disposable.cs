using System;

namespace ArtZilla.Net.Core.Patterns;

/// <summary>
/// Represent base IDisposable implementation
/// </summary>
[Obsolete("Anti-pattern. Keep unmanaged and managed resources separated")]
public abstract class Disposable : IDisposable {
	/// <summary>
	/// Gets a value indicating whether this <see cref="Disposable"/> is disposed.
	/// </summary>
	/// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
	public bool Disposed => _disposed;

	/// <summary>
	/// Invoking Dispose method if needed
	/// </summary>
	~Disposable() {
		if (_disposed)
			return;

		_disposed = true;
		Dispose(false);
	}

	/// <summary>
	/// Implementation of <see cref="IDisposable"/>
	/// </summary>
	public void Dispose() {
		if (_disposed)
			return;

		_disposed = true;
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Base Dispose method
	/// </summary>
	/// <param name="disposing"></param>
	protected virtual void Dispose(bool disposing) {
		if (disposing)
			DisposeManaged();

		DisposeUnmanaged();
	}

	/// <summary>
	/// Disposing managed objects
	/// </summary>
	protected virtual void DisposeManaged() {
		// nothing to do
	}

	/// <summary>
	/// Disposing unmanaged objects and resources
	/// </summary>
	protected virtual void DisposeUnmanaged() {
		// nothing to do
	}

	private bool _disposed = false;
}
