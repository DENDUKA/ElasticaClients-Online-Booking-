using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public static class Nakrutka
    {
        private static List<int> clients = new List<int>() { 7417, 7418, 7419, 7420, 7421, 7422, 7423, 7424, 7425, 7426, 7427, 7428, 7429, 7430, 7431, 7432, 7433, 7434, 7435 };
        private static int engelsId = 4;
        private static int gymEngId = 10;
        private static List<int> abNum = new List<int>() { 6, 8, 10, 12 };

        private static Dictionary<int, int> abonementPay = new Dictionary<int, int>()
        {
            { 6, 1950 },
            { 8, 2200 },
            { 10, 2950 },
            { 12, 3500 },
        };

        private static Dictionary<int, int> monthCup = new Dictionary<int, int>()
        {
            { 11, 46000 },
            { 12, 40000 },
            { 1, 50000 },
            { 2, 20000 },
            { 3, 70000 },
        };

        public static void Go(DateTime start, DateTime end)
        {
            //var trs = ElasticaClients.Logic.TrainingB.GetAllForGym(gymEngId, start, end);


            foreach (var m in monthCup)
            {
                ForMonth(m.Key);
            }





        }

        public static void ForMonth(int m)
        {
            int payd = 0;

            var rand = new Random();

            int y = (m == 11 || m == 12) ? 2021 : 2022;

            foreach (var clientId in clients)
            {

                var d = rand.Next(1, 28);
                var subDate = new DateTime(y, m, d);
                payd += CreateSub(clientId, subDate);

                if (payd >= monthCup[m])
                {
                    Console.WriteLine($"Достаточно накрутили");
                    break;
                }
            }

            foreach (var clientId in clients)
            {
                var subDate = new DateTime(y, m, rand.Next(26));
                payd += CreateSub(clientId, subDate);

                if (payd >= monthCup[m])
                {
                    Console.WriteLine($"Достаточно накрутили");
                    break;
                }
            }

            Console.WriteLine($"Пользователи кончились");


        }

        public static int CreateSub(int clientId, DateTime nextSubDate)
        {
            var rand = new Random();
            var abnum = rand.Next(0, 3);

            var abcost = abonementPay[abNum[abnum]];

            Subscription newSub = new Subscription()
            {
                AccountId = clientId,
                ActiveDays = 30,
                BranchId = engelsId,
                ByCash = false,
                Cost = abcost,
                BuyDate = nextSubDate,
                StatusId = (int)SubscriptionStatus.Activated,
                TrainingCount = abNum[abnum],
                ActivateDate = nextSubDate,
            };

            ElasticaClients.Logic.SubscriptionB.Add(newSub);

            Console.WriteLine($"Добавлено на {newSub.Cost}");

            return newSub.Cost;
        }
    }
}
