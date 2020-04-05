using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PortalService.Contract.ViewModel//.System
{
    //[KnownType(typeof(_ContractBase))]
    [Serializable]
    public class SysGroupProgramModel //: _ContractBase
    {
        public Guid GroupUuid { get; set; }
        public Guid FuncUuid { get; set; }
    }
}
