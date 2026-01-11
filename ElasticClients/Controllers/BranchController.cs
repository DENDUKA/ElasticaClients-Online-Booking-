using ElasticaClients.Logic;
using ElasticaClients.Models;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    public class BranchController : Controller
    {
        private readonly BranchB _branchB;

        public BranchController(BranchB branchB)
        {
            _branchB = branchB;
        }

        public ActionResult Index()
        {
            var branches = _branchB.GetAll();
            var model = branches.Select(x => (BranchModel)x).ToList();

            return View(model);
        }

        public ActionResult Details(int id)
        {
            if (_branchB.ChangeCurrentBranch(id))
            {
                Session["branchid"] = id;
            }

            var model = (BranchModel)_branchB.Get(id);
            return View(model);
        }
    }
}