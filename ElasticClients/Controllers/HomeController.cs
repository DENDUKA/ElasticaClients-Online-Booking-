using ElasticaClients.Logic;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
	public class HomeController : Controller
    {
        public ActionResult Index()
        {
			Start();
            return View();
        }

		private void Start()
		{
			new Batches();
		}
    }
}