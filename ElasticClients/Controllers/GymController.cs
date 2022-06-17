using ElasticaClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    public class GymController : Controller
    {
		[Authorize(Roles = "admin")]
		public ActionResult Index(int branchId)
        {
			return null;
        }

		[Authorize(Roles = "admin")]
		public ActionResult Details(int id)
		{
			var model = GymModel.Get(id);

			return View(model);
		}

		[Authorize(Roles = "admin")]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Create(GymModel model)
		{
			return View();
		}
		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Delete(int id)
		{
			return View();
		}
	}
}