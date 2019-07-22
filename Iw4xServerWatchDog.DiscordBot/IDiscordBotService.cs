using System.Threading.Tasks;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.Monitor;
using Iw4xServerWatchDog.ProcessManagement;

namespace Iw4xServerWatchDog.DiscordBot
{
	public interface IDiscordBotService
	{
		IDiscordBotConfig Config { get; }
		IServerMonitorService MonitorService { get; }
		ICommonResources Resources { get; }
		IProcessWatcherService WatcherService { get; }

		Task StartAsync ( );
	}
}