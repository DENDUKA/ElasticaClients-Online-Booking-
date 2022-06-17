using System.Data.Entity;

namespace ElasticaClients.DAL.Entities
{
	public class TrainingContext : DbContext
	{
		public TrainingContext() : base("FlyStretch")
		{ }

		public DbSet<Training> Trainings { get; set; }
	}
}
