using System;
using System.Collections.Generic;
using PortableJsonConfig;

namespace Iw4xServerWatchDog.DiscordBot.Configs
{
	public class DiscordBotConfig : IConfig<DiscordBotConfig>, IDiscordBotConfig
	{
		public string ConfigFileName { get; } = "Discord";
		public string ConfigFolderName { get; } = "Configs";

		public string Token { get; set; } = "Enter your Discord bot token here";
		public string Prefix { get; set; } = "!";

		// ReSharper disable once InconsistentNaming
		public string ExternalIP { get; set; } = "127.0.0.1";
		public TimeSpan MessageCooldown { get; set; } = TimeSpan.FromSeconds ( 2 );

		public DiscordBotConfig RestoreDefaults ( ) => this;

		public bool TryValidate ( out IList<Exception> exceptions )
		{
			exceptions = new List<Exception> ( );
			return exceptions.Count == 0;
		}
	}
}