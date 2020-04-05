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
    public class SysRoleInfoModel //: _ContractBase
    {
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string RoleType { get; set; }
        public string RoleSubType { get; set; }
    }
}
