using CommonLibrary.DBA;
using log4net;
using PortalService.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace PortalService.Impl.BL
{
    public class AaOrgBL
    {
        #region 成員變數

        private ILog logger = LogManager.GetLogger(typeof(AaOrgBL));
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
        /// Query OrgList
        /// </summary>
        /// <param name="p_isInternal">true: 內部使用者 OrgList, false: 外部使用者 OrgList</param>
        /// <returns></returns>
        public IEnumerable<string[]> QueryOrgList(bool p_isInternal)
        {
            List<string[]> m_list = new List<string[]>();
            string m_sql = @"SELECT org_uuid, org_id, org_name FROM ZT_AaOrg WHERE status_flag='Y' ";
            if (p_isInternal)
            {
                m_sql += " AND org_type = '00' ";
            }
            else
            {
                m_sql += " AND org_type <> '00' ";
            }
            m_sql += " ORDER BY org_id ";
            DataTable m_dataTable = g_dba.GetDataTable(m_sql.ToString());
            List<string> m_string;
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                m_string = new List<string>();
                m_string.Add(m_dataTable.Rows[i]["ORG_NAME"].ToString());
                m_string.Add(m_dataTable.Rows[i]["ORG_ID"].ToString());
                m_string.Add(m_dataTable.Rows[i]["ORG_UUID"].ToString());
                m_list.Add(m_string.ToArray());
            }
            return m_list;
        }

        public IEnumerable<string[]> QueryAaOrgRole(string p_uuid)
        {
            List<string[]> m_list = new List<string[]>();
            string m_sql = @"IF ((SELECT org_type FROM ZT_AaOrg WHERE org_uuid=@org_uuid)=@org_type)
SELECT code_uuid, code_id, code_name FROM ZT_SysCodeInfo WHERE cate IN (SELECT code_id FROM ZT_SysCodeInfo WHERE cate='ORG_PARM') 
AND cate NOT IN (SELECT org_id FROM ZT_AaOrg WHERE org_type<>@org_type) AND cate<>@cate ORDER BY seq
ELSE IF ((SELECT COUNT(*) FROM ZT_SysCodeInfo WHERE cate IN (SELECT org_id FROM ZT_AaOrg WHERE org_uuid=@org_uuid))>0)
SELECT code_uuid, code_id, code_name FROM ZT_SysCodeInfo WHERE cate IN (SELECT org_id FROM ZT_AaOrg WHERE org_uuid=@org_uuid) ORDER BY seq
ELSE
SELECT code_uuid, code_id, code_name FROM ZT_SysCodeInfo WHERE cate=@cate ORDER BY seq ";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@org_uuid", p_uuid));
            m_paraList.Add(new DBParameter("@org_type", "00"));
            m_paraList.Add(new DBParameter("@cate", "ALL"));
            DataTable m_dataTable = g_dba.GetDataTable(m_sql.ToString(), m_paraList.ToArray());
            List<string> m_string;
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                m_string = new List<string>();
                m_string.Add(m_dataTable.Rows[i]["code_uuid"].ToString());
                m_string.Add(m_dataTable.Rows[i]["code_id"].ToString());
                m_string.Add(m_dataTable.Rows[i]["code_name"].ToString());
                m_list.Add(m_string.ToArray());
            }
            return m_list;
        }

        /// <summary>
        /// AaUser Query
        /// </summary>
        /// <param name="p_row"></param>
        /// <returns></returns>
        public IEnumerable<AaOrgModel> AaOrgQuery(AaOrgModel p_ViewModel)
        {
            List<AaOrgModel> m_result = new List<AaOrgModel>();
            try
            {
                string m_sql = @" SELECT a.* FROM ZT_AaOrg a Where 1=1 ";

                List<DBParameter> m_paraList = new List<DBParameter>();
                //org_uuid
                if (!string.IsNullOrEmpty(p_ViewModel.org_uuid) && p_ViewModel.org_uuid.ToString() != Guid.Empty.ToString())
                {
                    m_sql += @" AND a.org_uuid =@org_uuid ";
                    m_paraList.Add(new DBParameter("@org_uuid", p_ViewModel.org_uuid));
                }

                //user_id
                if (!string.IsNullOrEmpty(p_ViewModel.org_id))
                {
                    m_sql += " AND a.org_id=@org_id ";
                    m_paraList.Add(new DBParameter("@org_id", p_ViewModel.org_id));
                }

                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                m_result = BLHelper.DataTableToList<AaOrgModel>(m_dataTable);

                //AaOrgBE m_be;
                //for (int i = 0; i < m_dataTable.Rows.Count; i++)
                //{
                //    m_be = genBE(m_dataTable.Rows[i]);
                //    m_result.Add(m_be);
                //}

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        /// <summary>
        /// 查詢角色清單
        /// </summary>
        /// <param name="p_varChar01">ROLE:角色參數</param>
        /// <param name="p_isAdminGroup">true:管理者群組, false:非管理者群組</param>
        /// <param name="p_isAdmin">true:登入者角色為 ADMIN, false:登入者角色非 ADMIN</param>
        /// <returns></returns>
        public IEnumerable<string[]> QueryAllRole(string p_varChar01, bool p_isAdminGroup, bool p_isAdmin)
        {
            List<string[]> m_list = new List<string[]>();
            string m_sql = @"SELECT c.code_uuid, c.code_id, c.code_name,*
FROM ZT_SysCodeInfo c
INNER JOIN ZT_SysCodeInfo c2 ON c.super_uuid=c2.code_uuid AND c2.status_flag='Y'
WHERE c.status_flag='Y' AND c.var_char01=@var_char01 ";

            //非管理群組
            if (!p_isAdminGroup)
            {
                m_sql += " AND c2.var_char02<>'DEFAULT' ";
            }

            //非 ADMIN 角色
            if (!p_isAdmin)
            {
                m_sql += " AND c2.var_char02<>'ADMIN' ";
            }

            m_sql += " ORDER BY c2.var_char02, c.code_id ";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@var_char01", p_varChar01));
            DataTable m_dataTable = g_dba.GetDataTable(m_sql.ToString(), m_paraList.ToArray());
            List<string> m_string;
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                m_string = new List<string>();
                m_string.Add(m_dataTable.Rows[i]["code_uuid"].ToString());
                m_string.Add(m_dataTable.Rows[i]["code_id"].ToString());
                m_string.Add(m_dataTable.Rows[i]["code_name"].ToString());
                m_list.Add(m_string.ToArray());
            }
            return m_list;
        }

        public string GetOrgId(string p_groupId)
        {
            string m_return = string.Empty;
            string m_sql = @"SELECT CASE WHEN c2.var_char02 in ('ADMIN','DEFAULT') THEN '822' ELSE c2.var_char02 END AS org_id
FROM ZT_SysCodeInfo c
INNER JOIN ZT_SysCodeInfo c2 ON c.cate=c2.code_id AND c2.cate='ORG_PARM'
WHERE c.code_id=@group_id ";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@group_id", p_groupId));
            DataTable m_dataTable = g_dba.GetDataTable(m_sql.ToString(), m_paraList.ToArray());
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                m_return = m_dataTable.Rows[0]["org_id"].ToString();
            }
            return m_return;
        }

        #endregion

        //#region private Methods

        ///// <summary>
        ///// 將DataRow轉成Entity
        ///// </summary>
        ///// <param name="p_row"></param>
        ///// <returns></returns>
        //private AaOrgModel genBE(DataRow p_row)
        //{
        //    AaOrgBE m_be = new AaOrgBE();
        //    if (!p_row.IsNull("org_uuid")
        //        && p_row["org_uuid"].ToString() != Guid.Empty.ToString())
        //        m_be.org_uuid = p_row["org_uuid"].ToString();
        //    if (!p_row.IsNull("org_type"))
        //        m_be.org_type = (string)p_row["org_type"];
        //    if (!p_row.IsNull("org_id"))
        //        m_be.org_id = (string)p_row["org_id"];
        //    if (!p_row.IsNull("org_name"))
        //        m_be.org_name = (string)p_row["org_name"];

        //    if (!p_row.IsNull("created_by"))
        //        m_be.created_by = p_row["created_by"].ToString();
        //    if (!p_row.IsNull("created_date"))
        //        m_be.created_date = (DateTime)p_row["created_date"];
        //    if (!p_row.IsNull("updated_by"))
        //        m_be.updated_by = p_row["updated_by"].ToString();
        //    if (!p_row.IsNull("updated_date"))
        //        m_be.updated_date = (DateTime)p_row["updated_date"];

        //    return m_be;
        //}

        //#endregion

    }
}
