using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ElasticaClients.Logic
{
    public class RoleB
    {
        private readonly IRoleDAL _roleDAL;
        private readonly AccountB _accountB;

        public RoleB(IRoleDAL roleDAL, AccountB accountB)
        {
            _roleDAL = roleDAL;
            _accountB = accountB;
        }

        public string[] GetRoles(string accId)
        {
            var acc = _accountB.GetLite(Convert.ToInt32(accId));

            if (acc != null)
            {
                if (acc.RoleId == Role.ownerId)
                    return Role.ownerRoles;
                if (acc.RoleId == Role.adminId)
                    return Role.adminRoles;
                if (acc.RoleId == Role.trainerId)
                    return Role.trainerRoles;
                if (acc.RoleId == Role.clientId)
                    return Role.clientRoles;
            }

            return new string[] { };
        }

        internal List<Role> GetEditRoles()
        {
            var roles = _roleDAL.GetAll();
            roles.RemoveAll(x => x.Id == Role.ownerId);
            return roles;
        }
    }
}