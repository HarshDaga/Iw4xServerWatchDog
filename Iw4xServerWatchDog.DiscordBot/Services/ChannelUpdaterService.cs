using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.DiscordBot.Services.Interfaces;
using Nito.AsyncEx;

namespace Iw4xServerWatchDog.DiscordBot.Services
{
	public class ChannelUpdaterService : IChannelUpdaterService
	{
		public IDiscordBotConfig Config { get; }
		public ILiveEmbedService EmbedService { get; }
		private readonly AsyncLock mutex = new AsyncLock ( );

		private ImmutableDictionary<int, IUserMessage> messages;
		private IMessageChannel channel;
		private DateTime nextMessageWaitTime;

		public ChannelUpdaterService ( IDiscordBotConfig config,
		                               ILiveEmbedService embedService )
		{
			Config       = config;
			EmbedService = embedService;

			messages            = ImmutableDictionary<int, IUserMessage>.Empty;
			nextMessageWaitTime = DateTime.Now;

			EmbedService.Subscribe ( UpdateChannel );
		}

		public async Task Subscribe ( IMessageChannel messageChannel, ulong botId )
		{
			if ( channel != null && channel != messageChannel )
				await ClearChannelAsync ( channel, botId );

			channel = messageChannel;
			await ClearChannelAsync ( messageChannel, botId );
			messages = messages.Clear ( );

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

			using ( await mutex.LockAsync ( ) )
			{
				var now = DateTime.Now;
				if ( nextMessageWaitTime > now )
					await Task.Delay ( nextMessageWaitTime - now );
				nextMessageWaitTime = now + Config.MessageCooldown;

				var port = embedInfo.Port;
				if ( messages.TryGetValue ( port, out var message ) )
					await message.ModifyAsync ( properties => { properties.Embed = embedInfo.Embed; } );
				else
				{
					message  = await channel.SendMessageAsync ( embed: embedInfo.Embed );
					messages = messages.SetItem ( port, message );
				}
			}
		}
	}
}