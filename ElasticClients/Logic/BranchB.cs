
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElasticaClients.Logic
{
	public static class BranchB
	{
		public static List<Branch> GetAll()
		{
			var branches = BranchDAL.GetAll();
			branches.Remove(branches.First(x=>x.Id == 4));

			return branches;
		}

		public static IEnumerable<SelectListItem> ToSelectListItems(int selectedId = 0)
		{
			List<Branch> accounts = GetAll();

			List<SelectListItem> x = new List<SelectListItem>
			{
				new SelectListItem() { Text = "", Value = "" }
			};

			x.AddRange(accounts
					  .Select(g =>
						  new SelectListItem
						  {
							  Selected = (g.Id == selectedId),
							  Text = g.Name,
							  Value = g.Id.ToString()
						  })
						  .OrderBy(y => y.Text)
						  .ToList());

			return x;
		}

		public static bool ChangeCurrentBranch(int branchId)
		{
			if (HttpContext.Current != null &&
				HttpContext.Current.User.Identity.IsAuthenticated)
			{
				var accId = HttpContext.Current.User.Identity.Name;
				var acc = AccountDAL.GetLite(int.Parse(accId));
				if (acc != null && acc.SettingsBranchId != branchId)
				{
					acc.SettingsBranchId = branchId;
					AccountDAL.Update(acc);
				}
			}

			return true;
		}

		public static int GetBranchId(HttpSessionStateBase session)
		{
			if (session != null && session["branchid"] == null)
			{
				var id = AccountB.GetSettingsBranchId();
				session["branchid"] = id;
				return id;
			}

			int.TryParse(session["branchid"].ToString(), out int branchId);

			return branchId;
		}
	}
}