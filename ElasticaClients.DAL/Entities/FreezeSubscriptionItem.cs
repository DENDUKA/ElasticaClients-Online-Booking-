using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticaClients.DAL.Entities
{
    public class FreezeSubscriptionItem
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        public int FreezeDays
        {
            get
            {
                return (End - Start).Days;
            }
        }
    }
}
