using System.Data.Entity;

namespace ElasticaClients.DAL.Entities
{
    public class TrainingItemContext : DbContext
    {
        public TrainingItemContext() : base("FlyStretch")
        { }

        public DbSet<TrainingItem> TrainingItems { get; set; }
    }
}