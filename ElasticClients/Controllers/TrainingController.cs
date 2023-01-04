using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using ElasticaClients.Models;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
	public class TrainingController : Controller
	{
		public ActionResult Index(int gymid)
		{
			var model = GymModel.Get(gymid);

			return View(model);
		}

		public ActionResult Details(int id)
		{
			TrainingModel model = (TrainingModel)TrainingB.Get(id);

			return View(model);
		}

		[Authorize(Roles = "admin")]
		public ActionResult Create(int gymid)
		{
			var gym = GymB.Get(gymid);
			ViewData["GymId"] = gymid;
			ViewData["GymName"] = gym.Name;
			TrainingModel model = new TrainingModel
			{
				Duration = new TimeSpan(0, 0, 0),
				Seats = 1
			};

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Create(TrainingModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					model.StartTime = model.StartDay.AddHours(model.StartTime.Hour).AddMinutes(model.StartTime.Minute);

					model.StatusId = (int)TrainingStatus.Active;
					int id = TrainingB.Add((Training)model);

					return RedirectToAction("Details", "Training", new { id });
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.InnerException.InnerException.Message);
			}

			var gym = GymB.Get(model.GymId);
			ViewData["GymId"] = gym.Id;
			ViewData["GymName"] = gym.Name;
			model.StatusId = (int)TrainingStatus.Active;

			return View(model);
		}

		[Authorize(Roles = "admin")]
		public ActionResult Edit(int id)
		{
			var model = TrainingModel.Get(id);
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Edit(int id, TrainingModel model)
		{
			if (ModelState.IsValid)
			{
				model.StartTime = new DateTime(model.StartDay.Year, model.StartDay.Month, model.StartDay.Day, model.StartTime.Hour, model.StartTime.Minute, 0);
				TrainingB.Update((Training)model);

				return RedirectToAction("Details", new { id = model.Id });
			}

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Delete(int id)
		{
			try
			{
				Training t = TrainingB.Get(id);

				TrainingB.Delete(id);

				return RedirectToAction("Index", new { gymid = t.GymId });
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Gym");
			}
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Cancel(int id)
		{
			try
			{
				TrainingB.Cancel(id);

				return RedirectToAction("Details", new { id });
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Gym");
			}
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Restore(int id)
		{
			try
			{
				TrainingB.Restore(id);

				return RedirectToAction("Details", new { id });
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Gym");
			}
		}

		[HttpPost]
		[Authorize(Roles = "admin,trainer")]
		public void ChangeStatus(int tiid, int statusid)
		{
			Debug.WriteLine("ChangeStatus");
			TrainingItemB.ChangeStatus(tiid, (TrainingItemStatus)statusid);
		}

		[HttpPost]
		[Authorize(Roles = "admin,trainer")]
		public void ChangePayStatus(int tiid, int statusid)
		{
			Debug.WriteLine("ChangePayStatus");
			TrainingItemB.ChangePayStatus(tiid, (TrainingItemPayStatus)statusid);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Reacalculate(int id)
		{
			TrainingB.ReacalculateValues(id);
			return RedirectToAction("Details", new { id });
		}
	}
}