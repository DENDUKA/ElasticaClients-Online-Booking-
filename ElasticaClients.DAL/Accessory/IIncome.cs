using ElasticaClients.DAL.Entities;
using System;

namespace ElasticaClients.DAL.Accessory
{
	public interface IIncome
	{
		int IncomeId { get; }
		string IncomeName { get; }
		IncomeType Type { get; }
		int Cost { get; }
		DateTime Date { get; }
	}
}