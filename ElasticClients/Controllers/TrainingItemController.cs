using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using ElasticaClients.Models;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
	public class TrainingItemController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Details(int id)
		{
			return View();
		}

		[Authorize(Roles = "admin, trainer")]
		public ActionResult Create(int trainingId)
		{
			TrainingItemModel model = new TrainingItemModel();
			model.RazovoeCost = 0;
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin, trainer")]
		public ActionResult Create(TrainingItemModel model)
		{
			if (ModelState.IsValid)
			{
				if (!TrainingItemB.Add((TrainingItem)model))
				{
					ModelState.AddModelError("", "Не смогли записать пользователя.");
					return View(model);
				}

				return RedirectToAction("Details", "Training", new { id = model.TrainingId });
			}

			return View(model);
		}

		[Authorize(Roles = "admin")]
		public ActionResult Edit(int id)
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		[Authorize(Roles = "admin")]
		public ActionResult Delete(int id)
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Delete(int id, FormCollection collection)
		{
			var ti = TrainingItemB.Get(id);

			TrainingItemB.Delete(id);

			return RedirectToAction("Details", "Training", new { id = ti.TrainingId });
		}

		[Authorize]
		public ActionResult SignUp(int trainingId)
		{
			TrainingItemModel model = new TrainingItemModel
			{
				RazovoeCost = 0,
				AccountId = int.Parse(System.Web.HttpContext.Current.User.Identity.Name)
			};

			var training = TrainingB.Get(trainingId);

			model.RazovoeCost = TrainingItemB.GetRazovoeCost(model.AccountId, training.Gym.BranchId);

			return View(model);
		}

		[Authorize]
		[HttpPost]
		public ActionResult SignUp(TrainingItemModel model)
		{
			var training = TrainingB.Get(model.TrainingId);

			model.RazovoeCost = TrainingItemB.GetRazovoeCost(model.AccountId, training.Gym.BranchId);

			if (ModelState.IsValid)
			{
				TrainingItem ti = (TrainingItem)model;
				ti.IsTrial = AccountB.IsFirstTime(ti.AccountId);

				TrainingItemB.Add(ti);
				return RedirectToAction("Details", "Training", new { id = model.TrainingId });
			}

			return View(model);
		}

		public ActionResult _GetSubscriptionsForAccount(int accId)
		{
			var model = SubscriptionB.GetForAccount(accId).Where(x => x.StatusId == (int)SubscriptionStatus.Activated || x.StatusId == (int)SubscriptionStatus.NotActivated).ToList();
			return PartialView(model);
		}
	}
}