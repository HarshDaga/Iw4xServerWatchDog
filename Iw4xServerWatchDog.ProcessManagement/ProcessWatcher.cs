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
				{
					Kill ( );
					Process = Process.Start ( startInfo );
				}
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
				KillProcess ( ProcessName, Name );
				KillProcess ( ProcessName, "ERROR" );
				KillProcess ( "conhost", Name );

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

		private Process FindProcess ( ) =>
			FindProcess ( ProcessName, Name );

		private static Process FindProcess ( string name, string title )
		{
			var processes = Process.GetProcessesByName ( name );
			return processes
				.FirstOrDefault (
					process => process.MainWindowTitle.Equals ( title, StringComparison.OrdinalIgnoreCase )
				);
		}

		private static void KillProcess ( string name, string title )
		{
			var process = FindProcess ( name, title );
			if ( process != null )
			{
				var pid = process.Id;
				process.Kill ( );
				Logger.Info ( $"Killed {name} {pid}: {title}" );
			}
		}

		public override string ToString ( ) =>
			$"{( IsRunning ? "Running" : "Stopped" )} {Port}: {Name}";
	}
}