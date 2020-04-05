using CommonLibrary.DBA;
using Entity.COM;
using log4net;
using PortalService.Contract;
using PortalService.Impl.BL;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using CommonLibrary.DES;

namespace PortalService.Impl
{
    public class ComService : IComService
    {
        private ILog logger = LogManager.GetLogger(typeof(ComService));
        private DBASqlLog g_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);
        private CommonBL g_CommonBL = new CommonBL();

        #region CommonBL
        public CustIDBE CustIDQuery(string p_id)
        {
            return g_CommonBL.CustIDQuery(p_id);
        }
        #endregion

        /// <summary>
        /// 產生交易序號
        /// </summary>
        /// <param name="p_sn_key"></param>
        /// <param name="p_sn_date"></param>
        /// <param name="p_recycle"></param>
        /// <returns></returns>
        public string generateSN(string p_sn_key, string p_sn_date)
        {
            string sn = null;
            string sqlStatement = "[dbo].[ZP_spGetNextSN]";
            SqlConnection p_conn = null;
            // Create a suitable command type and add the required parameter
            SqlCommand sqlCmd = null;
            SqlDataReader reader = null;

            try
            {
                //p_conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);
                string m_ConnectionString = ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString;
                string m_ErrorMessage = string.Empty;
                p_conn = new SqlConnection(DESCode.desDecryptBase64(m_ConnectionString, ref m_ErrorMessage));

                sqlCmd = new SqlCommand();
                sqlCmd.Connection = p_conn;
                sqlCmd.CommandText = sqlStatement;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@Q_SN_KEY", p_sn_key);
                sqlCmd.Parameters.AddWithValue("@Q_RECYCLE", string.Empty);
                sqlCmd.Parameters.AddWithValue("@Q_SN_DATE", p_sn_date);

                p_conn.Open();
                reader = sqlCmd.ExecuteReader();
                if (reader.Read())
                {
                    int serno = Convert.ToInt32(reader[0]);
                    sn = serno.ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                //throw ex;
            }
            finally
            {
                try
                {
                    if (p_conn != null)
                    {
                        p_conn.Close();
                        p_conn.Dispose();
                        p_conn = null;
                    }

                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                        reader = null;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                    new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                }
            }

            return sn;
        }
    }
}
