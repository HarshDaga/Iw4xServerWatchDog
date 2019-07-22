using System.Threading.Tasks;
using Discord;
using Iw4xServerWatchDog.DiscordBot.Configs;

namespace Iw4xServerWatchDog.DiscordBot.Services.Interfaces
{
	public interface IPersonalNotificationService
	{
		IDiscordBotConfig Config { get; }
		ILiveEmbedService EmbedService { get; }

		Task Subscribe ( IUser user );
	}
}