using CommonLibrary.DBA;
using Entity.SYS;
using log4net;
using PortalService.Contract;
using PortalService.Contract.ViewModel.System;
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
    public class UserProcess : IFlowProcess
    {
        private static ILog logger = LogManager.GetLogger(typeof(UserProcess));
        private Type g_sysTaskType = Type.GetType("PortalService.Impl.SysService,PortalService.Impl");
        private Type g_notifyTaskType = Type.GetType("PortalService.Impl.NotifyService,PortalService.Impl");
        private Type g_flowTaskType = Type.GetType("PortalService.Impl.FlowService,PortalService.Impl");
        private Type g_aaSysTaskType = Type.GetType("PortalService.Impl.AaSysService,PortalService.Impl");

        public FlwRtn flowApprove(FlwJobBE job, Guid userId, SysProgramBE program, DBASqlLog p_dba)
        {
            FlwRtn m_return = new FlwRtn();
            string m_ErrorMsg = CheckDataAvailable(job, userId, program, p_dba);

            if (false == string.IsNullOrWhiteSpace(m_ErrorMsg))
            {
                //送審資料狀態已被更新，不可覆核
                m_return.isSuccess = false;
                m_return.rtnMessage = m_ErrorMsg;
            }
            else
            {
                string m_sql;
                List<DBParameter> m_paraList = new List<DBParameter>();
                Type m_type = Type.GetType(string.Format("{0}, Entity", program.func_entity));
                switch (job.flw_type)
                {
                    case "A": //新增
                        m_sql = "UPDATE " + program.table_name + " SET status_flag = 'Y', updated_date = GETDATE(), updated_by = @updated_by WHERE " + program.key_name + " = @key";
                        m_paraList.Add(new DBParameter("@updated_by", userId));
                        m_paraList.Add(new DBParameter("@key", job.data_uuid));
                        p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());

                        notifyNT011(job, userId);
                        m_return.isSuccess = true;
                        break;

                    case "D": //刪除註記
                        m_sql = "UPDATE " + program.table_name + " SET status_flag = 'D', updated_date = GETDATE(), updated_by = @updated_by WHERE " + program.key_name + " = @key";
                        m_paraList.Add(new DBParameter("@updated_by", userId));
                        m_paraList.Add(new DBParameter("@key", job.data_uuid));

                        p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());
                        m_return.isSuccess = true;
                        break;

                    case "R": //密碼重送
                        var m_PwdEncrypt = string.Empty;
                        var m_FlwRtn = ResetPw(job, userId, p_dba, out m_PwdEncrypt);
                        if (m_FlwRtn.isSuccess)
                        {
                            notifyNT012(job, userId, m_PwdEncrypt);
                        }
                        m_return.isSuccess = true;
                        break;

                    case "U": //帳帳解除鎖定
                        m_sql = "UPDATE " + program.table_name + " SET status_flag = 'Y', pwd_status = 'Y', retry = '0' WHERE " + program.key_name + " = @key";
                        m_paraList.Add(new DBParameter("@key", job.data_uuid));

                        p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());

                        //AaUser m_be = getDataBE(job.job_uuid);
                        //notifyNT012(job, userId, m_be.pwd_encrypt);

                        m_return.isSuccess = true;
                        break;

                    default:
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

                            p_dba.ExecNonQueryTransaction(m_sb.ToString(), m_paraList.ToArray());
                            m_return.isSuccess = true;
                        }
                        else
                        {
                            m_return.isSuccess = false;
                            m_return.rtnMessage = "更新資料之設定有問題";
                        }
                        //m_sql = "UPDATE " + program.table_name + " SET status_flag = 'Y' WHERE " + program.key_name + " = @key";
                        //m_paraList.Add(new DBParameter("@key", job.data_uuid));

                        //p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());

                        break;
                }
            }

            return m_return;
        }

        public FlwRtn flowReject(FlwJobBE job, Guid userId, SysProgramBE program, DBASqlLog m_dba)
        {
            FlwRtn m_return = new FlwRtn();
            string m_ErrorMsg = CheckDataAvailable(job, userId, program, m_dba);

            if (false == string.IsNullOrWhiteSpace(m_ErrorMsg))
            {
                //送審資料狀態已被更新，不可覆核
                m_return.isSuccess = false;
                m_return.rtnMessage = m_ErrorMsg;
            }
            else
            {
                List<DBParameter> m_paraList = new List<DBParameter>();

                string m_sql = "UPDATE " + program.table_name + " SET status_flag = 'R' WHERE " + program.key_name + " = @key";
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

        private AaUser getDataBE(Guid p_JobUuid)
        {
            IFlowService process = (IFlowService)Activator.CreateInstance(g_flowTaskType);
            string m_orgData = process.QueryFlwOrgData(p_JobUuid);

            return (AaUser)Xml2Object(m_orgData, Type.GetType("Entity.SYS.AaUser, Entity"));
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

        /// <summary>
        /// 使用者申請完成通知
        /// </summary>
        /// <param name="job"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool notifyNT011(FlwJobBE job, Guid userId)
        {
            bool m_return = false;

            try
            {
                ISysService sysProcess = (ISysService)Activator.CreateInstance(g_sysTaskType);
                SysCodeInfoBE m_sysBE = sysProcess.QuerySysCodeInfoForFile("FILE_PATH", "CSFMPortal_WEB");
                AaUser m_be = getDataBE(job.job_uuid);

                DateTime m_now = DateTime.Now;
                NotifySendModel sendModel = new NotifySendModel();
                sendModel.NotifyCodeId = "NT011"; //使用者申請完成通知
                sendModel.UserUuid = userId;
                sendModel.ContactUserUuid = m_be.user_uuid;
                sendModel.IsSubscription = false;
                sendModel.DataXml = string.Format("<Notify><Date>{0}</Date><Time>{1}</Time><Id>{2}</Id><Name>{3}</Name><Reset>N</Reset><Url>{4}</Url><Attachement FileName=\"密碼函.zip\">{5}</Attachement></Notify>",
                    m_now.ToShortDateString(), m_now.ToString("HH:mm:ss"), m_be.org_id, m_be.user_name, m_sysBE.VarChar01, m_be.pwd_encrypt);

                INotifyService process = (INotifyService)Activator.CreateInstance(g_notifyTaskType);
                m_return = process.NotifySend(sendModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return m_return;
        }

        /// <summary>
        /// 密碼重設
        /// </summary>
        /// <param name="job"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool notifyNT012(FlwJobBE job, Guid userId, string p_pwdEncrypt)
        {
            bool m_return = false;

            try
            {
                ISysService sysProcess = (ISysService)Activator.CreateInstance(g_sysTaskType);
                SysCodeInfoBE m_sysBE = sysProcess.QuerySysCodeInfoForFile("FILE_PATH", "CSFMPortal_WEB");
                AaUser m_be = getDataBE(job.job_uuid);

                DateTime m_now = DateTime.Now;
                NotifySendModel sendModel = new NotifySendModel();
                sendModel.NotifyCodeId = "NT012"; //密碼重設
                sendModel.UserUuid = userId;
                sendModel.ContactUserUuid = m_be.user_uuid;
                sendModel.IsSubscription = false;
                sendModel.DataXml = string.Format("<Notify><Date>{0}</Date><Time>{1}</Time><Id>{2}</Id><Name>{3}</Name><Reset>N</Reset><Url>{4}</Url><Attachement FileName=\"密碼函.zip\">{5}</Attachement></Notify>",
                    m_now.ToShortDateString(), m_now.ToString("HH:mm:ss"), m_be.org_id, m_be.user_name, m_sysBE.VarChar01, p_pwdEncrypt);

                INotifyService process = (INotifyService)Activator.CreateInstance(g_notifyTaskType);
                m_return = process.NotifySend(sendModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return m_return;
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        private FlwRtn ResetPw(FlwJobBE job, Guid userId, DBASqlLog p_dba, out string m_PwdEncrypt)
        {
            var m_Return = new FlwRtn { isSuccess = true };
            m_PwdEncrypt = string.Empty;

            try
            {
                IAaSysService aaSysProcess = (IAaSysService)Activator.CreateInstance(g_aaSysTaskType);
                var m_AaUser = aaSysProcess.QueryAaUserByUidForResetPw(job.data_uuid);

                m_AaUser.pwd_status = "I"; //新建
                m_AaUser.pwd = string.Empty; //A碼 + B碼 (明碼, 待SQL加密)
                m_AaUser.pwd2 = string.Empty; //A碼 (明碼, 待SQL加密)
                m_AaUser.pwd3 = string.Empty; //B碼 (Encrypt)
                m_AaUser.pwd_encrypt = string.Empty; //密碼檔 (Base64)
                m_AaUser.pwd_modify_date = DateTime.Now;
                m_AaUser.status_flag = "Y"; //審核通過
                m_AaUser.retry = 0;
                m_AaUser.updated_by = userId;
                m_AaUser.updated_date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                ExecuteResetPwSQL(m_AaUser, p_dba);

                m_PwdEncrypt = m_AaUser.pwd_encrypt;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                m_Return.isSuccess = false;
                m_Return.rtnMessage = "PINBlock密碼重送發生例外狀況，請聯絡系統管理員!";
            }

            return m_Return;
        }

        /// <summary>
        /// 放行後，更新重設密碼後的資訊
        /// </summary>
        private void ExecuteResetPwSQL(AaUser p_AaUser, DBASqlLog p_dba)
        {
            string m_Sql = @"
UPDATE ZT_AaUser
   SET pwd_status = @pwd_status,
       pwd = (SELECT CONVERT(varchar(200),dbo.fn_EncryptByPassPhrasePwd(@pwd),2)),
       pwd2 = (SELECT CONVERT(varchar(200), dbo.fn_EncryptByPassPhrasePwd(@pwd2), 2)),
       pwd3 = @pwd3,
       pwd_encrypt = @pwd_encrypt,
       pwd_modify_date = @pwd_modify_date,
       status_flag = @status_flag,
       retry = @retry,
       updated_by = @updated_by,
       updated_date = @updated_date
 WHERE user_uuid = @user_uuid ";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@user_uuid", p_AaUser.user_uuid));
            m_paraList.Add(new DBParameter("@pwd_status", p_AaUser.pwd_status));
            m_paraList.Add(new DBParameter("@pwd", p_AaUser.pwd));
            m_paraList.Add(new DBParameter("@pwd2", p_AaUser.pwd2));
            m_paraList.Add(new DBParameter("@pwd3", p_AaUser.pwd3));
            m_paraList.Add(new DBParameter("@pwd_encrypt", p_AaUser.pwd_encrypt));
            m_paraList.Add(new DBParameter("@pwd_modify_date", p_AaUser.pwd_modify_date));
            m_paraList.Add(new DBParameter("@status_flag", p_AaUser.status_flag));
            m_paraList.Add(new DBParameter("@retry", p_AaUser.retry));
            m_paraList.Add(new DBParameter("@updated_by", p_AaUser.updated_by));
            m_paraList.Add(new DBParameter("@updated_date", Convert.ToDateTime(p_AaUser.updated_date)));

            if (false == p_dba.ExecNonQueryTransaction(m_Sql, m_paraList.ToArray()) && p_dba.isException)
            {
                throw new Exception(p_dba.ex.Message);
            }
        }
    }
}