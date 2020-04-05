using System;
using System.Text;

namespace Entity.SYS
{
    [Serializable]
    public class AaUser : IFlowHandler
    {
        public Guid user_uuid { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string org_type { get; set; }
        public Guid org_uuid { get; set; }
        public string org_id { get; set; }
        public string org_name { get; set; }
        public Guid role_uuid { get; set; }
        public string role_id { get; set; }
        public string role_name { get; set; }
        public string pwd_status { get; set; }
        public string pwd { get; set; }
        public string pwd2 { get; set; }
        public string pwd3 { get; set; }
        public string pwd4 { get; set; }
        public DateTime pwd_modify_date { get; set; }
        public string user_mail { get; set; }
        public string tel { get; set; }
        public string user_fax { get; set; }
        public string user_mobil { get; set; }
        public string status_flag { get; set; }
        public Guid created_by { get; set; }
        public string created_by_name { get; set; }
        public string created_date { get; set; }
        public Guid updated_by { get; set; }
        public string updated_by_name { get; set; }
        public string updated_date { get; set; }
        public int retry { get; set; }
        public string Salt { get; set; }
        public bool admin_user { get; set; }
        public string pwd_encrypt { get; set; }
        public int pwd_limit { get; set; }
        public DateTime pwd_limit_date { get; set; }

        public bool isUnlock { get; set; }

        public string[] DATA_FIELD
        {
            get
            {
                return new string[] { "user_uuid", "user_name", "role_id", "role_uuid", "user_mail", "status_flag", "updated_by", "updated_date" };
            }
        }

        public string htmlStringHandler()
        {
            StringBuilder m_strHtml = new StringBuilder();
            if (isUnlock)
            {
                m_strHtml.Append("<table class='main_table' border='1' cellpadding='1' cellspacing='1' style='width:100%;'>");
                m_strHtml.Append("<thead><tr><td colspan='2'>資料內容</td></tr></thead>");
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>帳號</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", user_id));
                m_strHtml.Append("</tr>");
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>姓名</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", user_name));
                m_strHtml.Append("</tr>");
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>角色</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", role_name));
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
            }
            else
            {
                m_strHtml.Append("<table class='main_table' border='1' cellpadding='1' cellspacing='1' style='width:100%;'>");
                m_strHtml.Append("<thead><tr><td colspan='2'>資料內容</td></tr></thead>");
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel' width='15%'>統一編號</th>");
                m_strHtml.Append(string.Format("<td class='editTable' width='85%'>{0}</td>", org_name));
                m_strHtml.Append("</tr>");
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>客戶代號</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", user_id));
                m_strHtml.Append("</tr>");
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>姓名</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", user_name));
                m_strHtml.Append("</tr>");
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>角色</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", role_name));
                m_strHtml.Append("</tr>");
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>Email信箱</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", user_mail));
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
            }

            return m_strHtml.ToString();
        }
    }
}
