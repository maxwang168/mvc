using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    public class FlwJobDataBE
    {
        public Guid job_data_uuid { get; set; }
        public Guid job_uuid { get; set; }
        public Guid job_item_uuid { get; set; }
        public string preview_data { get; set; }
        public string orginal_data { get; set; }
        public Guid xslt_uuid { get; set; }
        public string data_type { get; set; }
        public string status_flag { get; set; }
        public Guid created_by { get; set; }
        public DateTime created_date { get; set; }
        public Guid updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }
}
