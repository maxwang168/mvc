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
    public class RecInfoModel //: _ContractBase
    {
        public string created_by { get; set; } = Guid.Empty.ToString();

        public string creator_id { get; set; } = string.Empty;

        public string creator_name { get; set; } = string.Empty;

        public DateTime created_date { get; set; } = new DateTime();

        public string updated_by { get; set; } = Guid.Empty.ToString();

        public string updater_id { get; set; } = string.Empty;
        
        public string updater_name { get; set; } = string.Empty;

        public DateTime updated_date { get; set; } = new DateTime();
    }
}
