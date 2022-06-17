using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElasticaClients.Models
{
	public class BranchModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public List<Subscription> Subscriptions { get; set; }
		public List<Income> Incomes { get; set; }
		public List<Gym> Gyms { get; set; }

		internal static List<BranchModel> GetAll()
		{
			var branches = BranchB.GetAll();

			var models = new List<BranchModel>();
			foreach (var branch in branches)
			{
				models.Add((BranchModel)branch);
			}

			return models;
		}

		public static BranchModel Get(int branchId)
		{
			var branch = BranchDAL.Get(branchId);

			return (BranchModel)branch;
		}

		public static explicit operator BranchModel(Branch b)
		{
			return new BranchModel()
			{
				Id = b.Id,
				Incomes = b.Incomes,
				Name = b.Name,
				Subscriptions = b.Subscriptions,
				Gyms = b.Gyms,
			};
		}
	}
}