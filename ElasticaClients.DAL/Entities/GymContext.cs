using System.Data.Entity;

namespace ElasticaClients.DAL.Entities
{
	public class GymContext : DbContext
	{
		public GymContext()	: base("FlyStretch")
		{ }

		public DbSet<Gym> Gyms { get; set; }
	}
}