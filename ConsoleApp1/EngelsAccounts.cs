using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public static class EngelsAccounts
	{
		public static void GetAllAccsInEngels()
		{
			List<Account> engAccs = new List<Account>();

			var accs = ElasticaClients.Logic.AccountB.GetAll();
			int gymId = 10;

			foreach (var acc in accs)
			{
				if (acc.RoleId == 9)
				{
					Account a = ElasticaClients.Logic.AccountB.Get(acc.Id, new DateTime(2021, 01, 01), new DateTime(2022, 04, 20));
					if (a.TrainingItems.Any(x => x.Training.GymId == gymId))
					{
						engAccs.Add(a);
					}
				}
			}

			StringBuilder ids = new StringBuilder();
			foreach (var acc in engAccs)
			{
				ids.Append(acc.Id);
				ids.Append(",");
			}

			string Ids = ids.ToString();

		}
	}
}