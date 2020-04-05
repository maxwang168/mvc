using CommonLibrary.DBA;
using CommonLibrary.Service;
using Entity.BAS;
using log4net;
using PortalService.Contract;
using PortalService.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;

namespace PortalService.Impl.BL
{
    public class AnnounceInfoBL
    {
        #region 成員變數

        private ILog logger = LogManager.GetLogger(typeof(AnnounceInfoBL));
        private DBAccess g_dba = new DBAccess(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);

        #endregion

        #region Property

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrMessage { get; set; }

        /// <summary>
        /// 執行結果
        /// </summary>
        //public ResultBE ExecResult { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Query by Seach values
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public IEnumerable<AnnounceInfoBE> AnnounceInfoQuery()
        {
            List<AnnounceInfoBE> m_result = new List<AnnounceInfoBE>();
            try
            {
                string m_sql = @"SELECT  *
                                FROM    ZT_AnnounceInfo
                                WHERE   announce_kind = '1'
                                        AND status_flag = 'Y'
                                        AND ((ISNULL(start_date, '') = '' AND end_date >= CONVERT(VARCHAR(8), GETDATE(), 112))
                                                OR (ISNULL(start_date, '') = '' AND ISNULL(end_date, '') = '')
                                                OR (start_date <= CONVERT(VARCHAR(8), GETDATE(), 112) AND ISNULL(end_date, '') = '')
                                                OR (start_date <= CONVERT(VARCHAR(8), GETDATE(), 112) AND end_date >= CONVERT(VARCHAR(8), GETDATE(), 112)))";

                List<DBParameter> m_paraList = new List<DBParameter>();
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());

                AnnounceInfoBE m_BE = new AnnounceInfoBE();
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    m_BE = genAnnounceInfoBE(m_dataTable.Rows[i]);
                    m_result.Add(m_BE);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }

            return m_result;
        }

        public IEnumerable<AnnounceInfoBE> queryAnnounceInfo(AnnounceInfoModel p_model)
        {
            List<AnnounceInfoBE> m_result = new List<AnnounceInfoBE>();

            string m_sql = @"SELECT  a.*,
                                    ISNULL(s.code_name, '') AS announce_content_type_name,
                                    ISNULL(u1.user_name, '') AS created_by_name,
                                    ISNULL(u2.user_name, '') AS updated_by_name
                            FROM    ZT_AnnounceInfo a
                                    LEFT JOIN ZT_SysCodeInfo s ON a.announce_content_type = s.code_id AND s.cate = 'ANN_TYPE'
                                    LEFT JOIN ZT_AaUser u1 ON a.created_by = u1.user_uuid
                                    LEFT JOIN ZT_AaUser u2 ON a.updated_by = u2.user_uuid
                            WHERE   a.status_flag != 'N' ";

            List<DBParameter> m_paraList = new List<DBParameter>();
            if (p_model.StartDate != DateTime.MinValue)
            {
                m_sql += " AND a.start_date >= @start_date_s";
                m_paraList.Add(new DBParameter("@start_date_s", p_model.StartDate.Date));
            }
            if (p_model.EndDate != DateTime.MinValue)
            {
                m_sql += " AND a.start_date <= @start_date_e";
                m_paraList.Add(new DBParameter("@start_date_e", p_model.EndDate.Date.AddDays(1).AddSeconds(-1)));
            }
            m_sql += " ORDER BY a.start_date DESC";
            try
            {
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    AnnounceInfoBE m_be = genAnnounceInfoBE(m_dataTable.Rows[i]);
                    m_result.Add(m_be);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        public bool insertAnnounceInfoBE(AnnounceInfoBE p_be)
        {
            bool result = false;
            string m_sql = @"INSERT  INTO ZT_AnnounceInfo
                                    ([announce_uuid],[system_id], [market_uuid], [org_uuid], [user_uuid],
                                        [announce_kind] ,[announce_subject], [announce_content_type], [announce_text], [start_date],
                                        [end_date], [status_flag], [created_by], [created_date], [updated_by], [updated_date])
                            VALUES  (@announce_uuid, @system_id, @market_uuid, @org_uuid, @user_uuid,
                                        @announce_kind, @announce_subject, @announce_content_type, @announce_text, @start_date,
                                        @end_date, @status_flag, @created_by, @created_date, @updated_by, @updated_date) ";

            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@announce_uuid", p_be.announce_uuid));
                m_paraList.Add(new DBParameter("@system_id", "CSFM")); //p_be.system_id));
                m_paraList.Add(new DBParameter("@market_uuid", DBNull.Value)); //p_be.market_uuid));
                m_paraList.Add(new DBParameter("@org_uuid", p_be.OrgUuid));
                m_paraList.Add(new DBParameter("@user_uuid", p_be.UserUuid));
                m_paraList.Add(new DBParameter("@announce_kind", p_be.announce_kind));
                m_paraList.Add(new DBParameter("@announce_subject", p_be.announce_subject));
                m_paraList.Add(new DBParameter("@announce_content_type", p_be.announce_content_type));
                m_paraList.Add(new DBParameter("@announce_text", p_be.announce_text));
                if (p_be.start_date != DateTime.MinValue)
                {
                    m_paraList.Add(new DBParameter("@start_date", Convert.ToDateTime(p_be.start_date.ToShortDateString())));
                }
                else
                {
                    m_paraList.Add(new DBParameter("@start_date", Convert.ToDateTime(DateTime.Now.ToShortDateString()))); //立即生效, 所以存當天日期
                }
                if (p_be.end_date != DateTime.MinValue)
                {
                    m_paraList.Add(new DBParameter("@end_date", Convert.ToDateTime(p_be.end_date.ToShortDateString())));
                }
                else
                {
                    m_paraList.Add(new DBParameter("@end_date", DBNull.Value));
                }
                m_paraList.Add(new DBParameter("@status_flag", p_be.status_flag));
                m_paraList.Add(new DBParameter("@created_by", p_be.created_by));
                m_paraList.Add(new DBParameter("@created_date", p_be.created_date));
                m_paraList.Add(new DBParameter("@updated_by", p_be.updated_by));
                m_paraList.Add(new DBParameter("@updated_date", p_be.updated_date));

                int cnt = g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                ErrMessage = string.Empty;
                if (cnt < 0 || g_dba.isException)
                {
                    ErrMessage = g_dba.ex.Message;
                    result = false;
                }
                else
                    result = true;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_be.updated_by);
                result = false;
            }
            return result;
        }

        public bool updateAnnounceInfoBE(AnnounceInfoBE p_be)
        {
            bool result = false;
            string m_sql = @"UPDATE  ZT_AnnounceInfo
                                SET     announce_subject = @announce_subject,
                                        announce_content_type = @announce_content_type,
                                        announce_text = @announce_text,
                                        start_date = @start_date,
                                        end_date = @end_date,
                                        status_flag = @status_flag,
                                        updated_by = @updated_by,
                                        updated_date = @updated_date
                                WHERE   announce_uuid = @announce_uuid ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@announce_uuid", p_be.announce_uuid));
                m_paraList.Add(new DBParameter("@announce_subject", p_be.announce_subject));
                m_paraList.Add(new DBParameter("@announce_content_type", p_be.announce_content_type));
                m_paraList.Add(new DBParameter("@announce_text", p_be.announce_text));
                if (p_be.start_date != DateTime.MinValue)
                {
                    m_paraList.Add(new DBParameter("@start_date", Convert.ToDateTime(p_be.start_date.ToShortDateString())));
                }
                else
                {
                    m_paraList.Add(new DBParameter("@start_date", Convert.ToDateTime(DateTime.Now.ToShortDateString()))); //立即生效, 所以存當天日期
                }
                if (p_be.end_date != DateTime.MinValue)
                {
                    m_paraList.Add(new DBParameter("@end_date", Convert.ToDateTime(p_be.end_date.ToShortDateString())));
                }
                else
                {
                    m_paraList.Add(new DBParameter("@end_date", DBNull.Value));
                }
                m_paraList.Add(new DBParameter("@status_flag", p_be.status_flag));
                m_paraList.Add(new DBParameter("@updated_by", p_be.updated_by));
                m_paraList.Add(new DBParameter("@updated_date", p_be.updated_date));

                int cnt = g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                ErrMessage = string.Empty;
                if (cnt < 0 || g_dba.isException)
                {
                    ErrMessage = g_dba.ex.Message;
                    result = false;
                }
                else
                    result = true;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog("updateAnnounceInfoBE", "ERROR", ex.Message, p_be.updated_by);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// update status_flag by BE
        /// </summary>
        /// <param name="p_BE"></param>
        /// <returns></returns>
        public bool updateStatus(AnnounceInfoBE p_BE)
        {
            bool m_ok = false;
            string m_sql = @"UPDATE ZT_AnnounceInfo SET status_flag = @status_flag, updated_by = @updated_by, updated_date = @updated_date WHERE announce_uuid = @announce_uuid ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@announce_uuid", p_BE.announce_uuid));
                m_paraList.Add(new DBParameter("@status_flag", p_BE.status_flag));
                m_paraList.Add(new DBParameter("@updated_by", p_BE.updated_by));
                m_paraList.Add(new DBParameter("@updated_date", Convert.ToDateTime(p_BE.updated_date)));

                int cnt = g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                ErrMessage = string.Empty;
                if (cnt < 0 || g_dba.isException)
                {
                    ErrMessage = g_dba.ex.Message;
                    m_ok = false;
                }
                else
                    m_ok = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_BE.updated_by);
                m_ok = false;
            }
            return m_ok;
        }


        public bool deleteAnnounceInfoBE(AnnounceInfoBE p_be)
        {
            bool result = false;
            string m_sql = @"UPDATE  ZT_AnnounceInfo
                            SET     status_flag = @status_flag,
                                    updated_by = @updated_by,
                                    updated_date = @updated_date
                            WHERE   announce_uuid = @announce_uuid ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@announce_uuid", p_be.announce_uuid));
                m_paraList.Add(new DBParameter("@status_flag", "N"));
                m_paraList.Add(new DBParameter("@updated_by", p_be.updated_by));
                m_paraList.Add(new DBParameter("@updated_date", p_be.updated_date));

                int cnt = g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                ErrMessage = string.Empty;
                if (cnt < 0 || g_dba.isException)
                {
                    ErrMessage = g_dba.ex.Message;
                    result = false;
                }
                else
                    result = true;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_be.updated_by);
                result = false;
            }
            return result;
        }

        public AnnounceInfoModel GetAnnonuText(string uuid)
        {
            AnnounceInfoModel m_BE = new AnnounceInfoModel();
            try
            {
                string m_sql = @"SELECT  * FROM    ZT_AnnounceInfo WHERE   announce_uuid = @announce_uuid";

                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@announce_uuid", uuid));

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());

                if (m_dataTable.Rows.Count > 0)
                {
                    m_BE.AnnounceText = (string)m_dataTable.Rows[0]["announce_text"];
                }
                else
                {
                    m_BE.AnnounceText = string.Empty;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }

            return m_BE;
        }

        /// <summary>
        /// 依照服務端點依序儲存檔案
        /// </summary>
        /// <returns>Item1: 是否執行成功@true, false, Item2: 錯誤訊息</returns>
        public Tuple<bool, string> UploadFileToAP(AnnounceInfoFileUpload p_UploadFile, bool p_IsFromService)
        {
            string m_ErrMsg = string.Empty;
            try
            {
                if (p_IsFromService == false)
                {
                    string m_AnnounceInfoEndptNamePrefix =
                        ConfigurationManager.AppSettings["AnnounceInfoEndptNamePrefix"]as string;

                    if (m_AnnounceInfoEndptNamePrefix == null || m_AnnounceInfoEndptNamePrefix.Length < 1)
                    {
                        throw new Exception("組態設定檔錯誤，找不到公告服務模組前綴詞，請重新確認");
                    }

                    //loop through endpoints in web.config
                    //var m_Client = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;
                    //string m_EndpointName = string.Empty;
                    //string m_Msg = string.Empty;

                    //if (m_Client == null || m_Client.Endpoints == null)
                    //{
                    //    throw new Exception("組態設定檔錯誤，找不到對應公告服務模組，請重新確認");
                    //}

                    //for (int i = 0; i < m_Client.Endpoints.Count; i++)
                    //{
                    //    m_EndpointName = m_Client.Endpoints[i].Name;
                    //    if (m_EndpointName.Contains(m_AnnounceInfoEndptNamePrefix))
                    //    {
                    //        m_Msg = new WCFService<IBasService>(m_Client.Endpoints[i].Name).Use(
                    //                a => a.UploadFileToAP(p_UploadFile, true)).Item2;

                    //        if (m_Msg.Length > 0)
                    //        {
                    //            m_ErrMsg += $"Error on endpoint \"{m_EndpointName}\": {m_Msg}. ";
                    //            break;
                    //        }
                    //    }
                    //}
                }
                else
                {
                    string m_FileExtension = (p_UploadFile.FileExtension ?? "").ToLower();
                    string m_UploadFolderTemp = string.Empty;
                    string m_UploadFolderRoot = p_UploadFile.UploadFolderRoot;
                    string m_UploadFolder = string.Empty;
                    string m_FileName = p_UploadFile.AnnounceUuid.ToString();
                    byte[] m_FileBytes = p_UploadFile.FileBytes;

                    var m_SysCodeBE = new SysService().QuerySysCodeInfoForFile("File_PATH", "ANN_HTML_OLD");
                    if (m_SysCodeBE != null)
                    {
                        m_UploadFolder = m_SysCodeBE.VarChar01;
                        Directory.CreateDirectory(Path.Combine(m_UploadFolderRoot, m_UploadFolder));
                    }

                    m_UploadFolderTemp = Path.Combine(m_UploadFolderRoot, m_UploadFolder, $"{m_FileName}{m_FileExtension}");

                    using (FileStream m_TargetStream =
                        new FileStream(m_UploadFolderTemp, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        m_TargetStream.Write(m_FileBytes, 0, m_FileBytes.Length);
                    }
                    if (m_FileExtension == ".pdf")
                    {
                        m_UploadFolderTemp = Path.Combine(m_UploadFolderRoot, m_UploadFolder, $"{m_FileName}.html");
                        using (StreamWriter sw = new StreamWriter(m_UploadFolderTemp))
                        {
                            sw.Write($@"
<!DOCTYPE html>
<html lang='en'>
  <head>
    <style>
      html, body, object {{ width: 100%; height: 100%; min-height: 100%; }}
    </style>
  </head>
  <body>
    <object data='{m_FileName}{m_FileExtension}'></object>
  </body>
</html>");
                        }
                    }
                    //-----------------------------
                    //-----------------------------
                    m_SysCodeBE = new SysService().QuerySysCodeInfoForFile("File_PATH", "ANN_HTML");
                    if (m_SysCodeBE != null)
                    {
                        m_UploadFolder = m_SysCodeBE.VarChar01;
                        Directory.CreateDirectory(Path.Combine(m_UploadFolderRoot, m_UploadFolder));
                    }

                    m_UploadFolderTemp = Path.Combine(m_UploadFolderRoot, m_UploadFolder, $"{m_FileName}{m_FileExtension}");

                    using (FileStream m_TargetStream =
                        new FileStream(m_UploadFolderTemp, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        m_TargetStream.Write(m_FileBytes, 0, m_FileBytes.Length);
                    }
                    if (m_FileExtension == ".pdf")
                    {
                        m_UploadFolderTemp = Path.Combine(m_UploadFolderRoot, m_UploadFolder, $"{m_FileName}.html");
                        using (StreamWriter sw = new StreamWriter(m_UploadFolderTemp))
                        {
                            sw.Write($@"
<!DOCTYPE html>
<html lang='en'>
  <head>
    <style>
      html, body, object {{ width: 100%; height: 100%; min-height: 100%; }}
    </style>
  </head>
  <body>
    <object data='{m_FileName}{m_FileExtension}'></object>
  </body>
</html>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                return new Tuple<bool, string>(false, ex.Message);
            }

            if (m_ErrMsg != null && m_ErrMsg.Length > 0)
            {
                return new Tuple<bool, string>(false, m_ErrMsg);
            }
            else
            {
                return new Tuple<bool, string>(true, string.Empty);
            }
        }

        #endregion

        #region private Methods

        /// <summary>
        /// 將DataRow轉成Entity
        /// </summary>
        /// <param name="p_row"></param>
        /// <returns></returns>
        private AnnounceInfoBE genAnnounceInfoBE(DataRow p_row)
        {
            AnnounceInfoBE m_be = new AnnounceInfoBE();
            if (!p_row.IsNull("announce_uuid"))
            {
                m_be.announce_uuid = new Guid(p_row["announce_uuid"].ToString());
            }
            if (!p_row.IsNull("system_id"))
            {
                m_be.SystemId = (string)p_row["system_id"];
            }
            if (!p_row.IsNull("market_uuid"))
            {
                m_be.market_uuid = new Guid(p_row["market_uuid"].ToString());
            }
            if (!p_row.IsNull("org_uuid"))
            {
                m_be.OrgUuid = new Guid(p_row["org_uuid"].ToString());
            }
            if (!p_row.IsNull("user_uuid"))
            {
                m_be.UserUuid = new Guid(p_row["user_uuid"].ToString());
            }
            if (!p_row.IsNull("announce_kind"))
            {
                m_be.announce_kind = (string)p_row["announce_kind"];
            }
            if (!p_row.IsNull("announce_subject"))
            {
                m_be.announce_subject = (string)p_row["announce_subject"];
            }
            if (!p_row.IsNull("announce_content_type"))
            {
                m_be.announce_content_type = (string)p_row["announce_content_type"];
            }
            if (!p_row.IsNull("announce_text"))
            {
                m_be.announce_text = (string)p_row["announce_text"];
            }
            if (!p_row.IsNull("start_date"))
            {
                m_be.start_date = Convert.ToDateTime(p_row["start_date"]);
            }
            if (!p_row.IsNull("end_date"))
            {
                m_be.end_date = Convert.ToDateTime(p_row["end_date"]);
            }
            if (!p_row.IsNull("status_flag"))
            {
                m_be.status_flag = (string)p_row["status_flag"];
            }
            if (!p_row.IsNull("created_by"))
            {
                m_be.created_by = new Guid(p_row["created_by"].ToString());
            }
            if (p_row.ContainsColumn("created_by_name") && !p_row.IsNull("created_by_name"))
            {
                m_be.created_by_name = p_row["created_by_name"].ToString();
            }
            if (!p_row.IsNull("created_date"))
            {
                m_be.created_date = Convert.ToDateTime(p_row["created_date"]);
            }
            if (!p_row.IsNull("updated_by"))
            {
                m_be.updated_by = new Guid(p_row["updated_by"].ToString());
            }
            if (p_row.ContainsColumn("updated_by_name") && !p_row.IsNull("updated_by_name"))
            {
                m_be.updated_by_name = p_row["updated_by_name"].ToString();
            }
            if (!p_row.IsNull("updated_date"))
            {
                m_be.updated_date = Convert.ToDateTime(p_row["updated_date"]);
            }
            if (p_row.ContainsColumn("announce_content_type_name") && !p_row.IsNull("announce_content_type_name"))
            {
                m_be.announce_content_type_name = p_row["announce_content_type_name"].ToString();
            }

            return m_be;
        }

        #endregion

    }
}
