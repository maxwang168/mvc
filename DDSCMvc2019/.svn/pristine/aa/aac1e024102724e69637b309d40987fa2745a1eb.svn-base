using CommonLibrary.DBA;
using Entity.COM;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace PortalService.Impl.BL
{
    public class CommonBL
    {
        #region 成員變數
        private ILog logger = LogManager.GetLogger(typeof(CommonBL));
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
        /// <param name=""></param>
        /// <returns></returns>
        public CustIDBE CustIDQuery(string p_id)
        {
            CustIDBE m_result = new CustIDBE();
            try
            {
                string m_sql = @"
SELECT id.org_name 
FROM ZT_ID id
WHERE id.org_id=@org_id
";
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@org_id", p_id));
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                m_result.CustID = p_id;
                if (m_dataTable.Rows.Count > 0)
                {
                    m_result.CustIDName = m_dataTable.Rows[0][0].ToString();
                    m_result.IsExist = true;
                    m_result.Description = m_result.CustIDName;
                }
                else
                {
                    m_result.IsExist = false;
                    m_result.Description = "客戶資料不存在";
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

        #region private Methods

        #endregion

    }
}
