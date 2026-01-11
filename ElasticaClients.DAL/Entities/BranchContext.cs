using System.Data.Entity;

namespace ElasticaClients.DAL.Entities
{
    class BranchContext : DbContext
    {
        public BranchContext() : base("FlyStretch")
        { }

        public DbSet<Branch> Branches { get; set; }
    }
}
