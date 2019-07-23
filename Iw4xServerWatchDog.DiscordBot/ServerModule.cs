using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.DiscordBot.Services.Interfaces;
using Iw4xServerWatchDog.Monitor;

namespace Iw4xServerWatchDog.DiscordBot
{
	internal class ServerModule : ModuleBase<SocketCommandContext>
	{
		public IDiscordBotConfig Config { get; }
		public IServerMonitorService MonitorService { get; }
		public ILiveEmbedService EmbedService { get; }
		public IChannelUpdaterService ChannelUpdaterService { get; }
		public IPersonalNotificationService PersonalNotificationService { get; }
		public ICommonResources Resources { get; }

		public ServerModule (
			IDiscordBotConfig config,
			IServerMonitorService monitorService,
			ILiveEmbedService embedService,
			IChannelUpdaterService channelUpdaterService,
			IPersonalNotificationService personalNotificationService,
			ICommonResources resources
		)
		{
			Config                      = config;
			MonitorService              = monitorService;
			EmbedService                = embedService;
			ChannelUpdaterService       = channelUpdaterService;
			PersonalNotificationService = personalNotificationService;
			Resources                   = resources;
		}

		[Command ( "status", RunMode = RunMode.Async )]
		[Summary ( "Display the status of all servers" )]
		public async Task StatusAsync ( )
		{
			foreach ( var monitor in MonitorService.Monitors.Values )
			{
				var embed = EmbedService.GetEmbed ( monitor.Port );
				if ( embed != null )
					await Context.User.SendMessageAsync ( embed: embed );
			}
		}

		[Command ( "sub", RunMode = RunMode.Async )]
		[Remarks ( "Only 1 channel can be subscribed at a time" )]
		[Summary ( "Subscribe to a channel for live server status updates" )]
		[RequireBotPermission ( ChannelPermission.ManageMessages )]
		[RequireUserPermission ( ChannelPermission.ManageMessages )]
		public async Task SubAsync ( )
		{
			var channel = Context.Channel;
			var messages = channel.GetMessagesAsync ( );
			await Utility.DeleteMessagesAsync (
				messages,
				x => x.Author.Id == Context.Client.CurrentUser.Id
			);
			await Context.Message.TryDeleteAsync ( );

			await ChannelUpdaterService.Subscribe ( channel, Context.Client.CurrentUser.Id );
		}

		[Command ( "pingme", RunMode = RunMode.Async )]
		[Remarks ( "Only notifies when a server goes offline" )]
		[Summary ( "Receive server offline notifications as direct messages" )]
		public async Task PingMeAsync ( )
		{
			var user = Context.User;
			await PersonalNotificationService.Subscribe ( user );
			await Context.Message.TryDeleteAsync ( );
		}
	}
}