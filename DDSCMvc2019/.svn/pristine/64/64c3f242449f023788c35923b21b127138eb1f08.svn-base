using Entity.BAS;
using Entity.SYS;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace PortalService.Contract
{
    [ServiceContract]
    public interface ILogService
    {
        [OperationContract]
        void insertLog(SysUserLogBE log);

        [OperationContract]
        void InsertMonitorLog(string function_code, string level_code, string message, Guid userUuid);

    }
}
