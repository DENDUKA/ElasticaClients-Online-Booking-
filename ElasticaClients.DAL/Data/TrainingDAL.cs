using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using Z.EntityFramework.Plus;

namespace ElasticaClients.DAL.Data
{
	public static class TrainingDAL
	{
		static Stopwatch sw = new Stopwatch();

		public static List<Training> GetAllForGym(int gymId)
		{
			using (TrainingContext db = new TrainingContext())
			{
				return db.Trainings.Where(t => t.GymId == gymId)
					.Include(x => x.Trainer)
					//.Include(x => x.TrainingItems)
					.OrderBy(x => x.StartTime)
					.ToList();
			}
		}

		public static List<Training> GetAllForGym(int gymId, DateTime start, DateTime end)
		{
			sw.Restart();

			Debug.WriteLine($"GetAllForGym(int gymId, DateTime start, DateTime end) Start  {start}  {end}");
			List<Training> res = null;

			using (TrainingContext db = new TrainingContext())
			{
				res = db.Trainings.Where(t => t.GymId == gymId && t.StartTime >= start && t.StartTime <= end)
					.Include(x => x.Trainer)
					.ToList();
			}
			sw.Stop();
			Debug.WriteLine($"GetAllForGym(int gymId, DateTime start, DateTime end) {sw.ElapsedMilliseconds}");

			return res;
		}

		public static List<Training> GetAllForTrainer(int trainerId, DateTime start, DateTime end)
		{
			sw.Restart();

			Debug.WriteLine($"GetAllForTrainer(int trainerId, DateTime start, DateTime end) Start  {start}  {end}");

			List<Training> res = null;

			using (TrainingContext db = new TrainingContext())
			{
				res = db.Trainings.Where(t => t.TrainerId == trainerId && t.StartTime >= start && t.StartTime <= end)
					.Include(x => x.Trainer)
					.ToList();
			}

			sw.Stop();

			Debug.WriteLine($"GetAllForGym(int gymId, DateTime start, DateTime end) {sw.ElapsedMilliseconds}");

			return res;
		}

		public static void Update(Training t)
		{
			using (TrainingContext db = new TrainingContext())
			{
				db.Trainings.AddOrUpdate(t);
				db.SaveChanges();
			}
		}

		public static int Add(Training training)
		{
			using (TrainingContext db = new TrainingContext())
			{
				db.Trainings.Add(training);
				db.SaveChanges();
			}

			return training.Id;
		}

		public static Training Get(int id)
		{
			using (TrainingContext db = new TrainingContext())
			{
				return db.Trainings.Where(t => t.Id == id)
					.Include(x => x.Gym)
					.Include(x => x.Gym.Branch)
					.Include(x => x.Trainer)
					.Include(x => x.TrainingItems)
					.Include(x => x.TrainingItems.Select(y => y.Account))
					.FirstOrDefault();
			}
		}
		public static Training GetBase(int id)
		{
			using (TrainingContext db = new TrainingContext())
			{
				return db.Trainings.Where(t => t.Id == id)
					.FirstOrDefault();
			}
		}

		public static void Delete(int id)
		{
			using (TrainingContext db = new TrainingContext())
			{
				db.Trainings.Where(t => t.Id == id).Delete();
			}
		}

		public static Training GetByYclintsId(int id)
		{
			using (TrainingContext db = new TrainingContext())
			{
				return db.Trainings.Where(t => t.YClientId == id)
					.Include(x => x.Gym)
					.Include(x => x.Trainer)
					.Include(x => x.TrainingItems)
					.Include(x => x.TrainingItems.Select(y => y.Account))
					.FirstOrDefault();
			}
		}

		public static Training GetIndivid(DateTime date, string yclientId)
		{
			using (TrainingContext db = new TrainingContext())
			{
				return db.Trainings.Where(t => t.StartTime == date && t.YClientId == 0 && t.TrainingItems.Count(x => x.YClientsId.ToString() == yclientId) > 0)
					.Include(x => x.Gym)
					.Include(x => x.Trainer)
					.Include(x => x.TrainingItems)
					.Include(x => x.TrainingItems.Select(y => y.Account))
					.FirstOrDefault();
			}
		}
	}
}