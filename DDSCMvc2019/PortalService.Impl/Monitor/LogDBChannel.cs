using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.DES;

namespace PortalService.Impl.Monitor
{
    public class LogDBChannel : IMonitorChannel
    {

        public void ChannelProcess(string function_code, string level_code, string message, Guid userUuid)
        {
            string sql = @"INSERT INTO [ZT_MonitorLog] (level_code,function_code
,message,user_uuid,created_date) VALUES(@level_code,@function_code,@message,@user_uuid,getdate())";
            try
            {
                string m_ConnectionString = ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString;
                string m_ErrorMessage = string.Empty;
                using (SqlConnection connection = new SqlConnection(DESCode.desDecryptBase64(m_ConnectionString, ref m_ErrorMessage)))
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@level_code", level_code);
                    cmd.Parameters.AddWithValue("@function_code", function_code);
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters.AddWithValue("@user_uuid", userUuid);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
