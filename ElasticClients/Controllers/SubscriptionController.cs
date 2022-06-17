using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using ElasticaClients.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
	public class SubscriptionController : Controller
	{
		[Authorize(Roles = "admin")]
		public ActionResult Index()
		{
			int branchId = Helpers.NavigationHelper.GetBranchId(Session);

			var subs = SubscriptionB.GetForBranch(branchId);

			List<SubscriptionModel> model = new List<SubscriptionModel>();

			foreach (var a in subs)
			{
				model.Add((SubscriptionModel)a);
			}

			return View(model);
		}

		[Authorize]
		public ActionResult Details(int id)
		{
			var model = (SubscriptionModel)SubscriptionB.Get(id);

			var currentAccount = AccountB.GetCurrentUser();
			if (currentAccount != null && currentAccount.RoleId == Role.clientId)
			{
				if (model.AccountId != currentAccount.Id)
				{
					return RedirectToAction("Details", "Account", new { id = currentAccount.Id });
				}
			}

			return View(model);
		}

		[Authorize(Roles = "admin,trainer")]
		public ActionResult Create(int? branchId, int AccountId = 0)
		{
			SubscriptionModel model = new SubscriptionModel
			{
				BuyDate = DateTime.Today,
				BranchId = BranchB.GetBranchId(Session),
				ActiveDays = 31,
			};

			if (AccountId != 0)
			{
				model.AccountId = AccountId;
			}

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin,trainer")]
		public ActionResult Create(SubscriptionModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var sub = (Subscription)model;
					sub.StatusId = (int)SubscriptionStatus.NotActivated;
					
					SubscriptionB.Add(sub);

					if (model.TiToThisSub)
					{
						TrainingItemB.AddRazovoesToSubscription(sub.AccountId, sub.Id);
					}

					return RedirectToAction("Details", "Subscription", new { id = sub.Id });
				}

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
			var model = (SubscriptionModel)SubscriptionB.Get(id);
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Edit(int id, SubscriptionModel model)
		{
			if (ModelState.IsValid)
			{
				SubscriptionB.Update((Subscription)model);

				return RedirectToAction("Index");
			}

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "owner")]
		public ActionResult Delete(int id)
		{
			try
			{
				SubscriptionB.Delete(id);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index");
			}
		}

		[Authorize(Roles = "admin")]
		public ActionResult Activate(int id)
		{
			Subscription sub = SubscriptionB.Get(id);

			if (sub.StatusId != (int)SubscriptionStatus.NotActivated)
			{
				return RedirectToAction("Details", new { id });
			}

			var model = (SubscriptionModel)sub;

			model.ActivateDate = model.BuyDate;

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		[ValidateAntiForgeryToken]
		public ActionResult Activate(SubscriptionModel model)
		{
			if (model.ActivateDate != null)
			{
				SubscriptionB.Activate(model.Id, (DateTime)model.ActivateDate);
			}

			return RedirectToAction("Details", new { model.Id });
		}		

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Reacalculate(int id)
		{
			SubscriptionB.RecalculateValues(id);
			return RedirectToAction("Details", new { id });
		}
	}
}