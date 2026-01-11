using ElasticaClients.DAL.Accessory;

using System;
using System.Collections.Generic;

namespace ElasticaClients.DAL.Entities
{
    public class Income : IIncome
    {
        public Income()
        {
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        public DateTime Date { get; set; }
        public IncomeType Type { get; set; }

        public int IncomeId => Id;

        public string IncomeName => Name;

        public static Dictionary<IncomeType, string> IncomeTypeNameDictionary = new Dictionary<IncomeType, string>()
        {
            {IncomeType.Salary ,"Зарплата" },
            {IncomeType.Promo ,"Реклама" },
            {IncomeType.RentMy,"Моя аренда" },
            {IncomeType.RentMe ,"Субаренда" },
            {IncomeType.Subscription ,"Абонемент" },
            {IncomeType.Razovoe ,"Разовое" },
            {IncomeType.Other ,"Другое" },
        };
    }
}