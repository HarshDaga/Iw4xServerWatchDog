using System;
using System.Collections.Generic;
using Iw4xServerWatchDog.Monitor.Configs;

namespace Iw4xServerWatchDog.Monitor
{
	public interface IServerMonitorService : IObservable<ServerStatusChangedEventArgs>
	{
		IServersConfig Config { get; }
		IReadOnlyDictionary<int, ServerMonitor> Monitors { get; }

		void Add ( int port );
		void StartAll ( );
		void StopAll ( );
	}
}