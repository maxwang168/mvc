using Entity.SYS;
using PortalService.Contract.ViewModel.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PortalService.Contract
{
    [ServiceContract]
    public interface INotifyService
    {
        [OperationContract]
        bool NotifySend(NotifySendModel p_sendModel);

        [OperationContract]
        IEnumerable<NotifyRecBE> QueryNotifyRec(NotifyQueryModel p_model);

        [OperationContract]
        void UpdateNotifyStatus(int p_status, Guid p_recId);
    }
}
