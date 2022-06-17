using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using System.Collections.Generic;

namespace ElasticaClients.DAL.Cache
{
	public static class AccountCache
	{
		private static bool lightIsFill = false;
		private static Dictionary<int, Account> lightAccounts = new Dictionary<int, Account>();

		public static Account GetLite(int id)
		{
			if (!lightIsFill)
			{
				lightAccounts.Clear();

				lightAccounts = AccountDAL.GetAllLight();

				lightIsFill = true;
			}

			if (lightAccounts.ContainsKey(id))
			{
				return lightAccounts[id];
			}

			return null;
		}

		public static void BonusesChanged(int id, int bonuses)
		{
			if (lightAccounts.ContainsKey(id))
			{
				lightAccounts[id].Bonuses = bonuses;
			}
		}

		public static void AccontChanged()
		{
			lightIsFill = false;
		}
	}
}
