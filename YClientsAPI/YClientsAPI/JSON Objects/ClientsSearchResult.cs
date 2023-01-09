using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YClientsAPI.JSON_Objects
{
    public class ClientsSearchResult
    {
        public bool success { get; set; }
        public List<ClientJSON> data { get; set; }
        public Meta meta { get; set; }
    }
}
