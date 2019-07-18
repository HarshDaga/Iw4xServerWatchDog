using Iw4xServerWatchDog.Monitor.Types;

namespace Iw4xServerWatchDog.Monitor
{
	public enum ServerEventType
	{
		Online,
		Offline,
		Updated
	}

	public class ServerStatusChangedEventArgs
	{
		public ServerEventType Type { get; set; }
		public int Port { get; set; }
		public ServerInfo ServerInfo { get; set; }
		public ServerInfo OldServerInfo { get; set; }

		internal static ServerStatusChangedEventArgs Online ( ServerInfo info ) =>
			new ServerStatusChangedEventArgs
			{
				Type       = ServerEventType.Online,
				Port       = info.Port,
				ServerInfo = info
			};

		internal static ServerStatusChangedEventArgs Offline ( int port ) =>
			new ServerStatusChangedEventArgs
			{
				Type = ServerEventType.Offline,
				Port = port
			};

		internal static ServerStatusChangedEventArgs Updated ( ServerInfo info, ServerInfo old ) =>
			new ServerStatusChangedEventArgs
			{
				Type          = ServerEventType.Updated,
				Port          = info.Port,
				ServerInfo    = info,
				OldServerInfo = old
			};

		public override string ToString ( )
		{
			switch ( Type )
			{
				case ServerEventType.Online:
					return
						$"Online: {Port} {ServerInfo.Status.SvHostname} {ServerInfo.Status.MapName} {ServerInfo.Players.Count}/{ServerInfo.Status.SvMaxClients}";
				case ServerEventType.Offline:
					return $"Offline: {Port}";
				case ServerEventType.Updated:
					return
						$"Updated: {Port} {ServerInfo.Status.SvHostname} {ServerInfo.Status.MapName} {ServerInfo.Players.Count}/{ServerInfo.Status.SvMaxClients}";
				default:
					return $"Unknown event type {Type}";
			}
		}
	}
}