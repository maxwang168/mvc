using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalService.Contract.ViewModel.System
{
    public class FlowQueryModel
    {
        public string ProgramId { get; set; }
        public DateTime SendDateS { get; set; }
        public DateTime SendDateE { get; set; }
        public string StatusCode { get; set; }

        public Guid roleUuid { get; set; }
        public Guid jobUuid { get; set; }
        public Guid UserUuid { get; set; }
    }
}
