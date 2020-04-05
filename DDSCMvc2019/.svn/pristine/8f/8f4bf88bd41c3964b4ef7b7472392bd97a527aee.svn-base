using Entity.SYS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using CommonLibrary.DES;
using System.Net;
using log4net;

namespace PortalService.Impl.Monitor
{
    public class EmailChannel : IMonitorChannel
    {
        private static string msgTitle = "等級:{0} 功能名稱:{1} 監控通知";
        private static string msgBody = "等級:{0} 功能名稱:{1} 監控通知<br/>訊息內容{2}";
        private string g_host;
        private int g_port;
        private bool g_enableSSL;
        private string g_senderAddress;
        private string g_uid;
        private string g_pd;
        private SmtpClient g_client;

        private ILog g_logger = LogManager.GetLogger(typeof(EmailChannel));

        public EmailChannel()
        {
            string m_ErrMsg = string.Empty;
            SysService m_sysService = new SysService();
            SysCodeInfoBE m_info = m_sysService.QuerySysCodeInfoForFile("Notify", "SMTP");
            g_host = m_info.VarChar01;
            g_port = Convert.ToInt32(m_info.VarChar02);
            if (m_info.VarChar03 == "Y")
            {
                g_enableSSL = true;
            }
            g_senderAddress = m_info.VarChar04;
            g_uid = m_info.VarChar05;
            g_pd = CommonLibrary.DES.DESCode.desDecryptBase64(m_info.VarChar06, ref m_ErrMsg);
            if (!string.IsNullOrEmpty(m_ErrMsg))
            {
                throw new Exception(m_ErrMsg);
            }
            g_client = new SmtpClient()
            {
                Host = g_host,
                Port = g_port,
                EnableSsl = g_enableSSL,
                Timeout = 300000
            };
            if (!string.IsNullOrEmpty(g_uid) && !string.IsNullOrEmpty(g_pd))
            {
                g_client.Credentials = new System.Net.NetworkCredential(g_uid, g_pd);
            }
            ServicePointManager.ServerCertificateValidationCallback = delegate { return string.IsNullOrWhiteSpace(string.Empty); };
        }

        public void ChannelProcess(string function_code, string level_code, string message, Guid user_uuid)
        {
            List<MailMessage> messageList = new List<MailMessage>();
            
            string sql = @"SELECT F.function_name, L.level_name , [address]
  FROM [ZT_MonitorSetting] AS S
  JOIN [ZT_MonitorNotify] AS N ON N.notify_code = S.notify_code AND N.type = '02'
  JOIN [ZT_MonitorNotifyDetail] AS D ON D.notify_code = N.notify_code
  JOIN [ZT_MonitorFunction] AS F ON F.function_code = S.function_code
  JOIN [ZT_MonitorLevel] AS L ON L.level_code = S.level_code
  WHERE S.function_code=@function_code AND S.level_code=@level_code";
            try
            {
                string m_ConnectionString = ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString;
                string m_ErrorMessage = string.Empty;
                using (SqlConnection connection = new SqlConnection(DESCode.desDecryptBase64(m_ConnectionString, ref m_ErrorMessage)))
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@function_code", function_code);
                    cmd.Parameters.AddWithValue("@level_code", level_code);
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string funName = (string) reader["function_name"];
                            string levName = (string) reader["level_name"];
                            string address = (string) reader["address"];
                            MailMessage mail = new MailMessage();
                            try
                            {
                                mail.From = new MailAddress(g_senderAddress);
                                mail.To.Add(new MailAddress(address));
                                mail.Subject = string.Format(msgTitle, levName, funName);
                                mail.IsBodyHtml = true;
                                mail.Body = string.Format(msgBody, levName, funName, message);
                                messageList.Add(mail);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            for (int i = 0; i < messageList.Count; i++)
            {
                try
                {
                    g_logger.Debug(
                        $"From: {messageList[i].From}, To: {messageList[i].To[0]}, Subject: {messageList[i].Subject}, Body: {messageList[i].Body}");
                    g_client.Send(messageList[i]);
                    g_logger.Debug("Mail sent.");
                }
                catch (Exception ex)
                {
                    g_logger.Error(ex);
                    g_logger.Debug("Mail send failed.");
                    string a = ex.Message;
                }
            }
        }
    }
}
