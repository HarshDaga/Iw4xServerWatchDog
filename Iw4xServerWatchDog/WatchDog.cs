using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Iw4xServerWatchDog.DiscordBot;
using Iw4xServerWatchDog.Monitor;
using Iw4xServerWatchDog.Monitor.Configs;
using Iw4xServerWatchDog.ProcessManagement;
using NLog;

namespace Iw4xServerWatchDog
{
	public class WatchDog : IWatchDog
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( );

		public IServersConfig ServersConfig { get; }
		public IProcessWatcherService WatcherService { get; }
		public IServerMonitorService MonitorService { get; }
		public IDiscordBotService DiscordBotService { get; }

		public WatchDog ( IServerMonitorService monitorService,
		                  IProcessWatcherService watcherService,
		                  IServersConfig serversConfig,
		                  IDiscordBotService discordBotService )
		{
			MonitorService    = monitorService;
			WatcherService    = watcherService;
			ServersConfig     = serversConfig;
			DiscordBotService = discordBotService;
		}

		public void Init ( )
		{
			AddProcessWatchers ( );
			AddServerMonitors ( );

			MonitorService.Where ( x => x.Type == ServerEventType.Offline ).Subscribe ( OnOffline );
			MonitorService.Where ( x => x.Type == ServerEventType.Online ).Subscribe ( OnOnline );
			MonitorService.Where ( x => x.Type == ServerEventType.Updated ).Subscribe ( OnUpdated );
		}

		public async Task StartAsync ( )
		{
			MonitorService.StartAll ( );
			await DiscordBotService.StartAsync ( );
		}

		private void OnOnline ( ServerStatusChangedEventArgs args )
		{
			Logger.Info ( args );
		}

		private void OnOffline ( ServerStatusChangedEventArgs args )
		{
			Logger.Warn ( args );
			WatcherService.Restart ( args.Port );
		}

		private void OnUpdated ( ServerStatusChangedEventArgs args )
		{
			Logger.Debug ( args );

			if ( args.ServerInfo.Status.MapName != args.OldServerInfo.Status.MapName )
				Logger.Info (
					$"{args.Port} {args.ServerInfo.Status.SvHostname} changed map to {args.ServerInfo.Status.MapName}" );
		}

		private void AddProcessWatchers ( )
		{
			foreach ( var server in ServersConfig.Servers )
				WatcherService.Add ( new ProcessWatcher ( server.Port, server.Name, server.Path, server.ProcessName ) );
		}

		private void AddServerMonitors ( )
		{
			foreach ( var port in ServersConfig.Servers.Select ( x => x.Port ) )
				MonitorService.Add ( port );
		}
	}
}