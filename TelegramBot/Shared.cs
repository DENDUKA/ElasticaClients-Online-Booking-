using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot
{
	internal static class Shared
	{
		internal static ITelegramBotClient Bot = new TelegramBotClient("5419856813:AAFPyNiDReFzyDIXrvL41Ti8TG2u3tjzvgY");
		internal static Dictionary<long, State> State = new Dictionary<long, State>();
	}
}
