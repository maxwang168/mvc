using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    public class WorkFlowBE
    {
        public Guid uid { get; set; }
        public string program_id { get; set; }
        public Guid data_key { get; set; }
        public string content_bef { get; set; }
        public string content_aft { get; set; }
        public string send_user { get; set; }
        public DateTime send_date { get; set; }
        public string opinion { get; set; }
        public string status_code { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }

        public int sn { get; set; }
        public string program_name { get; set; }
        public string function_name_allow { get; set; }
        public string function_name_reject { get; set; }
        public string user_name { get; set; }
        public string status_name { get; set; }
    }
}
