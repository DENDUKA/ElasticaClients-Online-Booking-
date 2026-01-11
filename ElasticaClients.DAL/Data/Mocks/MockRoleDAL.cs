using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.DAL.Data.Mocks
{
    public class MockRoleDAL : IRoleDAL
    {
        private List<Role> _roles;

        public MockRoleDAL()
        {
            _roles = new List<Role>
            {
                new Role { Id = Role.adminId, Name = Role.admin },
                new Role { Id = Role.trainerId, Name = Role.trainer },
                new Role { Id = Role.clientId, Name = Role.client },
                new Role { Id = Role.ownerId, Name = Role.owner }
            };
        }

        public List<Role> GetAll()
        {
            return _roles;
        }
    }
}
