using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Logic
{
    public class GymB
    {
        private readonly IGymDAL _gymDAL;

        public GymB(IGymDAL gymDAL)
        {
            _gymDAL = gymDAL;
        }

        public IEnumerable<SelectListItem> ToSelectListItems(int selectedId, List<Gym> gyms = null)
        {
            if (gyms is null)
            {
                gyms = _gymDAL.GetAll();
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

        public Gym Get(int gymId)
        {
            return _gymDAL.Get(gymId);
        }

        public List<Gym> GetAll()
        {
            return _gymDAL.GetAll();
        }
    }
}