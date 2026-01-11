using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using ElasticaClients.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    public class IncomeController : Controller
    {
        private readonly IncomeB _incomeB;
        private readonly BranchB _branchB;
        private readonly ExcelGenerator _excelGenerator;
        private readonly TrainingB _trainingB;
        private readonly SubscriptionB _subscriptionB;
        private readonly TrainingItemB _trainingItemB;

        public IncomeController(IncomeB incomeB, BranchB branchB, ExcelGenerator excelGenerator, TrainingB trainingB, SubscriptionB subscriptionB, TrainingItemB trainingItemB)
        {
            _incomeB = incomeB;
            _branchB = branchB;
            _excelGenerator = excelGenerator;
            _trainingB = trainingB;
            _subscriptionB = subscriptionB;
            _trainingItemB = trainingItemB;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index(int branchId, int year = -1, int month = -1)
        {
            if (year == -1 || month == -1)
            {
                year = DateTime.Today.Year;
                month = DateTime.Today.Month;
            }

            DateTime start = new DateTime(year, month, 1);
            DateTime end = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

            ViewData["branchId"] = branchId;
            ViewData["DateTime"] = start;
            ViewData["DateFormat"] = $"{year} - {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}";
            var model = new IncomeTotalModel(_incomeB, _branchB, _trainingB, _subscriptionB, _trainingItemB, branchId, start, end);

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Report(int branchId, int year, int month)
        {
            var path = await _excelGenerator.MonthReportAsync(branchId, year, month);

            var fileBytes = System.IO.File.ReadAllBytes(path);

            var branch = _branchB.GetAll().Where(x => x.Id == branchId).First();
            var fileName = $"{branch.Name}.{month}.{year}.xlst";

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create(int branchId)
        {
            IncomeModel model = new IncomeModel
            {
                BranchId = branchId,
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(IncomeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _incomeB.Add((Income)model);

                    return RedirectToAction("Index", new { BranchId = model.BranchId });
                }

                ViewData["BranchId"] = model.BranchId;
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            var model = IncomeModel.Get(_incomeB, id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, IncomeModel model)
        {
            if (ModelState.IsValid)
            {
                _incomeB.Update((Income)model);

                return RedirectToAction("Index", new { BranchId = model.BranchId });
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                Income inc = _incomeB.Get(id);
                _incomeB.Delete(id);

                return RedirectToAction("Index", new { BranchId = inc.BranchId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Gym");
            }
        }
    }
}