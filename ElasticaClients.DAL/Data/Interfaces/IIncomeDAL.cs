using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ElasticaClients.DAL.Data.Interfaces
{
    public interface IIncomeDAL
    {
        List<Income> GetAll(int branchId, DateTime start, DateTime end);
        void Update(Income income);
        void Delete(int id);
        Income Get(int id);
        void Add(Income income);
    }
}
