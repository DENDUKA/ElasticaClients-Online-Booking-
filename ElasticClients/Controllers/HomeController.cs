using ElasticaClients.Logic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return await Task.FromResult((ActionResult)View());
        }
    }
}