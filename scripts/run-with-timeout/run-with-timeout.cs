// arguments are: <platform> <outputPath>

using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;

if (args.Length <= 1) {
	Console.WriteLine ($"Need two arguments (the timeout + the command to launch), got {args.Length} argument(s)");
	return 1;
}

var launchTimeout = TimeSpan.FromSeconds (10); // must launch within a few seconds.
var argIndex = 0;
var executionTimeout = TimeSpan.FromSeconds (int.Parse (args [argIndex++]));
var commands = args.Skip (argIndex).ToArray ();

var pid = Process.GetCurrentProcess ().Id;
var maxLaunchAttempts = 10;
var exitCode = -1;
for (var attempt = 0; attempt < maxLaunchAttempts; attempt++) {
	var launchTimeoutFile = Path.GetFullPath ($"launch-timeout-sentinel-{pid}-{attempt}.txt");
	var launchTimedOut = new ManualResetEvent (false);
	var p = new Process ();

	var launchTimer = new Thread (() => {
		if (p.WaitForExit ((int) launchTimeout.TotalMilliseconds)) {
			Console.WriteLine ($"App finished before launch timeout triggered.");
		} else if (!File.Exists (launchTimeoutFile)) {
			Console.WriteLine ($"Launch timed out after {launchTimeout.TotalSeconds} seconds.");
			launchTimedOut.Set ();
			p.Abort ();
		}
	}) {
		IsBackground = true,
	};

	try {
		p.StartInfo.FileName = commands [0];
		p.StartInfo.Arguments = string.Join (" ", commands.Skip (1));
		p.StartInfo.UseShellExecute = false;
		p.StartInfo.EnvironmentVariables ["LAUNCH_SENTINEL_FILE"] = launchTimeoutFile;

		Console.WriteLine ($"Launching (attempt #{attempt + 1}):");
		Console.WriteLine ($"    {p.StartInfo.FileName} {p.StartInfo.Arguments}");

		p.Start ();

		launchTimer.Start ();

		if (!p.WaitForExit ((int) executionTimeout.TotalMilliseconds)) {
			Console.WriteLine ($"Execution timed out after {executionTimeout.TotalSeconds} seconds.");
			p.Abort ();
			p.WaitForExit ();
		}

		launchTimer.Join ();

		exitCode = p.ExitCode;

		if (launchTimedOut.WaitOne (0)) {
			Console.WriteLine ($"Launching again since the launch timeout triggered.");
			continue;
		}
		Console.WriteLine ($"Execution completed with exit code {exitCode}");
	} finally {
		File.Delete (launchTimeoutFile);
		p.Dispose ();
	}
	break;
}

return exitCode;

static class NativeMethods {
	[DllImport ("/usr/lib/libc.dylib", SetLastError = true)]
	static extern int kill (int pid, int signal);

	public static void Abort (this Process process)
	{
		var exitTimeout = TimeSpan.FromSeconds (60);
		var pid = process.Id;
		Console.WriteLine ($"kill ({pid}, 6);");
		var rv = kill (pid, 6 /* SIGABRT - this triggers a crash report */);
		if (rv != 0) {
			// This might randomly happen, because there's a race condition here: we waited for the process to exit,
			// the timeout occurred so we decided to kill the process, and *then* the process exited, before we got
			// around to kill it. In that case, the kill call would fail.
			Console.WriteLine ($"Failed to execute 'kill -6 {pid}'. errno = {Marshal.GetLastWin32Error ()} - process already exited?");
			return;
		}
		var watch = Stopwatch.StartNew ();
		while (watch.Elapsed < exitTimeout) {
			Console.WriteLine ($"kill ({pid}, 0);");
			rv = kill (pid, 0); // check if pid is still alive (valid)
			if (rv != 0) {
				// Nope it's not, so it must have terminated.
				return;
			}
			Thread.Sleep (50);
		}

		// Send SIGKILL - time to finish it off.
		Console.WriteLine ($"kill ({pid}, 9);");
		kill (pid, 9);
	}
}
