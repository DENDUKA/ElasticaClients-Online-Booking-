using System.Data.Entity;

namespace ElasticaClients.DAL.Entities
{
    public class RoleContext : DbContext
    {
        public RoleContext() : base("FlyStretch")
        { }

        public DbSet<Role> Roles { get; set; }
    }
}