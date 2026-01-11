using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Z.EntityFramework.Plus;

namespace ElasticaClients.DAL.Data.Interfaces
{
    public class IncomeDAL : IIncomeDAL
    {
        public List<Income> GetAll(int branchId, DateTime start, DateTime end)
        {
            using (IncomeContext db = new IncomeContext())
            {
                return db.Incomes
                    .Where(x => x.BranchId == branchId && x.Date >= start && x.Date <= end)
                    .ToList();
            }
        }

        public void Update(Income income)
        {
            using (IncomeContext db = new IncomeContext())
            {
                db.Incomes.AddOrUpdate(income);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (IncomeContext db = new IncomeContext())
            {
                db.Incomes
                    .Where(x => x.Id == id)
                    .Delete();
            }
        }

        public Income Get(int id)
        {
            using (IncomeContext db = new IncomeContext())
            {
                return db.Incomes
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
            }
        }

        public void Add(Income income)
        {
            using (IncomeContext db = new IncomeContext())
            {
                db.Incomes.Add(income);
                db.SaveChanges();
            }
        }
    }
}