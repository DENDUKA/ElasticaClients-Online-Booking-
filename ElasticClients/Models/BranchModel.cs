using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElasticaClients.Models
{
    public class BranchModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<Income> Incomes { get; set; }
        public List<Gym> Gyms { get; set; }

        public static explicit operator BranchModel(Branch b)
        {
            return new BranchModel()
            {
                Id = b.Id,
                Incomes = b.Incomes,
                Name = b.Name,
                Subscriptions = b.Subscriptions,
                Gyms = b.Gyms,
            };
        }
    }
}