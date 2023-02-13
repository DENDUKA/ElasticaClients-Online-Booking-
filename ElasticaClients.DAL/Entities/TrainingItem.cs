using ElasticaClients.DAL.Accessory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ElasticaClients.DAL.Entities
{
	public class TrainingItem : IIncome
	{
		public int Id { get; set; }
		public int YClientsId { get; set; }
		[ForeignKey("Subscription")]
		public int SubscriptionId { get; set; }
		public Subscription Subscription { get; set; }
		[ForeignKey("Training")]
		public int TrainingId { get; set; }
		public Training Training { get; set; }
		[ForeignKey("Account")]
		public int AccountId { get; set; }
		public Account Account { get; set; }
		public int StatusId { get; set; }
		public string StatusName
		{
			get
			{
				return StatusNameDictionary[(TrainingItemStatus)StatusId];
			}
		}
		public int StatusPayId { get; set; }
		public bool Razovoe { get; set; }
		public int RazovoeCost { get; set; }
		public bool IsTrial { get; set; }

		public bool IsLast
		{
			get
			{
				if (Razovoe)
				{
					return false;
				}

				Subscription sub = Data.SubscriptionDAL.Get(SubscriptionId);
				if (sub.TrainingsLeft == 0)
				{
					var lastTI = sub.TrainingItems.OrderBy(x => x.Training.StartTime).Last();

					return lastTI.Id == Id;
				}

				return false;
			}
		}

		public int Cost => RazovoeCost;

		public DateTime Date => Training.StartTime;

		public int IncomeId => 0;

		public string IncomeName => "Разовое";

		public IncomeType Type => IncomeType.Razovoe;

		public static Dictionary<TrainingItemStatus, string> StatusNameDictionary = new Dictionary<TrainingItemStatus, string>()
		{
			{ TrainingItemStatus.canceledByAdmin, "Отменен админом" },
			{ TrainingItemStatus.no , "Не пришел" },
			{ TrainingItemStatus.unKnown , "---" },
			{ TrainingItemStatus.yes , "Пришел" },
		};

		public static void Delete(int id)
		{
			throw new NotImplementedException();
		}
    }
}