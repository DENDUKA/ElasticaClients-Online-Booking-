using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System;
using System.Linq;

namespace ConsoleApp1
{
    public static class SubscriptionMoves
    {
        public static void CreateOneTimeSubsForAllAccs()
        {
            var accs = Services.AccountDAL.GetAll();

            foreach (var acc in accs)
            {
                foreach (var branch in Services.BranchB.GetAll())
                {
                    var subs = Services.SubscriptionDAL.GetForAccount(acc.Id, branch.Id);

                    var razov = subs.Where(x => x.Name == "Разовый").FirstOrDefault();

                    if (razov == null)
                    {
                        Subscription sub = new Subscription();
                        sub.Name = "Разовый";
                        sub.BranchId = branch.Id;
                        sub.BuyDate = DateTime.Today;
                        sub.AccountId = acc.Id;

                        Services.SubscriptionDAL.Add(sub);
                        Console.WriteLine("sub " + sub.Id.ToString() + " " + branch.Id);
                    }
                    else
                    {
                        Console.WriteLine("Существует");
                    }
                }
            }
        }

        public static void RecalculateAllSubscription()
        {
            foreach (var x in Services.SubscriptionDAL.GetAll())
            {
                Services.SubscriptionB.RecalculateValues(x.Id);
                Console.WriteLine(x.Id);
            }
        }

        public static void SetRightSubscriptionStatus()
        {
            foreach (var sub in Services.SubscriptionDAL.GetAll())
            {
                if (sub.Name == "Разовый" && sub.StatusId != (int)SubscriptionStatus.Razovoe)
                {
                    sub.StatusId = (int)SubscriptionStatus.Razovoe;
                    Services.SubscriptionDAL.Update(sub);
                    Console.WriteLine($"{sub.Id} Обновлено");
                }
                else
                {
                    Console.WriteLine($"{sub.Id} Не требует обновления");
                }
            }
        }


        public static void SetRightSubWithBranch()
        {

            foreach (var ti in Services.TrainingItemDAL.GetAll().OrderBy(x => x.Id))
            {
                var training = Services.TrainingDAL.Get(ti.TrainingId);

                var branchId = training.Gym.BranchId;


                var subs = Services.SubscriptionDAL.GetForAccount(ti.AccountId, branchId);
                var razov = subs.Where(x => x.Name == "Разовый").FirstOrDefault();

                if (ti.SubscriptionId != razov.Id)
                {
                    ti.SubscriptionId = razov.Id;
                    Services.TrainingItemDAL.Update(ti);
                    Console.WriteLine($"{ti.Id} Обновлено");
                }
                else
                {
                    Console.WriteLine($"{ti.Id} Не требуется");
                }
            }
        }


        public static void SetRightSubscriptionForAllTrainingItems()
        {
            var tis = Services.TrainingItemDAL.GetAll();

            foreach (var ti in tis)
            {
                if (ti.Id == 14975)
                {
                }

                if (ti.Subscription.Name != "Разовый")
                {
                    var subs = Services.SubscriptionDAL.GetForAccount(ti.AccountId);
                    var razov = subs.Where(x => x.Name == "Разовый").FirstOrDefault();

                    if (razov != null)
                    {
                        ti.SubscriptionId = razov.Id;

                        Services.TrainingItemDAL.Update(ti);

                        Console.WriteLine(ti.Id + " обновлено");
                    }
                    else
                    {
                        Console.WriteLine("Абонепмента не существует");
                    }

                }
                else
                {
                    Console.WriteLine(ti.Id + " не требует обновления");
                }
            }
        }
    }
}