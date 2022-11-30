using System;
using ArtZilla.Net.Core.Patterns;

namespace ArtZilla.Net.Core.Interfaces;

///
public enum State {
	/// Stopped state
	Stopped,
	
	/// StartPending state
	StartPending,
	
	/// StopPending state
	StopPending,
	
	/// Running state
	Running,
	
	/// ContinuePending state
	ContinuePending,
	
	/// PausePending state 
	PausePending,
	
	/// Paused state
	Paused
}

/// interface for runnable object
public interface IRunnable {
	/// 
	bool IsStarted { get; }

	/// 
	void Start();

	/// 
	void Stop();
}

/// 
public interface IPausable : IRunnable {
	/// 
	bool IsPaused { get; }

	/// 
	void Pause();

	/// 
	void Continue();
}

/// Not ready for work. Untested.
[Obsolete("not ready for use")]
public class Runnable : Disposable, IRunnable {
	/// <inheritdoc />
	public bool IsStarted {
		get { lock (_sync) return _isStarted; }
		set {
			if (value)
				Start();
			else
				Stop();
		}
	}

	/// <inheritdoc />
	public void Start() {
		lock (_sync) {
			if (_isStarted)
				return;

			LockedStart();
			_isStarted = true;
		}
	}

	///
	protected virtual void LockedStart() { }

	/// <inheritdoc />
	public void Stop() {
		lock (_sync) {
			if (!_isStarted)
				return;

			LockedStop();
			_isStarted = false;
		}
	}
	
	///
	protected virtual void LockedStop() { }

	///
	protected override void DisposeManaged() => Stop();

	bool _isStarted;
	readonly object _sync = new();
}

/// Not ready for work. Untested.
[Obsolete("not ready for use")]
public class RunnableAndPausable : Disposable, IRunnable, IPausable {
	/// <inheritdoc />
	public bool IsStarted {
		get { lock (_sync) return _isStarted; }
		set {
			if (value)
				Start();
			else
				Stop();
		}
	}
	
	/// <inheritdoc />
	public bool IsPaused {
		get { lock (_sync) return _isPaused; }
		set {
			if (value)
				Pause();
			else
				Continue();
		}
	}
	
	/// <inheritdoc />
	public void Start() {
		lock (_sync) {
			if (_isStarted)
				return;

			if (_isPaused)
				LockedContinue();

			LockedStart();
			_isStarted = true;
		}
	}

	/// 
	protected virtual void LockedStart() { }
	
	/// <inheritdoc />
	public void Stop() {
		lock (_sync) {
			if (!_isStarted)
				return;

			if (_isPaused)
				LockedContinue();

			LockedStop();
			_isStarted = false;
		}
	}
	
	/// 
	protected virtual void LockedStop() { }
	
	/// <inheritdoc />
	public void Pause() {
		lock (_sync) {
			if (!_isPaused)
				return;

			if (!_isStarted)
				Start();

			LockedPause();
			_isPaused = true;
		}
	}
	
	/// 
	protected virtual void LockedPause() { }
	
	/// <inheritdoc />
	public void Continue() {
		lock (_sync) {
			if (!_isPaused)
				return;

			if (!_isStarted)
				Start();

			LockedContinue();
			_isPaused = false;
		}
	}
	
	/// 
	protected virtual void LockedContinue() { }

	/// <inheritdoc />
	protected override void DisposeManaged() => Stop();

	bool _isStarted;
	bool _isPaused;
	readonly object _sync = new();
}
