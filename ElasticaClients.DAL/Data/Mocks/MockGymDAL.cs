using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.DAL.Data.Mocks
{
    public class MockGymDAL : IGymDAL
    {
        public static List<Gym> _gyms = new List<Gym>
        {
            new Gym { Id = 1, Name = "Зал Йоги", BranchId = 1 },
            new Gym { Id = 2, Name = "Тренажерный зал", BranchId = 1 },
            new Gym { Id = 3, Name = "Большой зал", BranchId = 2 }
        };

        public Gym Get(int id)
        {
            var gym = _gyms.FirstOrDefault(g => g.Id == id);
            return PopulateRelations(gym);
        }

        public List<Gym> GetAll()
        {
            _gyms.ForEach(g => PopulateRelations(g));
            return _gyms;
        }

        public Gym GetByName(int branchId, string name)
        {
            var gym = _gyms.FirstOrDefault(g => g.BranchId == branchId && g.Name == name);
            return PopulateRelations(gym);
        }

        private Gym PopulateRelations(Gym gym)
        {
            if (gym != null && gym.Branch == null)
            {
                // Access static data directly
                gym.Branch = MockBranchDAL._branches.FirstOrDefault(b => b.Id == gym.BranchId);
            }
            return gym;
        }
    }
}