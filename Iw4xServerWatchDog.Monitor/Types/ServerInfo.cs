using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;

namespace Iw4xServerWatchDog.Monitor.Types
{
	public class ServerInfo : IEquatable<ServerInfo>
	{
		[JsonProperty ( "players" )]
		public List<Player> Players { get; set; }

		[JsonProperty ( "status" )]
		public Status Status { get; set; }

		[JsonProperty ( "port" )]

		public int Port { get; set; }

		public bool Equals ( ServerInfo other )
		{
			if ( other is null ) return false;
			if ( ReferenceEquals ( this, other ) ) return true;
			return Players.SequenceEqual ( other.Players ) &&
			       Equals ( Status, other.Status ) &&
			       Port == other.Port;
		}

		public override bool Equals ( object obj )
		{
			if ( obj is null ) return false;
			if ( ReferenceEquals ( this, obj ) ) return true;
			if ( obj is ServerInfo info ) return Equals ( info );
			return false;
		}

		[SuppressMessage ( "ReSharper", "NonReadonlyMemberInGetHashCode" )]
		public override int GetHashCode ( )
		{
			unchecked
			{
				var hashCode = Port;
				hashCode = ( hashCode * 397 ) ^ ( Status != null ? Status.GetHashCode ( ) : 0 );
				return hashCode;
			}
		}

		public override string ToString ( ) =>
			$"{Status} : {Players.Count} players";
	}
}