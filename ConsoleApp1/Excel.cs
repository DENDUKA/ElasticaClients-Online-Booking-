using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using IronXL;
using System;
using System.Collections.Generic;
using System.Linq;
using YClientsAPI;

namespace ConsoleApp1
{
    public class Excel
    {
        public List<ExcelFormat> ImportAccs()
        {
            List<ExcelFormat> accs = new List<ExcelFormat>();


            WorkBook workbook = WorkBook.Load(@"E:\Programming\FlyStretch\orzNames.xlsx");

            WorkSheet sheet = workbook.WorkSheets.First();

            foreach (var x in sheet.Rows)
            {
                var cells = x.ToList();
                if (string.IsNullOrEmpty(cells[0].StringValue))
                {
                    return accs;
                }

                ExcelFormat accInfo = new ExcelFormat();

                accInfo.name = cells[0].StringValue;
                accInfo.inst = cells[1].StringValue;


                accs.Add(accInfo);
            }

            return accs;
        }

        internal static void FillExcelByClient(List<YClientsAPI.ClientJSON> clients)
        {
            string path = @"E:\Programming\FlyStretch\YClientAccounts.xlsx";

            WorkBook workbook = WorkBook.Load(path);
            WorkSheet sheet = workbook.WorkSheets.First();

            int i = 1;

            foreach (var x in clients)
            {
                sheet["A" + i.ToString()].Value = x.Id;
                sheet["B" + i.ToString()].Value = x.Name;
                sheet["C" + i.ToString()].Value = x.Phone;
                sheet["D" + i.ToString()].Value = x.Email;
                i++;
            }

            workbook.Save();

        }

        public List<ClientJSON> ImportClients()
        {
            string path = @"E:\Programming\FlyStretch\YClientAccounts.xlsx";

            WorkBook workbook = WorkBook.Load(path);

            WorkSheet sheet = workbook.WorkSheets.First();

            List<ClientJSON> clients = new List<ClientJSON>();

            int i = 0;

            foreach (var x in sheet.Rows)
            {
                var cells = x.ToList();

                ClientJSON client = new ClientJSON();
                client.Id = cells[0].StringValue;
                client.Name = cells[1].StringValue;
                client.Phone = cells[2].StringValue;
                client.Email = cells[3].StringValue;

                clients.Add(client);

                Console.WriteLine(i++);
            }

            return clients;

        }

        public List<ExcelFormat> Import()
        {
            List<ExcelFormat> accs = new List<ExcelFormat>();

            WorkBook workbook = WorkBook.Load(@"C:\Server\data\OR3Test.xlsx");

            WorkSheet sheet = workbook.WorkSheets.First();

            foreach (var x in sheet.Rows)
            {
                var cells = x.ToList();
                if (string.IsNullOrEmpty(cells[0].StringValue))
                {
                    return accs;
                }

                ExcelFormat accInfo = new ExcelFormat();

                accInfo.name = cells[0].StringValue;

                for (int i = 2; i < 14; i++)
                {
                    if (cells[i].IsDateTime)
                    {
                        if (cells[i].Style.BackgroundColor == "#ffffff")
                        {
                            accInfo.trainingsDates.Add((DateTime)cells[i].DateTimeValue);
                        }
                        else
                        {
                            accInfo.notrainingsDates.Add((DateTime)cells[i].DateTimeValue);
                        }
                    }
                }

                accInfo.trainingCount = cells[16].Int32Value;

                if (cells[17].IsDateTime)
                {
                    accInfo.DateStart = (DateTime)cells[17].DateTimeValue;
                }
                else
                {
                    accInfo.DateStart = null;
                }

                if (cells[18].IsDateTime)
                {
                    accInfo.DateEnd = (DateTime)cells[18].DateTimeValue;
                }
                else
                {
                    accInfo.DateEnd = null;
                }

                //accInfo.Tel = cells[18].StringValue;
                accInfo.inst = cells[1].StringValue;

                accs.Add(accInfo);
            }

            return accs;
        }


        private int branchId = 5;//центр
        private int gymId = 11;//центр
        private int trainerId = 6276;//9na

        internal void ToBD(List<ExcelFormat> subs)
        {
            var accs = new AccountDAL().GetAll();

            foreach (var x in subs)
            {
                var currentAcc = CreateAcc(x);

                var currentSub = CreateSub(x, currentAcc);

                if (currentSub == null)
                {
                    continue;
                }

                foreach (var t in x.trainingsDates)
                {
                    var currentTraining = CreateTraining(t);

                    var currentTrainingItem = CreateTrainingItem(currentSub, currentAcc, currentTraining, (int)TrainingItemStatus.yes);
                }

                foreach (var t in x.notrainingsDates)
                {
                    var currentTraining = CreateTraining(t);

                    var currentTrainingItem = CreateTrainingItem(currentSub, currentAcc, currentTraining, (int)TrainingItemStatus.no);
                }
            }
        }

        internal void toDBAccs(List<ExcelFormat> accsEx)
        {
            var accs = new AccountDAL().GetAll();

            foreach (var x in accsEx)
            {
                var currentAcc = CreateAcc(x);
            }
        }

        private TrainingItem CreateTrainingItem(Subscription sub, Account acc, Training training, int status)
        {
            var tis = TrainingItemB.GetAllForAccount(acc.Id);

            if (tis.Any(x => x.TrainingId == training.Id))
            {
                var ti = tis.First(x => x.TrainingId == training.Id);

                Console.WriteLine($"Запись {ti.Id} найдена");

                return ti;
            }
            else
            {
                TrainingItem ti = new TrainingItem();
                ti.AccountId = acc.Id;
                ti.SubscriptionId = sub.Id;
                ti.TrainingId = training.Id;

                TrainingItemB.Add(ti);

                Console.WriteLine($"Запись {ti.Id} СОЗДАНА");

                return ti;
            }
        }

        private Training CreateTraining(DateTime t)
        {
            var trainings = TrainingB.GetAllForGym(gymId);

            if (trainings.Any(x => x.StartTime == t))
            {
                var tr = trainings.First(x => x.StartTime == t);

                Console.WriteLine($"Тренировка {t} найдена");

                return tr;
            }
            else
            {
                Training currentTraining = new Training();

                currentTraining.GymId = gymId;
                currentTraining.Name = "EL Trainings";
                currentTraining.Seats = 10;
                currentTraining.StartTime = t;
                currentTraining.StatusId = (int)TrainingStatus.Active;
                currentTraining.TrainerId = trainerId;

                new TrainingDAL().Add(currentTraining);

                Console.WriteLine($"Тренировка {t} СОЗДАНА");

                return currentTraining;
            }
        }

        private Subscription CreateSub(ExcelFormat x, Account currentAcc)
        {
            var accSubs = new SubscriptionDAL().GetForAccount(currentAcc.Id);

            DateTime buyDate;

            try
            {
                buyDate = x.DateStart != null ? (DateTime)x.DateStart : x.trainingsDates.Concat(x.notrainingsDates).Min();
            }
            catch (Exception)
            {
                return null;
            }

            Subscription currentSub;

            if (accSubs.Any(y => y.BuyDate == buyDate))
            {
                currentSub = accSubs.First(y => y.BuyDate == buyDate);

                Console.WriteLine($"Абонемент {currentSub.BuyDate} найден");
            }
            else
            {
                currentSub = new Subscription();

                currentSub.AccountId = currentAcc.Id;
                currentSub.BranchId = branchId;
                currentSub.BuyDate = buyDate;
                currentSub.Name = "Приобретен до 01.11.2021";
                currentSub.TrainingCount = x.trainingCount;

                if (x.DateStart != null && x.DateEnd == null)
                {
                    currentSub.StatusId = (int)SubscriptionStatus.NotActivated;
                    currentSub.BuyDate = (DateTime)x.DateStart;

                    Console.WriteLine($"Абонемент {currentSub.BuyDate} создан НЕАКТИВИРОВАННЫМ");
                }

                if (x.DateStart == null && x.DateEnd == null)
                {
                    DateTime dateend = x.trainingsDates.Concat(x.notrainingsDates).Max();

                    currentSub.ActivateDate = buyDate;
                    currentSub.ActiveDays = ((TimeSpan)(dateend - buyDate)).Days;
                    currentSub.StatusId = (int)SubscriptionStatus.Closed;

                    Console.WriteLine($"Абонемент {currentSub.BuyDate} создан ЗАВЕРШЕННЫМ");
                }

                if (x.DateStart != null && x.DateEnd != null)
                {
                    currentSub.ActivateDate = x.DateStart;
                    currentSub.ActiveDays = ((TimeSpan)(x.DateEnd - x.DateStart)).Days;
                    currentSub.StatusId = (int)SubscriptionStatus.Activated;

                    Console.WriteLine($"Абонемент {currentSub.BuyDate} создан АКТИВИРОВАННЫМ");

                    if (x.DateEnd < DateTime.Today)
                    {
                        currentSub.StatusId = (int)SubscriptionStatus.Closed;
                        Console.WriteLine($"Абонемент {currentSub.BuyDate} создан ЗАВЕРШЕННЫМ");
                    }


                }

                new SubscriptionDAL().Add(currentSub);
            }

            return currentSub;
        }

        private Account CreateAcc(ExcelFormat x)
        {
            var accs = new AccountDAL().GetAll();

            Account currentAcc = null;

            if (accs.Any(y => y.Name == x.name))
            {
                Console.WriteLine($"Аккаунт {x.name} уже в базе");
                currentAcc = accs.First(y => y.Name == x.name);
            }
            else
            {
                currentAcc = new Account();
                currentAcc.Name = x.name;
                currentAcc.Instagram = x.inst;
                currentAcc.Phone = "0000000000";
                currentAcc.RoleId = Role.clientId;

                new AccountDAL().Add(currentAcc);
                Console.WriteLine($"Аккаунт {x.name} был создан");
            }

            return currentAcc;
        }

        public static void FillInfo()
        {
            var accs = AccountB.GetAll().Where(x => x.Phone == "0000000000");

            Console.WriteLine($"Не заполнено телефонов : {accs.Count()}");
        }
    }
}
