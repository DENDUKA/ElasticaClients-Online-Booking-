using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ElasticaClients.Models
{
	public class SubscriptionModel
	{
		public int Id { get; set; }
		[Required]
		[DisplayName("Клиент ")]
		public int AccountId { get; set; }
		public Account Account { get; set; }
		[Required]
		[DisplayName("Филиал ")]
		public int BranchId { get; set; }
		public Branch Branch { get; set; }
		public int StatusId { get; set; }
		public string StatusName { get; set; }
		[Required]
		[DisplayName("Срок действия(дней) ")]
		public int ActiveDays { get; set; }
		[Required]
		[DisplayName("За наличные ")]
		public bool ByCash { get; set; }
		[Required]
		[DisplayName("Добавить разовое в этот абонемент ")]
		public bool TiToThisSub{ get; set; }
		[Required]
		[DisplayName("Стоимость ")]
		public int Cost { get; set; }
		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		[DisplayName("Дата покупки ")]
		public DateTime BuyDate { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		[DisplayName("Дата активации ")]
		public DateTime? ActivateDate { get; set; }
		[Required]
		[DisplayName("Количество тренировок ")]
		public int TrainingCount { get; set; }
		public List<TrainingItem> TrainingItems { get; set; }
		public List<FreezeSubscriptionItem> FreezeSubscriptionList { get; set; }

		public int TrainingsLeft { get; set; }

		public DateTime DateEnd
		{
			get
			{
				if (ActivateDate != null)
				{
					return ((DateTime)ActivateDate).AddDays(ActiveDays);
				}
				return default;
			}
		}

		public int DaysLeft
		{
			get
			{
				var d = (DateEnd - DateTime.Today).Days;

				return d > 0 ? d : 0;
			}
		}

		public FreezeSubscriptionItem ActiveFreeze 
		{ 
			get
			{
				if (StatusId == (int)SubscriptionStatus.Freezed)
				{
					if (FreezeSubscriptionList.Any())
					{
						return FreezeSubscriptionList.First(x => x.Id == FreezeSubscriptionList.Max(y => y.Id));
					}
				}

				return null;
			}
		}

		public int FreezeDaysLeft
		{
			get
			{
				if (ActiveFreeze is null)
				{
					return 0;
				}
				else
				{
					return (ActiveFreeze.End - DateTime.Today).Days;
				}
			}
		}

		public static explicit operator SubscriptionModel(Subscription s)
		{
			return new SubscriptionModel()
			{
				Id = s.Id,
				Account = s.Account,
				AccountId = s.Account.Id,
				Branch = s.Branch,
				BranchId = s.Branch.Id,
				TrainingItems = s.TrainingItems,
				BuyDate = s.BuyDate,
				StatusId = s.StatusId,
				StatusName = s.StatusName,
				ActiveDays = s.ActiveDays,
				ByCash = s.ByCash,
				Cost = s.Cost,
				TrainingCount = s.TrainingCount,
				ActivateDate = s.ActivateDate,
				FreezeSubscriptionList = s.FreezeSubscriptionList,
				TrainingsLeft = s.TrainingsLeft,
			};
		}

		public static explicit operator Subscription(SubscriptionModel s)
		{
			return new Subscription()
			{
				Id = s.Id,
				AccountId = s.AccountId,
				BranchId = s.BranchId,
				BuyDate = s.BuyDate,
				StatusId = s.StatusId,
				ActiveDays = s.ActiveDays,
				ByCash = s.ByCash,
				Cost = s.Cost,
				TrainingCount = s.TrainingCount,
				ActivateDate = s.ActivateDate,
				TrainingsLeft = s.TrainingsLeft,
			};
		}
	}
}