using System;
using System.Threading.Tasks;
using Discord;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.Monitor;

namespace Iw4xServerWatchDog.DiscordBot.Services
{
	public interface IChannelUpdaterService
	{
		IDiscordBotConfig Config { get; }
		ICommonResources Resources { get; }

		void Add ( int port, string serverName );
		Embed GetEmbed ( int port );
		Task Subscribe ( IMessageChannel messageChannel, ulong botId );
		IDisposable SubscribeTo ( IObservable<ServerStatusChangedEventArgs> observable );
	}
}