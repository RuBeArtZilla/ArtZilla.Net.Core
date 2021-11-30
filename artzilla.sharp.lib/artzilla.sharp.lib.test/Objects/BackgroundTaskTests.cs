using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Objects;

[TestClass]
public class BackgroundTaskTests {
	// [TestMethod]
	public void Example() {
		var local = 0;

		async Task LocalTask(int a, CancellationToken token = default) {
			local = -a;
			await Task.Delay(10, token);
			local = a;
			Console.WriteLine(a);
		}

		var task = new BackgroundTask<int>(LocalTask);
		for (var i = 0; i <= 10; i++)
			task.Run(i);

		var completed = SpinWait.SpinUntil(() => !task.IsExecuting, 1000);
		Assert.IsTrue(completed);

		Assert.AreEqual(10, local);
	}

	// [TestMethod]
	public async Task CancelExampleAsync() {
		async Task LocalTask(int a, CancellationToken token)
			=> await Task.Delay(TimeSpan.FromSeconds(2D), token);

		var sw = Stopwatch.StartNew();
		var task = new BackgroundTask<int>(LocalTask);

		task.Run(0);
		Assert.IsTrue(task.IsExecuting);

		task.Cancel();
		Assert.IsFalse(task.IsExecuting);

		await task.RunAsync(0);
		Assert.IsTrue(task.IsExecuting);

		task.Cancel();
		Assert.IsFalse(task.IsExecuting);

		Assert.IsFalse(sw.Elapsed.Seconds > 2D);
	}

}
