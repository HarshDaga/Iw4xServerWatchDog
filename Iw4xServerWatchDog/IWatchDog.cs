using Iw4xServerWatchDog.DiscordBot;
using Iw4xServerWatchDog.Monitor;
using Iw4xServerWatchDog.Monitor.Configs;
using Iw4xServerWatchDog.ProcessManagement;

namespace Iw4xServerWatchDog
{
	public interface IWatchDog
	{
		IDiscordBotService DiscordBotService { get; }
		IServerMonitorService MonitorService { get; }
		IServersConfig ServersConfig { get; }
		IProcessWatcherService WatcherService { get; }

		void Init ( );
		void Start ( );
	}
}