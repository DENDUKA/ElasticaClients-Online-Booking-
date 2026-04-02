using ElasticaClients.DAL.Entities;
using System.Collections.Generic;

namespace ElasticaClients.DAL.Data.Interfaces
{
    public interface IAppLogDAL
    {
        void Add(AppLog log);
        List<AppLog> GetPage(int page, int pageSize);
        int GetCount();
    }
}
