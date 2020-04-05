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
    public class SysRoleInfoBL
    {
        #region 成員變數

        private ILog logger = LogManager.GetLogger(typeof(SysRoleInfoBL));
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

        public IEnumerable<SysRoleInfoBE> SysRoleInfoQuery(SysRoleInfoModel p_sysRoleVM)
        {
            List<SysRoleInfoBE> m_result = new List<SysRoleInfoBE>();

            string m_sql = @"SELECT * FROM sys_role_info WHERE 1=1";
            if (!string.IsNullOrEmpty(p_sysRoleVM.RoleCode))
            {
                m_sql += " AND role_code=@role_code";
            }
            if (!string.IsNullOrEmpty(p_sysRoleVM.RoleName))
            {
                m_sql += " AND role_name Like @role_name";
            }
            if (!string.IsNullOrEmpty(p_sysRoleVM.RoleType))
            {
                m_sql += " AND role_type=@role_type";
            }
            if (!string.IsNullOrEmpty(p_sysRoleVM.RoleSubType))
            {
                m_sql += " AND role_subtype=@role_subtype";
            }
            m_sql += " ORDER BY role_code";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                if (!string.IsNullOrEmpty(p_sysRoleVM.RoleCode))
                {
                    m_paraList.Add(new DBParameter("@role_code", p_sysRoleVM.RoleCode));
                }
                if (!string.IsNullOrEmpty(p_sysRoleVM.RoleName))
                {
                    m_paraList.Add(new DBParameter("@role_name", "%" + p_sysRoleVM.RoleName + "%"));
                }
                if (!string.IsNullOrEmpty(p_sysRoleVM.RoleType))
                {
                    m_paraList.Add(new DBParameter("@role_type", p_sysRoleVM.RoleType));
                }
                if (!string.IsNullOrEmpty(p_sysRoleVM.RoleSubType))
                {
                    m_paraList.Add(new DBParameter("@role_subtype", p_sysRoleVM.RoleSubType));
                }
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    SysRoleInfoBE m_role = genSysRoleInfoBE(m_dataTable.Rows[i]);
                    m_result.Add(m_role);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        public SysRoleInfoBE genSysRoleInfoBE(DataRow p_row)
        {
            SysRoleInfoBE m_role = new SysRoleInfoBE();
            m_role.role_code = (string)p_row["role_code"];
            if (p_row["role_name"] != DBNull.Value)
            {
                m_role.role_name = (string)p_row["role_name"];
            }
            if (p_row["role_type"] != DBNull.Value)
            {
                m_role.role_type = (string)p_row["role_type"];
            }
            if (p_row["role_subtype"] != DBNull.Value)
            {
                m_role.role_subtype = (string)p_row["role_subtype"];
            }
            if (p_row["remark"] != DBNull.Value)
            {
                m_role.remark = (string)p_row["remark"];
            }
            m_role.CREATE_USER_UUID = (string)p_row["created_by"];
            m_role.CREATE_DATE = (DateTime)p_row["created_date"];
            m_role.MODIFY_USER_UUID = (string)p_row["updated_by"];
            m_role.MODIFY_DATE = (DateTime)p_row["updated_date"];

            return m_role;
        }

        public bool insertSysRoleInfo(SysRoleInfoBE p_role)
        {
            bool result = false;
            string m_sql = @"INSERT INTO sys_role_info (role_code,role_name,role_type,role_subtype,remark,created_by,created_date,updated_by,updated_date) 
VALUES (@role_code,@role_name,@role_type,@role_subtype,@remark,@created_by,@created_date,@updated_by,@updated_date)";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@role_code", p_role.role_code));
                m_paraList.Add(new DBParameter("@role_name", p_role.role_name));
                m_paraList.Add(new DBParameter("@role_type", p_role.role_type));
                m_paraList.Add(new DBParameter("@role_subtype", p_role.role_subtype));
                m_paraList.Add(new DBParameter("@remark", p_role.remark));
                m_paraList.Add(new DBParameter("@created_by", p_role.CREATE_USER_UUID));
                m_paraList.Add(new DBParameter("@created_date", p_role.CREATE_DATE));
                m_paraList.Add(new DBParameter("@updated_by", p_role.MODIFY_USER_UUID));
                m_paraList.Add(new DBParameter("@updated_date", p_role.MODIFY_DATE));

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
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, Guid.Parse(p_role.MODIFY_USER_UUID));
                result = false;
            }
            return result;
        }

        //eva
        public bool updateSysRoleInfo(SysRoleInfoBE p_role)
        {
            bool result = false;
            string m_sql = @"UPDATE sys_role_info SET role_name=@role_name,role_type=@role_type,role_subtype=@role_subtype,
remark=@remark,updated_by=@updated_by,updated_date=@updated_date WHERE role_code=@role_code";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@role_code", p_role.role_code));
                m_paraList.Add(new DBParameter("@role_name", p_role.role_name));
                m_paraList.Add(new DBParameter("@role_type", p_role.role_type));
                m_paraList.Add(new DBParameter("@role_subtype", p_role.role_subtype));
                m_paraList.Add(new DBParameter("@remark", p_role.remark));
                m_paraList.Add(new DBParameter("@updated_by", p_role.MODIFY_USER_UUID));
                m_paraList.Add(new DBParameter("@updated_date", p_role.MODIFY_DATE));

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
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, Guid.Parse(p_role.MODIFY_USER_UUID));
                result = false;
            }
            return result;
        }

        //eva
        public bool deleteSysRoleInfo(string p_roleCode)
        {
            bool result = false;
            string m_sql = @"DELETE FROM sys_role_info WHERE role_code=@role_code";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@role_code", p_roleCode));
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


        #endregion
    }
}
