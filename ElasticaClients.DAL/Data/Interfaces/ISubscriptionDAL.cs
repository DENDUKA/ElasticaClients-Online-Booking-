using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ElasticaClients.DAL.Data.Interfaces
{
    public interface ISubscriptionDAL
    {
        Subscription Get(int id);
        List<Subscription> GetAllWithStatus(int subStatus);
        List<Subscription> GetFreezeExpired();
        void Delete(int id);
        void Activate(int id, DateTime activateTime);
        void Update(Subscription sub);
        List<Subscription> GetAll(DateTime? start = null, DateTime? end = null);
        List<Subscription> GetForBranch(int branchId, bool includeRazovoe = false, DateTime? start = null, DateTime? end = null);
        List<Subscription> GetForAccount(int accId, int branchId = 0, bool includeRazovoe = false);
        void Add(Subscription sub);
        void AddFreeze(FreezeSubscriptionItem freeze);
        void UpdateFreeze(FreezeSubscriptionItem freeze);
        void DeleteFreeze(int id);
    }
}
