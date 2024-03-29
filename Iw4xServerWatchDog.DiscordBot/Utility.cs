﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using NLog;

namespace Iw4xServerWatchDog.DiscordBot
{
	internal static class Utility
	{
		// ReSharper disable once UnusedMember.Local
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( );
		private static readonly Regex ColorsRegex = new Regex ( @"\^([0-9]|\:)" );

		public static readonly RequestOptions RetryRequestOptions;

		static Utility ( )
		{
			RetryRequestOptions           = RequestOptions.Default;
			RetryRequestOptions.RetryMode = RetryMode.RetryRatelimit | RetryMode.RetryTimeouts;
		}

		public static string RemoveColors ( string str ) =>
			ColorsRegex.Replace ( str, "" );

		public static Task DeleteMessagesAsync ( IAsyncEnumerable<IReadOnlyCollection<IMessage>> messages ) =>
			DeleteMessagesAsync ( messages, m => true );

		public static Task DeleteMessagesAsync (
			IAsyncEnumerable<IReadOnlyCollection<IMessage>> messages,
			Func<IMessage, bool> predicate
		) => DeleteMessagesAsync ( messages, predicate, TimeSpan.FromMilliseconds ( 100 ) );

		public static async Task DeleteMessagesAsync (
			IAsyncEnumerable<IReadOnlyCollection<IMessage>> messages,
			Func<IMessage, bool> predicate,
			TimeSpan cooldown
		)
		{
			await messages.ForEachAsync ( async collection =>
				{
					foreach ( var m in collection.Where ( predicate ) )
					{
						await m.TryDeleteAsync ( );
						await Task.Delay ( cooldown );
					}
				}
			);
		}
	}
}