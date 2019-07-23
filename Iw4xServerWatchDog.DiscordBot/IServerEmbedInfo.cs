using Discord;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.Monitor;
using Iw4xServerWatchDog.Monitor.Types;

namespace Iw4xServerWatchDog.DiscordBot
{
	public interface IServerEmbedInfo
	{
		IDiscordBotConfig Config { get; }
		string ConnectString { get; }
		int Port { get; }
		Embed Embed { get; }
		ServerEventType EventType { get; }
		ICommonResources Resources { get; }
		ServerInfo ServerInfo { get; }

		void OnChange ( ServerStatusChangedEventArgs args );
	}
}