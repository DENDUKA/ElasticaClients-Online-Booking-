using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;

namespace ElasticaClients.DAL.Data
{
	public class SubscriptionDAL
	{
		public static Subscription Get(int id)
		{
			Subscription sub;

			using (SubscriptionContext db = new SubscriptionContext())
			{
				sub = db.Subscriptions.Where(s => s.Id == id)
					.Include(x => x.TrainingItems.Select(y => y.Training.Trainer))
					.Include(x => x.Account)
					.Include(x => x.Branch)
					.Include(x => x.FreezeSubscriptionList)
					.FirstOrDefault();
			}

			return sub;
		}

		public static List<Subscription> GetAllWithStatus(int subStatus)
		{
			using (SubscriptionContext db = new SubscriptionContext())
			{
				return db.Subscriptions
					.Where(x => x.StatusId == subStatus)
					.Include(x => x.Account)
					.Include(x => x.Branch)
					.Include(x => x.FreezeSubscriptionList)
					.Include(x => x.TrainingItems)
					.ToList();
			}
		}

		public static List<Subscription> GetFreezeExpired()
		{
			var now = DateTime.Now.Date;

			IQueryable<Subscription> freezedSubs;
			List<Subscription> subsFreezeExpired = new List<Subscription>();

			using (SubscriptionContext db = new SubscriptionContext())
			{
				freezedSubs = db.Subscriptions
					.Where(x => x.StatusId == (int)SubscriptionStatus.Freezed)
					.Include(x => x.FreezeSubscriptionList);

				foreach (var sub in freezedSubs)
				{
					var freeze = sub.FreezeSubscriptionList.First(x => x.Id == sub.FreezeSubscriptionList.Max(y => y.Id));

					if (freeze.Start <= DateTime.Today && freeze.End <= DateTime.Today)
					{
						subsFreezeExpired.Add(sub);
					}
				}
			}

			return subsFreezeExpired;
		}

		public static void Delete(int id)
		{
			using (SubscriptionContext db = new SubscriptionContext())
			{
				Subscription sub = new Subscription() { Id = id };
				db.Subscriptions.Attach(sub);
				db.Subscriptions.Remove(sub);
				db.SaveChanges();
			}
		}

		public static void Update(Subscription sub)
		{
			var oldSub = Get(sub.Id);

			using (SubscriptionContext db = new SubscriptionContext())
			{
				db.Subscriptions.Attach(sub);

				db.Entry(sub).State = EntityState.Modified;

				if (!(sub.StatusId == (int)SubscriptionStatus.Activated && oldSub.StatusId == (int)SubscriptionStatus.NotActivated))
				{
					db.Entry(sub).Property(x => x.ActivateDate).IsModified = false;
				}

				if (sub.StatusId == (int)SubscriptionStatus.Activated)
				{
					db.Entry(sub).Property(x => x.ActivateDate).IsModified = true;
				}					

				db.SaveChanges();
			}
		}

		public static List<Subscription> GetAll(DateTime? start = null, DateTime? end = null)
		{
			start = start is null ? DateTime.MinValue : start;
			end = end is null ? DateTime.MaxValue : end;

			using (SubscriptionContext db = new SubscriptionContext())
			{
				return db.Subscriptions
					.Where(x => x.BuyDate >= start && x.BuyDate <= end)
					.Include(x => x.Account)
					.Include(x => x.Branch)
					.Include(x => x.FreezeSubscriptionList)
					.ToList();
			}
		}

		public static List<Subscription> GetForBranch(int branchId, bool includeRazovoe = false, DateTime? start = null, DateTime? end = null)
		{
			start = start is null ? DateTime.MinValue : start;
			end = end is null ? DateTime.MaxValue : end;

			using (SubscriptionContext db = new SubscriptionContext())
			{
				var res = db.Subscriptions
					.Where(x => x.BranchId == branchId)
					.Where(x => x.BuyDate >= start && x.BuyDate <= end)
					.Include(x => x.Account)
					.Include(x => x.Branch)
					.Include(x => x.FreezeSubscriptionList);

				if (!includeRazovoe)
				{
					res = res.Where(x => x.StatusId != (int)SubscriptionStatus.Razovoe);
				}

				return res.ToList();
			}
		}

		public static List<Subscription> GetForAccount(int accId, int branchId = 0, bool includeRazovoe = false)
		{
			using (SubscriptionContext db = new SubscriptionContext())
			{
				var subs = db.Subscriptions
					.Where(x => x.AccountId == accId)
					.Include(x => x.FreezeSubscriptionList);

				if (branchId != 0)
				{
					subs = subs.Where(x => x.BranchId == branchId);
				}

				if (!includeRazovoe)
				{
					subs = subs.Where(x => x.StatusId != (int)SubscriptionStatus.Razovoe);
				}

				return subs.ToList();
			}
		}

		public static void Add(Subscription sub)
		{
			using (SubscriptionContext db = new SubscriptionContext())
			{
				db.Subscriptions.Add(sub);
				db.SaveChanges();
			}
		}

		public static void AddFreeze(FreezeSubscriptionItem freeze)
		{
			using (FreezeSubscriptionItemContext db = new FreezeSubscriptionItemContext())
			{
				db.FreezeSubscriptionList.Add(freeze);
				db.SaveChanges();
			}
		}

		public static void UpdateFreeze(FreezeSubscriptionItem freeze)
		{
			using (FreezeSubscriptionItemContext db = new FreezeSubscriptionItemContext())
			{
				db.FreezeSubscriptionList.Attach(freeze);
				db.Entry(freeze).State = EntityState.Modified;
				db.SaveChanges();
			}
		}

			public static void DeleteFreeze(int id)
		{
			using (FreezeSubscriptionItemContext db = new FreezeSubscriptionItemContext())
			{
				FreezeSubscriptionItem freeze = new FreezeSubscriptionItem() { Id = id };
				db.FreezeSubscriptionList.Attach(freeze);
				db.FreezeSubscriptionList.Remove(freeze);
				db.SaveChanges();
			}
		}
	}
}