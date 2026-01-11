using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticaClients.DAL.Data.Mocks
{
    public class MockAccountDAL : IAccountDAL
    {
        private static List<Account> _accounts;

        static MockAccountDAL()
        {
            _accounts = new List<Account>
            {
                new Account
                {
                    Id = 1,
                    Name = "Admin User",
                    Phone = "79000000001",
                    RoleId = Role.adminId,
                    Password = "admin", // In real app this should be hashed
                    Verified = true,
                    SettingsBranchId = 1
                },
                new Account
                {
                    Id = 2,
                    Name = "Trainer One",
                    Phone = "79000000002",
                    RoleId = Role.trainerId,
                    Password = "trainer",
                    Verified = true,
                    SettingsBranchId = 1
                },
                new Account
                {
                    Id = 3,
                    Name = "Client One",
                    Phone = "79000000003",
                    RoleId = Role.clientId,
                    Password = "client",
                    Verified = true,
                    SettingsBranchId = 1,
                    Bonuses = 100
                }
            };
        }

        public void Add(Account acc)
        {
            if (_accounts.Count > 0)
                acc.Id = _accounts.Max(a => a.Id) + 1;
            else
                acc.Id = 1;
            
            _accounts.Add(acc);
        }

        public void Delete(int id)
        {
            var acc = Get(id);
            if (acc != null)
                _accounts.Remove(acc);
        }

        public Account FindByPassword(string password)
        {
            var acc = _accounts.FirstOrDefault(a => a.Password == password);
            return PopulateRole(acc);
        }

        public Account Get(int id)
        {
            var acc = _accounts.FirstOrDefault(a => a.Id == id);
            return PopulateRole(acc);
        }

        public Account Get(int id, DateTime trainingsFrom, DateTime trainingsTo)
        {
            return Get(id);
        }

        public List<Account> GetAll()
        {
            _accounts.ForEach(a => PopulateRole(a));
            return _accounts;
        }

        public Dictionary<int, Account> GetAllLight()
        {
            _accounts.ForEach(a => PopulateRole(a));
            return _accounts.ToDictionary(a => a.Id, a => a);
        }

        public Account GetByPhone(string phone)
        {
            var acc = _accounts.FirstOrDefault(a => a.Phone == phone);
            return PopulateRole(acc);
        }

        public void GetByPhoneStorageProcedure(string phone)
        {
        }

        public Account GetLite(int id)
        {
            return Get(id);
        }

        public Account GetTrainer(int id)
        {
            var acc = _accounts.FirstOrDefault(a => a.Id == id && (a.RoleId == Role.trainerId || a.RoleId == Role.adminId));
            return PopulateRole(acc);
        }

        public List<Account> GetWithFilter(int page, int count, string filter)
        {
            var query = _accounts.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                query = query.Where(a => 
                    (a.Name != null && a.Name.ToLower().Contains(filter)) || 
                    (a.Phone != null && a.Phone.Contains(filter))
                );
            }

            var result = query.Skip((page - 1) * count).Take(count).ToList();
            result.ForEach(a => PopulateRole(a));
            return result;
        }

        private Account PopulateRole(Account acc)
        {
            if (acc != null && acc.Role == null)
            {
                acc.Role = new MockRoleDAL().GetAll().FirstOrDefault(r => r.Id == acc.RoleId);
            }
            return acc;
        }

        public Task<List<Account>> GetWithFilterAsync(int page, int count, string filter)
        {
            return Task.FromResult(GetWithFilter(page, count, filter));
        }

        public void Update(Account acc)
        {
            var existing = Get(acc.Id);
            if (existing != null)
            {
                existing.Name = acc.Name;
                existing.Phone = acc.Phone;
                existing.Email = acc.Email;
                existing.Instagram = acc.Instagram;
                existing.Password = acc.Password;
                existing.RoleId = acc.RoleId;
                existing.SettingsBranchId = acc.SettingsBranchId;
                existing.Verified = acc.Verified;
                existing.Bonuses = acc.Bonuses;
                existing.BonusesOff = acc.BonusesOff;
            }
        }

        public void UpdateBonuses(Account acc)
        {
            var existing = Get(acc.Id);
            if (existing != null)
            {
                existing.Bonuses = acc.Bonuses;
                existing.BonusesOff = acc.BonusesOff;
            }
        }
    }
}
