using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticaClients.Logic
{
    public class TrainingItemB
    {
        private readonly ITrainingItemDAL _trainingItemDAL;
        private readonly SubscriptionB _subscriptionB;
        private readonly TrainingB _trainingB;
        private readonly AccountB _accountB;
        private readonly LogB _logB;

        public TrainingItemB(ITrainingItemDAL trainingItemDAL, SubscriptionB subscriptionB, TrainingB trainingB, AccountB accountB, LogB logB)
        {
            _trainingItemDAL = trainingItemDAL;
            _subscriptionB = subscriptionB;
            _trainingB = trainingB;
            _accountB = accountB;
            _logB = logB;
        }
        public void ChangeStatus(int tiid, TrainingItemStatus tiStatus)
        {
            _trainingItemDAL.ChangeStatus(tiid, tiStatus);

            var ti = Get(tiid);

            _subscriptionB.RecalculateValues(ti.SubscriptionId);
            _trainingB.ReacalculateValues(ti.TrainingId);
            _accountB.ReacalculateBonuses(ti.AccountId);
        }

        public void ChangePayStatus(int tiid, TrainingItemPayStatus tiPayStatus)
        {
            _trainingItemDAL.ChangePayStatus(tiid, tiPayStatus);
        }

        public bool Add(TrainingItem trainingItem)
        {
            if (!IsAddValid(trainingItem))
            {
                return false;
            }

            if (trainingItem.Razovoe)
            {
                var training = _trainingB.Get(trainingItem.TrainingId);

                var razovoe = _subscriptionB.GetRazovoe(trainingItem.AccountId, training.Gym.BranchId);

                trainingItem.SubscriptionId = razovoe.Id;
                trainingItem.StatusPayId = (int)TrainingItemPayStatus.no;
            }
            else
            {
                IsFirstTrainingItemInSubscription(trainingItem);
            }

            trainingItem.StatusId = (int)TrainingItemStatus.unKnown;

            _trainingItemDAL.Add(trainingItem);

            _trainingB.ReacalculateValues(trainingItem.TrainingId);
            _subscriptionB.RecalculateValues(trainingItem.SubscriptionId);

            _logB.NewTrainingItem(trainingItem);

            return true;
        }

        internal List<TrainingItem> GetAllForBranch(int branchId, DateTime start, DateTime end, bool onlyRazovoe)
        {
            return _trainingItemDAL.GetAllForBranch(branchId, start, end, onlyRazovoe);
        }

        private void IsFirstTrainingItemInSubscription(TrainingItem trainingItem)
        {
            var sub = _subscriptionB.Get(trainingItem.SubscriptionId);
            if (sub.StatusId == (int)SubscriptionStatus.NotActivated && sub.TrainingItems.Count == 0)
            {
                Training training = _trainingB.Get(trainingItem.TrainingId);
                sub.StatusId = (int)SubscriptionStatus.Activated;
                sub.ActivateDate = training.StartTime.Date;
                _subscriptionB.Update(sub);
            }
        }

        public TrainingItem Get(int id)
        {
            return _trainingItemDAL.Get(id);
        }

        public void Delete(int id)
        {
            var ti = Get(id);

            _trainingItemDAL.Delete(id);

            _logB.DeleteTrainingItem(ti);

            _subscriptionB.RecalculateValues(ti.SubscriptionId);
            _trainingB.ReacalculateValues(ti.TrainingId);
        }

        public int GetRazovoeCost(int accountId, int branchId)
        {
            if (_accountB.IsFirstTime(accountId))
            {
                return 400;
            }

            return 700;
        }

        /// <summary>
        /// Добавление всех предстоящих разовых тренировок в абонемент на чиная с дня покупки абонемента
        /// </summary>
        /// <param name="id"></param>
        public void AddRazovoesToSubscription(int accId, int subId)
        {
            Subscription sub = _subscriptionB.Get(subId);

            var tis = GetAllForAccount(accId, sub.BuyDate, sub.BuyDate.AddDays(20))
                .Where(x => x.Razovoe && x.StatusId != (int)TrainingItemStatus.canceledByAdmin);

            foreach (var ti in tis)
            {
                ti.Razovoe = false;
                ti.SubscriptionId = subId;
                _trainingItemDAL.Update(ti);
            }
        }

        public List<TrainingItem> GetAllForAccount(int accountId)
        {
            var tis = _trainingItemDAL.GetAllForAccount(accountId);

            return tis is null ? new List<TrainingItem>() : tis;
        }

        public List<TrainingItem> GetAllForAccount(int accountId, DateTime start, DateTime end)
        {
            var tis = _trainingItemDAL.GetAllForAccount(accountId, start, end);

            return tis is null ? new List<TrainingItem>() : tis;
        }

        private bool IsAddValid(TrainingItem ti)
        {
            if (!ti.Razovoe)
            {
                Subscription sub = _subscriptionB.Get(ti.SubscriptionId);

                if (sub.TrainingsLeft <= 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}