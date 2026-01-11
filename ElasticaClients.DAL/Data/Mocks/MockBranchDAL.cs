using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticaClients.DAL.Data.Mocks
{
    public class MockBranchDAL : IBranchDAL
    {
        public static List<Branch> _branches = new List<Branch>
        {
            new Branch { Id = 1, Name = "Центральный" },
            new Branch { Id = 2, Name = "Северный" },
            new Branch { Id = 3, Name = "Третий" },
            new Branch { Id = 4, Name = "Четвертый" }
        };

        public Branch Get(int id)
        {
            var branch = _branches.FirstOrDefault(b => b.Id == id);
            return PopulateRelations(branch);
        }

        public List<Branch> GetAll()
        {
            _branches.ForEach(b => PopulateRelations(b));
            return _branches;
        }

        public Task<List<Branch>> GetAllAsync()
        {
            _branches.ForEach(b => PopulateRelations(b));
            return Task.FromResult(_branches);
        }

        private Branch PopulateRelations(Branch branch)
        {
            if (branch != null)
            {
                // Access static data directly to avoid recursion/stack overflow
                branch.Gyms = MockGymDAL._gyms.Where(g => g.BranchId == branch.Id).ToList();
                
                // Set back reference
                foreach(var gym in branch.Gyms)
                {
                    gym.Branch = branch;
                }
            }
            return branch;
        }
    }
}