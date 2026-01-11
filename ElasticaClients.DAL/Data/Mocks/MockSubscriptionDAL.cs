using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.DAL.Data.Mocks
{
    public class MockSubscriptionDAL : ISubscriptionDAL
    {
        private static List<Subscription> _subscriptions;

        static MockSubscriptionDAL()
        {
            _subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Id = 1,
                    AccountId = 3, // Client One
                    BranchId = 1,
                    Name = "8 Занятий",
                    Cost = 3000,
                    TrainingCount = 8,
                    TrainingsLeft = 8,
                    ActiveDays = 30,
                    BuyDate = DateTime.Today.AddDays(-5),
                    ActivateDate = DateTime.Today.AddDays(-2),
                    StatusId = (int)SubscriptionStatus.Activated,
                    ByCash = true
                }
            };
        }

        public void Add(Subscription sub)
        {
            if (_subscriptions.Count > 0)
                sub.Id = _subscriptions.Max(s => s.Id) + 1;
            else
                sub.Id = 1;
            _subscriptions.Add(sub);
        }

        public void AddFreeze(FreezeSubscriptionItem freeze)
        {
            var sub = Get(freeze.SubscriptionId);
            if (sub != null)
            {
                if (sub.FreezeSubscriptionList == null)
                    sub.FreezeSubscriptionList = new List<FreezeSubscriptionItem>();
                
                freeze.Id = new Random().Next(1, 10000);
                sub.FreezeSubscriptionList.Add(freeze);
                sub.StatusId = (int)SubscriptionStatus.Freezed;
            }
        }

        public void Activate(int id, DateTime activateTime)
        {
            var sub = Get(id);
            if (sub != null)
            {
                sub.ActivateDate = activateTime;
                sub.StatusId = (int)SubscriptionStatus.Activated;
            }
        }

        public void Delete(int id)
        {
            var sub = Get(id);
            if (sub != null)
                _subscriptions.Remove(sub);
        }

        public void DeleteFreeze(int id)
        {
            foreach (var sub in _subscriptions)
            {
                var freeze = sub.FreezeSubscriptionList?.FirstOrDefault(f => f.Id == id);
                if (freeze != null)
                {
                    sub.FreezeSubscriptionList.Remove(freeze);
                    break; // Assuming unique ID across system or per sub
                }
            }
        }

        public Subscription Get(int id)
        {
            var sub = _subscriptions.FirstOrDefault(s => s.Id == id);
            return PopulateRelations(sub);
        }

        public List<Subscription> GetAll(DateTime? start = null, DateTime? end = null)
        {
            var query = _subscriptions.AsQueryable();
            if (start.HasValue)
                query = query.Where(s => s.BuyDate >= start.Value);
            if (end.HasValue)
                query = query.Where(s => s.BuyDate <= end.Value);
            
            var result = query.ToList();
            result.ForEach(s => PopulateRelations(s));
            return result;
        }

        public List<Subscription> GetAllWithStatus(int subStatus)
        {
            var result = _subscriptions.Where(s => s.StatusId == subStatus).ToList();
            result.ForEach(s => PopulateRelations(s));
            return result;
        }

        public List<Subscription> GetForAccount(int accId, int branchId = 0, bool includeRazovoe = false)
        {
            var query = _subscriptions.Where(s => s.AccountId == accId);
            if (branchId > 0)
                query = query.Where(s => s.BranchId == branchId);
            
            var result = query.ToList();
            result.ForEach(s => PopulateRelations(s));
            return result;
        }

        public List<Subscription> GetForBranch(int branchId, bool includeRazovoe = false, DateTime? start = null, DateTime? end = null)
        {
            var query = _subscriptions.Where(s => s.BranchId == branchId);
             if (start.HasValue)
                query = query.Where(s => s.BuyDate >= start.Value);
            if (end.HasValue)
                query = query.Where(s => s.BuyDate <= end.Value);

            var result = query.ToList();
            result.ForEach(s => PopulateRelations(s));
            return result;
        }

        private Subscription PopulateRelations(Subscription sub)
        {
            if (sub != null)
            {
                if (sub.Account == null)
                    sub.Account = new MockAccountDAL().Get(sub.AccountId);
                if (sub.Branch == null)
                    sub.Branch = new MockBranchDAL().Get(sub.BranchId);
            }
            return sub;
        }

        public List<Subscription> GetFreezeExpired()
        {
            // Simple mock implementation
            return new List<Subscription>();
        }

        public void Update(Subscription sub)
        {
            var existing = _subscriptions.FirstOrDefault(s => s.Id == sub.Id);
            if (existing != null)
            {
                existing.Name = sub.Name;
                existing.AccountId = sub.AccountId;
                existing.BranchId = sub.BranchId;
                existing.Cost = sub.Cost;
                existing.ActiveDays = sub.ActiveDays;
                existing.StatusId = sub.StatusId;
                existing.BuyDate = sub.BuyDate;
                existing.ActivateDate = sub.ActivateDate;
                existing.TrainingCount = sub.TrainingCount;
                existing.TrainingsLeft = sub.TrainingsLeft;
            }
        }

        public void UpdateFreeze(FreezeSubscriptionItem freeze)
        {
             // Mock implementation
        }
    }
}
