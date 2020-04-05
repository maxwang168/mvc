using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PortalService.Contract.ViewModel
{
    [Serializable]
    public class FlowJobModel
    {
        public string FlowStatus { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string FuncId { get; set; }
        public Guid userUuid { get; set; }
    }
}
