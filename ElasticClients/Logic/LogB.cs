using ElasticaClients.DAL.Entities;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ElasticaClients.Logic
{
    public static class LogB
    {
        private static ITelegramBotClient bot = new TelegramBotClient("6093863103:AAEh29gaK9_NCIU1pTTPPaCrjY9IpeNeI-s");
        private static readonly ChatId chat = new ChatId(-794372155);

        public static void SendMessage()
        {
            bot.SendTextMessageAsync(chat, "Test 🎬");
        }

        public static void NewTrainingItem(TrainingItem ti)
        {
            ti = TrainingItemB.Get(ti.Id);

            var acc = AccountB.Get(int.Parse(HttpContext.Current.User.Identity.Name));

            string abOrRaz = ti.Razovoe ? $"Разовое ({ti.Cost})" : $"По Абонементу";

            if (ti.AccountId == acc.Id) //Сам записался
            {
                bot.SendTextMessageAsync(chat, $"🎬 {acc.Name} записался на тренировку {ti.Training} {abOrRaz}");
            }
            else
            {
                bot.SendTextMessageAsync(chat, $"👩‍💼 {acc.Name} записал на тренировку {ti.Account.Name} {ti.Training} {abOrRaz}");                
            }
        }

        public static void DeleteTrainingItem(TrainingItem ti)
        {
            var acc = AccountB.Get(int.Parse(HttpContext.Current.User.Identity.Name));

            bot.SendTextMessageAsync(chat, $"❌ {acc.Name} удалил {ti.Training} {ti.Account.Name}");
        }
    }
}