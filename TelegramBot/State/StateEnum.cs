using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
	internal enum StateEnum
	{
		Start = 0,
		EnterTelephone = 1,
		EnterTelephoneConfirmation = 2,
		MainMenu = 3,
		SelectBranch = 4,
		SelectDate = 5,
		SelectTraining = 6,
		SelectRazovoe = 7,
		WaitCommand = 8,
		TrainingCancellation = 9,
		TrainingCancellationLow5H = 10,
	}
}
