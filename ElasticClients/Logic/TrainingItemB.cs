using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.Logic
{
	public static class TrainingItemB
	{
		public static void ChangeStatus(int tiid, TrainingItemStatus tiStatus)
		{
			TrainingItemDAL.ChangeStatus(tiid, tiStatus);

			var ti = TrainingItemB.Get(tiid);

			SubscriptionB.RecalculateValues(ti.SubscriptionId);
			TrainingB.ReacalculateValues(ti.TrainingId);
			AccountB.ReacalculateBonuses(ti.AccountId);
		}

		public static void ChangePayStatus(int tiid, TrainingItemPayStatus tiPayStatus)
		{
			TrainingItemDAL.ChangePayStatus(tiid, tiPayStatus);
		}

		public static bool Add(TrainingItem trainingItem)
		{
			if (!IsAddValid(trainingItem))
			{
				return false;
			}

			if (trainingItem.Razovoe)
			{
				var training = TrainingB.Get(trainingItem.TrainingId);

				var razovoe = SubscriptionB.GetRazovoe(trainingItem.AccountId, training.Gym.BranchId);

				trainingItem.SubscriptionId = razovoe.Id;
				trainingItem.StatusPayId = (int)TrainingItemPayStatus.no;
			}
			else
			{
				IsFirstTrainingItemInSubscription(trainingItem);
			}

			trainingItem.StatusId = (int)TrainingItemStatus.unKnown;

			TrainingItemDAL.Add(trainingItem);

			TrainingB.ReacalculateValues(trainingItem.TrainingId);
			SubscriptionB.RecalculateValues(trainingItem.SubscriptionId);

			return true;
		}

		internal static List<TrainingItem> GetAllForBranch(int branchId, DateTime start, DateTime end, bool onlyRazovoe)
		{
			return TrainingItemDAL.GetAllForBranch(branchId, start, end, onlyRazovoe);
		}

		private static void IsFirstTrainingItemInSubscription(TrainingItem trainingItem)
		{
			var sub = SubscriptionB.Get(trainingItem.SubscriptionId);
			if (sub.StatusId == (int)SubscriptionStatus.NotActivated && sub.TrainingItems.Count == 0)
			{
				Training training = TrainingB.Get(trainingItem.TrainingId);
				sub.StatusId = (int)SubscriptionStatus.Activated;
				sub.ActivateDate = training.StartTime.Date;
				SubscriptionB.Update(sub);
			}
		}

		public static TrainingItem Get(int id)
		{
			return TrainingItemDAL.Get(id);
		}

		public static void Delete(int id)
		{
			var ti = TrainingItemB.Get(id);

			TrainingItemDAL.Delete(id);

			SubscriptionB.RecalculateValues(ti.SubscriptionId);
			TrainingB.ReacalculateValues(ti.TrainingId);
		}

		public static int GetRazovoeCost(int accountId, int branchId)
		{
			if (AccountB.IsFirstTime(accountId))
			{
				return 150;
			}

			return 400;
		}

		/// <summary>
		/// Добавление всех предстоящих разовых тренировок в абонемент на чиная с дня покупки абонемента
		/// </summary>
		/// <param name="id"></param>
		public static void AddRazovoesToSubscription(int accId, int subId)
		{
			Subscription sub = SubscriptionB.Get(subId);

			var tis = TrainingItemB.GetAllForAccount(accId, sub.BuyDate, sub.BuyDate.AddDays(20))
				.Where(x => x.Razovoe && x.StatusId != (int)TrainingItemStatus.canceledByAdmin);

			foreach (var ti in tis)
			{
				ti.Razovoe = false;
				ti.SubscriptionId = subId;
				TrainingItemDAL.Update(ti);
			}
		}

		public static List<TrainingItem> GetAllForAccount(int accountId)
		{
			var tis = TrainingItemDAL.GetAllForAccount(accountId);

			return tis is null ? new List<TrainingItem>() : tis;
		}

		public static List<TrainingItem> GetAllForAccount(int accountId, DateTime start, DateTime end)
		{
			var tis = TrainingItemDAL.GetAllForAccount(accountId, start, end);

			return tis is null ? new List<TrainingItem>() : tis;
		}

		private static bool IsAddValid(TrainingItem ti)
		{
			if (!ti.Razovoe)
			{
				Subscription sub = SubscriptionB.Get(ti.SubscriptionId);

				if (sub.TrainingsLeft <= 0)
				{
					return false;
				}
			}

			return true;
		}

		public static List<TrainingItem> GetFutureTrainings(int accountId)
		{
			return TrainingItemDAL.GetAllForAccount(accountId, DateTime.Today, DateTime.Today.Date.AddMonths(1)).OrderBy(x => x.Training.StartTime).Where(x => x.StatusId == (int)TrainingItemStatus.yes || x.StatusId == (int)TrainingItemStatus.unKnown).ToList();
		}
	}
}