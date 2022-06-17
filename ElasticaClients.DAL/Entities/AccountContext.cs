using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ElasticaClients.DAL.Entities
{
	public class AccountContext : DbContext
	{
		public AccountContext() : base("FlyStretch")
		{
		}

		public DbSet<Account> Accounts { get; set; }
	}
}