﻿using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.Entities;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElasticaClients.Controllers
{
	public class TrainingCalendarController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public JsonResult GetTrainings(DateTime start, DateTime end, int gymId)
		{
			var trainings = TrainingB.GetAllForGym(gymId, start, end);

			if (!AccountB.IsCurrentUserAdminTrainer())
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
			var trainings = TrainingB.GetAllForTrainer(trainerId, start, end);

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