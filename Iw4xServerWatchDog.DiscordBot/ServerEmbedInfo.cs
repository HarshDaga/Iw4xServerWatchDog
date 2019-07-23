using Discord;
using Iw4xServerWatchDog.Common.Configs;
using Iw4xServerWatchDog.Common.Types;
using Iw4xServerWatchDog.DiscordBot.Configs;
using Iw4xServerWatchDog.Monitor;
using Iw4xServerWatchDog.Monitor.Types;

namespace Iw4xServerWatchDog.DiscordBot
{
	internal class ServerEmbedInfo : IServerEmbedInfo
	{
		public IDiscordBotConfig Config { get; }
		public ICommonResources Resources { get; }
		public int Port { get; }
		public string ConnectString { get; }

		public ServerInfo ServerInfo { get; private set; }
		public ServerEventType EventType { get; private set; }
		public string ServerName { get; private set; }
		public string MapName { get; private set; }
		public Map Map { get; private set; }
		public string GameType { get; private set; }
		public int MaxPlayers { get; private set; }
		public Embed Embed { get; private set; }

		public ServerEmbedInfo ( IDiscordBotConfig config,
		                         ICommonResources resources,
		                         int port,
		                         string serverName )
		{
			Config     = config;
			Resources  = resources;
			Port       = port;
			ServerName = serverName;

			ConnectString = $"iw4x://{Config.ExternalIP}:{Port}";
		}

		public void OnChange ( ServerStatusChangedEventArgs args )
		{
			var old = ServerInfo;
			ServerInfo = args.ServerInfo;
			var isOffline = args.Type == ServerEventType.Offline;
			var info = isOffline ? old : args.ServerInfo;
			EventType = args.Type;

			var builder = GetBuilder ( info, isOffline );
			Embed = builder.Build ( );
		}

		private EmbedBuilder GetBuilder ( ServerInfo info, bool isOffline )
		{
			Update ( info );

			var builder = new EmbedBuilder
			{
				Title  = isOffline ? "Offline" : "Online",
				Author = new EmbedAuthorBuilder {Name = ServerName},
				Color  = isOffline ? Color.Red : Color.Green
			};
			if ( info != null )
				builder
					.WithThumbnailUrl ( Map?.Image )
					.AddField ( "Map", Map?.Name, true )
					.AddField ( "Game Type", GameType, true )
					.AddField ( "Players", $"{info.Players.Count}/{MaxPlayers}", true )
					.WithCurrentTimestamp ( );
			if ( isOffline )
				builder.WithFooter ( "The data above is the server state before going offline" );
			builder.Description = $"<{ConnectString}>";

			return builder;
		}

		private void Update ( ServerInfo info )
		{
			if ( info?.Status is null )
			{
				MapName    = GameType = null;
				Map        = null;
				MaxPlayers = 0;
				return;
			}

			ServerName = Utility.RemoveColors ( info.Status.SvHostname );
			MapName    = info.Status.MapName;
			GameType   = info.Status.GameType;
			MaxPlayers = info.Status.SvMaxClients;

			if ( Resources?.Map is null )
				return;

			if ( Resources.Map.TryGetValue ( MapName, out var map ) )
				Map = map;

			if ( Resources.GameType.TryGetValue ( GameType, out var gameTypeFull ) )
				GameType = gameTypeFull;
		}
	}
}