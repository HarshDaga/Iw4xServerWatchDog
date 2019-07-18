using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive.Subjects;
using System.Threading;

namespace Iw4xServerWatchDog.Monitor
{
	public class ServerMonitorCollection :
		IObservable<ServerStatusChangedEventArgs>
	{
		public string BaseUrl { get; }
		public IReadOnlyDictionary<int, ServerMonitor> Monitors => monitors;
		private readonly Subject<ServerStatusChangedEventArgs> subject;

		private ImmutableDictionary<int, ServerMonitor> monitors;

		public ServerMonitorCollection ( string baseUrl = @"http:\\localhost" )
		{
			BaseUrl  = baseUrl;
			monitors = ImmutableDictionary<int, ServerMonitor>.Empty;
			subject  = new Subject<ServerStatusChangedEventArgs> ( );
		}

		public IDisposable Subscribe ( IObserver<ServerStatusChangedEventArgs> observer ) =>
			subject.Subscribe ( observer );

		public void Add ( int port )
		{
			var monitor = new ServerMonitor ( port, BaseUrl );
			monitor.Subscribe ( subject );
			Interlocked.Exchange ( ref monitors, monitors.SetItem ( port, monitor ) );
		}

		public void StartAll ( )
		{
			foreach ( var monitor in monitors.Values )
				monitor.Start ( );
		}

		public void StopAll ( )
		{
			foreach ( var monitor in monitors.Values )
				monitor.Stop ( );
		}
	}
}