#if !NET40

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArtZilla.Net.Core;

/// task repeated on background thread
[Obsolete("Not ready for usage")]
public sealed class BackgroundTask<T> {
	/// todo: ...
	public delegate Task BackgroundAction(T arg, CancellationToken token = default);

	/// todo: ...
	public delegate void OnError(T arg, Exception exception);

	/// todo: ...
	public bool IsExecuting { get; private set; }

	/// todo: ...
	public BackgroundTask(BackgroundAction action)
		: this(action, OnErrorDefault) { }

	/// todo: ...
	public BackgroundTask(BackgroundAction action, OnError onError) {
		_action = action;
		_onError = onError ?? throw new ArgumentNullException(nameof(onError));
	}

	/// todo: ...
	public void Cancel()
		=> _cts.Cancel();

	/// todo: ...
	public void Run(T arg)
		=> ExecuteAsync(arg);

	/// todo: ...
	public async Task RunAsync(T arg)
		=> ExecuteAsync(arg);

	private async Task ExecuteAsync(T arg) {
		_args.Enqueue(arg);

		Task.Run(() => ExecuteAsync()); // await ExecuteAsync();
	}

	private async Task ExecuteAsync() {
		IsExecuting = true;

		var token = _cts.Token;
		while (!token.IsCancellationRequested && _args.Count > 0) {
			var arg = _args.Dequeue();

			try {
				await _action(arg, token);
			} catch (Exception e) {
				_onError(arg, e);
			}
		}

		IsExecuting = false;
	}

	private static void OnErrorDefault(T arg, Exception exception)
		=> Console.WriteLine($"BackgroundTask({arg}) thrown {exception.Message}");

	private readonly OnError _onError;
	private readonly BackgroundAction _action;
	private readonly Queue<T> _args = new();
	private readonly CancellationTokenSource _cts = new();
}

#endif