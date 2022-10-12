using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Bot
{
	internal static class Handles
	{
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;

                if (message.Text is null)
				{
                    return;
				}

                switch (message.Text.ToLower())
                {
                    case "/start":
                        {
                            Answers.Start(message.Chat.Id);

                            break;
                        }
                    default:
                        {
                            Answers.NextStage(message.Chat.Id, message);
                            break;
                        }
                }

                //if (message.Text.ToLower() == "/start")
                //{






                //    var h = new KeyboardButton("hi");
                //    var b = new KeyboardButton("bay");

                //    var rkm1 = new ReplyKeyboardMarkup(new KeyboardButton[] { h, b });

                //    //await botClient.SendTextMessageAsync(message.Chat.Id, "Accept Or Reject..", replyMarkup: rkm1);



                //    //await botClient.(message.Chat, "Добро пожаловать на борт, добрый путник!");


                //    //await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                //    return;
                //}
               // await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");



                if (message.Text.ToLower() == "hi")
                {
                    var h = new KeyboardButton("QQQ");
                    var b = new KeyboardButton("WWW");

                    var rkm1 = new ReplyKeyboardMarkup(new KeyboardButton[] { h, b });

                    await botClient.SendTextMessageAsync(message.Chat.Id, "GOGOG?", replyMarkup: rkm1);

                    return;
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}
