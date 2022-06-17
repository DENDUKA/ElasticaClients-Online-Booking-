using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public static class SubscriptionMoves
	{
		public static void CreateOneTimeSubsForAllAccs()
		{
			var accs = AccountDAL.GetAll();

			foreach (var acc in accs)
			{
				foreach (var branch in BranchB.GetAll())
				{
					var subs = SubscriptionDAL.GetForAccount(acc.Id, branch.Id);

					var razov = subs.Where(x => x.Name == "Разовый").FirstOrDefault();

					if (razov == null)
					{
						Subscription sub = new Subscription();
						sub.Name = "Разовый";
						sub.BranchId = branch.Id;
						sub.BuyDate = DateTime.Today;
						sub.AccountId = acc.Id;

						SubscriptionDAL.Add(sub);
						Console.WriteLine("sub " + sub.Id.ToString() + " " + branch.Id);
					}
					else
					{
						Console.WriteLine("Существует");
					}
				}
			}
		}

		public static void RecalculateAllSubscription()
		{
			foreach (var x in SubscriptionDAL.GetAll())
			{
				SubscriptionB.RecalculateValues(x.Id);
				Console.WriteLine(x.Id);
			}
		}

		public static void SetRightSubscriptionStatus()
		{
			foreach (var sub in SubscriptionDAL.GetAll())
			{
				if (sub.Name == "Разовый" && sub.StatusId != (int)SubscriptionStatus.Razovoe)
				{
					sub.StatusId = (int)SubscriptionStatus.Razovoe;
					SubscriptionDAL.Update(sub);
					Console.WriteLine($"{sub.Id} Обновлено");
				}
				else
				{
					Console.WriteLine($"{sub.Id} Не требует обновления");
				}
			}
		}


		public static void SetRightSubWithBranch()
		{

			foreach (var ti in TrainingItemDAL.GetAll().OrderBy(x=>x.Id))
			{
				var training = TrainingDAL.Get(ti.TrainingId);

				var branchId = training.Gym.BranchId;


				var subs = SubscriptionDAL.GetForAccount(ti.AccountId, branchId);
				var razov = subs.Where(x => x.Name == "Разовый").FirstOrDefault();

				if (ti.SubscriptionId != razov.Id)
				{
					ti.SubscriptionId = razov.Id;
					TrainingItemDAL.Update(ti);
					Console.WriteLine($"{ti.Id} Обновлено");
				}
				else
				{
					Console.WriteLine($"{ti.Id} Не требуется");
				}
			}
		}


		public static void SetRightSubscriptionForAllTrainingItems()
		{
			var tis = TrainingItemDAL.GetAll();

			foreach (var ti in tis)
			{
				if (ti.Id == 14975)
				{ 
				}

				if (ti.Subscription.Name != "Разовый")
				{
					var subs = SubscriptionDAL.GetForAccount(ti.AccountId);
					var razov = subs.Where(x => x.Name == "Разовый" ).FirstOrDefault();

					if (razov != null)
					{
						ti.SubscriptionId = razov.Id;

						TrainingItemDAL.Update(ti);

						Console.WriteLine(ti.Id + " обновлено");
					}
					else
					{
						Console.WriteLine("Абонепмента не существует");
					}

				}
				else
				{
					Console.WriteLine(ti.Id + " не требует обновления");
				}
			}
		}
	}
}
