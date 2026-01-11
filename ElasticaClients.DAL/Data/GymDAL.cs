using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ElasticaClients.DAL.Data
{
    public class GymDAL : IGymDAL
    {
        public List<Gym> GetAll()
        {
            using (GymContext db = new GymContext())
            {
                return db.Gyms.ToList();
            }
        }

        public Gym Get(int id)
        {
            using (GymContext db = new GymContext())
            {
                return db.Gyms.Where(g => g.Id == id)
                       //.Include(g => g.Trainings)
                       .Include(g => g.Branch)
                       .First();
            }
        }

        public Gym GetByName(int branchId, string name)
        {
            using (GymContext db = new GymContext())
            {
                return db.Gyms.Where(g => g.Name == name && g.BranchId == branchId)
                       .Include(g => g.Trainings)
                       .First();
            }
        }
    }
}