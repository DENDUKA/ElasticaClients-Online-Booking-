using ElasticaClients.Logic;
using ElasticaClients.Models;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    public class GymController : Controller
    {
        private readonly GymB _gymB;

        public GymController(GymB gymB)
        {
            _gymB = gymB;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index(int branchId)
        {
            return null;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            var model = (GymModel)_gymB.Get(id);

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(GymModel model)
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}