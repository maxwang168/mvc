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
    public interface IFlowService
    {
        [OperationContract]
        FlwRtn FlowStart(FlowStartModel p_startModel);

        [OperationContract]
        FlwRtn FlowUpdate(FlowUpdateModel p_startModel);

        [OperationContract]
        IEnumerable<string[]> QueryWorkFlowProgram(Guid p_UserUuid = new Guid());

        [OperationContract]
        IEnumerable<FlwJobItemUI> QueryFlow(FlowQueryModel p_model);
        
        [OperationContract]
        IEnumerable<TodoListBE> QueryTodoList(Guid p_roleUuid, Guid p_userUuid);

        [OperationContract]
        string QueryFlwOrgData(Guid p_jobUuid);

        [OperationContract]
        bool UpdateDataStatus(string p_funcId, Guid p_dataUuid, string p_status);

        [OperationContract]
        string CheckDataAvailable(string p_funcId, Guid p_dataUuid);
    }
}
