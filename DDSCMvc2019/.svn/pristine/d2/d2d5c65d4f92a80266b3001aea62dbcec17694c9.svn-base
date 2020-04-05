using CommonLibrary.DBA;
using Entity.SYS;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PortalService.FlowProcess
{
    public class CommonApprove : IFlowProcess
    {
        private static ILog logger = LogManager.GetLogger(typeof(IFlowProcess));

        public FlwRtn flowApprove(FlwJobBE job, Guid userId, SysProgramBE program, DBASqlLog m_dba)
        {
            FlwRtn m_return = new FlwRtn();
            string m_ErrorMsg = CheckDataAvailable(job, userId, program, m_dba);

            if (false == string.IsNullOrWhiteSpace(m_ErrorMsg))
            {
                //送審資料狀態已被更新，不可覆核
                m_return.isSuccess = false;
                m_return.rtnMessage = m_ErrorMsg;

                return m_return;
            }
            else
            {
                string m_sql = "UPDATE " + program.table_name + " SET status_flag = @flag, updated_date=getdate(), updated_by=@updated_by WHERE " + program.key_name + " = @key";
                List<DBParameter> m_paraList = new List<DBParameter>();
                if (job.flw_type == "D")//刪除註記
                {
                    m_paraList.Add(new DBParameter("@flag", "N"));
                    m_paraList.Add(new DBParameter("@key", job.data_uuid));
                    m_paraList.Add(new DBParameter("@updated_by", userId));
                    m_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());
                    m_return.isSuccess = true;
                }
                else if (job.flw_type == "A")
                {
                    m_paraList.Add(new DBParameter("@flag", "Y"));
                    m_paraList.Add(new DBParameter("@key", job.data_uuid));
                    m_paraList.Add(new DBParameter("@updated_by", userId));
                    m_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());
                    m_return.isSuccess = true;
                }
                else
                {
                    Type m_type = Type.GetType(string.Format("{0}, Entity", program.func_entity));
                    Object m_obj = Xml2Object(job.flwJobData.orginal_data, m_type);
                    PropertyInfo[] m_property = m_obj.GetType().GetProperties();
                    string[] m_fieldName = null;
                    for (int i = 0; i < m_property.Length; i++)
                    {
                        if (m_property[i].Name == "DATA_FIELD")
                        {
                            m_fieldName = (string[])m_property[i].GetValue(m_obj);
                            break;
                        }
                    }
                    if (m_fieldName != null)
                    {
                        StringBuilder m_sb = new StringBuilder();
                        m_sb.Append("UPDATE " + program.table_name + " SET ");
                        for (int i = 0; i < m_fieldName.Length; i++)
                        {
                            if (m_fieldName[i] != program.key_name)
                            {
                                m_sb.Append(m_fieldName[i]);
                                m_sb.Append("=@");
                                m_sb.Append(m_fieldName[i]);
                                if (i != m_fieldName.Length - 1)
                                {
                                    m_sb.Append(",");
                                }
                            }

                            for (int j = 0; j < m_property.Length; j++)
                            {
                                if (m_fieldName[i] != program.key_name)
                                {
                                    if (m_fieldName[i] == m_property[j].Name)
                                    {
                                        m_paraList.Add(new DBParameter("@" + m_fieldName[i], m_property[j].GetValue(m_obj)));
                                    }
                                }
                            }
                        }
                        m_sb.Append(" WHERE ");
                        m_sb.Append(program.key_name + " = @key");
                        m_paraList.Add(new DBParameter("@key", job.data_uuid));

                        m_dba.ExecNonQueryTransaction(m_sb.ToString(), m_paraList.ToArray());
                        m_return.isSuccess = true;
                    }
                    else
                    {
                        m_return.isSuccess = false;
                        m_return.rtnMessage = "更新資料之設定有問題";
                    }
                }
            }

            return m_return;
        }

        public FlwRtn flowReject(FlwJobBE job, Guid userId, SysProgramBE program, DBASqlLog m_dba)
        {
            FlwRtn m_return = new FlwRtn();
            string m_sql;
            string m_ErrorMsg = CheckDataAvailable(job, userId, program, m_dba);

            if (false == string.IsNullOrWhiteSpace(m_ErrorMsg))
            {
                //送審資料狀態已被更新，不可覆核
                m_return.isSuccess = false;
                m_return.rtnMessage = m_ErrorMsg;
            }
            else
            {
                if (job.flw_type == "A")
                {
                    //新增:退回
                    m_sql = "UPDATE " + program.table_name + " SET status_flag = 'N' WHERE " + program.key_name + " = @key";
                }
                else
                {
                    m_sql = "UPDATE " + program.table_name + " SET status_flag = 'R' WHERE " + program.key_name + " = @key";
                }

                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@key", job.data_uuid));

                m_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());
                m_return.isSuccess = true;
            }

            return m_return;
        }

        /// <summary>
        /// 確認資料狀態是否可覆核，或是已被異動過
        /// </summary>
        private string CheckDataAvailable(FlwJobBE p_Job, Guid p_UserId, SysProgramBE p_Program, DBASqlLog p_Dba)
        {
            string m_ErrorMsg = string.Empty;
            string m_Sql = string.Empty;
            List<DBParameter> m_ParaList = null;
            DataTable m_Dt = null;

            m_Sql = string.Format(" SELECT 1 FROM {0} WHERE {1} = @key AND status_flag = 'V' ",
                p_Program.table_name, p_Program.key_name);
            m_ParaList = new List<DBParameter>();
            m_ParaList.Add(new DBParameter("@key", p_Job.data_uuid));

            m_Dt = p_Dba.GetDataTableTransaction(m_Sql, m_ParaList.ToArray());

            if (m_Dt == null || m_Dt.Rows.Count == 0)
            {
                m_ErrorMsg = "送審資料狀態已被更新，請重新確認";
            }

            return m_ErrorMsg;
        }

        /// <summary>
        /// Xml String To Object
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object Xml2Object(string xml, Type type)
        {
            XmlSerializer m_xmlSear = new XmlSerializer(type);
            StringReader m_sr = new StringReader(xml);
            XmlTextReader reader = new XmlTextReader(m_sr);
            object transferObj = m_xmlSear.Deserialize(reader);

            return transferObj;
        }
    }
}
