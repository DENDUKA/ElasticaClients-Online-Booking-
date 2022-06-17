using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ElasticaClients.DAL.Entities
{
	public class SubscriptionContext : DbContext
	{
		public SubscriptionContext() : base("FlyStretch")
		{
			((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 4000; // seconds
		}

		public DbSet<Subscription> Subscriptions { get; set; }
	}
}