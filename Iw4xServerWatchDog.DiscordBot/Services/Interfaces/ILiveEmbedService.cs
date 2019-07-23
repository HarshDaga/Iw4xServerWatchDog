using System;
using System.Collections.Immutable;
using Discord;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.Monitor;

namespace Iw4xServerWatchDog.DiscordBot.Services.Interfaces
{
	public interface ILiveEmbedService : IObservable<IServerEmbedInfo>
	{
		IDiscordBotConfig Config { get; }
		ICommonResources Resources { get; }
		ImmutableDictionary<int, IServerEmbedInfo> Embeds { get; }

		void Add ( int port, string serverName );
		Embed GetEmbed ( int port );
		IDisposable SubscribeTo ( IObservable<ServerStatusChangedEventArgs> observable );
	}
}