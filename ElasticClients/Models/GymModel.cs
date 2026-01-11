using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElasticaClients.Models
{
    public class GymModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Зал: ")]
        public string Name { get; set; }
        public List<Training> Trainings { get; set; }

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