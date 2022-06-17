using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticaClients.Entities
{
	public class TrainingCalendarJSON
	{
		public TrainingCalendarJSON()
		{

		}

		public TrainingCalendarJSON(int id, string name, DateTime dateStart, DateTime dateEnd)
		{
			this.Id = id;
			this.Name = name;
			this.DateStart = dateStart;
			this.DateEnd = dateEnd;
		}

		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[StringLength(100)]
		public string Name { get; set; }

		[Column(TypeName = "date")]
		public DateTime DateStart { get; set; }

		[Column(TypeName = "date")]
		public DateTime DateEnd { get; set; }
	}
}