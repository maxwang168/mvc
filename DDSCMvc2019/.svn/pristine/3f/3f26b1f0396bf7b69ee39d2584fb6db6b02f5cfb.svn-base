using System;

namespace Entity.SYS
{
    [Serializable]
    public class UserLogTaiFexBE
    {
        /// <summary>
        /// 日常作業項目查詢
        /// 2017.05.21
        /// </summary>
        #region 成員變數

        private string g_seq;
        private string g_userLogUuid;
        private string g_orgId;
        private string g_orgName;
        private string g_userId;
        private string g_userName;
        private DateTime g_exeDate;
        private string g_funcId;
        private string g_funcName;
        private string g_exeBtn;
        private string g_exeQuery;

        #endregion

        #region Property

        public string Seq
        {
            get { return g_seq; }
            set { g_seq = value; }
        }
        public string UserLogUuid
        {
            get { return g_userLogUuid; }
            set { g_userLogUuid = value; }
        }

        public string OrgId
        {
            get { return g_orgId; }
            set { g_orgId = value; }
        }

        public string OrgName
        {
            get { return g_orgName; }
            set { g_orgName = value; }
        }

        public string UserId
        {
            get { return g_userId; }
            set { g_userId = value; }
        }

        public string UserName
        {
            get { return g_userName; }
            set { g_userName = value; }
        }

        public DateTime ExeDate
        {
            get { return g_exeDate; }
            set { g_exeDate = value; }
        }

        public string FuncId
        {
            get { return g_funcId; }
            set { g_funcId = value; }
        }

        public string FuncName
        {
            get { return g_funcName; }
            set { g_funcName = value; }
        }

        public string ExeBtn
        {
            get { return g_exeBtn; }
            set { g_exeBtn = value; }
        }

        public string ExeQuery
        {
            get { return g_exeQuery; }
            set { g_exeQuery = value; }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_orgId = string.Empty;
            g_orgName = string.Empty;
            g_userId = string.Empty;
            g_userName = string.Empty;
            g_exeDate = new DateTime();
            g_funcId = string.Empty;
            g_funcName = string.Empty;
            g_exeBtn = string.Empty;
            g_exeQuery = string.Empty;
        }

        #endregion

        #region 建構函數/Dispose

        public UserLogTaiFexBE()
        {
            resetVariables();
        }

        #endregion

    }
}
