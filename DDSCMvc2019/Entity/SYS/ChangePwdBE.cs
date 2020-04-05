using System;

namespace Entity.SYS
{
    [Serializable]
    public class ChangePwdBE
    {
        #region 成員變數

        private string g_orgId;
        private Guid g_userUid;
        private string g_userId;
        private string g_userName;
        private string g_oldPwd;
        private string g_newPwd;
        private string g_newPwd2;
        private string g_pwd3;
        private Guid g_updatedBy;
        private DateTime g_updatedDate;

        #endregion

        #region Property

        public string OrgId
        {
            get { return g_orgId; }
            set { g_orgId = value; }
        }

        public Guid UserUid
        {
            get { return g_userUid; }
            set { g_userUid = value; }
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

        public string OldPwd
        {
            get { return g_oldPwd; }
            set { g_oldPwd = value; }
        }

        public string NewPwd
        {
            get { return g_newPwd; }
            set { g_newPwd = value; }
        }

        public string NewPwd2
        {
            get { return g_newPwd2; }
            set { g_newPwd2 = value; }
        }

        public string Pwd3
        {
            get { return g_pwd3; }
            set { g_pwd3 = value; }
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

        #endregion

        #region Public Method

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_orgId = string.Empty;
            g_userUid = Guid.Empty;
            g_userId = string.Empty;
            g_userName = string.Empty;
            g_oldPwd = string.Empty;
            g_newPwd = string.Empty;
            g_newPwd2 = string.Empty;
            g_pwd3 = string.Empty;
            g_updatedBy = Guid.Empty;
            g_updatedDate = new DateTime();
        }

        #endregion

        #region 建構函數/Dispose

        public ChangePwdBE()
        {
            resetVariables();
        }

        #endregion

    }
}
