using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using IronXL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public static class LazerParcer
	{

		static List<Subscription> Subscriptions; 
		static LazerParcer()
		{
			Subscriptions = ElasticaClients.Logic.SubscriptionB.GetAll();
		}

		public static void Run()
		{
			var result = new List<LazerExcelData>();
			var path = "D:\\LZ.xlsx";

			Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
			Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
			Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[3];
			Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;


			var t = xlRange.Cells[3, 1].Value2;
			var r = xlRange.Cells[3, 1].Text;


			var curData = new DateTime();

			for (int i = 2; i <= 1000; i++)
			{
				var data = xlRange.Cells[i, 1].Text.ToString();
				if (!string.IsNullOrEmpty(data))
				{
					curData = DateTime.Parse(data);
				}

				var number = xlRange.Cells[i, 2].Text.ToString();
				if (!string.IsNullOrEmpty(number))
				{
					var lazData = new LazerExcelData
					{
						Data = curData,
						Phone = number,

						Podm = ((xlRange.Cells[i, 3].Text.ToString()) as string).ToUpper().Contains("Д") ? 1 :
						((xlRange.Cells[i, 3].Text.ToString()) as string).ToUpper().Contains("Н") ? 0 : 2,
						FullFoot = ((xlRange.Cells[i, 4].Text.ToString()) as string).ToUpper().Contains("Д") ? 1 :
						((xlRange.Cells[i, 4].Text.ToString()) as string).ToUpper().Contains("Н") ? 0 : 2,
						Goleni = ((xlRange.Cells[i, 5].Text.ToString()) as string).ToUpper().Contains("Д") ? 1 :
						((xlRange.Cells[i, 5].Text.ToString()) as string).ToUpper().Contains("Н") ? 0 : 2,
						BikiniFull = ((xlRange.Cells[i, 6].Text.ToString()) as string).ToUpper().Contains("Д") ? 1 :
						((xlRange.Cells[i, 6].Text.ToString()) as string).ToUpper().Contains("Н") ? 0 : 2,
						HandFull = ((xlRange.Cells[i, 7].Text.ToString()) as string).ToUpper().Contains("Д") ? 1 :
						((xlRange.Cells[i, 7].Text.ToString()) as string).ToUpper().Contains("Н") ? 0 : 2,
						BikiniClass = ((xlRange.Cells[i, 8].Text.ToString()) as string).ToUpper().Contains("Д") ? 1 :
						((xlRange.Cells[i, 8].Text.ToString()) as string).ToUpper().Contains("Н") ? 0 : 2,
						Bedra = ((xlRange.Cells[i, 9].Text.ToString()) as string).ToUpper().Contains("Д") ? 1 :
						((xlRange.Cells[i, 9].Text.ToString()) as string).ToUpper().Contains("Н") ? 0 : 2,
						Areol = ((xlRange.Cells[i, 10].Text.ToString()) as string).ToUpper().Contains("Д") ? 1 :
						((xlRange.Cells[i, 10].Text.ToString()) as string).ToUpper().Contains("Н") ? 0 : 2,
						Face = ((xlRange.Cells[i, 11].Text.ToString()) as string).ToUpper().Contains("Д") ? 1 :
						((xlRange.Cells[i, 11].Text.ToString()) as string).ToUpper().Contains("Н") ? 0 : 2,
						Guba = ((xlRange.Cells[i, 12].Text.ToString()) as string).ToUpper().Contains("Д") ? 1 :
						((xlRange.Cells[i, 12].Text.ToString()) as string).ToUpper().Contains("Н") ? 0 : 2
					};

					result.Add(lazData);

					AddToBD(lazData);

					Console.WriteLine(lazData);
				}
			}
		}

		private static void AddToBD(LazerExcelData lazData)
		{
			var acc = FindOrAddAcc(lazData.Phone);

			var training = FindOrAddTraining(lazData.Data);



			var servises = GetServisesList(lazData);



			ElasticaClients.Logic.TrainingItemB.Add(
				new TrainingItem()
				{
					SubscriptionId = Subscriptions.First(x => x.StatusId == (int)SubscriptionStatus.Razovoe && x.AccountId == acc.Id).Id,
					TrainingId = training.Id,
					AccountId = acc.Id,
					Razovoe = true,
					StatusId = 1,
					StatusPayId = 1,
					ServisesList = servises,
				}
			);
			Console.WriteLine("!!! добавлен новый Тренировка");



		}

		private static string GetServisesList(LazerExcelData lazData)
		{
			string res = "";

			if (lazData.Podm == 1)
				res += " s8";

			if (lazData.FullFoot == 1)
				res += " s15";

			if (lazData.Goleni == 1)
				res += " s13";

			if (lazData.BikiniFull == 1)
				res += " s2";

			if (lazData.HandFull == 1)
				res += " s10";
			
			if (lazData.BikiniClass == 1)
				res += " s1";

			if (lazData.Bedra == 1)
				res += " s14";

			if (lazData.Areol == 1)
				res += " s3";

			if (lazData.Face == 1)
				res += " s24";

			if (lazData.Guba == 1)
				res += " s18";

			return res.Trim();
		}

		private static Training FindOrAddTraining(DateTime data)
		{
			Random r = new Random();
			var trainings = ElasticaClients.Logic.TrainingB.GetAllForGym(1);

			var training = trainings.FirstOrDefault(x=>x.StartTime == data);

			if (training is null)
			{
				int newtID = ElasticaClients.Logic.TrainingB.Add(new Training()
				{
					Seats = 1,
					GymId = 1,
					Duration = new TimeSpan(1, 0, 0),
					Name = "Процедура",
					StartTime = data.AddSeconds(r.Next(5000)),
					TrainerId = 5,
				});

				Console.WriteLine("!! добавлен новый Тренировка");

				return ElasticaClients.Logic.TrainingB.Get(newtID);
			}
			else
			{
				Console.WriteLine("!! Тренировка существует " + data.ToString());

			}

			trainings = ElasticaClients.Logic.TrainingB.GetAllForGym(1);

			return trainings.FirstOrDefault(x => x.StartTime == data);
		}

		private static Account FindOrAddAcc(string phone)
		{
			phone = phone.Substring(1);

			var curAcc = ElasticaClients.Logic.AccountB.GetByPhone(phone);

			if (curAcc is null)
			{
				ElasticaClients.Logic.AccountB.Add(new Account() { Phone = phone, RoleId = 9 });
				Console.WriteLine("!  добавлен новый Аккаунт");
			}
			else
			{
				Console.WriteLine("!  Аккаунт уже существует");
				return curAcc;
			}

			curAcc = ElasticaClients.Logic.AccountB.GetByPhone(phone);

			return curAcc;
		}
	}
}
