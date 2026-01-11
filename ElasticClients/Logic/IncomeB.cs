using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Logic
{
    public class IncomeB
    {
        private readonly IIncomeDAL _incomeDAL;

        public IncomeB(IIncomeDAL incomeDAL)
        {
            _incomeDAL = incomeDAL;
        }

        public Income Get(int id)
        {
            return _incomeDAL.Get(id);
        }

        public void Delete(int id)
        {
            _incomeDAL.Delete(id);
        }

        public void Add(Income income)
        {
            _incomeDAL.Add(income);
        }

        public void Update(Income income)
        {
            _incomeDAL.Update(income);
        }

        public List<Income> GetAll(int gymId, DateTime start, DateTime end)
        {
            return _incomeDAL.GetAll(gymId, start, end);
        }

        public IEnumerable<SelectListItem> ToSelectListItems(int selectedId = 0)
        {
            var typesList = Income.IncomeTypeNameDictionary.Values.ToList();

            List<SelectListItem> res = new List<SelectListItem>
            {
                new SelectListItem() { Text = "", Value = "" }
            };

            foreach (var x in Income.IncomeTypeNameDictionary)
            {
                res.Add(new SelectListItem() { Text = x.Value, Value = ((int)x.Key).ToString() });
            }

            return res;
        }
    }
}