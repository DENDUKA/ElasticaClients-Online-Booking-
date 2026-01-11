using System;

namespace ElasticaClients.DAL.Accessory
{
    public class Salary : IIncome
    {
        public int IncomeId { get; set; }

        public string IncomeName { get; set; }

        public IncomeType Type { get; set; }

        public int Cost { get; set; }

        public DateTime Date { get; set; }
    }
}
