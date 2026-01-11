using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ElasticaClients.Logic
{
    public class BranchB
    {
        private readonly IBranchDAL _branchDAL;
        private readonly IAccountDAL _accountDAL;

        public BranchB(IBranchDAL branchDAL, IAccountDAL accountDAL)
        {
            _branchDAL = branchDAL;
            _accountDAL = accountDAL;
        }

        public Branch Get(int branchId)
        {
            return _branchDAL.Get(branchId);
        }

        public List<Branch> GetAll()
        {
            var branches = _branchDAL.GetAll();

            var forDelete = branches.FirstOrDefault(x => x.Id == 4);
            if (forDelete != null) branches.Remove(forDelete);

            return branches;
        }

        public async Task<List<Branch>> GetAllAsync()
        {
            var branches = await _branchDAL.GetAllAsync();

            var forDelete = branches.FirstOrDefault(x => x.Id == 4);
            if(forDelete != null) branches.Remove(forDelete);

            return branches;
        }

        public IEnumerable<SelectListItem> ToSelectListItems(int selectedId = 0)
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

        public bool ChangeCurrentBranch(int branchId)
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var accId = HttpContext.Current.User.Identity.Name;
                var acc = _accountDAL.GetLite(int.Parse(accId));
                if (acc != null && acc.SettingsBranchId != branchId)
                {
                    acc.SettingsBranchId = branchId;
                    _accountDAL.Update(acc);
                }
            }

            return true;
        }

        public int GetBranchId(HttpSessionStateBase session)
        {
            if (session != null && session["branchid"] == null)
            {
                int branchId = 0;
                Account acc = null;

                if (HttpContext.Current != null &&
                    HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var accId = HttpContext.Current.User.Identity.Name;
                    acc = _accountDAL.GetLite(int.Parse(accId));
                    if (acc != null)
                    {
                        branchId = acc.SettingsBranchId;
                    }
                }

                if (branchId == 0)
                {
                    branchId = GetAll().First().Id;
                    if (acc != null)
                    {
                        acc.SettingsBranchId = branchId;
                        _accountDAL.Update(acc);
                    }
                }

                session["branchid"] = branchId;
                return branchId;
            }

            int.TryParse(session["branchid"].ToString(), out int resultBranchId);

            return resultBranchId;
        }
    }
}