using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Cache;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Logic
{
	public static class AccountB
	{
		public static IEnumerable<SelectListItem> ToSelectListItems(int selectedId = 0)
		{
			List<Account> accounts = AccountDAL.GetAll();

			List<SelectListItem> x = new List<SelectListItem>
			{
				new SelectListItem() { Text = "", Value = "" }
			};

			x.AddRange(accounts
					  .Select(g =>
						  new SelectListItem
						  {
							  Selected = (g.Id == selectedId),
							  Text = $"{g.Name} (+7{g.Phone})" ,
							  Value = g.Id.ToString()
						  })
						  .OrderBy(y => y.Text)
						  .ToList());

			return x;
		}

		public static List<Account> GetWithFilter(int page, int count, string filter)
		{
			return AccountDAL.GetWithFilter(page, count, filter);
		}

		public static List<Account> GetAll()
		{
			return AccountDAL.GetAll();
		}

		public static int GetSettingsBranchId()
		{
			int branchId = 0;
			Account acc = null;

			if (System.Web.HttpContext.Current != null &&
				System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
			{
				var accId = System.Web.HttpContext.Current.User.Identity.Name;

				acc = AccountDAL.GetLite(int.Parse(accId));
				if (acc != null)
				{
					branchId = acc.SettingsBranchId;
				}
			}

			if (branchId == 0)
			{
				branchId = BranchB.GetAll().First().Id;
				if (acc != null)
				{
					acc.SettingsBranchId = branchId;
					AccountDAL.Update(acc);
				}
			}

			return branchId;
		}

		public static Account GetTrainer(int id)
		{
			return AccountDAL.GetTrainer(id);
		}

		public static Account Get(int id, DateTime trainingsFrom, DateTime trainingsTo)
		{
			return AccountDAL.Get(id, trainingsFrom, trainingsTo);
		}

		public static bool IsFirstTime(int accountId)
		{
			var tis = TrainingItemB.GetAllForAccount(accountId);

			return !tis.Any(x => x.StatusId == (int)TrainingItemStatus.yes || x.StatusId == (int)TrainingItemStatus.unKnown || x.StatusId == (int)TrainingItemStatus.no);
		}

		public static Account Get(int id)
		{
			var acc = AccountDAL.Get(id);
			return AccountDAL.Get(id);
		}

		public static void Delete(int id)
		{
			AccountDAL.Delete(id);
		}

		public static void Add(Account account)
		{
			AccountDAL.Add(account); 
			CreateRazovoeSubs(account);
		}

		public static void Update(Account account)
		{
			AccountDAL.Update(account);
		}

		public static Account GetLite(int id)
		{
			return AccountDAL.GetLite(id);
		}

		public static Account GetCurrentUser()
		{
			if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
			{
				return GetLite(int.Parse(System.Web.HttpContext.Current.User.Identity.Name));
			}

			return null;
		}

		public static bool IsCurrentUserOwner()
		{
			return System.Web.HttpContext.Current.User.IsInRole(Role.owner);
		}

		internal static void BonusesOff(int accountId, int bonusesOff)
		{
			var acc = Get(accountId);
			acc.BonusesOff += bonusesOff;
			Update(acc);
		}

		public static bool IsCurrentUserAdmin()
		{
			return System.Web.HttpContext.Current.User.IsInRole(Role.admin);
		}

		public static bool IsCurrentUserAdminTrainer()
		{
			return System.Web.HttpContext.Current.User.IsInRole(Role.trainer);
		}

		public static bool IsCurrentUserClient()
		{
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
			{
				return true;
			}

			Account acc = GetLite(int.Parse(System.Web.HttpContext.Current.User.Identity.Name));
			return acc.RoleId == Role.clientId;
		}

		private static readonly DateTime bonusesDateStart = new DateTime(2021, 11, 20, 0, 0, 0);
		private static readonly DateTime bonusesDateEnd = new DateTime(2022, 3, 1, 0, 0, 0);

		public static void ReacalculateBonuses(int id)
		{
			var acc = AccountDAL.Get(id);

			acc.Bonuses = acc.TrainingItems.Count(x => x.Date >= bonusesDateStart && x.Date < bonusesDateEnd && !x.Razovoe && x.StatusId == (int)TrainingItemStatus.yes);

			AccountDAL.UpdateBonuses(acc);

			AccountCache.BonusesChanged(acc.Id, acc.Bonuses);
		}

		private static void CreateRazovoeSubs(Account account)
		{
			foreach (var b in BranchB.GetAll())
			{
				Subscription sub = new Subscription()
				{
					AccountId = account.Id,
					BuyDate = DateTime.Today,
					BranchId = b.Id,
					Cost = 0,
					ActiveDays = 0,
					ByCash = false,
					TrainingCount = 0,
					StatusId = (int)SubscriptionStatus.Razovoe,
					Name = "Разовый",
					ActivateDate = null,
				};

				SubscriptionDAL.Add(sub);
			}
		}

		public static Account GetByPhone(string phone)
		{
			return AccountDAL.GetByPhone(phone);
		}
	}
}