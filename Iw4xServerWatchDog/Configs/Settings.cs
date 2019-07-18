using System;
using System.Collections.Generic;
using PortableJsonConfig;

namespace Iw4xServerWatchDog.Configs
{
	public class Settings : IConfig<Settings>
	{
		public string ConfigFileName => "Settings";

		public List<ServerPoco> Servers { get; set; } = new List<ServerPoco>
		{
			new ServerPoco {Port = 28960, Name = "Server Name as in cfg", Path = "path to bat file"}
		};

		public Settings RestoreDefaults ( ) => this;

		public bool TryValidate ( out IList<Exception> exceptions )
		{
			exceptions = new List<Exception> ( );

			return exceptions.Count == 0;
		}
	}
}