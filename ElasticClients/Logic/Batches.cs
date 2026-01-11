using System.Timers;

namespace ElasticaClients.Logic
{
    public class Batches
    {
        private readonly SubscriptionB _subscriptionB;
        private readonly Timer _everyHour = new Timer(2000000);

        public Batches(SubscriptionB subscriptionB)
        {
            _subscriptionB = subscriptionB;
            _everyHour.Elapsed += EveryHourEvent;
            _everyHour.Start();

            EveryHourEvent(null, null);
        }

        private void EveryHourEvent(object source, ElapsedEventArgs e)
        {
            //TrainingB.UpdateStatusBatch(DateTime.Now.AddHours(-2), DateTime.Now.AddHours(1));

            _subscriptionB.BatchUnfreeze();

            _subscriptionB.BatchCloseSubscription();

            _subscriptionB.BatchActivateByTime();
        }
    }
}