using System;
using System.Collections.Generic;
using PortableJsonConfig;

namespace Iw4xServerWatchDog.Monitor.Configs
{
	public class ServersConfig : IConfig<ServersConfig>, IServersConfig
	{
		public string ConfigFileName => "Servers";
		public string ConfigFolderName => "Configs";

		public string BaseUrl { get; set; } = @"http:\\localhost";
		public TimeSpan PollingFrequency { get; set; } = TimeSpan.FromSeconds ( 1 );
		public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds ( 6 );
		public TimeSpan RestartTime { get; set; } = TimeSpan.FromSeconds ( 8 );

		public List<ServerPoco> Servers { get; set; } = new List<ServerPoco>
		{
			new ServerPoco {Port = 28960, Name = "Server Name as in cfg", Path = "path to bat file"}
		};

		public ServersConfig RestoreDefaults ( ) => this;

		public bool TryValidate ( out IList<Exception> exceptions )
		{
			exceptions = new List<Exception> ( );

			return exceptions.Count == 0;
		}
	}
}