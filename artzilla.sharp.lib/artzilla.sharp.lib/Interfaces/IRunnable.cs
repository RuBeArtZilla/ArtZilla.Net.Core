using ArtZilla.Net.Core.Patterns;

namespace ArtZilla.Net.Core.Interfaces;

public enum State {
	Stopped,
	StartPending,
	StopPending,
	Running,
	ContinuePending,
	PausePending,
	Paused
}

/// <summary>Запускаемый класс</summary>
public interface IRunnable {
	bool IsStarted { get; }

	void Start();
	void Stop();
}

public interface IPausable : IRunnable {
	bool IsPaused { get; }

	void Pause();
	void Continue();
}

/// <summary> Not ready for work. Untested. </summary>
public class Runnable : Disposable, IRunnable {
	public bool IsStarted {
		get { lock (_sync) return _isStarted; }
		set {
			if (value)
				Start();
			else
				Stop();
		}
	}

	public void Start() {
		lock (_sync) {
			if (_isStarted)
				return;

			LockedStart();
			_isStarted = true;
		}
	}

	protected virtual void LockedStart() { }

	public void Stop() {
		lock (_sync) {
			if (!_isStarted)
				return;

			LockedStop();
			_isStarted = false;
		}
	}

	protected virtual void LockedStop() { }

	protected override void DisposeManaged() => Stop();

	private bool _isStarted;
	private readonly object _sync = new object();
}

/// <summary> Not ready for work. Untested. </summary>
public class RunnableAndPausable : Disposable, IRunnable, IPausable {
	public bool IsStarted {
		get { lock (_sync) return _isStarted; }
		set {
			if (value)
				Start();
			else
				Stop();
		}
	}

	public bool IsPaused {
		get { lock (_sync) return _isPaused; }
		set {
			if (value)
				Pause();
			else
				Continue();
		}
	}

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

	protected virtual void LockedStart() { }

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

	protected virtual void LockedStop() { }

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

	protected virtual void LockedPause() { }

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

	protected virtual void LockedContinue() { }

	protected override void DisposeManaged() => Stop();

	private bool _isStarted;
	private bool _isPaused;
	private readonly object _sync = new object();
}
