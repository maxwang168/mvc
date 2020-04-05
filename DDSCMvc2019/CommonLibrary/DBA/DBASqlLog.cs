using System; 
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using System.Collections;
using System.Security.Principal;

namespace CommonLibrary.DBA
{
    public class DBASqlLog
    {
        #region Θ跑计


        private bool blException;
        private Exception exProc;
        private int g_changedRecCount = 0;
        private DBAccess g_dba;
        private string g_table_name = string.Empty;
        private string g_prg_id = string.Empty;
        private string g_prg_name = string.Empty;
        private string g_action_mode = string.Empty;
        private string g_sql_content = string.Empty;
        private string g_status_code = string.Empty;
        private int g_rec_count = 0;
        private string g_user_ip = string.Empty;
        private string g_created_by = string.Empty;

        #endregion

        #region Property

        /// <summary>
        /// 戈钵笆掸计
        /// </summary>
        /// <remarks>
        /// 2012.10.29 Yvonne, added
        /// </remarks>
        public int changedRecCount
        {
            get { return g_changedRecCount; }
        }

        public Exception ex
        {
            get
            { return exProc; }
        }

        public bool isException
        {
            get
            { return blException; }
        }

        public bool isDuplicate
        {
            get
            {
                if (exProc == null)
                    return false;

                if (exProc.GetType().Name == "SqlException")
                {
                    if (((SqlException)exProc.GetBaseException()).Number == 2627)
                        return true;
                }
                return false;
            }
        }

        public string errMsg
        {
            get
            {
                if (exProc == null)
                    return "";
                else
                    return exProc.Message.ToString();
            }
        }

        public string table_name
        {
            get { return g_table_name; }
            set { g_table_name = value; }
        }

        public string prg_id
        {
            get { return g_prg_id; }
            set { g_prg_id = value; }
        }

        public string prg_name
        {
            get { return g_prg_name; }
            set { g_prg_name = value; }
        }

        public string action_mode
        {
            get { return g_action_mode; }
            set { g_action_mode = value; }
        }

        public string sql_content
        {
            get { return g_sql_content; }
            set { g_sql_content = value; }
        }

        public string status_code
        {
            get { return g_status_code; }
            set { g_status_code = value; }
        }

        public int rec_count
        {
            get { return g_rec_count; }
            set { g_rec_count = value; }
        }

        public string user_ip
        {
            get { return g_user_ip; }
            set { g_user_ip = value; }
        }

        public string created_by
        {
            get { return g_created_by; }
            set { g_created_by = value; }
        }

        #endregion

        #region 篶ㄧ计
        /*public DBASqlLog(int intSource)
        {
            g_dba = new DBAccess(intSource);
        }*/

        public DBASqlLog(string strConnectionString)
        {
            g_dba = new DBAccess(strConnectionString);
        }

        public DBASqlLog()
        {
            g_dba = new DBAccess();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string strSQL)
        {
            initVar();
            action_mode = "Query";
            DataTable m_return = new DataTable();

            try
            {
                m_return = g_dba.GetDataTable(strSQL);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                status_code = "Exception";
                throw ex;
            }
            finally
            {
                if (m_return != null)
                    rec_count = m_return.Rows.Count;
                else
                    rec_count = 0;

                sql_content = strSQL;
                addToSqlLog();
            }

            return m_return;
        }

        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string strSQL, DBParameter[] commandParameters)
        {
            action_mode = "Query";
            initVar();

            DataTable m_return = new DataTable();

            try
            {
                m_return = g_dba.GetDataTable(strSQL, commandParameters);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                status_code = "Exception";
                throw ex;
            }
            finally
            {
                if (m_return != null)
                    rec_count = m_return.Rows.Count;
                else
                    rec_count = 0;

                if (commandParameters != null)
                    sql_content = genParaStmt(commandParameters) + strSQL;
                else
                    sql_content = strSQL;

                addToSqlLog();
            }

            return m_return;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="strSQL"></param>
        ///// <returns></returns>
        ///// <remarks>
        ///// 2012.10.28 Yvonne, created
        ///// </remarks>
        public DataTable GetDataTableTransaction(string strSQL)
        {
            initVar();
            action_mode = "Query";
            DataTable m_return = new DataTable();

            try
            {
                m_return = g_dba.GetDataTableTransaction(strSQL);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                status_code = "Exception";
                throw ex;
            }
            finally
            {
                if (m_return != null)
                    rec_count = m_return.Rows.Count;
                else
                    rec_count = 0;

                sql_content = strSQL;
                addToSqlLog();
            }

            return m_return;


        }

        ///// <summary>
        ///// GetDataTableTransaction
        ///// </summary>
        ///// <param name="strSQL"></param>
        ///// <param name="commandParameters"></param>
        ///// <returns></returns>
        public DataTable GetDataTableTransaction(string strSQL, DBParameter[] commandParameters)
        {
            initVar();
            action_mode = "Query";
            DataTable m_return = new DataTable();

            try
            {
                m_return = g_dba.GetDataTableTransaction(strSQL, commandParameters);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                status_code = "Exception";
                throw ex;
            }
            finally
            {
                if (m_return != null)
                    rec_count = m_return.Rows.Count;
                else
                    rec_count = 0;

                if (commandParameters != null)
                    sql_content = genParaStmt(commandParameters) + strSQL;
                else
                    sql_content = strSQL;

                addToSqlLog();
            }

            return m_return;

        }

        /// <summary>
        /// GetData
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public string GetData(string strSQL)
        {
            action_mode = "Query";
            string m_return = string.Empty;

            initVar();

            try
            {
                m_return = g_dba.GetData(strSQL);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                g_changedRecCount = 0;
                throw ex;
            }
            finally
            {
                if (m_return != null)
                    rec_count = 1;
                else
                    rec_count = 0;

                sql_content = strSQL;

                addToSqlLog();
            }

            return m_return;
        }

        public string GetData(string strSQL, SqlParameter[] commandParameters)
        {
            action_mode = "Query";
            string m_return = string.Empty;

            initVar();

            try
            {

                m_return = g_dba.GetData(strSQL, commandParameters);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                g_changedRecCount = 0;
                throw ex;
            }
            finally
            {
                if (m_return != null)
                    rec_count = 1;
                else
                    rec_count = 0;

                if (commandParameters != null)
                    sql_content = genParaStmt(commandParameters) + strSQL;
                else
                    sql_content = strSQL;

                addToSqlLog();
            }

            return m_return;

        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="strSQL"></param>
        ///// <returns></returns>
        ///// <remarks>
        ///// 2012.08.24 Yvonne, created
        ///// </remarks>
        public string GetDataTransaction(string strSQL)
        {
            string m_return = string.Empty;
            action_mode = "Query";
            initVar();

            try
            {
                m_return = g_dba.GetDataTransaction(strSQL);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                g_changedRecCount = 0;
                status_code = "Exception";
                throw ex;
            }
            finally
            {
                if (m_return != null)
                    rec_count = 1;
                else
                    rec_count = 0;

                sql_content = strSQL;

                addToSqlLog();
            }

            return m_return;
        }

        ////NewMethod
        public DataSet GetDataSet(string[] sqls)
        {
            initVar();
            action_mode = "Query";
            DataSet m_return = new DataSet();
            string strSQL = string.Empty;
            try
            {
                m_return = g_dba.GetDataSet(sqls);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                status_code = "Exception";
                throw ex;
            }
            finally
            {
                for (int i = 0; i < sqls.Length; i++)
                {
                    strSQL += sqls[i].ToString() + ";";
                }

                if (m_return != null)
                {
                    for (int i = 0; i < m_return.Tables.Count; i++)
                    {
                        rec_count += m_return.Tables[i].Rows.Count;
                    }

                }
                else
                {
                    rec_count = 0;
                }

                sql_content = strSQL;

                addToSqlLog();
            }
            return m_return;
        }


        public void BeginTrans()
        {
            g_dba.BeginTrans();
        }

        public void Commit()
        {
            g_dba.Commit();
        }

        public void Rollback()
        {
            g_dba.Rollback();
        }

        ///// <summary>
        ///// ExecNonQuery
        ///// </summary>
        ///// <param name="strSQL"></param>
        ///// <returns></returns>
        public int ExecNonQuery(string strSQL)
        {
            initVar();

            try
            {
                g_changedRecCount = g_dba.ExecNonQuery(strSQL);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                g_changedRecCount = 0;
                status_code = "Exception";
                throw ex;
            }
            finally
            {
                rec_count = g_changedRecCount;
                sql_content = strSQL;

                if (strSQL.ToLower().Contains("insert"))
                {
                    action_mode = "Insert";
                }
                else if (strSQL.ToLower().Contains("update"))
                {
                    action_mode = "Update";
                }
                else if (strSQL.ToLower().Contains("delete"))
                {
                    action_mode = "Delete";
                }

                addToSqlLog();
            }

            return g_changedRecCount;

        }

        /// <summary>
        /// ExecNonQuery
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecNonQuery(string strSQL, DBParameter[] commandParameters)
        {
            initVar();

            try
            {
                g_changedRecCount = g_dba.ExecNonQuery(strSQL, commandParameters);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else if (g_dba.isDuplicate)
                {
                    status_code = "Faikl";
                }
                else
                {
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                g_changedRecCount = 0;
                status_code = "Exception";
                throw ex;
            }
            finally
            {
                rec_count = g_changedRecCount;
                if (commandParameters != null)
                    sql_content = genParaStmt(commandParameters) + strSQL;
                else
                    sql_content = strSQL;

                if (strSQL.ToLower().Contains("insert"))
                {
                    action_mode = "insert";
                }
                else if (strSQL.ToLower().Contains("update"))
                {
                    action_mode = "Update";
                }
                else if (strSQL.ToLower().Contains("delete"))
                {
                    action_mode = "Delete";
                }

                addToSqlLog();
            }

            return g_changedRecCount;

        }

        ///// <summary>
        ///// ExecNonQueryTransaction
        ///// </summary>
        ///// <param name="strSQL"></param>
        ///// <returns></returns>
        public bool ExecNonQueryTransaction(string strSQL)
        {
            bool m_return = false;

            initVar();

            try
            {
                m_return = g_dba.ExecNonQueryTransaction(strSQL);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    g_changedRecCount = 1;
                    m_return = true;
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                status_code = "Exception";
                throw ex;
            }
            finally
            {
                rec_count = g_changedRecCount;
                sql_content = strSQL;
                if (strSQL.ToLower().Contains("insert"))
                {
                    action_mode = "insert";
                }
                else if (strSQL.ToLower().Contains("update"))
                {
                    action_mode = "Update";
                }
                else if (strSQL.ToLower().Contains("delete"))
                {
                    action_mode = "Delete";
                }
                addToSqlLog();
            }

            return m_return;


        }

        ///// <summary>
        ///// ExecNonQueryTransaction
        ///// </summary>
        ///// <param name="strSQL"></param>
        ///// <param name="commandParameters"></param>
        ///// <returns></returns>
        public bool ExecNonQueryTransaction(string strSQL, DBParameter[] commandParameters)
        {
            bool m_return = false;

            initVar();

            try
            {
                m_return = g_dba.ExecNonQueryTransaction(strSQL, commandParameters);

                if (g_dba.isException)
                {
                    blException = true;
                    exProc = g_dba.ex;
                    status_code = "Exception";
                }
                else
                {
                    g_changedRecCount = 1;
                    m_return = true;
                    status_code = "Success";
                }
            }
            catch (Exception ex)
            {
                blException = true;
                exProc = ex;
                status_code = "Exception";
                throw ex;
            }
            finally
            {
                rec_count = g_changedRecCount;
                //if (commandParameters != null)
                //    sql_content = genParaStmt(commandParameters) + strSQL;
                //else
                //    sql_content = strSQL;

                //if (strSQL.ToLower().Contains("insert"))
                //{
                //    action_mode = "Insert";
                //}
                //else if (strSQL.ToLower().Contains("update"))
                //{
                //    action_mode = "Update";
                //}
                //else if (strSQL.ToLower().Contains("delete"))
                //{
                //    action_mode = "Delete";
                //}
                //addToSqlLog();
            }

            return m_return;
        }

        #endregion


        #region Private Methods
        /// <summary>
        /// 砞﹚跑计﹍
        /// </summary>
        private void initVar()
        {
            blException = false;
            exProc = null;
            g_changedRecCount = -1;
        }

        /// <summary>
        /// 盢把计皚锣ΘSQL粂猭
        /// </summary>
        /// <param name="paraList"></param>
        /// <returns></returns>
        private string genParaStmt(SqlParameter[] paraList)
        {
            string m_return = string.Empty;
            string m_declareStr = "Declare {0} {1}({2})";
            string m_declareNum = "Declare {0} {1}";
            string m_setStr = " Set {0} = '{1}'";
            string m_setNum = " Set {0} = {1}";

            for (int i = 0; i < paraList.Length; i++)
            {
                switch (paraList[i].DbType)
                {
                    case DbType.String:
                        m_return += string.Format(m_declareStr, paraList[i].ParameterName, paraList[i].SqlDbType, paraList[i].Size.ToString()) + "\n";
                        m_return += string.Format(m_setStr, paraList[i].ParameterName, paraList[i].Value.ToString()) + "\n";
                        break;

                    case DbType.Int16:
                    case DbType.Int32:
                    case DbType.Int64:
                        m_return += string.Format(m_declareNum, paraList[i].ParameterName, paraList[i].SqlDbType) + "\n";
                        m_return += string.Format(m_setNum, paraList[i].ParameterName, paraList[i].Value.ToString()) + "\n";
                        break;

                    default:
                        //ノ计Α
                        m_return += string.Format(m_declareNum, paraList[i].ParameterName, paraList[i].SqlDbType) + "\n";
                        //砞﹚よΑノ﹃
                        m_return += string.Format(m_setStr, paraList[i].ParameterName, paraList[i].Value.ToString()) + "\n";
                        break;
                }

            }

            return m_return;
        }

        /// <summary>
        /// 盢把计皚锣ΘSQL粂猭
        /// </summary>
        /// <param name="paraList"></param>
        /// <returns></returns>
        private string genParaStmt(DBParameter[] paraList)
        {
            string m_return = string.Empty;
            //string m_declareStr = "Declare {0} {1}({2})";
            string m_declareNum = "Declare {0} {1}";
            string m_setStr = " Set {0} = '{1}'";
            string m_setNum = " Set {0} = {1}";

            for (int i = 0; i < paraList.Length; i++)
            {
                switch (paraList[i].SqlDataType)
                {
                    case SqlDbType.Int:
                        m_return += string.Format(m_declareNum, paraList[i].Name, paraList[i].SqlDataType) + "\n";
                        m_return += string.Format(m_setNum, paraList[i].Name, paraList[i].Value.ToString()) + "\n";
                        break;

                    default:
                        //ノ计Α
                        m_return += string.Format(m_declareNum, paraList[i].Name, paraList[i].SqlDataType) + "\n";
                        //砞﹚よΑノ﹃
                        m_return += string.Format(m_setStr, paraList[i].Name, paraList[i].Value.ToString()) + "\n";
                        break;
                }

            }

            return m_return;
        }

        /// <summary>
        /// 盢磅︽SQL糶Log
        /// </summary>
        private void addToSqlLog()
        {
            /*if (HttpContext.Current.Session["table_name"] != null)
            {
                g_table_name = HttpContext.Current.Session["table_name"].ToString();
                HttpContext.Current.Session["table_name"] = null;
            }
            if (HttpContext.Current.Session["prg_id"] != null)
            {
                g_prg_id = HttpContext.Current.Session["prg_id"].ToString();
                HttpContext.Current.Session["prg_id"] = null;
            }
            if (HttpContext.Current.Session["prg_name"] != null)
            {
                g_prg_name = HttpContext.Current.Session["prg_name"].ToString();
                HttpContext.Current.Session["prg_name"] = null;
            }


            if (HttpContext.Current.Session["userData"] != null)
            {
                g_created_by = ((UserClass)HttpContext.Current.Session["userData"]).userInfo.user.ID;
                g_user_ip = ((UserClass)HttpContext.Current.Session["userData"]).userInfo.userIP;
            }


            string m_sql = "insert into sql_log (table_name, prg_id,prg_name, action_mode, sql_content, status_code, rec_count, user_ip, created_by) " +
                           "values(@table_name, @prg_id, @prg_name, @action_mode, @sql_content, @status_code, @rec_count, @user_ip, @created_by)";

            try
            {
                DBAccess m_dba = new DBAccess();

                SqlParameter[] m_sqlPara = { new System.Data.SqlClient.SqlParameter("@table_name", g_table_name),
                                     new System.Data.SqlClient.SqlParameter("@prg_id", g_prg_id), 
                                     new System.Data.SqlClient.SqlParameter("@prg_name", g_prg_name), 
                                     new System.Data.SqlClient.SqlParameter("@action_mode", g_action_mode), 
                                     new System.Data.SqlClient.SqlParameter("@sql_content", g_sql_content),
                                     new System.Data.SqlClient.SqlParameter("@status_code", g_status_code),
                                     new System.Data.SqlClient.SqlParameter("@rec_count", g_rec_count),
                                     new System.Data.SqlClient.SqlParameter("@user_ip", g_user_ip),
                                     new System.Data.SqlClient.SqlParameter("@created_by", g_created_by)};


                m_dba.ExecNonQuery(m_sql, m_sqlPara);
            }
            catch (Exception ex)
            {
                //ぃノ矪瞶
            }*/
        }

        #endregion

    }

}