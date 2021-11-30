using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArtZilla.Net.Core;

// todo: ...

/// <summary> Represent a background repeated task </summary>
[Obsolete("dev in process")]
public class TaskRepeater {
	public TimeSpan Cooldown { get; set; }



	public TaskRepeater(Task task) {

	}

	private Thread _thread;
}
