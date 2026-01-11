using ElasticaClients.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Models
{
    public class RoleModel
    {
        public static IEnumerable<SelectListItem> GetSelectListItems(RoleB roleB, int selectedId = 0)
        {
            var roles = roleB.GetEditRoles();

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