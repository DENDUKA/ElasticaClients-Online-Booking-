
using System.Web.Mvc;

namespace ElasticaClients.Helpers
{
	public static class NavigationHelper
	{
		public static int GetBranchId(System.Web.HttpSessionStateBase session)
		{
			if (session != null && session["branchid"] == null)
			{
				var id = Logic.AccountB.GetSettingsBranchId();
				session["branchid"] = id;
				return id;
			}

			int.TryParse(session["branchid"].ToString(), out int branchId);

			return branchId;
		}
	}
}