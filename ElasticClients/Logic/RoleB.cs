using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.Logic
{
	public static class RoleB
	{
		public static string[] GetRoles(string accId)
		{
			var acc = AccountB.GetLite(Convert.ToInt32(accId));

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

		internal static List<Role> GetEditRoles()
		{
			var roles = RoleDAL.GetAll();
			roles.RemoveAll(x => x.Id == Role.ownerId);
			return roles;
		}
	}
}