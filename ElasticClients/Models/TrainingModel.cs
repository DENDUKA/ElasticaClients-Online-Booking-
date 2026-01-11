using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElasticaClients.Models
{
    public class TrainingModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Дата начала")]
        public DateTime StartDay { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayName("Время начала")]
        public DateTime StartTime { get; set; }

        public DateTime EndTime
        {
            get
            {
                return StartTime.Add(Duration);
            }
        }

        [Required]
        [DisplayName("Продолжительность")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan Duration { get; set; }

        [Required]
        [DisplayName("Количество мест")]
        public int Seats { get; set; }

        [Required]
        [DisplayName("Зал")]
        public int GymId { get; set; }

        public Gym Gym { get; set; }

        [Required]
        [DisplayName("Статус")]
        public int StatusId { get; set; }

        [DisplayName("Занято мест")]
        public int SeatsTaken { get; set; }

        [DisplayName("Свободно мест")]
        public int SeatsFree
        {
            get
            {
                return Seats - SeatsTaken;
            }
        }

        [DisplayName("Зарплата")]
        public int TrainerPay { get; set; }

        public string StatusName
        {
            get
            {
                return Training.StatusNameDictionary[(TrainingStatus)StatusId];
            }
        }

        [Required]
        [DisplayName("Тренер")]
        public int TrainerId { get; set; }

        public Account Trainer { get; set; }

        public List<TrainingItem> TrainingItems { get; set; }

        internal static List<TrainingModel> GetAllForGym(TrainingB trainingB, int gymId)
        {
            List<TrainingModel> model = new List<TrainingModel>();

            foreach (var t in trainingB.GetAllForGym(gymId))
            {
                model.Add((TrainingModel)t);
            }

            return model;
        }

        public static TrainingModel Get(TrainingB trainingB, int id)
        {
            return (TrainingModel)trainingB.Get(id);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            var start = StartDay.AddHours(StartTime.Hour).AddMinutes(StartTime.Minute);

            //if (!TrainingB.IsTimeFree(GymId, start, start.Add(Duration), Id))
            //{
            //	results.Add(new ValidationResult("На это время назначена другая тренировка."));
            //}

            return results;
        }

        public static explicit operator TrainingModel(Training t)
        {
            return new TrainingModel()
            {
                Id = t.Id,
                Name = t.Name,
                StartTime = t.StartTime,
                StartDay = t.StartTime,
                Seats = t.Seats,
                Gym = t.Gym,
                GymId = t.GymId,
                StatusId = t.StatusId,
                Trainer = t.Trainer,
                TrainerId = t.TrainerId,
                TrainingItems = t.TrainingItems,
                Duration = t.Duration,
                SeatsTaken = t.SeatsTaken,
                TrainerPay = t.TrainerPay,
            };
        }

        public static explicit operator Training(TrainingModel tm)
        {
            return new Training()
            {
                Id = tm.Id,
                Name = tm.Name,
                StartTime = tm.StartTime,
                Seats = tm.Seats,
                GymId = tm.GymId,
                StatusId = tm.StatusId,
                TrainerId = tm.TrainerId,
                Duration = tm.Duration,
                SeatsTaken = tm.SeatsTaken,
                TrainerPay = tm.TrainerPay,
            };
        }
    }
}