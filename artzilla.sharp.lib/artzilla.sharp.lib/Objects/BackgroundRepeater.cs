using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArtZilla.Net.Core;

/// Represent a background repeated action
public class BackgroundRepeater {
	public const string StopGuid = "{B098C6A3-C478-4E2B-969A-36B5F6D0B780}";

	/// Default value of <see cref="Cooldown"/> in milliseconds
	public const double DefaultCooldownMsec = 1000D;

	/// Default value of <see cref="IsCatchExceptions"/>
	public const bool DefaultIsCatchExceptions = true;

	/// Period between repeating background operation
	public TimeSpan Cooldown {
		get => _cooldown;
		set => _cooldown = CheckCooldown(value) ? value : throw new ArgumentOutOfRangeException(nameof(Cooldown));
	}

	/// When true any exception from repeated operation will be ignored
	public bool IsCatchExceptions { get; set; } = DefaultIsCatchExceptions;
	
	/// 
	public ExceptionHandlerDelegate ExceptionHandler {
		get => _exceptionHandler;
		set => _exceptionHandler = value;
	}

	/// 
	public delegate void ExceptionHandlerDelegate(BackgroundRepeater sender, Exception exception);

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	public BackgroundRepeater(Action action)
		: this(t => Cancelable(action, t)) {
		if (action == null)
			throw new ArgumentNullException(nameof(action));
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <param name="name">Task name</param>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	public BackgroundRepeater(string name, Action action)
		: this(name, t => Cancelable(action, t)) {
		if (action == null)
			throw new ArgumentNullException(nameof(action));
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	public BackgroundRepeater(Action<CancellationToken> action)
		=> _action = action ?? throw new ArgumentNullException(nameof(action));

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <param name="name">Task name</param>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	public BackgroundRepeater(string name, Action<CancellationToken> action) : this(action)
		=> _threadName = name ?? throw new ArgumentNullException(nameof(name));

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	public BackgroundRepeater(Action action, TimeSpan cooldown)
		: this(t => Cancelable(action, t), cooldown) {
		if (action == null)
			throw new ArgumentNullException(nameof(action));
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="name">Task name</param>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	public BackgroundRepeater(string name, Action action, TimeSpan cooldown)
		: this(name, t => Cancelable(action, t), cooldown) {
		if (action == null)
			throw new ArgumentNullException(nameof(action));
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	public BackgroundRepeater(Action<CancellationToken> action, TimeSpan cooldown)
		: this(action)
		=> Cooldown = cooldown;

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="name"></param>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	public BackgroundRepeater(string name, Action<CancellationToken> action, TimeSpan cooldown)
		: this(name, action)
		=> Cooldown = cooldown;

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	/// <param name="exceptionHandler">todo...</param>
	public BackgroundRepeater(Action action, TimeSpan cooldown, ExceptionHandlerDelegate exceptionHandler)
		: this(t => Cancelable(action, t), cooldown, exceptionHandler) {
		if (action == null)
			throw new ArgumentNullException(nameof(action));
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="name">Task name</param>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	/// <param name="exceptionHandler">todo...</param>
	public BackgroundRepeater(string name, Action action, TimeSpan cooldown, ExceptionHandlerDelegate exceptionHandler)
		: this(name, t => Cancelable(action, t), cooldown, exceptionHandler) {
		if (action == null)
			throw new ArgumentNullException(nameof(action));
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	/// <param name="exceptionHandler">todo...</param>
	public BackgroundRepeater(Action<CancellationToken> action, TimeSpan cooldown, ExceptionHandlerDelegate exceptionHandler)
		: this(action, cooldown) {
		_exceptionHandler = exceptionHandler;
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="name"></param>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	/// <param name="exceptionHandler">todo...</param>
	public BackgroundRepeater(string name, Action<CancellationToken> action, TimeSpan cooldown, ExceptionHandlerDelegate exceptionHandler)
		: this(name, action, cooldown) {
		_exceptionHandler = exceptionHandler;
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	/// <param name="isStarted">Initial state of the repeater</param>
	/// <param name="exceptionHandler">todo...</param>
	public BackgroundRepeater(Action action, TimeSpan cooldown, bool isStarted, ExceptionHandlerDelegate exceptionHandler = null)
		: this(t => Cancelable(action, t), cooldown, isStarted, exceptionHandler) {
		if (action == null)
			throw new ArgumentNullException(nameof(action));
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="name">Task name</param>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	/// <param name="isStarted">Initial state of the repeater</param>
	/// <param name="exceptionHandler">todo...</param>
	public BackgroundRepeater(string name, Action action, TimeSpan cooldown, bool isStarted, ExceptionHandlerDelegate exceptionHandler = null)
		: this(name, t => Cancelable(action, t), cooldown, isStarted, exceptionHandler) {
		if (action == null)
			throw new ArgumentNullException(nameof(action));
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	/// <param name="isStarted">Initial state of the repeater</param>
	/// <param name="exceptionHandler">todo...</param>
	public BackgroundRepeater(Action<CancellationToken> action, TimeSpan cooldown, bool isStarted, ExceptionHandlerDelegate exceptionHandler = null)
		: this(action, cooldown) {
		_exceptionHandler = exceptionHandler;
		Enabled(isStarted);
	}

	/// Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat.
	/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
	/// <param name="name"></param>
	/// <param name="action">The delegate that represents the code to repeat.</param>
	/// <param name="cooldown">Period between repeating background operation.</param>
	/// <param name="isStarted">Initial state of the repeater</param>
	/// <param name="exceptionHandler">todo...</param>
	public BackgroundRepeater(string name, Action<CancellationToken> action, TimeSpan cooldown, bool isStarted, ExceptionHandlerDelegate exceptionHandler = null)
		: this(name, action, cooldown) {
		_exceptionHandler = exceptionHandler;
		Enabled(isStarted);
	}

	/// <inheritdoc />
	public override string ToString() => _threadName + " (" + Cooldown + ") - " + (IsStarted() ? "enabled" : "disabled");

	/// Set repeater on/off
	/// <param name="value">todo</param>
	public void Enabled(bool value) { // todo: write test?
		if (value) Start();
		else Stop();
	}

	/// Start repeating
	public void Start() {
		lock (_sync) {
			if (_cts != null)
				return;

			_cts = new();
			_thread = new(o => Repeater(_action, o)) {
				IsBackground = true,
				Name = _threadName,
			};

			_thread?.Start(_cts.Token);
		}
	}

	/// Stop repeating
	public void Stop() {
		lock (_sync) {
			if (_cts == null)
				return;

			try {
				_cts.Cancel();
				_thread?.Join();
			} finally {
				_cts = null;
				_thread = null;
			}
		}
	}

	/// Gets a value indicating the execution status of current <see cref="BackgroundRepeater"/>
	public bool IsStarted() {
		lock (_sync)
			return _cts != null;
	}

	/// Invoke this inside of repeated method to stop repeating
	public static void InnerStop() =>
		throw new OperationCanceledException(StopGuid);

	static bool CheckCooldown(TimeSpan cooldown)
		=> cooldown >= TimeSpan.Zero;

	static void Cancelable(Action action, CancellationToken token) {
		try {
			using var t = new Task(action, token);
			t.Start();
			t.Wait(token);
		} catch (AggregateException ae) {
			if (ae.InnerExceptions.Any(e => e is OperationCanceledException && e.Message.Equals(StopGuid)))
				throw new OperationCanceledException(StopGuid);
			throw;
		}
	}

	void Repeater(Action<CancellationToken> action, object token) {
		var t = (CancellationToken) token;

		try {
			while (!t.IsCancellationRequested) {
				if (IsCatchExceptions) {
					try {
						action.Invoke(t);
					} catch (OperationCanceledException e) {
						if (e.Message.Equals(StopGuid))
							throw; // Must exit when receive this
					} catch (Exception e) {
						_exceptionHandler?.Invoke(this, e); // Test this line
					}
				} else {
					action.Invoke(t);
				}

				if (!t.IsCancellationRequested)
					t.WaitHandle.WaitOne(Cooldown);
			}
		} catch (OperationCanceledException) {
			// ignored, worked in rare cases!
		} finally {
			_cts = null;
			_thread = null;
		}
	}

	Thread _thread;
	TimeSpan _cooldown = TimeSpan.FromMilliseconds(DefaultCooldownMsec);
	CancellationTokenSource _cts;
	ExceptionHandlerDelegate _exceptionHandler;
	readonly string _threadName = nameof(BackgroundRepeater);
	readonly object _sync = new object();
	readonly Action<CancellationToken> _action;
}
