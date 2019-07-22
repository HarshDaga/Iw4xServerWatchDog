using System;

namespace Iw4xServerWatchDog.DiscordBot.Configs
{
	public interface IDiscordBotConfig
	{
		string Token { get; set; }
		string Prefix { get; set; }

		// ReSharper disable once InconsistentNaming
		string ExternalIP { get; set; }
		TimeSpan MessageCooldown { get; set; }
	}
}