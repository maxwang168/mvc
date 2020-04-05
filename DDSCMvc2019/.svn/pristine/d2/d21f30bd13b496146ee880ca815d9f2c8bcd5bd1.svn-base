using System;

namespace Entity.SYS
{
    [Serializable]
    public class FlowJobBE
    {
        #region 成員變數

        private Guid g_jobUuid;
        private Guid g_flwUuid;
        private Guid g_userInfoUuid;
        private string user_info_name;//申請人名稱
        private Guid g_dataUuid;
        private Guid g_orgUuid;
        private string g_systemId;
        private string g_functionId;
        private string g_functionName;
        private string g_serviceName;
        private string g_flwStatus;
        private string flw_status_name;//狀態名稱
        private string g_callBack;
        private string g_returnData;
        private string g_flwType;
        private string flw_type_name;//執行動作名稱
        private string g_statusFlag;
        private Guid g_createdBy;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private DateTime g_updatedDate;
        public int sn { get; set; }//序號
        public string content { get; set; }//
        public string function_entity { get; set; }
        public string orginal_data { get; set; }

        #endregion

        #region Property

        public Guid JobUuid
        {
            get { return g_jobUuid; }
            set { g_jobUuid = value; }
        }

        public Guid FlwUuid
        {
            get { return g_flwUuid; }
            set { g_flwUuid = value; }
        }
        public Guid UserInfoUuid
        {
            get { return g_userInfoUuid; }
            set { g_userInfoUuid = value; }
        }
        public Guid DataUuid
        {
            get { return g_dataUuid; }
            set { g_dataUuid = value; }
        }
        public Guid OrgUuid
        {
            get { return g_orgUuid; }
            set { g_orgUuid = value; }
        }

        public string SystemId
        {
            get { return g_systemId; }
            set { g_systemId = value; }
        }

        public string FunctionId
        {
            get { return g_functionId; }
            set { g_functionId = value; }
        }

        public string FunctionName
        {
            get { return g_functionName; }
            set { g_functionName = value; }
        }
        //service_name
        public string ServiceName
        {
            get { return g_serviceName; }
            set { g_serviceName = value; }
        }
        public string FlwStatus
        {
            get { return g_flwStatus; }
            set { g_flwStatus = value; }
        }
        public string FlwStatusName
        {
            get { return flw_status_name; }
            set { flw_status_name = value; }
        }

        public string CallBack
        {
            get { return g_callBack; }
            set { g_callBack = value; }
        }

        public string ReturnData
        {
            get { return g_returnData; }
            set { g_returnData = value; }
        }

        public string FlwType
        {
            get { return g_flwType; }
            set { g_flwType = value; }
        }

        public string StatusFlag
        {
            get { return g_statusFlag; }
            set { g_statusFlag = value; }
        }

        public Guid CreatedBy
        {
            get { return g_createdBy; }
            set { g_createdBy = value; }
        }

        public DateTime CreatedDate
        {
            get { return g_createdDate; }
            set { g_createdDate = value; }
        }

        public Guid UpdatedBy
        {
            get { return g_updatedBy; }
            set { g_updatedBy = value; }
        }

        public DateTime UpdatedDate
        {
            get { return g_updatedDate; }
            set { g_updatedDate = value; }
        }
        //
        public string UserInfoName
        {
            get { return user_info_name; }
            set { user_info_name = value; }
        }
        public string FlwTypeName
        {
            get { return flw_type_name; }
            set { flw_type_name = value; }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_jobUuid = Guid.Empty;
            g_flwUuid = Guid.Empty;
            g_userInfoUuid = Guid.Empty;
            g_dataUuid = Guid.Empty;
            g_orgUuid = Guid.Empty;
            g_systemId = string.Empty;
            g_functionId = string.Empty;
            g_functionName = string.Empty;
            g_serviceName = string.Empty;
            g_flwStatus = string.Empty;
            g_callBack = string.Empty;
            g_returnData = string.Empty;
            g_flwType = string.Empty;
            g_statusFlag = string.Empty;
            g_createdBy = Guid.Empty;
            g_createdDate = new DateTime();
            g_updatedBy = Guid.Empty;
            g_updatedDate = new DateTime();
            user_info_name = string.Empty;//
            flw_type_name = string.Empty;//
            flw_status_name = string.Empty;//
        }

        #endregion

        #region 建構函數/Dispose

        public FlowJobBE()
        {
            resetVariables();
        }

        #endregion

    }
}
