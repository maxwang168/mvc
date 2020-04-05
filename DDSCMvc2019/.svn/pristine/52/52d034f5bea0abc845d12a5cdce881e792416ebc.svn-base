using CommonLibrary.DBA;
using Entity.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalService.FlowProcess
{
    public interface IFlowProcess
    {
        FlwRtn flowApprove(FlwJobBE job, Guid userId, SysProgramBE program, DBASqlLog m_dba);
        FlwRtn flowReject(FlwJobBE job, Guid userId, SysProgramBE program, DBASqlLog m_dba);
    }
}
