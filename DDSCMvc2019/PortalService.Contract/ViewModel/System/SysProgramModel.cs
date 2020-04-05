using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PortalService.Contract.ViewModel
{
    //[KnownType(typeof(_ContractBase))]
    [Serializable]
    public class SysProgramModel //: _ContractBase
    {
        public string ProgramType { get; set; }
        public string Menu { get; set; }
        public string SubMenu { get; set; }
        public string ProgramID { get; set; }
        public string ProgramName { get; set; }
    }
}
