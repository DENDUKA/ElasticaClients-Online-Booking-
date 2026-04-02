using ElasticaClients.Logic;
using System;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    [Authorize(Roles = "admin")]
    public class LogController : Controller
    {
        private const int PageSize = 30;
        private readonly AppLogFileStore _logFileStore;

        public LogController(AppLogFileStore logFileStore)
        {
            _logFileStore = logFileStore;
        }

        public ActionResult Index(int page = 1)
        {
            int total = _logFileStore.GetCount();
            int totalPages = (int)Math.Ceiling((double)total / PageSize);

            if (page < 1) page = 1;
            if (totalPages > 0 && page > totalPages) page = totalPages;

            var logs = _logFileStore.GetPage(page, PageSize);

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View(logs);
        }
    }
}
