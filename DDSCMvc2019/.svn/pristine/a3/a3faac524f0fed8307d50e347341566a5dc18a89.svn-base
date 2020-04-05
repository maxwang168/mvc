using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.FileImport
{
    [Serializable]
    public class DataLogInfoDtlBE
    {
        #region 成員變數
        private Guid g_logDetailUuid;
        private Guid g_logUuid;
        private string g_failSeq;
        private string g_failReason;
        private string g_failContent;
        private string g_statusFlag;
        private Guid g_createdBy;
        private string g_createdByName;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private string g_updatedByName;
        private DateTime g_updatedDate;
        #endregion

        #region Property
        [Display(Name = "LogDetailUuid")]
        public Guid LogDetailUuid
        {
            get { return g_logDetailUuid; }
            set { g_logDetailUuid = value; }
        }

        [Display(Name = "LogUuid")]
        public Guid LogUuid
        {
            get { return g_logUuid; }
            set { g_logUuid = value; }
        }

        [Display(Name = "FailSeq")]
        public string FailSeq
        {
            get { return g_failSeq; }
            set { g_failSeq = value; }
        }

        [Display(Name = "FailReason")]
        public string FailReason
        {
            get { return g_failReason; }
            set { g_failReason = value; }
        }

        [Display(Name = "FailContent")]
        public string FailContent
        {
            get { return g_failContent; }
            set { g_failContent = value; }
        }

        [Display(Name = "StatusFlag")]
        public string StatusFlag
        {
            get { return g_statusFlag; }
            set { g_statusFlag = value; }
        }

        [Display(Name = "建立者代號")]
        public Guid CreatedBy
        {
            get { return g_createdBy; }
            set { g_createdBy = value; }
        }

        [Display(Name = "建立者姓名")]
        public string CreatedByName
        {
            get { return g_createdByName; }
            set { g_createdByName = value; }
        }

        [Display(Name = "建立日期")]
        public DateTime CreatedDate
        {
            get { return g_createdDate; }
            set { g_createdDate = value; }
        }

        [Display(Name = "異動者代號")]
        public Guid UpdatedBy
        {
            get { return g_updatedBy; }
            set { g_updatedBy = value; }
        }

        [Display(Name = "異動者姓名")]
        public string UpdatedByName
        {
            get { return g_updatedByName; }
            set { g_updatedByName = value; }
        }

        [Display(Name = "異動日期")]
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
            g_logDetailUuid = new Guid();
            g_logUuid = new Guid();
            g_failSeq = string.Empty;
            g_failReason = string.Empty;
            g_failContent = string.Empty;
            g_statusFlag = string.Empty;
            g_createdBy = new Guid();
            g_createdByName = string.Empty;
            g_createdDate = new DateTime();
            g_updatedBy = new Guid();
            g_updatedByName = string.Empty;
            g_updatedDate = new DateTime();
        }
        #endregion

        #region 建構函數/Dispose
        public DataLogInfoDtlBE()
        {
            resetVariables();
        }
        #endregion

    }
}
