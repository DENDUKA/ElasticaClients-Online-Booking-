using ElasticaClients.DAL.Accessory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticaClients.DAL.Entities
{
    public class Subscription : IIncome
    {
        public int Id { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string Name { get; set; }
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        public int Cost { get; set; }
        public int ActiveDays { get; set; }
        public bool ByCash { get; set; }
        public int StatusId { get; set; }
        public string StatusName
        {
            get
            {
                return StatusNameDictionary[(SubscriptionStatus)StatusId];
            }
        }
        public DateTime BuyDate { get; set; }
        public DateTime? ActivateDate { get; set; }
        public List<TrainingItem> TrainingItems { get; set; }
        public List<FreezeSubscriptionItem> FreezeSubscriptionList { get; set; }
        public int TrainingCount { get; set; }
        public int TrainingsLeft { get; set; }
        public DateTime Date => BuyDate;

        public int IncomeId => 0;

        public DateTime DateEnd
        {
            get
            {
                if (ActivateDate != null)
                {
                    return ((DateTime)ActivateDate).AddDays(ActiveDays);
                }
                return default;
            }
        }

        public int? DaysLeft
        {
            get
            {
                if (ActivateDate != null)
                {
                    return (((DateTime)ActivateDate).AddDays(ActiveDays) - DateTime.Today).Days;
                }

                return null;
            }
        }

        public string IncomeName => $"Абонемент {TrainingCount} занятий";

        public IncomeType Type => IncomeType.Subscription;

        public static Dictionary<SubscriptionStatus, string> StatusNameDictionary = new Dictionary<SubscriptionStatus, string>()
        {
            { SubscriptionStatus.NotActivated , "Не активирован" },
            { SubscriptionStatus.Activated , "Активирован" },
            { SubscriptionStatus.Closed , "Завершен" },
            { SubscriptionStatus.Razovoe , "Разовое" },
            { SubscriptionStatus.Freezed , "Заморожен" },
        };

        public Subscription()
        {
            TrainingItems = new List<TrainingItem>();
            FreezeSubscriptionList = new List<FreezeSubscriptionItem>();
        }
    }
}