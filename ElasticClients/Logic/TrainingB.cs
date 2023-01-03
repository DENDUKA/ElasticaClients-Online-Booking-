using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace ElasticaClients.Logic
{
	public static class TrainingB
	{
		private static Stopwatch sw = new Stopwatch();

		public static Training Get(int id)
		{
			return TrainingDAL.Get(id);
		}

		public static void Update(Training t)
		{
			TrainingDAL.Update(t);
			ReacalculateValues(t.Id);
		}

		public static List<Training> GetAllForGym(int gymId)
		{
			return TrainingDAL.GetAllForGym(gymId);
		}

		public static List<Training> GetAllForGym(int gymId, DateTime start, DateTime end)
		{
			return TrainingDAL.GetAllForGym(gymId, start, end);
		}

		internal static List<Training> GetAllForTrainer(int trainerId, DateTime start, DateTime end)
		{
			return TrainingDAL.GetAllForTrainer(trainerId, start, end);
		}

		//public static void UpdateStatusBatch(DateTime start, DateTime end)
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

		public static int GetPay(int count)
		{
			if (count == 0) return 0;
			if (count < 4) count = 4;
			if (count > 10) count = 10;

			return trainerPay[count];
		}

		internal static int Add(Training training)
		{
			return TrainingDAL.Add(training);
		}

		/// <summary>
		/// Поля SeatsLeft, TrainerPay - вычисляемые
		/// </summary>
		public static void ReacalculateValues(int id)
		{
			sw.Restart();

			var training = TrainingB.Get(id);
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

			TrainingDAL.Update(training);

			sw.Stop();
			Debug.WriteLine($"Training ReacalculateValues {sw.ElapsedMilliseconds}");
		}


		public static bool IsTimeFree(int gymId, DateTime startTime, DateTime endTime, int currentTrainingId)
		{
			var trainings = TrainingDAL.GetAllForGym(gymId, startTime.AddHours(-12), startTime.AddHours(12));

			foreach (var t in trainings)
			{
				if (startTime >= t.EndTime || t.StartTime >= endTime)
				{

				}
				else
				{
					if(t.Id != currentTrainingId)
					return false;
				}
			}

			return true;
		}

		//Записан ли человек на тренировку
		public static bool IsAccountSigned(int trainingId, int accountId)
		{
			var training = TrainingDAL.Get(trainingId);

			return training.TrainingItems.Exists(x => x.AccountId == accountId);
		}

		//Есть ли места на тренировке
		public static bool IsHaveSeat(int trainingId)
		{
			var training = TrainingDAL.Get(trainingId);

			return training.Seats - training.SeatsTaken > 0;
		}

		public static void Cancel(int id)
		{
			Training t = Get(id);
			t.StatusId = (int)TrainingStatus.Canceled;
			Update(t);

			foreach (var ti in t.TrainingItems)
			{
				TrainingItemB.ChangeStatus(ti.Id, TrainingItemStatus.canceledByAdmin);
			}
		}

		internal static void Restore(int id)
		{
			Training t = Get(id);
			t.StatusId = (int)TrainingStatus.Active;
			Update(t);
		}

		public static void Delete(int id)
		{
			TrainingDAL.Delete(id);
		}

		private static readonly Dictionary<int, int> trainerPay = new Dictionary<int, int>()
		{
			{ 4, 300 },
			{ 5, 350 },
			{ 6, 400},
			{ 7, 400},
			{ 8, 500},
			{ 9, 500},
			{ 10, 500},
		};

        private static readonly Dictionary<int, int> trainerPaySynnyGroup = new Dictionary<int, int>()
        {
            { 1, 300 },
            { 2, 300 },
            { 3, 300 },
            { 4, 350 },
            { 5, 400 },
            { 6, 450 },
            { 7, 450 },
            { 8, 500 },
            { 9, 500 },
            { 10, 600 },
        };

        private static readonly Dictionary<int, int> trainerPaySynnyIndivid = new Dictionary<int, int>()
        {
            { 1, 400 },
            { 2, 500 },
        };

        private static readonly Dictionary<int, int> trainerCenterGroup = new Dictionary<int, int>()
		{
			{ 1, 300 },
			{ 2, 300 },
			{ 3, 300 },
			{ 4, 350 },
			{ 5, 350 },
			{ 6, 400 },
			{ 7, 450 },
			{ 8, 500 },
			{ 9, 500 },
			{ 10, 500 },
		};

        private static readonly Dictionary<int, int> trainerPayCenterIndivid = new Dictionary<int, int>()
        {
            { 1, 400 },
            { 2, 500 },
        };
    }
}