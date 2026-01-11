using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticaClients.DAL.Data.Interfaces
{
    public interface IBranchDAL
    {
        List<Branch> GetAll();
        Branch Get(int id);
        Task<List<Branch>> GetAllAsync();
    }
}
