using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElasticaClients.DAL.Entities
{
	public class Branch
	{
		public int Id { get; set; }
		[Required]
		[DisplayName("Филиал :")]
		public string Name { get; set; }
		public List<Subscription> Subscriptions { get; set; }
		public List<Income> Incomes { get; set; }
		public List<Gym> Gyms { get; set; }
		public Branch()
		{
			Subscriptions = new List<Subscription>();
			Incomes = new List<Income>();
			Gyms = new List<Gym>();
		}
	}
}