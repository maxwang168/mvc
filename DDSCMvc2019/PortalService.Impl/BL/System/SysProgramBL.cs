using CommonLibrary.DBA;
using Entity.SYS;
using log4net;
using PortalService.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;

namespace PortalService.Impl.BL
{
    public class SysProgramBL
    {
        #region 成員變數

        private ILog logger = LogManager.GetLogger(typeof(SysProgramBL));
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

        public IEnumerable<SysProgramBE> SysProgramQuery(SysProgramModel p_sysProgramVM)
        {
            List<SysProgramBE> m_result = new List<SysProgramBE>();
            List<DBParameter> m_paraList = new List<DBParameter>();
            string m_sql = @"SELECT p1.super_uuid as class1, p1.func_name as class2, 
                                    ISNULL(p2.func_name,' ') as pp_name,
                                    p.*, ISNULL(AaUser1.user_name, '') as created_by_name,ISNULL(AaUser2.user_name, '') as updated_by_name 
                            FROM ZT_SysProgram AS p 
                            INNER JOIN ZT_SysProgram AS p1 ON p.super_uuid = p1.func_uuid 
                            LEFT JOIN ZT_SysProgram AS p2 ON p1.super_uuid=p2.func_uuid 
                            LEFT JOIN ZT_AaUser AS AaUser1 ON p.created_by = AaUser1.user_uuid
                            LEFT JOIN ZT_AaUser AS AaUser2 ON p.updated_by = AaUser2.user_uuid
                            WHERE 1=1";
            try
            {

                if ((!string.IsNullOrEmpty(p_sysProgramVM.ProgramType)))
                {
                    m_sql += " AND p.func_type=@program_type";
                    m_paraList.Add(new DBParameter("@program_type", p_sysProgramVM.ProgramType));
                }

                if (!string.IsNullOrEmpty(p_sysProgramVM.ProgramID) || !string.IsNullOrEmpty(p_sysProgramVM.ProgramName))
                {
                    //如果程式代碼或名稱有輸入, 則不要判斷選單
                    if (!string.IsNullOrEmpty(p_sysProgramVM.ProgramID))
                    {
                        m_sql += " AND p.func_id=@program_id";
                        m_paraList.Add(new DBParameter("@program_id", p_sysProgramVM.ProgramID));
                    }
                    if (!string.IsNullOrEmpty(p_sysProgramVM.ProgramName))
                    {
                        m_sql += " AND p.func_name LIKE @program_name";
                        m_paraList.Add(new DBParameter("@program_name", "%" + p_sysProgramVM.ProgramName + "%"));
                    }
                }
                else if (!string.IsNullOrEmpty(p_sysProgramVM.Menu) && string.IsNullOrEmpty(p_sysProgramVM.SubMenu))
                {
                    //主選單有值, 子選單沒有值
                    m_sql += " AND (p.super_uuid=@Menu OR p1.super_uuid=@Menu OR p.func_uuid=@Menu)";
                    m_paraList.Add(new DBParameter("@Menu", p_sysProgramVM.Menu));
                }
                else if (!string.IsNullOrEmpty(p_sysProgramVM.SubMenu))
                {
                    m_sql += " AND p.super_uuid=@subMenu";
                    m_paraList.Add(new DBParameter("@subMenu", p_sysProgramVM.SubMenu));
                }
                m_sql += " ORDER BY p1.seq_no, p.seq_no ";

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    SysProgramBE m_program = genSysProgramBE(m_dataTable.Rows[i]);
                    m_result.Add(m_program);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        public bool insertSysProgramBE(SysProgramBE p_code)
        {
            bool m_success = false;
            string m_sql = @"INSERT INTO ZT_SysProgram (
                             func_uuid, func_type, super_uuid, func_id, func_name,
                             func_url, system_id, seq_no, flow_uuid, descriptions,
                             status_flag, created_by, created_date, updated_by, updated_date
                             ) VALUES (
                             @func_uuid, @func_type, @super_uuid, @func_id, @func_name,
                             @func_url, @system_id, @seq_no, @flow_uuid, @descriptions,
                             @status_flag, @created_by, @created_date, @updated_by, @updated_date)";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@func_uuid", p_code.func_uuid));
                m_paraList.Add(new DBParameter("@func_type", p_code.program_type));
                m_paraList.Add(new DBParameter("@super_uuid", p_code.parent_id));
                m_paraList.Add(new DBParameter("@func_id", p_code.program_id));
                m_paraList.Add(new DBParameter("@func_name", p_code.program_name));

                m_paraList.Add(new DBParameter("@func_url", p_code.program_url));
                m_paraList.Add(new DBParameter("@system_id", p_code.system_id));
                m_paraList.Add(new DBParameter("@seq_no", p_code.seq_no));
                m_paraList.Add(new DBParameter("@flow_uuid", DBNull.Value));
                m_paraList.Add(new DBParameter("@descriptions", p_code.program_desc));

                m_paraList.Add(new DBParameter("@status_flag", p_code.status_flag));
                m_paraList.Add(new DBParameter("@created_by", p_code.CREATE_USER_UUID));
                m_paraList.Add(new DBParameter("@created_date", p_code.CREATE_DATE));
                m_paraList.Add(new DBParameter("@updated_by", p_code.MODIFY_USER_UUID));
                m_paraList.Add(new DBParameter("@updated_date", p_code.MODIFY_DATE));

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, Guid.Parse(p_code.MODIFY_USER_UUID));
                m_success = false;
            }
            return m_success;
        }

        public bool updateSysProgramBE(SysProgramBE p_code)
        {
            bool m_success = false;
            string m_sql = @"UPDATE ZT_SysProgram SET             
                             func_type=@func_type, super_uuid=@super_uuid, func_id=@func_id, func_name=@func_name, func_url=@func_url, 
                             system_id=@system_id, seq_no=@seq_no, flow_uuid=@flow_uuid, descriptions=@descriptions, status_flag=@status_flag, 
                             updated_by=@updated_by, updated_date=@updated_date 
            WHERE func_uuid=@func_uuid";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@func_uuid", p_code.func_uuid));
                m_paraList.Add(new DBParameter("@func_type", p_code.program_type));
                m_paraList.Add(new DBParameter("@super_uuid", p_code.parent_id));
                m_paraList.Add(new DBParameter("@func_id", p_code.program_id));
                m_paraList.Add(new DBParameter("@func_name", p_code.program_name));

                m_paraList.Add(new DBParameter("@func_url", p_code.program_url));
                m_paraList.Add(new DBParameter("@system_id", p_code.system_id));
                m_paraList.Add(new DBParameter("@seq_no", p_code.seq_no));
                m_paraList.Add(new DBParameter("@flow_uuid", DBNull.Value));
                m_paraList.Add(new DBParameter("@descriptions", p_code.program_desc));

                m_paraList.Add(new DBParameter("@status_flag", p_code.status_flag));
                m_paraList.Add(new DBParameter("@updated_by", p_code.MODIFY_USER_UUID));
                m_paraList.Add(new DBParameter("@updated_date", p_code.MODIFY_DATE));

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, Guid.Parse(p_code.MODIFY_USER_UUID));
                m_success = false;
            }
            return m_success;
        }

        public bool deleteSysProgramBE(string p_funcUuid)
        {
            bool m_success = false;
            string m_sql = @"DELETE FROM ZT_SysProgram WHERE func_uuid=@func_uuid";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@func_uuid", p_funcUuid));
                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                m_success = false;
            }
            return m_success;
        }

        public IEnumerable<string[]> QueryProgram(Guid p_UserUuid, bool p_IsAllQueriable)
        {
            List<string[]> m_list = new List<string[]>();
            string m_sql;

            if (p_IsAllQueriable)
            {
                //使用者執行歷程(全部)SysUserLog-n
                m_sql = @"
SELECT DISTINCT P.func_id, P.func_name, P.super_uuid, P.seq_no
  FROM ZT_SysGroupProgram GP
  JOIN ZT_SysGroup G
    ON G.group_uuid = GP.group_uuid
  JOIN (SELECT A.code_id AS role_id
          FROM ZT_SysCodeInfo A
          JOIN (SELECT C.CATE
                  FROM ZT_AaUser U2
                  JOIN ZT_SysCodeInfo C
                    ON C.code_id = U2.role_id
                 WHERE U2.user_uuid = @user_uuid) B
            ON A.CATE = B.CATE) U
    ON U.role_id = G.group_id
  JOIN ZT_SysProgram P
    ON P.func_uuid = GP.func_uuid
   AND P.func_type = 'F'
 ORDER BY P.super_uuid, P.seq_no ";
            }
            else
            {
                //使用者執行歷程SysUserLog
                m_sql = @"
SELECT DISTINCT P.func_id, P.func_name, P.super_uuid, P.seq_no
  FROM ZT_SysGroupProgram GP
  JOIN ZT_SysGroup G
    ON G.group_uuid = GP.group_uuid
  JOIN ZT_AaUser U
    ON U.role_id = G.group_id
  JOIN ZT_SysProgram P
    ON P.func_uuid = GP.func_uuid
   AND P.func_type = 'F'
 WHERE 1 = 1
   AND U.user_uuid = @user_uuid
 ORDER BY P.super_uuid, P.seq_no ";
            }

            DataTable m_dataTable = g_dba.GetDataTable(m_sql, new[] {new DBParameter("@user_uuid", p_UserUuid)});

            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                string[] m_string = new string[2];
                m_string[0] = m_dataTable.Rows[i]["func_id"].ToString().Trim();
                m_string[1] = m_dataTable.Rows[i]["func_name"].ToString();
                m_list.Add(m_string);
            }
            return m_list;
        }

        public IEnumerable<string[]> QueryMenu(string p_type)
        {
            List<string[]> m_list = new List<string[]>();

            string m_sql = @"SELECT super_uuid, func_uuid, func_name, func_type
                            FROM ZT_SysProgram WHERE status_flag = 'Y' and  super_uuid is not null ";
            if (p_type.Trim() == "")
            {
                m_sql += " and func_type <> 'F' ";
            }
            else
            {
                m_sql += string.Format(" and FUNC_TYPE='' ", p_type);
            }
            m_sql += " ORDER BY super_uuid,seq_no ";

            DataTable m_dataTable = g_dba.GetDataTable(m_sql);
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                string[] m_string = new string[3];
                m_string[0] = m_dataTable.Rows[i]["func_uuid"].ToString().Trim();
                m_string[1] = m_dataTable.Rows[i]["func_name"].ToString();
                m_string[2] = m_dataTable.Rows[i]["func_type"].ToString();
                m_list.Add(m_string);
            }
            return m_list;
        }

        public IEnumerable<string[]> QuerySubMenu(string p_parent)
        {
            List<string[]> m_list = new List<string[]>();

            string m_sql = @"SELECT func_uuid,func_name 
                             FROM ZT_SysProgram WHERE func_type='M' AND super_uuid=@parent  ORDER BY seq_no";
            List<DBParameter> p_paraList = new List<DBParameter>();
            p_paraList.Add(new DBParameter("@parent", p_parent));
            DataTable m_dataTable = g_dba.GetDataTable(m_sql, p_paraList.ToArray());
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                string[] m_string = new string[2];
                m_string[0] = m_dataTable.Rows[i]["func_uuid"].ToString().Trim();
                m_string[1] = m_dataTable.Rows[i]["func_name"].ToString();
                m_list.Add(m_string);
            }
            return m_list;
        }

        public IEnumerable<string> GetProgramInfo(string p_funcID)
        {
            List<string> m_result = new List<string>();

            string m_sql = @"SELECT p.program_name as prg_name
                                , isnull(pp.program_name,'') as prg_name2
                                , isnull(ppp.program_name,'') as prg_name3 
                            FROM sys_program AS p left join sys_program AS pp on isnull(p.parent_id,'')=pp.program_id
                                left join sys_program AS ppp on isnull(pp.parent_id,'')=ppp.program_id
                            WHERE p.program_id = @program_id";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@program_id", p_funcID));

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                if (m_dataTable != null && m_dataTable.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(m_dataTable.Rows[0]["prg_name3"].ToString()))
                        m_result.Add(m_dataTable.Rows[0]["prg_name3"].ToString());
                    if (!string.IsNullOrEmpty(m_dataTable.Rows[0]["prg_name2"].ToString()))
                        m_result.Add(m_dataTable.Rows[0]["prg_name2"].ToString());
                    m_result.Add(m_dataTable.Rows[0]["prg_name"].ToString());
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

        #region Private Methods

        private SysProgramBE genSysProgramBE(DataRow p_row)
        {
            SysProgramBE m_program = new SysProgramBE();

            m_program.func_uuid = p_row["func_uuid"].ToString();
            m_program.program_id = p_row["func_id"].ToString();
            m_program.program_type = (string)p_row["func_type"];
            m_program.program_name = (string)p_row["func_name"];
            if (p_row["descriptions"] != DBNull.Value)
                m_program.program_desc = (string)p_row["descriptions"];
            m_program.parent_id = p_row["super_uuid"].ToString();
            m_program.seq_no = (int)p_row["seq_no"];
            if (p_row["func_url"] != DBNull.Value)
                m_program.program_url = (string)p_row["func_url"];
            m_program.CREATE_USER_UUID = p_row["CREATED_BY"].ToString();
            m_program.CREATE_USER_NAME = p_row["created_by_name"].ToString();
            m_program.CREATE_DATE = (DateTime)p_row["CREATED_DATE"];
            m_program.MODIFY_USER_UUID = p_row["UPDATED_BY"].ToString();
            m_program.MODIFY_USER_NAME = p_row["updated_by_name"].ToString();
            m_program.MODIFY_DATE = (DateTime)p_row["UPDATED_DATE"];
            m_program.Menu = p_row["class1"].ToString();
            m_program.SubMenu = p_row["super_uuid"].ToString();
            m_program.RootProgramName = (string)p_row["class2"];
            if (p_row["pp_name"] != DBNull.Value)
                m_program.GRootProgramName = (string)p_row["pp_name"];

            if (p_row["func_entity"] != DBNull.Value)
            {
                m_program.func_entity = (string)p_row["func_entity"];
            }
            if (p_row["table_name"] != DBNull.Value)
            {
                m_program.table_name = (string)p_row["table_name"];
            }
            if (p_row["key_name"] != DBNull.Value)
            {
                m_program.key_name = (string)p_row["key_name"];
            }
            if (p_row["func_class"] != DBNull.Value)
            {
                m_program.func_class = (string)p_row["func_class"];
            }

            return m_program;
        }

        #endregion
    }
}
