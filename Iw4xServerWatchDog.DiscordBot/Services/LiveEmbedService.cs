﻿using System;
using System.Collections.Immutable;
using System.Reactive.Subjects;
using System.Threading;
using Discord;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.DiscordBot.Services.Interfaces;
using Iw4xServerWatchDog.Monitor;

namespace Iw4xServerWatchDog.DiscordBot.Services
{
	internal class LiveEmbedService : ILiveEmbedService
	{
		public IDiscordBotConfig Config { get; }
		public ICommonResources Resources { get; }
		public ImmutableDictionary<int, IServerEmbedInfo> Embeds => embeds;

		private readonly Subject<IServerEmbedInfo> subject;
		private ImmutableDictionary<int, IServerEmbedInfo> embeds;

		public LiveEmbedService ( IDiscordBotConfig config, ICommonResources resources )
		{
			Config    = config;
			Resources = resources;

			embeds  = ImmutableDictionary<int, IServerEmbedInfo>.Empty;
			subject = new Subject<IServerEmbedInfo> ( );
		}

		public void Add ( int port, string serverName )
		{
			var embedInfo = new ServerEmbedInfo ( Config, Resources, port, serverName );
			Interlocked.Exchange ( ref embeds, embeds.SetItem ( port, embedInfo ) );
		}

		public Embed GetEmbed ( int port ) =>
			embeds.TryGetValue ( port, out var embedInfo ) ? embedInfo.Embed : null;

		public IDisposable SubscribeTo ( IObservable<ServerStatusChangedEventArgs> observable ) =>
			observable.Subscribe ( OnServerStatusChange );

		public IDisposable Subscribe ( IObserver<IServerEmbedInfo> observer ) =>
			subject.Subscribe ( observer );

		private void OnServerStatusChange ( ServerStatusChangedEventArgs args )
		{
			if ( !embeds.TryGetValue ( args.Port, out var embedInfo ) )
			{
				embedInfo = new ServerEmbedInfo ( Config, Resources, args.Port, "Unknown" );
				Interlocked.Exchange ( ref embeds, embeds.SetItem ( args.Port, embedInfo ) );
			}

			if ( args.Type == ServerEventType.Updated &&
			     args.ServerInfo.Players.Count == args.OldServerInfo.Players.Count &&
			     args.ServerInfo.Status.GameType == args.OldServerInfo.Status.GameType &&
			     args.ServerInfo.Status.MapName == args.OldServerInfo.Status.MapName )
				return;

			embedInfo.OnChange ( args );
			subject.OnNext ( embedInfo );
		}
	}
}