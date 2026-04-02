using ElasticaClients.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Web;

namespace ElasticaClients.Logic
{
    public class LogB
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AccountB _accountB;
        private readonly AppLogFileStore _logFileStore;

        public LogB(IServiceProvider serviceProvider, AccountB accountB, AppLogFileStore logFileStore)
        {
            _serviceProvider = serviceProvider;
            _accountB = accountB;
            _logFileStore = logFileStore;
        }

        public void NewTrainingItem(TrainingItem ti)
        {
            var trainingItemB = _serviceProvider.GetRequiredService<TrainingItemB>();
            ti = trainingItemB.Get(ti.Id);

            var acc = _accountB.Get(int.Parse(HttpContext.Current.User.Identity.Name));

            string abOrRaz = ti.Razovoe ? $"Разовое ({ti.Cost})" : "По Абонементу";

            AppLog log;

            if (ti.AccountId == acc.Id)
            {
                log = new AppLog
                {
                    CreatedAt = DateTime.Now,
                    ActionType = "Записался",
                    ActorName = acc.Name,
                    ClientName = null,
                    TrainingInfo = ti.Training.ToString(),
                    PaymentType = abOrRaz,
                    Cost = ti.Razovoe ? (int?)ti.Cost : null,
                };
            }
            else
            {
                log = new AppLog
                {
                    CreatedAt = DateTime.Now,
                    ActionType = "Записал",
                    ActorName = acc.Name,
                    ClientName = ti.Account.Name,
                    TrainingInfo = ti.Training.ToString(),
                    PaymentType = abOrRaz,
                    Cost = ti.Razovoe ? (int?)ti.Cost : null,
                };
            }

            _logFileStore.Add(log);
        }

        public void DeleteTrainingItem(TrainingItem ti)
        {
            var acc = _accountB.Get(int.Parse(HttpContext.Current.User.Identity.Name));

            _logFileStore.Add(new AppLog
            {
                CreatedAt = DateTime.Now,
                ActionType = "Удалил",
                ActorName = acc.Name,
                ClientName = ti.Account?.Name,
                TrainingInfo = ti.Training?.ToString(),
                PaymentType = null,
                Cost = null,
            });
        }
    }
}
