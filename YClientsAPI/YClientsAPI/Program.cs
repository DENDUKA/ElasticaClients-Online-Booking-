using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading;

namespace YClientsAPI
{
	public class Program
	{
		public static void Main()
		{
			//TrainingSynchronise();


			//YClientsAPI api = new YClientsAPI();
			//api.Authentication("79085513403", "753698", "sxwmygnagd8sbrrtpjpy");

			//string readText = File.ReadAllText(@"C:\Users\DENDUKA\Desktop\эластика\API\ClientsIdsSun.txt");

			//var ids = readText.Split(new[] { '\r', '\n', }, StringSplitOptions.RemoveEmptyEntries);

			//foreach (var x in ids)
			//{
			//	Account acc = new Account
			//	{
			//		Password = x,
			//		RoleId = 9
			//	};

			//	ElasticaClients.DAL.Data.AccountDAL.Add(acc);
			//}




			//var accs = ElasticaClients.DAL.Data.AccountDAL.GetAll();
			//var curs = accs.Where(x => x.Instagram == "!!!!!!!");


			//foreach (var x in curs)
			//{
			//	ElasticaClients.DAL.Data.AccountDAL.Delete(x.Id);
			//}


			//var cc = api.GetClient(current.Password, "151893");


			//current.Name = cc.Name;
			//current.Email = cc.Email;
			//current.Phone = cc.Phone;

			//var accByPhone =  ElasticaClients.DAL.Data.AccountDAL.GetByPhone(cc.Phone);

			//if (accByPhone != null)
			//{
			//	current.Instagram = "!!!!!!!";
			//}



			//System.Diagnostics.Debug.WriteLine(cc.Id);
			//System.Diagnostics.Debug.WriteLine(cc.Name);
			//System.Diagnostics.Debug.WriteLine(current.Email);



			//ElasticaClients.DAL.Data.AccountDAL.Update(current);

			Thread.Sleep(500);
			
		}

		public static void ActivitySync()
		{
			string[] companys = new string[] { "126060", "151893" };

			YClientsAPI api = new YClientsAPI();
		}

		public static void ClientsSynchronise()
		{
			string[] companys = new string[] { "126060", "151893" };

			YClientsAPI api = new YClientsAPI();


			api.SyncClientsInCompany(companys[0]);
			api.SyncClientsInCompany(companys[1]);
		}

		public static List<ClientJSON> GetYClients()
		{
			string[] companys = new string[] { "126060", "151893" };

			YClientsAPI api = new YClientsAPI();

			List<ClientJSON> Yclients = new List<ClientJSON>();

			int i = 0;

			foreach (var cn in companys)
			{
				var ids = api.GetAllClientsId(cn).OrderBy(x => int.Parse(x)).ToList();

				foreach (var x in ids)
				{
					Console.WriteLine($"{i}|{x}");
					Yclients.Add(api.GetClient(x, cn));
					i++;
				}
			}

			return Yclients;
		}

		public static void ClientPhoneSynchronise(List< Account> noFillingAccs)
		{
			string[] companys = new string[] { "126060", "151893" };

			YClientsAPI api = new YClientsAPI();
		}

		public static void RecordsSynchronise()
		{
			string[] companys = new string[] { "126060", "151893" };
			YClientsAPI api = new YClientsAPI();

			//api.SyncRecordsInCompany(companys[1], DateTime.Today.AddDays(0), DateTime.Today.AddDays(1));
			//Console.WriteLine("======");
			//api.SyncRecordsInCompany(companys[1], DateTime.Today.AddDays(0), DateTime.Today.AddDays(0));
			//Console.WriteLine("======");
			//api.SyncRecordsInCompany(companys[1], DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1));


			//api.SyncRecordsInCompany(companys[1], DateTime.Today.AddDays(-3), DateTime.Today.AddDays(-3));


			for (int i = 0; i < 1000; i++)
			{
				api.SyncRecordsInCompany(companys[0], DateTime.Today.AddDays(-i), DateTime.Today.AddDays(-i));
				Console.WriteLine("======");
			}
		}
	}
}