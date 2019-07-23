using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Discord;

namespace Iw4xServerWatchDog.DiscordBot
{
	internal static class DiscordExtensions
	{
		public static RequestOptions Retry => Utility.RetryRequestOptions;

		public static async Task TryDeleteAsync ( this IMessage message )
		{
			try
			{
				await message.DeleteAsync ( Retry );
			}
			catch ( Exception )
			{
				// ignored
			}
		}

		public static async Task<IUserMessage> TrySendEmbedAsync (
			this IMessageChannel channel,
			Embed embed
		)
		{
			try
			{
				return await channel.SendMessageAsync ( null, false, embed, Retry );
			}
			catch ( Exception )
			{
				Console.WriteLine ( "embed" );
				// ignored
			}

			return null;
		}

		[SuppressMessage ( "ReSharper", "InconsistentNaming" )]
		public static async Task<IUserMessage> TrySendMessageAsync (
			this IMessageChannel channel,
			string text = null,
			bool isTTS = false,
			Embed embed = null
		)
		{
			try
			{
				return await channel.SendMessageAsync ( text, isTTS, embed, Retry );
			}
			catch ( Exception )
			{
				Console.WriteLine ( "message" );
				// ignored
			}

			return null;
		}

		public static async Task TryModifyAsync (
			this IUserMessage message,
			Action<MessageProperties> func
		)
		{
			try
			{
				await message.ModifyAsync ( func, Retry );
			}
			catch ( Exception )
			{
				Console.WriteLine ( "modify" );
				// ignored
			}
		}
	}
}