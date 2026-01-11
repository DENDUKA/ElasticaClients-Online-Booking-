using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticaClients.DAL.Data
{
    public class BranchDAL : IBranchDAL
    {
        public List<Branch> GetAll()
        {
            using (BranchContext db = new BranchContext())
            {
                return db.Branches.ToList();
            }
        }

        public Branch Get(int id)
        {
            using (BranchContext db = new BranchContext())
            {
                return db.Branches.Where(g => g.Id == id)
                       .Include(g => g.Gyms)
                       .First();
            }
        }

        public async Task<List<Branch>> GetAllAsync()
        {
            using (BranchContext db = new BranchContext())
            {
                return await Task.Run(() => db.Branches.ToList());
            }
        }
    }
}