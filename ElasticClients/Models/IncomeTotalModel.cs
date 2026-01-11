using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ElasticaClients.Models
{
    public class IncomeTotalModel
    {
        private Stopwatch sw = new Stopwatch();
        private readonly IncomeB _incomeB;
        private readonly BranchB _branchB;
        private readonly TrainingB _trainingB;
        private readonly SubscriptionB _subscriptionB;
        private readonly TrainingItemB _trainingItemB;

        public IncomeTotalModel(IncomeB incomeB, BranchB branchB, TrainingB trainingB, SubscriptionB subscriptionB, TrainingItemB trainingItemB, int branchId, DateTime start, DateTime end)
        {
            _incomeB = incomeB;
            _branchB = branchB;
            _trainingB = trainingB;
            _subscriptionB = subscriptionB;
            _trainingItemB = trainingItemB;

            Trainings = new List<Training>();

            BranchId = branchId;
            Start = start;
            End = end;

            LoadIncomes();
            LoadSubscriptions();
            LoadRazovoes();
            LoadSalary();

            SetAllIncome();
        }

        public List<IIncome> AllIncomes { get; set; }

        public List<Income> Incomes { get; set; }

        public List<Subscription> Subscriptions { get; set; }

        public List<TrainingItem> Razovoes { get; set; }

        public List<Training> Trainings { get; set; }

        public int BranchId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        private int salaryTotal = 0;
        public int SalaryTotal
        {
            get
            {
                return salaryTotal;
            }
        }

        private int subscriptionsTotal = 0;
        public int SubscriptionsTotal
        {
            get
            {
                return subscriptionsTotal;
            }
        }

        private int razovoesTotal = 0;
        public int RazovoesTotal
        {
            get
            {
                return razovoesTotal;
            }
        }

        public int TotalIncome
        {
            get
            {
                return AllIncomes.Sum(x => x.Cost);
            }
        }

        public void SetAllIncome()
        {
            sw.Restart();

            AllIncomes = new List<IIncome>();

            foreach (var inc in Incomes)
            {
                AllIncomes.Add(inc);
            }

            foreach (var sub in Subscriptions)
            {
                AllIncomes.Add(sub);
            }

            foreach (var raz in Razovoes)
            {
                AllIncomes.Add(raz);
            }

            var trainersPay = Trainings.GroupBy(x => x.Trainer.Name).Select(s => new { Name = s.Key, Salary = s.Sum(z => z.TrainerPay) });

            foreach (var tp in trainersPay)
            {
                AllIncomes.Add(new Salary() { Cost = tp.Salary * -1, Date = End, IncomeId = 0, IncomeName = $"{tp.Name} Зарплата", Type = IncomeType.Salary });
            }

            AllIncomes = AllIncomes.OrderBy(x => x.Date).ToList();

            sw.Stop();
            Debug.WriteLine($"SetAllIncome() {sw.ElapsedMilliseconds}");
        }

        private void LoadIncomes()
        {
            sw.Restart();

            Incomes = _incomeB.GetAll(BranchId, Start, End);

            sw.Stop();
            Debug.WriteLine($"LoadIncomes() {sw.ElapsedMilliseconds}");
        }

        private void LoadSalary()
        {
            sw.Restart();

            var branch = _branchB.Get(BranchId);

            foreach (var gym in branch.Gyms)
            {
                Trainings.AddRange(_trainingB.GetAllForGym(gym.Id, Start, End));
            }

            Trainings = Trainings.Where(x => x.StatusId == (int)TrainingStatus.Active).ToList();
            salaryTotal = Trainings.Sum(x => x.TrainerPay);

            sw.Stop();
            Debug.WriteLine($"LoadSubscriptions() {sw.ElapsedMilliseconds}");
        }

        private void LoadSubscriptions()
        {
            sw.Restart();

            Subscriptions = _subscriptionB.GetForBranch(BranchId, Start, End, false);
            subscriptionsTotal = Subscriptions.Sum(x => x.Cost);

            sw.Stop();
            Debug.WriteLine($"LoadSubscriptions() {sw.ElapsedMilliseconds}");
        }

        private void LoadRazovoes()
        {
            sw.Restart();

            Razovoes = _trainingItemB.GetAllForBranch(BranchId, Start, End, true);
            razovoesTotal = Razovoes.Sum(x => x.RazovoeCost);

            sw.Stop();
            Debug.WriteLine($"LoadRazovoes() {sw.ElapsedMilliseconds}");
        }
    }
}