using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArtZilla.Net.Core {
	/// <summary> Represent a background repeated action </summary>
	public class BackgroundRepeater {
		private const String StopGuid = "{B098C6A3-C478-4E2B-969A-36B5F6D0B780}";

		/// <summary> Default value of <see cref="Cooldown"/> in milliseconds </summary>
		public const Double DefaultCooldownMsec = 1000D;

		/// <summary> Default value of <see cref="IsCatchExceptions"/></summary>
		public const Boolean DefaultIsCatchExceptions = true;

		/// <summary> Period between repeating background operation </summary>
		public TimeSpan Cooldown {
			get { return _cooldown; }
			set { _cooldown = CheckCooldown(value) ? value : throw new ArgumentOutOfRangeException(nameof(Cooldown)); }
		}

		/// <summary> When true any exception from repeated operation will be ignored </summary>
		public Boolean IsCatchExceptions { get; set; } = DefaultIsCatchExceptions;

		/// <summary> Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat. </summary>
		/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
		/// <param name="action">The delegate that represents the code to repeat.</param>
		public BackgroundRepeater(Action action) : this(t => Cancelable(action, t)) {
			if (action == null)
				throw new ArgumentNullException(nameof(action));
		}

		/// <summary> Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat. </summary>
		/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
		/// <param name="action">The delegate that represents the code to repeat.</param>
		/// <param name="cooldown">Period between repeating background operation.</param>
		public BackgroundRepeater(Action action, TimeSpan cooldown) : this(t => Cancelable(action, t), cooldown) {
			if (action == null)
				throw new ArgumentNullException(nameof(action));
		}

		/// <summary> Initializes a new <see cref="BackgroundRepeater"/> with specified action to repeat. </summary>
		/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
		/// <param name="action">The delegate that represents the code to repeat.</param>
		/// <param name="cooldown">Period between repeating background operation.</param>
		/// <param name="isStarted">Initial state of the repeater</param>
		public BackgroundRepeater(Action action, TimeSpan cooldown, bool isStarted) : this(t => Cancelable(action, t), cooldown, isStarted) {
			if (action == null)
				throw new ArgumentNullException(nameof(action));
		}

		/// <summary> Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat. </summary>
		/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
		/// <param name="action">The delegate that represents the code to repeat.</param>
		/// <param name="cooldown">Period between repeating background operation.</param>
		/// <param name="isStarted">Initial state of the repeater</param>
		public BackgroundRepeater(Action<CancellationToken> action, TimeSpan cooldown, bool isStarted) : this(action, cooldown)
			=> Enabled(isStarted);

		/// <summary> Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat. </summary>
		/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The <paramref name="cooldown"/> argument is negative time.</exception>
		/// <param name="action">The delegate that represents the code to repeat.</param>
		/// <param name="cooldown">Period between repeating background operation.</param>
		public BackgroundRepeater(Action<CancellationToken> action, TimeSpan cooldown) : this(action)
			=> Cooldown = cooldown;

		/// <summary> Initializes a new <see cref="BackgroundRepeater"/> with specified cancellable action to repeat. </summary>
		/// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
		/// <param name="action">The delegate that represents the code to repeat.</param>
		public BackgroundRepeater(Action<CancellationToken> action)
			=> _action = action ?? throw new ArgumentNullException(nameof(action));

		/// <summary> Set repeater on/off </summary>
		public void Enabled(Boolean value) { // todo: write test?
			if (value) Start();
			else Stop();
		}

		/// <summary> Start repeating </summary>
		public void Start() {
			Debug.Assert(_sync != null, "_sync != null");

			lock (_sync) {
				if (_cts != null)
					return;

				_cts = new CancellationTokenSource();
				_thread = new Thread(o => Repeater(_action, o)) {
					IsBackground = true,
					Name = nameof(BackgroundRepeater),
				};

				_thread?.Start(_cts.Token);
			}
		}

		/// <summary> Stop repeating </summary>
		public void Stop() {
			Debug.Assert(_sync != null, "_sync != null");

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

		/// <summary> Gets a value indicating the execution status of current <see cref="BackgroundRepeater"/> </summary>
		public Boolean IsStarted() {
			Debug.Assert(_sync != null, "_sync != null");

			lock (_sync)
				return _cts != null;
		}

		/// <summary> Invoke this inside of repeated method to stop repeating </summary>
		public static void InnerStop() =>
			throw new OperationCanceledException(StopGuid);

		private static bool CheckCooldown(TimeSpan cooldown) => cooldown >= TimeSpan.Zero;

		private static void Cancelable(Action action, CancellationToken token) {
			Debug.Assert(action != null, "action != null");

			try {
				using (var t = new Task(action, token)) {
					t.Start();
					t.Wait(token);
				}
			} catch (AggregateException ae) {
				if (ae.InnerExceptions.Any(e => e is OperationCanceledException && e.Message.Equals(StopGuid)))
					throw new OperationCanceledException(StopGuid);
				throw;
			}
		}

		private void Repeater(Action<CancellationToken> action, Object token) {
			Debug.Assert(token is CancellationToken, "Wrong cancellation token.");
			Debug.Assert(action != null, "action != null");

			var t = (CancellationToken) token;

			try {
				while (!t.IsCancellationRequested) {
					if (IsCatchExceptions) {
						try {
							action.Invoke(t);
						} catch (OperationCanceledException e) {
							if (e.Message.Equals(StopGuid))
								throw; // Must exit when recieve this
						} catch {
							// ignored
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

		private Thread _thread;
		private TimeSpan _cooldown = TimeSpan.FromMilliseconds(DefaultCooldownMsec);
		private CancellationTokenSource _cts;
		private readonly Object _sync = new Object();
		private readonly Action<CancellationToken> _action;
	}
}