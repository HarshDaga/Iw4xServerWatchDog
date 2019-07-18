using System;
using System.Threading;
using Iw4xServerWatchDog.Configs;
using PortableJsonConfig;

namespace Iw4xServerWatchDog
{
	public class Program
	{
		private static readonly ManualResetEvent Mre = new ManualResetEvent ( false );

		public static void Main ( )
		{
			Console.Title = "IW4X Server Watch Dog";

			var settings = ConfigManager<Settings>.Instance;

			var watchDog = new WatchDog ( settings );
			watchDog.Init ( );
			watchDog.Start ( );

			Console.CancelKeyPress += ( _, args ) => Mre.Set ( );
			Mre.WaitOne ( );
		}
	}
}