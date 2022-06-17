using ElasticaClients.DAL.Accessory;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElasticaClients.Entities
{
	public class AccountStat
	{
		public AccountStat(int accId)
		{
			try
			{
				var acc = AccountB.GetLite(accId);

				SubscriptionCount = acc.Subscriptions.Count(x => x.StatusId != (int)SubscriptionStatus.Razovoe);
				TrainingCount = acc.TrainingItems.Count(x => x.StatusId == (int)TrainingItemStatus.yes);
				IsBeenOnTraining = TrainingCount > 0;
				if (TrainingCount != 0)
				{
					var LastTraining = acc.TrainingItems
						.Where(x => x.StatusId == (int)TrainingItemStatus.yes)
						.OrderBy(x => x.Training.StartTime)
						.FirstOrDefault();

					if (LastTraining != null)
					{
						LastTrainingDate = LastTraining.Training.StartTime;
					}
				}

				IsActiveSubscription = acc.Subscriptions.Any(x => x.StatusId == (int)SubscriptionStatus.Activated);
				if (!IsActiveSubscription)
				{
					IsUnActiveSubscription = acc.Subscriptions.Any(x => x.StatusId == (int)SubscriptionStatus.NotActivated);
					if (!IsUnActiveSubscription)
					{
						var lastSubscription = acc.Subscriptions
							.Where(x => x.StatusId != (int)SubscriptionStatus.Razovoe)
							.OrderBy(x => ((DateTime)x.ActivateDate).AddDays(x.ActiveDays)).FirstOrDefault();

						if (lastSubscription != null)
						{
							DaysAfterLastSubscription = (DateTime.Today - ((DateTime)lastSubscription.ActivateDate).AddDays(lastSubscription.ActiveDays)).Days;
						}
					}
				}

			}
			catch (Exception ex)
			{
				
			}
		}

		public bool IsBeenOnTraining { get; private set; }
		public int SubscriptionCount { get; private set; }
		public int TrainingCount { get; private set; }
		public DateTime LastTrainingDate { get; private set; }
		public bool IsActiveSubscription {get; private set;}
		public bool IsUnActiveSubscription { get; private set; }
		public int DaysAfterLastSubscription { get; private set; }
	}
}