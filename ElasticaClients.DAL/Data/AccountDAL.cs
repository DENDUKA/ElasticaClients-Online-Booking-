using ElasticaClients.DAL.Cache;
using ElasticaClients.DAL.Entities;
using ElasticaClients.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ElasticaClients.DAL.Data
{
	public static class AccountDAL
	{
		private static Stopwatch sw = new Stopwatch();
		private static List<Account> accounts = new List<Account>();

		private static bool IsAccsLoaded = false;

		public static void GetByPhoneStorageProcedure(string phone)
		{
			using (AccountContext db = new AccountContext())
			{
				System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@phone", phone);
				var account = db.Database.SqlQuery<Account>("MyProcedure @phone", param).ToList();

				
				foreach (var acc in account)
				{
					Debug.WriteLine("{0} - {1}", acc.Name, acc.Id);
				}

			}
		}


		public static Account Get(int id)
		{
			sw.Restart();

			Account acc;
			using (AccountContext dbacc = new AccountContext())
			{
				acc = dbacc.Accounts.Where(a => a.Id == id)
					.Include(x => x.Role)
					.Include(x => x.Subscriptions)
					.Include(x => x.Subscriptions.Select(y => y.Branch))
					.Include(x => x.Subscriptions.Select(y => y.FreezeSubscriptionList))
					.Include(x => x.Subscriptions.Select(y => y.TrainingItems.Select(z => z.Training)))
					.Include(x => x.Trainings)
					//.Include(x => x.Trainings.Select(y => y.Gym))
					//.Include(x => x.Trainings.Select(y => y.TrainingItems))
					.FirstOrDefault();
			}

			sw.Stop();
			Debug.WriteLine($"Account Get(int id) {sw.ElapsedMilliseconds}");

			return acc;
		}

		public static List<Account> GetWithFilter(int page, int count, string filter)
		{
			IsLoaded();

			return accounts
				.Where(x => x.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
				.Skip((page - 1) * count)
				.Take(count)
				.ToList();			
		}

		public static async Task<List<Account>> GetWithFilterAsync(int page, int count, string filter)
		{
			IsLoaded();

            return await Task.Run(() => accounts
				.Where(x => x.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
				.Skip((page - 1) * count)
				.Take(count)
				.ToList());
		}

        public static List<Account> GetAll()
		{
			sw.Restart();

			IsLoaded();

			sw.Stop();
			Debug.WriteLine($"List<Account> GetAll() {sw.ElapsedMilliseconds}");

			return accounts;
		}

		public static Dictionary<int, Account> GetAllLight()
		{
			sw.Restart();

			Dictionary<int, Account> accs;
			using (AccountContext db = new AccountContext())
			{
				accs = db.Accounts
				   .Include(x => x.Role)
				   .ToDictionary(x => x.Id, x => x);
			}

			sw.Stop();
			Debug.WriteLine($"Dictionary<int, Account> GetAllLight() {sw.ElapsedMilliseconds}");

			return accs;
		}

		public static Account GetTrainer(int id)
		{
			sw.Restart();

			Account acc;
			using (AccountContext dbacc = new AccountContext())
			{
				acc = dbacc.Accounts.Where(a => a.Id == id)
					.Include(x => x.Role)
					.Include(x => x.Subscriptions)
					.Include(x => x.Subscriptions.Select(y => y.Branch))
					//.Include(x => x.Subscriptions.Select(y => y.TrainingItems))
					//.Include(x => x.Trainings)
					//.Include(x => x.Trainings.Select(y => y.Gym))
					//.Include(x => x.Trainings.Select(y => y.TrainingItems))
					.FirstOrDefault();
			}

			sw.Stop();
			Debug.WriteLine($"Account GetTrainer(int id) {sw.ElapsedMilliseconds}");

			return acc;
		}

		public static Account Get(int id, DateTime trainingsFrom, DateTime trainingsTo)
		{
			sw.Restart();

			Account acc = null;
			using (AccountContext dbacc = new AccountContext())
			{
				acc = dbacc.Accounts
					.Include(x => x.Role)
					.Include(x => x.Trainings.Select(y => y.TrainingItems))
					.Include(x => x.Subscriptions.Select(y => y.TrainingItems.Select(z => z.Training)))
					.Include(x => x.Subscriptions.Select(y => y.Branch))
					.Include(x => x.Subscriptions.Select(y => y.FreezeSubscriptionList))
					.FirstOrDefault(a => a.Id == id);


				//.Include(x => x.Trainings.Select(y => y.Gym))
				//.Include(x => x.Trainings.Select(y => y.TrainingItems))

				var ts = dbacc.Accounts.Where(a => a.Id == id)
					.Select(x => x.Trainings.Where(y => y.StartTime > trainingsFrom && y.StartTime < trainingsTo))
					.FirstOrDefault().ToList();

				acc.Trainings = ts;
			}

			sw.Stop();
			Debug.WriteLine($"Account Get(int id, DateTime trainingsFrom, DateTime trainingsTo) {sw.ElapsedMilliseconds}");

			return acc;
		}

		public static Account GetLite(int id)
		{
			sw.Restart();

			var account =  AccountCache.GetLite(id);

			sw.Stop();
			Debug.WriteLine($"Account GetLite {sw.ElapsedMilliseconds}");

			return account;
		}

		public static Account FindByPassword(string password)
		{
			sw.Restart();

			Account acc = null;
			using (AccountContext dbacc = new AccountContext())
			{
				acc = dbacc.Accounts
					.Include(x => x.Role)
					.Include(x => x.Trainings.Select(y => y.TrainingItems))
					.Include(x => x.Subscriptions.Select(y => y.TrainingItems.Select(z => z.Training)))
					.FirstOrDefault(a => a.Password == password);
			}

			sw.Stop();
			Debug.WriteLine($"FindByPassword(string password) {sw.ElapsedMilliseconds}");

			return acc;
		}

		public static void Delete(int id)
		{
			sw.Restart();

			using (AccountContext db = new AccountContext())
			{
				db.Accounts.Where(a => a.Id == id).Delete();

				//Account acc = new Account() { Id = id };
				//db.Accounts.Attach(acc);
				//db.Accounts.Remove(acc);
				//db.SaveChanges();
			}

			sw.Stop();
			Debug.WriteLine($"Delete(int id) {sw.ElapsedMilliseconds}");

			IsAccsLoaded = false;

			AccountCache.AccontChanged();
		}

		public static void Add(Account acc)
		{
			sw.Restart();

			using (AccountContext db = new AccountContext())
			{
				db.Accounts.Add(acc);
				db.SaveChanges();
			}

			sw.Stop();
			Debug.WriteLine($"Add(Account acc) {sw.ElapsedMilliseconds}");

			IsAccsLoaded = false;

			AccountCache.AccontChanged();
		}

		public static void UpdateBonuses(Account acc)
		{
			sw.Restart();

			using (AccountContext db = new AccountContext())
			{
				db.Accounts.Attach(acc);

				db.Entry(acc).State = EntityState.Unchanged;

				db.Entry(acc).Property("Bonuses").IsModified = true;

				db.Accounts.AddOrUpdate();
				db.SaveChanges();
			}

			sw.Stop();
			Debug.WriteLine($"UpdateBonuses(Account acc) {sw.ElapsedMilliseconds}");
		}

		public static void Update(Account acc)
		{
			sw.Restart();

			using (AccountContext db = new AccountContext())
			{
				db.Accounts.Attach(acc);

				db.Entry(acc).State = EntityState.Modified;

				if (string.IsNullOrEmpty(acc.Password))
				{
					db.Entry(acc).Property("Password").IsModified = false;
				}

				db.Accounts.AddOrUpdate();
				db.SaveChanges();
			}

			sw.Stop();
			Debug.WriteLine($"Update(Account acc) {sw.ElapsedMilliseconds}");

			IsAccsLoaded = false;

			AccountCache.AccontChanged();
		}

		public static Account GetByPhone(string phone)
		{
			sw.Restart();

			using (AccountContext db = new AccountContext())
			{
				return db.Accounts
					.Include(x => x.Role)
					.FirstOrDefault(x => x.Phone.Equals(phone));
			}

			sw.Stop();
			Debug.WriteLine($"Account GetByPhone(string phone) {sw.ElapsedMilliseconds}");
		}

		private static void IsLoaded()
		{
			if (!IsAccsLoaded)
			{
				using (AccountContext db = new AccountContext())
				{
					accounts = db.Accounts
						.Include(x => x.Role)
						.ToList();
				}

				IsAccsLoaded = true;
			}
		}
	}
}