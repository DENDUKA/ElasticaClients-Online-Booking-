using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using YClientsAPI;

namespace ConsoleApp1
{
    public static class Functions
    {
        public static void BindingPhoneInBD(List<ClientJSON> clientsEx)
        {
            var noFillingAccs = AccountB.GetAll().Where(x => x.Phone == "0000000000").ToList();

            Console.WriteLine($"Не заполненных телефонов : {noFillingAccs.Count}");

            foreach (var x in clientsEx)
            {
                x.Name = x.Name.ToLower();
            }

            int i = 0;

            foreach (var acc in noFillingAccs)
            {



                var nameSureName = acc.Name.ToLower().Split(' ');
                if (nameSureName.Length != 2)
                {
                    Console.WriteLine($"Имя не полное :{acc.Name}");
                    continue;
                }
                else
                {
                    bool b = false;

                    foreach (var cEx in clientsEx)
                    {
                        if (cEx.Name.Contains(nameSureName[0]) && cEx.Name.Contains(nameSureName[1]))
                        {
                            i++;

                            acc.Email = cEx.Email;
                            acc.Phone = cEx.Phone.Replace("+7", "");

                            AccountB.Update(acc);

                            Console.WriteLine($"НАйдено !! {acc.Name}");
                            b = true;
                            break;
                        }
                    }

                    if (!b)
                    {
                        Console.WriteLine($"Не нашли {acc.Name}");
                    }

                }




            }
        }

        public static void CreateRazovoeForAll()
        {
            var accs = AccountB.GetAll();
            var branches = BranchB.GetAll();

            foreach (var acc in accs)
            {
                var subs = SubscriptionB.GetAll().Where(s => s.StatusId == (int)SubscriptionStatus.Razovoe && s.AccountId == acc.Id).ToList();

                if (subs.Count < branches.Count)
                {
                    foreach (var b in branches)
                    {
                        if (subs.Any(x => x.StatusId == (int)SubscriptionStatus.Razovoe))
                        {

                        }
                        else
                        {
                            Subscription razSub = new Subscription();

                            razSub.BuyDate = DateTime.Today.Date;
                            razSub.BranchId = b.Id;
                            razSub.AccountId = acc.Id;
                            razSub.Cost = 0;
                            razSub.StatusId = (int)SubscriptionStatus.Razovoe;

                            SubscriptionB.Add(razSub);

                            Console.WriteLine($"Для {acc.Name} создан разовый аб в {b.Name}");
                        }
                    }
                }
                else if (subs.Count > branches.Count)
                {

                }
            }
        }
    }
}
