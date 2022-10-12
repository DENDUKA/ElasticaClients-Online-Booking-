using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Bot
{
	internal static class Answers
	{
		internal static async void Start(long chatId)
		{
			var state = GetState(chatId);

			state.state = StateEnum.Start;

			if (state.AccountId == 0)
			{
				EnterTelephoneNumber(chatId);
			}
			else
			{
				MainMenu(chatId);
			}
		}

		private static async void MainMenu(long chatId)
		{
			var state = GetState(chatId);

			state.state = StateEnum.MainMenu;

			var b1 = new KeyboardButton(Zapisatsa);
			var b2 = new KeyboardButton(KogdaZapisan);
			var b3 = new KeyboardButton(OstatokPoAbonementu);
			var b4 = new KeyboardButton(Otmena);
			var b5 = new KeyboardButton(Exit);

			var keyboard = new ReplyKeyboardMarkup(new KeyboardButton[][] { new KeyboardButton[] { b1 }, new KeyboardButton[] { b2 }, new KeyboardButton[] { b3 }, new KeyboardButton[] { b4 },new KeyboardButton[] { b5 } });

			var account = AccountDAL.Get(state.AccountId);

			if (account != null)
			{
				state.state = StateEnum.WaitCommand;
				await Shared.Bot.SendTextMessageAsync(chatId, $"Здравствуйте {account.Name} : ", replyMarkup: keyboard);
				return;
			}
			else
			{
				state.state = StateEnum.EnterTelephone;
			}
		}

		private static async void EnterTelephoneNumber(long chatId)
		{
			var state = GetState(chatId);
			state.state = StateEnum.EnterTelephone;
			await Shared.Bot.SendTextMessageAsync(chatId, "Ведите свой номер телефона в формате +7********** :");
		}

		internal static void NextStage(long id, Message message = null)
		{
			var state = GetState(id);
			switch (state.state)
			{
				case (StateEnum.EnterTelephone):
					{
						if (Authenticate(id, message))
						{
							SuccessAuthenticate(id, message);
						}
						else
						{
							FailedAuthenticate(id, message);
						}

						break;
					}
				case (StateEnum.MainMenu):
					{
						MainMenu(id);
						break;
					}
				case (StateEnum.WaitCommand):
					{
						if (message.Text == Zapisatsa)
						{
							SignForWorkout(id);
						}
						else if (message.Text == KogdaZapisan)
						{
							GetFutureWorkout(id);
						}
						else if (message.Text == OstatokPoAbonementu)
						{
							GetSubscriptionLeft(id);
						}
						else if (message.Text == Otmena)
						{
							SelectTrainingCancellation(id);
						}						

						break;
					}
				case (StateEnum.TrainingCancellation):
					{
						TrainingCancellation(id, message);
						break;
					}
				case (StateEnum.TrainingCancellationLow5H):
					{
						TrainingCancellationLow5H(id, message);
						break;
					}
				case (StateEnum.SelectBranch):
					{
						SelectBranch(id, message);
						break;
					}
				case (StateEnum.SelectDate):
					{
						SelectDate(id, message);
						break;
					}
				case (StateEnum.SelectTraining):
					{
						SelectTraining(id, message);
						break;
					}
				case (StateEnum.SelectRazovoe):
					{
						SelectRazovoe(id, message);
						break;
					}

				default:
					{
						state.state = StateEnum.Start;
						break;
					}
			}
		}

		private static async void SignForWorkout(long id)
		{
			var state = GetState(id);

			var branhes = BranchDAL.GetAll();

			var b1 = new KeyboardButton(branhes[0].Name);
			var b2 = new KeyboardButton(branhes[2].Name);

			var keyboard = new ReplyKeyboardMarkup(new KeyboardButton[][] { new KeyboardButton[] { b1 }, new KeyboardButton[] { b2 } });

			state.state = StateEnum.SelectBranch;

			await Shared.Bot.SendTextMessageAsync(id, $"Выберете филиал :", replyMarkup: keyboard);

			return;
		}

		private static async void SelectBranch(long id, Message message)
		{
			var state = GetState(id);
			var branch = BranchDAL.GetByName(message.Text);

			if (branch == null)
			{
				ToMainMenu(id, $"Такого филиала не существует :");
				return;
			}

			state.branchId = branch.Id;
			state.state = StateEnum.SelectDate;

			DateTime date = DateTime.Now;

			List<KeyboardButton> btns = new List<KeyboardButton>();

			for (int i = 0; i < 9; i++)
			{
				var d = date.AddDays(i);
				btns.Add(new KeyboardButton($"{d.ToString("dd.MM.yyyy")} {d.ToString("ddd")}"));
			}

			var keyboard = new ReplyKeyboardMarkup(new KeyboardButton[][] 
			{	new KeyboardButton[] { btns[0], btns[1], btns[2] }, 
				new KeyboardButton[] { btns[3], btns[4], btns[5] }, 
				new KeyboardButton[] { btns[6], btns[7], btns[8] } 
			});

			await Shared.Bot.SendTextMessageAsync(id, $"Выберете дату :", replyMarkup: keyboard);
		}

		private static async void SelectDate(long id, Message message)
		{
			var state = GetState(id);

			DateTime date;

			if (!DateTime.TryParse(message.Text, out date) || date.Date < DateTime.Today.Date)
			{
				ToMainMenu(id, $"Дата некорректная :");
				return;
			}

			state.date = date;

			var gym = GymDAL.GetForBranch(state.branchId).First();

			state.gymId = gym.Id;

			var trainings = TrainingDAL.GetAllForGym(gym.Id, date.Date, date.Date.AddDays(1)).OrderBy(x => x.StartTime).ToList();

			var buttons = new List<List<KeyboardButton>>();

			if (trainings.Count == 0)
			{
				ToMainMenu(id, $"На эту дату тренировок нет(( :");
				return;
			}

			foreach (var t in trainings)
			{
				buttons.Add(new List<KeyboardButton>() { $"{t.StartTime.ToString("HH:mm")} {t.Name} {t.SeatsLeft}/{t.Seats} {t.Trainer.Name}" });
			}

			var keyboard = new ReplyKeyboardMarkup(buttons);

			state.state = StateEnum.SelectTraining;

			await Shared.Bot.SendTextMessageAsync(id, $"Выберете тренировку :", replyMarkup: keyboard);
		}

		private static async void SelectRazovoe(long id, Message message)
		{
			var state = GetState(id);
			var text = message.Text.ToLower();

			if (text == "да")
			{
				var sub = SubscriptionDAL.GetForAccount(state.AccountId, true, (int)SubscriptionStatus.Razovoe).First();

				if (sub is null)
				{
					ToMainMenu(id, $"Данные не корректны");
					return;
				}

				TrainingItem ti = new TrainingItem()
				{
					AccountId = state.AccountId,
					Razovoe = true,
					SubscriptionId = sub.Id,
					TrainingId = state.trainingId,
				};

				if (TrainingItemB.Add(ti))
				{
					//УСПЕШНО ЗАПИСАНЫ ПО АБОНЕМЕНТУ
					var training = TrainingDAL.Get(state.trainingId);

					ToMainMenu(id, $"Вы успешно записаны на тренировку {training.Name} по разовому посещению {training.StartTime.ToString("dd.MM")} в {training.StartTime:HH:mm}");

					return;
				}
			}
			else if (text == "нет")
			{
				ToMainMenu(id, $"ЧТОТО НАДО НАПИСАТЬ");

				return;
			}
		}

		private static async void SelectTraining(long id, Message message)
		{
			var state = GetState(id);

			var training = GetTrainingFromMessage(state, message);

			if (training is null)
			{
				ToMainMenu(id, $"Данные не корректны");
				return;
			}

			state.trainingId = training.Id;

			if (TrainingB.IsAccountSigned(state.trainingId, state.AccountId))
			{
				ToMainMenu(id, $"Вы уже записаны на эту тренировку");

				return;
			}

			if (training.SeatsLeft > 0)
			{
				var activatedSubs = SubscriptionDAL.GetForAccount(state.AccountId, false, (int)SubscriptionStatus.Activated).Where(x => x.TrainingsLeft > 0);

				Subscription subscription = null;

				if (activatedSubs.Count() > 0)
				{
					subscription = activatedSubs.First();
				}
				else
				{
					var notActivatedSubs = SubscriptionDAL.GetForAccount(state.AccountId, false, (int)SubscriptionStatus.NotActivated);

					if (notActivatedSubs.Count > 0)
					{
						subscription = notActivatedSubs.First();
					}
					else
					{
						var b1 = new KeyboardButton("Да");
						var b2 = new KeyboardButton("Нет");

						var keyboard = new ReplyKeyboardMarkup(new KeyboardButton[] { b1, b2 });

						state.state = StateEnum.SelectRazovoe;
						await Shared.Bot.SendTextMessageAsync(id, $"У вас нет абонемента , записать как разовое (400 р)?\n\n* на тренировке вы можете приобрести абонемент и включить это занятие в абонемент", replyMarkup: keyboard);

						return;
					}
				}

				TrainingItem ti = new TrainingItem()
				{
					AccountId = state.AccountId,
					Razovoe = false,
					SubscriptionId = subscription.Id,
					TrainingId = state.trainingId,
				};

				if (TrainingItemB.Add(ti))
				{
					//УСПЕШНО ЗАПИСАНЫ ПО АБОНЕМЕНТУ
					ToMainMenu(id, $"Вы успешно записаны на тренировку {training.Name} по абонементу {training.StartTime.ToString("dd.MM")} в {training.StartTime.ToString("HH:mm")}");

					return;
				}
				else
				{
					ToMainMenu(id, $"Данные не корректны");

					return;
				}
			}
			else
			{
				ToMainMenu(id, $"Извините мет на этой тренировке мест нет ((");

				return;
			}
		}

		private static async void GetFutureWorkout(long id)
		{
			var state = GetState(id);

			var tis = TrainingItemB.GetFutureTrainings(state.AccountId);

			string text = "";

			if (tis.Count == 0)
			{
				text = "Вы не записаны в ближайшие даты";
			}
			else
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendLine("Ваши ближайшие тренировки : ");

				foreach (var ti in tis)
				{
					sb.AppendLine($"{ti.Training} {ti.Training.Gym.Branch.Name}");
				}

				text = sb.ToString();
			}

			await Shared.Bot.SendTextMessageAsync(id, text, replyMarkup: null);

			state.state = StateEnum.MainMenu;
		}

		private static async void SelectTrainingCancellation(long id)
		{
			var state = GetState(id);

			var tis = TrainingItemB.GetFutureTrainings(state.AccountId);

			var text = "";

			ReplyKeyboardMarkup keyboard = null;

			if (tis.Count() == 0)
			{
				text = "Вы не записаны в ближайшие даты";
			}
			else
			{
				text = "Выберете тренировку, которую хотите отменить";

				var buttons = new List<List<KeyboardButton>>();

				foreach (var ti in tis)
				{
					buttons.Add(new List<KeyboardButton>() { $"{ti.Training} {ti.Training.Gym.Branch.Name}" } );
				}

				keyboard = new ReplyKeyboardMarkup(buttons);
			}

			await Shared.Bot.SendTextMessageAsync(id, text, replyMarkup: keyboard);

			state.state = StateEnum.TrainingCancellation;
		}

		private static void TrainingCancellationLow5H(long id, Message message)
		{
			var state = GetState(id);

			var ti = TrainingItemB.Get(state.trainingItemId);

			if (ti.AccountId == state.AccountId)
			{
				//ОТМЕНЕНА ТРЕНИРОВКА < за 5 ЧАСОВ
				TrainingItemB.ChangeStatus(state.trainingItemId, TrainingItemStatus.no);
			}

			ToMainMenu(id, "Запись успешно отменена ");
		}

		private static async void TrainingCancellation(long id, Message message)
		{
			var state = GetState(id);

			Training training  = GetTrainingFromMessageDateTime(state, message);

			if (training is null)
			{
				ToMainMenu(id, $"Данные не корректны");
				return;
			}

			if (DateTime.Now >= training.StartTime)
			{
				ToMainMenu(id, $"Вы не можете выписаться с этой тренировки");
				return;
			}

			var ti = TrainingB.Get(training.Id).TrainingItems.Where(x => x.AccountId == state.AccountId).FirstOrDefault();

			if ((training.StartTime - DateTime.Now).TotalHours < 5)
			{
				state.state = StateEnum.TrainingCancellationLow5H;

				var b1 = new KeyboardButton("Да");
				var b2 = new KeyboardButton("Нет");

				var keyboard = new ReplyKeyboardMarkup(new KeyboardButton[] { b1, b2 });

				state.trainingItemId = ti.Id;

				await Shared.Bot.SendTextMessageAsync(id, $"Занятие сгорит: отмена меньше чем за 5 часов, точно отменить?", replyMarkup: keyboard);

				return;
			}

			//ОТМЕНИТЬ ТРЕНИРОВКУ Без СГОРАНИЯ

			if (ti == null)
			{
				ToMainMenu(id, "Отменить не удалось");

				return;
			}

			TrainingItemB.Delete(ti.Id);

			ToMainMenu(id, "Запись успешно отменена ");

			return;
		}

		private static Training GetTrainingFromMessageDateTime(State state, Message message)
		{
			//06.07 10:30 МФР + РАСТЯЖКА  Саратов Солнечный
			DateTime time;

			int gymId = 0;

			try
			{
				var split = message.Text.Split(' ');

				time = DateTime.Parse($"{split[0]}.{DateTime.Today.Year} {split[1]}");

				var branch = BranchDAL.GetAll().Where(x => x.Name.Contains(split[split.Length - 1])).First();
				branch = BranchDAL.Get(branch.Id);

				gymId = branch.Gyms.First().Id;

			}
			catch (Exception ex)
			{
				return null;
			}

			var training = TrainingDAL.GetAllForGymByTime(gymId, time);

			if (training == null)
			{
				return null;
			}

			return training;
		}

		private static Training GetTrainingFromMessage(State state, Message message)
		{
			DateTime time;

			try
			{
				var t = message.Text.Split(' ')[0];
				time = DateTime.Parse(t);
			}
			catch (Exception ex)
			{
				return null;
			}

			state.date = state.date.AddMinutes(time.Minute).AddHours(time.Hour);

			var training = TrainingDAL.GetAllForGymByTime(state.gymId, state.date);

			if (training == null)
			{
				return null;
			}

			return training;
		}

		private static async void GetSubscriptionLeft(long id)
		{
			var state = GetState(id);

			var activateSubs = SubscriptionDAL.GetForAccount(state.AccountId, subscriptionStatus: (int)SubscriptionStatus.Activated, includeTrainingItems: true);
			var notActivateSubs = SubscriptionDAL.GetForAccount(state.AccountId, subscriptionStatus: (int)SubscriptionStatus.NotActivated, includeTrainingItems: true);

			activateSubs.AddRange(notActivateSubs);


			if (activateSubs.Count == 0)
			{
				await Shared.Bot.SendTextMessageAsync(id, "У Вас нет абонементов 🟩🟥⬛️🟨", replyMarkup: null);

				state.state = StateEnum.MainMenu;
				return;
			}

			StringBuilder sb = new StringBuilder();

			foreach (var sub in activateSubs)
			{
				if (sub.StatusId == (int)SubscriptionStatus.Activated)
				{
					sb.AppendLine($"Абонемент на {sub.TrainingCount} занятий. Активен до: {sub.DateEnd.Date:dd.MM.yyyy}");

					foreach (var t in sub.TrainingItems)
					{
						sb.AppendLine($"{(TiStatusColor.ContainsKey(t.StatusId)?TiStatusColor[t.StatusId]:string.Empty)}  {t.Training.StartTime.Date:dd.MM}");
					}
					sb.AppendLine($"Остаток : {sub.TrainingsLeft}");
					sb.AppendLine();
				}

				if (sub.StatusId == (int)SubscriptionStatus.NotActivated)
				{
					sb.AppendLine($"Абонемент на {sub.TrainingCount} занятий.");
					sb.AppendLine($"Дата покупки : {sub.BuyDate:dd.MM.yyy}");

				}
			}

			//"  🟩-зел    🟥- крас    ⬛️ - черн     🟨- желт"

			await Shared.Bot.SendTextMessageAsync(id, sb.ToString(), replyMarkup: null);

			state.state = StateEnum.MainMenu;
		}

		private static Dictionary<int, string> TiStatusColor = new Dictionary<int, string>()
		{
			{(int)TrainingItemStatus.yes, "🟩" },
			{(int)TrainingItemStatus.no, "🟥" },
			{(int)TrainingItemStatus.unKnown, "⬛️" },
		};

		private static async void FailedAuthenticate(long id, Message message)
		{
			var state = GetState(id);

			state.state = StateEnum.EnterTelephone;
			//NextStage(id, message);

			await Shared.Bot.SendTextMessageAsync(id, "Пользователь с таким номером не найден");
		}

		private static async void SuccessAuthenticate(long id, Message message)
		{
			var state = GetState(id);

			state.state = StateEnum.MainMenu;
			NextStage(id, message);

			
		}

		private const string Zapisatsa = "Записаться на тренировку";		
		public const string KogdaZapisan = "Узнать когда записан";
		private const string OstatokPoAbonementu = "Остаток по абонементу";
		private const string Otmena = "Отмена тренировки";
		public const string Exit = "Выйти";

		private static bool Authenticate(long chatId, Message message)
		{
			var phone = message.Text.Replace("+7", "");

			var acc = AccountDAL.GetByPhone(phone);

			if (acc is null)
			{
				return false;
			}


			var state = GetState(chatId);
			state.AccountId = acc.Id;

			acc.TelegramId = chatId.ToString();
			AccountDAL.Update(acc);

			return true;
		}

		private static void AddChatId(long chatId)
		{
			if (Shared.State.ContainsKey(chatId))
			{
				Shared.State.Add(chatId, new State());
			}
			else
			{
				Shared.State[chatId].Clear();
			}
		}

		private static State GetState(long chatId)
		{
			if (Shared.State.ContainsKey(chatId))
			{ 
				return Shared.State[chatId];
			}

			var acc = AccountDAL.GetByTelegramId(chatId.ToString());

			var state = new State();

			if (acc !=  null)
			{
				state.AccountId = acc.Id;				
			}

			Shared.State.Add(chatId, state);

			return state;
		}

		private static async void ToMainMenu(long id, string text = null)
		{
			var state = GetState(id);
			state.state = StateEnum.MainMenu;
			await Shared.Bot.SendTextMessageAsync(id, text, replyMarkup: null);
			NextStage(id);
			return;
		}
	}
}
