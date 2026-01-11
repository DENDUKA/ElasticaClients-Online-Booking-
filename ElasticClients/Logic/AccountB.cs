using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Cache;
using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElasticaClients.Logic
{
    public class AccountB
    {
        private readonly IAccountDAL _accountDAL;
        private readonly ISubscriptionDAL _subscriptionDAL;
        private readonly ITrainingItemDAL _trainingItemDAL;
        private readonly BranchB _branchB;

        public AccountB(IAccountDAL accountDAL, ISubscriptionDAL subscriptionDAL, ITrainingItemDAL trainingItemDAL, BranchB branchB)
        {
            _accountDAL = accountDAL;
            _subscriptionDAL = subscriptionDAL;
            _trainingItemDAL = trainingItemDAL;
            _branchB = branchB;
        }

        public IEnumerable<SelectListItem> ToSelectListItems(int selectedId = 0)
        {
            List<Account> accounts = _accountDAL.GetAll();

            List<SelectListItem> x = new List<SelectListItem>
            {
                new SelectListItem() { Text = "", Value = "" }
            };

            x.AddRange(accounts
                      .Select(g =>
                          new SelectListItem
                          {
                              Selected = (g.Id == selectedId),
                              Text = g.Name,
                              Value = g.Id.ToString()
                          })
                          .OrderBy(y => y.Text)
                          .ToList());

            return x;
        }

        public List<Account> GetWithFilter(int page, int count, string filter)
        {
            return _accountDAL.GetWithFilter(page, count, filter);
        }

        public async Task<List<Account>> GetWithFilterAsync(int page, int count, string filter)
        {
            return await _accountDAL.GetWithFilterAsync(page, count, filter);
        }

        public List<Account> GetAll()
        {
            return _accountDAL.GetAll();
        }

        public int GetSettingsBranchId()
        {
            int branchId = 0;
            Account acc = null;

            if (System.Web.HttpContext.Current != null &&
                System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var accId = System.Web.HttpContext.Current.User.Identity.Name;

                acc = _accountDAL.GetLite(int.Parse(accId));
                if (acc != null)
                {
                    branchId = acc.SettingsBranchId;
                }
            }

            if (branchId == 0)
            {
                branchId = _branchB.GetAll().First().Id;
                if (acc != null)
                {
                    acc.SettingsBranchId = branchId;
                    _accountDAL.Update(acc);
                }
            }

            return branchId;
        }

        public Account GetTrainer(int id)
        {
            return _accountDAL.GetTrainer(id);
        }

        public Account Get(int id, DateTime trainingsFrom, DateTime trainingsTo)
        {
            return _accountDAL.Get(id, trainingsFrom, trainingsTo);
        }

        public bool IsFirstTime(int accountId)
        {
            var tis = _trainingItemDAL.GetAllForAccount(accountId);

            if (tis == null)
            {
                return true;
            }

            return !tis.Any(x => x.StatusId == (int)TrainingItemStatus.yes || x.StatusId == (int)TrainingItemStatus.unKnown || x.StatusId == (int)TrainingItemStatus.no);
        }

        internal Account Get(int id)
        {
            return _accountDAL.Get(id);
        }

        public void Delete(int id)
        {
            _accountDAL.Delete(id);
        }

        public void Add(Account account)
        {
            _accountDAL.Add(account);
            CreateRazovoeSubs(account);
        }

        public void Update(Account account)
        {
            _accountDAL.Update(account);
        }

        public Account GetLite(int id)
        {
            return _accountDAL.GetLite(id);
        }

        public Account GetCurrentUser()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return GetLite(int.Parse(System.Web.HttpContext.Current.User.Identity.Name));
            }

            return null;
        }

        public bool IsCurrentUserOwner()
        {
            return System.Web.HttpContext.Current.User.IsInRole(Role.owner);
        }

        internal void BonusesOff(int accountId, int bonusesOff)
        {
            var acc = Get(accountId);
            acc.BonusesOff += bonusesOff;
            Update(acc);
        }

        public bool IsCurrentUserAdmin()
        {
            return System.Web.HttpContext.Current.User.IsInRole(Role.admin);
        }

        public bool IsCurrentUserAdminTrainer()
        {
            return System.Web.HttpContext.Current.User.IsInRole(Role.trainer);
        }

        public bool IsCurrentUserClient()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return true;
            }

            Account acc = GetLite(int.Parse(System.Web.HttpContext.Current.User.Identity.Name));
            return acc.RoleId == Role.clientId;
        }

        private static readonly DateTime bonusesDateStart = new DateTime(2021, 11, 20, 0, 0, 0);
        private static readonly DateTime bonusesDateEnd = new DateTime(2022, 3, 1, 0, 0, 0);

        public void ReacalculateBonuses(int id)
        {
            var acc = _accountDAL.Get(id);

            acc.Bonuses = acc.TrainingItems.Count(x => x.Date >= bonusesDateStart && x.Date < bonusesDateEnd && !x.Razovoe && x.StatusId == (int)TrainingItemStatus.yes);

            _accountDAL.UpdateBonuses(acc);

            AccountCache.BonusesChanged(acc.Id, acc.Bonuses);
        }

        private void CreateRazovoeSubs(Account account)
        {
            foreach (var b in _branchB.GetAll())
            {
                Subscription sub = new Subscription()
                {
                    AccountId = account.Id,
                    BuyDate = DateTime.Today,
                    BranchId = b.Id,
                    Cost = 0,
                    ActiveDays = 0,
                    ByCash = false,
                    TrainingCount = 0,
                    StatusId = (int)SubscriptionStatus.Razovoe,
                    Name = "Разовый",
                    ActivateDate = null,
                };

                _subscriptionDAL.Add(sub);
            }
        }

        internal Account GetByPhone(string phone)
        {
            return _accountDAL.GetByPhone(phone);
        }
    }
}