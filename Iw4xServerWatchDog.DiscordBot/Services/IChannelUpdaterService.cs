using System.Threading.Tasks;
using Discord;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;

namespace Iw4xServerWatchDog.DiscordBot.Services
{
	public interface IChannelUpdaterService
	{
		IDiscordBotConfig Config { get; }
		ILiveEmbedService EmbedService { get; }
		ICommonResources Resources { get; }

		Task Subscribe ( IMessageChannel messageChannel, ulong botId );
	}
}