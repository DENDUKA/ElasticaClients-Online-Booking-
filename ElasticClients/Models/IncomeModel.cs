using ElasticaClients.DAL.Accessory;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ElasticaClients.Models
{
    public class IncomeModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Стоимость")]
        public int Cost { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Дата")]
        public DateTime Date { get; set; }
        [DisplayName("Зал")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        [Required]
        public IncomeType Type { get; set; }

        internal static List<IncomeModel> GetList(IncomeB incomeB, SubscriptionB subscriptionB, int gymId, DateTime? start, DateTime? end)
        {
            DateTime st = DateTime.MinValue;
            DateTime en = DateTime.MaxValue;

            if (start != null)
            {
                st = (DateTime)start;
            }
            if (end != null)
            {
                en = (DateTime)end;
            }

            var list = incomeB.GetAll(gymId, st, en);

            var model = new List<IncomeModel>();

            foreach (var i in list)
            {
                model.Add((IncomeModel)i);
            }

            var subs = subscriptionB.GetAll(start, end);

            foreach (var x in subs)
            {
                model.Add(new IncomeModel()
                {
                    Id = 0,
                    Date = x.BuyDate,
                    Branch = x.Branch,
                    BranchId = x.BranchId,
                });
            }

            model.OrderBy(x => x.Date);

            return model;
        }

        public static IncomeModel Get(IncomeB incomeB, int id)
        {
            var inc = incomeB.Get(id);

            if (inc is null)
                return null;

            return (IncomeModel)inc;
        }

        public static explicit operator IncomeModel(Income i)
        {
            return new IncomeModel()
            {
                Id = i.Id,
                Name = i.Name,
                Cost = i.Cost,
                BranchId = i.BranchId,
                Branch = i.Branch,
                Date = i.Date,
                Type = i.Type,
            };
        }

        public static explicit operator Income(IncomeModel i)
        {
            return new Income()
            {
                Id = i.Id,
                Name = i.Name,
                Cost = i.Cost,
                BranchId = i.BranchId,
                Branch = i.Branch,
                Date = i.Date,
                Type = i.Type,
            };
        }
    }
}