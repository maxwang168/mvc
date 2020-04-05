using CommonLibrary.DBA;
using Entity.BAS;
using log4net;
using PortalService.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace PortalService.Impl.BL
{//BAS/NotifyRec/q/Index
    public class NotifyRecBL
    {
        #region 成員變數

        private ILog logger = LogManager.GetLogger(typeof(NotifyRecBL));
        private DBAccess g_dba = new DBAccess(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);

        #endregion

        #region Property

        #endregion

        #region Public Methods

        /// <summary>
        /// Query by Seach values
        /// </summary>
        /// <param name="p_ViewModel"></param>
        /// <returns></returns>
        public IEnumerable<NotifyRecBE> NotifyRecQuery(NotifyRecModel p_ViewModel)
        {
            List<NotifyRecBE> m_result = new List<NotifyRecBE>();
            try
            {
                string m_sql = @"SELECT a.*, 
ISNULL(AaUser1.user_name, '') as created_by_name,
ISNULL(AaUser2.user_name, '') as updated_by_name, 
ISNULL(AaUser3.user_name, '') as user_by_name 
FROM ZT_NotifyRec AS a
LEFT JOIN ZT_AaUser AS AaUser1 ON a.created_by = AaUser1.user_uuid
LEFT JOIN ZT_AaUser AS AaUser2 ON a.updated_by = AaUser2.user_uuid
LEFT JOIN ZT_AaUser AS AaUser3 ON a.user_uuid = AaUser3.user_uuid
WHERE 1=1 ";

                List<DBParameter> m_paraList = new List<DBParameter>();
                //起
                if (!string.IsNullOrEmpty(p_ViewModel.DateStart))
                {
                    m_sql += @" AND a.updated_date >= @date_s ";
                    m_paraList.Add(new DBParameter("@date_s", p_ViewModel.DateStart));
                }

                //訖
                if (!string.IsNullOrEmpty(p_ViewModel.DateEnd))
                {
                    m_sql += " AND a.updated_date <= @date_e ";
                    m_paraList.Add(new DBParameter("@date_e", (p_ViewModel.DateEnd)));
                }

                //contact
                if (!string.IsNullOrEmpty(p_ViewModel.Contact))
                {
                    m_sql += " AND a.contact_addr Like @contact_addr ";
                    m_paraList.Add(new DBParameter("@contact_addr", "%" + p_ViewModel.Contact + "%"));
                }

                //Channel
                if (!string.IsNullOrEmpty(p_ViewModel.Channel))
                {
                    m_sql += " AND a.channel = @channel ";
                    m_paraList.Add(new DBParameter("@channel", p_ViewModel.Channel));
                }

                //Status
                if (!string.IsNullOrEmpty(p_ViewModel.Status))
                {
                    m_sql += " AND a.proc_status = @proc_status ";
                    m_paraList.Add(new DBParameter("@proc_status", p_ViewModel.Status));
                }

                m_sql += " ORDER BY a.[updated_date] DESC";

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());

                NotifyRecBE m_BE = new NotifyRecBE();
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    m_BE = genNotifyRecBE(m_dataTable.Rows[i]);
                    m_result.Add(m_BE);
                }
                //foreach (DataRow dr in m_dataTable.Rows)
                //{
                //    m_result.Add(BLHelper.DataRowToBE<MemberProfileBE>(dr));
                //}
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }

            return m_result;
        }
        /*
        /// <summary>
        /// Query-count of Same work_date (before insert)
        /// </summary>
        /// <param name="p_work_date">p_work_date</param>
        /// <param name="p_uuid">member_uuid</param>
        /// <returns></returns>
        public int BasWorkingQryCnt(string p_work_date, Guid p_uuid)
        {
            int m_result = -1;
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                string m_sql = @" 
SELECT count(1) as count 
FROM ZT_BasWorking 
WHERE work_date = @work_date AND bas_working_uuid <> @bas_working_uuid ";
                m_paraList.Add(new DBParameter("@work_date", p_work_date));
                m_paraList.Add(new DBParameter("@bas_working_uuid", p_uuid));

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                if (m_dataTable != null && m_dataTable.Rows.Count >= 1)
                    m_result = Convert.ToInt32(m_dataTable.Rows[0][0]);
            }
            catch (Exception ex)
            {
                m_result = -1;
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }
        */
        /*
        /// <summary>
        /// insert by BE
        /// </summary>
        /// <param name="p_BE"></param>
        /// <returns></returns>
        public bool insertBasWorkingBE(BasWorkingBE p_BE)
        {
            bool m_ok = false;
            string m_sql = @"INSERT INTO ZT_NotifyRec (rec_uuid, notify_uuid, user_uuid,
contact_addr, channel, req_data, notify_title, notify_data, proc_status, schedule,
status_flag, created_by ,created_date, updated_by, updated_date) VALUES (@rec_uuid, 
@notify_uuid, @user_uuid, @contact_addr, @channel, @req_data, @notify_title, @notify_data,
@proc_status, @schedule, @status_flag, @created_by , getdate(), @updated_by, getdate())";
            try
            {
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
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_BE.UpdatedBy);
                m_ok = false;
            }
            return m_ok;
        }

        /// <summary>
        /// update by BE
        /// </summary>
        /// <param name="p_BE"></param>
        /// <returns></returns>
        public bool updateBasWorkingBE(BasWorkingBE p_BE)
        {
            bool m_ok = false;
            string m_sql = @"UPDATE [dbo].[ZT_BasWorking] SET work_date=@work_date
,delivery_date_yn=@delivery_date_yn,trade_date_yn=@trade_date_yn
,updated_by=@updated_by,updated_date=@updated_date 
WHERE bas_working_uuid=@bas_working_uuid";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@work_date", p_BE.WorkDate));
                m_paraList.Add(new DBParameter("@delivery_date_yn", p_BE.DeliveryDate));
                m_paraList.Add(new DBParameter("@trade_date_yn", p_BE.TradeDate));
                m_paraList.Add(new DBParameter("@updated_by", p_BE.UpdatedBy));
                m_paraList.Add(new DBParameter("@updated_date", Convert.ToDateTime(p_BE.UpdatedDate)));
                m_paraList.Add(new DBParameter("@bas_working_uuid", p_BE.BasWorkingUuid));

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
                  new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_BE.UpdatedBy);
                m_ok = false;
            }
            return m_ok;
        }

        /// <summary>
        /// delete by key
        /// </summary>
        /// <param name="p_uuid"></param>
        /// <returns></returns>
        public bool deleteBasWorkingBE(Guid p_uuid)
        {
            bool m_ok = false;
            string m_sql = @"DELETE FROM [dbo].[ZT_BasWorking] 
            WHERE bas_working_uuid= @bas_working_uuid AND 1=1";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@bas_working_uuid", p_uuid));

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
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                m_ok = false;
            }
            return m_ok;
        }
        */
        public IEnumerable<string[]> QueryForChannel()
        {
            List<string[]> m_list = new List<string[]>();

            /*string m_sql = @"
SELECT distinct B.channel 
FROM ZT_NotifyRec B
WHERE 1=1 
ORDER BY B.channel";*/
            //參數
            string m_sql = @"
SELECT distinct S.code_id,S.code_name,B.channel
FROM ZT_SysCodeInfo S
FULL JOIN ZT_NotifyRec B
ON S.code_id = B.channel
WHERE S.cate = 'NOTCHANNEL' 
";

            DataTable m_dataTable = g_dba.GetDataTable(m_sql.ToString());
            List<string> m_string;
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                m_string = new List<string>();
                m_string.Add(m_dataTable.Rows[i]["code_id"].ToString());
                m_string.Add(m_dataTable.Rows[i]["code_name"].ToString());
                m_list.Add(m_string.ToArray());
            }
            return m_list;
        }
        public IEnumerable<string[]> QueryForStatus()
        {
            List<string[]> m_list = new List<string[]>();

            /*string m_sql = @"
SELECT distinct B.proc_status 
FROM ZT_NotifyRec B
WHERE 1=1 
ORDER BY B.proc_status";*/
            //參數
            string m_sql = @"
SELECT distinct S.code_id,S.code_name,B.proc_status
FROM ZT_SysCodeInfo S
FULL JOIN ZT_NotifyRec B
ON S.code_id = B.proc_status
WHERE S.cate = 'STATUS' 
";

            DataTable m_dataTable = g_dba.GetDataTable(m_sql.ToString());
            List<string> m_string;
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                m_string = new List<string>();
                m_string.Add(m_dataTable.Rows[i]["code_id"].ToString());
                m_string.Add(m_dataTable.Rows[i]["code_name"].ToString());
                m_list.Add(m_string.ToArray());
            }
            return m_list;
        }

        public IEnumerable<NotifyRecBE> QueryNotifyRec(NotifyRecModel p_model)
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
                    m_rec.RecUuid = (Guid)m_dataTable.Rows[i]["rec_uuid"];
                    m_rec.NotifyUuid = (Guid)m_dataTable.Rows[i]["notify_uuid"];
                    m_rec.UserUuid = (Guid)m_dataTable.Rows[i]["user_uuid"];
                    m_rec.Contact = (string)m_dataTable.Rows[i]["contact_addr"];
                    m_rec.Channel = (string)m_dataTable.Rows[i]["channel"];
                    m_rec.ReqData = (string)m_dataTable.Rows[i]["req_data"];
                    m_rec.NotifyTitle = (string)m_dataTable.Rows[i]["notify_title"];
                    m_rec.NotifyData = (string)m_dataTable.Rows[i]["notify_data"];
                    m_rec.Status = ((int)m_dataTable.Rows[i]["proc_status"]).ToString();
                    m_rec.Schedule = (DateTime)m_dataTable.Rows[i]["schedule"];
                    m_rec.StatusFlag = (string)m_dataTable.Rows[i]["status_flag"];
                    m_rec.CreatedBy = (Guid)m_dataTable.Rows[i]["created_by"];
                    m_rec.CreatedDate = (DateTime)m_dataTable.Rows[i]["created_date"];
                    m_rec.UpdatedBy = (Guid)m_dataTable.Rows[i]["updated_by"];
                    m_rec.UpdatedDate = (DateTime)m_dataTable.Rows[i]["updated_date"];

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

        #endregion

        #region private Methods

        /// <summary>
        /// 將DataRow轉成Entity
        /// </summary>
        /// <param name="p_row"></param>
        /// <returns></returns>
        private NotifyRecBE genNotifyRecBE(DataRow p_row)
        {
            NotifyRecBE m_be = new NotifyRecBE();
            if (!p_row.IsNull("rec_uuid"))
            {
                m_be.RecUuid = new Guid(p_row["rec_uuid"].ToString());
            }
            if (!p_row.IsNull("updated_date"))
            {
                m_be.Date = ((DateTime)p_row["updated_date"]).ToString("yyyy/MM/dd");
            }
            if (!p_row.IsNull("user_uuid"))
            {
                m_be.UserUuid = new Guid(p_row["user_uuid"].ToString());
            }
            if (!p_row.IsNull("user_by_name"))
            {
                m_be.UserByName = p_row["user_by_name"].ToString();
            }
            if (!p_row.IsNull("contact_addr"))
            {
                m_be.Contact = (string)p_row["contact_addr"];
            }
            if (!p_row.IsNull("channel"))
            {
                m_be.Channel = (string)p_row["channel"];
            }
            if (!p_row.IsNull("proc_status"))
            {
                m_be.Status = p_row["proc_status"].ToString();
            }
            if (!p_row.IsNull("created_by"))
            {
                m_be.CreatedBy = new Guid(p_row["created_by"].ToString());
            }
            if (!p_row.IsNull("created_by_name"))
            {
                m_be.CreatedByName = p_row["created_by_name"].ToString();
            }
            if (!p_row.IsNull("created_date"))
            {
                m_be.CreatedDate = Convert.ToDateTime(p_row["created_date"].ToString());
            }
            if (!p_row.IsNull("updated_by"))
            {
                m_be.UpdatedBy = new Guid(p_row["updated_by"].ToString());
            }
            if (!p_row.IsNull("updated_by_name"))
            {
                m_be.UpdatedByName = p_row["updated_by_name"].ToString();
            }
            if (!p_row.IsNull("updated_date"))
            {
                m_be.UpdatedDate = Convert.ToDateTime(p_row["updated_date"].ToString());
            }

            return m_be;
        }

        #endregion

    }
}
