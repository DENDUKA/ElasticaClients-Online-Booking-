using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Logic
{
	public static class IncomeB
	{
		public static Income Get(int id)
		{
			return IncomeDAL.Get(id);
		}

		public static void Delete(int id)
		{
			IncomeDAL.Delete(id);
		}

		public static void Add(Income income)
		{
			IncomeDAL.Add(income);
		}

		public static void Update(Income income)
		{
			IncomeDAL.Update(income);
		}

		public static List<Income> GetAll(int gymId, DateTime start, DateTime end)
		{
			return IncomeDAL.GetAll(gymId, start, end);
		}

		public static IEnumerable<SelectListItem> ToSelectListItems(int selectedId = 0)
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