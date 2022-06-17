using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ElasticaClients.Models
{
	public class AccountModel : IValidatableObject
	{
		public int Id { get; set; }
		[Required]
		[DisplayName("Телефон")]
		[MaxLength(10, ErrorMessage = "Количество цифр должно быть 10")]
		[MinLength(10, ErrorMessage = "Количество цифр должно быть 10")]
		[Range(-1, 99999999999, ErrorMessage = "Только числа")]
		[DataType(DataType.Currency)]
		[DisplayFormat(DataFormatString = "{0}")]
		public string Phone { get; set; }
		[DisplayName("Instagram ")]
		public string Instagram { get; set; }
		[DisplayName("Имя ")]
		[Required]
		public string Name { get; set; }
		[DisplayName("Почта ")]
		public string Email { get; set; }
		[Required]
		public int RoleId { get; set; }
		public Role Role { get; set; }
		[DataType(DataType.Date)]
		[DisplayName("C ")]
		public DateTime? TrainingsFrom { get; set; }
		[DataType(DataType.Date)]
		[DisplayName("До ")]
		public DateTime? TrainingsTo { get; set; }
		[DisplayName("Бонусы ")]
		public int Bonuses { get; set; }
		public int BonusesOff { get; set; }
		public int BonusesSummary => Bonuses - BonusesOff;
		public List<Subscription> Subscriptions { get; set; }
		public List<TrainingItem> TrainingItems { get; set; }
		public List<Training> Trainings { get; set; }

		public int SettingsBranchId { get; set; }


		public void CurrentWeek()
		{
			if (TrainingsFrom is null || TrainingsTo is null)
			{
				DateTime startOfWeek = DateTime.Today.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)DateTime.Today.DayOfWeek);
				TrainingsFrom = startOfWeek;
				TrainingsTo = startOfWeek.AddDays(6);
			}
		}

		public static List<AccountModel> ToModelList(List<Account> accs)
		{
			List<AccountModel> model = new List<AccountModel>();

			foreach (var a in accs)
			{
				model.Add((AccountModel)a);
			}

			return model;
		}


		public static explicit operator AccountModel(Account acc)
		{
			return new AccountModel()
			{
				Id = acc.Id,
				Phone = acc.Phone,
				Email = acc.Email,
				Instagram = acc.Instagram,
				Name = acc.Name,
				Role = acc.Role,
				RoleId = acc.RoleId,
				Subscriptions = acc.Subscriptions,
				TrainingItems = acc.TrainingItems,
				Trainings = acc.Trainings,
				SettingsBranchId = acc.SettingsBranchId,
				Bonuses = acc.Bonuses,
				BonusesOff = acc.BonusesOff,
			};
		}

		public static explicit operator Account(AccountModel model)
		{
			return new Account()
			{
				Id = model.Id,
				Email = model.Email,
				Instagram = model.Instagram,
				Name = model.Name,
				Phone = model.Phone,
				RoleId = model.RoleId,
				SettingsBranchId = model.SettingsBranchId,
				Bonuses = model.Bonuses,
				BonusesOff = model.BonusesOff,
			};
		}

		public int SubscriptionItemRemain(int subscriptionId)
		{
			return Subscriptions.First(x => x.Id == subscriptionId).TrainingCount - TrainingItems.Count(x => x.SubscriptionId == subscriptionId);
		}

		internal static bool LoginSuccess(string phone, string password)
		{
			var acc = AccountDAL.GetByPhone(phone);

			if (acc != null)
			{
				if (acc.Password != null && acc.Password.Equals(password))
				{
					FormsAuthentication.SetAuthCookie(acc.Id.ToString(), true);
					return true;
				}
			}

			return false;
		}

		internal static bool LoginSuccessByPhone(string phone)
		{
			var acc = AccountDAL.GetByPhone(phone);

			if (acc != null && acc.RoleId == Role.clientId)
			{
				FormsAuthentication.SetAuthCookie(acc.Id.ToString(), true);
				return true;
			}

			return false;
		}


		public static IEnumerable<SelectListItem> ClientsToSelectListItems(List<Account> accs = null)
		{
			if (accs is null)
			{
				accs = AccountDAL.GetAll();
			}

			List<SelectListItem> li = new List<SelectListItem>();

			li.AddRange(accs.OrderBy(a => a.Name)
					  .Where(a => a.Role.Id == Role.clientId)
					  .Select(a =>
						  new SelectListItem
						  {
							  Text = a.Name,
							  Value = a.Id.ToString()
						  }));
			return li;
		}

		public static IEnumerable<SelectListItem> TrainersToSelectListItems(int selectedId = 0, List<Account> accs = null)
		{
			if (accs is null)
			{
				accs = AccountDAL.GetAll();
			}

			List<SelectListItem> li = new List<SelectListItem>();

			if (selectedId == 0)
			{
				li.Add(new SelectListItem() { Selected = true });
			}

			li.AddRange(accs.OrderBy(a => a.Name)
					  .Where(a => a.Role.Id == Role.trainerId)
					  .Select(a =>
						  new SelectListItem
						  {
							  Text = a.Name,
							  Value = a.Id.ToString(),
							  Selected = a.Id == selectedId,
						  }));
			return li;
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var results = new List<ValidationResult>();

			if (Id == 0)
			{
				var acc = AccountB.GetByPhone(Phone);

				if (acc != null)
				{
					results.Add(new ValidationResult("Аккаунт с таким телефоном уже зарегистрирован"));
				}
			}

			return results;
		}
	}
}