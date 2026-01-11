using System;

namespace YClientsAPI.JSON_Objects.Activity
{
    public class ActivityJSON
    {
        public int id { get; set; }
        public int staff_id { get; set; }
        public DateTime date { get; set; }
        public int capacity { get; set; }
        public ServiceJSON service { get; set; }
        public StaffJSON staff { get; set; }
    }
}
