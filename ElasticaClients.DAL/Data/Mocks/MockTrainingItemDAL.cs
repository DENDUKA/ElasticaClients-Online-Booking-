using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.DAL.Data.Mocks
{
    public class MockTrainingItemDAL : ITrainingItemDAL
    {
        private static List<TrainingItem> _items;

        static MockTrainingItemDAL()
        {
            _items = new List<TrainingItem>();
            // Let's add one booking for Client One
            // Assuming Training ID 1 exists (from MockTrainingDAL loop)
            // Assuming Subscription ID 1 exists (from MockSubscriptionDAL)
            _items.Add(new TrainingItem
            {
                Id = 1,
                AccountId = 3,
                SubscriptionId = 1,
                TrainingId = 1,
                StatusId = (int)TrainingItemStatus.unKnown, // Or whatever the enum value is
                StatusPayId = (int)TrainingItemPayStatus.yes,
                Razovoe = false,
                IsTrial = false
            });
        }

        public void Add(TrainingItem ti)
        {
            if (_items.Count > 0)
                ti.Id = _items.Max(i => i.Id) + 1;
            else
                ti.Id = 1;
            _items.Add(ti);
        }

        public void ChangePayStatus(int tiid, TrainingItemPayStatus tiPayStatus)
        {
            var item = Get(tiid);
            if (item != null)
                item.StatusPayId = (int)tiPayStatus;
        }

        public void ChangeStatus(int tiid, TrainingItemStatus tiStatus)
        {
            var item = Get(tiid);
            if (item != null)
                item.StatusId = (int)tiStatus;
        }

        public void Delete(int id)
        {
            var item = Get(id);
            if (item != null)
                _items.Remove(item);
        }

        public TrainingItem Get(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.Training = new MockTrainingDAL().Get(item.TrainingId);
                item.Account = new MockAccountDAL().Get(item.AccountId);
                item.Subscription = new MockSubscriptionDAL().Get(item.SubscriptionId);
            }
            return item;
        }

        public List<TrainingItem> GetAll()
        {
            return _items;
        }

        public List<TrainingItem> GetAllForAccount(int accountId, DateTime? start = null, DateTime? end = null)
        {
            var query = _items.Where(i => i.AccountId == accountId);
            
            // To filter by date we need the Training object loaded
            // Since this is a simple mock, we will load training for all items in the query first
            var results = new List<TrainingItem>();
            foreach(var item in query)
            {
                // We reuse Get to ensure relations are loaded
                var fullItem = Get(item.Id);
                if (fullItem.Training != null)
                {
                    if (start.HasValue && fullItem.Training.StartTime < start.Value) continue;
                    if (end.HasValue && fullItem.Training.StartTime > end.Value) continue;
                    results.Add(fullItem);
                }
            }
            return results;
        }

        public List<TrainingItem> GetAllForBranch(int branchId, DateTime start, DateTime end, bool onlyRazovoe)
        {
             var results = new List<TrainingItem>();
             foreach(var item in _items)
             {
                 var fullItem = Get(item.Id);
                 if (fullItem.Training != null && fullItem.Training.Gym != null && fullItem.Training.Gym.BranchId == branchId)
                 {
                    if (fullItem.Training.StartTime >= start && fullItem.Training.StartTime <= end)
                    {
                        if (onlyRazovoe && !fullItem.Razovoe) continue;
                        results.Add(fullItem);
                    }
                 }
             }
             return results;
        }

        public TrainingItem GetByYClients(int id)
        {
            return _items.FirstOrDefault(i => i.YClientsId == id);
        }

        public void Update(TrainingItem ti)
        {
            var existing = _items.FirstOrDefault(i => i.Id == ti.Id);
            if (existing != null)
            {
                existing.AccountId = ti.AccountId;
                existing.SubscriptionId = ti.SubscriptionId;
                existing.TrainingId = ti.TrainingId;
                existing.StatusId = ti.StatusId;
                existing.StatusPayId = ti.StatusPayId;
                existing.Razovoe = ti.Razovoe;
                existing.IsTrial = ti.IsTrial;
                existing.YClientsId = ti.YClientsId;
            }
        }
    }
}
