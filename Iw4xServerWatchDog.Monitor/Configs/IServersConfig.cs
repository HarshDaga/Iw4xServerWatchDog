using System;
using System.Collections.Generic;

namespace Iw4xServerWatchDog.Monitor.Configs
{
	public interface IServersConfig
	{
		string BaseUrl { get; set; }
		TimeSpan PollingFrequency { get; set; }
		TimeSpan RestartTime { get; set; }
		List<ServerPoco> Servers { get; set; }
		TimeSpan Timeout { get; set; }
	}
}