using System.Collections.Generic;

namespace ElasticaClients.DAL.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Account> Accounts { get; set; }
        public Role()
        {
            Accounts = new List<Account>();
        }

        public const int adminId = 7, trainerId = 8, clientId = 9, ownerId = 10;
        public const string admin = "admin", trainer = "trainer", client = "client", owner = "owner";

        public static string[] ownerRoles = new string[] { owner, admin, trainer, client };
        public static string[] adminRoles = new string[] { admin, trainer, client };
        public static string[] trainerRoles = new string[] { trainer, client };
        public static string[] clientRoles = new string[] { client };
    }
}