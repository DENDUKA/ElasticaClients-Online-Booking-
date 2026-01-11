using System;
using System.Collections.Generic;

namespace YClientsAPI.JSON_Objects.Records
{
    public class RecordsSearchResult
    {
        public int id { get; set; }
        public int company_id { get; set; }
        public int seance_length { get; set; }
        public DateTime date { get; set; }
        public List<ServiceJSON> services { get; set; }
        public StaffJSON staff { get; set; }
        public ClientJSON client { get; set; }
        public string comment { get; set; }
        public string activity_id { get; set; }
    }
}
