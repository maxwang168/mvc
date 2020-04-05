using Entity.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Entity
{
    public class UserData
    {
        public AaUser UserInfo { get; set; }
        public string ProgramXml { get; set; }
        public List<AaGroup> GroupList { get; set; }

        public string SelectedSystem { get; set; }
        public string CurrencyType { get; set; }
        public Guid LoginToken { get; set; }
    }
}
