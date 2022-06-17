using ElasticaClients.DAL.Data;
using ElasticaClients.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Models
{
	public class RoleModel
	{
		public static IEnumerable<SelectListItem> GetSelectListItems(int selectedId = 0)
		{
			var roles = RoleB.GetEditRoles(); 

			List<SelectListItem> x = roles
					  .OrderByDescending(y => y.Id)
					  .Select(g =>
						  new SelectListItem
						  {
							  Selected = g.Id == selectedId,
							  Text = g.Name,
							  Value = g.Id.ToString()
						  }).ToList();

			return x;
		}
	}
}