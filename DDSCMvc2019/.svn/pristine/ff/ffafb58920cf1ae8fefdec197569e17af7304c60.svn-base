using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.DES;
using log4net;

namespace CommonLibrary.DBA
{
    public class DBAccess
    {
        #region 成員變數
        private SqlConnection g_conn;
        private bool g_isException;
        private bool g_isDBException;
        private bool g_isInTransaction;
        private Exception g_exProc;
        private SqlException g_dbExProc;
        private SqlTransaction g_transaction;
        private SqlCommand g_TransationCmd;

        private ILog logger = LogManager.GetLogger(typeof(DBAccess));

        #endregion

        #region Public Methods
        public bool isException
        {
            get
            {
                return g_isException;
            }
        }

        public bool isDBException
        {
            get
            {
                return g_isDBException;
            }
        }

        public bool isDuplicate
        {
            get
            {
                return false;
            }
        }
        public Exception ex
        {
            get
            {
                return g_dbExProc;
            }
        }

        public DBAccess()
        {
            g_conn = new SqlConnection();
            g_conn.ConnectionString = ConfigurationManager.AppSettings["StrConnection"];
            
        }

        public DBAccess(string p_connectionString)
        {
            var m_errorMessage = string.Empty;
            g_conn = new SqlConnection
            {
                ConnectionString = DESCode.desDecryptBase64(p_connectionString, ref m_errorMessage)
                //ConnectionString = (p_connectionString, ref m_errorMessage)
            };
        }

        /// <summary>
        /// SQL Query
        /// </summary>
        /// <param name="p_strSQL"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string p_strSQL)
        {
            InitVar();
            SqlDataAdapter m_da = new SqlDataAdapter();
            SqlCommand m_cmd = new SqlCommand();
            DataTable m_dt = new DataTable();
            m_cmd.CommandText = p_strSQL;
            m_cmd.Connection = g_conn;
            m_cmd.CommandTimeout = g_conn.ConnectionTimeout;
            m_da.SelectCommand = m_cmd;

            try
            {
                g_conn.Open();
                m_da.Fill(m_dt);
            }
            catch (SqlException ex)
            {
                ProcException(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                ProcException(ex);
                throw ex;
            }
            finally
            {
                m_cmd.Dispose();
                m_da.Dispose();
                g_conn.Close();
            }
            return m_dt;
        }

        public DataTable GetDataTable(string p_strSQL, DBParameter[] commandParameters)
        {
            InitVar();
            SqlDataAdapter m_da = new SqlDataAdapter();
            SqlCommand m_cmd = new SqlCommand();
            DataTable m_dt = new DataTable();
            m_cmd.CommandText = p_strSQL;
            m_cmd.Connection = g_conn;
            foreach (DBParameter commandParameter in commandParameters)
            {
                SqlParameter sqlParam = new SqlParameter(commandParameter.Name, commandParameter.Value);
                if (sqlParam.Direction == ParameterDirection.InputOutput && sqlParam.Value == null)
                {
                    sqlParam.Value = (object)DBNull.Value;
                }
                m_cmd.Parameters.Add(sqlParam);
            }
            m_da.SelectCommand = m_cmd;
            try
            {
                this.g_conn.Open();
                m_da.Fill(m_dt);
            }
            catch (Exception ex)
            {
                ProcException(ex);
                throw ex;
            }
            finally
            {
                m_cmd.Dispose();
                m_da.Dispose();
                g_conn.Close();
            }
            return m_dt;
        }

        public DataTable GetDataTableTransaction(string p_strSQL)
        {
            InitVar();
            SqlDataAdapter m_da = new SqlDataAdapter();
            DataTable m_dt = new DataTable();
            g_TransationCmd.CommandText = p_strSQL;
            m_da.SelectCommand = g_TransationCmd;
            try
            {
                m_da.Fill(m_dt);
            }
            catch (Exception ex)
            {
                ProcException(ex);
                throw ex;
            }
            finally
            {
                
            }
            return m_dt;
        }

        public DataTable GetDataTableTransaction(string p_strSQL, DBParameter[] p_commandParameters)
        {
            InitVar();
            g_TransationCmd.Parameters.Clear();
            SqlDataAdapter m_da = new SqlDataAdapter();
            DataTable m_dt = new DataTable();
            g_TransationCmd.CommandText = p_strSQL;
            if (p_commandParameters != null)
            {
                foreach (DBParameter commandParameter in p_commandParameters)
                {
                    SqlParameter sqlParam = new SqlParameter(commandParameter.Name, commandParameter.Value);
                    sqlParam.Direction = ParameterDirection.Input;
                    if ((sqlParam.Direction == ParameterDirection.Input) && (commandParameter.Value == null))
                    {
                        sqlParam.Value = DBNull.Value;
                    }
                    g_TransationCmd.Parameters.Add(sqlParam);
                }
            }
            m_da.SelectCommand = g_TransationCmd;
            try
            {
                m_da.Fill(m_dt);
            }
            catch (Exception ex)
            {
                ProcException(ex);
                throw ex;
            }
            finally
            {
                
            }
            return m_dt;
        }

        public string GetData(string p_strSQL)
        {
            InitVar();
            string str = "";
            SqlCommand m_cmd = new SqlCommand();
            m_cmd.CommandText = p_strSQL;
            m_cmd.Connection = g_conn;
            try
            {
                g_conn.Open();
                str = m_cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                ProcException(ex);
                throw ex;
            }
            finally
            {
                m_cmd.Dispose();
                g_conn.Close();
            }
            return str;
        }

        public string GetData(string p_strSQL, SqlParameter[] commandParameters)
        {
            InitVar();
            string str = "";
            SqlCommand m_cmd = new SqlCommand();
            m_cmd.CommandText = p_strSQL;
            m_cmd.Connection = g_conn;
            foreach (SqlParameter commandParameter in commandParameters)
            {
                if (commandParameter.Direction == ParameterDirection.InputOutput && commandParameter.Value == null)
                {
                    commandParameter.Value = (object)DBNull.Value;
                }
                m_cmd.Parameters.Add(commandParameter);
            }
            try
            {
                g_conn.Open();
                if (m_cmd.ExecuteScalar() != null)
                {
                    str = m_cmd.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                ProcException(ex);
                throw ex;
            }
            finally
            {
                m_cmd.Dispose();
                g_conn.Close();
            }
            return str;
        }

        public string GetDataTransaction(string p_strSQL)
        {
            InitVar();
            string str = "";
            g_TransationCmd.CommandText = p_strSQL;
            try
            {
                var data = g_TransationCmd.ExecuteScalar();
                if (data != null)
                {
                    str = data.ToString();
                }
            }
            catch (Exception ex)
            {
                ProcException(ex);
                throw ex;
            }
            finally
            {
                
            }
            return str;
        }

        public string GetDataTransaction(string p_strSQL, DBParameter[] p_commandParameters)
        {
            InitVar();
            g_TransationCmd.Parameters.Clear();
            foreach (DBParameter para in p_commandParameters)
            {
                SqlParameter sqlParam = new SqlParameter(para.Name, para.Value);
                sqlParam.Direction = ParameterDirection.InputOutput;
                if ((sqlParam.Direction == ParameterDirection.InputOutput) && (para.Value == null))
                {
                    sqlParam.Value = DBNull.Value;
                }
                this.g_TransationCmd.Parameters.Add(sqlParam);
            }
            string str = "";
            try
            {
                g_TransationCmd.CommandText = p_strSQL;
                var data = g_TransationCmd.ExecuteScalar();
                if (data != null)
                {
                    str = data.ToString();
                }
            }
            catch (Exception ex)
            {
                ProcException(ex);
                throw ex;
            }
            finally
            {

            }
            return str;
        }

        /// <summary>
        /// SQL Query
        /// </summary>
        /// <param name="p_strSQL"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string[] p_strSQL)
        {
            InitVar();
            if (p_strSQL == null || p_strSQL.Length == 0)
            {
                return null;
            }
            string m_sql = p_strSQL[0];
            for (int i = 1; i < p_strSQL.Length; i++)
            {
                m_sql += ";" + p_strSQL[i];
            }
            DataSet m_ds = new DataSet();
            SqlCommand m_cmd = new SqlCommand();
            try
            {
                if (g_conn.State == ConnectionState.Closed)
                {
                    g_conn.Open();
                }
                m_cmd.Connection = g_conn;
                m_cmd.CommandText = m_sql;
                using (SqlDataAdapter da = new SqlDataAdapter(m_cmd))
                {
                    da.Fill(m_ds);
                }
                return m_ds;
            }
            catch (Exception ex)
            {
                ProcException(ex);
                throw ex;
            }
            finally
            {
                m_cmd.Dispose();
                g_conn.Close();
            }
        }

        /// <summary>
        /// SQL Execute
        /// </summary>
        /// <param name="p_strSQL"></param>
        /// <param name="p_commandParameters"></param>
        /// <returns></returns>
        public int ExecNonQuery(string p_strSQL)
        {
            InitVar();
            int m_intUpdated;
            SqlCommand m_cmd = new SqlCommand();
            m_cmd.CommandText = p_strSQL;

            try
            {
                g_conn.Open();
                m_cmd.Connection = g_conn;
                m_cmd.CommandTimeout = g_conn.ConnectionTimeout;
                m_intUpdated = m_cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                m_intUpdated = 0;
                ProcException(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                m_intUpdated = 0;
                ProcException(ex);
                throw ex;
            }
            finally
            {
                g_conn.Close();
            }

            return m_intUpdated;
        }

        /// <summary>
        /// SQL Execute
        /// </summary>
        /// <param name="p_strSQL"></param>
        /// <param name="p_commandParameters"></param>
        /// <returns></returns>
        public int ExecNonQuery(string p_strSQL, DBParameter[] p_commandParameters)
        {
            InitVar();
            int m_intUpdated;
            SqlCommand m_cmd = new SqlCommand();
            m_cmd.CommandText = p_strSQL;

            foreach (DBParameter para in p_commandParameters)
            {
                SqlParameter sqlParam = new SqlParameter(para.Name, para.Value);
                if ((sqlParam.Direction == ParameterDirection.InputOutput) && (para.Value == null))
                {
                    sqlParam.Value = DBNull.Value;
                }
                m_cmd.Parameters.Add(sqlParam);
            }
            try
            {
                g_conn.Open();
                m_cmd.Connection = g_conn;
                m_cmd.CommandTimeout = g_conn.ConnectionTimeout;
                m_intUpdated = m_cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                m_intUpdated = 0;

                g_isException = true;
                g_isDBException = true;
                g_dbExProc = ex;

                ProcException(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                m_intUpdated = 0;

                g_isException = true;
                g_exProc = ex;

                ProcException(ex);
            }
            finally
            {
                g_conn.Close();
            }

            return m_intUpdated;
        }

        public bool ExecNonQueryTransaction(string strSQL)
        {
            InitVar();
            bool flag;
            try
            {
                this.g_TransationCmd.CommandText = strSQL;
                this.g_TransationCmd.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception ex)
            {
                ProcException(ex);
                flag = false;
                throw ex;
            }
            finally
            {
                
            }
            return flag;
        }

        public bool ExecNonQueryTransaction(string strSQL, DBParameter[] commandParameters)
        {
            InitVar();
            g_TransationCmd.Parameters.Clear();
            foreach (DBParameter para in commandParameters)
            {
                SqlParameter sqlParam = new SqlParameter(para.Name, para.Value);
                sqlParam.Direction = ParameterDirection.Input;
                if ((sqlParam.Direction == ParameterDirection.Input) && (para.Value == null))
                {
                    sqlParam.Value = DBNull.Value;
                }
                this.g_TransationCmd.Parameters.Add(sqlParam);
            }
            bool flag;
            try
            {
                this.g_TransationCmd.CommandText = strSQL;
                this.g_TransationCmd.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception ex)
            {
                ProcException(ex);
                flag = false;
                throw ex;
            }
            finally
            {
                
            }
            return flag;
        }

        public void BeginTrans()
        {
            this.g_conn.Open();
            this.g_transaction = g_conn.BeginTransaction();
            this.g_TransationCmd = this.g_conn.CreateCommand();
            this.g_TransationCmd.Transaction = this.g_transaction;
            this.g_isInTransaction = true;
        }

        public void Commit()
        {
            if (!this.g_isInTransaction)
            {
                throw new Exception("Please start transaction first");
            }
            this.g_transaction.Commit();
            this.g_conn.Close();
            this.g_isInTransaction = false;
            this.g_transaction.Dispose();
            //this.scmComm.Dispose();
        }

        public void Rollback()
        {
            if (!this.g_isInTransaction)
            {
                throw new Exception("Please start transaction first");
            }
            this.g_transaction.Rollback();
            this.g_isInTransaction = false;
            this.g_transaction.Dispose();
            this.g_conn.Close();
            //this.scmComm.Dispose();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 變數初始化
        /// </summary>
        private void InitVar()
        {
            g_isException = false;
            g_isDBException = false;
        }

        /// <summary>
        /// SQL Exception
        /// </summary>
        /// <param name="p_ex"></param>
        private void ProcException(SqlException p_ex)
        {
            g_isException = true;
            g_isDBException = true;
            g_dbExProc = p_ex;

            logger.Error(p_ex.Message, p_ex);
        }

        /// <summary>
        /// Exception
        /// </summary>
        /// <param name="p_ex"></param>
        private void ProcException(Exception p_ex)
        {
            g_isException = true;
            g_exProc = p_ex;

            logger.Error(p_ex.Message, p_ex);
        }
        
        #endregion
    }
}
