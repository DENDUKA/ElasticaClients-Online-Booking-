using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ElasticaClients.Logic
{
	public static class SubscriptionB
	{
		private static Stopwatch sw = new Stopwatch();

		public static void Add(Subscription sub)
		{
			SubscriptionDAL.Add(sub);

			RecalculateValues(sub.Id);
		}

		public static void BatchUnfreeze()
		{
			var freezeExpiredSubs = SubscriptionDAL.GetFreezeExpired();

			freezeExpiredSubs.ForEach(x => UnFreeze(x.Id));
		}

		public static void BatchActivateByTime()
		{
			var subs = SubscriptionDAL.GetAllWithStatus((int)SubscriptionStatus.NotActivated);

			var ss = subs.Where(x => x.BuyDate.AddDays(14) <= DateTime.Today).ToList();

			foreach (var sub in ss)
			{
				//активация через 14 дней после покупки
				if (sub.BuyDate.AddDays(14) <= DateTime.Today)
				{
					SubscriptionB.Activate(sub.Id, sub.BuyDate.AddDays(14));
				}
			}
		}

		public static void BatchCloseSubscription()
		{
			var subs = SubscriptionDAL.GetAllWithStatus((int)SubscriptionStatus.Activated);

			var byTyme = subs.Where(x => x.DaysLeft < 0).ToList();
			//var byTrainingZero = 

			foreach (var sub in subs)
			{
				CloseByTimeLeft(sub);
				CloseByTrainingsLeft(sub);
			}
		}

		private static void CloseByTimeLeft(Subscription sub)
		{
			if (sub.StatusId == (int)SubscriptionStatus.Activated && sub.DaysLeft < 0)
			{
				sub.StatusId = (int)SubscriptionStatus.Closed;
				SubscriptionB.Update(sub);
				Debug.WriteLine($"Закрыт по времени {sub.Id}");
			}
		}

		public static void CloseByTrainingsLeft(Subscription sub)
		{
			if (sub.StatusId == (int)SubscriptionStatus.Activated)
			{
				int actualTiCount = sub.TrainingItems.Count(x => x.StatusId == (int)TrainingItemStatus.yes || x.StatusId == (int)TrainingItemStatus.no);
				if (actualTiCount >= sub.TrainingCount)
				{
					sub.StatusId = (int)SubscriptionStatus.Closed;
					SubscriptionB.Update(sub);
					Debug.WriteLine($"Закрыт по окончанию тренировок {sub.Id} осталось {sub.TrainingCount - actualTiCount}");
				}
			}
		}

		public static bool Delete(int id)
		{
			var sub = SubscriptionB.Get(id);

			if (sub.TrainingItems.Count == 0)
			{
				SubscriptionDAL.Delete(id);
				return true;
			}

			return false;
		}

		//public static void Activate(int id, DateTime activateDate)
		//{
		//	SubscriptionDAL.Activate(id, activateDate);
  //          RecalculateValues(id);
  //      }

		public static void Update(Subscription sub)
		{
			SubscriptionDAL.Update(sub);
			RecalculateValues(sub.Id);
		}

		public static List<Subscription> GetForBranch(int branchId, bool includeRazovoe = false)
		{
			var subs = SubscriptionDAL.GetForBranch(branchId, includeRazovoe);
			return subs;
		}

		public static List<Subscription> GetForBranch(int branchId, DateTime start, DateTime end, bool includeRazovoe = false)
		{
			var subs = SubscriptionDAL.GetForBranch(branchId, includeRazovoe, start, end);
			return subs;
		}

		public static List<Subscription> GetAll()
		{
			return SubscriptionDAL.GetAll();
		}

		public static Subscription Get(int id)
		{
			return SubscriptionDAL.Get(id);
		}

		public static Subscription GetRazovoe(int accId, int branchId)
		{
			var subs = SubscriptionDAL.GetForAccount(accId, branchId, true);

			return subs.Where(x => x.StatusId == (int)SubscriptionStatus.Razovoe).First();
		}

		//branchId == 0 - для всех филиалов
		public static List<Subscription> GetForAccount(int accId, int branchId = 0)
		{
			var subs = SubscriptionDAL.GetForAccount(accId, branchId);

			return subs.Where(x => x.StatusId != (int)SubscriptionStatus.Razovoe).ToList();
		}

		public static void AddFreeze(FreezeSubscriptionItem freeze)
		{
			Subscription sub = SubscriptionDAL.Get(freeze.SubscriptionId);

			if (sub.StatusId == (int)SubscriptionStatus.Activated)
			{
				sub.ActiveDays += freeze.FreezeDays;
				sub.StatusId = (int)SubscriptionStatus.Freezed;

				SubscriptionDAL.Update(sub);
				SubscriptionDAL.AddFreeze(freeze);
			}
		}

		public static void UnFreeze(int subscriptionId)
		{
			Subscription sub = SubscriptionDAL.Get(subscriptionId);

			UnFreeze(sub);
		}

		public static void UnFreeze(Subscription sub)
		{
			if (sub.StatusId == (int)SubscriptionStatus.Freezed)
			{
				var freeze = sub.FreezeSubscriptionList.First(x => x.Id == sub.FreezeSubscriptionList.Max(y => y.Id));

				if (freeze.Start <= DateTime.Today && freeze.End >= DateTime.Today)
				{
					int daysDiff = (freeze.End - DateTime.Today).Days;

					freeze.End = DateTime.Today;

					sub.StatusId = (int)SubscriptionStatus.Activated;
					sub.ActiveDays -= daysDiff;

					SubscriptionDAL.Update(sub);
					SubscriptionDAL.UpdateFreeze(freeze);
				}

				if (freeze.Start <= DateTime.Today && freeze.End <= DateTime.Today)
				{
					sub.StatusId = (int)SubscriptionStatus.Activated;

					SubscriptionDAL.Update(sub);
					SubscriptionDAL.UpdateFreeze(freeze);
				}
			}
		}

		public static void RecalculateValues(int id)
		{
			sw.Restart();

			var sub = SubscriptionDAL.Get(id);

			if (sub is null)
			{
				return;
			}

			//только те, которые учитываются в абонементе
			var actualTrainingItems = sub.TrainingItems.Where(x =>
			x.StatusId == (int)TrainingItemStatus.yes ||
			x.StatusId == (int)TrainingItemStatus.unKnown ||
			x.StatusId == (int)TrainingItemStatus.no).ToList();

			sub.TrainingsLeft = sub.TrainingCount - actualTrainingItems.Count;

			if (sub.TrainingsLeft < 0)
			{
				sub.TrainingsLeft = 0;
			}

			// Проверка даты активации

			if (sub.StatusId == (int)SubscriptionStatus.Activated && actualTrainingItems.Count > 0)
			{
				var dateActivateByDate = sub.BuyDate.AddDays(14);
				var firstTrainingDate = actualTrainingItems.Min(x => x.Date.Date);


				sub.ActivateDate = dateActivateByDate < firstTrainingDate ? dateActivateByDate : firstTrainingDate;
			}

			SubscriptionDAL.Update(sub);

			sw.Stop();

			Debug.WriteLine($"SubscriptionB.RecalculateValues {sw.ElapsedMilliseconds}");
		}

		public static void Activate(int id, DateTime activateDate)
		{
			Subscription sub = Get(id);

			if (sub.StatusId == (int)SubscriptionStatus.NotActivated)
			{
				sub.ActivateDate = activateDate;				

                SubscriptionDAL.Activate(id, activateDate);
            }
		}
	}
}