using CommonLibrary.DBA;
using Entity.SYS;
using log4net;
using PortalService.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Text;

namespace PortalService.Impl.BL
{
    public class SysCodeInfoBL
    {
        #region 成員變數

        private ILog logger = LogManager.GetLogger(typeof(SysCodeInfoBL));
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

        public IEnumerable<SysCodeInfoBE> SysCodeInfoQuery(SysCodeInfoModel p_sysCodeVM)
        {
            List<SysCodeInfoBE> m_result = new List<SysCodeInfoBE>();

            string m_sql = @"SELECT 
ISNULL(s2.cate, '') as group_id, ISNULL(s2.code_name, '') as group_name, s2.code_uuid as group_uuid,
s1.cate as sub_group_id, s1.code_name as sub_group_name, s1.code_uuid as sub_uuid,
S.*, ISNULL(AaUser1.user_name, '') as created_by_name,ISNULL(AaUser2.user_name, '') as updated_by_name 
FROM ZT_SysCodeInfo AS s
LEFT JOIN ZT_SysCodeInfo AS s1 ON s.super_uuid = s1.code_uuid
LEFT JOIN ZT_SysCodeInfo AS s2 ON s1.super_uuid = s2.code_uuid
LEFT JOIN ZT_AaUser AS AaUser1 ON s.created_by = AaUser1.user_uuid
LEFT JOIN ZT_AaUser AS AaUser2 ON s.updated_by = AaUser2.user_uuid
where s.super_uuid <> '00000000-0000-0000-0000-000000000000' ";

            //選擇群組
            if (!string.IsNullOrEmpty(p_sysCodeVM.Group) && string.IsNullOrEmpty(p_sysCodeVM.SubGroup))
            {
                m_sql += @" AND (s.code_uuid =@mainGroup or
(s.super_uuid in (select code_uuid from ZT_SysCodeInfo where super_uuid=@mainGroup))
or s.super_uuid in (@mainGroup)) ";
            }

            //選群組+子群組
            if (!string.IsNullOrEmpty(p_sysCodeVM.Group) && !string.IsNullOrEmpty(p_sysCodeVM.SubGroup))
            {
                m_sql += " AND (s.code_uuid=@subGroup or s.super_uuid=@subGroup) ";
            }

            if (!string.IsNullOrEmpty(p_sysCodeVM.Code))
            {
                m_sql += " AND s.code_id LiKE @code_id";
            }
            if (!string.IsNullOrEmpty(p_sysCodeVM.CodeName))
            {
                m_sql += " AND s.code_name LIKE @code_name";
            }
            //m_sql += " ORDER BY group_id,sub_group_id,s.seq ";
            m_sql += " ORDER BY s2.seq, s1.seq, s.seq ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                if (!string.IsNullOrEmpty(p_sysCodeVM.Group))
                {
                    m_paraList.Add(new DBParameter("@mainGroup", p_sysCodeVM.Group));
                }
                if (!string.IsNullOrEmpty(p_sysCodeVM.SubGroup))
                {
                    m_paraList.Add(new DBParameter("@subGroup", p_sysCodeVM.SubGroup));
                }
                if (!string.IsNullOrEmpty(p_sysCodeVM.Code))
                {
                    m_paraList.Add(new DBParameter("@code_id", "%" + p_sysCodeVM.Code + "%"));
                }
                if (!string.IsNullOrEmpty(p_sysCodeVM.CodeName))
                {
                    m_paraList.Add(new DBParameter("@code_name", "%" + p_sysCodeVM.CodeName + "%"));
                }
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    SysCodeInfoBE m_code = genSysCodeInfoBE(m_dataTable.Rows[i]);
                    m_result.Add(m_code);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        public SysCodeInfoBE QuerySysCodeInfo(string p_uuid)
        {
            SysCodeInfoBE m_result = null;
            string m_sql = @"SELECT s2.cate as class1, s2.code_name as class2, s.*, ISNULL(AaUser1.user_name, '') as created_by_name,ISNULL(AaUser2.user_name, '') as updated_by_name 
FROM ZT_SysCodeInfo AS s
INNER JOIN ZT_SysCodeInfo AS s2 ON s.cate = s2.code_id
LEFT JOIN ZT_AaUser AS AaUser1 ON s.created_by = AaUser1.user_uuid
LEFT JOIN ZT_AaUser AS AaUser2 ON s.updated_by = AaUser2.user_uuid
WHERE s.code_uuid = @code_uuid 
ORDER BY s.seq";

            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@code_uuid", p_uuid));
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                if (m_dataTable.Rows.Count > 0)
                {
                    m_result = genSysCodeInfoBE(m_dataTable.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        public SysCodeInfoBE QueryByCateCodeId(string p_codeId, string p_cate)
        {
            SysCodeInfoBE m_result = new SysCodeInfoBE();
            string m_sql = @"SELECT *, created_by as created_by_name, updated_by as updated_by_name
FROM ZT_SysCodeInfo WHERE code_id=@code_id AND cate=@cate";

            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@code_id", p_codeId));
                m_paraList.Add(new DBParameter("@cate", p_cate));
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                if (m_dataTable.Rows.Count > 0)
                {
                    m_result.CodeUuid = new Guid(m_dataTable.Rows[0]["code_uuid"].ToString());
                    m_result.CodeId = (string)m_dataTable.Rows[0]["code_id"];
                    m_result.CodeName = (string)m_dataTable.Rows[0]["code_name"];
                    m_result.Cate = (string)m_dataTable.Rows[0]["cate"];
                    m_result.SuperUuid = new Guid(m_dataTable.Rows[0]["super_uuid"].ToString());
                    m_result.Seq = (int)m_dataTable.Rows[0]["seq"];
                    if (m_dataTable.Rows[0]["code_name_p"] != DBNull.Value)
                    {
                        m_result.CodeNameP = (string)m_dataTable.Rows[0]["code_name_p"];
                    }
                    if (m_dataTable.Rows[0]["code_sname_p"] != DBNull.Value)
                    {
                        m_result.CodeSNameP = (string)m_dataTable.Rows[0]["code_sname_p"];
                    }
                    if (m_dataTable.Rows[0]["code_name_s"] != DBNull.Value)
                    {
                        m_result.CodeNameS = (string)m_dataTable.Rows[0]["code_name_s"];
                    }
                    if (m_dataTable.Rows[0]["code_sname_s"] != DBNull.Value)
                    {
                        m_result.CodeSNameS = (string)m_dataTable.Rows[0]["code_sname_s"];
                    }
                    m_result.ModifyStatus = (string)m_dataTable.Rows[0]["modify_status"];
                    m_result.StatusFlag = (string)m_dataTable.Rows[0]["status_flag"];
                    if (m_dataTable.Rows[0]["description"] != DBNull.Value)
                    {
                        m_result.Description = (string)m_dataTable.Rows[0]["description"];
                    }
                    if (m_dataTable.Rows[0]["var_char01"] != DBNull.Value)
                    {
                        m_result.VarChar01 = (string)m_dataTable.Rows[0]["var_char01"];
                        m_result.OrgVarChar01 = m_result.VarChar01;
                    }
                    if (m_dataTable.Rows[0]["var_char02"] != DBNull.Value)
                    {
                        m_result.VarChar02 = (string)m_dataTable.Rows[0]["var_char02"];
                        m_result.OrgVarChar02 = m_result.VarChar02;
                    }
                    if (m_dataTable.Rows[0]["var_char03"] != DBNull.Value)
                    {
                        m_result.VarChar03 = (string)m_dataTable.Rows[0]["var_char03"];
                        m_result.OrgVarChar03 = m_result.VarChar03;
                    }
                    if (m_dataTable.Rows[0]["var_char04"] != DBNull.Value)
                    {
                        m_result.VarChar04 = (string)m_dataTable.Rows[0]["var_char04"];
                        m_result.OrgVarChar04 = m_result.VarChar04;
                    }
                    if (m_dataTable.Rows[0]["var_char05"] != DBNull.Value)
                    {
                        m_result.VarChar05 = (string)m_dataTable.Rows[0]["var_char05"];
                        m_result.OrgVarChar05 = m_result.VarChar05;
                    }
                    if (m_dataTable.Rows[0]["var_char06"] != DBNull.Value)
                    {
                        m_result.VarChar06 = (string)m_dataTable.Rows[0]["var_char06"];
                        m_result.OrgVarChar06 = m_result.VarChar06;
                    }
                    if (m_dataTable.Rows[0]["var_char07"] != DBNull.Value)
                    {
                        m_result.VarChar07 = (string)m_dataTable.Rows[0]["var_char07"];
                        m_result.OrgVarChar07 = m_result.VarChar07;
                    }
                    if (m_dataTable.Rows[0]["var_char08"] != DBNull.Value)
                    {
                        m_result.VarChar08 = (string)m_dataTable.Rows[0]["var_char08"];
                        m_result.OrgVarChar08 = m_result.VarChar08;
                    }
                    if (m_dataTable.Rows[0]["var_char09"] != DBNull.Value)
                    {
                        m_result.VarChar09 = (string)m_dataTable.Rows[0]["var_char09"];
                        m_result.OrgVarChar09 = m_result.VarChar09;
                    }
                    if (m_dataTable.Rows[0]["var_char10"] != DBNull.Value)
                    {
                        m_result.VarChar10 = (string)m_dataTable.Rows[0]["var_char10"];
                        m_result.OrgVarChar10 = m_result.VarChar10;
                    }
                    m_result.CreatedBy = new Guid(m_dataTable.Rows[0]["created_by"].ToString());
                    m_result.CreatedByName = m_dataTable.Rows[0]["created_by_name"].ToString();
                    m_result.CreatedDate = (DateTime)m_dataTable.Rows[0]["created_date"];
                    m_result.UpdatedBy = new Guid(m_dataTable.Rows[0]["updated_by"].ToString());
                    m_result.UpdatedByName = m_dataTable.Rows[0]["updated_by_name"].ToString();
                    m_result.UpdatedDate = (DateTime)m_dataTable.Rows[0]["updated_date"];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        public IEnumerable<string[]> QueryByCate(string p_cate)
        {
            List<string[]> m_list = new List<string[]>();

            string m_sql = @"SELECT code_id, code_name, code_uuid, var_char01  
							FROM ZT_SysCodeInfo 
							WHERE status_flag='Y' AND cate=@cate ";

            m_sql += " ORDER BY seq ";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@cate", p_cate));

            DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                string[] m_string = new string[4];
                m_string[0] = m_dataTable.Rows[i]["code_id"].ToString().Trim();
                m_string[1] = m_dataTable.Rows[i]["code_name"].ToString();
                m_string[2] = m_dataTable.Rows[i]["code_uuid"].ToString();
                m_string[3] = m_dataTable.Rows[i]["var_char01"].ToString();
                m_list.Add(m_string);
            }
            return m_list;
        }


        public IEnumerable<SysRoleInfoBE> QueryRoleCodeName()
        {
            List<SysRoleInfoBE> m_result = new List<SysRoleInfoBE>();
            string m_sql = "SELECT * FROM ZT_SysGroup ORDER BY group_id";
            DataTable m_dataTable = g_dba.GetDataTable(m_sql);
            SysRoleInfoBL m_BL = new SysRoleInfoBL();
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                SysRoleInfoBE m_roleInfo = m_BL.genSysRoleInfoBE(m_dataTable.Rows[i]);
                m_result.Add(m_roleInfo);
            }
            return m_result;
        }

        public bool insertSysCodeInfoBE(SysCodeInfoBE p_code)
        {
            bool m_success = false;
            string m_sql = @"INSERT INTO ZT_SysCodeInfo 
(code_uuid,code_id,code_name,cate,super_uuid,seq,code_name_p,code_sname_p,code_name_s,code_sname_s,
modify_status,status_flag,description,var_char01,var_char02,var_char03,var_char04,var_char05,var_char06,var_char07,
var_char08,var_char09,var_char10,created_by,created_date,updated_by,updated_Date) VALUES 
(@code_uuid,@code_id,@code_name,@cate,@super_uuid,@seq,@code_name_p,@code_sname_p,@code_name_s,@code_sname_s,
@modify_status,@status_flag,@description,@var_char01,@var_char02,@var_char03,@var_char04,@var_char05,@var_char06,@var_char07,
@var_char08,@var_char09,@var_char10,@created_by,@created_date,@updated_by,@updated_date)";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@code_uuid", p_code.CodeUuid));
                m_paraList.Add(new DBParameter("@code_id", p_code.CodeId));
                m_paraList.Add(new DBParameter("@code_name", p_code.CodeName));
                m_paraList.Add(new DBParameter("@cate", p_code.Cate));
                m_paraList.Add(new DBParameter("@super_uuid", p_code.SuperUuid));
                m_paraList.Add(new DBParameter("@seq", p_code.Seq));
                m_paraList.Add(new DBParameter("@code_name_p", p_code.CodeNameP));
                m_paraList.Add(new DBParameter("@code_sname_p", p_code.CodeSNameP));
                m_paraList.Add(new DBParameter("@code_name_s", p_code.CodeNameS));
                m_paraList.Add(new DBParameter("@code_sname_s", p_code.CodeSNameS));
                m_paraList.Add(new DBParameter("@modify_status", p_code.ModifyStatus));
                m_paraList.Add(new DBParameter("@status_flag", p_code.StatusFlag));
                m_paraList.Add(new DBParameter("@description", p_code.Description));
                m_paraList.Add(new DBParameter("@var_char01", p_code.VarChar01));
                m_paraList.Add(new DBParameter("@var_char02", p_code.VarChar02));
                m_paraList.Add(new DBParameter("@var_char03", p_code.VarChar03));
                m_paraList.Add(new DBParameter("@var_char04", p_code.VarChar04));
                m_paraList.Add(new DBParameter("@var_char05", p_code.VarChar05));
                m_paraList.Add(new DBParameter("@var_char06", p_code.VarChar06));
                m_paraList.Add(new DBParameter("@var_char07", p_code.VarChar07));
                m_paraList.Add(new DBParameter("@var_char08", p_code.VarChar08));
                m_paraList.Add(new DBParameter("@var_char09", p_code.VarChar09));
                m_paraList.Add(new DBParameter("@var_char10", p_code.VarChar10));
                m_paraList.Add(new DBParameter("@created_by", p_code.CreatedBy));
                m_paraList.Add(new DBParameter("@created_date", p_code.CreatedDate));
                m_paraList.Add(new DBParameter("@updated_by", p_code.UpdatedBy));
                m_paraList.Add(new DBParameter("@updated_date", p_code.UpdatedDate));

                foreach (DBParameter obj in m_paraList)
                {
                    if (obj.Value == null)
                    {
                        obj.Value = "";
                    }
                }

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_code.UpdatedBy);
                m_success = false;
            }
            return m_success;
        }

        public bool updateSysCodeInfoBE(SysCodeInfoBE p_code)
        {
            bool m_success = false;
            string m_sql = @"UPDATE ZT_SysCodeInfo SET 
code_id=@code_id,code_name=@code_name,seq=@seq,code_name_p=@code_name_p,code_sname_p=@code_sname_p,code_name_s=@code_name_s,
code_sname_s=@code_sname_s,modify_status=@modify_status,status_flag=@status_flag,description=@description,var_char01=@var_char01,
var_char02=@var_char02,var_char03=@var_char03,var_char04=@var_char04,var_char05=@var_char05,var_char06=@var_char06,
var_char07=@var_char07,var_char08=@var_char08,var_char09=@var_char09,var_char10=@var_char10,updated_by=@updated_by,
updated_date=@updated_date WHERE code_uuid=@code_uuid";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@code_id", p_code.CodeId));
                m_paraList.Add(new DBParameter("@code_name", p_code.CodeName));
                m_paraList.Add(new DBParameter("@seq", p_code.Seq));
                m_paraList.Add(new DBParameter("@code_name_p", p_code.CodeNameP));
                m_paraList.Add(new DBParameter("@code_sname_p", p_code.CodeSNameP));
                m_paraList.Add(new DBParameter("@code_name_s", p_code.CodeNameS));
                m_paraList.Add(new DBParameter("@code_sname_s", p_code.CodeSNameS));
                m_paraList.Add(new DBParameter("@modify_status", p_code.ModifyStatus));
                m_paraList.Add(new DBParameter("@status_flag", p_code.StatusFlag));
                m_paraList.Add(new DBParameter("@description", p_code.Description));
                m_paraList.Add(new DBParameter("@var_char01", p_code.VarChar01));
                m_paraList.Add(new DBParameter("@var_char02", p_code.VarChar02));
                m_paraList.Add(new DBParameter("@var_char03", p_code.VarChar03));
                m_paraList.Add(new DBParameter("@var_char04", p_code.VarChar04));
                m_paraList.Add(new DBParameter("@var_char05", p_code.VarChar05));
                m_paraList.Add(new DBParameter("@var_char06", p_code.VarChar06));
                m_paraList.Add(new DBParameter("@var_char07", p_code.VarChar07));
                m_paraList.Add(new DBParameter("@var_char08", p_code.VarChar08));
                m_paraList.Add(new DBParameter("@var_char09", p_code.VarChar09));
                m_paraList.Add(new DBParameter("@var_char10", p_code.VarChar10));
                m_paraList.Add(new DBParameter("@updated_by", p_code.UpdatedBy));
                m_paraList.Add(new DBParameter("@updated_date", p_code.UpdatedDate));
                m_paraList.Add(new DBParameter("@code_uuid", p_code.CodeUuid));

                foreach (DBParameter obj in m_paraList)
                {
                    if (obj.Value == null)
                    {
                        obj.Value = "";
                    }
                }

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_code.UpdatedBy);
                m_success = false;
            }
            return m_success;
        }

        public bool deleteSysCodeInfoBE(string p_codeType, string p_codeUuid)
        {
            bool m_success = false;
            string m_sql = string.Empty;

            switch (p_codeType)
            {
                case "R":
                    m_sql = @"DELETE FROM ZT_SysCodeInfo WHERE (super_uuid IN (SELECT code_uuid FROM ZT_SysCodeInfo WHERE super_uuid=@code_uuid))
OR (super_uuid=@code_uuid) OR (code_uuid=@code_uuid)";
                    break;

                case "S":
                    m_sql = @"DELETE FROM ZT_SysCodeInfo WHERE (super_uuid=@code_uuid)
OR (code_uuid=@code_uuid)";
                    break;

                case "P":
                    m_sql = @"DELETE FROM ZT_SysCodeInfo WHERE code_uuid=@code_uuid ";
                    break;
            }

            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@code_uuid", p_codeUuid));
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

        //public IEnumerable<string, string> QueryFirstLevel()
        public IEnumerable<string[]> QueryFirstLevel()
        {
            //List<string> m_list = new List<string>();
            List<string[]> m_list = new List<string[]>();
            //string m_sql = "SELECT DISTINCT cate FROM sys_code_info WHERE super_uuid = '00000000-0000-0000-0000-000000000000' ORDER BY cate";
            StringBuilder m_sql = new StringBuilder();
            m_sql.AppendLine("SELECT                                                  ");
            m_sql.AppendLine("  CODE_UUID                                             ");
            m_sql.AppendLine(" ,CODE_ID                                               ");
            m_sql.AppendLine(" ,CODE_NAME                                             ");
            m_sql.AppendLine(" ,CATE                                                  ");
            m_sql.AppendLine(" ,SUPER_UUID                                            ");
            m_sql.AppendLine("FROM                                                    ");
            m_sql.AppendLine("  ZT_SysCodeInfo                                         ");
            m_sql.AppendLine("WHERE                                                   ");
            m_sql.AppendLine("  super_uuid = '00000000-0000-0000-0000-000000000001'   ");
            m_sql.AppendLine("ORDER BY                                                ");
            m_sql.AppendLine("  seq                                                  ");
            DataTable m_dataTable = g_dba.GetDataTable(m_sql.ToString());
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                string[] m_string = new string[4];
                m_string[0] = (string)m_dataTable.Rows[i]["CODE_NAME"];
                m_string[1] = m_dataTable.Rows[i]["CODE_ID"].ToString();
                m_string[2] = m_dataTable.Rows[i]["CODE_UUID"].ToString();
                m_string[3] = m_dataTable.Rows[i]["SUPER_UUID"].ToString();
                m_list.Add(m_string);
            }
            return m_list;
        }

        public IEnumerable<string[]> QueryFirstLevelForUser()
        {
            //List<string> m_list = new List<string>();
            List<string[]> m_list = new List<string[]>();
            //string m_sql = "SELECT DISTINCT cate FROM sys_code_info WHERE super_uuid = '00000000-0000-0000-0000-000000000000' ORDER BY cate";
            StringBuilder m_sql = new StringBuilder();
            m_sql.AppendLine("SELECT                                                  ");
            m_sql.AppendLine("  CODE_UUID                                             ");
            m_sql.AppendLine(" ,CODE_ID                                               ");
            m_sql.AppendLine(" ,CODE_NAME                                             ");
            m_sql.AppendLine(" ,CATE                                                  ");
            m_sql.AppendLine(" ,SUPER_UUID                                            ");
            m_sql.AppendLine("FROM                                                    ");
            m_sql.AppendLine("  ZT_SysCodeInfo                                         ");
            m_sql.AppendLine("WHERE                                                   ");
            m_sql.AppendLine("  super_uuid = '00000000-0000-0000-0000-000000000001'   ");
            m_sql.AppendLine("  AND code_id='USR_PARM'   ");
            m_sql.AppendLine("ORDER BY                                                ");
            m_sql.AppendLine("  seq                                                  ");
            DataTable m_dataTable = g_dba.GetDataTable(m_sql.ToString());
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                string[] m_string = new string[4];
                m_string[0] = (string)m_dataTable.Rows[i]["CODE_NAME"];
                m_string[1] = m_dataTable.Rows[i]["CODE_ID"].ToString();
                m_string[2] = m_dataTable.Rows[i]["CODE_UUID"].ToString();
                m_string[3] = m_dataTable.Rows[i]["SUPER_UUID"].ToString();
                m_list.Add(m_string);
            }
            return m_list;
        }

        //public IEnumerable<string[]> QuerySecondLevel(string p_code_name, string p_code_uuid)
        public IEnumerable<string[]> QuerySecondLevel(string p_super_uuid)
        {
            List<string[]> m_list = new List<string[]>();
            //string m_sql = "SELECT DISTINCT code_uuid, code_id, code_name FROM sys_code_info WHERE cate=@cate ORDER BY code_name";
            StringBuilder m_sql = new StringBuilder();
            m_sql.AppendLine(@" SELECT                                                                          ");
            m_sql.AppendLine(@"  S.CODE_UUID                                                                    ");
            m_sql.AppendLine(@" ,S.CODE_ID                                                                      ");
            m_sql.AppendLine(@" ,S.CODE_NAME                                                                    ");
            m_sql.AppendLine(@"FROM                                                                             ");
            m_sql.AppendLine(@"  ZT_SysCodeInfo AS S                                                            ");
            m_sql.AppendLine(@"WHERE                                                                            ");
            m_sql.AppendLine(@"  SUPER_UUID =@SUPER_UUID                     ");
            m_sql.AppendLine("ORDER BY                                                ");
            m_sql.AppendLine("  S.seq                                                 ");
            List<DBParameter> p_paraList = new List<DBParameter>();
            p_paraList.Add(new DBParameter("@SUPER_UUID", new Guid(p_super_uuid)));
            DataTable m_dataTable = g_dba.GetDataTable(m_sql.ToString(), p_paraList.ToArray());
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                string[] m_string = new string[3];
                m_string[0] = (string)m_dataTable.Rows[i]["CODE_NAME"];
                m_string[1] = m_dataTable.Rows[i]["CODE_ID"].ToString();
                m_string[2] = m_dataTable.Rows[i]["CODE_UUID"].ToString();
                m_list.Add(m_string);
            }
            return m_list;
        }

        public SysCodeInfoBE QuerySysCodeInfoForFile(string cate, string code_id)
        {
            SysCodeInfoBE m_result = new SysCodeInfoBE();
            string m_sql = @"
SELECT s.var_char01,
       s.var_char02,
       s.var_char03,
       s.var_char04,
       s.var_char05,

       s.var_char06,
       s.var_char07,
       s.var_char08,
       s.var_char09,
       s.var_char10
FROM ZT_SysCodeInfo AS s
WHERE s.cate = @cate and s.code_id = @code_id
ORDER BY s.seq";

            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@cate", cate));
                m_paraList.Add(new DBParameter("@code_id", code_id));
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                if (m_dataTable.Rows.Count > 0)
                {
                    m_result.VarChar01 = m_dataTable.Rows[0]["var_char01"].ToString();
                    m_result.VarChar02 = m_dataTable.Rows[0]["var_char02"].ToString();
                    m_result.VarChar03 = m_dataTable.Rows[0]["var_char03"].ToString();
                    m_result.VarChar04 = m_dataTable.Rows[0]["var_char04"].ToString();
                    m_result.VarChar05 = m_dataTable.Rows[0]["var_char05"].ToString();

                    m_result.VarChar06 = m_dataTable.Rows[0]["var_char06"].ToString();
                    m_result.VarChar07 = m_dataTable.Rows[0]["var_char07"].ToString();
                    m_result.VarChar08 = m_dataTable.Rows[0]["var_char08"].ToString();
                    m_result.VarChar09 = m_dataTable.Rows[0]["var_char09"].ToString();
                    m_result.VarChar10 = m_dataTable.Rows[0]["var_char10"].ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        public int SysCodeInfoQryCnt(string p_code_id, string p_cate)
        {
            int m_result = -1;
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                string m_sql = @"SELECT COUNT(1) AS cnt FROM ZT_SysCodeInfo WHERE code_id=@code_id AND cate=@cate ";
                m_paraList.Add(new DBParameter("@code_id", p_code_id));
                m_paraList.Add(new DBParameter("@cate", p_cate));

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

        #endregion

        #region Private Methods

        private SysCodeInfoBE genSysCodeInfoBE(DataRow p_row)
        {
            SysCodeInfoBE m_code = new SysCodeInfoBE();
            m_code.CodeUuid = new Guid(p_row["code_uuid"].ToString());
            m_code.CodeId = (string)p_row["code_id"];
            m_code.CodeName = (string)p_row["code_name"];
            m_code.Cate = (string)p_row["cate"];
            m_code.SuperUuid = new Guid(p_row["super_uuid"].ToString());
            m_code.Seq = (int)p_row["seq"];
            if (p_row["code_name_p"] != DBNull.Value)
            {
                m_code.CodeNameP = (string)p_row["code_name_p"];
            }
            if (p_row["code_sname_p"] != DBNull.Value)
            {
                m_code.CodeSNameP = (string)p_row["code_sname_p"];
            }
            if (p_row["code_name_s"] != DBNull.Value)
            {
                m_code.CodeNameS = (string)p_row["code_name_s"];
            }
            if (p_row["code_sname_s"] != DBNull.Value)
            {
                m_code.CodeSNameS = (string)p_row["code_sname_s"];
            }
            m_code.ModifyStatus = (string)p_row["modify_status"];
            m_code.StatusFlag = (string)p_row["status_flag"];
            if (p_row["description"] != DBNull.Value)
            {
                m_code.Description = (string)p_row["description"];
            }
            if (p_row["var_char01"] != DBNull.Value)
            {
                m_code.VarChar01 = (string)p_row["var_char01"];
                m_code.OrgVarChar01 = m_code.VarChar01;
            }
            if (p_row["var_char02"] != DBNull.Value)
            {
                m_code.VarChar02 = (string)p_row["var_char02"];
                m_code.OrgVarChar02 = m_code.VarChar02;
            }
            if (p_row["var_char03"] != DBNull.Value)
            {
                m_code.VarChar03 = (string)p_row["var_char03"];
                m_code.OrgVarChar03 = m_code.VarChar03;
            }
            if (p_row["var_char04"] != DBNull.Value)
            {
                m_code.VarChar04 = (string)p_row["var_char04"];
                m_code.OrgVarChar04 = m_code.VarChar04;
            }
            if (p_row["var_char05"] != DBNull.Value)
            {
                m_code.VarChar05 = (string)p_row["var_char05"];
                m_code.OrgVarChar05 = m_code.VarChar05;
            }
            if (p_row["var_char06"] != DBNull.Value)
            {
                m_code.VarChar06 = (string)p_row["var_char06"];
                m_code.OrgVarChar06 = m_code.VarChar06;
            }
            if (p_row["var_char07"] != DBNull.Value)
            {
                m_code.VarChar07 = (string)p_row["var_char07"];
                m_code.OrgVarChar07 = m_code.VarChar07;
            }
            if (p_row["var_char08"] != DBNull.Value)
            {
                m_code.VarChar08 = (string)p_row["var_char08"];
                m_code.OrgVarChar08 = m_code.VarChar08;
            }
            if (p_row["var_char09"] != DBNull.Value)
            {
                m_code.VarChar09 = (string)p_row["var_char09"];
                m_code.OrgVarChar09 = m_code.VarChar09;
            }
            if (p_row["var_char10"] != DBNull.Value)
            {
                m_code.VarChar10 = (string)p_row["var_char10"];
                m_code.OrgVarChar10 = m_code.VarChar10;
            }
            m_code.CreatedBy = new Guid(p_row["created_by"].ToString());
            m_code.CreatedByName = p_row["created_by_name"].ToString();
            m_code.CreatedDate = (DateTime)p_row["created_date"];
            m_code.UpdatedBy = new Guid(p_row["updated_by"].ToString());
            m_code.UpdatedByName = p_row["updated_by_name"].ToString();
            m_code.UpdatedDate = (DateTime)p_row["updated_date"];

            //子群組
            if (string.IsNullOrEmpty(p_row["group_name"].ToString()) && !string.IsNullOrEmpty(p_row["sub_group_name"].ToString()))
            {
                m_code.GroupName = (string)p_row["sub_group_name"];
                m_code.GroupId = (string)p_row["cate"] + "|" + p_row["super_uuid"];
            }
            else
            {
                m_code.GroupName = (string)p_row["group_name"];
                if (p_row["group_uuid"] != DBNull.Value)
                {
                    m_code.GroupId = (string)p_row["sub_group_id"] + "|" + p_row["group_uuid"].ToString();
                }
                if (p_row["sub_group_name"] != DBNull.Value)
                {
                    m_code.SubGroupName = (string)p_row["sub_group_name"];
                }
                m_code.Cate = m_code.Cate + "|" + p_row["super_uuid"].ToString();
            }

            return m_code;
        }
        #endregion

    }
}
