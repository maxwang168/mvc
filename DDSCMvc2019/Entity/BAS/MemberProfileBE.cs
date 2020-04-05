using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entity.BAS
{
    [Serializable]
    public class MemberProfileBE : IFlowHandler
    {
        #region 成員變數
        private Guid g_memberUuid;
        private string g_memBankNo;
        private string g_memBankNoName;
        private string g_memCode;
        private string g_memCurr;
        private string g_memName;
        private string g_memSettleAc;
        private string g_memNo;
        private string g_memActype;
        private string g_memIdNo;
        private string g_statusFlag;
        private Guid g_createdBy;
        private string g_createdByName;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private string g_updatedByName;
        private DateTime g_updatedDate;
        #endregion

        #region Property
        [Display(Name = "member_uuid")]
        public Guid member_uuid
        {
            get { return g_memberUuid; }
            set { g_memberUuid = value; }
        }

        [Display(Name = "結算行")]
        [Required]
        public string mem_bank_no
        {
            get { return g_memBankNo; }
            set { g_memBankNo = value; }
        }

        [Display(Name = "MemBankNoName")]
        public string MemBankNoName
        {
            get { return g_memBankNoName; }
            set { g_memBankNoName = value; }
        }

        [Display(Name = "結算會員")]
        [Required]
        [MaxLength(7)]
        public string mem_code
        {
            get { return g_memCode; }
            set { g_memCode = value; }
        }

        [Display(Name = "幣別")]
        [Required]
        public string mem_curr
        {
            get { return g_memCurr; }
            set { g_memCurr = value; }
        }

        /// <summary>
        /// 幣別名稱，顯示為「幣別」
        /// </summary>
        [Display(Name = "幣別")]
        public string MemCurr_desc { get; set; }

        [Display(Name = "名稱")]
        [Required]
        [MaxLength(40)]
        public string mem_name
        {
            get { return g_memName; }
            set { g_memName = value; }
        }

        [Display(Name = "保證金專戶帳號")]
        [Required]
        [MaxLength(21)]
        public string mem_settle_ac
        {
            get { return g_memSettleAc; }
            set { g_memSettleAc = value; }
        }

        [Display(Name = "mem_no")]
        public string mem_no
        {
            get { return g_memNo; }
            set { g_memNo = value; }
        }

        [Display(Name = "帳號種類")]
        public string mem_actype
        {
            get { return g_memActype; }
            set { g_memActype = value; }
        }

        [Display(Name = "統一編號")]
        [Required]
        [MaxLength(14)]
        public string mem_id_no
        {
            get { return g_memIdNo; }
            set { g_memIdNo = value; }
        }

        [Display(Name = "狀態")]
        public string status_flag
        {
            get { return g_statusFlag; }
            set { g_statusFlag = value; }
        }

        [Display(Name = "建立者代號")]
        public Guid created_by
        {
            get { return g_createdBy; }
            set { g_createdBy = value; }
        }

        [Display(Name = "建立者姓名")]
        public string created_by_name
        {
            get { return g_createdByName; }
            set { g_createdByName = value; }
        }

        [Display(Name = "建立日期")]
        public DateTime created_date
        {
            get { return g_createdDate; }
            set { g_createdDate = value; }
        }

        [Display(Name = "異動者代號")]
        public Guid updated_by
        {
            get { return g_updatedBy; }
            set { g_updatedBy = value; }
        }

        [Display(Name = "異動者姓名")]
        public string updated_by_name
        {
            get { return g_updatedByName; }
            set { g_updatedByName = value; }
        }

        [Display(Name = "異動日期")]
        public DateTime updated_date
        {
            get { return g_updatedDate; }
            set { g_updatedDate = value; }
        }

        public string[] DATA_FIELD
        {
            get
            {
                return new string[] { "member_uuid", "mem_bank_no", "mem_code", "mem_curr", "mem_name",
                    "mem_settle_ac", "mem_no", "mem_actype", "mem_id_no", "status_flag", "updated_by", "updated_date" };
            }
        }

        public string htmlStringHandler()
        {
            StringBuilder m_strHtml = new StringBuilder();
            m_strHtml.Append("<table class='main_table' border='1' cellpadding='1' cellspacing='1' style='width:100%;'>");
            m_strHtml.Append("<thead><tr><td colspan='2'>資料內容</td></tr></thead>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel' width='15%'>統一編號</th>");
            m_strHtml.Append(string.Format("<td class='editTable' width='85%'>{0}</td>", mem_id_no));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>結算會員</th>");
            m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", mem_code));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>幣別</th>");
            m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", mem_curr + "-" + MemCurr_desc));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>名稱</th>");
            m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", mem_name));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>結算行</th>");
            m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", mem_bank_no + "-" + MemBankNoName));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<th class='mainLabel'>保證金專戶帳號</th>");
            m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", mem_settle_ac));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<th class='mainLabel'>帳號種類</th>");
            m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", mem_actype == "0" ? "0-客戶帳" : "1-自有帳"));
            m_strHtml.Append("</tr>");
            //m_strHtml.Append("<tr>");
            //m_strHtml.Append("<th class='mainLabel'>建立者</th>");
            //m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", created_by_name));
            //m_strHtml.Append("</tr>");
            //m_strHtml.Append("<tr>");
            //m_strHtml.Append("<th class='mainLabel'>建立時間</th>");
            //m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", created_date));
            //m_strHtml.Append("</tr>");
            //m_strHtml.Append("<tr>");
            //m_strHtml.Append("<th class='mainLabel'>異動者</th>");
            //m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", updated_by_name));
            //m_strHtml.Append("</tr>");
            //m_strHtml.Append("<tr>");
            //m_strHtml.Append("<th class='mainLabel'>異動時間</th>");
            //m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", updated_date));
            //m_strHtml.Append("</tr>");
            m_strHtml.Append("</table>");
            return m_strHtml.ToString();
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_memberUuid = new Guid();
            g_memBankNo = string.Empty;
            g_memBankNoName = string.Empty;
            g_memCode = string.Empty;
            g_memCurr = string.Empty;
            g_memName = string.Empty;
            g_memSettleAc = string.Empty;
            g_memNo = string.Empty;
            g_memActype = string.Empty;
            g_memIdNo = string.Empty;
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
        public MemberProfileBE()
        {
            resetVariables();
        }
        #endregion

    }
}
