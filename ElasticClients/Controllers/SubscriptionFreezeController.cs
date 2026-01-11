using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using ElasticaClients.Models;
using System;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    public class SubscriptionFreezeController : Controller
    {
        private readonly SubscriptionB _subscriptionB;

        public SubscriptionFreezeController(SubscriptionB subscriptionB)
        {
            _subscriptionB = subscriptionB;
        }

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
                    _subscriptionB.AddFreeze((FreezeSubscriptionItem)model);

                    var sub = _subscriptionB.Get(model.SubscriptionId);

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
            _subscriptionB.UnFreeze(SubscriptionId);

            Response.Redirect(Request.UrlReferrer.ToString());
        }
    }
}