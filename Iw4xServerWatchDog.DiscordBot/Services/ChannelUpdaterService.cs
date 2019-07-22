using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Discord;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;

namespace Iw4xServerWatchDog.DiscordBot.Services
{
	public class ChannelUpdaterService : IChannelUpdaterService
	{
		public IDiscordBotConfig Config { get; }
		public ICommonResources Resources { get; }
		public ILiveEmbedService EmbedService { get; }

		private readonly ConcurrentDictionary<int, IUserMessage> messages;
		private IMessageChannel channel;
		private DateTime nextMessageWaitTime;

		public ChannelUpdaterService ( IDiscordBotConfig config,
		                               ICommonResources resources,
		                               ILiveEmbedService embedService )
		{
			Config       = config;
			Resources    = resources;
			EmbedService = embedService;

			messages            = new ConcurrentDictionary<int, IUserMessage> ( );
			nextMessageWaitTime = DateTime.Now;

			EmbedService.Subscribe ( UpdateChannel );
		}

		public async Task Subscribe ( IMessageChannel messageChannel, ulong botId )
		{
			if ( channel != null && channel != messageChannel )
				await ClearChannelAsync ( channel, botId );

			channel = messageChannel;
			await ClearChannelAsync ( messageChannel, botId );
			messages.Clear ( );

			foreach ( var embedInfo in EmbedService.Embeds.Values )
				await UpdateChannelAsync ( embedInfo );
		}

		private static async Task ClearChannelAsync ( IMessageChannel messageChannel, ulong botId )
		{
			await Utility.DeleteMessagesAsync (
				messageChannel.GetMessagesAsync ( ),
				m => m.Author.Id == botId
			);
		}

		private void UpdateChannel ( ServerEmbedInfo embedInfo ) =>
			Task.Run ( ( ) => UpdateChannelAsync ( embedInfo ) );

		private async Task UpdateChannelAsync ( ServerEmbedInfo embedInfo )
		{
			if ( channel is null )
				return;

			var now = DateTime.Now;
			if ( nextMessageWaitTime > now )
				await Task.Delay ( nextMessageWaitTime - now );
			nextMessageWaitTime = now + Config.MessageCooldown;

			var port = embedInfo.Port;
			if ( messages.TryGetValue ( port, out var message ) )
				await message.ModifyAsync ( properties => { properties.Embed = embedInfo.Embed; } );
			else
			{
				message        = await channel.SendMessageAsync ( embed: embedInfo.Embed );
				messages[port] = message;
			}
		}
	}
}