using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.BAS
{
    [Serializable]
    public class NotifyRecBE
    {
        #region 成員變數
        private Guid g_recUuid;
        private Guid g_notifyUuid;
        private Guid g_userUuid;
        private string g_userByName;
        private string g_date;
        private string g_contact;
        private string g_channel;
        private string g_reqData;
        private string g_notifyTitle;
        private string g_notifyData;
        private string g_status;
        private DateTime g_schedule;
        private string g_statusFlag;
        private Guid g_createdBy;
        private string g_createdByName;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private string g_updatedByName;
        private DateTime g_updatedDate;
        #endregion

        #region Property
        [Display(Name = "RecUuid")]
        public Guid RecUuid
        {
            get { return g_recUuid; }
            set { g_recUuid = value; }
        }

        public Guid NotifyUuid
        {
            get { return g_notifyUuid; }
            set { g_notifyUuid = value; }
        }

        [Display(Name = "使用者")]
        public Guid UserUuid
        {
            get { return g_userUuid; }
            set { g_userUuid = value; }
        }

        [Display(Name = "使用者姓名")]
        public string UserByName
        {
            get { return g_userByName; }
            set { g_userByName = value; }
        }
        [Display(Name = "日期")]
        //[Required]
        public string Date
        {
            get { return g_date; }
            set { g_date = value; }
        }

        [Display(Name = "聯絡資訊")]
        //[Required]
        //[MaxLength(7)]
        public string Contact
        {
            get { return g_contact; }
            set { g_contact = value; }
        }

        [Display(Name = "傳送管道")]
        //[Required]
        public string Channel
        {
            get { return g_channel; }
            set { g_channel = value; }
        }

        public string ReqData
        {
            get { return g_reqData; }
            set { g_reqData = value; }
        }

        public string NotifyTitle
        {
            get { return g_notifyTitle; }
            set { g_notifyTitle = value; }
        }

        public string NotifyData
        {
            get { return g_notifyData; }
            set { g_notifyData = value; }
        }

        [Display(Name = "傳送狀態")]
        //[Required]
        public string Status
        {
            get { return g_status; }
            set { g_status = value; }
        }
        
        public DateTime Schedule
        {
            get { return g_schedule; }
            set { g_schedule = value; }
        }

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
            g_recUuid = new Guid();
            g_userUuid = new Guid();
            g_userByName = string.Empty;
            g_date = string.Empty;
            g_contact = string.Empty;
            g_channel = string.Empty;
            g_reqData = string.Empty;
            g_notifyTitle = string.Empty;
            g_notifyData = string.Empty;
            g_status = string.Empty;
            g_schedule = new DateTime();
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
        public NotifyRecBE()
        {
            resetVariables();
        }
        #endregion

    }
}
