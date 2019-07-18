using System;
using Newtonsoft.Json;

namespace Iw4xServerWatchDog.Monitor.Types
{
	public class Player : IEquatable<Player>
	{
		[JsonProperty ( "name" )]
		public string Name { get; set; }

		[JsonProperty ( "ping" )]
		public long Ping { get; set; }

		[JsonProperty ( "score" )]
		public long Score { get; set; }

		public bool Equals ( Player other )
		{
			if ( other is null ) return false;
			if ( ReferenceEquals ( this, other ) ) return true;
			return string.Equals ( Name, other.Name ) &&
			       Ping == other.Ping &&
			       Score == other.Score;
		}

		public override bool Equals ( object obj )
		{
			if ( obj is null ) return false;
			if ( ReferenceEquals ( this, obj ) ) return true;
			if ( obj is Player player ) return Equals ( player );
			return false;
		}

		public override int GetHashCode ( ) => 0;
	}
}