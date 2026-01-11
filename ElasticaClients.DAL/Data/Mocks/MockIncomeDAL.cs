using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.DAL.Data.Mocks
{
    public class MockIncomeDAL : IIncomeDAL
    {
        private static List<Income> _incomes = new List<Income>();

        public void Add(Income income)
        {
            if (_incomes.Count > 0)
                income.Id = _incomes.Max(i => i.Id) + 1;
            else
                income.Id = 1;

            _incomes.Add(income);
        }

        public void Delete(int id)
        {
            var income = Get(id);
            if (income != null)
                _incomes.Remove(income);
        }

        public Income Get(int id)
        {
            return _incomes.FirstOrDefault(i => i.Id == id);
        }

        public List<Income> GetAll(int branchId, DateTime start, DateTime end)
        {
            return _incomes
                .Where(i => i.BranchId == branchId && i.Date >= start && i.Date <= end)
                .ToList();
        }

        public void Update(Income income)
        {
            var existing = Get(income.Id);
            if (existing != null)
            {
                existing.Name = income.Name;
                existing.Cost = income.Cost;
                existing.BranchId = income.BranchId;
                existing.Date = income.Date;
                existing.Type = income.Type;
            }
        }
    }
}
