using CommonLibrary.DBA;
using Entity.SYS;
using log4net;
using PortalService.Contract;
using PortalService.Contract.ViewModel.System;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;

namespace PortalService.Impl
{
    public class NotifyService : INotifyService
    {
        private DBASqlLog g_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);
        private static ILog logger = LogManager.GetLogger(typeof(NotifyService));

        public bool NotifySend(NotifySendModel p_sendModel)
        {
            try
            {
                NotifyData m_notifyData = GetNotifyData(p_sendModel.NotifyCodeId);

                if (m_notifyData == null)
                {
                    return false;
                }
                string m_title = Transform(m_notifyData.template_title, p_sendModel.DataXml);
                string m_body = Transform(m_notifyData.template, p_sendModel.DataXml);
                List<string> m_contactList;
                if (p_sendModel.IsSubscription)
                {
                    m_contactList = new List<string>();
                }
                else
                {
                    if (p_sendModel.ContactUserUuid == Guid.Empty)
                    {
                        m_contactList = p_sendModel.ContactAddrList;
                    }
                    else
                    {
                        m_contactList = new List<string>();
                        m_contactList.Add(GetUserContact(p_sendModel.ContactUserUuid, m_notifyData.channel));
                    }
                }
                DateTime m_now = DateTime.Now;
                NotifyRecBE m_rec = new NotifyRecBE();

                m_rec.notify_uuid = m_notifyData.notify_uuid;
                m_rec.user_uuid = p_sendModel.ContactUserUuid;
                m_rec.proc_status = 0;
                m_rec.channel = m_notifyData.channel;
                m_rec.notify_title = m_title;
                m_rec.notify_data = m_body;
                m_rec.req_data = p_sendModel.DataXml;
                if (p_sendModel.Schedule != DateTime.MinValue)
                {
                    m_rec.schedule = p_sendModel.Schedule;
                }
                else
                {
                    m_rec.schedule = m_now;
                }
                m_rec.status_flag = "Y";
                m_rec.created_by = p_sendModel.UserUuid;
                m_rec.updated_by = p_sendModel.UserUuid;
                for (int i = 0; i < m_contactList.Count; i++)
                {
                    m_rec.rec_uuid = Guid.NewGuid();
                    m_rec.contact_addr = m_contactList[i];

                    InsertNotify(m_rec);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                return false;
            }
        }

        private void InsertNotify(NotifyRecBE p_rec)
        {
            string m_sql = @"INSERT INTO ZT_NotifyRec (rec_uuid, notify_uuid, user_uuid,
contact_addr, channel, req_data, notify_title, notify_data, proc_status, schedule,
status_flag, created_by ,created_date, updated_by, updated_date) VALUES (@rec_uuid, 
@notify_uuid, @user_uuid, @contact_addr, @channel, @req_data, @notify_title, @notify_data,
@proc_status, @schedule, @status_flag, @created_by , getdate(), @updated_by, getdate())";
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@rec_uuid", p_rec.rec_uuid));
            m_paraList.Add(new DBParameter("@notify_uuid", p_rec.notify_uuid));
            m_paraList.Add(new DBParameter("@user_uuid", p_rec.user_uuid));
            m_paraList.Add(new DBParameter("@contact_addr", p_rec.contact_addr));
            m_paraList.Add(new DBParameter("@channel", p_rec.channel));
            m_paraList.Add(new DBParameter("@req_data", p_rec.req_data));
            m_paraList.Add(new DBParameter("@notify_title", p_rec.notify_title));
            m_paraList.Add(new DBParameter("@notify_data", p_rec.notify_data));
            m_paraList.Add(new DBParameter("@proc_status", p_rec.proc_status));
            m_paraList.Add(new DBParameter("@schedule", p_rec.schedule));
            m_paraList.Add(new DBParameter("@status_flag", p_rec.status_flag));
            m_paraList.Add(new DBParameter("@created_by", p_rec.created_by));
            m_paraList.Add(new DBParameter("@updated_by", p_rec.updated_by));

            g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
        }

        private string GetUserContact(Guid p_userUuid, string p_channel)
        {
            string m_contactAddr = string.Empty;
            string m_sql = @"SELECT * FROM ZT_AaUser WITH (NOLOCK) WHERE user_uuid=@user_uuid";
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@user_uuid", p_userUuid));
            try
            {
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                if (m_dataTable.Rows.Count > 0)
                {
                    if (p_channel == "E" || p_channel == "M")
                    {
                        m_contactAddr = (string)m_dataTable.Rows[0]["user_mail"];
                    }
                    else if (p_channel == "S")
                    {
                        m_contactAddr = (string)m_dataTable.Rows[0]["user_mobil"];
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_contactAddr;
        }

        private NotifyData GetNotifyData(string p_codeId)
        {
            NotifyData m_notifyData = null;

            string m_sql = @"SELECT N.notify_uuid,N.code_id,T.channel,T.template,
T.template_title 
FROM ZT_Notify AS N JOIN ZT_NotifyTemplate AS T ON
N.template_uuid = T.template_uuid
WHERE N.code_id=@code_id AND N.status_flag = 'Y'";
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@code_id", p_codeId));

            try
            {
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());

                if (m_dataTable.Rows.Count > 0)
                {
                    m_notifyData = new NotifyData();
                    m_notifyData.channel = (string)m_dataTable.Rows[0]["channel"];
                    m_notifyData.code_id = (string)m_dataTable.Rows[0]["code_id"];
                    m_notifyData.notify_uuid = (Guid)m_dataTable.Rows[0]["notify_uuid"];
                    m_notifyData.template = (string)m_dataTable.Rows[0]["template"];
                    m_notifyData.template_title = (string)m_dataTable.Rows[0]["template_title"];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_notifyData;
        }

        private string Transform(string p_xslt, string p_xml)
        {
            StringReader xsltInput = new StringReader(p_xslt);
            StringReader xmlInput = new StringReader(p_xml);
            XmlTextReader xsltReader = new XmlTextReader(xsltInput);
            XmlTextReader xmlReader = new XmlTextReader(xmlInput);

            StringWriter stringWriter = new StringWriter();
            XmlTextWriter transformedXml = new XmlTextWriter(stringWriter);
            XslCompiledTransform xsltTransform = new XslCompiledTransform();
            try
            {
                xsltTransform.Load(xsltReader);

                xsltTransform.Transform(xmlReader, transformedXml);
            }
            catch (XmlException xmlEx)
            {
                throw xmlEx;
            }
            catch (XsltException xsltEx)
            {
                throw xsltEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            string m_transData = stringWriter.ToString();

            return m_transData;
        }

        public IEnumerable<NotifyRecBE> QueryNotifyRec(NotifyQueryModel p_model)
        {
            List<NotifyRecBE> m_result = new List<NotifyRecBE>();
            string m_sql = "SELECT * FROM ZT_NotifyRec WHERE 1=1";
            List<DBParameter> m_paraList = new List<DBParameter>();

            if (p_model.RecUuid != Guid.Empty)
            {
                m_sql += " AND rec_uuid = @rec_uuid";
                m_paraList.Add(new DBParameter("@rec_uuid", p_model.RecUuid));
            }
            if (p_model.SendDateS != DateTime.MinValue)
            {
                m_sql += " AND updated_date >= @sdate";
                m_paraList.Add(new DBParameter("@sdate", p_model.SendDateS));
            }
            if (p_model.SendDateS != DateTime.MinValue)
            {
                m_sql += " AND updated_date <= @edate";
                m_paraList.Add(new DBParameter("@edate", p_model.SendDateE));
            }
            if (p_model.PorcStatus.HasValue)
            {
                m_sql += " AND proc_status = @proc_status";
                m_paraList.Add(new DBParameter("@proc_status", p_model.PorcStatus));
            }
            if (p_model.Schedule != DateTime.MinValue)
            {
                m_sql += " AND schedule <= @schedule";
                m_paraList.Add(new DBParameter("@schedule", p_model.Schedule));
            }
            if (p_model.UserUuid != null && p_model.UserUuid != Guid.Empty)
            {
                m_sql += " AND user_uuid = @user_uuid";
                m_paraList.Add(new DBParameter("@user_uuid", p_model.UserUuid));
            }
            m_sql += " ORDER BY created_date DESC ";
            try
            {
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    NotifyRecBE m_rec = new NotifyRecBE();
                    m_rec.rec_uuid = (Guid)m_dataTable.Rows[i]["rec_uuid"];
                    m_rec.notify_uuid = (Guid)m_dataTable.Rows[i]["notify_uuid"];
                    m_rec.user_uuid = (Guid)m_dataTable.Rows[i]["user_uuid"];
                    m_rec.contact_addr = (string)m_dataTable.Rows[i]["contact_addr"];
                    m_rec.channel = (string)m_dataTable.Rows[i]["channel"];
                    m_rec.req_data = (string)m_dataTable.Rows[i]["req_data"];
                    m_rec.notify_title = (string)m_dataTable.Rows[i]["notify_title"];
                    m_rec.notify_data = (string)m_dataTable.Rows[i]["notify_data"];
                    m_rec.proc_status = (int)m_dataTable.Rows[i]["proc_status"];
                    m_rec.schedule = (DateTime)m_dataTable.Rows[i]["schedule"];
                    m_rec.status_flag = (string)m_dataTable.Rows[i]["status_flag"];
                    m_rec.created_by = (Guid)m_dataTable.Rows[i]["created_by"];
                    m_rec.created_date = (DateTime)m_dataTable.Rows[i]["created_date"];
                    m_rec.updated_by = (Guid)m_dataTable.Rows[i]["updated_by"];
                    m_rec.updated_date = (DateTime)m_dataTable.Rows[i]["updated_date"];

                    m_result.Add(m_rec);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }

            return m_result;
        }

        public void UpdateNotifyStatus(int p_status, Guid p_recId)
        {
            string m_sql = @"UPDATE ZT_NotifyRec SET proc_status=@proc_status 
WHERE rec_uuid=@rec_uuid";
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@proc_status", p_status));
            m_paraList.Add(new DBParameter("@rec_uuid", p_recId));
            try
            {
                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
        }


    }
}
