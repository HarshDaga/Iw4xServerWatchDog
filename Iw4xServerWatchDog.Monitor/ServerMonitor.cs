using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Iw4xServerWatchDog.Monitor.Types;
using Newtonsoft.Json;
using RestSharp;
using static Iw4xServerWatchDog.Monitor.ServerStatusChangedEventArgs;

namespace Iw4xServerWatchDog.Monitor
{
	public class ServerMonitor :
		IObservable<ServerStatusChangedEventArgs>
	{
		public int Port { get; }
		public bool IsRunning { get; private set; }
		public ServerInfo Server { get; private set; }
		public TimeSpan PollingFrequency { get; set; } = TimeSpan.FromSeconds ( 1 );
		public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds ( 6 );
		public TimeSpan RestartTime { get; set; } = TimeSpan.FromSeconds ( 10 );
		public int OfflineCount { get; private set; }
		public bool IsOnline => Server != null;

		private readonly RestClient client;
		private readonly Subject<ServerStatusChangedEventArgs> subject;
		private CancellationTokenSource cts;

		public ServerMonitor ( int port, string baseUrl = @"http:\\localhost" )
		{
			Port    = port;
			client  = new RestClient ( $"{baseUrl}:{Port}" );
			subject = new Subject<ServerStatusChangedEventArgs> ( );
		}

		public IDisposable Subscribe ( IObserver<ServerStatusChangedEventArgs> observer ) =>
			subject.Subscribe ( observer );

		public void Start ( )
		{
			if ( IsRunning )
				return;
			IsRunning = true;
			cts       = new CancellationTokenSource ( );

			Task.Run ( UpdateAsync, cts.Token );
		}

		public void Stop ( )
		{
			if ( !IsRunning )
				return;
			IsRunning = false;

			cts.Cancel ( );
		}

		private async Task UpdateAsync ( )
		{
			while ( !cts.IsCancellationRequested )
			{
				try
				{
					var old = Server;
					var resp = await GetResponse ( );
					if ( !resp.IsSuccessful )
					{
						HandleOffline ( );
						await Task.Delay ( RestartTime );
						continue;
					}

					Server      = JsonConvert.DeserializeObject<ServerInfo> ( resp.Data );
					Server.Port = Port;

					if ( old is null )
						subject.OnNext ( Online ( Server ) );
					else if ( !Server.Equals ( old ) )
						subject.OnNext ( Updated ( Server, old ) );
				}
				catch ( Exception )
				{
					HandleOffline ( );
				}

				await Task.Delay ( PollingFrequency, cts.Token );
			}
		}

		private async Task<IRestResponse<string>> GetResponse ( )
		{
			var req = new RestRequest ( "info" ) {Timeout = (int) Timeout.TotalMilliseconds};
			var resp = await client.ExecuteGetTaskAsync<string> ( req, cts.Token );
			return resp;
		}

		private void HandleOffline ( )
		{
			var wasOffline = Server is null;
			Server = null;
			if ( !wasOffline || OfflineCount == 0 )
			{
				++OfflineCount;
				subject.OnNext ( Offline ( Port ) );
			}
		}

		public override string ToString ( ) =>
			$"Monitor Running: {IsRunning} Server: {Server}";
	}
}