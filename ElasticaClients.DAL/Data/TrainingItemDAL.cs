using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using Z.EntityFramework.Plus;

namespace ElasticaClients.DAL.Data
{
	public static class TrainingItemDAL
	{
		public static void Delete(int id)
		{
			using (TrainingItemContext db = new TrainingItemContext())
			{
				db.TrainingItems.Where(ti => ti.Id == id).Delete();
			}
		}

		public static TrainingItem Get(int id)
		{
			using (TrainingItemContext db = new TrainingItemContext())
			{
				return db.TrainingItems
					.Where(x => x.Id == id)
					.FirstOrDefault();
			}
		}

		public static void ChangeStatus(int tiid, TrainingItemStatus tiStatus)
		{
			TrainingItem ti = new TrainingItem
			{
				Id = tiid,
				StatusId = (int)tiStatus
			};

			using (TrainingItemContext db = new TrainingItemContext())
			{
				db.TrainingItems.Attach(ti);
				db.Entry(ti).Property(x => x.StatusId).IsModified = true;
				db.SaveChanges();
			}
		}

		public static void ChangePayStatus(int tiid, TrainingItemPayStatus tiPayStatus)
		{
			TrainingItem ti = new TrainingItem
			{
				Id = tiid,
				StatusPayId = (int)tiPayStatus
			};

			using (TrainingItemContext db = new TrainingItemContext())
			{
				db.TrainingItems.Attach(ti);
				db.Entry(ti).Property(x => x.StatusPayId).IsModified = true;
				db.SaveChanges();
			}
		}

		public static List<TrainingItem> GetAll()
		{
			using (TrainingItemContext db = new TrainingItemContext())
			{
				return db.TrainingItems
					.Include(x => x.Subscription)
					.ToList();
			}
		}

		public static List<TrainingItem> GetAllForBranch(int branchId, DateTime start, DateTime end, bool onlyRazovoe)
		{
			var tiList = new List<TrainingItem>();
			
			using (TrainingItemContext db = new TrainingItemContext())
			{
				var result = db.TrainingItems
					.Where(x => x.Subscription.BranchId == branchId);

				if (onlyRazovoe)
				{
					result = result.Where(x => x.Razovoe);
				}

				result = result
					.Where(x => x.Training.StartTime >= start && x.Training.StartTime <= end)
					.Include(x=>x.Training);

				tiList = result.ToList();
			}

			return tiList;
		}

		public static void Update(TrainingItem ti)
		{
			using (TrainingItemContext db = new TrainingItemContext())
			{
				db.TrainingItems.AddOrUpdate(ti);
				db.SaveChanges();
			}
		}

		public static void Add(TrainingItem ti)
		{
			using (TrainingItemContext db = new TrainingItemContext())
			{
				db.TrainingItems.Add(ti);
				db.SaveChanges();
			}
		}

		public static TrainingItem GetByYClients(int id)
		{
			using (TrainingItemContext db = new TrainingItemContext())
			{
				return db.TrainingItems
					.Where(x => x.YClientsId == id)
					.FirstOrDefault();
			}
		}

		public static List<TrainingItem> GetAllForAccount(int accountId, DateTime? start = null, DateTime? end = null)
		{
			DateTime StartDate = start is null ? DateTime.Today.AddMonths(-200) : (DateTime)start;
			DateTime EndDate = end is null ? DateTime.Today.AddMonths(200) : (DateTime)end;

			using (TrainingItemContext db = new TrainingItemContext())
			{
				return db.TrainingItems
					.Where(x => x.Training.StartTime > StartDate && x.Training.StartTime < EndDate)
					.Where(x => x.AccountId == accountId)
					.Include(x => x.Training.Gym.Branch)
					.ToList();
			}
		}
	}
}