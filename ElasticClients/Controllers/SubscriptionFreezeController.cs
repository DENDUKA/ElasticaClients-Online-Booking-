using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using ElasticaClients.Models;
using System;
using System.Web.Mvc;
using System.Web.UI;

namespace ElasticaClients.Controllers
{
	public class SubscriptionFreezeController : Controller
    {
		[Authorize(Roles = "admin")]
		public ActionResult Create(int subscriptionId)
        {
			FreezeSubscriptionModel model = new FreezeSubscriptionModel
			{
				SubscriptionId = subscriptionId,
				Start = DateTime.Today,
				End = DateTime.Today,
			};

			return View(model);
        }

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Create(FreezeSubscriptionModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					SubscriptionB.AddFreeze((FreezeSubscriptionItem)model);

					var sub = SubscriptionB.Get(model.SubscriptionId);

					return RedirectToAction("Details", "Account", new { Id = sub.AccountId });
				}
				else
				{
					return View(model);
				}


			}
			catch (Exception ex)
			{
				return View(model);
			}
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public void Unfreeze(int SubscriptionId)
		{
			SubscriptionB.UnFreeze(SubscriptionId);
			
			Response.Redirect(Request.UrlReferrer.ToString());
		}
	}
}