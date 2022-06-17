using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
	public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

		public void ReadAndAddAllClients()
		{
			YClientsAPI.Program.Main();
		}

		public void UnfreezeAllExpired()
		{
			SubscriptionB.BatchUnfreeze();
		}

		public void CloseSubscription()
		{
			SubscriptionB.BatchCloseSubscription();
		}

		public void RecalculateAllBonuses()
		{
			foreach (var x in AccountB.GetAll())
			{
				AccountB.ReacalculateBonuses(x.Id);
				Debug.WriteLine($"RecalculateAllBonuses {x.Id}");
			}			
		}

		public void ParceExcelClients()
		{
			var els = ParceFile();


			var accs = AccountB.GetAll();
			var subs = SubscriptionB.GetAll();
			var trs = TrainingB.GetAllForGym(3);

			foreach (var e in els)
			{
				var sa = accs.Find(x => x.Name == e.Name);

				if (sa != null)
				{
					e.acc = sa;
				}
				else
				{
					AccountB.Add(new Account() { Name = e.Name, RoleId = Role.clientId });
				}

				e.Dates.Sort();

				var sub = subs.Find(x => x.BuyDate == e.Dates[0]);

				if (sub != null)
				{
					e.sub = sub;
				}
				else
				{
					Subscription newsub = new Subscription()
					{
						AccountId = e.acc.Id,
						BranchId = 3,
						BuyDate = e.Dates[0],
						StatusId = (int)SubscriptionStatus.NotActivated,
					};
					SubscriptionDAL.Add(newsub);
				}

				foreach (var d in e.Dates)
				{
					var t = trs.Find(x => x.StartTime == d);

					if (t is null)
					{
						Training newT = new Training()
						{
							StartTime = d,
							GymId = 3,
							Seats = 10,
							Name = "TrainingFromExcel",
							StatusId = (int)TrainingStatus.Active,
							TrainerId = 24,
						};

						TrainingB.Add(newT);
					}

					trs = TrainingB.GetAllForGym(3);

					t = trs.Find(x => x.StartTime == d);

					var trainingItem = t.TrainingItems.Find(x => x.AccountId == e.acc.Id);

					if (trainingItem is null)
					{
						TrainingItem ti = new TrainingItem()
						{
							AccountId = e.acc.Id,
							SubscriptionId = e.sub.Id,
							TrainingId = t.Id,
						};

						TrainingItemB.Add(ti);
					}
				}
			}
		}

		public List<ElasticLine> ParceFile()
		{
			string filePath = @"C:\Users\DENDUKA\source\repos\ElasticClients\ElasticClients\obj\Debug\solnechny_1_1_1.xlsx";

			List<ElasticLine> els = new List<ElasticLine>();

			using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					do
					{
						while (reader.Read())
						{
							ElasticLine el = new ElasticLine();

							el.Name = reader.GetString(0);

							//string ss = reader.GetString(13);

							el.Count = reader.GetDouble(13);

							for (int i = 1; i <= 12; i++)
							{
								try
								{
									DateTime date = reader.GetDateTime(i);
									if (date != null)
									{
										el.Dates.Add(Convert.ToDateTime(date));
									}
								}
								catch (Exception)
								{ }
							}

							els.Add(el);

							//var x = reader.GetString(10);
						}
					} while (reader.NextResult());
				}
			}
			return els;
		}
    }

	public class ElasticLine
	{
		public Account acc;
		public string Name { get; set; }
		public List<DateTime> Dates = new List<DateTime>();
		public double Count;
		public Subscription sub;
	}
}