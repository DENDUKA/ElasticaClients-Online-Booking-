using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticaClients.DAL.Entities
{
	public class FreezeSubscriptionItemContext : DbContext
	{
		public FreezeSubscriptionItemContext() : base("FlyStretch")
		{ }

		public DbSet<FreezeSubscriptionItem> FreezeSubscriptionList { get; set; }
	}
}
