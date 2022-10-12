using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
	internal class State
	{
		public State()
		{
			Clear();
		}
		public int AccountId { get; set; }
		public StateEnum state { get; set; }
		public int branchId { get; set; }
		public DateTime date { get; set; }
		public int trainingId { get; set; }
		public int gymId { get; set; }
		public int trainingItemId { get; set; }

		internal void Clear()
		{
			state = StateEnum.Start;
			branchId = 0;			
			trainingId = 0;
			gymId = 0;
		}
	}
}
