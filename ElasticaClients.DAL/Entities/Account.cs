using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticaClients.DAL.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Instagram { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<TrainingItem> TrainingItems { get; set; }
        public List<Training> Trainings { get; set; }
        public int SettingsBranchId { get; set; }
        public bool Verified { get; set; }
        public int Bonuses { get; set; }
        public int BonusesOff { get; set; }
        public Account()
        {
            Trainings = new List<Training>();
            Subscriptions = new List<Subscription>();
            TrainingItems = new List<TrainingItem>();
        }
    }
}