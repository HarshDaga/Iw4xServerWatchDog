using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Iw4xServerWatchDog.Monitor.Types;
using static Iw4xServerWatchDog.Monitor.ServerStatusChangedEventArgs;

namespace Iw4xServerWatchDog.Monitor
{
	public class ServerMonitor :
		IObservable<ServerStatusChangedEventArgs>
	{
		public int Port { get; }
		public bool IsRunning { get; private set; }
		public ServerInfo Server { get; private set; }
		public string Url { get; }
		public TimeSpan PollingFrequency { get; set; } = TimeSpan.FromSeconds ( 1 );
		private readonly Subject<ServerStatusChangedEventArgs> subject;

		private CancellationTokenSource cts;

		public ServerMonitor ( int port, string baseUrl = @"http:\\localhost" )
		{
			Port    = port;
			Url     = $@"{baseUrl}:{Port}/info";
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
					Server      = await Url.GetJsonAsync<ServerInfo> ( cts.Token );
					Server.Port = Port;

					if ( old is null )
						subject.OnNext ( Online ( Server ) );
					else if ( !Server.Equals ( old ) )
						subject.OnNext ( Updated ( Server, old ) );
				}
				catch ( Exception )
				{
					var wasOffline = Server is null;
					Server = null;
					if ( !wasOffline )
						subject.OnNext ( Offline ( Port ) );
				}

				await Task.Delay ( PollingFrequency, cts.Token );
			}
		}
	}
}