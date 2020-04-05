using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    [Serializable]
    public class AaGroup
    {
        public Guid group_uuid { get; set; }
        public Guid system_uuid { get; set; }
        public string system_id { get; set; }
        public string org_id { get; set; }
        public string group_id { get; set; }
        public string group_name { get; set; }
        public string admin_group { get; set; }
        public string status_flag { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }
}
