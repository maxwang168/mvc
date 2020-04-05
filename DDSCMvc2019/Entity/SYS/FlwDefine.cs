using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    [Serializable]
    public class FlwDefine
    {
        public Guid flw_uuid { get; set; }
        public string flw_id { get; set; }
        public string flw_fname { get; set; }
        public string flw_description { get; set; }
        public string flw_type { get; set; }
        public string is_default { get; set; }
        public string status_flag { get; set; }
        public Guid created_by { get; set; }
        public DateTime created_date { get; set; }
        public Guid updated_by { get; set; }
        public DateTime updated_date { get; set; }

        private List<FlwDefineItem> g_defineList;
        public List<FlwDefineItem> DefineList
        {
            get
            {
                if (g_defineList == null)
                {
                    g_defineList = new List<FlwDefineItem>();                   
                }
                return g_defineList;
            }
            set
            {
                g_defineList = value;
            }
        }

    }
}
