using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.DAL.Data
{
    public class AppLogDAL : IAppLogDAL
    {
        public void Add(AppLog log)
        {
            using (var db = new AppLogContext())
            {
                db.AppLogs.Add(log);
                db.SaveChanges();
            }
        }

        public List<AppLog> GetPage(int page, int pageSize)
        {
            using (var db = new AppLogContext())
            {
                return db.AppLogs
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        public int GetCount()
        {
            using (var db = new AppLogContext())
            {
                return db.AppLogs.Count();
            }
        }
    }
}
