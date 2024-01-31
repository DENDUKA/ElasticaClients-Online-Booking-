using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using ElasticaClients.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace ElasticaClients.Controllers
{
    public class AccountController : Controller
	{
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> Index(int page = 1, int count = 1000000, string filter = "")
		{
			var accs = await AccountB.GetWithFilterAsync(page, count, filter);

			var model = AccountModel.ToModelList(accs);

			var res = await Task.Run(() => View(model));

			return res;
		}
		
		public ActionResult Details(int id)
		{
			AccountModel model = new AccountModel();
			
			if (AccountB.IsCurrentUserClient())
			{
				Account pageAcc = AccountB.GetLite(id);

				if (pageAcc.RoleId != Role.trainerId)
				{
					if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
					{
						return RedirectToAction("Registration", "Account");
					}

					if (System.Web.HttpContext.Current.User.Identity.Name != id.ToString())
					{
						return Details(int.Parse(System.Web.HttpContext.Current.User.Identity.Name));
					}
				}
			}

			var acc = AccountB.GetLite(id);

			if (acc.Role.Id == Role.trainerId)
			{
				model = (AccountModel)AccountB.GetTrainer(id);
				return View("TrainerDetails", model);
			}

			model = (AccountModel)AccountB.Get(id);

			return View(model);
		}

		[Authorize(Roles = "admin")]
		public ActionResult Create()
		{
			return View();
		}
		
		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Create(AccountModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Account acc = (Account)model;

					AccountB.Add(acc);

					return RedirectToAction("Details", new { id = acc.Id});
				}

				return View(model);
			}
			catch
			{
				return View();
			}
		}

		[Authorize(Roles = "admin")]
		public ActionResult Edit(int id)
		{
			var model = (AccountModel)AccountB.GetLite(id);


			return View(model);
		}
		
		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Edit(AccountModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					AccountB.Update((Account)model);

					return RedirectToAction("Details", new { id = model.Id});
				}

				return View(model);
			}
			catch
			{
				return View();
			}
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult Delete(int id)
		{
			try
			{
				AccountB.Delete(id);					

				return RedirectToAction("Index");
			}
			catch
			{
				return RedirectToAction("Index");
			}
		}

		public ActionResult Registration()
		{
			AccountModel model = new AccountModel();
			return View(model);
		}

		public ActionResult BonusesOff(int AccountId)
		{
			AccountModel model = (AccountModel)AccountB.Get(AccountId);
			return View(model);
		}

		[HttpPost]
		public ActionResult BonusesOff(int AccountId, int bonusesOff)
		{
			AccountB.BonusesOff(AccountId, bonusesOff);
			return RedirectToAction("Details", new { id = AccountId });
		}

		[HttpPost]
		public ActionResult Registration(AccountModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Account acc = (Account)model;

					acc.RoleId = Role.clientId;

					AccountB.Add(acc);

					FormsAuthentication.SetAuthCookie(acc.Id.ToString(), true);

					return RedirectToAction("Details", new { id = acc.Id });
				}

				return View(model);
			}
			catch
			{
				return View(model);
			}
		}

		public ActionResult AdminLogin()
		{
			return View();
		}

		[HttpPost]
		public ActionResult AdminLogin(string login, string password, string returnUrl)
		{
			if (AccountModel.LoginSuccess(login, password))
			{
				return Redirect(string.IsNullOrEmpty(returnUrl) ? "~" : returnUrl);
			}
			
			TempData["Status"] = "Пара Логин/Пароль не найдена";

			return View();
		}


		[HttpGet]
		public JsonResult GetTrainerData(int accId, string start, string end)
		{

			DateTime dateStart = DateTime.ParseExact(start, new string[] { "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None);
			DateTime dateEnd = DateTime.ParseExact(end, new string[] { "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None);

			var trainings = TrainingB.GetAllForTrainer(accId, dateStart, dateEnd);

			var json = Json(new { pay = trainings.Sum(x => x.TrainerPay) }, JsonRequestBehavior.AllowGet);

			return json;
		}

		#region Login
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(string phone, string returnUrl)
		{
			if (AccountModel.LoginSuccessByPhone(phone))
			{
				return Redirect(string.IsNullOrEmpty(returnUrl) ? "~" : returnUrl);
			}
			TempData["Status"] = "Такого номера не найдено";
			return View();
		}

		[ChildActionOnly]
		public ActionResult UserInfo()
		{
			return PartialView();
		}

		[HttpGet]
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return Redirect("~");
		}
		#endregion
	}
}
