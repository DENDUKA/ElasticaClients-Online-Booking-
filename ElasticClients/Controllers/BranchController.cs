using ElasticaClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    public class BranchController : Controller
    {
        public ActionResult Index()
        {
            var model = BranchModel.GetAll();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            if (Logic.BranchB.ChangeCurrentBranch(id))
            {
                Session["branchid"] = id;
            }

            var model = BranchModel.Get(id);
            return View(model);
        }
    }
}