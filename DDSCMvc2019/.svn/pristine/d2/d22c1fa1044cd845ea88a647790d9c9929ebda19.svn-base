using CommonLibrary.DBA;
using Entity.SYS;
using log4net;
using PortalService.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace PortalService.Impl.BL
{
    public class FlowJobBL
    {
        #region 成員變數

        private ILog logger = LogManager.GetLogger(typeof(FlowJobBL));
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
        public IEnumerable<FlowJobBE> FlowJobQuery(FlowJobModel p_VM)
        {
            List<FlowJobBE> m_result = new List<FlowJobBE>();


            string m_sql = @"SELECT l.*, D.orginal_data, P.func_entity,
ISNULL(AaUser1.user_name, '') as created_by_name,
ISNULL(AaUser2.user_name, '') as updated_by_name, 
ISNULL(AaUser3.user_name, '') as user_info_name 
FROM ZT_FlwJob l
JOIN ZT_FlwJobData AS D ON D.job_uuid = l.job_uuid 
JOIN ZT_SysProgram AS P ON P.func_id = l.function_id
LEFT JOIN ZT_AaUser AS AaUser1 ON l.created_by = AaUser1.user_uuid
LEFT JOIN ZT_AaUser AS AaUser2 ON l.updated_by = AaUser2.user_uuid
LEFT JOIN ZT_AaUser AS AaUser3 ON l.user_info_uuid = AaUser3.user_uuid
WHERE 1=1 AND l.created_by = @userUuid";

            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                if ((!string.IsNullOrEmpty(p_VM.FuncId)))
                {
                    m_sql += " AND l.function_id=@function_id";
                    m_paraList.Add(new DBParameter("@function_id", p_VM.FuncId));
                }
                if ((!string.IsNullOrEmpty(p_VM.FlowStatus)))
                {
                    m_sql += " AND l.flw_status=@flw_status";
                    m_paraList.Add(new DBParameter("@flw_status", p_VM.FlowStatus));
                }
                if ((!string.IsNullOrEmpty(p_VM.DateStart)))
                {
                    m_sql += " AND l.created_date>=@startDate";
                    m_paraList.Add(new DBParameter("@startDate", p_VM.DateStart));
                }
                if ((!string.IsNullOrEmpty(p_VM.DateEnd)))
                {
                    m_sql += " AND l.created_date<=@endDate";
                    m_paraList.Add(new DBParameter("@endDate", p_VM.DateEnd));
                }
                m_paraList.Add(new DBParameter("@userUuid", p_VM.userUuid));
                m_sql += " ORDER BY l.created_date DESC";


                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                FlowJobBE m_BE = new FlowJobBE();

                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {

                    m_BE = genFlowJobBE(m_dataTable.Rows[i]);
                    //m_BE.userUuid = m_BE.CreatedBy;//使用者uuid與創建者相同
                    m_BE.sn = i + 1;
                    m_BE.FlwType = (string)m_dataTable.Rows[i]["flw_type"];
                    switch (m_BE.FlwType)
                    {
                        case "A":
                            m_BE.FlwTypeName = "新增";
                            break;
                        case "M":
                            m_BE.FlwTypeName = "修改";
                            break;
                        case "D":
                            m_BE.FlwTypeName = "刪除";
                            break;
                        case "R":
                            m_BE.FlwTypeName = "密碼重送";
                            break;
                        case "U":
                            m_BE.FlwTypeName = "帳號解鎖";
                            break;
                        default:
                            m_BE.FlwTypeName = m_BE.FlwType;
                            break;
                    }
                    m_BE.FlwStatus = (string)m_dataTable.Rows[i]["flw_status"];
                    switch (m_BE.FlwStatus)
                    {
                        case "G":
                            m_BE.FlwStatusName = "待覆核";
                            break;
                        case "F":
                            m_BE.FlwStatusName = "已覆核";
                            break;
                        case "R":
                            m_BE.FlwStatusName = "退回";
                            break;
                        default:
                            m_BE.FlwStatusName = m_BE.FlwStatus;
                            break;
                    }
                    m_BE.function_entity = (string)m_dataTable.Rows[i]["func_entity"];
                    m_BE.orginal_data = m_dataTable.Rows[i]["orginal_data"].ToString();

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


        public IEnumerable<string[]> QueryRecProgram(Guid p_UserUuid)
        {
            List<string[]> m_list = new List<string[]>();

            //申請查詢FlowJob
            string m_sql = @"
SELECT DISTINCT P.func_id, P.func_name, P.super_uuid, P.seq_no
  FROM ZT_SysGroupProgram GP
  JOIN ZT_SysGroup G
    ON G.group_uuid = GP.group_uuid
  JOIN ZT_AaUser U
    ON U.role_id = G.group_id
  JOIN ZT_SysProgram P
    ON P.func_uuid = GP.func_uuid
   AND P.func_type = 'F'
   AND ISNULL(P.func_entity, '') <> ''
 WHERE 1 = 1
   AND U.user_uuid = @user_uuid
 ORDER BY P.super_uuid, P.seq_no ";

            DataTable m_dataTable = g_dba.GetDataTable(m_sql, new[] {new DBParameter("@user_uuid", p_UserUuid)});
            List<string> m_string;
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                m_string = new List<string>();
                m_string.Add(m_dataTable.Rows[i]["func_id"].ToString());
                m_string.Add(m_dataTable.Rows[i]["func_name"].ToString());
                m_list.Add(m_string.ToArray());
            }
            return m_list;
        }

        public IEnumerable<string[]> QueryRecStatus()
        {
            List<string[]> m_list = new List<string[]>();


            string m_sql = @"
SELECT distinct s.code_id, s.code_name, j.flw_status, s.seq
FROM ZT_SysCodeInfo s
FULL JOIN ZT_FlwJob j
ON s.code_id = j.flw_status
WHERE j.status_flag='Y' and cate='FLOW_STATUS'
ORDER BY s.seq";

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
        public FlowJobBE genFlowJobBE(DataRow p_row)
        {
            FlowJobBE m_be = new FlowJobBE();
            m_be.JobUuid = (Guid)p_row["job_uuid"];


            if (p_row["flw_uuid"] != DBNull.Value)
            {
                m_be.FlwUuid = (Guid)p_row["flw_uuid"];
            }
            if (p_row["user_info_uuid"] != DBNull.Value)
            {
                m_be.UserInfoUuid = (Guid)p_row["user_info_uuid"];
            }
            if (p_row["data_uuid"] != DBNull.Value)
            {
                m_be.DataUuid = (Guid)p_row["data_uuid"];
            }
            if (p_row["org_uuid"] != DBNull.Value)
            {
                m_be.OrgUuid = (Guid)p_row["org_uuid"];
            }
            if (p_row["system_id"] != DBNull.Value)
            {
                m_be.SystemId = p_row["system_id"].ToString().Trim();
            }
            if (p_row["function_id"] != DBNull.Value)
            {
                m_be.FunctionId = p_row["function_id"].ToString().Trim();
            }
            if (p_row["function_name"] != DBNull.Value)
            {
                m_be.FunctionName = p_row["function_name"].ToString().Trim();
            }
            if (p_row["service_name"] != DBNull.Value)
            {
                m_be.ServiceName = p_row["service_name"].ToString().Trim();
            }
            if (p_row["flw_status"] != DBNull.Value)
            {
                m_be.FlwStatus = p_row["flw_status"].ToString().Trim();
            }
            if (p_row["call_back"] != DBNull.Value)
            {
                m_be.CallBack = p_row["call_back"].ToString().Trim();
            }
            if (p_row["return_data"] != DBNull.Value)
            {
                m_be.ReturnData = p_row["return_data"].ToString().Trim();
            }
            if (p_row["flw_type"] != DBNull.Value)
            {
                m_be.FlwType = p_row["flw_type"].ToString().Trim();
            }
            if (p_row["status_flag"] != DBNull.Value)
            {
                m_be.StatusFlag = p_row["status_flag"].ToString().Trim();
            }
            if (p_row["created_by"] != DBNull.Value)
            {
                m_be.CreatedBy = (Guid)p_row["created_by"];
            }

            if (p_row["created_date"] != DBNull.Value)
            {
                m_be.CreatedDate = (DateTime)p_row["created_date"];
            }
            if (p_row["updated_by"] != DBNull.Value)
            {
                m_be.UpdatedBy = (Guid)p_row["updated_by"];
            }

            if (p_row["updated_date"] != DBNull.Value)
            {
                m_be.UpdatedDate = (DateTime)p_row["updated_date"];
            }

            //
            if (p_row["user_info_name"] != DBNull.Value)
            {
                m_be.UserInfoName = p_row["user_info_name"].ToString().Trim();
            }
            //m_be.function_entity = (string)p_row["func_entity"];
            //m_be.orginal_data = p_row["orginal_data"].ToString();

            return m_be;
        }

        #endregion

    }
}
