using ElasticaClients.Logic;

namespace ElasticaClients.Helpers
{
    public class NavigationHelper
    {
        private readonly AccountB _accountB;

        public NavigationHelper(AccountB accountB)
        {
            _accountB = accountB;
        }

        public int GetBranchId(System.Web.HttpSessionStateBase session)
        {
            if (session != null && session["branchid"] == null)
            {
                var id = _accountB.GetSettingsBranchId();
                session["branchid"] = id;
                return id;
            }

            int.TryParse(session["branchid"].ToString(), out int branchId);

            return branchId;
        }
    }
}