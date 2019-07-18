using System;
using System.Linq;
using System.Reactive.Linq;
using Iw4xServerWatchDog.Configs;
using Iw4xServerWatchDog.Monitor;
using Iw4xServerWatchDog.ProcessManagement;
using NLog;

namespace Iw4xServerWatchDog
{
	public class WatchDog
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( );

		public Settings Settings { get; }
		public ProcessWatcherCollection Watchers { get; private set; }
		public ServerMonitorCollection ServerMonitors { get; private set; }

		public WatchDog ( Settings settings )
		{
			Settings = settings;
		}

		public void Init ( )
		{
			CreateProcessWatchers ( );
			CreateServerMonitors ( );

			ServerMonitors.Where ( x => x.Type == ServerEventType.Offline ).Subscribe ( OnOffline );
			ServerMonitors.Where ( x => x.Type == ServerEventType.Online ).Subscribe ( OnOnline );
			ServerMonitors.Where ( x => x.Type == ServerEventType.Updated ).Subscribe ( OnUpdated );
		}

		public void Start ( )
		{
			Watchers.StartAll ( );
			ServerMonitors.StartAll ( );
		}

		private void OnOnline ( ServerStatusChangedEventArgs args )
		{
			Logger.Info ( args );
		}

		private void OnOffline ( ServerStatusChangedEventArgs args )
		{
			Logger.Warn ( args );
			Watchers.Restart ( args.Port );
		}

		private void OnUpdated ( ServerStatusChangedEventArgs args )
		{
			Logger.Debug ( args );

			if ( args.ServerInfo.Status.MapName != args.OldServerInfo.Status.MapName )
				Logger.Info (
					$"{args.Port} {args.ServerInfo.Status.SvHostname} changed map to {args.ServerInfo.Status.MapName}" );
		}

		private void CreateProcessWatchers ( )
		{
			Watchers = new ProcessWatcherCollection ( );
			foreach ( var server in Settings.Servers )
				Watchers.Add ( new ProcessWatcher ( server.Port, server.Name, server.Path, server.ProcessName ) );
		}

		private void CreateServerMonitors ( )
		{
			ServerMonitors = new ServerMonitorCollection ( );
			foreach ( var port in Settings.Servers.Select ( x => x.Port ) )
				ServerMonitors.Add ( port );
		}
	}
}