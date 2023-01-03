using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElasticaClients.Models
{
	public class TrainingItemModel : IValidatableObject
	{
		public int Id { get; set; }
		[DisplayName("Абонемент ")]
		public int SubscriptionId { get; set; }
		public Subscription Subscription { get; set; }
		[Required]
		[DisplayName("Тренировка ")]
		public int TrainingId { get; set; }
		public Training Training { get; set; }
		[Required]
		[DisplayName("Клиент ")]
		public int AccountId { get; set; }
		public Account Account { get; set; }
		[DisplayName("Разовое ")]
		public bool Razovoe { get; set; }
		[DisplayName("Стоимость ")]
		[Range(0, 999999, ErrorMessage = "Не отрицательное")]
		public int RazovoeCost { get; set; }
		[DisplayName("Пробное ")]
		public bool IsTrial { get; set; }

		public static explicit operator TrainingItem(TrainingItemModel model)
		{
			return new TrainingItem()
			{
				Id = model.Id,
				AccountId = model.AccountId,
				SubscriptionId = model.SubscriptionId,
				TrainingId = model.TrainingId,
				Razovoe = model.Razovoe,
				RazovoeCost = model.RazovoeCost,
				IsTrial = model.IsTrial,
			};
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var results = new List<ValidationResult>();

			var training = TrainingB.Get(TrainingId);

			if (TrainingB.IsAccountSigned(TrainingId, AccountId))
			{
				results.Add(new ValidationResult("Пользователь уже записан на эту тренировку"));

				//ModelState.AddModelError(string.Empty, "Пользователь уже записан на эту тренировку");
				//return View(model);
			}

			if (training.StatusId != (int)TrainingStatus.Active)
			{
				results.Add(new ValidationResult("Тренировка отменена"));
			}

			if (!TrainingB.IsHaveSeat(TrainingId))
			{
				results.Add(new ValidationResult("Мест нет"));

				//ModelState.AddModelError(string.Empty, "Нет мест");
				//return View(model);
			}

			if (Razovoe)
			{

			}
			else
			{
				var sub = SubscriptionB.Get(SubscriptionId);

				if (sub != null && (sub.StatusId == (int)SubscriptionStatus.Activated))
				{
					sub.DateEnd.AddDays(1);
					if (training.StartTime > sub.DateEnd)
					{
						results.Add(new ValidationResult("Тренировка позже даты окончания абонемента"));
					}
				}

				if (sub != null && (sub.StatusId == (int)SubscriptionStatus.Activated || sub.StatusId == (int)SubscriptionStatus.NotActivated))
				{

				}
				else
				{
					results.Add(new ValidationResult("Выберете активный	 абонемент", new[] { "SubscriptionId" }));
				}
			}

			return results;
		}
	}
}