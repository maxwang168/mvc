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
    public class SysGroupProgramBL
    {
        #region 成員變數

        private ILog logger = LogManager.GetLogger(typeof(SysGroupProgramBL));
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
        public IEnumerable<SysGroupProgramBE> SysGroupProgramQuery(SysGroupProgramModel p_viewModel)
        {
            List<SysGroupProgramBE> m_result = new List<SysGroupProgramBE>();

            string m_sql = @"SELECT ROW_NUMBER() OVER (ORDER BY p1.seq_no,p.seq_no) AS sn
, p.func_uuid, p.func_id, p.func_name, p.func_type,g.group_uuid, g.group_id
, CASE WHEN gp.group_uuid IS NULL THEN 'N' ELSE 'Y' END AS status_flag
FROM ZT_SysProgram p
INNER JOIN ZT_SysProgram p1 ON p.super_uuid=p1.func_uuid
LEFT JOIN ZT_SysGroup g ON g.group_uuid=@group_uuid
LEFT JOIN ZT_SysGroupProgram gp ON gp.group_uuid=g.group_uuid and gp.func_uuid=p.func_uuid
WHERE p.status_flag='Y' AND p.func_type = 'F'
ORDER BY p1.seq_no,p.seq_no";

            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@group_uuid", p_viewModel.GroupUuid));
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    SysGroupProgramBE m_code = genSysGroupProgramBE(m_dataTable.Rows[i]);
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

        public bool modifySysGroupProgramBE(List<SysGroupProgramBE> p_listVM)
        {
            bool m_success = false;
            //_LogWriter.BeginTrans();
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@group_uuid", p_listVM[0].GroupUuid));

            try
            {
                string m_sql = @"DELETE FROM ZT_SysGroupProgram WHERE group_uuid=@group_uuid";

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray()); //_LogWriter.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());

                //if (m_success)
                //{
                m_sql = @"
INSERT INTO ZT_SysGroupProgram (permission_uuid,group_uuid,func_uuid,status_flag,created_by,created_date,updated_by,updated_date) 
                VALUES (@permission_uuid,@group_uuid,@func_uuid,@status_flag,@created_by,@created_date,@updated_by,@updated_date)";

                for (int i = 0; i < p_listVM.Count; i++)
                {
                    List<DBParameter> m_paraList2 = new List<DBParameter>();
                    m_paraList2.Add(new DBParameter("@permission_uuid", Guid.NewGuid()));
                    m_paraList2.Add(new DBParameter("@group_uuid", p_listVM[i].GroupUuid));
                    m_paraList2.Add(new DBParameter("@func_uuid", p_listVM[i].FuncUuid));
                    m_paraList2.Add(new DBParameter("@status_flag", "Y"));
                    m_paraList2.Add(new DBParameter("@created_by", p_listVM[i].CreatedBy));
                    m_paraList2.Add(new DBParameter("@created_date", DateTime.Now));
                    m_paraList2.Add(new DBParameter("@updated_by", p_listVM[i].UpdatedBy));
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
                //}
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                m_success = false;
            }
            //finally
            //{
            //    if (m_success)
            //    {
            //        _LogWriter.Commit();
            //    }
            //    else
            //    {
            //        _LogWriter.Rollback();
            //    }
            //}
            return m_success;
        }

        #endregion

        #region Private Methods
        private SysGroupProgramBE genSysGroupProgramBE(DataRow p_row)
        {
            SysGroupProgramBE m_result = new SysGroupProgramBE();
            if (p_row["sn"] != DBNull.Value)
            {
                m_result.Sn = Convert.ToInt32(p_row["sn"]);
            }
            if (p_row["func_uuid"] != DBNull.Value)
            {
                m_result.FuncUuid = (Guid)p_row["func_uuid"];
            }
            if (p_row["func_id"] != DBNull.Value)
            {
                m_result.FuncId = (string)p_row["func_id"];
            }
            if (p_row["func_name"] != DBNull.Value)
            {
                m_result.FuncName = (string)p_row["func_name"];
            }
            if (p_row["func_type"] != DBNull.Value)
            {
                m_result.FuncType = (string)p_row["func_type"];
            }
            if (p_row["group_uuid"] != DBNull.Value)
            {
                m_result.GroupUuid = (Guid)p_row["group_uuid"];
            }
            if (p_row["group_id"] != DBNull.Value)
            {
                m_result.GroupId = (string)p_row["group_id"];
            }
            if (p_row["status_flag"] != DBNull.Value)
            {
                m_result.StatusFlag = p_row["status_flag"].ToString() == "Y" ? true : false;
            }

            switch (m_result.FuncType)
            {
                case "A":
                    m_result.FuncTypeName = "系統";
                    break;

                case "M":
                    m_result.FuncTypeName = "選單";
                    break;

                case "F":
                    m_result.FuncTypeName = "程式";
                    break;

                default:
                    m_result.FuncTypeName = m_result.FuncType;
                    break;
            }

            return m_result;
        }
        #endregion
    }
}
