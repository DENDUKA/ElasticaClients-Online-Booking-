using System;
using System.Timers;

namespace ElasticaClients.Logic
{
	public class Batches
	{
		public Batches()
		{
			EveryHour.Elapsed += EveryHourEvent;
			EveryHour.Start();

			EveryHourEvent(null, null);
		}

		private static void EveryHourEvent(object source, ElapsedEventArgs e)
		{
			//TrainingB.UpdateStatusBatch(DateTime.Now.AddHours(-2), DateTime.Now.AddHours(1));

			SubscriptionB.BatchUnfreeze();

			SubscriptionB.BatchCloseSubscription();

			SubscriptionB.BatchActivateByTime();
		}
		
		private static readonly Timer EveryHour = new Timer(2000000);
	}
}