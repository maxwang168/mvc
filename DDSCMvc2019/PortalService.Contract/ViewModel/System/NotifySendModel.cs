using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalService.Contract.ViewModel.System
{
    public class NotifySendModel
    {
        private Guid g_contactUserUuid;
        public string NotifyCodeId { get; set; }
        public string DataXml { get; set; }
        public List<string> ContactAddrList { get; set; }
        public Guid ContactUserUuid
        {
            get
            {
                if (g_contactUserUuid == null)
                {
                    return Guid.Empty;
                }
                else
                {
                    return g_contactUserUuid;
                }
            }
            set
            {
                g_contactUserUuid = value;
            }
        }
        public DateTime Schedule { get; set; }
        public Guid UserUuid { get; set; }
        public bool IsSubscription { get; set; }
    }
}
