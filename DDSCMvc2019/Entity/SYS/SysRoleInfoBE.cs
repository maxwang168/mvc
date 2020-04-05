using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    public class SysRoleInfoBE
    {
        public string role_code { get; set; }
        public string role_name { get; set; }
        public string role_type { get; set; }
        public string role_subtype { get; set; }
        public string remark { get; set; }
        public string CREATE_USER_UUID { get; set; }
        public string CREATE_ORG_ID { get; set; }
        public System.DateTime CREATE_DATE { get; set; }
        public string MODIFY_USER_UUID { get; set; }
        public string MODIFY_ORG_ID { get; set; }
        public System.DateTime MODIFY_DATE { get; set; }
    }
}
