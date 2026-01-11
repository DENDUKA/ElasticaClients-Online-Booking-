using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using ElasticaClients.Models;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    public class TrainingController : Controller
    {
        private readonly TrainingB _trainingB;
        private readonly GymB _gymB;
        private readonly TrainingItemB _trainingItemB;

        public TrainingController(TrainingB trainingB, GymB gymB, TrainingItemB trainingItemB)
        {
            _trainingB = trainingB;
            _gymB = gymB;
            _trainingItemB = trainingItemB;
        }

        public ActionResult Index(int gymid)
        {
            var model = (GymModel)_gymB.Get(gymid);

            return View(model);
        }

        public ActionResult Details(int id)
        {
            TrainingModel model = (TrainingModel)_trainingB.Get(id);

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create(int gymid)
        {
            var gym = _gymB.Get(gymid);
            ViewData["GymId"] = gymid;
            ViewData["GymName"] = gym.Name;
            TrainingModel model = new TrainingModel();
            model.Duration = new TimeSpan(1, 0, 0);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(TrainingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.StartTime = model.StartDay.AddHours(model.StartTime.Hour).AddMinutes(model.StartTime.Minute);

                    model.StatusId = (int)TrainingStatus.Active;
                    int id = _trainingB.Add((Training)model);

                    return RedirectToAction("Details", "Training", new { id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException.InnerException.Message);
            }

            var gym = _gymB.Get(model.GymId);
            ViewData["GymId"] = gym.Id;
            ViewData["GymName"] = gym.Name;
            model.StatusId = (int)TrainingStatus.Active;

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            var model = (TrainingModel)_trainingB.Get(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, TrainingModel model)
        {
            if (ModelState.IsValid)
            {
                model.StartTime = new DateTime(model.StartDay.Year, model.StartDay.Month, model.StartDay.Day, model.StartTime.Hour, model.StartTime.Minute, 0);
                _trainingB.Update((Training)model);

                return RedirectToAction("Details", new { id = model.Id });
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                Training t = _trainingB.Get(id);

                _trainingB.Delete(id);

                return RedirectToAction("Index", new { gymid = t.GymId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Gym");
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Cancel(int id)
        {
            try
            {
                _trainingB.Cancel(id);

                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Gym");
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Restore(int id)
        {
            try
            {
                _trainingB.Restore(id);

                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Gym");
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin,trainer")]
        public void ChangeStatus(int tiid, int statusid)
        {
            Debug.WriteLine("ChangeStatus");
            _trainingItemB.ChangeStatus(tiid, (TrainingItemStatus)statusid);
        }

        [HttpPost]
        [Authorize(Roles = "admin,trainer")]
        public void ChangePayStatus(int tiid, int statusid)
        {
            Debug.WriteLine("ChangePayStatus");
            _trainingItemB.ChangePayStatus(tiid, (TrainingItemPayStatus)statusid);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Reacalculate(int id)
        {
            _trainingB.ReacalculateValues(id);
            return RedirectToAction("Details", new { id });
        }
    }
}