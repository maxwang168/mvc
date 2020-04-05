using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web;

namespace Entity.BAS
{
    [Serializable]
    public class AnnounceInfoBE : IFlowHandler
    {
        #region 成員變數
        private Guid g_announceUuid;
        private string g_systemId;
        private Guid g_marketUuid;
        private Guid g_orgUuid;
        private Guid g_userUuid;
        private string g_announceKind;
        private string g_announceSubject;
        private string g_announceContentType;
        private string g_announceText;
        private DateTime g_startDate;
        private DateTime g_endDate;
        private string g_statusFlag;
        private Guid g_createdBy;
        private string g_createdByName;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private string g_updatedByName;
        private DateTime g_updatedDate;
        private string g_flowStatus;
        private string g_announceContentTypeName;
        #endregion

        #region Property
        [Display(Name = "announce_uuid")]
        public Guid announce_uuid
        {
            get { return g_announceUuid; }
            set { g_announceUuid = value; }
        }

        [Display(Name = "SystemId")]
        public string SystemId
        {
            get { return g_systemId; }
            set { g_systemId = value; }
        }

        [Display(Name = "market_uuid")]
        public Guid market_uuid
        {
            get { return g_marketUuid; }
            set { g_marketUuid = value; }
        }

        [Display(Name = "OrgUuid")]
        public Guid OrgUuid
        {
            get { return g_orgUuid; }
            set { g_orgUuid = value; }
        }

        [Display(Name = "UserUuid")]
        public Guid UserUuid
        {
            get { return g_userUuid; }
            set { g_userUuid = value; }
        }

        [Display(Name = "announce_kind")]
        public string announce_kind
        {
            get { return g_announceKind; }
            set { g_announceKind = value; }
        }

        [Display(Name = "announce_subject")]
        public string announce_subject
        {
            get { return g_announceSubject; }
            set { g_announceSubject = value; }
        }

        [Display(Name = "announce_content_type")]
        public string announce_content_type
        {
            get { return g_announceContentType; }
            set { g_announceContentType = value; }
        }

        [Display(Name = "announce_text")]
        public string announce_text
        {
            get { return g_announceText; }
            set { g_announceText = value; }
        }

        [Display(Name = "start_date")]
        public DateTime start_date
        {
            get { return g_startDate; }
            set { g_startDate = value; }
        }

        [Display(Name = "end_date")]
        public DateTime end_date
        {
            get { return g_endDate; }
            set { g_endDate = value; }
        }

        [Display(Name = "status_flag")]
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

        [Display(Name = "FlowStatus")]
        public string FlowStatus
        {
            get { return g_flowStatus; }
            set { g_flowStatus = value; }
        }

        [Display(Name = "announce_content_type_name")]
        public string announce_content_type_name
        {
            get { return g_announceContentTypeName; }
            set { g_announceContentTypeName = value; }
        }

        public string Ann_Path { get; set; }
        public string Ann_File { get; set; }
        public string Ann_Url { get; set; }

        #endregion

        #region Public Method
        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_announceUuid = new Guid();
            g_systemId = string.Empty;
            g_marketUuid = new Guid();
            g_orgUuid = new Guid();
            g_userUuid = new Guid();
            g_announceKind = string.Empty;
            g_announceSubject = string.Empty;
            g_announceContentType = string.Empty;
            g_announceText = string.Empty;
            g_startDate = new DateTime();
            g_endDate = new DateTime();
            g_statusFlag = string.Empty;
            g_createdBy = new Guid();
            g_createdByName = string.Empty;
            g_createdDate = new DateTime();
            g_updatedBy = new Guid();
            g_updatedByName = string.Empty;
            g_updatedDate = new DateTime();
            g_flowStatus = string.Empty;
            g_announceContentTypeName = string.Empty;
        }

        public string[] DATA_FIELD
        {
            get
            {
                return new string[] { "announce_uuid", "announce_subject", "announce_content_type", "announce_text", "start_date",
                    "end_date", "status_flag", "updated_by", "updated_date" };
            }
        }

        public string htmlStringHandler()
        {
            StringBuilder m_strHtml = new StringBuilder();
            m_strHtml.Append("<table class='main_table' border='1' cellpadding='1' cellspacing='1' style='width:100%;'>");
            m_strHtml.Append("<thead><tr><td colspan='2'>資料內容</td></tr></thead>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel' width='15%'>主旨</th>");
            m_strHtml.Append(string.Format("<td class='editTable' width='85%'>{0}</td>", announce_subject));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>類型</th>");
            m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", announce_content_type + "-" + announce_content_type_name));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>公告訊息</th>");
            if (announce_content_type == "HTML")
            {
                string urlContent = string.Format("<a href=\"{0}/{1}/{2}.html\" target=\"_blank\">{3}</a>", Ann_Url, Ann_Path, announce_uuid, announce_subject);
                m_strHtml.Append(string.Format("<td class='editTable' style='white-space: pre-wrap; '>{0}</td>", urlContent));
            }
            else
            {
                m_strHtml.Append(string.Format("<td class='editTable' style='white-space: pre-wrap; '>{0}</td>", announce_text));
            }
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>上架日期</th>");
            m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", start_date == DateTime.MinValue ? "" : start_date.ToString("yyyy/MM/dd")));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>下架日期</th>");
            m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", end_date == DateTime.MinValue ? "" : end_date.ToString("yyyy/MM/dd")));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
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

        #region 建構函數/Dispose
        public AnnounceInfoBE()
        {
            resetVariables();
        }
        #endregion

    }
}
