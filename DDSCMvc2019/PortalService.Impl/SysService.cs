using CommonLibrary.DBA;
using CommonLibrary.Login;
using Entity;
using Entity.FileImport;
using Entity.SYS;
using log4net;
using PortalService.Contract;
using PortalService.Contract.ViewModel;
using PortalService.Contract.ViewModel.System;
using PortalService.Impl.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace PortalService.Impl
{
    public class SysService : ISysService
    {
        private static ILog logger = LogManager.GetLogger(typeof(SysService));
        private DBASqlLog g_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);
        private SysCodeInfoBL g_sysCodeInfoBL = new SysCodeInfoBL();
        private SysProgramBL g_programBL = new SysProgramBL();

        #region SysCodeInfo
        public IEnumerable<SysCodeInfoBE> SysCodeInfoQuery(SysCodeInfoModel p_sysCodeVM)
        {
            return g_sysCodeInfoBL.SysCodeInfoQuery(p_sysCodeVM);
        }

        public SysCodeInfoBE QuerySysCodeInfo(string p_uuid)
        {
            return g_sysCodeInfoBL.QuerySysCodeInfo(p_uuid);
        }

        public SysCodeInfoBE QueryByCateCodeId(string p_codeId, string p_cate)
        {
            return g_sysCodeInfoBL.QueryByCateCodeId(p_codeId, p_cate);
        }

        public IEnumerable<string[]> QueryByCate(string p_cate)
        {
            return g_sysCodeInfoBL.QueryByCate(p_cate);
        }

        public bool insertSysCodeInfoBE(SysCodeInfoBE p_code)
        {
            return g_sysCodeInfoBL.insertSysCodeInfoBE(p_code);
        }

        public bool updateSysCodeInfoBE(SysCodeInfoBE p_code)
        {
            return g_sysCodeInfoBL.updateSysCodeInfoBE(p_code);
        }

        public bool deleteSysCodeInfoBE(string p_codeType, string p_codeUuid)
        {
            return g_sysCodeInfoBL.deleteSysCodeInfoBE(p_codeType, p_codeUuid);
        }

        public IEnumerable<string[]> QueryFirstLevel()
        {
            return g_sysCodeInfoBL.QueryFirstLevel();
        }

        public IEnumerable<string[]> QuerySecondLevel(string p_cate)
        {
            return g_sysCodeInfoBL.QuerySecondLevel(p_cate);
        }

        public IEnumerable<string[]> QueryFirstLevelForUser()
        {
            return g_sysCodeInfoBL.QueryFirstLevelForUser();
        }

        public SysCodeInfoBE QuerySysCodeInfoForFile(string cate, string code_id)
        {
            return g_sysCodeInfoBL.QuerySysCodeInfoForFile(cate, code_id);
        }

        public IEnumerable<string[]> QueryUuidByCate(string p_cate)
        {
            List<string[]> m_list = new List<string[]>();

            string m_sql = @"SELECT code_uuid, code_name  
                            FROM ZT_SysCodeInfo 
                            WHERE status_flag='Y' AND cate=@cate ";

            m_sql += " ORDER BY seq ";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@cate", p_cate));

            DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                string[] m_string = new string[2];
                m_string[0] = m_dataTable.Rows[i]["code_uuid"].ToString().Trim();
                m_string[1] = m_dataTable.Rows[i]["code_name"].ToString();
                m_list.Add(m_string);
            }
            return m_list;
        }

        public int SysCodeInfoQryCnt(string p_code_id, string p_cate)
        {
            return g_sysCodeInfoBL.SysCodeInfoQryCnt(p_code_id, p_cate);
        }

        public bool IsDataModified(Guid p_CodeUuid, Guid p_UpdatedBy, DateTime p_UpdatedDate)
        {
            string m_sql = @"
SELECT TOP 1 'X'
  FROM ZT_SysCodeInfo 
 WHERE code_uuid = @code_uuid
   AND updated_by = @updated_by
   AND updated_date = @updated_date ";

            List<SqlParameter> m_paraList = new List<SqlParameter>();
            m_paraList.Add(new SqlParameter("@code_uuid", p_CodeUuid));
            m_paraList.Add(new SqlParameter("@updated_by", p_UpdatedBy));
            m_paraList.Add(new SqlParameter("@updated_date", p_UpdatedDate));

            string m_String = g_dba.GetData(m_sql, m_paraList.ToArray());
            return string.IsNullOrWhiteSpace(m_String);
        }

        #endregion

        #region SysProgram
        public IEnumerable<SysProgramBE> SysProgramQuery(SysProgramModel p_sysProgramVM)
        {
            return g_programBL.SysProgramQuery(p_sysProgramVM);
        }

        public bool insertSysProgramBE(SysProgramBE p_program)
        {
            return g_programBL.insertSysProgramBE(p_program);
        }

        public bool updateSysProgramBE(SysProgramBE p_program)
        {
            return g_programBL.updateSysProgramBE(p_program);
        }

        public bool deleteSysProgramBE(string p_programId)
        {
            return g_programBL.deleteSysProgramBE(p_programId);
        }

        public IEnumerable<string[]> QueryMenu(string p_type)
        {
            return g_programBL.QueryMenu(p_type);
        }
        public IEnumerable<string[]> QuerySubMenu(string p_parent)
        {
            return g_programBL.QuerySubMenu(p_parent);
        }
        #endregion

        #region SysGroupProgram
        public IEnumerable<SysGroupProgramBE> SysGroupProgramQuery(SysGroupProgramModel p_viewModel)
        {
            SysGroupProgramBL m_bl = new SysGroupProgramBL();
            return m_bl.SysGroupProgramQuery(p_viewModel);
        }

        public bool modifySysGroupProgramBE(List<SysGroupProgramBE> p_listVM)
        {
            SysGroupProgramBL m_bl = new SysGroupProgramBL();
            return m_bl.modifySysGroupProgramBE(p_listVM);
        }
        #endregion

        #region SysGroup
        public IEnumerable<SysGroupBE> SysGroupQuery(SysGroupModel p_viewModel)
        {
            SysGroupBL m_bl = new SysGroupBL();
            return m_bl.SysGroupQuery(p_viewModel);
        }

        public bool insertSysGroupBE(SysGroupBE p_be)
        {
            SysGroupBL m_bl = new SysGroupBL();
            return m_bl.insertSysGroupBE(p_be);
        }

        public bool updateSysGroupBE(SysGroupBE p_be)
        {
            SysGroupBL m_bl = new SysGroupBL();
            return m_bl.updateSysGroupBE(p_be);
        }

        public bool deleteSysGroupBE(Guid p_groupUuid)
        {
            SysGroupBL m_bl = new SysGroupBL();
            return m_bl.deleteSysGroupBE(p_groupUuid);
        }

        public int SysGroupProgramQryCnt(string p_groupId, Guid p_groupUuid) //, string p_orgId, Guid p_groupUuid)
        {
            SysGroupBL m_bl = new SysGroupBL();
            return m_bl.SysGroupProgramQryCnt(p_groupId, p_groupUuid); //, p_orgId, p_groupUuid);
        }

        public IEnumerable<string[]> QueryGroupName(bool p_isAdminGroup, bool p_isAdmin)
        {
            SysGroupBL m_bl = new SysGroupBL();
            return m_bl.QueryGroupName(p_isAdminGroup, p_isAdmin);
        }
        #endregion

        #region FlowJob


        public IEnumerable<FlowJobBE> FlowJobQuery(FlowJobModel p_viewModel)
        {
            FlowJobBL m_bl = new FlowJobBL();
            return m_bl.FlowJobQuery(p_viewModel);
        }

        public IEnumerable<string[]> QueryRecProgram(Guid p_UserUuid)
        {
            FlowJobBL m_bl = new FlowJobBL();
            return m_bl.QueryRecProgram(p_UserUuid);
        }
        public IEnumerable<string[]> QueryRecStatus()
        {
            FlowJobBL m_bl = new FlowJobBL();
            return m_bl.QueryRecStatus();
        }
        #endregion

        #region SysUserLog

        public IEnumerable<SysUserLogBE> SysUserLogQuery(SysUserLogModel p_sysUserLogVM, Guid p_UserUuid)
        {
            List<SysUserLogBE> m_result = new List<SysUserLogBE>();
            List<DBParameter> m_paraList = new List<DBParameter>();

            string m_sql = @"
SELECT L.*, U.user_id, U.user_name, P.func_name
  FROM ZT_SysUserLog L
  JOIN ZT_AaUser U
    ON L.user_uuid = U.user_uuid
  JOIN (SELECT A.code_id AS role_id
          FROM ZT_SysCodeInfo A
          JOIN (SELECT C.CATE
                  FROM ZT_AaUser U2
                  JOIN ZT_SysCodeInfo C
                    ON C.code_id = U2.role_id
                 WHERE U2.user_uuid = @user_uuid) B
            ON A.CATE = B.CATE) U3
    ON U3.role_id = u.role_id
  JOIN ZT_SysProgram P
    ON L.func_id = P.func_id
 WHERE 1 = 1 ";

            try
            {
                //僅能查詢自己歸屬的群組內人員
                m_paraList.Add(new DBParameter("@user_uuid", p_UserUuid));

                if ((!string.IsNullOrEmpty(p_sysUserLogVM.FuncID)))
                {
                    m_sql += " AND p.func_id=@program_id";
                    m_paraList.Add(new DBParameter("@program_id", p_sysUserLogVM.FuncID));
                }
                if ((!string.IsNullOrEmpty(p_sysUserLogVM.UserID)))
                {
                    m_sql += " AND u.[user_id]=@user_id";
                    m_paraList.Add(new DBParameter("@user_id", p_sysUserLogVM.UserID));
                }
                if ((!string.IsNullOrEmpty(p_sysUserLogVM.DateStart)))
                {
                    m_sql += " AND l.exe_date>=@startDate";
                    m_paraList.Add(new DBParameter("@startDate", p_sysUserLogVM.DateStart));
                }
                if ((!string.IsNullOrEmpty(p_sysUserLogVM.DateEnd)))
                {
                    m_sql += " AND l.exe_date<=@endDate";
                    m_paraList.Add(new DBParameter("@endDate", p_sysUserLogVM.DateEnd));
                }

                m_sql += " ORDER BY l.exe_date DESC";


                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    SysUserLogBE m_program = genSysUserLogBE(m_dataTable.Rows[i]);
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

        /// <summary>
        /// 日常作業項目查詢
        /// </summary>
        /// <param name="p_vm"></param>
        /// <returns></returns>
        public IEnumerable<UserLogTaiFexBE> UserLogTaifexQuery(UserLogTaiFexModel p_vm)
        {
            List<UserLogTaiFexBE> m_result = new List<UserLogTaiFexBE>();
            List<DBParameter> m_paraList = new List<DBParameter>();

            string m_sql = @" SELECT ROW_NUMBER() OVER(ORDER BY exe_date) AS seq,
                                     l.user_log_uuid,
                                     o.org_id,
                                     o.org_name,
                                     u.user_id,
                                     u.user_name,
                                     l.exe_date,
                                     l.func_id,
                                     l.exe_btn,
                                     l.exe_query,
                                     p.func_name
                                FROM ZT_AaOrg o
                                LEFT JOIN ZT_AaUser u ON o.org_uuid = u.org_uuid
                                LEFT JOIN ZT_SysUserLog l ON U.USER_UUID = L.user_uuid
                                LEFT JOIN ZT_SysProgram p ON L.func_id = p.func_id
                               WHERE o.org_type='01'
                                 AND u.user_id = @UserId
                                 AND l.exe_date >= @DateStart
                                 AND l.exe_date <= @DateEnd
                               ORDER BY exe_date DESC ";

            try
            {
                m_paraList.Add(new DBParameter("@UserId", p_vm.UserId));
                m_paraList.Add(new DBParameter("@DateStart", p_vm.DateStart.Date));
                m_paraList.Add(new DBParameter("@DateEnd", p_vm.DateEnd.Date.AddDays(1).AddMilliseconds(-1)));

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    UserLogTaiFexBE m_logBe = genUserLogTaiFexBE(m_dataTable.Rows[i]);
                    m_result.Add(m_logBe);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }

            return m_result;
        }

        private SysUserLogBE genSysUserLogBE(DataRow p_row)
        {
            SysUserLogBE m_userLog = new SysUserLogBE();

            m_userLog.user_log_uuid = (Guid)p_row["user_log_uuid"];
            m_userLog.user_info_uuid = (Guid)p_row["user_info_uuid"];
            m_userLog.user_uuid = (Guid)p_row["user_uuid"];
            m_userLog.user_id = (string)p_row["user_id"];
            m_userLog.user_name = (string)p_row["user_name"];
            m_userLog.func_id = (string)p_row["func_id"];
            m_userLog.func_name = (string)p_row["func_name"];
            m_userLog.exe_date = (DateTime)p_row["exe_date"];
            m_userLog.exe_btn = (string)p_row["exe_btn"];
            m_userLog.exe_query = (string)p_row["exe_query"];
            m_userLog.exe_result = (string)p_row["exe_result"];
            m_userLog.status_flag = (string)p_row["status_flag"];
            m_userLog.created_by = (Guid)p_row["created_by"];
            m_userLog.created_date = (DateTime)p_row["created_date"];
            m_userLog.updated_by = (Guid)p_row["updated_by"];
            m_userLog.updated_date = (DateTime)p_row["updated_date"];

            return m_userLog;
        }

        private UserLogTaiFexBE genUserLogTaiFexBE(DataRow p_row)
        {
            UserLogTaiFexBE m_be = new UserLogTaiFexBE();

            m_be.Seq = p_row["seq"] == DBNull.Value ? "" : p_row["seq"].ToString();
            m_be.UserLogUuid = p_row["user_log_uuid"] == DBNull.Value ? "" : p_row["user_log_uuid"].ToString();
            m_be.OrgId = p_row["org_id"] == DBNull.Value ? "" : (string)p_row["org_id"];
            m_be.OrgName = p_row["org_name"] == DBNull.Value ? "" : (string)p_row["org_name"];
            m_be.UserId = p_row["user_id"] == DBNull.Value ? "" : (string)p_row["user_id"];
            m_be.UserName = p_row["user_name"] == DBNull.Value ? "" : (string)p_row["user_name"];
            m_be.ExeDate = (DateTime)p_row["exe_date"];
            if (p_row["func_id"] != DBNull.Value)
            {
                if (p_row["func_id"].ToString().Length == 3)
                {
                    m_be.FuncId = p_row["func_id"].ToString().Substring(1, 2);
                }
                else
                {
                    m_be.FuncId = (string)p_row["func_id"];
                }
            }
            else
            {
                m_be.FuncId = "";
            }
            m_be.FuncName = p_row["func_name"] == DBNull.Value ? "" : (string)p_row["func_name"];
            m_be.ExeBtn = p_row["exe_btn"] == DBNull.Value ? "" : (string)p_row["exe_btn"];
            m_be.ExeQuery = p_row["exe_query"] == DBNull.Value ? "" : (string)p_row["exe_query"];

            return m_be;
        }

        #endregion

        #region Login

        public UserData Login(LoginViewModel p_loginVM)
        {
            string m_loginModual = ConfigurationManager.AppSettings["LoginModual"];
            Type m_taskType = Type.GetType(m_loginModual);
            ILoginProcess process = (ILoginProcess)Activator.CreateInstance(m_taskType);
            UserData m_userData = null;
            if (process != null)
            {
                m_userData = process.Login(p_loginVM);
            }
            else
            {
                logger.Error("Login Modual未定義");
            }

            return m_userData;
        }

        public bool UpdatePwdStatus(Guid p_userUuid, int p_retry, int p_retryMax)
        {
            bool m_ok = false;
            string m_sql = @"UPDATE ZT_AaUser SET retry=@retry, updated_by=@user_uuid, updated_date=@updated_date ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_uuid", p_userUuid));
                m_paraList.Add(new DBParameter("@retry", p_retry));
                m_paraList.Add(new DBParameter("@updated_date", DateTime.Now));
                if (p_retry >= p_retryMax)
                {
                    m_sql += ", pwd_status=@pwd_status ";
                    m_paraList.Add(new DBParameter("@pwd_status", "L"));
                }
                m_sql += " WHERE user_uuid=@user_uuid ";

                int cnt = g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                if (cnt < 0 || g_dba.isException)
                {
                    m_ok = false;
                }
                else
                    m_ok = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_userUuid);

                m_ok = false;
            }
            return m_ok;
        }

        /// <summary>
        /// 查詢LDAP參數
        /// </summary>
        /// <returns></returns>
        private LdapSystemBE getLdapSystemData()
        {
            LdapSystemBE m_result = new LdapSystemBE();
            string m_sql = "";
            DataTable m_dataTable = null;
            List<DBParameter> m_paraList = new List<DBParameter>();
            string m_ErrMsg = "";

            try
            {
                m_sql = @"select code_uuid, code_id, code_name, cate, modify_status, description, status_flag, 
                                 var_char01, var_char02, var_char03, var_char04, var_char05, 
                                 var_char06, var_char07, var_char08, var_char09, var_char10 
                          from [dbo].[ZT_SysCodeInfo]
                          where cate='CTBC_SYS' and code_id='LDAP' ";
                m_paraList.Clear();
                m_dataTable = g_dba.GetDataTable(m_sql);

                if (m_dataTable != null && m_dataTable.Rows.Count > 0)
                {
                    // 查詢結果=成功
                    m_result.query_result = true;

                    for (int i = 0; i < m_dataTable.Rows.Count; i++)
                    {
                        // Ldap連線模式
                        if (m_dataTable.Rows[i]["var_char01"] != DBNull.Value)
                        {
                            m_result.ldap_system = (string)m_dataTable.Rows[i]["var_char01"];
                        }
                        // Ldap Server
                        if (m_dataTable.Rows[i]["var_char02"] != DBNull.Value)
                        {
                            m_result.ldap_server = (string)m_dataTable.Rows[i]["var_char02"];
                        }
                        // Ldap Server Port
                        if (m_dataTable.Rows[i]["var_char03"] != DBNull.Value)
                        {
                            // 預設Port=389
                            short m_LdapServerPortShort = 389;
                            short.TryParse(m_dataTable.Rows[i]["var_char03"].ToString(), out m_LdapServerPortShort);
                            if (m_LdapServerPortShort > 0)
                            {
                                m_result.ldap_server_port = m_LdapServerPortShort;
                            }
                        }
                        // 連線系統帳號
                        if (m_dataTable.Rows[i]["var_char04"] != DBNull.Value)
                        {
                            m_result.system_user = (string)m_dataTable.Rows[i]["var_char04"];
                        }
                        // 連線系統帳號DN
                        if (m_dataTable.Rows[i]["var_char05"] != DBNull.Value)
                        {
                            m_result.system_user_dn = (string)m_dataTable.Rows[i]["var_char05"];
                        }
                        // 連線系統密碼
                        if (m_dataTable.Rows[i]["var_char06"] != DBNull.Value)
                        {
                            // 呼叫取得 DES Decrypt 資料
                            m_result.system_pwd = CommonLibrary.DES.DESCode.desDecryptBase64((string)m_dataTable.Rows[i]["var_char06"], ref m_ErrMsg);

                            // 更新回應訊息
                            if (string.IsNullOrWhiteSpace(m_ErrMsg) == false)
                            {
                                m_result.response_message = m_ErrMsg;
                            }
                        }
                        // 密碼逾期天數
                        if (m_dataTable.Rows[i]["var_char07"] != DBNull.Value)
                        {
                            // 預設密碼逾期天數=90
                            short m_PdExpiredDays = 90;
                            short.TryParse(m_dataTable.Rows[i]["var_char07"].ToString(), out m_PdExpiredDays);
                            if (m_PdExpiredDays > 0)
                            {
                                m_result.pwd_expired_days = m_PdExpiredDays;
                            }
                        }
                        // 角色代碼
                        if (m_dataTable.Rows[i]["var_char08"] != DBNull.Value)
                        {
                            m_result.role = (string)m_dataTable.Rows[i]["var_char08"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());

                m_result.query_result = false;
                m_result.response_message = ex.Message;
            }

            return m_result;
        }

        

        public bool ModifyUserInfo(string p_userId, string p_userName, string p_roleId, string p_mail)
        {
            bool m_success = false;

            try
            {
                string m_sql = @"IF NOT EXISTS (SELECT 1 FROM ZT_AaUser WHERE user_id=@user_id AND org_id = '822')
INSERT INTO ZT_AaUser (user_uuid,user_id,user_name,org_uuid,org_id,role_id,role_uuid,pwd_status,pwd,pwd2,pwd3,pwd4
,pwd_modify_date,user_mail,tel,user_fax,user_mobil,status_flag,created_by,created_date,updated_by,updated_date,retry,Salt)
VALUES (@user_uuid,@user_id,@user_name,(SELECT org_uuid FROM ZT_AaOrg WHERE org_type=@org_type)
,(SELECT org_id FROM ZT_AaOrg WHERE org_type=@org_type),@role_id
,(SELECT code_uuid FROM ZT_SysCodeInfo WHERE code_id=@role_id),'Y','','','','',GETDATE(),@mail,'','','','Y'
,@user_uuid,GETDATE(),@user_uuid,GETDATE(),'0','salt');
ELSE
UPDATE ZT_AaUser SET user_name=@user_name, role_id=@role_id, role_uuid=(SELECT code_uuid FROM ZT_SysCodeInfo WHERE code_id=@role_id AND code_id <> ''),
user_mail=@mail,updated_by=(SELECT user_uuid FROM ZT_AaUser WHERE user_id=@user_id AND org_id = '822'),updated_date=GETDATE() WHERE user_id=@user_id
AND EXISTS (SELECT TOP 1 1 FROM ZT_AaOrg WHERE org_id = ZT_AaUser.org_id AND org_type = @org_type) AND　org_id = '822'";

                List<DBParameter> m_paraList = new List<DBParameter>();
                Guid m_userUuid = Guid.NewGuid();
                m_paraList.Add(new DBParameter("@user_uuid", m_userUuid));
                m_paraList.Add(new DBParameter("@user_id", p_userId));
                m_paraList.Add(new DBParameter("@user_name", p_userName));
                m_paraList.Add(new DBParameter("@role_id", p_roleId));
                m_paraList.Add(new DBParameter("@mail", p_mail));
                m_paraList.Add(new DBParameter("@org_type", "00"));

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

        /// <summary>
        /// 取得6碼英數字 (For DDSC PinBlock 測試)
        /// </summary>
        /// <returns></returns>
        public string getPinBlock()
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                //int t = rand.Next(allCharArray.Length);
                int t = 0;
                if (temp != -1 && temp == t)
                {
                    return getPinBlock();
                }
                temp = t;
                randomCode += allCharArray[t];
            }

            return randomCode;
        }

        public bool CheckLoginStatus(Guid p_loginToken)
        {
            bool m_result = false;
            string m_sql = "SELECT * FROM ZT_SysUserInfo WHERE user_info_uuid=@token AND status_flag='Y'";
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@token", p_loginToken));
            DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
            if (m_dataTable.Rows.Count > 0)
            {
                m_result = true;
            }

            return m_result;
        }

        #endregion

        #region ZTClrList

        public IEnumerable<ZtClrlistBE> ZTClrListQuery(ZTClrListViewModel p_viewModel, bool p_IsSearch)
        {
            List<ZtClrlistBE> m_result = new List<ZtClrlistBE>();
            List<DBParameter> m_paraList = new List<DBParameter>();

            string m_sql = @"SELECT  t.*,
                                    CONVERT(VARCHAR(10), t.keep_days) + ' ' + c.code_name AS clearTypeName,
                                    uc.user_name AS created_by_name,
                                    uu.user_name AS updated_by_name
                            FROM    Zt_ClrList t
                                    LEFT JOIN ZT_SysCodeInfo c ON t.clear_type = c.code_id
                                    LEFT JOIN ZT_AaUser uc ON t.created_by = uc.user_uuid
                                    LEFT JOIN ZT_AaUser uu ON t.updated_by = uu.user_uuid
                            WHERE   c.cate = 'CLEAR_TYPE' ";

            try
            {
                if ((!string.IsNullOrEmpty(p_viewModel.ClearTable)))
                {
                    if (p_IsSearch)
                    {
                        m_sql += " AND t.clear_table like @clear_table";
                        m_paraList.Add(new DBParameter("@clear_table", "%" + p_viewModel.ClearTable.Trim() + "%"));
                    }
                    else
                    {
                        m_sql += " AND t.clear_table = @clear_table";
                        m_paraList.Add(new DBParameter("@clear_table", p_viewModel.ClearTable));
                    }
                }

                m_sql += " ORDER BY t.seq ";

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    ZtClrlistBE m_clear = genZTClrListBE(m_dataTable.Rows[i]);
                    m_result.Add(m_clear);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        public bool insertZTClrListBE(ZtClrlistBE p_clear)
        {
            bool m_success = false;

            string m_sql = @"INSERT  INTO Zt_ClrList
                    (clear_uuid,
                     clear_table_uuid,
                     clear_table,
                     clear_table_type,
                     clear_type,
                     keep_days,
                     seq,
                     is_achieve,
                     descriptions,
                     file_path,
                     status_flag,
                     created_by,
                     created_date,
                     updated_by,
                     updated_date)
            VALUES  (@clear_uuid,
                     @clear_table_uuid,
                     @clear_table,
                     @clear_table_type,
                     @clear_type,
                     @keep_days,
                     @seq,
                     @is_achieve,
                     @descriptions,
                     @file_path,
                     @status_flag,
                     @created_by,
                     @created_date,
                     @updated_by,
                     @updated_date)";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();

                m_paraList.Add(new DBParameter("@clear_uuid", p_clear.ClearUuid));
                m_paraList.Add(new DBParameter("@clear_table_uuid", p_clear.ClearTableUuid));
                m_paraList.Add(new DBParameter("@clear_table", p_clear.ClearTable));
                m_paraList.Add(new DBParameter("@clear_table_type", p_clear.ClearTableType));
                m_paraList.Add(new DBParameter("@clear_type", p_clear.ClearType));
                m_paraList.Add(new DBParameter("@keep_days", p_clear.KeepDays));
                m_paraList.Add(new DBParameter("@seq", p_clear.Seq));
                m_paraList.Add(new DBParameter("@is_achieve", p_clear.IsAchieve));
                m_paraList.Add(new DBParameter("@descriptions", p_clear.Descs));
                m_paraList.Add(new DBParameter("@file_path", p_clear.FilePath));
                m_paraList.Add(new DBParameter("@status_flag", p_clear.status_flag));
                m_paraList.Add(new DBParameter("@created_by", p_clear.created_by));
                m_paraList.Add(new DBParameter("@created_date", p_clear.created_date));
                m_paraList.Add(new DBParameter("@updated_by", p_clear.updated_by));
                m_paraList.Add(new DBParameter("@updated_date", p_clear.updated_date));

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_clear.updated_by);
                m_success = false;
            }
            return m_success;
        }

        public bool insertZTClrListBatch(string p_clearUuid, string p_tableListUuid, string p_createdBy)
        {
            bool m_success = false;

            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                string m_sql = string.Empty;

                if (string.IsNullOrEmpty(p_tableListUuid))
                {
                    m_sql = @"DELETE  ZT_ClrTableList
                            WHERE   clear_uuid = @clear_uuid;";

                    m_paraList.Add(new DBParameter("@clear_uuid", p_clearUuid));

                }
                else
                {

                    p_tableListUuid = p_tableListUuid.Trim(',');
                    p_tableListUuid = string.IsNullOrEmpty(p_tableListUuid) ? "" : "'" + p_tableListUuid.Replace(",", "','").Trim(',') + "'";

                    m_sql = @"DELETE  ZT_ClrTableList
                            WHERE   clear_uuid = @clear_uuid;
                            INSERT  INTO ZT_ClrTableList
                                    (table_list_uuid,
                                     clear_uuid,
                                     table_name,
                                     table_desc,
                                     clear_seq,
                                     status_flag,
                                     created_by,
                                     created_date,
                                     updated_by,
                                     updated_date)
                                    SELECT  table_uuid,
                                            @clear_uuid,
                                            table_name,
                                            table_desc,
                                            @clear_seq,
                                            @status_flag,
                                            @created_by,
                                            GETDATE(),
                                            @updated_by,
                                            GETDATE()
                                    FROM    ZT_EtlTable
                                    WHERE   table_uuid IN (" + p_tableListUuid + ");";

                    m_paraList.Add(new DBParameter("@clear_uuid", p_clearUuid));
                    //m_paraList.Add(new DBParameter("@clear_table_uuid", p_tableListUuid));

                    m_paraList.Add(new DBParameter("@clear_seq", "10"));
                    m_paraList.Add(new DBParameter("@status_flag", "Y"));

                    m_paraList.Add(new DBParameter("@created_by", p_createdBy));
                    m_paraList.Add(new DBParameter("@updated_by", p_createdBy));
                }

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, Guid.Parse(p_createdBy));
                m_success = false;
            }
            return m_success;
        }

        public bool updateZTClrListBE(ZtClrlistBE p_clear)
        {
            bool m_success = false;

            string m_sql = @"UPDATE  Zt_ClrList
                                SET     clear_table_uuid = @clear_table_uuid,
                                        clear_table = @clear_table,
                                        clear_table_type = @clear_table_type,
                                        clear_type = @clear_type,
                                        keep_days = @keep_days,
                                        seq = @seq,
                                        is_achieve = @is_achieve,
                                        descriptions = @descriptions,
                                        file_path = @file_path,
                                        status_flag = @status_flag,
                                        updated_by = @updated_by,
                                        updated_date = @updated_date
                                WHERE   clear_uuid = @clear_uuid";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();

                m_paraList.Add(new DBParameter("@clear_uuid", p_clear.ClearUuid));
                m_paraList.Add(new DBParameter("@clear_table_uuid", p_clear.ClearTableUuid));
                m_paraList.Add(new DBParameter("@clear_table", p_clear.ClearTable));
                m_paraList.Add(new DBParameter("@clear_table_type", p_clear.ClearTableType));
                m_paraList.Add(new DBParameter("@clear_type", p_clear.ClearType));
                m_paraList.Add(new DBParameter("@keep_days", p_clear.KeepDays));
                m_paraList.Add(new DBParameter("@seq", p_clear.Seq));
                m_paraList.Add(new DBParameter("@is_achieve", p_clear.IsAchieve));
                m_paraList.Add(new DBParameter("@descriptions", p_clear.Descs));
                m_paraList.Add(new DBParameter("@file_path", p_clear.FilePath));
                m_paraList.Add(new DBParameter("@status_flag", p_clear.status_flag));
                m_paraList.Add(new DBParameter("@updated_by", p_clear.updated_by));
                m_paraList.Add(new DBParameter("@updated_date", p_clear.updated_date));

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_clear.updated_by);
                m_success = false;
            }
            return m_success;
        }

        public bool deleteZTClrListBE(string p_clearUuid)
        {
            bool m_success = false;
            string m_sql = @"DELETE  FROM ZT_ClrTableList
                            WHERE   clear_uuid = @clear_uuid;
                            DELETE  FROM ZT_ClrList
                            WHERE   clear_uuid = @clear_uuid;";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@clear_uuid", p_clearUuid));
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

        private ZtClrlistBE genZTClrListBE(DataRow p_row)
        {
            ZtClrlistBE m_clear = new ZtClrlistBE();

            m_clear.ClearUuid = Guid.Parse(p_row["clear_uuid"].ToString());
            m_clear.ClearTableUuid = Guid.Parse(p_row["clear_table_uuid"].ToString());
            m_clear.ClearTable = p_row["clear_table"].ToString();
            m_clear.ClearType = p_row["clear_type"].ToString();
            m_clear.KeepDays = int.Parse(p_row["keep_days"].ToString());
            m_clear.Seq = int.Parse(p_row["seq"].ToString());
            m_clear.IsAchieve = p_row["is_achieve"].ToString();
            m_clear.Descs = p_row["descriptions"].ToString();
            m_clear.status_flag = p_row["status_flag"].ToString();

            m_clear.created_by = Guid.Parse(p_row["created_by"].ToString());
            m_clear.created_by_name = p_row["created_by_name"].ToString();
            m_clear.created_date = (DateTime)p_row["created_date"];
            m_clear.updated_by = Guid.Parse(p_row["updated_by"].ToString());
            m_clear.updated_by_name = p_row["updated_by_name"].ToString();
            m_clear.updated_date = (DateTime)p_row["updated_date"];

            m_clear.ClearTypeName = p_row["clearTypeName"].ToString();

            return m_clear;
        }

        #endregion ZTClrList

        #region ZT_EtlTable
        /// <summary>
        /// 取得ZT_EtlTable資料
        /// 2016-12-23 主要針對 is_houskeeping='1'
        /// </summary>
        /// <param name="etlTableViewModel"></param>
        /// <returns></returns>
        public IEnumerable<EtlTableBE> QueryHouskeepingTableList(EtlTableViewModel etlTableViewModel)
        {
            List<EtlTableBE> m_result = new List<EtlTableBE>();
            List<DBParameter> m_paraList = new List<DBParameter>();

            StringBuilder sqlScript = new StringBuilder();
            sqlScript.Append(@"SELECT  e.table_uuid, ");
            sqlScript.Append(@"        e.table_name, ");
            sqlScript.Append(@"        e.table_desc, ");
            sqlScript.Append(@"        e.status_flag, ");
            sqlScript.Append(@"        e.created_by, ");
            sqlScript.Append(@"        e.created_date, ");
            sqlScript.Append(@"        e.updated_by, ");
            sqlScript.Append(@"        e.updated_date, ");
            sqlScript.Append(@"        is_houskeeping, ");
            sqlScript.Append(@"        ISNULL(AaUser1.user_name, '') AS created_by_name, ");
            sqlScript.Append(@"        ISNULL(AaUser2.user_name, '') AS updated_by_name ");
            sqlScript.Append(@"FROM    ZT_EtlTable e ");
            sqlScript.Append(@"        LEFT JOIN ZT_AaUser AS AaUser1 ON e.created_by = AaUser1.user_uuid ");
            sqlScript.Append(@"        LEFT JOIN ZT_AaUser AS AaUser2 ON e.updated_by = AaUser2.user_uuid ");
            sqlScript.Append(@"WHERE   e.is_houskeeping = '1' ");

            if (!string.IsNullOrEmpty(etlTableViewModel.TABLE_NAME))
            {
                sqlScript.Append(@"AND e.table_name like @table_name ");
                m_paraList.Add(new DBParameter("@table_name", "%" + etlTableViewModel.TABLE_NAME.Trim() + "%"));
            }

            sqlScript.Append(@"ORDER BY e.table_name; ");

            try
            {

                DataTable m_dataTable = g_dba.GetDataTable(sqlScript.ToString(), m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    EtlTableBE m_clear = genZTEtlTableBE(m_dataTable.Rows[i]);
                    m_result.Add(m_clear);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }

            return m_result;
        }

        public EtlTableBE ZTEtlTableQuery(ZTClrTableListViewModel p_viewModel)
        {
            EtlTableBE m_result = new EtlTableBE();
            List<DBParameter> m_paraList = new List<DBParameter>();

            StringBuilder sqlScript = new StringBuilder();
            sqlScript.Append(@"SELECT  e.table_uuid, ");
            sqlScript.Append(@"        e.table_name, ");
            sqlScript.Append(@"        e.table_desc, ");
            sqlScript.Append(@"        e.status_flag, ");
            sqlScript.Append(@"        e.created_by, ");
            sqlScript.Append(@"        e.created_date, ");
            sqlScript.Append(@"        e.updated_by, ");
            sqlScript.Append(@"        e.updated_date, ");
            sqlScript.Append(@"        ISNULL(AaUser1.user_name, '') AS created_by_name, ");
            sqlScript.Append(@"        ISNULL(AaUser2.user_name, '') AS updated_by_name ");
            sqlScript.Append(@"FROM    ZT_EtlTable e ");
            sqlScript.Append(@"        LEFT JOIN ZT_AaUser AS AaUser1 ON e.created_by = AaUser1.user_uuid ");
            sqlScript.Append(@"        LEFT JOIN ZT_AaUser AS AaUser2 ON e.updated_by = AaUser2.user_uuid ");
            sqlScript.Append(@"WHERE   1 = 1 ");

            try
            {
                if (!string.IsNullOrEmpty(p_viewModel.ClearUuid))
                {
                    sqlScript.Append(@"AND e.table_uuid = @table_uuid ");
                    m_paraList.Add(new DBParameter("@table_uuid", p_viewModel.ClearUuid));
                }

                DataTable m_dataTable = g_dba.GetDataTable(sqlScript.ToString(), m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    m_result = genZTEtlTableBE(m_dataTable.Rows[i]);

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        //public IEnumerable<EtlTableBE> QueryHouskeepingTableListForHouseKeeping(string table_name)
        //{
        //    List<EtlTableBE> result = new List<EtlTableBE>();
        //    List<DBParameter> paraList = new List<DBParameter>();

        //    StringBuilder sqlScript = new StringBuilder();
        //    sqlScript.Append(@"SELECT A.TABLE_UUID, A.TABLE_NAME, A.TABLE_DESC FROM ZT_EtlTable A  ");
        //    sqlScript.Append(@"WHERE TABLE_NAME=@table_name AND IS_ETL<>'1' AND A.IS_HOUSKEEPING='1' ");
        //    paraList.Add(new DBParameter("@table_name", table_name));

        //    try
        //    {

        //        DataTable tblResult = g_dba.GetDataTable(sqlScript.ToString(), paraList.ToArray());
        //        foreach (DataRow row in tblResult.Rows)
        //        {
        //            EtlTableBE item = new EtlTableBE();
        //            item.TABLE_UUID = new Guid(row["TABLE_UUID"].ToString());
        //            item.TABLE_NAME = row["TABLE_NAME"].ToString();
        //            item.TABLE_DESC = row["TABLE_DESC"].ToString();

        //            result.Add(item);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //        new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
        //    }

        //    return result;
        //}


        /// <summary>
        /// 新增一筆資料 ZT_EtlTable
        /// </summary>
        /// <param name="etlTableBE"></param>
        /// <returns></returns>
        public bool InsertHouskeepingTableBE(EtlTableBE p_table)
        {
            List<DBParameter> m_paraList = new List<DBParameter>();

            try
            {
                string sqlScript = @"UPDATE  ZT_EtlTable SET     is_houskeeping = '1' WHERE   table_name = @table_name";

                m_paraList.Clear();
                m_paraList.Add(new DBParameter("@table_name", p_table.TABLE_NAME));

                if (g_dba.ExecNonQuery(sqlScript.ToString(), m_paraList.ToArray()) < 1)
                {
                    sqlScript = @"INSERT  INTO ZT_EtlTable
                                    (table_uuid, table_name, table_desc, status_flag, created_by, created_date, updated_by, updated_date, is_etl, is_houskeeping)
                            VALUES  (@table_uuid, @table_name, @table_desc, @status_flag, @created_by, @created_date, @updated_by, @updated_date, @is_etl, @is_houskeeping)";

                    m_paraList.Clear();
                    m_paraList.Add(new DBParameter("@table_uuid", Guid.NewGuid()));
                    m_paraList.Add(new DBParameter("@table_name", p_table.TABLE_NAME));
                    m_paraList.Add(new DBParameter("@table_desc", p_table.TABLE_DESC));
                    m_paraList.Add(new DBParameter("@status_flag", p_table.STATUS_FLAG));
                    m_paraList.Add(new DBParameter("@created_by", p_table.CREATE_USER_UUID));
                    m_paraList.Add(new DBParameter("@created_date", p_table.MODIFY_DATE));
                    m_paraList.Add(new DBParameter("@updated_by", p_table.MODIFY_USER_UUID));
                    m_paraList.Add(new DBParameter("@updated_date", p_table.MODIFY_DATE));
                    m_paraList.Add(new DBParameter("@is_etl", "0"));
                    m_paraList.Add(new DBParameter("@is_houskeeping", "1"));

                    g_dba.ExecNonQuery(sqlScript.ToString(), m_paraList.ToArray());

                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, Guid.Parse(p_table.MODIFY_USER_UUID));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 修改資料 ZT_EtlTable
        /// </summary>
        /// <param name="etlTableBE"></param>
        /// <returns></returns>
        public bool UpdateHouskeepingTableBE(EtlTableBE p_table)
        {
            string sqlScript = @"UPDATE  ZT_EtlTable
                                SET     table_desc = @table_desc,
                                        updated_by = @updated_by,
                                        updated_date = @updated_date
                                WHERE   table_uuid = @table_uuid";

            try
            {
                List<DBParameter> paraList = new List<DBParameter>();
                paraList.Add(new DBParameter("@table_uuid", p_table.TABLE_UUID));
                paraList.Add(new DBParameter("@table_desc", p_table.TABLE_DESC));
                //paraList.Add(new DBParameter("@status_flag", p_table.STATUS_FLAG));
                paraList.Add(new DBParameter("@updated_by", p_table.MODIFY_USER_UUID));
                paraList.Add(new DBParameter("@updated_date", p_table.MODIFY_DATE));

                g_dba.ExecNonQuery(sqlScript.ToString(), paraList.ToArray());

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, Guid.Parse(p_table.MODIFY_USER_UUID));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 刪除資料 ZT_EtlTable
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public bool DeleteHouskeepingTableBE(string p_tableUuid)
        {
            string sqlScript = @"UPDATE  ZT_EtlTable
                                SET     is_houskeeping = '0'
                                WHERE   table_uuid = @table_uuid;";

            try
            {
                List<DBParameter> paraList = new List<DBParameter>();
                paraList.Add(new DBParameter("@table_uuid", p_tableUuid));

                g_dba.ExecNonQuery(sqlScript, paraList.ToArray());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                return false;
            }

            return true;
        }

        private EtlTableBE genZTEtlTableBE(DataRow p_row)
        {
            EtlTableBE m_table = new EtlTableBE();
            char m_out = 'N';

            m_table.TABLE_UUID = Guid.Parse(p_row["table_uuid"].ToString());
            m_table.TABLE_NAME = p_row["table_name"].ToString();
            m_table.TABLE_DESC = p_row["table_desc"].ToString();
            m_table.STATUS_FLAG = char.TryParse(p_row["status_flag"].ToString().Trim(), out m_out) ? 'N' : char.Parse(p_row["status_flag"].ToString().Trim());

            m_table.CREATE_USER_UUID = p_row["created_by"].ToString();
            m_table.CREATE_USER_NAME = p_row["created_by_name"].ToString();
            m_table.CREATE_DATE = (DateTime)p_row["created_date"];
            m_table.MODIFY_USER_UUID = p_row["updated_by"].ToString();
            m_table.MODIFY_USER_NAME = p_row["updated_by_name"].ToString();
            m_table.MODIFY_DATE = (DateTime)p_row["updated_date"];

            return m_table;
        }
        #endregion

        #region ZtClrtablelist

        public string[] SelectedClrtablelistQuery(string p_clearUuid)
        {
            List<string> m_result = new List<string>();
            List<DBParameter> m_paraList = new List<DBParameter>();

            string m_sql = @"SELECT  t.table_list_uuid
                            FROM    ZT_ClrTableList t
                            WHERE   1 = 1 ";

            try
            {
                if ((!string.IsNullOrEmpty(p_clearUuid)))
                {
                    m_sql += " AND   t.clear_uuid = @clear_uuid ";
                    m_paraList.Add(new DBParameter("@clear_uuid", p_clearUuid));
                }

                m_sql += " ORDER BY t.clear_seq ";

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    m_result.Add(m_dataTable.Rows[i][0].ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result.ToArray();
        }


        private ZtClrtablelistBE genZtClrtablelistBE(DataRow p_row)
        {
            ZtClrtablelistBE m_clear = new ZtClrtablelistBE();

            m_clear.TableListUuid = Guid.Parse(p_row["table_list_uuid"].ToString());
            m_clear.ClearUuid = Guid.Parse(p_row["clear_uuid"].ToString());
            m_clear.TableName = p_row["table_name"].ToString();
            m_clear.TableDesc = p_row["table_desc"].ToString();
            m_clear.ClearSeq = int.Parse(p_row["clear_seq"].ToString());
            m_clear.status_flag = p_row["status_flag"].ToString();

            m_clear.created_by = Guid.Parse(p_row["created_by"].ToString());
            m_clear.created_date = (DateTime)p_row["created_date"];
            m_clear.updated_by = Guid.Parse(p_row["updated_by"].ToString());
            m_clear.updated_date = (DateTime)p_row["updated_date"];

            return m_clear;
        }

        #endregion ZtClrtablelist

        #region ZT_EtlColumn
        public IEnumerable<EtlColumnBE> QueryEtlColumnList(EtlColumnViewModel etlColumnViewModel)
        {
            List<EtlColumnBE> result = new List<EtlColumnBE>();

            StringBuilder sqlScript = new StringBuilder();
            sqlScript.Append(@"SELECT SEQNO = ROW_NUMBER() OVER (ORDER BY col.CREATED_DATE), tbl.TABLE_NAME, tbl.TABLE_DESC, col.COLUMN_NAME, col.COLUMN_DESC, col.COLUMN_TYPE, col.COLUMN_LENGTH, col.DIGITAL_LENGTH ");
            sqlScript.Append(@"FROM ZT_EtlTable tbl ");
            sqlScript.Append(@"INNER JOIN ZT_EtlColumn col ON tbl.TABLE_UUID=col.TABLE_UUID ");
            sqlScript.Append(@"WHERE tbl.TABLE_NAME=@table_name ");

            try
            {
                List<DBParameter> paraList = new List<DBParameter>();
                paraList.Add(new DBParameter("@table_name", etlColumnViewModel.TABLE_NAME));
                if (!string.IsNullOrEmpty(etlColumnViewModel.COLUMN_NAME))
                {
                    sqlScript.Append(@"AND col.COLUMN_NAME=@column_name ");
                    paraList.Add(new DBParameter("@column_name", etlColumnViewModel.COLUMN_NAME));
                }

                DataTable tblResult = g_dba.GetDataTable(sqlScript.ToString(), paraList.ToArray());
                foreach (DataRow row in tblResult.Rows)
                {
                    EtlColumnBE item = new EtlColumnBE();
                    item.SEQNO = row["SEQNO"].ToString();
                    item.COLUMN_NAME = row["COLUMN_NAME"].ToString();
                    item.COLUMN_DESC = row["COLUMN_DESC"].ToString();
                    item.COLUMN_TYPE = row["COLUMN_TYPE"].ToString();
                    item.COLUMN_LENGTH = Convert.ToInt32(row["COLUMN_LENGTH"].ToString());
                    item.DIGITAL_LENGTH = Convert.ToInt32(row["DIGITAL_LENGTH"].ToString());

                    result.Add(item);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }

            return result;
        }

        #region CRUD
        /// <summary>
        /// 新增資料 ZT_EtlColumn
        /// </summary>
        /// <param name="etlColumnBE"></param>
        /// <returns></returns>
        public bool InsertEtlColumnBE(EtlColumnViewModel etlColumnViewModel)
        {
            StringBuilder sqlScript = new StringBuilder();
            sqlScript.Append(@"INSERT INTO ZT_EtlColumn VALUES ");
            sqlScript.Append(@"(@column_uuid, @table_uuid, @column_name, @column_desc, ");
            sqlScript.Append(@"@column_type, @column_length, @digital_length, @status_flag, ");
            sqlScript.Append(@"@created_by, @created_date, ");
            sqlScript.Append(@"@updated_by, @updated_date);");

            try
            {
                Guid table_uuid = new Guid(etlColumnViewModel.TABLE_UUID);
                List<DBParameter> paraList = new List<DBParameter>();
                paraList.Add(new DBParameter("@column_uuid", Guid.NewGuid()));
                paraList.Add(new DBParameter("@table_uuid", table_uuid));
                paraList.Add(new DBParameter("@column_name", etlColumnViewModel.COLUMN_NAME));
                paraList.Add(new DBParameter("@column_desc", etlColumnViewModel.COLUMN_DESC));
                paraList.Add(new DBParameter("@column_type", etlColumnViewModel.COLUMN_TYPE));
                paraList.Add(new DBParameter("@column_length", etlColumnViewModel.COLUMN_LENGTH));
                paraList.Add(new DBParameter("@digital_length", etlColumnViewModel.DIGITAL_LENGTH));
                paraList.Add(new DBParameter("@status_flag", etlColumnViewModel.STATUS_FLAG));
                paraList.Add(new DBParameter("@created_by", etlColumnViewModel.CREATED_BY));
                paraList.Add(new DBParameter("@created_date", etlColumnViewModel.CREATED_DATE));
                paraList.Add(new DBParameter("@updated_by", etlColumnViewModel.UPDATED_BY));
                paraList.Add(new DBParameter("@updated_date", etlColumnViewModel.UPDATED_DATE));

                g_dba.ExecNonQuery(sqlScript.ToString(), paraList.ToArray());

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, etlColumnViewModel.UPDATED_BY);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 修改資料 ZT_EtlColumn
        /// </summary>
        /// <param name="etlColumnBE"></param>
        /// <returns></returns>
        public bool UpdateEtlColumnBE(EtlColumnViewModel etlColumnViewModel)
        {
            StringBuilder sqlScript = new StringBuilder();
            sqlScript.Append(@"UPDATE ZT_EtlColumn SET ");
            sqlScript.Append(@"COLUMN_NAME=@column_name, COLUMN_DESC=@column_desc, COLUMN_TYPE=@column_type, ");
            sqlScript.Append(@"COLUMN_LENGTH=@column_length, DIGITAL_LENGTH=@digital_length, STATUS_FLAG=@status_flag, ");
            sqlScript.Append(@"UPDATED_BY=@updated_by, UPDATED_DATE=@updated_date ");
            sqlScript.Append(@"WHERE TABLE_UUID=@table_uuid AND COLUMN_NAME=@column_name_ori; ");

            try
            {
                List<DBParameter> paraList = new List<DBParameter>();
                paraList.Add(new DBParameter("@table_uuid", etlColumnViewModel.TABLE_UUID));
                paraList.Add(new DBParameter("@column_name_ori", etlColumnViewModel.COLUMN_NAME_ORI));
                paraList.Add(new DBParameter("@column_name", etlColumnViewModel.COLUMN_NAME));
                paraList.Add(new DBParameter("@column_desc", etlColumnViewModel.COLUMN_DESC));
                paraList.Add(new DBParameter("@column_type", etlColumnViewModel.COLUMN_TYPE));
                paraList.Add(new DBParameter("@column_length", etlColumnViewModel.COLUMN_LENGTH));
                paraList.Add(new DBParameter("@digital_length", etlColumnViewModel.DIGITAL_LENGTH));
                paraList.Add(new DBParameter("@status_flag", etlColumnViewModel.STATUS_FLAG));
                paraList.Add(new DBParameter("@updated_by", etlColumnViewModel.UPDATED_BY));
                paraList.Add(new DBParameter("@updated_date", etlColumnViewModel.UPDATED_DATE));

                g_dba.ExecNonQuery(sqlScript.ToString(), paraList.ToArray());

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, etlColumnViewModel.UPDATED_BY);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 刪除一筆資料 ZT_EtlColumn
        /// </summary>
        /// <param name="table_uuid"></param>
        /// <param name="column_name"></param>
        /// <returns></returns>
        public bool DeleteEtlColumnBE(string table_uuid, string column_name)
        {
            StringBuilder sqlScript = new StringBuilder();
            sqlScript.Append(@"DELETE ZT_EtlColumn WHERE COLUMN_NAME=@column_name AND TABLE_UUID=@table_uuid; ");

            try
            {
                Guid guid = new Guid(table_uuid);

                List<DBParameter> paraList = new List<DBParameter>();
                paraList.Add(new DBParameter("@column_name", column_name));
                paraList.Add(new DBParameter("@table_uuid", guid));

                g_dba.ExecNonQuery(sqlScript.ToString(), paraList.ToArray());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                return false;
            }

            return true;

        }

        /// <summary>
        /// 刪除整筆資料 ZT_EtlColumn
        /// </summary>
        /// <param name="column_uuid"></param>
        /// <param name="table_uuid"></param>
        /// <returns></returns>
        //public bool DeleteEtlColumnBE(Guid table_uuid)
        //{
        //    StringBuilder sqlScript = new StringBuilder();
        //    sqlScript.Append(@"DELETE ZT_EtlColumn WHERE TABLE_UUID=@table_uuid; ");

        //    try
        //    {
        //        List<DBParameter> paraList = new List<DBParameter>();
        //        paraList.Add(new DBParameter("@table_uuid", table_uuid));

        //        g_dba.ExecNonQuery(sqlScript.ToString(), paraList.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //        new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
        //        return false;
        //    }

        //    return true;

        //}
        #endregion

        #endregion

        public IEnumerable<SysRoleProgramsBE> SysRoleProgramsQuery(SysRoleProgramsViewModel p_viewModel)
        {
            List<SysRoleProgramsBE> m_result = new List<SysRoleProgramsBE>();

            string m_sql = @"SELECT t.sn, t.role_code, t.program_id, t.program_name, t.program_desc
, CASE WHEN t.chkAddNew=0 THEN NULL ELSE ISNULL(rp.isAddNew,0) END AS isAddNew
, CASE WHEN t.chkUpdate=0 THEN NULL ELSE ISNULL(rp.isUpdate,0) END AS isUpdate
, CASE WHEN t.chkDelete=0 THEN NULL ELSE ISNULL(rp.isDelete,0) END AS isDelete 
, CASE WHEN t.chkQuery=0 THEN NULL ELSE ISNULL(rp.isQuery,0) END AS isQuery
, CASE WHEN t.chkView=0 THEN NULL ELSE ISNULL(rp.isView,0) END AS isView
, CASE WHEN t.chkPrint=0 THEN NULL ELSE ISNULL(rp.isPrint,0) END AS isPrint
, CASE WHEN t.chkImport=0 THEN NULL ELSE ISNULL(rp.isImport,0) END AS isImport
, CASE WHEN t.chkExport=0 THEN NULL ELSE ISNULL(rp.isExport,0) END AS isExport
, CASE WHEN t.chkExecute=0 THEN NULL ELSE ISNULL(rp.isExecute,0) END AS isExecute
FROM (
    SELECT ROW_NUMBER() OVER (ORDER BY sp.program_id) AS sn
    , (SELECT ri.role_code FROM sys_role_info ri WHERE ri.role_code=@role_code) as role_code
    , sp.program_id, sp.program_name, sp.program_desc, ISNULL(sp.isAddNew,0) AS chkAddNew
    , ISNULL(sp.isUpdate,0) AS chkUpdate, ISNULL(sp.isDelete,0) AS chkDelete, ISNULL(sp.isQuery,0) AS chkQuery
    , ISNULL(sp.isView,0) AS chkView, ISNULL(sp.isPrint,0) AS chkPrint, ISNULL(sp.isImport,0) AS chkImport
    , ISNULL(sp.isExport,0) AS chkExport, ISNULL(sp.isExecute,0) AS chkExecute
    FROM sys_program sp
    ) t
LEFT JOIN sys_role_programs rp ON rp.role_code=t.role_code and rp.program_id=t.program_id
ORDER BY t.program_id";

            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@role_code", p_viewModel.RoleCode));
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    SysRoleProgramsBE m_code = genSysRoleProgramsBE(m_dataTable.Rows[i]);
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

        private SysRoleProgramsBE genSysRoleProgramsBE(DataRow p_row)
        {
            SysRoleProgramsBE m_result = new SysRoleProgramsBE();
            m_result.sn = Convert.ToInt32(p_row["sn"]);
            m_result.role_code = (string)p_row["role_code"];
            m_result.program_id = (string)p_row["program_id"];
            m_result.program_name = (string)p_row["program_name"];
            m_result.program_desc = (string)p_row["program_desc"];

            if (p_row["isAddNew"] != DBNull.Value)
            {
                m_result.isAddNew = (bool)p_row["isAddNew"];
            }

            if (p_row["isUpdate"] != DBNull.Value)
            {
                m_result.isUpdate = (bool)p_row["isUpdate"];
            }

            if (p_row["isDelete"] != DBNull.Value)
            {
                m_result.isDelete = (bool)p_row["isDelete"];
            }

            if (p_row["isQuery"] != DBNull.Value)
            {
                m_result.isQuery = (bool)p_row["isQuery"];
            }

            if (p_row["isView"] != DBNull.Value)
            {
                m_result.isView = (bool)p_row["isView"];
            }

            if (p_row["isPrint"] != DBNull.Value)
            {
                m_result.isPrint = (bool)p_row["isPrint"];
            }

            if (p_row["isImport"] != DBNull.Value)
            {
                m_result.isImport = (bool)p_row["isImport"];
            }

            if (p_row["isExport"] != DBNull.Value)
            {
                m_result.isExport = (bool)p_row["isExport"];
            }

            if (p_row["isExecute"] != DBNull.Value)
            {
                m_result.isExecute = (bool)p_row["isExecute"];
            }
            return m_result;
        }

        public bool modifySysRoleProgramsBE(List<SysRoleProgramsBE> p_listVM)
        {
            bool m_success = false;
            //g_dba.BeginTrans();
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@role_code", p_listVM[0].role_code));

            try
            {
                string m_sql = @"DELETE FROM sys_role_programs WHERE role_code=@role_code";

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray()); //g_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());

                m_sql = @"INSERT INTO sys_role_programs (role_code,program_id,isAddNew,isUpdate,isDelete,isQuery,isView
,isPrint,isImport,isExport,isExecute,created_by,created_date,updated_by,updated_date) VALUES (@role_code,@program_id,@isAddNew
,@isUpdate,@isDelete,@isQuery,@isView,@isPrint,@isImport,@isExport,@isExecute,@created_by,@created_date,@updated_by,@updated_date)";

                for (int i = 0; i < p_listVM.Count; i++)
                {
                    List<DBParameter> m_paraList2 = new List<DBParameter>();
                    m_paraList2.Add(new DBParameter("@role_code", p_listVM[i].role_code));
                    m_paraList2.Add(new DBParameter("@program_id", p_listVM[i].program_id));
                    m_paraList2.Add(new DBParameter("@isAddNew", p_listVM[i].isAddNew == null ? false : p_listVM[i].isAddNew));
                    m_paraList2.Add(new DBParameter("@isUpdate", p_listVM[i].isUpdate == null ? false : p_listVM[i].isUpdate));
                    m_paraList2.Add(new DBParameter("@isDelete", p_listVM[i].isDelete == null ? false : p_listVM[i].isDelete));
                    m_paraList2.Add(new DBParameter("@isQuery", p_listVM[i].isQuery == null ? false : p_listVM[i].isQuery));
                    m_paraList2.Add(new DBParameter("@isView", p_listVM[i].isView == null ? false : p_listVM[i].isView));
                    m_paraList2.Add(new DBParameter("@isPrint", p_listVM[i].isPrint == null ? false : p_listVM[i].isPrint));
                    m_paraList2.Add(new DBParameter("@isImport", p_listVM[i].isImport == null ? false : p_listVM[i].isImport));
                    m_paraList2.Add(new DBParameter("@isExport", p_listVM[i].isExport == null ? false : p_listVM[i].isExport));
                    m_paraList2.Add(new DBParameter("@isExecute", p_listVM[i].isExecute == null ? false : p_listVM[i].isExecute));
                    m_paraList2.Add(new DBParameter("@created_by", "sys"));
                    m_paraList2.Add(new DBParameter("@created_date", DateTime.Now));
                    m_paraList2.Add(new DBParameter("@updated_by", "sys"));
                    m_paraList2.Add(new DBParameter("@updated_date", DateTime.Now));

                    int m_changeCount = g_dba.ExecNonQuery(m_sql, m_paraList2.ToArray());

                    if (m_changeCount == 1)
                    {
                        m_success = true;
                    }
                    else
                    {
                        m_success = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                m_success = false;
            }

            return m_success;
        }

        public IEnumerable<SysRoleInfoBE> QueryRoleCodeName()
        {
            List<SysRoleInfoBE> m_result = new List<SysRoleInfoBE>();
            string m_sql = "SELECT * FROM sys_role_info ORDER BY role_code";
            DataTable m_dataTable = g_dba.GetDataTable(m_sql);
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                SysRoleInfoBE m_roleInfo = genSysRoleInfoBE(m_dataTable.Rows[i]);
                m_result.Add(m_roleInfo);
            }
            return m_result;
        }

        public IEnumerable<string[]> QueryProgram(Guid p_UserUuid, bool p_IsAllQueriable)
        {
            return g_programBL.QueryProgram(p_UserUuid, p_IsAllQueriable);
        }

        public IEnumerable<SysRoleInfoBE> SysRoleInfoQuery(SysRoleInfoViewModel p_sysRoleVM)
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

        private SysRoleInfoBE genSysRoleInfoBE(DataRow p_row)
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
            //m_role.created_by = (string)p_row["created_by"];
            //m_role.created_date = (DateTime)p_row["created_date"];
            //m_role.updated_by = (string)p_row["updated_by"];
            //m_role.updated_date = (DateTime)p_row["updated_date"];

            return m_role;
        }

        public bool insertSysRoleInfo(SysRoleInfoBE p_role)
        {
            bool m_success = false;
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
                //m_paraList.Add(new DBParameter("@created_by", p_role.created_by));
                //m_paraList.Add(new DBParameter("@created_date", p_role.created_date));
                //m_paraList.Add(new DBParameter("@updated_by", p_role.updated_by));
                //m_paraList.Add(new DBParameter("@updated_date", p_role.updated_date));

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, Guid.Parse(p_role.MODIFY_USER_UUID));
                m_success = false;
            }
            return m_success;
        }

        //eva
        public bool updateSysRoleInfo(SysRoleInfoBE p_role)
        {
            bool m_success = false;
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
                //m_paraList.Add(new DBParameter("@updated_by", p_role.updated_by));
                //m_paraList.Add(new DBParameter("@updated_date", p_role.updated_date));

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, Guid.Parse(p_role.MODIFY_USER_UUID));
                m_success = false;
            }
            return m_success;
        }

        //eva
        public bool deleteSysRoleInfo(string p_roleCode)
        {
            bool m_success = false;
            string m_sql = @"DELETE FROM sys_role_info WHERE role_code=@role_code";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@role_code", p_roleCode));
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

        /// <summary>
        /// 查詢指定功能代號
        /// </summary>
        /// <param name="p_funcID"></param>
        /// <returns></returns>
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


        public IEnumerable<WorkFlowBE> WorkFlowQuery(WorkFlowViewModel p_workflowVM)
        {
            List<WorkFlowBE> m_result = new List<WorkFlowBE>();

            string m_sql = @"SELECT ROW_NUMBER() OVER (ORDER BY wf.program_id) AS sn,
wf.*, sp.program_name, sp.function_name_allow, sp.function_name_reject, ui.account_name AS user_name, ci.code_name AS status_name
FROM work_flow wf
INNER JOIN sys_program sp ON sp.program_id=wf.program_id
INNER JOIN sys_user_info ui ON ui.account_no=wf.send_user
INNER JOIN ZT_SysCodeInfo ci ON ci.code_id=wf.status_code AND ci.cate='review'
WHERE 1=1";
            if (!string.IsNullOrEmpty(p_workflowVM.ProgramId))
            {
                m_sql += " AND wf.program_id=@program_id";
            }
            if (p_workflowVM.SendDateS != DateTime.MinValue)
            {
                m_sql += " AND wf.send_date>=@send_date_s";
            }
            if (p_workflowVM.SendDateE != DateTime.MinValue)
            {
                m_sql += " AND wf.send_date<=@send_date_e";
            }
            if (!string.IsNullOrEmpty(p_workflowVM.StatusCode))
            {
                m_sql += " AND wf.status_code=@status_code";
            }
            m_sql += " ORDER BY wf.program_id";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                if (!string.IsNullOrEmpty(p_workflowVM.ProgramId))
                {
                    m_paraList.Add(new DBParameter("@program_id", p_workflowVM.ProgramId));
                }
                if (p_workflowVM.SendDateS != DateTime.MinValue)
                {
                    m_paraList.Add(new DBParameter("@send_date_s", p_workflowVM.SendDateS));
                }
                if (p_workflowVM.SendDateE != DateTime.MinValue)
                {
                    m_paraList.Add(new DBParameter("@send_date_e", p_workflowVM.SendDateE));
                }
                if (!string.IsNullOrEmpty(p_workflowVM.StatusCode))
                {
                    m_paraList.Add(new DBParameter("@status_code", p_workflowVM.StatusCode));
                }
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    WorkFlowBE m_workflow = genWorkFlowBE(m_dataTable.Rows[i]);
                    m_result.Add(m_workflow);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        private WorkFlowBE genWorkFlowBE(DataRow p_row)
        {
            WorkFlowBE m_workflow = new WorkFlowBE();
            m_workflow.uid = (Guid)p_row["uid"];
            m_workflow.program_id = (string)p_row["program_id"];
            m_workflow.data_key = (Guid)p_row["data_key"];
            m_workflow.content_bef = (string)p_row["content_bef"];
            m_workflow.content_aft = (string)p_row["content_aft"];
            m_workflow.send_user = (string)p_row["send_user"];
            m_workflow.send_date = (DateTime)p_row["send_date"];
            m_workflow.status_code = (string)p_row["status_code"];
            if (p_row["created_by"] != DBNull.Value)
            {
                m_workflow.created_by = (string)p_row["created_by"];
            }
            m_workflow.created_date = (DateTime)p_row["created_date"];
            if (p_row["updated_by"] != DBNull.Value)
            {
                m_workflow.updated_by = (string)p_row["updated_by"];
            }
            m_workflow.updated_date = (DateTime)p_row["updated_date"];

            m_workflow.sn = Convert.ToInt32(p_row["sn"]);
            m_workflow.program_name = (string)p_row["program_name"];
            m_workflow.function_name_allow = (string)p_row["function_name_allow"];
            m_workflow.function_name_reject = (string)p_row["function_name_reject"];
            m_workflow.user_name = (string)p_row["user_name"];
            m_workflow.status_name = (string)p_row["status_name"];
            return m_workflow;
        }

        public bool updateWorkFlowBE(WorkFlowBE p_wf)
        {
            bool m_success = false;
            string m_sql = @"UPDATE work_flow SET opinion=@opinion, updated_by=@updated_by, updated_date=@updated_date WHERE uid=@uid";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@uid", p_wf.uid));
                m_paraList.Add(new DBParameter("@opinion", p_wf.opinion));
                m_paraList.Add(new DBParameter("@updated_by", p_wf.updated_by));
                m_paraList.Add(new DBParameter("@updated_date", p_wf.updated_date));

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_success = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, Guid.Parse(p_wf.updated_by));
                m_success = false;
            }
            return m_success;
        }


        public Tuple<string, DataSet> SysQueryData(SysQueryModel p_queryData)
        {
            DataSet m_result = null;
            try
            {
                if (!p_queryData.IsUpdate)
                {
                    string[] m_sql = new string[1];
                    m_sql[0] = p_queryData.QueryData;
                    m_result = g_dba.GetDataSet(m_sql);
                }
                else
                {
                    g_dba.ExecNonQuery(p_queryData.QueryData);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(typeof(SysService).Name, "ERROR", ex.Message, Guid.Empty);
                return new Tuple<string, DataSet>(ex.Message, null);
            }

            return new Tuple<string, DataSet>(string.Empty, m_result);
        }

        public string SysQueryUpdate(SysQueryModel p_queryData)
        {
            string m_result = string.Empty;
            try
            {
                if (p_queryData.IsUpdate)
                {
                    m_result = $"執行成功 ({g_dba.ExecNonQuery(p_queryData.QueryData)} 個資料列受到影響)";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(typeof(SysService).Name, "ERROR", ex.Message, Guid.Empty);
                m_result = ex.Message;
            }

            return m_result;
        }
    }
}
