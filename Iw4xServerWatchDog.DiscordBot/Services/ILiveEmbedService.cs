﻿using System;
using System.Collections.Immutable;
using Discord;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.Monitor;

namespace Iw4xServerWatchDog.DiscordBot.Services
{
	public interface ILiveEmbedService : IObservable<ServerEmbedInfo>
	{
		IDiscordBotConfig Config { get; }
		ICommonResources Resources { get; }
		ImmutableDictionary<int, ServerEmbedInfo> Embeds { get; }

		void Add ( int port, string serverName );
		Embed GetEmbed ( int port );
		IDisposable SubscribeTo ( IObservable<ServerStatusChangedEventArgs> observable );
	}
}