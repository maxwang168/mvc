using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalService.Contract.ViewModel.System
{
    public class NotifyQueryModel
    {
        public Guid RecUuid { get; set; }
        public DateTime SendDateS { get; set; }
        public DateTime SendDateE { get; set; }
        public int? PorcStatus { get; set; }
        public Guid UserUuid { get; set; }

        public DateTime Schedule { get; set; }
    }
}
