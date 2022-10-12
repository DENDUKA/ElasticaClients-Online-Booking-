using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ElasticaClients.DAL.Data
{
	public static class GymDAL
	{
		public static List<Gym> GetAll()
		{
			using (GymContext db = new GymContext())
			{
				return db.Gyms.ToList();
			}
		}

		public static Gym Get(int id)
		{
			using (GymContext db = new GymContext())
			{
				return db.Gyms.Where(g => g.Id == id)
					   //.Include(g => g.Trainings)
					   .Include(g => g.Branch)
					   .First();
			}
		}

		public static Gym GetByName(int branchId, string name)
		{
			using (GymContext db = new GymContext())
			{
				return db.Gyms.Where(g => g.Name == name && g.BranchId == branchId)
					   .Include(g => g.Trainings)
					   .First();
			}
		}

		public static List<Gym> GetForBranch(int branchId)
		{
			using (GymContext db = new GymContext())
			{
				return db.Gyms.Where(g => g.BranchId == branchId)
					   .Include(g => g.Trainings)
					   .ToList();
			}
		}
	}
}