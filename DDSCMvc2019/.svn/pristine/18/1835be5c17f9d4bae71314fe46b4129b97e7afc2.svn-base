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
    public class AaUserBL
    {
        #region 成員變數

        private ILog logger = LogManager.GetLogger(typeof(AaUserBL));
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
        /// <param name="p_ViewModel"></param>
        /// <param name="p_PwdStatus">欲查詢的密碼狀態，中間用逗號隔開，ex: L,I</param>
        /// <param name="p_StatusFlag">欲查詢的資料狀態，中間用逗號隔開，ex: Y,V</param>
        /// <returns></returns>
        public IEnumerable<AaUser> AaUserQuery(AaUserModel p_ViewModel, string p_PwdStatus, string p_StatusFlag)
        {
            List<AaUser> m_result = new List<AaUser>();
            try
            {
                string m_sql = @"SELECT a.user_uuid,a.user_id,a.user_name,a.org_uuid,a.org_id,a.role_id,a.role_uuid,a.pwd_status,
(SELECT [dbo].[fn_DecryptByPassPhrasePwd](CONVERT(VARBINARY(200),a.pwd,2))) AS pwd,
(SELECT [dbo].[fn_DecryptByPassPhrasePwd](CONVERT(VARBINARY(200),a.pwd2,2))) AS pwd2,
a.pwd3,a.pwd4,a.pwd_modify_date,a.user_mail,a.tel,a.user_fax,a.user_mobil,a.status_flag,
a.created_by,a.created_date,a.updated_by,a.updated_date,a.retry,a.Salt,a.pwd_encrypt,
ISNULL((SELECT org_name FROM ZT_AaOrg AS o WHERE org_uuid=a.org_uuid AND org_id=a.org_id AND status_flag='Y'),'') AS org_name, 
ISNULL((SELECT code_name FROM ZT_SysCodeInfo AS c WHERE code_uuid=a.role_uuid AND code_id=a.role_id AND status_flag='Y'),'') AS role_name, 
ISNULL(AaUser1.[user_name], '') as created_by_name,
ISNULL(AaUser2.[user_name], '') as updated_by_name 
FROM ZT_AaUser AS a
LEFT JOIN ZT_AaUser AS AaUser1 ON a.created_by = AaUser1.user_uuid
LEFT JOIN ZT_AaUser AS AaUser2 ON a.updated_by = AaUser2.user_uuid
INNER JOIN ZT_AaOrg AS org ON org.org_uuid=a.org_uuid AND org.org_id=a.org_id AND org.status_flag='Y' 
WHERE 1=1 ";

                List<DBParameter> m_paraList = new List<DBParameter>();
                //true: 內部使用者, false: 外部使用者
                if (p_ViewModel.IsInternal)
                {
                    m_sql += @" AND org.org_type = '00' ";
                }
                else
                {
                    m_sql += @" AND org.org_type <> '00' ";
                }

                //org_uuid
                if (!string.IsNullOrEmpty(p_ViewModel.OrgUuid))
                {
                    m_sql += @" AND a.org_uuid =@org_uuid ";
                    m_paraList.Add(new DBParameter("@org_uuid", p_ViewModel.OrgUuid));
                }

                //user_id
                if (!string.IsNullOrEmpty(p_ViewModel.UserId))
                {
                    m_sql += " AND a.user_id like @user_id ";
                    m_paraList.Add(new DBParameter("@user_id", string.Format("%{0}%", p_ViewModel.UserId)));
                }

                ////user_name
                //if (!string.IsNullOrEmpty(p_ViewModel.UserName))
                //{
                //    m_sql += " AND a.user_name Like @user_name ";
                //    m_paraList.Add(new DBParameter("@user_name", "%" + p_ViewModel.UserName + "%"));
                //}

                ////role_uuid
                //if (!string.IsNullOrEmpty(p_ViewModel.RoleUuid))
                //{
                //    m_sql += " AND a.role_uuid=@role_uuid ";
                //    m_paraList.Add(new DBParameter("@role_uuid", p_ViewModel.RoleUuid));
                //}

                //pwd_status   L,I
                if (!string.IsNullOrWhiteSpace(p_PwdStatus))
                {
                    var m_strSplit = p_PwdStatus.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (m_strSplit?.Length > 0)
                    {
                        string m_PwdStatusSQL = "1 = 2";
                        for (int i = 0; i < m_strSplit.Length; i++)
                        {
                            m_PwdStatusSQL += $" OR a.pwd_status = @pwd_status{i} ";
                            m_paraList.Add(new DBParameter($"@pwd_status{i}", m_strSplit[i]));
                        }
                        m_sql += $"AND ({m_PwdStatusSQL})";
                    }
                    //m_sql += string.Format(" AND a.pwd_status IN ('{0}') ", p_PwdStatus.Replace(",", "','"));
                }

                //status_flag   Y,V
                if (!string.IsNullOrWhiteSpace(p_StatusFlag))
                {
                    var m_strSplit = p_StatusFlag.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (m_strSplit?.Length > 0)
                    {
                        string m_StatusFlagSQL = "1 = 2";
                        for (int i = 0; i < m_strSplit.Length; i++)
                        {
                            m_StatusFlagSQL += $" OR a.status_flag = @status_flag{i} ";
                            m_paraList.Add(new DBParameter($"@status_flag{i}", m_strSplit[i]));
                        }
                        m_sql += $"AND ({m_StatusFlagSQL})";
                    }
                    //m_sql += string.Format(" AND a.status_flag IN ('{0}') ", p_StatusFlag.Replace(",", "','"));
                }

                m_sql += " ORDER BY status_flag DESC, org_id, [user_id] ";

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());

                AaUser m_BE = new AaUser();
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    m_BE = genAaUserBE(m_dataTable.Rows[i]);
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

        /// <summary>
        /// Query by user_id and pwd
        /// </summary>
        /// <param name="p_user_id"></param>
        /// <param name="p_user_pwd"></param>
        /// <returns>AaUserBE</returns>
        public AaUserModel AaUserQryBE(string p_user_id, string p_user_pwd)
        {
            AaUserModel m_BE = null;
            try
            {
                string m_sql = @"
SELECT a.*, (
        SELECT org_name
          FROM ZT_AaOrg AS o
         WHERE org_uuid = a.org_uuid
           AND org_id = a.org_id
           AND status_flag = 'Y'
        ) AS org_name, (
        SELECT code_name
          FROM ZT_SysCodeInfo AS c
         WHERE cate = 'ROLE'
           AND code_uuid = a.role_uuid
           AND code_id = a.role_id
           AND status_flag = 'Y'
        ) AS role_name, (
        SELECT ISNULL(user_id, '') AS user_id
          FROM ZT_AaUser AS u
         WHERE user_uuid = a.created_by
        ) AS creator_id, (
        SELECT ISNULL(user_id, '') AS user_id
          FROM ZT_AaUser AS u1
         WHERE user_uuid = a.updated_by
        ) AS updater_id
FROM ZT_AaUser AS a
WHERE 1 = 1 and status_flag = 'Y' and pwd_status = 'Y' ";

                List<DBParameter> m_paraList = new List<DBParameter>();
                //user_id
                if (!string.IsNullOrEmpty(p_user_id))
                {
                    m_sql += " AND a.user_id=@user_id ";
                    m_paraList.Add(new DBParameter("@user_id", p_user_id));
                }
                //user_pwd
                if (!string.IsNullOrEmpty(p_user_pwd))
                {
                    m_sql += " AND a.pwd=@pwd ";
                    m_paraList.Add(new DBParameter("@pwd", p_user_pwd));
                }

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                if (m_dataTable != null && m_dataTable.Rows.Count >= 1)
                {
                    m_BE = BLHelper.DataRowToBE<AaUserModel>(m_dataTable.Rows[0]);
                    //m_BE.recInfo = BLHelper.DataRowToBE<RecInfoModel>(m_dataTable.Rows[0]);
                }
                //if (m_dataTable != null && m_dataTable.Rows.Count >= 1)
                //    m_BE = genBE(m_dataTable.Rows[0]);

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            finally
            {
                if (m_BE == null)
                    m_BE = new AaUserModel();
            }

            return m_BE;
        }

        /// <summary>
        /// Query-count of Same user_id (before insert)
        /// </summary>
        /// <param name="p_user_id">user_id</param>
        /// <returns></returns>
        public int AaUserQryCnt(string p_user_id, string p_org_id)
        {
            int m_result = -1;
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                string m_sql = @" SELECT COUNT(1) AS cnt FROM ZT_AaUser WHERE org_id=@org_id AND user_id = @user_id AND status_flag in ('V', 'Y')";
                m_paraList.Add(new DBParameter("@org_id", p_org_id));
                m_paraList.Add(new DBParameter("@user_id", p_user_id));

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

        /// <summary>
        /// DB驗證傳入帳號密碼
        /// </summary>
        /// <param name="p_user_id">user_id</param>
        /// <param name="p_user_pwd">user_pwd</param>
        /// <returns></returns>
        public int checkAuthQryCnt(string p_user_id, string p_user_pwd)
        {
            int m_result = -1;
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                string m_sql = @" select count(1) as cnt from ZT_AaUser 
where status_flag = 'Y' and pwd_status = 'Y' and user_id = @user_id and pwd = @pwd ";
                m_paraList.Add(new DBParameter("@user_id", p_user_id));
                m_paraList.Add(new DBParameter("@pwd", p_user_pwd));

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

        /// <summary>
        /// insert by BE
        /// </summary>
        /// <param name="p_BE"></param>
        /// <returns></returns>
        public bool insertAaUserBE(AaUser p_BE)
        {
            bool m_ok = false;
            string m_sql = @"INSERT INTO ZT_AaUser (
 user_uuid, user_id, user_name, org_uuid, org_id, role_id, role_uuid, pwd_status, pwd, pwd2, pwd3, pwd4, 
 pwd_modify_date, user_mail, tel, user_fax, user_mobil, status_flag, created_by, created_date, updated_by, 
 updated_date, retry, Salt, pwd_encrypt
) VALUES (
 @user_uuid, @user_id, @user_name, @org_uuid, @org_id, @role_id, @role_uuid, @pwd_status
, (SELECT CONVERT(varchar(200),dbo.fn_EncryptByPassPhrasePwd(@pwd),2))
, (SELECT CONVERT(varchar(200),dbo.fn_EncryptByPassPhrasePwd(@pwd2),2))
, @pwd3, @pwd4, @pwd_modify_date, @user_mail, @tel, @user_fax, @user_mobil, @status_flag
, @created_by, @created_date, @updated_by, @updated_date, @retry, @Salt, @pwd_encrypt)";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_uuid", p_BE.user_uuid));
                m_paraList.Add(new DBParameter("@user_id", p_BE.user_id));
                m_paraList.Add(new DBParameter("@user_name", p_BE.user_name));
                m_paraList.Add(new DBParameter("@org_uuid", p_BE.org_uuid));
                m_paraList.Add(new DBParameter("@org_id", p_BE.org_id));
                m_paraList.Add(new DBParameter("@role_id", p_BE.role_id));
                m_paraList.Add(new DBParameter("@role_uuid", p_BE.role_uuid));
                m_paraList.Add(new DBParameter("@pwd_status", p_BE.pwd_status));
                m_paraList.Add(new DBParameter("@pwd", p_BE.pwd));
                m_paraList.Add(new DBParameter("@pwd2", p_BE.pwd2));
                m_paraList.Add(new DBParameter("@pwd3", p_BE.pwd3));
                m_paraList.Add(new DBParameter("@pwd4", p_BE.pwd4));

                m_paraList.Add(new DBParameter("@pwd_modify_date", p_BE.pwd_modify_date));
                m_paraList.Add(new DBParameter("@user_mail", p_BE.user_mail));
                m_paraList.Add(new DBParameter("@tel", p_BE.tel));
                m_paraList.Add(new DBParameter("@user_fax", p_BE.user_fax));
                m_paraList.Add(new DBParameter("@user_mobil", p_BE.user_mobil));
                m_paraList.Add(new DBParameter("@status_flag", p_BE.status_flag));
                m_paraList.Add(new DBParameter("@created_by", p_BE.created_by));
                m_paraList.Add(new DBParameter("@created_date", Convert.ToDateTime(p_BE.created_date)));
                m_paraList.Add(new DBParameter("@updated_by", p_BE.updated_by));

                m_paraList.Add(new DBParameter("@updated_date", Convert.ToDateTime(p_BE.updated_date)));
                m_paraList.Add(new DBParameter("@retry", p_BE.retry, SqlDbType.SmallInt));
                m_paraList.Add(new DBParameter("@Salt", p_BE.Salt));
                m_paraList.Add(new DBParameter("@pwd_encrypt", p_BE.pwd_encrypt));

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

        /// <summary>
        /// update by BE
        /// </summary>
        /// <param name="p_BE"></param>
        /// <returns></returns>
        public bool updateAaUserBE(AaUser p_BE)
        {
            bool m_ok = false;
            string m_sql = @"UPDATE ZT_AaUser SET user_name = @user_name, role_id = @role_id, role_uuid = @role_uuid
, user_mail = @user_mail, status_flag = @status_flag, updated_by = @updated_by,updated_date = @updated_date WHERE user_uuid = @user_uuid ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_uuid", p_BE.user_uuid));
                m_paraList.Add(new DBParameter("@user_name", p_BE.user_name));
                m_paraList.Add(new DBParameter("@role_id", p_BE.role_id));
                m_paraList.Add(new DBParameter("@role_uuid", p_BE.role_uuid));
                m_paraList.Add(new DBParameter("@user_mail", p_BE.user_mail));
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

        /// <summary>
        /// update status_flag by BE
        /// </summary>
        /// <param name="p_BE"></param>
        /// <returns></returns>
        public bool updateStatus(AaUser p_BE)
        {
            bool m_ok = false;
            string m_sql = @"UPDATE ZT_AaUser SET status_flag = @status_flag, updated_by = @updated_by,updated_date = @updated_date WHERE user_uuid = @user_uuid ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_uuid", p_BE.user_uuid));
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

        /// <summary>
        /// delete by key
        /// </summary>
        /// <param name="p_uuid"></param>
        /// <returns></returns>
        public bool deleteAaUserBE(Guid p_uuid)
        {
            bool m_ok = false;
            string m_sql = @"DELETE FROM ZT_AaUser WHERE user_uuid = @user_uuid ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_uuid", p_uuid));

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

        /// <summary>
        /// reset password by BE
        /// </summary>
        /// <param name="p_be"></param>
        /// <returns></returns>
        public bool resetAaUserBE(AaUser p_be)
        {
            bool m_ok = false;
            string m_sql = @"UPDATE ZT_AaUser SET pwd_status=@pwd_status, pwd=(SELECT CONVERT(varchar(200),dbo.fn_EncryptByPassPhrasePwd(@pwd),2))
, pwd2 = (SELECT CONVERT(varchar(200), dbo.fn_EncryptByPassPhrasePwd(@pwd2), 2)), pwd3 = @pwd3, pwd_encrypt = @pwd_encrypt, pwd_modify_date = @pwd_modify_date
, status_flag = @status_flag, retry = @retry, updated_by = @updated_by, updated_date = @updated_date WHERE user_uuid =@user_uuid";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_uuid", p_be.user_uuid));
                m_paraList.Add(new DBParameter("@pwd_status", p_be.pwd_status));
                m_paraList.Add(new DBParameter("@pwd", p_be.pwd));
                m_paraList.Add(new DBParameter("@pwd2", p_be.pwd2));
                m_paraList.Add(new DBParameter("@pwd3", p_be.pwd3));
                m_paraList.Add(new DBParameter("@pwd_encrypt", p_be.pwd_encrypt));
                m_paraList.Add(new DBParameter("@pwd_modify_date", p_be.pwd_modify_date));
                m_paraList.Add(new DBParameter("@status_flag", p_be.status_flag));
                m_paraList.Add(new DBParameter("@retry", p_be.retry));
                m_paraList.Add(new DBParameter("@updated_by", p_be.updated_by));
                m_paraList.Add(new DBParameter("@updated_date", Convert.ToDateTime(p_be.updated_date)));

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
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_be.updated_by);
                m_ok = false;
            }
            return m_ok;
        }

        /// <summary>
        /// change password by BE
        /// </summary>
        /// <param name="p_be"></param>
        /// <returns></returns>
        public bool changePwdChangePwdBE(ChangePwdBE p_be)
        {
            bool m_ok = false;
            string m_sql = @"UPDATE ZT_AaUser SET pwd=(SELECT CONVERT(VARCHAR(200),dbo.fn_EncryptByPassPhrasePwd(@pwd),2)), pwd3=@pwd3, pwd_status=@pwd_status, pwd_modify_date=@updated_date
, updated_by=@updated_by, updated_date=@updated_date WHERE user_uuid = @user_uuid ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_uuid", p_be.UserUid));
                m_paraList.Add(new DBParameter("@pwd", p_be.NewPwd));
                m_paraList.Add(new DBParameter("@pwd3", p_be.Pwd3));
                m_paraList.Add(new DBParameter("@pwd_status", "Y"));
                m_paraList.Add(new DBParameter("@updated_by", p_be.UpdatedBy));
                m_paraList.Add(new DBParameter("@updated_date", Convert.ToDateTime(p_be.UpdatedDate)));

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
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_be.UpdatedBy);
                m_ok = false;
            }
            return m_ok;
        }

        public bool CheckPwd(Guid p_userUuid, string p_pwd)
        {
            bool m_ok = false;
            string m_sql = @"SELECT COUNT(*) AS cnt FROM ZT_AaUser WHERE user_uuid=@user_uuid 
AND (SELECT dbo.fn_DecryptByPassPhrasePwd((SELECT CONVERT(VARBINARY(100),pwd,2) FROM ZT_AaUser 
WHERE user_uuid=@user_uuid)))=@pwd ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_uuid", p_userUuid));
                m_paraList.Add(new DBParameter("@pwd", p_pwd));

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                if (m_dataTable != null && m_dataTable.Rows.Count > 0)
                {
                    int m_result = Convert.ToInt32(m_dataTable.Rows[0]["cnt"]);
                    if (m_result > 0)
                    {
                        m_ok = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                m_ok = false;
            }
            return m_ok;
        }

        public AaUser QueryAaUserByUid(Guid p_uuid)
        {
            AaUser m_result = new AaUser();
            string m_sql = @"SELECT * FROM ZT_AaUser WITH (NOLOCK) WHERE user_uuid=@user_uuid";
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@user_uuid", p_uuid));
            try
            {
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());

                if (m_dataTable != null && m_dataTable.Rows.Count > 0)
                {
                    m_result = genAaUserBE(m_dataTable.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }

            return m_result;
        }

        public AaUser QueryAaUserById(string p_id)
        {
            AaUser m_result = new AaUser();
            string m_sql = @"SELECT * FROM ZT_AaUser WITH (NOLOCK) WHERE user_id=@user_id";
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@user_id", p_id));
            try
            {
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());

                if (m_dataTable != null && m_dataTable.Rows.Count > 0)
                {
                    m_result = genAaUserBE(m_dataTable.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }

            return m_result;
        }

        public IEnumerable<AaUser> QueryAaUserByRoleUid(Guid p_uuid)
        {
            List<AaUser> m_result = new List<AaUser>();
            string m_sql = @"SELECT * FROM ZT_AaUser WITH (NOLOCK) WHERE role_uuid=@role_uuid";
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@role_uuid", p_uuid));
            try
            {
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());

                AaUser m_BE = new AaUser();
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    m_BE = genAaUserBE(m_dataTable.Rows[i]);
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

        /// <summary>
        /// 使用者解鎖
        /// </summary>
        /// <param name="p_BE"></param>
        /// <returns></returns>
        public bool UnlockUserData(AaUser p_BE)
        {
            bool m_ok = false;
            string m_sql = @"UPDATE  ZT_AaUser
                            SET     pwd_status = @pwd_status,
                                    status_flag = @status_flag,
                                    retry = @retry,
                                    updated_by = @updated_by,
                                    updated_date = @updated_date
                            WHERE   user_uuid = @user_uuid ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_uuid", p_BE.user_uuid));
                m_paraList.Add(new DBParameter("@pwd_status", "V"));
                m_paraList.Add(new DBParameter("@status_flag", "V"));
                m_paraList.Add(new DBParameter("@retry", p_BE.retry));
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

        internal AaUser QueryAaUserByUidForResetPw(Guid p_UserUuid)
        {
            AaUser m_AaUser = new AaUser();
            var m_Sql = @"
SELECT org_id,
       user_id,
       dbo.fn_DecryptByPassPhrasePwd(CONVERT(VARBINARY(200), pwd2, 2)) AS pwd2
  FROM ZT_AaUser
 WHERE user_uuid = @user_uuid ";

            try
            {
                List<DBParameter> m_List = new List<DBParameter>();
                m_List.Add(new DBParameter("@user_uuid", p_UserUuid));

                var m_Dt = g_dba.GetDataTable(m_Sql, m_List.ToArray());
                if (m_Dt != null && m_Dt.Rows.Count > 0)
                {
                    var m_Dr = m_Dt.Rows[0];
                    m_AaUser.user_uuid = p_UserUuid;
                    m_AaUser.org_id = m_Dr["org_id"] as string;
                    m_AaUser.user_id = m_Dr["user_id"] as string;
                    m_AaUser.pwd2 = m_Dr["pwd2"] as string;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }

            return m_AaUser;
        }

        #endregion

        #region private Methods

        /// <summary>
        /// 將DataRow轉成Entity
        /// </summary>
        /// <param name="p_row"></param>
        /// <returns></returns>
        private AaUser genAaUserBE(DataRow p_row)
        {
            AaUser m_be = new AaUser();
            if (!p_row.IsNull("user_uuid"))
            {
                m_be.user_uuid = new Guid(p_row["user_uuid"].ToString());
            }
            if (!p_row.IsNull("user_id"))
            {
                m_be.user_id = (string)p_row["user_id"];
            }
            if (!p_row.IsNull("user_name"))
            {
                m_be.user_name = (string)p_row["user_name"];
            }
            if (!p_row.IsNull("org_uuid"))
            {
                m_be.org_uuid = new Guid(p_row["org_uuid"].ToString());
            }
            if (!p_row.IsNull("org_id"))
            {
                m_be.org_id = (string)p_row["org_id"];
            }
            if (!p_row.IsNull("role_uuid"))
            {
                m_be.role_uuid = new Guid(p_row["role_uuid"].ToString());
            }
            if (!p_row.IsNull("role_id"))
            {
                m_be.role_id = (string)p_row["role_id"];
            }
            if (!p_row.IsNull("pwd_status"))
            {
                m_be.pwd_status = (string)p_row["pwd_status"];
            }
            if (!p_row.IsNull("pwd"))
            {
                m_be.pwd = (string)p_row["pwd"];
            }
            if (!p_row.IsNull("pwd2"))
            {
                m_be.pwd2 = (string)p_row["pwd2"];
            }
            if (!p_row.IsNull("pwd3"))
            {
                m_be.pwd3 = (string)p_row["pwd3"];
            }
            if (!p_row.IsNull("pwd4"))
            {
                m_be.pwd4 = (string)p_row["pwd4"];
            }
            if (!p_row.IsNull("pwd_modify_date"))
            {
                m_be.pwd_modify_date = (DateTime)p_row["pwd_modify_date"];
            }
            if (!p_row.IsNull("user_mail"))
            {
                m_be.user_mail = (string)p_row["user_mail"];
            }
            if (!p_row.IsNull("tel"))
            {
                m_be.tel = (string)p_row["tel"];
            }
            if (!p_row.IsNull("user_fax"))
            {
                m_be.user_fax = (string)p_row["user_fax"];
            }
            if (!p_row.IsNull("user_mobil"))
            {
                m_be.user_mobil = (string)p_row["user_mobil"];
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
                m_be.created_date = Convert.ToDateTime(p_row["created_date"]).ToString("yyyy/MM/dd HH:mm:ss");
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
                m_be.updated_date = Convert.ToDateTime(p_row["updated_date"]).ToString("yyyy/MM/dd HH:mm:ss"); ;
            }
            if (!p_row.IsNull("retry"))
            {
                m_be.retry = Convert.ToInt32(p_row["retry"]);
            }
            if (!p_row.IsNull("Salt"))
            {
                m_be.Salt = (string)p_row["Salt"];
            }
            if (p_row.ContainsColumn("org_name") && !p_row.IsNull("org_name"))
            {
                m_be.org_name = (string)p_row["org_name"];
            }
            if (p_row.ContainsColumn("role_name") && !p_row.IsNull("role_name"))
            {
                m_be.role_name = (string)p_row["role_name"];
            }
            if (!p_row.IsNull("pwd_encrypt"))
            {
                m_be.pwd_encrypt = (string)p_row["pwd_encrypt"];
            }

            return m_be;
        }

        ///// <summary>
        ///// 將DataTable轉換成對應的Entity集合
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        //public IEnumerable<T> DataTableToEntities<T>(DataTable dt)
        //{
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        T result = Activator.CreateInstance<T>();
        //        foreach (DataColumn column in dt.Columns)
        //        {
        //            typeof(T).GetProperty(column.ColumnName).SetValue(result, DbNullToNull(row[column.ColumnName]), null);
        //        }
        //        yield return result;
        //    }
        //}

        ///// <summary>
        ///// 若值為DBNull.Value, 則轉為Null
        ///// </summary>
        ///// <param name="original"></param>
        ///// <returns></returns>
        //public object DbNullToNull(this object original)
        //{
        //    return original == DBNull.Value ? null : original;
        //}

        #endregion

    }
}
