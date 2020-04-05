using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    [Serializable]
    public class FlwJob
    {
        public Guid job_uuid {get;set;}
        public Guid flw_uuid { get;set;}
        public Guid user_info_uuid { get;set;}
        public Guid data_uuid { get;set;}
        public string org_uuid { get;set;}
        public string system_id { get;set;}
        public string function_id { get;set;}
        public string function_name { get;set;}
        public string service_name { get;set;}
        public string flw_status { get;set;}
        public string call_back { get;set;}
        public string return_data { get;set;}
        public string flw_type { get;set;}
        public string status_flag { get;set;}
        public Guid created_by { get;set;}
        public DateTime created_date { get;set;}
        public Guid updated_by { get;set;}
        public DateTime updated_date { get;set;}
    }
}
