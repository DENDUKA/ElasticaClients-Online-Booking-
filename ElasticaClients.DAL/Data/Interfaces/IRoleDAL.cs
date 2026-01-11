using ElasticaClients.DAL.Entities;
using System.Collections.Generic;

namespace ElasticaClients.DAL.Data.Interfaces
{
    public interface IRoleDAL
    {
        List<Role> GetAll();
    }
}
