using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    public class SysRoleProgramsBE
    {
        public int id { get; set; }
        public string role_code { get; set; }
        public string program_id { get; set; }
        public bool? isAddNew { get; set; }
        public bool? isUpdate { get; set; }
        public bool? isDelete { get; set; }
        public bool? isQuery { get; set; }
        public bool? isView { get; set; }
        public bool? isPrint { get; set; }
        public bool? isImport { get; set; }
        public bool? isExport { get; set; }
        public bool? isExecute { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }

        public int sn { get; set; }
        public string program_name { get; set; }
        public string program_desc { get; set; }
    }
}
