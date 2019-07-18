using System;
using System.Diagnostics;
using System.Linq;
using NLog;

namespace Iw4xServerWatchDog.ProcessManagement
{
	public class ProcessWatcher
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( );

		public string ProcessName { get; }
		public int Port { get; }
		public string Name { get; }
		public string Path { get; }

		private Process Process { get; set; }
		public bool IsRunning => Process?.HasExited == false;
		public int PId => Process?.Id ?? 0;

		private readonly ProcessStartInfo startInfo;

		public ProcessWatcher ( int port, string name, string path, string processName = "iw4x" )
		{
			Port        = port;
			Name        = name;
			Path        = path;
			ProcessName = processName;
			Process     = FindProcess ( );

			startInfo = new ProcessStartInfo ( Path );
			var dir = System.IO.Path.GetDirectoryName ( Path );
			if ( dir != null )
				startInfo.WorkingDirectory = dir;
		}

		public void Start ( )
		{
			try
			{
				if ( !IsRunning )
					Process = FindProcess ( );
				if ( !IsRunning )
					Process = Process.Start ( startInfo );
			}
			catch ( Exception e )
			{
				Logger.Error ( e );
			}
		}

		public void Kill ( )
		{
			try
			{
				var proc = FindProcess ( );
				proc?.Kill ( );
				Process = null;
			}
			catch ( Exception e )
			{
				Logger.Error ( e );
			}
		}

		public void Restart ( )
		{
			Kill ( );
			Start ( );
		}

		private Process FindProcess ( )
		{
			var processes = Process.GetProcessesByName ( ProcessName );
			return processes
				.FirstOrDefault (
					process => process.MainWindowTitle.Equals ( Name, StringComparison.OrdinalIgnoreCase )
				);
		}

		public override string ToString ( ) =>
			$"{( IsRunning ? "Running" : "Stopped" )} {Port}: {Name}";
	}
}