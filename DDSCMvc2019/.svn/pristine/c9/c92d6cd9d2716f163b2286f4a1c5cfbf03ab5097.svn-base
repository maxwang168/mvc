using System;

namespace Entity.SYS
{
    public class ZtClrlistBE
    {
        #region 成員變數

        private Guid g_clearUuid;
        private Guid g_clearTableUuid;
        private string g_clearTable;
        private string g_clearTableType;
        private string g_clearType;
        private int g_keepDays;
        private int g_seq;
        private string g_isAchieve;
        private string g_descriptions;
        private string g_filePath;
        private string g_statusFlag;
        private Guid g_createdBy;
        private string g_createdByName;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private string g_updatedByName;
        private DateTime g_updatedDate;

        private string g_clearTypeName;

        #endregion

        #region Property

        public Guid ClearUuid
        {
            get { return g_clearUuid; }
            set { g_clearUuid = value; }
        }

        /// <summary>
        /// 清檔描述檔UUID
        /// </summary>
        public Guid ClearTableUuid
        {
            get { return g_clearTableUuid; }
            set { g_clearTableUuid = value; }
        }

        /// <summary>
        /// 清檔名稱
        /// </summary>
        public string ClearTable
        {
            get { return g_clearTable; }
            set { g_clearTable = value; }
        }

        /// <summary>
        /// 檔案種類 (預設:資料表, 檔案)
        /// </summary>
        public string ClearTableType
        {
            get { return g_clearTableType; }
            set { g_clearTableType = value; }
        }

        /// <summary>
        /// 資料保留方式 (M:月/D:日/Y:年)
        /// </summary>
        public string ClearType
        {
            get { return g_clearType; }
            set { g_clearType = value; }
        }

        /// <summary>
        /// 資料保留期限
        /// </summary>
        public int KeepDays
        {
            get { return g_keepDays; }
            set { g_keepDays = value; }
        }

        /// <summary>
        /// 下拉顯示順序
        /// </summary>
        public int Seq
        {
            get { return g_seq; }
            set { g_seq = value; }
        }

        /// <summary>
        /// 是否 ACHIEVE
        /// </summary>
        public string IsAchieve
        {
            get { return g_isAchieve; }
            set { g_isAchieve = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descs
        {
            get { return g_descriptions; }
            set { g_descriptions = value; }
        }

        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string FilePath
        {
            get { return g_filePath; }
            set { g_filePath = value; }
        }

        /// <summary>
        /// 狀態(I:初始,N:無效,Y:有效,F:審核中,X:退件)
        /// </summary>
        public string status_flag
        {
            get { return g_statusFlag; }
            set { g_statusFlag = value; }
        }

        /// <summary>
        /// 建立人員
        /// </summary>
        public Guid created_by
        {
            get { return g_createdBy; }
            set { g_createdBy = value; }
        }

        public string created_by_name
        {
            get { return g_createdByName; }
            set { g_createdByName = value; }
        }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime created_date
        {
            get { return g_createdDate; }
            set { g_createdDate = value; }
        }

        /// <summary>
        /// 異動人員
        /// </summary>
        public Guid updated_by
        {
            get { return g_updatedBy; }
            set { g_updatedBy = value; }
        }

        public string updated_by_name
        {
            get { return g_updatedByName; }
            set { g_updatedByName = value; }
        }

        /// <summary>
        /// 異動日期
        /// </summary>
        public DateTime updated_date
        {
            get { return g_updatedDate; }
            set { g_updatedDate = value; }
        }

        public string ClearTypeName
        {
            get { return g_clearTypeName; }
            set { g_clearTypeName = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_clearUuid = new Guid();
            g_clearTableUuid = new Guid();
            g_clearTable = string.Empty;
            g_clearTableType = string.Empty;
            g_clearType = string.Empty;
            g_keepDays = 0;
            g_seq = 0;
            g_isAchieve = string.Empty;
            g_descriptions = string.Empty;
            g_filePath = string.Empty;
            g_statusFlag = string.Empty;
            g_createdBy = new Guid();
            g_createdByName = string.Empty;
            g_createdDate = new DateTime();
            g_updatedBy = new Guid();
            g_updatedByName = string.Empty;
            g_updatedDate = new DateTime();

            g_clearTypeName = string.Empty;

        }

        #endregion

        #region 建構函數/Dispose

        public ZtClrlistBE()
        {
            resetVariables();
        }

        #endregion
    }
}
