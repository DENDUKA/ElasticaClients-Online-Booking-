using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticaClients.DAL.Entities
{
	public class Gym
	{
		public int Id { get; set; }
		public string Name { get; set; }
		[ForeignKey("Branch")]
		public int BranchId { get; set; }
		public Branch Branch { get; set; }
		public List<Training> Trainings { get; set; }
		public Gym()
		{
			Trainings = new List<Training>();
		}
	}
}