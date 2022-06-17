using ElasticaClients.DAL.Entities;
using ElasticaClients.DAL.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElasticaClients.Models
{
	public class GymModel
	{
		public int Id { get; set;}
		[Required]
		[DisplayName("Зал: ")]
		public string Name { get; set; }
		public List<Training> Trainings { get; set; }

		internal static List<GymModel> GetAll()
		{
			var gyms = GymDAL.GetAll();

			var models = new List<GymModel>();
			foreach (var gym in gyms)
			{
				models.Add((GymModel)gym);
			}

			return models;
		}

		internal static GymModel Get(int id)
		{
			var gym = GymDAL.Get(id);
			GymModel model = (GymModel)gym;
			return model;
		}

		public static explicit operator GymModel(Gym g)
		{
			return new GymModel()
			{
				Id = g.Id,
				Name = g.Name,
				Trainings = g.Trainings,
			};
		}
	}
}