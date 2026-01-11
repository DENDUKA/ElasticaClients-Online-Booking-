using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ElasticaClients.Logic
{
    public class TrainingB
    {
        private static Stopwatch sw = new Stopwatch();
        private readonly ITrainingDAL _trainingDAL;
        private readonly ITrainingItemDAL _trainingItemDAL;

        public TrainingB(ITrainingDAL trainingDAL, ITrainingItemDAL trainingItemDAL)
        {
            _trainingDAL = trainingDAL;
            _trainingItemDAL = trainingItemDAL;
        }

        public Training Get(int id)
        {
            return _trainingDAL.Get(id);
        }

        public void Update(Training t)
        {
            _trainingDAL.Update(t);
            ReacalculateValues(t.Id);
        }

        public List<Training> GetAllForGym(int gymId)
        {
            return _trainingDAL.GetAllForGym(gymId);
        }

        public List<Training> GetAllForGym(int gymId, DateTime start, DateTime end)
        {
            return _trainingDAL.GetAllForGym(gymId, start, end);
        }

        internal List<Training> GetAllForTrainer(int trainerId, DateTime start, DateTime end)
        {
            return _trainingDAL.GetAllForTrainer(trainerId, start, end);
        }

        //public  static void UpdateStatusBatch(DateTime start, DateTime end)
        //{
        //	using (TrainingContext db = new TrainingContext())
        //	{
        //		var ts = db.Trainings
        //			.Include(x => x.TrainingItems)
        //			.Where(x => x.StartTime > start && x.StartTime < end && x.StatusId == Status.willId);

        //		foreach (var x in ts)
        //		{
        //			if (x.StatusId == Status.willId)
        //			{
        //				if (x.TrainingItems.Count == 0)
        //				{
        //					x.StatusId = Status.canceledId;
        //					Update(x);
        //				}
        //				if (x.TrainingItems.Count != 0 && x.StartTime < DateTime.Now)
        //				{
        //					x.StatusId = Status.completedId;
        //					Update(x);
        //				}
        //			}
        //		}
        //	}
        //}

        public int GetPay(int count)
        {
            if (count == 0) return 0;
            if (count > 10) count = 10;

            return trainerPay[count];
        }

        internal int Add(Training training)
        {
            return _trainingDAL.Add(training);
        }

        /// <summary>
        /// Поля SeatsLeft, TrainerPay - вычисляемые
        /// </summary>
        public void ReacalculateValues(int id)
        {
            sw.Restart();

            var training = Get(id);
            if (training is null)
            {
                return;
            }

            //пришли на тренировку
            var onTraining = training.TrainingItems.Where(x =>
            x.StatusId == (int)TrainingItemStatus.yes);

            training.TrainerPay = GetPay(onTraining.Count());

            //записано на тренировку
            var signToTraining = training.TrainingItems.Where(x =>
                x.StatusId == (int)TrainingItemStatus.yes ||
                x.StatusId == (int)TrainingItemStatus.unKnown);

            training.SeatsTaken = signToTraining.Count();

            _trainingDAL.Update(training);

            sw.Stop();
            Debug.WriteLine($"Training ReacalculateValues {sw.ElapsedMilliseconds}");
        }


        public bool IsTimeFree(int gymId, DateTime startTime, DateTime endTime, int currentTrainingId)
        {
            var trainings = _trainingDAL.GetAllForGym(gymId, startTime.AddHours(-12), startTime.AddHours(12));

            foreach (var t in trainings)
            {
                if (startTime >= t.EndTime || t.StartTime >= endTime)
                {

                }
                else
                {
                    if (t.Id != currentTrainingId)
                        return false;
                }
            }

            return true;
        }

        //Записан ли человек на тренировку
        public bool IsAccountSigned(int trainingId, int accountId)
        {
            var training = _trainingDAL.Get(trainingId);

            return training.TrainingItems.Exists(x => x.AccountId == accountId);
        }

        //Есть ли места на тренировке
        public bool IsHaveSeat(int trainingId)
        {
            var training = _trainingDAL.Get(trainingId);

            return training.Seats - training.SeatsTaken > 0;
        }

        public void Cancel(int id)
        {
            Training t = Get(id);
            t.StatusId = (int)TrainingStatus.Canceled;
            Update(t);

            foreach (var ti in t.TrainingItems)
            {
                _trainingItemDAL.ChangeStatus(ti.Id, TrainingItemStatus.canceledByAdmin);
            }
        }

        internal void Restore(int id)
        {
            Training t = Get(id);
            t.StatusId = (int)TrainingStatus.Active;
            Update(t);
        }

        public void Delete(int id)
        {
            _trainingDAL.Delete(id);
        }

        private static readonly Dictionary<int, int> trainerPay = new Dictionary<int, int>()
        {
            { 1, 300 },
            { 2, 300 },
            { 3, 300 },
            { 4, 400 },
            { 5, 450 },
            { 6, 450 },
            { 7, 500 },
            { 8, 500 },
            { 9, 500 },
            { 10, 600 },
        };

        private static readonly Dictionary<int, int> trainerPaySynnyGroup = new Dictionary<int, int>()
        {
            { 1, 300 },
            { 2, 300 },
            { 3, 300 },
            { 4, 400 },
            { 5, 450 },
            { 6, 450 },
            { 7, 500 },
            { 8, 500 },
            { 9, 500 },
            { 10, 600 },
        };

        private static readonly Dictionary<int, int> trainerPaySynnyIndivid = new Dictionary<int, int>()
        {
            { 1, 475 },
            { 2, 750 },
        };

        private static readonly Dictionary<int, int> trainerCenterGroup = new Dictionary<int, int>()
        {
            { 1, 300 },
            { 2, 300 },
            { 3, 300 },
            { 4, 400 },
            { 5, 450 },
            { 6, 450 },
            { 7, 500 },
            { 8, 500 },
            { 9, 500 },
            { 10, 600 },
        };

        private static readonly Dictionary<int, int> trainerPayCenterIndivid = new Dictionary<int, int>()
        {
            { 1, 475 },
            { 2, 750 },
        };
    }
}