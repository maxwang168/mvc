using CommonLibrary.DBA;
using CommonLibrary.Service;
using Entity.BAS;
using Entity.SYS;
using log4net;
using PortalService.Contract;
using PortalService.Impl.Monitor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PortalService.Impl
{
    public class LogService : ILogService
    {
        private ILog logger = LogManager.GetLogger(typeof(LogService));
        private DBASqlLog g_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);

        #region ZT_SysUserLog
        public void insertLog(SysUserLogBE log)
        {
            string m_sql = @"INSERT INTO [dbo].[ZT_SysUserLog] ([user_log_uuid],[user_info_uuid],[user_uuid],[func_id],[exe_date],[exe_btn],[exe_query],[exe_result],[status_flag],[created_by],[created_date],[updated_by],[updated_date])
VALUES (@user_log_uuid,@user_info_uuid,@user_uuid,@func_id,@exe_date,@exe_btn,@exe_query,@exe_result,@status_flag,@created_by,@created_date,@updated_by,@updated_date)";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_log_uuid", log.user_log_uuid));
                m_paraList.Add(new DBParameter("@user_info_uuid", log.user_info_uuid));
                m_paraList.Add(new DBParameter("@user_uuid", log.user_uuid));
                m_paraList.Add(new DBParameter("@func_id", log.func_id));
                m_paraList.Add(new DBParameter("@exe_date", log.exe_date));
                m_paraList.Add(new DBParameter("@exe_btn", log.exe_btn));
                m_paraList.Add(new DBParameter("@exe_query", log.exe_query));
                m_paraList.Add(new DBParameter("@exe_result", log.exe_result));
                m_paraList.Add(new DBParameter("@status_flag", log.status_flag));
                m_paraList.Add(new DBParameter("@created_by", log.created_by));
                m_paraList.Add(new DBParameter("@created_date", log.created_date));
                m_paraList.Add(new DBParameter("@updated_by", log.updated_by));
                m_paraList.Add(new DBParameter("@updated_date", log.updated_date));

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, log.updated_by);
            }
        }
        #endregion

        #region MonitorLog
        public void InsertMonitorLog(string function_code, string level_code, string message, Guid userUuid)
        {
            string m_sql = @"SELECT C.var_char01
  FROM [ZT_MonitorSetting] AS S
  JOIN [ZT_MonitorNotify] AS N ON N.notify_code = S.notify_code
  JOIN [ZT_SysCodeInfo] AS C ON N.[type] = C.code_id AND cate='NOTIFY_TYPE'
  WHERE function_code=@function_code AND level_code=@level_code";
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@function_code", function_code));
            m_paraList.Add(new DBParameter("@level_code", level_code));
            try
            {
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    string processName = (string)m_dataTable.Rows[i]["var_char01"];
                    Type m_taskType = Type.GetType(string.Format("PortalService.Impl.Monitor.{0}, PortalService.Impl", processName));
                    IMonitorChannel channel = (IMonitorChannel)Activator.CreateInstance(m_taskType);
                    channel.ChannelProcess(function_code, level_code, message, userUuid);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, userUuid);
            }
        }
        #endregion

       
        
    }
}
