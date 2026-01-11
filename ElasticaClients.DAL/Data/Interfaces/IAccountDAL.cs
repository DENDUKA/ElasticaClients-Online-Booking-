using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticaClients.DAL.Data.Interfaces
{
    public interface IAccountDAL
    {
        void GetByPhoneStorageProcedure(string phone);
        Account Get(int id);
        List<Account> GetWithFilter(int page, int count, string filter);
        Task<List<Account>> GetWithFilterAsync(int page, int count, string filter);
        List<Account> GetAll();
        Dictionary<int, Account> GetAllLight();
        Account GetTrainer(int id);
        Account Get(int id, DateTime trainingsFrom, DateTime trainingsTo);
        Account GetLite(int id);
        Account FindByPassword(string password);
        void Delete(int id);
        void Add(Account acc);
        void UpdateBonuses(Account acc);
        void Update(Account acc);
        Account GetByPhone(string phone);
    }
}
