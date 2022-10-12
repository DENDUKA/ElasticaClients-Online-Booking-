using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Bot
{
	internal static class Logging
	{
		const int chatId = -737795694;

		public static void Log(string text)
		{
			Shared.Bot.SendTextMessageAsync(chatId, text);
		}
	}
}