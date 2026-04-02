using System.Data.Entity;

namespace ElasticaClients.DAL.Entities
{
    public class AppLogContext : DbContext
    {
        public AppLogContext() : base("FlyStretch")
        {
            Database.SetInitializer<AppLogContext>(null);
        }

        public DbSet<AppLog> AppLogs { get; set; }
    }
}
