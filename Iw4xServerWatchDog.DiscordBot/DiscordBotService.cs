using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.DiscordBot.Services;
using Iw4xServerWatchDog.Monitor;
using Iw4xServerWatchDog.ProcessManagement;
using NLog;

namespace Iw4xServerWatchDog.DiscordBot
{
	public class DiscordBotService : IDiscordBotService
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( );

		public IDiscordBotConfig Config { get; }
		public IServerMonitorService MonitorService { get; }
		public IProcessWatcherService WatcherService { get; }
		public ICommonResources Resources { get; }

		private readonly DiscordSocketClient client;
		private readonly CommandService commands;
		private readonly ChannelUpdaterService channelUpdaterService;
		private readonly IServiceProvider services;

		public DiscordBotService ( IDiscordBotConfig config,
		                                  IServerMonitorService monitorService,
		                                  IProcessWatcherService watcherService,
		                                  ICommonResources resources )
		{
			Config         = config;
			MonitorService = monitorService;
			WatcherService = watcherService;
			Resources      = resources;

			client                 =  new DiscordSocketClient ( );
			client.Log             += Log;
			client.MessageReceived += MessageReceivedAsync;

			commands                 =  new CommandService ( );
			commands.Log             += Log;
			commands.CommandExecuted += CommandExecutedAsync;

			channelUpdaterService = new ChannelUpdaterService ( Config, Resources );

			var container = BuildContainer ( );
			services = new AutofacServiceProvider ( container );
			AddCommands ( );
		}

		public async Task StartAsync ( )
		{
			await client.LoginAsync ( TokenType.Bot, Config.Token );
			await client.StartAsync ( );
			await client.SetGameAsync ( "iw4x", null, ActivityType.Watching );

			foreach ( var server in MonitorService.Config.Servers )
				channelUpdaterService.Add ( server.Port, server.Name );
			channelUpdaterService.SubscribeTo ( MonitorService );
		}

		private IContainer BuildContainer ( )
		{
			var builder = new ContainerBuilder ( );
			builder.RegisterInstance ( client ).AsSelf ( ).As<IDiscordClient> ( );
			builder.RegisterInstance ( commands ).AsSelf ( );
			builder.RegisterInstance ( MonitorService ).As<IServerMonitorService> ( );
			builder.RegisterInstance ( WatcherService ).As<IProcessWatcherService> ( );
			builder.RegisterInstance ( Resources ).As<ICommonResources> ( );
			builder.RegisterInstance ( Config ).As<IDiscordBotConfig> ( );
			builder.RegisterInstance ( channelUpdaterService ).As<IChannelUpdaterService> ( );
			return builder.Build ( );
		}

		private void AddCommands ( )
		{
			commands
				.AddModuleAsync<ServerModule> ( services )
				.GetAwaiter ( )
				.GetResult ( );
		}

		private async Task MessageReceivedAsync ( SocketMessage rawMessage )
		{
			if ( !( rawMessage is SocketUserMessage message ) ) return;
			if ( message.Source != MessageSource.User ) return;

			var argPos = 0;
			if ( !message.HasStringPrefix ( Config.Prefix, ref argPos, StringComparison.OrdinalIgnoreCase ) )
				return;

			var context = new SocketCommandContext ( client, message );
			await commands.ExecuteAsync ( context, argPos, services );
		}

		private async Task CommandExecutedAsync ( Optional<CommandInfo> command,
		                                          ICommandContext context,
		                                          IResult result )
		{
			if ( !command.IsSpecified )
				return;

			if ( result.IsSuccess )
				return;

			await context.Channel.SendMessageAsync ( $"Error: {result}" );
		}

		private static Task Log ( LogMessage arg )
		{
			switch ( arg.Severity )
			{
				case LogSeverity.Critical:
					Logger.Fatal ( arg.Message, arg );
					break;
				case LogSeverity.Error:
					Logger.Error ( arg.Message, arg );
					break;
				case LogSeverity.Warning:
					Logger.Warn ( arg.Message, arg );
					break;
				case LogSeverity.Info:
					Logger.Info ( arg.Message );
					break;
				case LogSeverity.Verbose:
					Logger.Debug ( arg.Message );
					break;
				case LogSeverity.Debug:
					Logger.Trace ( arg.Message );
					break;
			}

			return Task.CompletedTask;
		}
	}
}