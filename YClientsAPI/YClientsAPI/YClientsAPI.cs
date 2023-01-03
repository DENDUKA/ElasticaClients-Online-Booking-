using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YClientsAPI.JSON_Objects;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using YClientsAPI.JSON_Objects.Records;
using YClientsAPI.JSON_Objects.Activity;
using Newtonsoft.Json.Linq;
using ElasticaClients.DAL.Accessory;

namespace YClientsAPI
{
	public class YClientsAPI
	{
		private Auth Auth { get; set; }
		private string Partner_token { get; set; }

		private Dictionary<string, int> YclientBranchIds = new Dictionary<string, int>() { { "126060", 1 }, { "151893", 2 } };

		//subscription 153 - centr 155 - sun
		private Dictionary<string, int> YclientSubscriptionIds = new Dictionary<string, int>() { { "126060", 153 }, { "151893", 155 } };

		public YClientsAPI()
		{
			Authentication("79085513403", "7991487", "sxwmygnagd8sbrrtpjpy");
		}

		public void Authentication(string login, string pass, string partner_token)
		{
			Partner_token = partner_token;

			string url = $"https://api.yclients.com/api/v1/auth?login={login}&password={pass}";

			var client = new RestClient(url);

			var request = new RestRequest
			{
				RequestFormat = DataFormat.Json,
				Method = Method.POST,
			};
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Authorization", $"Bearer {Partner_token}");

			var r = client.Execute(request);

			Auth = JsonConvert.DeserializeObject<Auth>(r.Content);
		}

		public ClientJSON GetClient(string id, string companyId)
		{
			string url = $"https://api.yclients.com/api/v1/client/{companyId}/{id}";

			var client = new RestClient(url);
			var request = new RestRequest()
			{
				RequestFormat = DataFormat.Json,
				Method = Method.GET,
			};

			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Authorization", $"Bearer {Partner_token}, User {Auth.User_token}");

			var r = client.Execute(request);

			//Client c = JsonConvert.DeserializeObject<Client>(r.Content);

			return JsonConvert.DeserializeObject<ClientJSON>(r.Content);
		}



		public List<Account> SyncClientsInCompany(string companyId)
		{
			List<Account> accounts = new List<Account>();

			var clientsIds = GetAllClientsId(companyId);

			int i = 0;

			foreach (var x in clientsIds)
			{
				var client = GetClient(x, companyId);

				//Console.WriteLine($"{i++} {client.Id} {client.Phone}");

				var clientInBD = AccountDAL.FindByPassword(client.Id.ToString());
				if (clientInBD != null)
				{
					Console.WriteLine($"Существует {client.Id.ToString()}");
					if (clientInBD.Email != client.Email ||
						clientInBD.Name != client.Name ||
						clientInBD.Phone != client.Phone)
					{
						Console.WriteLine("Не совпадает");

						clientInBD.Email = client.Email;
						clientInBD.Name = client.Name;
						clientInBD.Phone = client.Phone;

						AccountDAL.Update(clientInBD);
					}

					accounts.Add(clientInBD);
				}
				else
				{
					Console.WriteLine("Не существует");
					Account account = new Account()
					{
						Password = client.Id,
						Email = client.Email,
						Name = client.Name,
						Phone = client.Phone,
						RoleId = Role.clientId,
					};

					AccountDAL.Add(account);

					accounts.Add(account);

					Console.WriteLine("Добавлен");
				}

				i++;
				Console.WriteLine(i);
			}

			return accounts;
		}

		public List<string> GetAllClientsId(string companyId)
		{
			int page = 0;
			int countOnPage = 200;
			int countAll = int.MaxValue;

			List<string> result = new List<string>();

			string url = $"https://api.yclients.com/api/v1/company/{companyId}/clients/search";




			while (countAll > result.Count)
			{
				var client = new RestClient(url);
				var request = new RestRequest()
				{
					RequestFormat = DataFormat.Json,
					Method = Method.POST,
				};

				request.AddHeader("Accept", "application/vnd.yclients.v2+json");
				request.AddHeader("Content-Type", "application/json");
				request.AddHeader("Authorization", $"Bearer {Partner_token}, User {Auth.User_token}");


				page++;

				var clientFilter = new ClientsSearch(page, countOnPage);

				string json = JsonConvert.SerializeObject(clientFilter, Formatting.Indented);

				request.AddJsonBody(json);

				var r = client.Execute(request);

				var requestResult = JsonConvert.DeserializeObject<ClientsSearchResult>(r.Content);

				if (requestResult.success)
				{
					result.AddRange(requestResult.data.Select(x => x.Id));
					countAll = requestResult.meta.total_count;
				}
				else
				{
					return result;
				}
				Console.WriteLine($"{page} {countOnPage} {result.Count}");
			}


			return result;
		}

		public List<string> GetAllClientsIdv2(string companyId)
		{
			int page = 0;
			int countOnPage = 200;
			int countAll = int.MaxValue;

			List<string> result = new List<string>();

			string url = $"https://api.yclients.com/api/v1/company/{companyId}/clients/search";

			var client = new RestClient(url);
			var request = new RestRequest()
			{
				RequestFormat = DataFormat.Json,
				Method = Method.POST,
			};

			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Authorization", $"Bearer {Partner_token}, User {Auth.User_token}");


			while (countAll > result.Count)
			{
				page++;

				var clientFilter = new ClientsSearch(page, countOnPage);

				string json = JsonConvert.SerializeObject(clientFilter, Formatting.Indented);

				request.AddJsonBody(json);

				var r = client.Execute(request);

				var requestResult = JsonConvert.DeserializeObject<ClientsSearchResult>(r.Content);

				if (requestResult.success)
				{
					result.AddRange(requestResult.data.Select(x => x.Id));
					countAll = requestResult.meta.total_count;
				}
				else
				{
					return result;
				}
				Console.WriteLine($"{page} {countOnPage} {result.Count}");
			}


			return result;
		}
		#region Activity

		public ActivityJSON GetActivity(string companyId, string activityId)
		{
			string url = $"https://api.yclients.com/api/v1/activity/{companyId}/{activityId}";

			var client = new RestClient(url);
			var request = new RestRequest()
			{
				RequestFormat = DataFormat.Json,
				Method = Method.GET,
			};

			//request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Authorization", $"Bearer {Partner_token}, User {Auth.User_token}");

			var r = client.Execute(request);

			var requestResult = JsonConvert.DeserializeObject<ActivityJSON>(JObject.Parse(r.Content)["data"].ToString());

			return requestResult;
		}

		internal Training SyncActivity(RecordsSearchResult rsr)
		{
			string activityId = rsr.activity_id;
			string companyId = rsr.company_id.ToString();

			if (activityId == "0") // индивид
			{
				return SyncIndivid(companyId, rsr);
			}

			var activityJSON = GetActivity(companyId, activityId);

			if (activityJSON is null)
			{
			}
			return SyncTraining(companyId, activityJSON);
		}

		public Training SyncIndivid(string branchId, RecordsSearchResult rsr)
		{
			var t = TrainingDAL.GetIndivid(rsr.date, rsr.client.Id);

			if (t != null)
			{
				Console.WriteLine("Индивид существует");
				return t;
			}

			var gym = GymDAL.GetByName(YclientBranchIds[branchId], rsr.staff.name);

			Training training = new Training
			{
				YClientId = 0,
				Name = "Индивид",
				StartTime = rsr.date,
				Seats = 1,
				GymId = gym.Id,
				StatusId = (int)TrainingStatus.Active,
				TrainerId = 24,
			};

			TrainingDAL.Add(training);

			Console.WriteLine($"Индивид добавлен : {training.Name}");

			return training;
		}

		public Training SyncTraining(string branchId, ActivityJSON activity)
		{
			var t = TrainingDAL.GetByYclintsId(activity.id);

			if (t != null)
			{
				Console.WriteLine($"Тренировка существует {activity.service.title}") ;
				return t;
			}

			var gym = GymDAL.GetByName(YclientBranchIds[branchId], activity.staff.name);

			Training training = new Training
			{
				YClientId = activity.id,
				Name = activity.service.title,
				StartTime = activity.date,
				Seats = activity.capacity,
				GymId = gym.Id,
				StatusId = (int)TrainingStatus.Active,
				TrainerId = 24
			};

			TrainingDAL.Add(training);

			Console.WriteLine($"Тренировка добавлена : {training.Name}");
			return training;
		}

		#endregion

		public void SyncRecordsInCompany(string companyId, DateTime start, DateTime end)
		{
			int page = 1;
			int countOnPage = 100;

			string url = $"https://api.yclients.com/api/v1/records/{companyId}";

			var client = new RestClient(url);

			var request = new RestRequest()
			{
				RequestFormat = DataFormat.Json,
				Method = Method.GET,
			};

			string dateStartStr = start.ToString("yyy-MM-dd");
			string dateEndStr = end.ToString("yyy-MM-dd");

			string cDateStartStr = new DateTime(1800,1,1).ToString("yyy-MM-dd");
			string cDateEndStr = new DateTime(3000, 1, 1).ToString("yyy-MM-dd");

			//request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Authorization", $"Bearer {Partner_token}, User {Auth.User_token}");

			request.AddParameter("page", page);
			request.AddParameter("count", countOnPage);
			request.AddParameter("start_date", dateStartStr);
			request.AddParameter("end_date", dateEndStr);
			request.AddParameter("c_start_date", cDateStartStr);
			request.AddParameter("c_end_date", cDateEndStr);
			request.AddParameter("status", -2);

			var r = client.Execute(request);

			var json = JObject.Parse(r.Content)["data"].ToString();

			var requestResult = JsonConvert.DeserializeObject<List<RecordsSearchResult>>(json);


			foreach (var x in requestResult)
			{
				var tiFromBD = TrainingItemDAL.GetByYClients(x.id);
				if (tiFromBD is null)
				{
					if (x.client.Id is null)
					{
						break;
					}

					var acc = AccountFromJSON(x.client);

					var tr = SyncActivity(x);

					TrainingItem ti = new TrainingItem
					{
						YClientsId = x.id,
						AccountId = acc.Id,
						SubscriptionId = YclientSubscriptionIds[companyId],
						TrainingId = tr.Id
					};

					TrainingItemDAL.Add(ti);
					Console.WriteLine($"{x.date} TrainingItem добавлен");
				}
				else
				{
					Console.WriteLine($"{x.date} TrainingItem существует");
				}
			}
		}

		private Account AccountFromJSON(ClientJSON client)
		{
			var account = AccountDAL.FindByPassword(client.Id.ToString());
			if (account is null)
			{
				var acc = new Account
				{
					Name = client.Name,
					Password = client.Id,
					Phone = client.Phone,
					Email = client.Email,
					RoleId = Role.clientId
				};

				AccountDAL.Add(acc);
				Console.WriteLine($"Аккаунт добавлен : {acc.Name}");
				return acc;
			}

			Console.WriteLine("Аккаунт в базе");
			return account;
		}
	}
}
