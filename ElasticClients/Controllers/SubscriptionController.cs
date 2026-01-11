using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Helpers;
using ElasticaClients.Logic;
using ElasticaClients.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly SubscriptionB _subscriptionB;
        private readonly AccountB _accountB;
        private readonly BranchB _branchB;
        private readonly TrainingItemB _trainingItemB;
        private readonly NavigationHelper _navigationHelper;

        public SubscriptionController(
            SubscriptionB subscriptionB, 
            AccountB accountB, 
            BranchB branchB, 
            TrainingItemB trainingItemB,
            NavigationHelper navigationHelper)
        {
            _subscriptionB = subscriptionB;
            _accountB = accountB;
            _branchB = branchB;
            _trainingItemB = trainingItemB;
            _navigationHelper = navigationHelper;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            int branchId = _navigationHelper.GetBranchId(Session);

            var subs = _subscriptionB.GetForBranch(branchId);

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
            var model = (SubscriptionModel)_subscriptionB.Get(id);

            var currentAccount = _accountB.GetCurrentUser();
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
                BranchId = _branchB.GetBranchId(Session),
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

                    _subscriptionB.Add(sub);

                    if (model.TiToThisSub)
                    {
                        _trainingItemB.AddRazovoesToSubscription(sub.AccountId, sub.Id);
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
            var model = (SubscriptionModel)_subscriptionB.Get(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, SubscriptionModel model)
        {
            if (ModelState.IsValid)
            {
                _subscriptionB.Update((Subscription)model);

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
                _subscriptionB.Delete(id);

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
            Subscription sub = _subscriptionB.Get(id);

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
                _subscriptionB.Activate(model.Id, (DateTime)model.ActivateDate);
            }

            return RedirectToAction("Details", new { model.Id });
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Reacalculate(int id)
        {
            _subscriptionB.RecalculateValues(id);
            return RedirectToAction("Details", new { id });
        }
    }
}