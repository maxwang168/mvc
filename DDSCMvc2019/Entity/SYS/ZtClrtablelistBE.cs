using System;

namespace Entity.SYS
{
    public class ZtClrtablelistBE
    {
        #region 成員變數

        private Guid g_tableListUuid;
        private Guid g_clearUuid;
        private string g_tableName;
        private string g_tableDesc;
        private int g_clearSeq;
        private string g_statusFlag;
        private Guid g_createdBy;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private DateTime g_updatedDate;

        #endregion

        #region Property


        /// <summary>
        /// 
        /// </summary>
        public Guid TableListUuid
        {
            get { return g_tableListUuid; }
            set { g_tableListUuid = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid ClearUuid
        {
            get { return g_clearUuid; }
            set { g_clearUuid = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TableName
        {
            get { return g_tableName; }
            set { g_tableName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TableDesc
        {
            get { return g_tableDesc; }
            set { g_tableDesc = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ClearSeq
        {
            get { return g_clearSeq; }
            set { g_clearSeq = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string status_flag
        {
            get { return g_statusFlag; }
            set { g_statusFlag = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid created_by
        {
            get { return g_createdBy; }
            set { g_createdBy = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime created_date
        {
            get { return g_createdDate; }
            set { g_createdDate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid updated_by
        {
            get { return g_updatedBy; }
            set { g_updatedBy = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime updated_date
        {
            get { return g_updatedDate; }
            set { g_updatedDate = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_tableListUuid = new Guid();
            g_clearUuid = new Guid();
            g_tableName = string.Empty;
            g_tableDesc = string.Empty;
            g_clearSeq = 0;
            g_statusFlag = string.Empty;
            g_createdBy = new Guid();
            g_createdDate = new DateTime();
            g_updatedBy = new Guid();
            g_updatedDate = new DateTime();

        }

        #endregion

        #region 建構函數/Dispose

        public ZtClrtablelistBE()
        {
            resetVariables();
        }

        #endregion
    }
}
