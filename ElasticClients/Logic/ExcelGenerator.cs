using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElasticaClients.DAL.Entities;
using ElasticaClients.DAL.Accessory;
using ElasticaClients.Models;
using ElasticaClients.DAL.Data.Interfaces;

namespace ElasticaClients.Logic
{
    public class ExcelGenerator
    {
        private string individ = "индивид".ToLower();
        private Dictionary<int, Pair> ostalisByTrainer = new Dictionary<int, Pair>();
        private int TrainingValueR = 47;
        private int TrainingValueC = 20;

        private int TrainingValueInfoR = 47;
        private int TrainingValueInfoC = 13;

        private int SubsR = 67;
        private int SubsC = 25;

        private int TrainingR = 67;
        private int TrainingC = 40;

        private int IncomeR = 67;
        private int IncomeC = 40;

        private int Month = 0;
        private int Year = 0;

        private int BranchId;
        private DateTime dateStart;
        private DateTime dateEnd;

        private Dictionary<int, Account> accounts;
        private Dictionary<int, Subscription> subscriptions;

        private readonly IAccountDAL _accountDAL;
        private readonly IBranchDAL _branchDAL;
        private readonly ITrainingDAL _trainingDAL;
        private readonly ISubscriptionDAL _subscriptionDAL;
        private readonly IIncomeDAL _incomeDAL;
        private readonly SubscriptionB _subscriptionB;
        private readonly IncomeB _incomeB;
        private readonly BranchB _branchB;
        private readonly TrainingB _trainingB;
        private readonly TrainingItemB _trainingItemB;

        public ExcelGenerator(IAccountDAL accountDAL, IBranchDAL branchDAL, ITrainingDAL trainingDAL, ISubscriptionDAL subscriptionDAL, IIncomeDAL incomeDAL, SubscriptionB subscriptionB, IncomeB incomeB, BranchB branchB, TrainingB trainingB, TrainingItemB trainingItemB)
        {
            _accountDAL = accountDAL;
            _branchDAL = branchDAL;
            _trainingDAL = trainingDAL;
            _subscriptionDAL = subscriptionDAL;
            _incomeDAL = incomeDAL;
            _subscriptionB = subscriptionB;
            _incomeB = incomeB;
            _branchB = branchB;
            _trainingB = trainingB;
            _trainingItemB = trainingItemB;
            accounts = _accountDAL.GetAllLight();
        }

        public async Task<string> MonthReportAsync(int branchId, int year, int month)
        {
            var basePath = $"C:\\Server\\dataEL\\Excel\\Base.xlsx";
            var filePath = $"C:\\Server\\dataEL\\Excel\\{Guid.NewGuid()}.xlsx";
            File.Copy(basePath, filePath);

            await Task.Run(() =>
            {

                Month = month;
                Year = year;
                BranchId = branchId;

                Application excel = new Application();
                Workbook wb = excel.Workbooks.Open(filePath);
                var wsheet = (Worksheet)wb.Sheets[1];

                dateStart = new DateTime(year, month, 1);
                dateEnd = dateStart.AddMonths(1);

                var branch = _branchDAL.Get(branchId);

                var trainings = new List<Training>();

                foreach (var gym in branch.Gyms)
                {
                    trainings.AddRange(_trainingDAL.GetAllForGym(gym.Id, dateStart, dateEnd));
                }

                trainings = trainings.OrderBy(x => x.StartTime).ToList();

                for (int i = 0; i < trainings.Count; i++)
                {
                    trainings[i] = _trainingDAL.Get(trainings[i].Id);
                }

                wsheet.Cells[27, 2] = DateTime.DaysInMonth(year, month);

                var subs = _subscriptionB.GetForBranch(branchId, dateStart.AddMonths(-12), dateEnd.AddYears(3));

                subscriptions = subs.ToDictionary(x => x.Id, x => x);

                for (int d = 1; d <= DateTime.DaysInMonth(year, month); d++)
                {
                    var trainingsDay = TrainingsDay(trainings, d, month);

                    var allCount = 0;
                    var byAbon = 0;
                    var razovoe = 0;
                    var trial = 0;
                    var ostalsa = 0;
                    var fired = 0;

                    foreach (var t in trainingsDay)
                    {
                        if (t.Name[1] != individ[1] && t.Name[2] != individ[2])
                        {
                            var trialTI = TrialTIs(t.TrainingItems);
                            var abonTI = BySubscription(t.TrainingItems);
                            var ostalis = Ostalis(trialTI);
                            var razovoeTI = RazovoeTIs(t.TrainingItems);
                            var firedTI = FiredBySubscription(abonTI);

                            allCount += AllValueTrainingItems(t.TrainingItems).Count;
                            byAbon += abonTI.Count;
                            fired += firedTI.Count;
                            razovoe += razovoeTI.Count;
                            trial += trialTI.Count;
                            ostalsa += ostalis.Count;

                            TrialByTrainer(trialTI, ostalis);

                            TrainingValue(abonTI, razovoeTI, trialTI, firedTI, wsheet);
                        }
                        else
                        {
                        }
                    }

                    wsheet.Cells[3, 1 + d] = byAbon;
                    wsheet.Cells[4, 1 + d] = razovoe;
                    wsheet.Cells[5, 1 + d] = trial;
                    wsheet.Cells[6, 1 + d] = fired;
                    wsheet.Cells[7, 1 + d] = ostalsa;
                    wsheet.Cells[8, 1 + d] = subscriptions.Where(x => x.Value.BuyDate.Date == new DateTime(year, month, d)).Count();
                }

                WriteOstalisByTrainer(wsheet);

                //WriteByTraining(trainings, wsheet);

                WriteSubs(subs, wsheet);

                WriteAllInOutCome(wsheet);

                wb.Save();
                wb.Close();
            });

            return filePath;
        }

        private void WriteAllInOutCome(Worksheet wsheet)
        {
            var incomes = new IncomeTotalModel(_incomeB, _branchB, _trainingB, _subscriptionB, _trainingItemB, BranchId, dateStart, dateEnd).AllIncomes;

            var incomeByType = incomes.GroupBy(x => x.Type).ToDictionary(x => x.Key, x => x.ToList().Sum(y => y.Cost));

            IncomeType key = (IncomeType)0;
            wsheet.Cells[IncomeR, IncomeC] = "ЗП";
            wsheet.Cells[IncomeR, IncomeC + 1] = incomeByType.ContainsKey(key) ? incomeByType[key] : 0;
            IncomeR++;
            key = (IncomeType)1;
            wsheet.Cells[IncomeR, IncomeC] = "Промо";
            wsheet.Cells[IncomeR, IncomeC + 1] = incomeByType.ContainsKey(key) ? incomeByType[key] : 0;
            IncomeR++;
            key = (IncomeType)3;
            wsheet.Cells[IncomeR, IncomeC] = "Аренда";
            wsheet.Cells[IncomeR, IncomeC + 1] = incomeByType.ContainsKey(key) ? incomeByType[key] : 0;
            IncomeR++;
            key = (IncomeType)4;
            wsheet.Cells[IncomeR, IncomeC] = "СубАренда";
            wsheet.Cells[IncomeR, IncomeC + 1] = incomeByType.ContainsKey(key) ? incomeByType[key] : 0;
            IncomeR++;
            key = (IncomeType)5;
            wsheet.Cells[IncomeR, IncomeC] = "Абонементы";
            wsheet.Cells[IncomeR, IncomeC + 1] = incomeByType.ContainsKey(key) ? incomeByType[key] : 0;
            IncomeR++;
            key = (IncomeType)6;
            wsheet.Cells[IncomeR, IncomeC] = "По Разовому";
            wsheet.Cells[IncomeR, IncomeC + 1] = incomeByType.ContainsKey(key) ? incomeByType[key] : 0;
            IncomeR++;
            key = (IncomeType)7;
            wsheet.Cells[IncomeR, IncomeC] = "Другое";
            wsheet.Cells[IncomeR, IncomeC + 1] = incomeByType.ContainsKey(key) ? incomeByType[key] : 0;
            IncomeR++;
        }

        private void WriteByTraining(List<Training> trainings, Worksheet wsheet)
        {
            int count = 0;
            foreach (var t in trainings)
            {
                if (t.StatusId != (int)TrainingStatus.Active)
                {
                    continue;
                }

                float income = 0;
                float zp = 0;


                foreach (var ti in t.TrainingItems)
                {
                    if (subscriptions.ContainsKey(ti.SubscriptionId))
                    {
                        ti.Subscription = subscriptions[ti.SubscriptionId];
                    }
                    else
                    {
                        ti.Subscription = _subscriptionDAL.Get(ti.SubscriptionId);
                    }

                    if (ti.Razovoe || ti.IsTrial)
                    {
                        if (ti.StatusId == (int)TrainingItemStatus.yes)
                        {
                            income += ti.Cost;
                        }
                    }
                    else
                    {
                        income += ti.Subscription.Cost / ti.Subscription.TrainingCount;
                    }
                }

                if (income == 0)
                {
                    continue;
                }

                zp += t.TrainerPay;

                wsheet.Cells[TrainingR, TrainingC] = t.Name;
                wsheet.Cells[TrainingR, TrainingC + 1] = t.StartTime;
                wsheet.Cells[TrainingR, TrainingC + 2] = income;
                wsheet.Cells[TrainingR, TrainingC + 3] = zp;

                count++;
                TrainingR++;
            }

            var outcome = _incomeDAL.GetAll(BranchId, dateStart, dateEnd).Sum(x => x.Cost);

            wsheet.Cells[TrainingR - count - 8, TrainingC + 4] = count;
            wsheet.Cells[TrainingR - count - 7, TrainingC + 4] = outcome / count;


            for (int i = 1; i <= count; i++)
            {
                wsheet.Cells[TrainingR - i, TrainingC + 4] = outcome / count;
            }
        }

        private void WriteSubs(List<Subscription> subs, Worksheet wsheet)
        {
            var subsByAcc = subs.GroupBy(x => x.AccountId).ToDictionary(g => g.Key, g => g.ToList());
            int col = SubsC;

            foreach (var kv in subsByAcc)
            {
                wsheet.Cells[SubsR, SubsC] = subsByAcc[kv.Key][0].Account.Name;

                var accSubs = kv.Value.OrderBy(x => x.BuyDate).ToList();

                StatusOfSub(accSubs, wsheet);

                foreach (var v in accSubs)
                {
                    if ((v.DateEnd < dateEnd && v.DateEnd >= dateStart) ||
                        (v.ActivateDate < dateEnd && v.ActivateDate >= dateStart) ||
                        (v.ActivateDate <= dateStart && v.DateEnd > dateEnd))
                    {
                        wsheet.Cells[SubsR, col + 1] = v.ActivateDate;
                        wsheet.Cells[SubsR, col + 2] = v.DateEnd;
                        wsheet.Cells[SubsR, col + 4] = v.Cost;

                        SubsR++;
                    }
                }

                //Debug.WriteLine($"{kv.Key}");

                //    Debug.WriteLine($"{v.Name} {v.Id}");
                //}

                //SubsR++;
            }
        }

        private void StatusOfSub(List<Subscription> subs, Worksheet wsheet)
        {
            var lastSubInMonthI = subs.FindLastIndex(x => x.DateEnd >= dateStart && x.DateEnd < dateEnd);

            var subFirstI = subs.FindIndex(x => x.DateEnd >= dateStart && x.DateEnd < dateEnd);

            var crossSubI = subs.FindIndex(x => x.ActivateDate < dateStart && x.DateEnd > dateEnd);

            var activateSubI = subs.FindIndex(x => x.ActivateDate >= dateStart && x.ActivateDate < dateEnd);

            if (crossSubI != -1)
            {
                //Активен на протяжении всего месяца
                wsheet.Cells[SubsR, SubsC + 3] = 1;
                return;
            }

            if (subs[0].ActivateDate >= dateStart && subs[0].ActivateDate < dateEnd)
            {
                //первый аб
                wsheet.Cells[SubsR, SubsC + 3] = 3;
                wsheet.Cells[SubsR, SubsC + 5] = subs.Count;
                return;
            }

            if (lastSubInMonthI != -1)
            {
                if (lastSubInMonthI == subs.Count - 1)
                {
                    //не продлил
                    wsheet.Cells[SubsR, SubsC + 3] = 0;
                    return;
                }
                else
                {
                    if (subs[lastSubInMonthI + 1].BuyDate <= subs[lastSubInMonthI].DateEnd.AddDays(4))
                    {
                        //За 3 дня продлил
                        wsheet.Cells[SubsR, SubsC + 3] = 2;
                        return;
                    }
                    else if (subs[lastSubInMonthI + 1].BuyDate <= subs[lastSubInMonthI].DateEnd.AddMonths(1))
                    {
                        //продление меньше месяца
                        wsheet.Cells[SubsR, SubsC + 3] = 4;
                        return;
                    }
                    else
                    {
                        //продление больше месяца
                        wsheet.Cells[SubsR, SubsC + 3] = 5;
                        return;
                    }
                }
            }

            if (subs[0].Account.Name == "Копертехина Анна")
            {

            }

            if (activateSubI != -1)
            {
                if (activateSubI == 0)
                {
                    //первый аб
                    wsheet.Cells[SubsR, SubsC + 3] = 3;
                    wsheet.Cells[SubsR, SubsC + 5] = subs.Count;
                    return;
                }
                else
                {
                    var days = (subs[activateSubI].Date - subs[activateSubI - 1].DateEnd).Days;
                    if (days <= 3)
                    {
                        //За 3 дня продлил
                        wsheet.Cells[SubsR, SubsC + 3] = 2;
                        return;
                    }
                    else if (days <= 31)
                    {
                        //продление меньше месяца
                        wsheet.Cells[SubsR, SubsC + 3] = 4;
                        return;
                    }
                    else
                    {
                        //продление больше месяца
                        wsheet.Cells[SubsR, SubsC + 3] = 5;
                        return;
                    }
                }
            }

            //Аб активен на протяжении всего месяца
            //wsheet.Cells[SubsR, SubsC + 3] = "???";
        }

        private void TrainingValue(List<TrainingItem> abonTI, List<TrainingItem> razovoeTI, List<TrainingItem> trialTI, List<TrainingItem> firedTI, Worksheet wsheet)
        {
            var subsSum = 0f;
            var razSum = 0f;
            var trialSum = 0f;
            var firedSum = 0f;
            var countPeaple = abonTI.Count + razovoeTI.Count + trialTI.Count;

            foreach (var s in abonTI)
            {
                var sub = _subscriptionDAL.Get(s.SubscriptionId);

                var costOne = sub.Cost / (float)sub.TrainingCount;
                subsSum += costOne;

                wsheet.Cells[TrainingValueInfoR, TrainingValueInfoC + 1] = costOne;
                TrainingValueInfoR++;
            }

            foreach (var ti in razovoeTI)
            {
                var costOne = ti.RazovoeCost;
                razSum += costOne;

                wsheet.Cells[TrainingValueInfoR, TrainingValueInfoC + 2] = costOne;
                TrainingValueInfoR++;
            }

            foreach (var ti in trialTI)
            {
                var costOne = ti.RazovoeCost;
                trialSum += costOne;

                wsheet.Cells[TrainingValueInfoR, TrainingValueInfoC + 3] = costOne;
                TrainingValueInfoR++;
            }

            foreach (var ti in firedTI)
            {
                var sub = _subscriptionDAL.Get(ti.SubscriptionId);

                var costOne = sub.Cost / (float)sub.TrainingCount;
                firedSum += costOne;

                wsheet.Cells[TrainingValueInfoR, TrainingValueInfoC + 4] = costOne;
                TrainingValueInfoR++;
            }

            TrainingValueInfoR++;
        }

        private void WriteOstalisByTrainer(Worksheet wsheet)
        {
            int startL = 46;
            int startC = 2;

            foreach (var kv in ostalisByTrainer)
            {
                var trainer = accounts[kv.Key];
                wsheet.Cells[startL, startC] = trainer.Name;
                wsheet.Cells[startL + 1, startC] = kv.Value.X;
                wsheet.Cells[startL + 2, startC] = kv.Value.Y;

                startC++;
            }
        }

        private Dictionary<int, Pair> TrialByTrainer(List<TrainingItem> trialTI, List<TrainingItem> ostalisTI)
        {
            foreach (var trial in trialTI)
            {
                if (ostalisByTrainer.ContainsKey(trial.Training.TrainerId))
                {
                    ostalisByTrainer[trial.Training.TrainerId].X++;
                }
                else
                {
                    ostalisByTrainer.Add(trial.Training.TrainerId, new Pair() { X = 1 });
                }
            }

            foreach (var ti in ostalisTI)
            {
                var acc = accounts[ti.AccountId];

                if (acc.Subscriptions.Where(x => x.Cost > 0).Count() >= 1)
                {
                    ostalisByTrainer[ti.Training.TrainerId].Y++;
                }
            }

            return ostalisByTrainer;
        }

        private List<TrainingItem> FiredBySubscription(List<TrainingItem> abonTI)
        {
            var fired = abonTI.Where(x => x.StatusId == (int)TrainingItemStatus.no).ToList();
            return fired;
        }

        private List<TrainingItem> Ostalis(List<TrainingItem> trainingItems)
        {
            List<TrainingItem> res = new List<TrainingItem>();
            foreach (var ti in trainingItems)
            {
                var acc = _accountDAL.Get(ti.AccountId);

                if (acc.Subscriptions.Where(x => x.Cost > 0).Count() >= 1)
                {
                    res.Add(ti);
                }
            }

            return res;
        }

        /// <summary>
        /// Все значимые TrainingItems , где были уплачены $
        /// </summary>
        private List<TrainingItem> AllValueTrainingItems(List<TrainingItem> trainingItems)
        {
            return trainingItems
                .Where(x => (x.StatusId == (int)TrainingItemStatus.yes) ||
                (x.StatusId == (int)TrainingItemStatus.no && !x.Razovoe)).ToList();
        }

        private List<TrainingItem> BySubscription(List<TrainingItem> trainingItems)
        {
            return trainingItems.Where(x => !x.Razovoe && !x.IsTrial).ToList();
        }

        private List<TrainingItem> RazovoeTIs(List<TrainingItem> trainingItems)
        {
            return trainingItems.Where(x => x.Razovoe && !x.IsTrial).ToList();
        }

        private static List<TrainingItem> TrialTIs(List<TrainingItem> trainingItems)
        {
            return trainingItems.Where(x => x.IsTrial).ToList();
        }

        private static List<Training> TrainingsDay(List<Training> trainings, int day, int month)
        {
            return trainings
                            .Where(
                                t => t.StartTime.Month == month &&
                                t.StartTime.Day == day &&
                                t.StatusId == (int)TrainingStatus.Active).ToList();
        }
    }

    public class Pair
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}