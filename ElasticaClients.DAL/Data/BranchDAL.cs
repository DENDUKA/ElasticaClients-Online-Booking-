using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticaClients.DAL.Data
{
	public static class BranchDAL
	{
		public static List<Branch> GetAll()
		{
			using (BranchContext db = new BranchContext())
			{
				return db.Branches.ToList();
			}
		}

		public static Branch Get(int id)
		{
			using (BranchContext db = new BranchContext())
			{
				return db.Branches.Where(g => g.Id == id)
					   .Include(g => g.Gyms)
					   .First();
			}
		}

		public static async Task<List<Branch>> GetAllAsync()
		{
			using (BranchContext db = new BranchContext())
			{
				return await Task.Run(() => db.Branches.ToList());
			}
		}
	}
}