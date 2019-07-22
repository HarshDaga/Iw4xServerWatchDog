using System;
using System.Threading;
using Autofac;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.Monitor;
using Iw4xServerWatchDog.Monitor.Configs;
using Iw4xServerWatchDog.ProcessManagement;
using PortableJsonConfig;

namespace Iw4xServerWatchDog
{
	public class Program
	{
		private static readonly ManualResetEvent Mre = new ManualResetEvent ( false );

		public static void Main ( )
		{
			Console.Title = "IW4X Server Watch Dog";

			var container = BuildContainer ( );

			var watchDog = container.Resolve<IWatchDog> ( );
			watchDog.Init ( );
			watchDog.Start ( );

			Console.CancelKeyPress += ( _, args ) => Mre.Set ( );
			Mre.WaitOne ( );
		}

		public static IContainer BuildContainer ( )
		{
			var builder = new ContainerBuilder ( );

			var servers = ConfigManager<ServersConfig>.Instance;
			var discordConfig = ConfigManager<DiscordBotConfig>.Instance;
			var resources = ConfigManager<CommonResources>.Instance;

			builder.RegisterInstance ( servers ).As<IServersConfig> ( );
			builder.RegisterInstance ( discordConfig ).As<IDiscordBotConfig> ( );
			builder.RegisterInstance ( resources ).As<ICommonResources> ( );
			builder.RegisterType<ServerMonitorService> ( ).As<IServerMonitorService> ( ).SingleInstance ( );
			builder.RegisterType<ProcessWatcherService> ( ).As<IProcessWatcherService> ( ).SingleInstance ( );
			builder.RegisterType<DiscordBotServiceService> ( ).As<IDiscordBotService> ( ).SingleInstance ( );
			builder.RegisterType<WatchDog> ( ).As<IWatchDog> ( ).SingleInstance ( );

			return builder.Build ( );
		}
	}
}