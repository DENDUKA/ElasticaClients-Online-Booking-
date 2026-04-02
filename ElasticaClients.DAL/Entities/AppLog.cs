using System;

namespace ElasticaClients.DAL.Entities
{
    public class AppLog
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ActionType { get; set; }
        public string ActorName { get; set; }
        public string ClientName { get; set; }
        public string TrainingInfo { get; set; }
        public string PaymentType { get; set; }
        public int? Cost { get; set; }
    }
}
