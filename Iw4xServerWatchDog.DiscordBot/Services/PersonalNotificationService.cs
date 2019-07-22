using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.DiscordBot.Services.Interfaces;
using Iw4xServerWatchDog.Monitor;
using Nito.AsyncEx;

namespace Iw4xServerWatchDog.DiscordBot.Services
{
	public class PersonalNotificationService : IPersonalNotificationService
	{
		public IDiscordBotConfig Config { get; }
		public ILiveEmbedService EmbedService { get; }
		private readonly AsyncLock mutex = new AsyncLock ( );

		private ImmutableDictionary<ulong, IUser> users;

		public PersonalNotificationService ( IDiscordBotConfig config,
		                                     ILiveEmbedService embedService )
		{
			Config       = config;
			EmbedService = embedService;

			users = ImmutableDictionary<ulong, IUser>.Empty;

			EmbedService.Subscribe ( NotifyUsers );
		}

		public async Task Subscribe ( IUser user )
		{
			bool enabled;
			using ( mutex.Lock ( ) )
			{
				enabled = !users.ContainsKey ( user.Id );
				users   = enabled ? users.SetItem ( user.Id, user ) : users.Remove ( user.Id );
			}

			await user.SendMessageAsync ( $"Turned notifications {( enabled ? "on" : "off" )}" );
		}

		private void NotifyUsers ( ServerEmbedInfo embedInfo ) =>
			Task.Run ( ( ) => NotifyUsersAsync ( embedInfo ) );

		private async Task NotifyUsersAsync ( ServerEmbedInfo embedInfo )
		{
			if ( embedInfo.EventType != ServerEventType.Offline )
				return;

			using ( await mutex.LockAsync ( ) )
			{
				foreach ( var user in users.Values ) await user.SendMessageAsync ( embed: embedInfo.Embed );
			}
		}
	}
}