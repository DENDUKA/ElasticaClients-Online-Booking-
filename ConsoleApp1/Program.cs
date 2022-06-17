using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{

			AccountDAL.GetByPhoneStorageProcedure("92711606212");






			//EngelsAccounts.GetAllAccsInEngels();



			//Nakrutka.Go( new DateTime(2021,11,01), new DateTime(2022, 03, 30));

			//Console.ReadKey();


			//var activSubs = SubscriptionB.GetAll().Where(x => x.StatusId == (int)SubscriptionStatus.Activated).ToList();

			//List<Subscription> withError = new List<Subscription>();

			//foreach (var s in activSubs)
			//{
			//	var sub = SubscriptionB.Get(s.Id);

			//	if(sub.StatusId != (int)SubscriptionStatus.Activated)
			//	{
			//		continue;
			//	}

			//	if (sub.TrainingItems.Any())
			//	{
			//		var minDateStart = sub.TrainingItems.Min(x => x.Date);

			//		if (minDateStart < sub.ActivateDate)
			//		{
			//			withError.Add(sub);
			//		}
			//	}
			//	else
			//	{
			//		withError.Add(sub);
			//	}
			//}

			//foreach (var x in withError)
			//{
			//	Console.WriteLine(x.Id);
			//}


			//Console.ReadKey();

			//SubscriptionB.BatchActivateByTime();

			//Functions.CreateRazovoeForAll();

			//Excel ex = new Excel();
			//var subs = ex.Import();
			//var accs = ex.ImportAccs();

			//ex.toDBAccs(accs);

			//ex.ToBD(subs);


			//Console.ReadKey();

			//Excel ex = new Excel();
			//var clients = ex.ImportClients();

			//Functions.BindingPhoneInBD(clients);

			//var clients = YClientsAPI.Program.GetYClients();

			//Excel.FillExcelByClient(clients);

			//var noFillingAccs = AccountB.GetAll().Where(x => x.Phone == "0000000000").ToList();
			//YClientsAPI.Program.ClientPhoneSynchronise(noFillingAccs);

			//SubscriptionMoves.RecalculateAllSubscription();

			//SubscriptionMoves.SetRightSubscriptionStatus();

			//YClientsAPI.Program.ClientsSynchronise();

			//YClientsAPI.Program.RecordsSynchronise();

			//YClientsAPI.Program.ActivitySync();

			//var gs = ElasticaClients.DAL.Data.GymDAL.GetAll();
			//int i = 0;


			//using (AccountContext db = new AccountContext())
			//{
			//	Role roleAdmin = new Role() { Name = Role.admin };
			//	Role roleTrainer = new Role() { Name = Role.trainer };
			//	Role roleClient = new Role() { Name = Role.client };

			//	Account accAdmin = new Account() { Name = "Admin1", Email = "admin1@test.test", Instagram = "admin1", Role = roleAdmin };

			//	Account accTrainer1 = new Account() { Name = "Trainer1", Email = "trainer1@test.test", Instagram = "trainer1", Role = roleTrainer };
			//	Account accTrainer2 = new Account() { Name = "Trainer2", Email = "trainer2@test.test", Instagram = "trainer2", Role = roleTrainer };

			//	Account accClient1 = new Account() { Name = "Client1", Email = "client1@test.test", Instagram = "client1", Role = roleClient };
			//	Account accClient2 = new Account() { Name = "Client2", Email = "client2@test.test", Instagram = "client2", Role = roleClient };
			//	Account accClient3 = new Account() { Name = "Client3", Email = "client3@test.test", Instagram = "client3", Role = roleClient };
			//	Account accClient4 = new Account() { Name = "Client4", Email = "client4@test.test", Instagram = "client4", Role = roleClient };
			//	Account accClient5 = new Account() { Name = "Client5", Email = "client5@test.test", Instagram = "client5", Role = roleClient };
			//	Account accClient6 = new Account() { Name = "Client6", Email = "client6@test.test", Instagram = "client6", Role = roleClient };
			//	Account accClient7 = new Account() { Name = "Client7", Email = "client7@test.test", Instagram = "client7", Role = roleClient };
			//	Account accClient8 = new Account() { Name = "Client8", Email = "client8@test.test", Instagram = "client8", Role = roleClient };

			//	Gym gymSun = new Gym() { Name = "Солнечный" };
			//	Gym gymCentr = new Gym() { Name = "Центр" };

			//	Price price1 = new Price() { Name = "Lite (6)", Cost = 800, Count = 6 };
			//	Price price2 = new Price() { Name = "Summer (8)", Cost = 1000, Count = 8 };
			//	Price price3 = new Price() { Name = "Middle (10)", Cost = 1200, Count = 10 };
			//	Price price4 = new Price() { Name = "Full (14)", Cost = 1600, Count = 14 };

			//	Subscription sub1 = new Subscription() { Gym = gymSun, Account = accClient1, Price = price1 };
			//	Subscription sub2 = new Subscription() { Gym = gymSun, Account = accClient2, Price = price2 };
			//	Subscription sub3 = new Subscription() { Gym = gymSun, Account = accClient3, Price = price3 };
			//	Subscription sub4 = new Subscription() { Gym = gymSun, Account = accClient4, Price = price4 };
			//	Subscription sub5 = new Subscription() { Gym = gymCentr, Account = accClient5, Price = price3 };
			//	Subscription sub6 = new Subscription() { Gym = gymCentr, Account = accClient6, Price = price3 };
			//	Subscription sub7 = new Subscription() { Gym = gymCentr, Account = accClient7, Price = price2 };
			//	Subscription sub8 = new Subscription() { Gym = gymCentr, Account = accClient8, Price = price4 };
			//	Subscription sub9 = new Subscription() { Gym = gymCentr, Account = accClient6, Price = price2 };
			//	Subscription sub10 = new Subscription() { Gym = gymCentr, Account = accClient3, Price = price1 };
			//	Subscription sub11 = new Subscription() { Gym = gymCentr, Account = accClient2, Price = price3 };
			//	Subscription sub12 = new Subscription() { Gym = gymCentr, Account = accClient1, Price = price2 };

			//	Status statusClose = new Status() { Name = "Завершена" };
			//	Status statusFuture = new Status() { Name = "Будет" };
			//	Status statusCancel = new Status() { Name = "Отменена" };

			//	Training tr1 = new Training() { StartTime = new DateTime(2020, 07, 29, 12, 0, 0), Status = statusClose, Trainer = accTrainer1, Gym = gymSun, Seats = 7 };
			//	Training tr2 = new Training() { StartTime = new DateTime(2020, 07, 29, 14, 0, 0), Status = statusFuture, Trainer = accTrainer1, Gym = gymSun, Seats = 9 };
			//	Training tr3 = new Training() { StartTime = new DateTime(2020, 07, 29, 16, 0, 0), Status = statusCancel, Trainer = accTrainer2, Gym = gymCentr, Seats = 8 };
			//	Training tr4 = new Training() { StartTime = new DateTime(2020, 07, 29, 18, 0, 0), Status = statusFuture, Trainer = accTrainer2, Gym = gymCentr, Seats = 10 };

			//	TrainingItem ti1 = new TrainingItem() { Account = accClient1, Subscription = sub1, Training = tr1 };
			//	TrainingItem ti2 = new TrainingItem() { Account = accClient2, Subscription = sub2, Training = tr2 };
			//	TrainingItem ti3 = new TrainingItem() { Account = accClient3, Subscription = sub3, Training = tr2 };
			//	TrainingItem ti4 = new TrainingItem() { Account = accClient1, Subscription = sub12, Training = tr4 };
			//	TrainingItem ti5 = new TrainingItem() { Account = accClient2, Subscription = sub11, Training = tr4 };
			//	TrainingItem ti6 = new TrainingItem() { Account = accClient3, Subscription = sub10, Training = tr3 };
			//	TrainingItem ti7 = new TrainingItem() { Account = accClient1, Subscription = sub1, Training = tr1 };
			//	TrainingItem ti8 = new TrainingItem() { Account = accClient2, Subscription = sub2, Training = tr2 };
			//	TrainingItem ti9 = new TrainingItem() { Account = accClient3, Subscription = sub3, Training = tr1 };
			//	TrainingItem ti10 = new TrainingItem() { Account = accClient1, Subscription = sub1, Training = tr1 };
			//	TrainingItem ti11 = new TrainingItem() { Account = accClient2, Subscription = sub11, Training = tr3 };
			//	TrainingItem ti12 = new TrainingItem() { Account = accClient3, Subscription = sub10, Training = tr4 };

			//	accClient1.Subscriptions.AddRange(new Subscription[] { sub1, sub12 });
			//	accClient2.Subscriptions.AddRange(new Subscription[] { sub2, sub11 });
			//	accClient3.Subscriptions.AddRange(new Subscription[] { sub3, sub10 });
			//	accClient4.Subscriptions.AddRange(new Subscription[] { sub4 });
			//	accClient5.Subscriptions.AddRange(new Subscription[] { sub5 });
			//	accClient6.Subscriptions.AddRange(new Subscription[] { sub6, sub9 });
			//	accClient7.Subscriptions.AddRange(new Subscription[] { sub7 });
			//	accClient8.Subscriptions.AddRange(new Subscription[] { sub8 });

			//	accClient1.TrainingItems.AddRange(new TrainingItem[] { ti1, ti4, ti7, ti10 });
			//	accClient2.TrainingItems.AddRange(new TrainingItem[] { ti2, ti5, ti8, ti11 });
			//	accClient3.TrainingItems.AddRange(new TrainingItem[] { ti3, ti6, ti9, ti12 });

			//	tr1.TrainingItems.AddRange(new TrainingItem[] { ti1, ti7, ti9, ti10 });
			//	tr2.TrainingItems.AddRange(new TrainingItem[] { ti2, ti3, ti8 });
			//	tr3.TrainingItems.AddRange(new TrainingItem[] { ti6, ti11 });
			//	tr4.TrainingItems.AddRange(new TrainingItem[] { ti4, ti5, ti12 });

			//	db.Accounts.AddRange(new Account[] { accAdmin, accTrainer1, accTrainer2, accClient1, accClient2, accClient3, accClient4, accClient5, accClient6, accClient7, accClient8 });
			//	//db.SaveChanges();
			//}


		}
	}
}