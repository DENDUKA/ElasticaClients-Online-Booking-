using System.Collections.Generic;

namespace YClientsAPI.JSON_Objects
{
    public class ClientsSearchResult
    {
        public bool success { get; set; }
        public List<ClientJSON> data { get; set; }
        public Meta meta { get; set; }
    }
}
