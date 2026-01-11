using ElasticaClients.DAL.Accessory;
using ElasticaClients.Logic;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
    public class TrainingCalendarController : Controller
    {
        private readonly TrainingB _trainingB;
        private readonly AccountB _accountB;

        public TrainingCalendarController(TrainingB trainingB, AccountB accountB)
        {
            _trainingB = trainingB;
            _accountB = accountB;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTrainings(DateTime start, DateTime end, int gymId)
        {
            var trainings = _trainingB.GetAllForGym(gymId, start, end);

            if (!_accountB.IsCurrentUserAdminTrainer())
            {
                trainings = trainings.Where(x => x.StartTime >= DateTime.Today.Date).ToList();
            }

            var json = Json(trainings.Select(x => new
            {
                id = x.Id,
                title = $"{x.Name} \nОсталось:{x.SeatsLeft}",
                //someimportantkeyid = 1,
                start = x.StartTime.ToString("s"), //ev.DateStart.ToString("s"),
                end = x.StartTime.Add(x.Duration).ToString("s"),
                allDay = false,
                color = x.StatusId == (int)TrainingStatus.Active ? "blue" : "red"
                //start = x.StartTime.ToString("s"),
                //end = x.StartTime.AddHours(4).ToString("s"),
                //statusString = "ss1",
                //StatusColor = "blue",
                //color = "blue",
                //className = "",
                //someKey = 1,
                //allDay = false,
            }).ToArray(), JsonRequestBehavior.AllowGet);

            return json;
        }

        [HttpGet]
        public JsonResult GetTrainingsForTrainer(DateTime start, DateTime end, int trainerId)
        {
            var trainings = _trainingB.GetAllForTrainer(trainerId, start, end);

            var json = Json(trainings.Select(x => new
            {
                id = x.Id,
                title = x.Name,
                //someimportantkeyid = 1,
                start = x.StartTime.ToString("s"), //ev.DateStart.ToString("s"),
                end = x.StartTime.Add(x.Duration).ToString("s"),
                allDay = false,
                color = x.StatusId == (int)TrainingStatus.Active ? "blue" : "red"
                //start = x.StartTime.ToString("s"),
                //end = x.StartTime.AddHours(4).ToString("s"),
                //statusString = "ss1",
                //StatusColor = "blue",
                //color = "blue",
                //className = "",
                //someKey = 1,
                //allDay = false,
            }).ToArray(), JsonRequestBehavior.AllowGet); ;

            return json;
        }
    }
}