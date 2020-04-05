using Entity.Monitor;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace PortalService.Contract
{
    [ServiceContract]
    public interface IMonitorService
    {
        //[OperationContract]
        //ReturnMsg CreateFunction(string functionCode, string functionName, Guid creadeBy);
        //[OperationContract]
        //ReturnMsg UpdateFunction(string functionCode, string functionName, Guid updatedBy);
        //[OperationContract]
        //IEnumerable<FunctionBE> QueryFunction(string functionCode, string functionName);
        //[OperationContract]
        //ReturnMsg DeleteFunction(string functionCode);
        [OperationContract]
        IEnumerable<string[]> functionSelectItem();
        //[OperationContract]
        //ReturnMsg CreateLevel(string levelCode, string levelName, string creadeBy);
        //[OperationContract]
        //ReturnMsg UpdateLevel(string levelCode, string levelName, string userNo);
        //[OperationContract]
        //IEnumerable<LevelBE> QueryLevel(string levelCode, string levelName);
        //[OperationContract]
        //ReturnMsg DeleteLevel(string levelCode);
        [OperationContract]
        IEnumerable<string[]> levelSelectItem();
        [OperationContract]
        IEnumerable<MonitorLogBE> QueryMonitorLog(string level, string function, DateTime sDate, DateTime eDate);
        //[OperationContract]
        //ReturnMsg CreateNotify(string functionCode, string levelCode, string notifyNotify, string creadeBy);
        //[OperationContract]
        //IEnumerable<MonitorBE> QueryNotify(string functionCode, string levelCode, string notifyCode);
        //[OperationContract]
        //ReturnMsg DeleteNotify(string functionCode, string levelCode, string notifyCode);
        //[OperationContract]
        //List<string[]> GetFunctionItem();
        //[OperationContract]
        //List<string[]> GetLevelItem();
        //[OperationContract]
        //List<string[]> GetNotifyItem();
        //[OperationContract]
        //IEnumerable<string[]> Querynotify_type();
        //[OperationContract]
        //bool insertNotifytype(NotifytypeBE p_be);
        //[OperationContract]
        //bool insertNotifytypedtl(NotifytypeBE p_be);
        //[OperationContract]
        //bool deleteNotifytype(string p_notify_code);
        //[OperationContract]
        //bool deleteNotifytypedtl(string p_notify_code, string p_address, string p_id);
        //[OperationContract]
        //IEnumerable<NotifytypeBE> selectNotifytype(NotifytypeViewModel p_notifytypeVM);
        //[OperationContract]
        //IEnumerable<NotifytypeBE> selectNotifytypedtl(NotifytypeViewModel p_notifytypeVM);
        //[OperationContract]
        //bool updateNotifytype(NotifytypeBE p_be);
        //[OperationContract]
        //bool updateNotifytypedtl(NotifytypeBE p_be);
    }
}

