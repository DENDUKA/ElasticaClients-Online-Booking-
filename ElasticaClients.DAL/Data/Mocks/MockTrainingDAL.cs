using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.DAL.Data.Mocks
{
    public class MockTrainingDAL : ITrainingDAL
    {
        private static List<Training> _trainings;

        static MockTrainingDAL()
        {
            _trainings = new List<Training>();
            
            // Generate some trainings for the current week
            var startDate = DateTime.Today;
            for (int i = 0; i < 7; i++)
            {
                var date = startDate.AddDays(i);
                
                // Morning Training
                _trainings.Add(new Training
                {
                    Id = i * 2 + 1,
                    Name = "Morning Yoga",
                    StartTime = date.AddHours(10),
                    Duration = TimeSpan.FromHours(1),
                    Seats = 10,
                    GymId = 1, // Gym 1 from MockGymDAL
                    TrainerId = 2, // Trainer One from MockAccountDAL
                    StatusId = (int)TrainingStatus.Active,
                    TrainerPay = 500
                });

                // Evening Training
                _trainings.Add(new Training
                {
                    Id = i * 2 + 2,
                    Name = "Evening Power",
                    StartTime = date.AddHours(19),
                    Duration = TimeSpan.FromHours(1.5),
                    Seats = 15,
                    GymId = 1,
                    TrainerId = 2,
                    StatusId = (int)TrainingStatus.Active,
                    TrainerPay = 700
                });
            }
        }

        public int Add(Training training)
        {
            if (_trainings.Count > 0)
                training.Id = _trainings.Max(t => t.Id) + 1;
            else
                training.Id = 1;

            _trainings.Add(training);
            return training.Id;
        }

        public void Delete(int id)
        {
            var t = Get(id);
            if (t != null)
                _trainings.Remove(t);
        }

        public Training Get(int id)
        {
            var t = _trainings.FirstOrDefault(x => x.Id == id);
            return PopulateRelations(t);
        }

        public List<Training> GetAllForGym(int gymId)
        {
            var result = _trainings.Where(t => t.GymId == gymId).ToList();
            result.ForEach(t => PopulateRelations(t));
            return result;
        }

        public List<Training> GetAllForGym(int gymId, DateTime start, DateTime end)
        {
            var result = _trainings
                .Where(t => t.GymId == gymId && t.StartTime >= start && t.StartTime <= end)
                .ToList();
            result.ForEach(t => PopulateRelations(t));
            return result;
        }

        public List<Training> GetAllForTrainer(int trainerId, DateTime start, DateTime end)
        {
            var result = _trainings
                .Where(t => t.TrainerId == trainerId && t.StartTime >= start && t.StartTime <= end)
                .ToList();
            result.ForEach(t => PopulateRelations(t));
            return result;
        }

        public Training GetBase(int id)
        {
            return Get(id);
        }

        private Training PopulateRelations(Training t)
        {
            if (t != null)
            {
                if (t.Gym == null)
                    t.Gym = new MockGymDAL().Get(t.GymId);
                if (t.Trainer == null)
                    t.Trainer = new MockAccountDAL().Get(t.TrainerId);
                
                if (t.TrainingItems == null)
                    t.TrainingItems = new List<TrainingItem>();
            }
            return t;
        }

        public Training GetByYclintsId(int id)
        {
            return _trainings.FirstOrDefault(x => x.YClientId == id);
        }

        public Training GetIndivid(DateTime date, string yclientId)
        {
            // Implementation specific to logic, for mock we just return null or first match
            return _trainings.FirstOrDefault(t => t.StartTime.Date == date.Date); 
        }

        public void Update(Training t)
        {
            var existing = _trainings.FirstOrDefault(x => x.Id == t.Id);
            if (existing != null)
            {
                existing.Name = t.Name;
                existing.StartTime = t.StartTime;
                existing.Duration = t.Duration;
                existing.Seats = t.Seats;
                existing.GymId = t.GymId;
                existing.TrainerId = t.TrainerId;
                existing.StatusId = t.StatusId;
                existing.TrainerPay = t.TrainerPay;
            }
        }
    }
}
