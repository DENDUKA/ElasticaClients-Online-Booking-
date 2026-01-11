using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ElasticaClients.Logic
{
    public class SubscriptionB
    {
        private static Stopwatch sw = new Stopwatch();
        private readonly ISubscriptionDAL _subscriptionDAL;

        public SubscriptionB(ISubscriptionDAL subscriptionDAL)
        {
            _subscriptionDAL = subscriptionDAL;
        }

        public void Add(Subscription sub)
        {
            _subscriptionDAL.Add(sub);

            RecalculateValues(sub.Id);
        }

        public void BatchUnfreeze()
        {
            var freezeExpiredSubs = _subscriptionDAL.GetFreezeExpired();

            freezeExpiredSubs.ForEach(x => UnFreeze(x.Id));
        }

        public void BatchActivateByTime()
        {
            var subs = _subscriptionDAL.GetAllWithStatus((int)SubscriptionStatus.NotActivated);

            var ss = subs.Where(x => x.BuyDate.AddDays(14) <= DateTime.Today).ToList();

            foreach (var sub in ss)
            {
                //активация через 14 дней после покупки
                if (sub.BuyDate.AddDays(14) <= DateTime.Today)
                {
                    Activate(sub.Id, sub.BuyDate.AddDays(14));
                }
            }
        }

        public void BatchCloseSubscription()
        {
            var subs = _subscriptionDAL.GetAllWithStatus((int)SubscriptionStatus.Activated);

            var byTyme = subs.Where(x => x.DaysLeft < 0).ToList();
            //var byTrainingZero = 

            foreach (var sub in subs)
            {
                CloseByTimeLeft(sub);
                CloseByTrainingsLeft(sub);
            }
        }

        private void CloseByTimeLeft(Subscription sub)
        {
            if (sub.StatusId == (int)SubscriptionStatus.Activated && sub.DaysLeft < 0)
            {
                sub.StatusId = (int)SubscriptionStatus.Closed;
                Update(sub);
                Debug.WriteLine($"Закрыт по времени {sub.Id}");
            }
        }

        public void CloseByTrainingsLeft(Subscription sub)
        {
            if (sub.StatusId == (int)SubscriptionStatus.Activated)
            {
                int actualTiCount = sub.TrainingItems.Count(x => x.StatusId == (int)TrainingItemStatus.yes || x.StatusId == (int)TrainingItemStatus.no);
                if (actualTiCount >= sub.TrainingCount)
                {
                    sub.StatusId = (int)SubscriptionStatus.Closed;
                    Update(sub);
                    Debug.WriteLine($"Закрыт по окончанию тренировок {sub.Id} осталось {sub.TrainingCount - actualTiCount}");
                }
            }
        }

        public bool Delete(int id)
        {
            var sub = Get(id);

            if (sub.TrainingItems.Count == 0)
            {
                _subscriptionDAL.Delete(id);
                return true;
            }

            return false;
        }

        //public  static void Activate(int id, DateTime activateDate)
        //{
        //	_subscriptionDAL.Activate(id, activateDate);
        //          RecalculateValues(id);
        //      }

        public void Update(Subscription sub)
        {
            _subscriptionDAL.Update(sub);
            RecalculateValues(sub.Id);
        }

        public List<Subscription> GetForBranch(int branchId, bool includeRazovoe = false)
        {
            var subs = _subscriptionDAL.GetForBranch(branchId, includeRazovoe);
            return subs;
        }

        public List<Subscription> GetForBranch(int branchId, DateTime start, DateTime end, bool includeRazovoe = false)
        {
            var subs = _subscriptionDAL.GetForBranch(branchId, includeRazovoe, start, end);
            return subs;
        }

        public List<Subscription> GetAll()
        {
            return _subscriptionDAL.GetAll();
        }

        public List<Subscription> GetAll(DateTime? start, DateTime? end)
        {
            return _subscriptionDAL.GetAll(start, end);
        }

        public Subscription Get(int id)
        {
            return _subscriptionDAL.Get(id);
        }

        public Subscription GetRazovoe(int accId, int branchId)
        {
            var subs = _subscriptionDAL.GetForAccount(accId, branchId, true);

            return subs.Where(x => x.StatusId == (int)SubscriptionStatus.Razovoe).First();
        }

        //branchId == 0 - для всех филиалов
        public List<Subscription> GetForAccount(int accId, int branchId = 0)
        {
            var subs = _subscriptionDAL.GetForAccount(accId, branchId);

            return subs.Where(x => x.StatusId != (int)SubscriptionStatus.Razovoe).ToList();
        }

        public void AddFreeze(FreezeSubscriptionItem freeze)
        {
            Subscription sub = _subscriptionDAL.Get(freeze.SubscriptionId);

            if (sub.StatusId == (int)SubscriptionStatus.Activated)
            {
                sub.ActiveDays += freeze.FreezeDays;
                sub.StatusId = (int)SubscriptionStatus.Freezed;

                _subscriptionDAL.Update(sub);
                _subscriptionDAL.AddFreeze(freeze);
            }
        }

        public void UnFreeze(int subscriptionId)
        {
            Subscription sub = _subscriptionDAL.Get(subscriptionId);

            UnFreeze(sub);
        }

        public void UnFreeze(Subscription sub)
        {
            if (sub.StatusId == (int)SubscriptionStatus.Freezed)
            {
                var freeze = sub.FreezeSubscriptionList.First(x => x.Id == sub.FreezeSubscriptionList.Max(y => y.Id));

                if (freeze.Start <= DateTime.Today && freeze.End >= DateTime.Today)
                {
                    int daysDiff = (freeze.End - DateTime.Today).Days;

                    freeze.End = DateTime.Today;

                    sub.StatusId = (int)SubscriptionStatus.Activated;
                    sub.ActiveDays -= daysDiff;

                    _subscriptionDAL.Update(sub);
                    _subscriptionDAL.UpdateFreeze(freeze);
                }

                if (freeze.Start <= DateTime.Today && freeze.End <= DateTime.Today)
                {
                    sub.StatusId = (int)SubscriptionStatus.Activated;

                    _subscriptionDAL.Update(sub);
                    _subscriptionDAL.UpdateFreeze(freeze);
                }

                if (freeze.Start > DateTime.Today)
                {
                    sub.StatusId = (int)SubscriptionStatus.Activated;

                    _subscriptionDAL.Update(sub);
                    _subscriptionDAL.UpdateFreeze(freeze);
                }
            }
        }

        public void RecalculateValues(int id)
        {
            sw.Restart();

            var sub = _subscriptionDAL.Get(id);

            if (sub is null)
            {
                return;
            }

            //только те, которые учитываются в абонементе
            var actualTrainingItems = sub.TrainingItems.Where(x =>
            x.StatusId == (int)TrainingItemStatus.yes ||
            x.StatusId == (int)TrainingItemStatus.unKnown ||
            x.StatusId == (int)TrainingItemStatus.no).ToList();

            sub.TrainingsLeft = sub.TrainingCount - actualTrainingItems.Count;

            if (sub.TrainingsLeft < 0)
            {
                sub.TrainingsLeft = 0;
            }

            // Проверка даты активации

            if (sub.StatusId == (int)SubscriptionStatus.Activated && actualTrainingItems.Count > 0)
            {
                var dateActivateByDate = sub.BuyDate.AddDays(14);
                var firstTrainingDate = actualTrainingItems.Min(x => x.Date.Date);


                sub.ActivateDate = dateActivateByDate < firstTrainingDate ? dateActivateByDate : firstTrainingDate;
            }

            _subscriptionDAL.Update(sub);

            sw.Stop();

            Debug.WriteLine($"RecalculateValues {sw.ElapsedMilliseconds}");
        }

        public void Activate(int id, DateTime activateDate)
        {
            Subscription sub = Get(id);

            if (sub.StatusId == (int)SubscriptionStatus.NotActivated)
            {
                sub.ActivateDate = activateDate;

                _subscriptionDAL.Activate(id, activateDate);
            }
        }
    }
}