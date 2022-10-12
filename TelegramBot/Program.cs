using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramBot
{
	internal class Program
	{
        static void Main(string[] args)
		{
            Console.WriteLine("Запущен бот " + Shared.Bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            Shared.Bot.StartReceiving(
                Bot.Handles.HandleUpdateAsync,
                Bot.Handles.HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            Shared.Bot.SendTextMessageAsync(-737795694, "Hi");

            

            Console.ReadLine();
        }
	}
}
