using System.Data.Entity;

namespace ElasticaClients.DAL.Entities
{
    public class FreezeSubscriptionItemContext : DbContext
    {
        public FreezeSubscriptionItemContext() : base("FlyStretch")
        { }

        public DbSet<FreezeSubscriptionItem> FreezeSubscriptionList { get; set; }
    }
}
