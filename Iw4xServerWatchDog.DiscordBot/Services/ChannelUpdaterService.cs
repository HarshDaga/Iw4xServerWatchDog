using System;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Discord;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.Monitor;

namespace Iw4xServerWatchDog.DiscordBot.Services
{
	public class ChannelUpdaterService : IObservable<ServerEmbedInfo>, IChannelUpdaterService
	{
		public IDiscordBotConfig Config { get; }
		public ICommonResources Resources { get; }

		private readonly ConcurrentDictionary<int, ServerEmbedInfo> embeds;
		private readonly ConcurrentDictionary<int, IUserMessage> messages;
		private readonly Subject<ServerEmbedInfo> subject;
		private IMessageChannel channel;
		private DateTime nextMessageWaitTime;

		public ChannelUpdaterService ( IDiscordBotConfig config, ICommonResources resources )
		{
			Config    = config;
			Resources = resources;

			embeds              = new ConcurrentDictionary<int, ServerEmbedInfo> ( );
			messages            = new ConcurrentDictionary<int, IUserMessage> ( );
			subject             = new Subject<ServerEmbedInfo> ( );
			nextMessageWaitTime = DateTime.Now;
		}

		public void Add ( int port, string serverName )
		{
			var embedInfo = new ServerEmbedInfo ( Config, Resources, port, serverName );
			embeds[port] = embedInfo;
		}

		public Embed GetEmbed ( int port ) =>
			embeds.TryGetValue ( port, out var embedInfo ) ? embedInfo.Embed : null;

		public IDisposable SubscribeTo ( IObservable<ServerStatusChangedEventArgs> observable ) =>
			observable.Subscribe ( OnServerStatusChange );

		public async Task Subscribe ( IMessageChannel messageChannel, ulong botId )
		{
			if ( channel != null && channel != messageChannel )
				await ClearChannelAsync ( channel, botId );

			channel = messageChannel;
			await ClearChannelAsync ( messageChannel, botId );
			messages.Clear ( );

			foreach ( var embedInfo in embeds.Values )
				await UpdateChannelAsync ( embedInfo );
		}

		public IDisposable Subscribe ( IObserver<ServerEmbedInfo> observer ) =>
			subject.Subscribe ( observer );

		private static async Task ClearChannelAsync ( IMessageChannel messageChannel, ulong botId )
		{
			await Utility.DeleteMessagesAsync (
				messageChannel.GetMessagesAsync ( ),
				m => m.Author.Id == botId,
				TimeSpan.FromMilliseconds ( 100 )
			);
		}

		private void OnServerStatusChange ( ServerStatusChangedEventArgs args )
		{
			if ( !embeds.TryGetValue ( args.Port, out var embedInfo ) )
			{
				embedInfo         = new ServerEmbedInfo ( Config, Resources, args.Port, "Unknown" );
				embeds[args.Port] = embedInfo;
			}

			if ( args.Type == ServerEventType.Updated &&
			     args.ServerInfo.Players.Count == args.OldServerInfo.Players.Count &&
			     args.ServerInfo.Status.GameType == args.OldServerInfo.Status.GameType &&
			     args.ServerInfo.Status.MapName == args.OldServerInfo.Status.MapName )
				return;

			embedInfo.OnChange ( args );
			subject.OnNext ( embedInfo );
			Task.Run ( ( ) => UpdateChannelAsync ( embedInfo ) );
		}

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