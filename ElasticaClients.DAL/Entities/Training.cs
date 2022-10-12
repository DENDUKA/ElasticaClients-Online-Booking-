using ElasticaClients.DAL.Accessory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticaClients.DAL.Entities
{
	public class Training
	{
		public int Id { get; set; }
		public int YClientId { get; set; }
		public string Name { get; set; }
		public DateTime StartTime { get; set; }
		public TimeSpan Duration { get; set; }
		public int Seats { get; set; }
		public int SeatsLeft => Seats - SeatsTaken;
		[ForeignKey("Gym")]
		public int GymId { get; set; }
		public Gym Gym { get; set; }
		public int StatusId { get; set; }
		public int SeatsTaken { get; set; }
		public int TrainerPay { get; set; }
		public string StatusName
		{
			get
			{
				return StatusNameDictionary[(TrainingStatus)StatusId];
			}
		}
		[ForeignKey("Trainer")]
		public int TrainerId { get; set; }
		public Account Trainer { get; set; }
		public List<TrainingItem> TrainingItems { get; set; }
		public DateTime EndTime
		{
			get
			{
				return StartTime.Add(Duration);
			}
		}

		public Training()
		{
			TrainingItems = new List<TrainingItem>();
		}

		public static Dictionary<TrainingStatus, string> StatusNameDictionary = new Dictionary<TrainingStatus, string>()
		{
			{ TrainingStatus.Active , "Активна" },
			{ TrainingStatus.Canceled , "Отменена" },
		};

		public override string ToString()
		{
			return $"{StartTime:dd.MM HH:mm} {Name}";
		}
	}
}