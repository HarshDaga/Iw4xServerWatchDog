using System;
using Iw4xServerWatchDog.Monitor.Converters;
using Newtonsoft.Json;

// ReSharper disable StringLiteralTypo

namespace Iw4xServerWatchDog.Monitor.Types
{
	public class Status : IEquatable<Status>
	{
		public bool Equals ( Status other )
		{
			if ( other is null ) return false;
			if ( ReferenceEquals ( this, other ) ) return true;

			return Equals ( GameType, other.GameType ) &&
			       GHardcore == other.GHardcore &&
			       Equals ( GameName, other.GameName ) &&
			       IsPrivate == other.IsPrivate &&
			       Equals ( MapName, other.MapName ) &&
			       MatchType == other.MatchType && Protocol == other.Protocol &&
			       Equals ( ShortVersion, other.ShortVersion ) &&
			       SvAllowAnonymous == other.SvAllowAnonymous &&
			       SvAllowClientConsole == other.SvAllowClientConsole &&
			       SvFloodProtect == other.SvFloodProtect &&
			       Equals ( SvHostname, other.SvHostname ) &&
			       SvMaxPing == other.SvMaxPing &&
			       SvMaxRate == other.SvMaxRate &&
			       SvMaxClients == other.SvMaxClients &&
			       SvMinPing == other.SvMinPing &&
			       SvPrivateClients == other.SvPrivateClients &&
			       SvPrivateClientsForClients == other.SvPrivateClientsForClients &&
			       SvPure == other.SvPure &&
			       SvSecurityLevel == other.SvSecurityLevel;
		}

		private static bool Equals ( string a, string b ) =>
			string.Equals ( a, b, StringComparison.OrdinalIgnoreCase );

		public override bool Equals ( object obj )
		{
			if ( obj is null ) return false;
			if ( ReferenceEquals ( this, obj ) ) return true;
			if ( obj is Status status ) return Equals ( status );
			return false;
		}

		public override int GetHashCode ( ) => 0;

		public override string ToString ( ) => SvHostname;

		#region JSON properties

		[JsonProperty ( "checksum" )]
		public string CheckSum { get; set; }

		[JsonProperty ( "g_gametype" )]
		public string GameType { get; set; }

		[JsonProperty ( "g_hardcore" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long GHardcore { get; set; }

		[JsonProperty ( "gamename" )]
		public string GameName { get; set; }

		[JsonProperty ( "isPrivate" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long IsPrivate { get; set; }

		[JsonProperty ( "mapname" )]
		public string MapName { get; set; }

		[JsonProperty ( "matchtype" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long MatchType { get; set; }

		[JsonProperty ( "protocol" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long Protocol { get; set; }

		[JsonProperty ( "shortversion" )]
		public string ShortVersion { get; set; }

		[JsonProperty ( "sv_allowAnonymous" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvAllowAnonymous { get; set; }

		[JsonProperty ( "sv_allowClientConsole" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvAllowClientConsole { get; set; }

		[JsonProperty ( "sv_floodProtect" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvFloodProtect { get; set; }

		[JsonProperty ( "sv_hostname" )]
		public string SvHostname { get; set; }

		[JsonProperty ( "sv_maxPing" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvMaxPing { get; set; }

		[JsonProperty ( "sv_maxRate" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvMaxRate { get; set; }

		[JsonProperty ( "sv_maxclients" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvMaxClients { get; set; }

		[JsonProperty ( "sv_minPing" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvMinPing { get; set; }

		[JsonProperty ( "sv_privateClients" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvPrivateClients { get; set; }

		[JsonProperty ( "sv_privateClientsForClients" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvPrivateClientsForClients { get; set; }

		[JsonProperty ( "sv_pure" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvPure { get; set; }

		[JsonProperty ( "sv_securityLevel" )]
		[JsonConverter ( typeof (ParseStringConverter) )]
		public long SvSecurityLevel { get; set; }

		#endregion
	}
}