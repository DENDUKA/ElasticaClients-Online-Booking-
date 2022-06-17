using ElasticaClients.DAL.Data;
using ElasticaClients.DAL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Logic
{
	public static class GymB
	{
		public static IEnumerable<SelectListItem> ToSelectListItems(int selectedId, List<Gym> gyms =null )
		{
			if (gyms is null)
			{
				gyms = GymDAL.GetAll();
			}

			return gyms.OrderBy(g => g.Name)
					  .Select(g =>
						  new SelectListItem
						  {
							  Selected = (g.Id == selectedId),
							  Text = g.Name,
							  Value = g.Id.ToString()
						  });
		}

		public static Gym Get(int gymId)
		{
			return GymDAL.Get(gymId);
		}
	}
}