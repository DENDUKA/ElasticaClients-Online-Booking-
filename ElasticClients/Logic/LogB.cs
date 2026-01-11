using ElasticaClients.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ElasticaClients.Logic
{
    public class LogB
    {
        private static ITelegramBotClient bot = new TelegramBotClient("6093863103:AAEh29gaK9_NCIU1pTTPPaCrjY9IpeNeI-s");
        private static readonly ChatId chat = new ChatId(-794372155);
        private readonly IServiceProvider _serviceProvider;
        private readonly AccountB _accountB;

        public LogB(IServiceProvider serviceProvider, AccountB accountB)
        {
            _serviceProvider = serviceProvider;
            _accountB = accountB;
        }

        public void SendMessage()
        {
            bot.SendTextMessageAsync(chat, "Test 🎬");
        }

        public void NewTrainingItem(TrainingItem ti)
        {
            var trainingItemB = _serviceProvider.GetRequiredService<TrainingItemB>();
            ti = trainingItemB.Get(ti.Id);

            var acc = _accountB.Get(int.Parse(HttpContext.Current.User.Identity.Name));

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

        public void DeleteTrainingItem(TrainingItem ti)
        {
            var acc = _accountB.Get(int.Parse(HttpContext.Current.User.Identity.Name));

            bot.SendTextMessageAsync(chat, $"❌ {acc.Name} удалил {ti.Training} {ti.Account.Name}");
        }
    }
}