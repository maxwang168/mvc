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
    public class SysGroupBL
    {
        #region 成員變數

        private ILog logger = LogManager.GetLogger(typeof(SysGroupProgramBL));
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
        public IEnumerable<SysGroupBE> SysGroupQuery(SysGroupModel p_sysGroupVM)
        {
            List<SysGroupBE> m_result = new List<SysGroupBE>();

            string m_sql = @"SELECT g.*, o.org_uuid, o.org_id, o.org_name, c2.code_uuid AS role_uuid, c2.code_id AS role_id, c2.code_name AS role_name,
ISNULL(u1.[user_name],g.created_by) AS created_by_name, ISNULL(u1.[user_name],g.updated_by) AS updated_by_name
FROM dbo.ZT_SysGroup g
INNER JOIN dbo.ZT_AaOrg o ON g.org_id=o.org_id
INNER JOIN dbo.ZT_SysCodeInfo c ON c.cate='ORG_PARM'
INNER JOIN dbo.ZT_SysCodeInfo c2 ON g.group_id=c2.code_id AND c.code_id=c2.cate
LEFT JOIN dbo.ZT_AaUser u1 ON g.created_by=u1.user_uuid
LEFT JOIN dbo.ZT_AaUser u2 ON g.updated_by=u2.user_uuid
WHERE 1=1 ";
            if (!string.IsNullOrEmpty(p_sysGroupVM.GroupId))
            {
                m_sql += " AND g.group_id=@group_id";
            }
            //非管理群組
            if (!p_sysGroupVM.IsAdminGroup)
            {
                m_sql += " AND c.var_char02<>'DEFAULT' ";
            }
            //非 ADMIN 角色
            if (!p_sysGroupVM.IsAdmin)
            {
                m_sql += " AND c.var_char02<>'ADMIN' ";
            }
            m_sql += " ORDER BY g.org_id, g.group_id";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                if (!string.IsNullOrEmpty(p_sysGroupVM.GroupId))
                {
                    m_paraList.Add(new DBParameter("@group_id", p_sysGroupVM.GroupId));
                }
                if (!string.IsNullOrEmpty(p_sysGroupVM.GroupName))
                {
                    m_paraList.Add(new DBParameter("@group_name", "%" + p_sysGroupVM.GroupName + "%"));
                }
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    SysGroupBE m_be = genSysGroupBE(m_dataTable.Rows[i]);
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

        public bool insertSysGroupBE(SysGroupBE p_be)
        {
            bool result = false;
            string m_sql = @"INSERT INTO [dbo].[ZT_SysGroup] ([group_uuid],[system_id],[org_id],[group_id],[group_name],[admin_group],[status_flag],[created_by],[created_date],[updated_by],[updated_date])
VALUES (@group_uuid,@system_id,@org_id,@group_id,@group_name,@admin_group,@status_flag,@created_by,@created_date,@updated_by,@updated_date) ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@group_uuid", p_be.GroupUuid));
                m_paraList.Add(new DBParameter("@system_id", p_be.SystemId));
                m_paraList.Add(new DBParameter("@org_id", p_be.OrgId));
                m_paraList.Add(new DBParameter("@group_id", p_be.GroupId));
                m_paraList.Add(new DBParameter("@group_name", p_be.GroupName));
                m_paraList.Add(new DBParameter("@admin_group", p_be.AdminGroup ? "Y" : "N"));
                m_paraList.Add(new DBParameter("@status_flag", p_be.StatusFlag));
                m_paraList.Add(new DBParameter("@created_by", p_be.CreatedBy));
                m_paraList.Add(new DBParameter("@created_date", p_be.CreatedDate));
                m_paraList.Add(new DBParameter("@updated_by", p_be.UpdatedBy));
                m_paraList.Add(new DBParameter("@updated_date", p_be.UpdatedDate));

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
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_be.UpdatedBy);
                result = false;
            }
            return result;
        }

        public bool updateSysGroupBE(SysGroupBE p_be)
        {
            bool result = false;
            string m_sql = @"UPDATE [dbo].[ZT_SysGroup] SET group_id=@group_id, group_name=@group_name, admin_group=@admin_group
, updated_by=@updated_by, updated_date=@updated_date WHERE group_uuid=@group_uuid";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@group_uuid", p_be.GroupUuid));
                m_paraList.Add(new DBParameter("@group_id", p_be.GroupId));
                m_paraList.Add(new DBParameter("@group_name", p_be.GroupName));
                m_paraList.Add(new DBParameter("@admin_group", p_be.AdminGroup ? "Y" : "N"));
                m_paraList.Add(new DBParameter("@updated_by", p_be.UpdatedBy));
                m_paraList.Add(new DBParameter("@updated_date", p_be.UpdatedDate));

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
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_be.UpdatedBy);
                result = false;
            }
            return result;
        }

        public bool deleteSysGroupBE(Guid p_groupUuid)
        {
            bool result = false;
            string m_sql = @"DELETE [dbo].[ZT_SysGroup] WHERE group_uuid=@group_uuid;
DELETE [dbo].[ZT_SysGroupProgram] WHERE group_uuid=@group_uuid ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@group_uuid", p_groupUuid));

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
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                result = false;
            }
            return result;
        }

        public int SysGroupProgramQryCnt(string p_groupId, Guid p_groupUuid) //, string p_orgId, Guid p_groupUuid)
        {
            int m_result = -1;
            try
            {
                string m_sql = @"SELECT COUNT(*) AS cnt FROM [dbo].[ZT_SysGroup] WHERE status_flag='Y' AND group_id=@group_id "; // AND org_id=@org_id AND group_id=@group_id ";

                List<DBParameter> m_paraList = new List<DBParameter>();
                //m_paraList.Add(new DBParameter("@org_id", p_orgId));
                m_paraList.Add(new DBParameter("@group_id", p_groupId));

                if (p_groupUuid != Guid.Empty)
                {
                    m_sql += " AND group_uuid<>@group_uuid ";
                    m_paraList.Add(new DBParameter("@group_uuid", p_groupUuid));
                }

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                if (m_dataTable != null && m_dataTable.Rows.Count >= 1)
                {
                    m_result = Convert.ToInt32(m_dataTable.Rows[0]["cnt"]);
                }
            }
            catch (Exception ex)
            {
                m_result = -1;
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        /// <summary>
        /// 查詢角色
        /// </summary>
        /// <param name="p_isAdminGroup">true:管理者群組, false:非管理者群組</param>
        /// <param name="p_isAdmin">true:登入者角色為 ADMIN, false:登入者角色非 ADMIN</param>
        /// <returns></returns>
        public IEnumerable<string[]> QueryGroupName(bool p_isAdminGroup, bool p_isAdmin)
        {
            List<string[]> m_list = new List<string[]>();
            string m_sql = @"SELECT group_id, group_name FROM ZT_SysGroup g
INNER JOIN ZT_SysCodeInfo c ON c.cate='ORG_PARM'
INNER JOIN ZT_SysCodeInfo c2 ON g.group_id=c2.code_id AND c.code_id=c2.cate
WHERE g.status_flag='Y' ";

            //非管理群組
            if (!p_isAdminGroup)
            {
                m_sql += " AND c.var_char02<>'DEFAULT' ";
            }

            //非 ADMIN 角色
            if (!p_isAdmin)
            {
                m_sql += " AND c.var_char02<>'ADMIN' ";
            }

            m_sql += " ORDER BY c.var_char02, g.group_id ";
            DataTable m_dataTable = g_dba.GetDataTable(m_sql);
            List<string> m_string;
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                m_string = new List<string>();
                m_string.Add(m_dataTable.Rows[i]["group_id"].ToString());
                m_string.Add(m_dataTable.Rows[i]["group_name"].ToString());
                m_list.Add(m_string.ToArray());
            }
            return m_list;
        }

        public SysGroupBE genSysGroupBE(DataRow p_row)
        {
            SysGroupBE m_be = new SysGroupBE();
            m_be.GroupUuid = (Guid)p_row["group_uuid"];
            if (p_row["system_id"] != DBNull.Value)
            {
                m_be.SystemId = p_row["system_id"].ToString().Trim();
            }
            if (p_row["org_id"] != DBNull.Value)
            {
                m_be.OrgId = p_row["org_id"].ToString().Trim();
            }
            if (p_row["group_id"] != DBNull.Value)
            {
                m_be.GroupId = p_row["group_id"].ToString().Trim();
            }
            if (p_row["group_name"] != DBNull.Value)
            {
                m_be.GroupName = p_row["group_name"].ToString().Trim();
            }
            if (p_row["admin_group"] != DBNull.Value)
            {
                m_be.AdminGroup = p_row["admin_group"].ToString() == "Y" ? true : false;
            }
            if (p_row["status_flag"] != DBNull.Value)
            {
                m_be.StatusFlag = p_row["status_flag"].ToString().Trim();
            }
            if (p_row["created_by"] != DBNull.Value)
            {
                m_be.CreatedBy = (Guid)p_row["created_by"];
            }
            if (p_row["created_by_name"] != DBNull.Value)
            {
                m_be.CreatedByName = p_row["created_by_name"].ToString().Trim();
            }
            if (p_row["created_date"] != DBNull.Value)
            {
                m_be.CreatedDate = (DateTime)p_row["created_date"];
            }
            if (p_row["updated_by"] != DBNull.Value)
            {
                m_be.UpdatedBy = (Guid)p_row["updated_by"];
            }
            if (p_row["updated_by_name"] != DBNull.Value)
            {
                m_be.UpdatedByName = p_row["updated_by_name"].ToString().Trim();
            }
            if (p_row["updated_date"] != DBNull.Value)
            {
                m_be.UpdatedDate = (DateTime)p_row["updated_date"];
            }
            if (p_row["org_uuid"] != DBNull.Value)
            {
                m_be.OrgUuid = (Guid)p_row["org_uuid"];
            }
            if (p_row["org_name"] != DBNull.Value)
            {
                m_be.OrgName = p_row["org_name"].ToString().Trim();
            }

            m_be.OrgIdName = m_be.OrgId + "-" + m_be.OrgName;

            if (p_row["role_uuid"] != DBNull.Value)
            {
                m_be.RoleUuid = (Guid)p_row["role_uuid"];
            }
            m_be.GroupIdName = m_be.GroupId + "-" + m_be.GroupName;

            return m_be;
        }

        #endregion

    }
}
