using System.Data.Entity;

namespace ElasticaClients.DAL.Entities
{
    public class IncomeContext : DbContext
    {
        public IncomeContext() : base("FlyStretch")
        { }

        public DbSet<Income> Incomes { get; set; }
    }
}