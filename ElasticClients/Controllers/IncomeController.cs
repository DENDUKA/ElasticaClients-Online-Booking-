using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using ElasticaClients.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
	public class IncomeController : Controller
	{
		[Authorize(Roles = "admin")]
		public ActionResult Index(int branchId, int year = -1, int month = -1)
		{
			if (year == -1 || month == -1)
			{
				year = DateTime.Today.Year;
				month = DateTime.Today.Month;
			}

			DateTime start = new DateTime(year, month, 1);
			DateTime end = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

			ViewData["branchId"] = branchId;
			ViewData["DateTime"] = start;
			ViewData["DateFormat"] = $"{year} - {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}";
			var model = new IncomeTotalModel(branchId, start, end);

			return View(model);
		}

		[Authorize(Roles = "admin")]
		public ActionResult Details(int id)
		{
			return View();
		}

		[Authorize(Roles = "admin")]
		public ActionResult Create(int branchId)
		{
			IncomeModel model = new IncomeModel
			{
				BranchId = branchId,				
			};
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Create(IncomeModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					IncomeB.Add((Income)model);

					return RedirectToAction("Index", new { BranchId = model.BranchId });
				}

				ViewData["BranchId"] = model.BranchId;
				return View(model);
			}
			catch (Exception ex)
			{
				return View(model);
			}
		}

		[Authorize(Roles = "admin")]
		public ActionResult Edit(int id)
		{
			var model = IncomeModel.Get(id);
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Edit(int id, IncomeModel model)
		{
			if (ModelState.IsValid)
			{
				IncomeB.Update((Income)model);

				return RedirectToAction("Index", new { BranchId = model.BranchId });
			}

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Delete(int id)
		{
			try
			{
				Income inc = IncomeB.Get(id);
				IncomeB.Delete(id);

				return RedirectToAction("Index", new { BranchId = inc.BranchId });
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Gym");
			}
		}
	}
}