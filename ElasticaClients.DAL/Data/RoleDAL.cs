using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.DAL.Data
{
    public class RoleDAL : IRoleDAL
    {
        public List<Role> GetAll()
        {
            using (RoleContext db = new RoleContext())
            {
                return db.Roles.ToList();
            }
        }
    }
}