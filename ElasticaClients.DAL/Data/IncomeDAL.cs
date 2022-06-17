using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Z.EntityFramework.Plus;

namespace ElasticaClients.DAL.Data
{
	public static class IncomeDAL
	{
		public static List<Income> GetAll(int branchId, DateTime start, DateTime end)
		{
			using (IncomeContext db = new IncomeContext())
			{
				return db.Incomes
					.Where(x => x.BranchId == branchId && x.Date >= start && x.Date <= end)
					.ToList();
			}
		}

		public static void Update(Income income)
		{
			using (IncomeContext db = new IncomeContext())
			{
				db.Incomes.AddOrUpdate(income);
				db.SaveChanges();
			}
		}

		public static void Delete(int id)
		{
			using (IncomeContext db = new IncomeContext())
			{
				db.Incomes
					.Where(x => x.Id == id)
					.Delete();
			}
		}

		public static Income Get(int id)
		{
			using (IncomeContext db = new IncomeContext())
			{
				return db.Incomes
					.Where(x => x.Id == id)
					.FirstOrDefault();
			}
		}

		public static void Add(Income income)
		{
			using (IncomeContext db = new IncomeContext())
			{
				db.Incomes.Add(income);
				db.SaveChanges();
			}
		}
	}
}