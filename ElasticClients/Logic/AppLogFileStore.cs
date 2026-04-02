using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace ElasticaClients.Logic
{
    public class AppLogFileStore
    {
        private static readonly object _lock = new object();
        private static readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        private static string FilePath =>
            HostingEnvironment.MapPath("~/App_Data/applogs.json");

        public void Add(AppLog log)
        {
            lock (_lock)
            {
                var dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var line = _serializer.Serialize(log);
                File.AppendAllText(FilePath, line + Environment.NewLine);
            }
        }

        public List<AppLog> GetPage(int page, int pageSize)
        {
            return ReadAll()
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetCount()
        {
            return ReadAll().Count;
        }

        private List<AppLog> ReadAll()
        {
            lock (_lock)
            {
                if (!File.Exists(FilePath))
                    return new List<AppLog>();

                var logs = new List<AppLog>();
                foreach (var line in File.ReadAllLines(FilePath))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        logs.Add(_serializer.Deserialize<AppLog>(line));
                }
                return logs;
            }
        }
    }
}
