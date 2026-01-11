using ElasticaClients.DAL.Entities;
using System.Collections.Generic;

namespace ElasticaClients.DAL.Data.Interfaces
{
    public interface IGymDAL
    {
        List<Gym> GetAll();
        Gym Get(int id);
        Gym GetByName(int branchId, string name);
    }
}
