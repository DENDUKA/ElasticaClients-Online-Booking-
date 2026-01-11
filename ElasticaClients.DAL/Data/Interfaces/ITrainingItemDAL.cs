using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ElasticaClients.DAL.Data.Interfaces
{
    public interface ITrainingItemDAL
    {
        void Delete(int id);
        TrainingItem Get(int id);
        void ChangeStatus(int tiid, TrainingItemStatus tiStatus);
        void ChangePayStatus(int tiid, TrainingItemPayStatus tiPayStatus);
        List<TrainingItem> GetAll();
        List<TrainingItem> GetAllForBranch(int branchId, DateTime start, DateTime end, bool onlyRazovoe);
        void Update(TrainingItem ti);
        void Add(TrainingItem ti);
        TrainingItem GetByYClients(int id);
        List<TrainingItem> GetAllForAccount(int accountId, DateTime? start = null, DateTime? end = null);
    }
}
