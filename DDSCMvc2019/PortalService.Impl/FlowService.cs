using CommonLibrary.DBA;
using Entity.SYS;
using log4net;
using PortalService.Contract;
using PortalService.Contract.ViewModel;
using PortalService.Contract.ViewModel.System;
using PortalService.FlowProcess;
using PortalService.Impl.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace PortalService.Impl
{
    public class FlowService : IFlowService
    {
        private static ILog logger = LogManager.GetLogger(typeof(SysService));
        private DBASqlLog g_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);
        private DBASqlLog gquery_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);

        public FlwRtn FlowStart(FlowStartModel p_startModel)
        {
            FlwRtn m_return = new FlwRtn();
            try
            {
                FlwDefine m_define = GetFlwDefine(p_startModel.FlwId);
                if (m_define != null)
                {
                    if (m_define.DefineList.Count > 1)//確認流程一定要有兩階以上
                    {
                        List<FlwUser> m_userList = GetNextApproveList(p_startModel, m_define, 1);//流程啟動一定抓下一階
                        if (m_userList.Count > 0)
                        {
                            DBASqlLog m_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);
                            try
                            {
                                DateTime now = DateTime.Now;
                                //搜尋function Name
                                SysProgramBL m_programBl = new SysProgramBL();
                                SysProgramModel m_queryModel = new SysProgramModel();
                                m_queryModel.ProgramID = p_startModel.FunctionId;
                                List<SysProgramBE> m_program = m_programBl.SysProgramQuery(m_queryModel).ToList();

                                m_dba.BeginTrans();
                                Guid m_jobId = InsertFlwJob(p_startModel, m_define, m_dba, m_program);
                                FlwJobItemBE m_item = new FlwJobItemBE();
                                m_item.job_uuid = m_jobId;
                                m_item.flw_item_uuid = m_define.DefineList[1].flw_item_uuid;
                                m_item.flow_status = FlwParameter.FLW_STATUS_GO;
                                m_item.parent_uuid = Guid.Empty;
                                m_item.approve_remark = string.Empty;
                                m_item.approve_status = "";
                                m_item.status_flag = "Y";
                                m_item.created_by = p_startModel.UserUuid;
                                m_item.created_date = now;
                                m_item.updated_by = p_startModel.UserUuid;
                                m_item.updated_date = now;

                                TodoListBE m_todo = new TodoListBE();
                                m_todo.data_uuid = p_startModel.DataUuid;
                                m_todo.system_id = p_startModel.SystemId;
                                m_todo.function_id = p_startModel.FunctionId;
                                if (string.IsNullOrEmpty(m_program[0].flow_name))
                                {
                                    m_todo.function_name = m_program[0].program_name;
                                }
                                else
                                {
                                    m_todo.function_name = m_program[0].flow_name;
                                }
                                m_todo.todo_content = "有一筆覆核資料待覆核";
                                m_todo.todo_flag = "G";
                                m_todo.created_by = p_startModel.UserUuid;
                                m_todo.updated_by = p_startModel.UserUuid;
                                for (int i = 0; i < m_userList.Count; i++)
                                {
                                    m_todo.todo_uuid = Guid.NewGuid();
                                    m_todo.role_uuid = m_userList[i].UserGuid;
                                    InsertTodoList(m_todo, m_dba);

                                    m_item.job_item_uuid = Guid.NewGuid();
                                    m_item.todo_uuid = m_todo.todo_uuid;
                                    m_item.flow_user_uuid = m_userList[i].UserGuid;
                                    InsertFlwJobItem(m_item, m_dba);

                                    notifyNT019(m_userList[i], p_startModel, m_program[0].program_name);
                                }

                                InsertFlwJobData(m_jobId, m_item.job_item_uuid, p_startModel, m_dba);

                                m_dba.Commit();
                                m_return.isSuccess = true;
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex.Message, ex);
                                //new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_startModel.UserUuid);
                                m_dba.Rollback();
                                throw ex;
                            }
                        }
                        else
                        {
                            m_return.isSuccess = false;
                            m_return.rtnMessage = "找不到可以覆核的人員";
                        }
                    }
                    else
                    {
                        m_return.isSuccess = false;
                        m_return.rtnMessage = "找不到主管覆核流程設定";
                    }
                }
                else
                {
                    m_return.isSuccess = false;
                    m_return.rtnMessage = "沒有覆核相關設定";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                //new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_startModel.UserUuid);
                m_return.isSuccess = false;
                m_return.rtnMessage = ex.Message;
            }

            return m_return;
        }

        public FlwRtn FlowUpdate(FlowUpdateModel p_updateModel)
        {
            FlwRtn m_return = new FlwRtn();
            try
            {
                FlwJobBE m_job = GetFlwJobFromItem(p_updateModel.JobItemUuid);

                if (m_job != null)
                {
                    SysProgramBL m_programBl = new SysProgramBL();
                    SysProgramModel m_queryModel = new SysProgramModel();
                    m_queryModel.ProgramID = m_job.function_id;
                    List<SysProgramBE> m_proList = m_programBl.SysProgramQuery(m_queryModel).ToList();
                    if (m_proList.Count == 0)
                    {
                        m_return.isSuccess = false;
                        m_return.rtnMessage = "找不到覆核紀錄中對應的功能代號設定";
                        return m_return;
                    }

                    FlwDefine m_define = GetFlwDefine(m_job.flw_uuid);

                    int step = 0;
                    DBASqlLog m_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);
                    try
                    {
                        for (int i = 0; i < m_define.DefineList.Count; i++)
                        {
                            if (m_job.flwJobItem.flw_item_uuid == m_define.DefineList[i].flw_item_uuid)
                            {
                                step = i;
                                break;
                            }
                        }
                        //覆核程序
                        m_dba.BeginTrans();
                        if (p_updateModel.Approve == FlwParameter.FLW_APPROVE_APPROVE)
                        {
                            //覆核種類其一覆核
                            bool m_realApprove = false;
                            if (m_define.DefineList[step].approve_type == "2")
                            {
                                UpdateFlwJobItemStatus(p_updateModel, m_dba);
                                UpdateTodoFinish(m_job.flwJobItem.todo_uuid, p_updateModel.UserUuid, m_dba);
                                m_realApprove = true;
                            }
                            else if (m_define.DefineList[step].approve_type == "1")//全部放行
                            {

                            }

                            if (m_realApprove)
                            {
                                if (step < m_define.DefineList.Count - 1) //產生下一階的覆核資料
                                {
                                    DateTime now = DateTime.Now;
                                    FlowStartModel p_startModel = new FlowStartModel();
                                    p_startModel.RoleUuid = m_job.flwJobItem.flow_user_uuid;
                                    p_startModel.UserUuid = p_updateModel.UserUuid;

                                    List<FlwUser> m_userList = GetNextApproveList(p_startModel, m_define, step+1);
                                    FlwJobItemBE m_item = new FlwJobItemBE();
                                    m_item.job_uuid = m_job.job_uuid;
                                    m_item.flw_item_uuid = m_define.DefineList[step+1].flw_item_uuid;
                                    m_item.flow_status = FlwParameter.FLW_STATUS_GO;
                                    m_item.parent_uuid = m_job.flwJobItem.job_item_uuid;
                                    m_item.approve_remark = string.Empty;
                                    m_item.approve_status = "";
                                    m_item.status_flag = "Y";
                                    m_item.created_by = p_updateModel.UserUuid;
                                    m_item.created_date = now;
                                    m_item.updated_by = p_updateModel.UserUuid;
                                    m_item.updated_date = now;

                                    TodoListBE m_todo = new TodoListBE();
                                    m_todo.data_uuid = m_job.data_uuid;
                                    m_todo.system_id = m_job.system_id;
                                    m_todo.function_id = m_job.function_id;
                                    m_todo.function_name = m_proList[0].program_name;
                                    m_todo.todo_content = "有一筆覆核資料待覆核";
                                    m_todo.todo_flag = "G";
                                    m_todo.created_by = p_updateModel.UserUuid;
                                    m_todo.updated_by = p_updateModel.UserUuid;


                                    for (int i = 0; i < m_userList.Count; i++)
                                    {
                                        m_todo.todo_uuid = Guid.NewGuid();
                                        m_todo.role_uuid = m_userList[i].UserGuid;
                                        InsertTodoList(m_todo, m_dba);
                                        m_item.job_item_uuid = Guid.NewGuid();
                                        m_item.todo_uuid = m_todo.todo_uuid;
                                        m_item.flow_user_uuid = m_userList[i].UserGuid;
                                        InsertFlwJobItem(m_item, m_dba);
                                        notifyNT019(m_userList[i], p_startModel, m_proList[0].program_name);
                                    }
                                    m_return.isSuccess = true;
                                }
                                else //沒有需要覆核的階層,完成覆核
                                {
                                    
                                    UpdateFlwJobStatus(m_job.job_uuid, FlwParameter.FLW_STATUS_FINISH, p_updateModel.UserUuid, m_dba);
                                    Type m_taskType = Type.GetType(string.Format("PortalService.FlowProcess.{0}, PortalService.FlowProcess",
                                                    m_proList[0].func_class));
                                    IFlowProcess process = (IFlowProcess)Activator.CreateInstance(m_taskType);
                                    m_return = process.flowApprove(m_job, p_updateModel.UserUuid, m_proList[0], m_dba);
                                    if (m_return.isSuccess)
                                    {
                                        notifyNT020(m_job, p_updateModel.UserUuid, m_job.function_name);
                                    }
                                }
                            }
                        }
                        else if (p_updateModel.Approve == FlwParameter.FLW_APPROVE_RJECT)//退回程序
                        {
                            UpdateFlwJobItemStatus(p_updateModel, m_dba);
                            UpdateTodoFinish(m_job.flwJobItem.todo_uuid, p_updateModel.UserUuid, m_dba);
                            UpdateFlwJobStatus(m_job.job_uuid, FlwParameter.FLW_STATUS_REJECT, p_updateModel.UserUuid, m_dba);

                            Type m_taskType = Type.GetType(string.Format("PortalService.FlowProcess.{0}, PortalService.FlowProcess",
                                             m_proList[0].func_class));
                            IFlowProcess process = (IFlowProcess)Activator.CreateInstance(m_taskType);
                            m_return = process.flowReject(m_job, p_updateModel.UserUuid, m_proList[0], m_dba);

                            if (m_return.isSuccess)
                            {
                                notifyNT202(m_job, p_updateModel.UserUuid, m_job.function_name);
                            }
                        }
                        else
                        {
                            m_return.isSuccess = false;
                            m_return.rtnMessage = "不支援的覆核型態";
                        }

                        m_dba.Commit();
                    }
                    catch (Exception ex)
                    {
                        m_dba.Rollback();
                        logger.Error(ex.Message, ex);
                        //new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_updateModel.UserUuid);
                        throw ex;
                    }
                }
                else
                {
                    m_return.isSuccess = false;
                    m_return.rtnMessage = "找不到覆核主檔資料";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                //new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, p_updateModel.UserUuid);
                m_return.isSuccess = false;
                m_return.rtnMessage = ex.Message;
            }
            return m_return;
        }

        private Guid InsertFlwJob(FlowStartModel p_startModel, FlwDefine p_define, DBASqlLog p_dba, List<SysProgramBE> p_program)
        {
            string m_sql = @"INSERT INTO ZT_FlwJob (job_uuid, flw_uuid, user_info_uuid, data_uuid,
org_uuid, system_id ,function_id, function_name, service_name, flw_status, call_back, return_data,
flw_type, status_flag, created_by, created_date, updated_by, updated_date) VALUES (@job_uuid, 
@flw_uuid, @user_info_uuid, @data_uuid, @org_uuid, @system_id, @function_id, @function_name, 
@service_name, @flw_status, @call_back, @return_data, @flw_type, @status_flag, @created_by, 
@created_date, @updated_by, @updated_date)";

            DateTime now = DateTime.Now;
            List<DBParameter> m_paraList = new List<DBParameter>();
            Guid m_jobId = Guid.NewGuid();
            m_paraList.Add(new DBParameter("@job_uuid", m_jobId));
            m_paraList.Add(new DBParameter("@flw_uuid", p_define.flw_uuid));
            m_paraList.Add(new DBParameter("@user_info_uuid", p_startModel.UserUuid));
            m_paraList.Add(new DBParameter("@data_uuid", p_startModel.DataUuid));
            m_paraList.Add(new DBParameter("@org_uuid", p_startModel.OrgUuid));
            m_paraList.Add(new DBParameter("@system_id", p_startModel.SystemId));
            m_paraList.Add(new DBParameter("@function_id", p_startModel.FunctionId));
            if (string.IsNullOrEmpty(p_program[0].flow_name))
            {
                m_paraList.Add(new DBParameter("@function_name", p_program[0].program_name));
            }
            else
            {
                m_paraList.Add(new DBParameter("@function_name", p_program[0].flow_name));
            }
            m_paraList.Add(new DBParameter("@service_name", string.Empty));
            m_paraList.Add(new DBParameter("@flw_status", FlwParameter.FLW_STATUS_GO));
            m_paraList.Add(new DBParameter("@call_back", string.Empty));
            m_paraList.Add(new DBParameter("@return_data", string.Empty));
            m_paraList.Add(new DBParameter("@flw_type", p_startModel.FlwType));
            m_paraList.Add(new DBParameter("@status_flag", "Y"));
            m_paraList.Add(new DBParameter("@created_by", p_startModel.UserUuid));
            m_paraList.Add(new DBParameter("@created_date", now));
            m_paraList.Add(new DBParameter("@updated_by", p_startModel.UserUuid));
            m_paraList.Add(new DBParameter("@updated_date", now));

            p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());

            return m_jobId;
        }

        private List<FlwUser> GetNextApproveList(FlowStartModel p_startModel, FlwDefine p_define, int p_step)
        {
            List<FlwUser> m_result = new List<FlwUser>();
            string m_sql = @"SELECT * from ZT_SysCodeInfo WHERE 
CATE = 'USER_GROUP'
AND var_char04 = @var_char04";
            List<DBParameter> m_paraList = new List<DBParameter>();
            //m_paraList.Add(new DBParameter("@role_uuid", p_startModel.RoleUuid));
            m_paraList.Add(new DBParameter("@var_char04", p_define.DefineList[p_step].role_id));
            logger.Debug("role_id" + p_define.DefineList[p_step].role_id);
            DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
            if (m_dataTable.Rows.Count > 0)
            {
                FlwUser m_user = new FlwUser();
                m_user.UserGuid = (Guid)m_dataTable.Rows[0]["code_uuid"];
                m_user.RoleId = (string)m_dataTable.Rows[0]["code_id"];

                m_result.Add(m_user);
            }
            return m_result;
        }

        private void InsertFlwJobItem(FlwJobItemBE p_jobItem, DBASqlLog p_dba)
        {
            string m_sql = @"INSERT INTO ZT_FlwJobItem (job_item_uuid, job_uuid, flw_item_uuid,
todo_uuid, flow_user_uuid, approve_by_uuid, approve_date, approve_remark, approve_status,
flow_status, parent_uuid, status_flag, created_by, created_date, updated_by, updated_date)
VALUES (@job_item_uuid, @job_uuid, @flw_item_uuid, @todo_uuid, @flow_user_uuid, @approve_by_uuid, 
@approve_date, @approve_remark, @approve_status, @flow_status, @parent_uuid, @status_flag, 
@created_by, @created_date, @updated_by, @updated_date)";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@job_item_uuid", p_jobItem.job_item_uuid));
            m_paraList.Add(new DBParameter("@job_uuid", p_jobItem.job_uuid));
            m_paraList.Add(new DBParameter("@flw_item_uuid", p_jobItem.flw_item_uuid));
            m_paraList.Add(new DBParameter("@todo_uuid", p_jobItem.todo_uuid));
            m_paraList.Add(new DBParameter("@flow_user_uuid", p_jobItem.flow_user_uuid));
            m_paraList.Add(new DBParameter("@approve_by_uuid", p_jobItem.approve_by_uuid));
            if (p_jobItem.approve_date == DateTime.MinValue)
            {
                m_paraList.Add(new DBParameter("@approve_date", DateTime.MaxValue));
            }
            else
            {
                m_paraList.Add(new DBParameter("@approve_date", p_jobItem.approve_date));
            }
            m_paraList.Add(new DBParameter("@approve_remark", p_jobItem.approve_remark));
            m_paraList.Add(new DBParameter("@approve_status", p_jobItem.approve_status));
            m_paraList.Add(new DBParameter("@flow_status", p_jobItem.flow_status));
            m_paraList.Add(new DBParameter("@parent_uuid", p_jobItem.parent_uuid));
            m_paraList.Add(new DBParameter("@status_flag", p_jobItem.status_flag));
            m_paraList.Add(new DBParameter("@created_by", p_jobItem.created_by));
            m_paraList.Add(new DBParameter("@created_date", p_jobItem.created_date));
            m_paraList.Add(new DBParameter("@updated_by", p_jobItem.updated_by));
            m_paraList.Add(new DBParameter("@updated_date", p_jobItem.updated_date));

            p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());

        }

        private void InsertFlwJobData(Guid p_jobId, Guid p_jobItemId, FlowStartModel p_startModel, DBASqlLog p_dba)
        {
            string m_sql = @"INSERT INTO ZT_FlwJobData (job_data_uuid, job_uuid, job_item_uuid
, preview_data, orginal_data, xslt_uuid, data_type, status_flag, created_by, created_date
, updated_by, updated_date) VALues (@job_data_uuid, @job_uuid, @job_item_uuid
, @preview_data, @orginal_data, @xslt_uuid, @data_type, @status_flag, @created_by, @created_date
, @updated_by, @updated_date)";

            DateTime now = DateTime.Now;
            List<DBParameter> m_paraList = new List<DBParameter>();
            Guid m_jobDataId = Guid.NewGuid();
            m_paraList.Add(new DBParameter("@job_data_uuid", m_jobDataId));
            m_paraList.Add(new DBParameter("@job_uuid", p_jobId));
            m_paraList.Add(new DBParameter("@job_item_uuid", p_jobItemId));
            m_paraList.Add(new DBParameter("@preview_data", p_startModel.BeforeDataXml));
            m_paraList.Add(new DBParameter("@orginal_data", p_startModel.DataXml));
            m_paraList.Add(new DBParameter("@xslt_uuid", Guid.Empty));
            m_paraList.Add(new DBParameter("@data_type", "XML"));
            m_paraList.Add(new DBParameter("@status_flag", "Y"));
            m_paraList.Add(new DBParameter("@created_by", p_startModel.UserUuid));
            m_paraList.Add(new DBParameter("@created_date", now));
            m_paraList.Add(new DBParameter("@updated_by", p_startModel.UserUuid));
            m_paraList.Add(new DBParameter("@updated_date", now));

            p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());
        }

        private FlwDefine GetFlwDefine(string p_flwId)
        {
            FlwDefine m_define = new FlwDefine();
            string m_sql = @"SELECT D.*, I.flw_item_uuid, I.dep_uuid, I.role_id,
I.flw_seq, I.approve_type, I.reject_org_uuid, I.reject_role, I.reject_flw_seq, I.flow_class
FROM ZT_FlwDefine AS D JOIN ZT_FlwDefineItem AS I
ON D.flw_uuid = I.flw_uuid AND I.status_flag = 'Y'
WHERE D.status_flag = 'Y' AND flw_id=@flw_id ORDER BY flw_seq";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@flw_id", p_flwId));

            DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
            if (m_dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        m_define = new FlwDefine();
                        m_define.flw_uuid = (Guid)m_dataTable.Rows[i]["flw_uuid"];
                        m_define.flw_id = (string)m_dataTable.Rows[i]["flw_id"];
                        m_define.flw_fname = (string)m_dataTable.Rows[i]["flw_fname"];
                        m_define.flw_description = (string)m_dataTable.Rows[i]["flw_description"];
                        m_define.is_default = (string)m_dataTable.Rows[i]["is_default"];
                        m_define.status_flag = (string)m_dataTable.Rows[i]["status_flag"];
                    }
                    FlwDefineItem m_item = new FlwDefineItem();
                    m_item.flw_item_uuid = (Guid)m_dataTable.Rows[i]["flw_item_uuid"];
                    m_item.flw_uuid = (Guid)m_dataTable.Rows[i]["flw_uuid"];
                    m_item.dep_uuid = (Guid)m_dataTable.Rows[i]["dep_uuid"];
                    m_item.role_id = (string)m_dataTable.Rows[i]["role_id"];
                    m_item.flw_seq = (int)m_dataTable.Rows[i]["flw_seq"];
                    m_item.approve_type = (string)m_dataTable.Rows[i]["approve_type"];
                    m_item.reject_org_uuid = (string)m_dataTable.Rows[i]["reject_org_uuid"];
                    m_item.reject_role = (string)m_dataTable.Rows[i]["reject_role"];
                    m_item.reject_flw_seq = (int)m_dataTable.Rows[i]["reject_flw_seq"];
                    m_item.flow_class = (string)m_dataTable.Rows[i]["flow_class"];

                    m_define.DefineList.Add(m_item);
                }
            }
            return m_define;
        }

        private FlwDefine GetFlwDefine(Guid p_flwUuid)
        {
            FlwDefine m_define = new FlwDefine();
            string m_sql = @"SELECT D.*, I.flw_item_uuid, I.dep_uuid, I.role_id,
I.flw_seq, I.approve_type, I.reject_org_uuid, I.reject_role, I.reject_flw_seq, I.flow_class
FROM ZT_FlwDefine AS D JOIN ZT_FlwDefineItem AS I
ON D.flw_uuid = I.flw_uuid AND I.status_flag = 'Y'
WHERE D.status_flag = 'Y' AND D.flw_uuid=@flw_uuid ORDER BY flw_seq";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@flw_uuid", p_flwUuid));

            DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
            if (m_dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        m_define = new FlwDefine();
                        m_define.flw_uuid = (Guid)m_dataTable.Rows[i]["flw_uuid"];
                        m_define.flw_id = (string)m_dataTable.Rows[i]["flw_id"];
                        m_define.flw_fname = (string)m_dataTable.Rows[i]["flw_fname"];
                        m_define.flw_description = (string)m_dataTable.Rows[i]["flw_description"];
                        m_define.is_default = (string)m_dataTable.Rows[i]["is_default"];
                        m_define.status_flag = (string)m_dataTable.Rows[i]["status_flag"];
                    }
                    FlwDefineItem m_item = new FlwDefineItem();
                    m_item.flw_item_uuid = (Guid)m_dataTable.Rows[i]["flw_item_uuid"];
                    m_item.flw_uuid = (Guid)m_dataTable.Rows[i]["flw_uuid"];
                    m_item.dep_uuid = (Guid)m_dataTable.Rows[i]["dep_uuid"];
                    m_item.role_id = (string)m_dataTable.Rows[i]["role_id"];
                    m_item.flw_seq = (int)m_dataTable.Rows[i]["flw_seq"];
                    m_item.approve_type = (string)m_dataTable.Rows[i]["approve_type"];
                    m_item.reject_org_uuid = (string)m_dataTable.Rows[i]["reject_org_uuid"];
                    m_item.reject_role = (string)m_dataTable.Rows[i]["reject_role"];
                    m_item.reject_flw_seq = (int)m_dataTable.Rows[i]["reject_flw_seq"];
                    m_item.flow_class = (string)m_dataTable.Rows[i]["flow_class"];

                    m_define.DefineList.Add(m_item);
                }
            }
            return m_define;
        }

        private FlwJobBE GetFlwJobFromItem(Guid p_flwJobItemId)
        {
            FlwJobBE m_job = null;
            string m_sql = @"SELECT J.*,I.flw_item_uuid,I.todo_uuid,D.orginal_data FROM ZT_FlwJob AS J 
JOIN ZT_FlwJobItem AS I ON J.job_uuid = I.job_uuid
JOIN ZT_FlwJobData AS D ON J.job_uuid = D.job_uuid
WHERE I.job_item_uuid = @job_item_uuid";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@job_item_uuid", p_flwJobItemId));

            DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
            if (m_dataTable.Rows.Count > 0)
            {
                m_job = new FlwJobBE();
                m_job.job_uuid = (Guid)m_dataTable.Rows[0]["job_uuid"];
                m_job.flw_uuid = (Guid)m_dataTable.Rows[0]["flw_uuid"];
                m_job.data_uuid = (Guid)m_dataTable.Rows[0]["data_uuid"];
                m_job.flw_type = (string)m_dataTable.Rows[0]["flw_type"];
                m_job.function_id = (string)m_dataTable.Rows[0]["function_id"];
                m_job.function_name = (string)m_dataTable.Rows[0]["function_name"];
                m_job.created_by = (Guid)m_dataTable.Rows[0]["created_by"];
                m_job.created_date = (DateTime)m_dataTable.Rows[0]["created_date"];
                FlwJobItemBE item = new FlwJobItemBE();
                item.flw_item_uuid = (Guid)m_dataTable.Rows[0]["flw_item_uuid"];
                item.todo_uuid = (Guid)m_dataTable.Rows[0]["todo_uuid"];
                m_job.flwJobItem = item;
                FlwJobDataBE m_data = new FlwJobDataBE();
                m_data.orginal_data = (string)m_dataTable.Rows[0]["orginal_data"];
                m_job.flwJobData = m_data;
            }
            return m_job;
        }

        private void UpdateFlwJobItemStatus(FlowUpdateModel p_updateModel, DBASqlLog p_dba)
        {
            string m_sql = @"UPDATE ZT_FlwJobItem SET approve_date = getdate(),
approve_remark = @approve_remark, approve_status=@approve_status, updated_by=@updated_by,
updated_date = getdate(), approve_by_uuid = @approve_by_uuid, flow_status=@flow_status
WHERE job_item_uuid=@job_item_uuid";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@approve_remark", p_updateModel.Remark));
            m_paraList.Add(new DBParameter("@approve_status", p_updateModel.Approve));
            m_paraList.Add(new DBParameter("@job_item_uuid", p_updateModel.JobItemUuid));
            m_paraList.Add(new DBParameter("@updated_by", p_updateModel.UserUuid));
            m_paraList.Add(new DBParameter("@approve_by_uuid", p_updateModel.UserUuid));
            m_paraList.Add(new DBParameter("@flow_status", FlwParameter.FLW_STATUS_FINISH));

            p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());

        }

        private void UpdateFlwJobStatus(Guid p_jobId, string p_flwStatus, Guid p_userId, DBASqlLog p_dba)
        {
            string m_sql = @"UPDATE ZT_FlwJob SET flw_status = @flw_status,
updated_by=@updated_by, updated_date = getdate()
WHERE job_uuid=@job_uuid";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@flw_status", p_flwStatus));
            m_paraList.Add(new DBParameter("@updated_by", p_userId));
            m_paraList.Add(new DBParameter("@job_uuid", p_jobId));

            p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());
        }

        public IEnumerable<FlwJobItemUI> QueryFlow(FlowQueryModel p_model)
        {
            List<FlwJobItemUI> m_return = new List<FlwJobItemUI>();
            string m_sql = @"SELECT I.job_item_uuid, J.function_id, J.function_name, I.approve_status, I.approve_remark,
J.flw_type, J.created_date AS ApplyDate, I.approve_date, D.orginal_data, P.func_entity, A.[user_name]
FROM ZT_FlwJobItem AS I JOIN ZT_FlwJob AS J ON
J.job_uuid = I.job_uuid JOIN ZT_FlwJobData AS D ON
D.job_uuid = I.job_uuid JOIN ZT_SysProgram AS P ON
P.func_id = J.function_id JOIN ZT_AaUser AS A ON
A.user_uuid = J.user_info_uuid
WHERE flow_user_uuid = @userUuid";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@userUuid", p_model.roleUuid));
                if (p_model.UserUuid != null)
                {
                    m_sql += " AND I.created_by != @creUuid";
                    m_paraList.Add(new DBParameter("@creUuid", p_model.UserUuid));
                }
                if (p_model.SendDateS != DateTime.MinValue)
                {
                    m_sql += " AND J.created_date BETWEEN @sdate AND @edate";
                    m_paraList.Add(new DBParameter("@sdate", p_model.SendDateS.Date));
                    m_paraList.Add(new DBParameter("@edate", p_model.SendDateE.Date.AddDays(1).AddSeconds(-1)));
                }

                if (!string.IsNullOrEmpty(p_model.ProgramId))
                {
                    m_sql += " AND J.function_id = @function_id";
                    m_paraList.Add(new DBParameter("@function_id", p_model.ProgramId));
                }
                if (p_model.StatusCode != "X")
                {
                    m_sql += " AND I.approve_status = @status";
                    m_paraList.Add(new DBParameter("@status", p_model.StatusCode));
                }
                m_sql += " ORDER BY J.created_date DESC";
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                for (int i = 0; i < m_dataTable.Rows.Count; i++)
                {
                    FlwJobItemUI m_item = new FlwJobItemUI();
                    m_item.sn = i + 1;
                    m_item.apply_date = (DateTime)m_dataTable.Rows[i]["ApplyDate"];
                    m_item.approve_date = (DateTime)m_dataTable.Rows[i]["approve_date"];
                    m_item.flw_type = (string)m_dataTable.Rows[i]["flw_type"];
                    switch (m_item.flw_type)
                    {
                        case "A":
                            m_item.flw_type_name = "新增";
                            break;
                        case "M":
                            m_item.flw_type_name = "修改";
                            break;
                        case "D":
                            m_item.flw_type_name = "刪除";
                            break;
                        case "R":
                            m_item.flw_type_name = "密碼重送";
                            break;
                        case "U":
                            m_item.flw_type_name = "帳號解鎖";
                            break;
                        case "N":
                            m_item.flw_type_name = "重送";
                            break;
                        default:
                            m_item.flw_type_name = m_item.flw_type;
                            break;
                    }
                    m_item.flw_status = (string)m_dataTable.Rows[i]["approve_status"];
                    if (m_item.flw_status == "R")
                    {
                        m_item.flw_status_name = "退回";
                    }
                    else if (m_item.flw_status == "Y")
                    {
                        m_item.flw_status_name = "已覆核";
                    }
                    else
                    {
                        m_item.flw_status_name = "待覆核";
                    }
                    m_item.remark = (string)m_dataTable.Rows[i]["approve_remark"];
                    m_item.function_id = (string)m_dataTable.Rows[i]["function_id"];
                    m_item.function_name = (string)m_dataTable.Rows[i]["function_name"];
                    m_item.function_entity = (string)m_dataTable.Rows[i]["func_entity"];
                    m_item.job_item_uuid = (Guid)m_dataTable.Rows[i]["job_item_uuid"];
                    m_item.user_info_uuid = p_model.roleUuid;
                    m_item.user_info_name = (string)m_dataTable.Rows[i]["user_name"];
                    m_item.orginal_data = m_dataTable.Rows[i]["orginal_data"].ToString();

                    m_return.Add(m_item);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                //new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_return;
        }

        public IEnumerable<string[]> QueryWorkFlowProgram(Guid p_UserUuid)
        {
            List<string[]> m_list = new List<string[]>();
            List<DBParameter> m_Params = new List<DBParameter>();
            string m_sql;

            if (p_UserUuid == Guid.Empty)
            {
                //FlowVM.ToString()
                m_sql = @"
SELECT DISTINCT P.func_id, P.func_name, P.super_uuid, P.seq_no
  FROM ZT_SysProgram P
 WHERE P.func_type = 'F'
 ORDER BY P.super_uuid, P.seq_no ";
            }
            else
            {
                //放行作業Flow、放行紀錄查詢FlowLog
                m_Params.Add(new DBParameter("@user_uuid", p_UserUuid));
                m_sql = @"
SELECT DISTINCT P.func_id, P.func_name, P.super_uuid, P.seq_no
  FROM ZT_SysGroupProgram GP
  JOIN ZT_SysGroup G
    ON G.group_uuid = GP.group_uuid
  JOIN (SELECT A.code_id AS role_id
          FROM ZT_SysCodeInfo A
          JOIN (SELECT C.CATE
                  FROM ZT_AaUser U2
                  JOIN ZT_SysCodeInfo C
                    ON C.code_id = U2.role_id
                 WHERE U2.user_uuid = @user_uuid) B
            ON A.CATE = B.CATE) U
    ON U.role_id = G.group_id
  JOIN ZT_SysProgram P
    ON P.func_uuid = GP.func_uuid
   AND P.func_type = 'F'
   AND ISNULL(P.func_entity, '') <> ''
 ORDER BY P.super_uuid, P.seq_no ";
            }
            DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_Params.ToArray());

            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                string[] m_string = new string[2];
                m_string[0] = m_dataTable.Rows[i]["func_id"].ToString().Trim();
                m_string[1] = m_dataTable.Rows[i]["func_name"].ToString();
                m_list.Add(m_string);
            }
            return m_list;
        }

        public void InsertTodoList(TodoListBE p_todoList, DBASqlLog p_dba)
        {
            string m_sql = @"INSERT INTO ZT_TodoList (todo_uuid, role_uuid, data_uuid, system_id,
function_id, function_name, todo_content, todo_flag, status_flag, created_by, created_date, updated_by,
updated_date) VALUES (@todo_uuid, @role_uuid, @data_uuid, @system_id, @function_id, @function_name, @todo_content, 
@todo_flag, @status_flag, @created_by, getdate(), @updated_by, getdate())";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@todo_uuid", p_todoList.todo_uuid));
            m_paraList.Add(new DBParameter("@role_uuid", p_todoList.role_uuid));
            m_paraList.Add(new DBParameter("@data_uuid", p_todoList.data_uuid));
            m_paraList.Add(new DBParameter("@system_id", p_todoList.system_id));
            m_paraList.Add(new DBParameter("@function_id", p_todoList.function_id));
            m_paraList.Add(new DBParameter("@function_name", p_todoList.function_name));
            m_paraList.Add(new DBParameter("@todo_content", p_todoList.todo_content));
            m_paraList.Add(new DBParameter("@todo_flag", p_todoList.todo_flag));
            m_paraList.Add(new DBParameter("@status_flag", "Y"));
            m_paraList.Add(new DBParameter("@created_by", p_todoList.created_by));
            m_paraList.Add(new DBParameter("@updated_by", p_todoList.updated_by));

            p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());
        }

        public IEnumerable<TodoListBE> QueryTodoList(Guid p_roleUuid, Guid p_userUuid)
        {
            List<TodoListBE> m_result = new List<TodoListBE>();
            string m_sql = @"SELECT function_id, function_name, COUNT(*) AS Total 
FROM ZT_TodoList WHERE role_uuid=@role_uuid AND todo_flag = 'G' AND updated_by != @userUuid
GROUP BY function_id, function_name";

            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@role_uuid", p_roleUuid));
            m_paraList.Add(new DBParameter("@userUuid", p_userUuid));
            SqlCommand cmd = new SqlCommand(m_sql);
            DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
            for (int i = 0; i < m_dataTable.Rows.Count; i++)
            {
                TodoListBE todo = new TodoListBE();
                todo.function_id = (string)m_dataTable.Rows[i]["function_id"];
                int m_count = (int)m_dataTable.Rows[i]["Total"];
                if (m_count > 1)
                {
                    todo.function_name = (string)m_dataTable.Rows[i]["function_name"] + "(" + m_count + ")";
                }
                else
                {
                    todo.function_name = (string)m_dataTable.Rows[i]["function_name"]+"(1)";
                }
                m_result.Add(todo);
            }

            return m_result;
        }

        private void UpdateTodoFinish(Guid p_todoUuid, Guid p_userUuid, DBASqlLog p_dba)
        {
            string m_sql = @"UPDATE ZT_TodoList SET todo_flag = 'F', updated_by=@userUuid, updated_date=getdate() 
WHERE todo_uuid=@todo_uuid";
            List<DBParameter> m_paraList = new List<DBParameter>();
            m_paraList.Add(new DBParameter("@todo_uuid", p_todoUuid));
            m_paraList.Add(new DBParameter("@userUuid", p_userUuid));
            p_dba.ExecNonQueryTransaction(m_sql, m_paraList.ToArray());
        }

        public string QueryFlwOrgData(Guid p_jobUuid)
        {
            string m_return = string.Empty;
            string m_sql = "SELECT orginal_data FROM ZT_FlwJobData WHERE job_uuid=@job_uuid";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@job_uuid", p_jobUuid));
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());

                if (m_dataTable != null && m_dataTable.Rows.Count > 0)
                {
                    m_return = (string)m_dataTable.Rows[0]["orginal_data"];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                //new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_return;
        }

        /// <summary>
        /// 放行作業申請
        /// </summary>
        /// <returns></returns>
        private bool notifyNT019(FlwUser p_flwUser, FlowStartModel p_startModel, string p_programName)
        {
            bool m_return = false;

            try
            {
                SysService sysProcess = new SysService();
                SysCodeInfoBE m_be = sysProcess.QuerySysCodeInfoForFile("FILE_PATH", "CSFM_WEB");

                //List<string[]> emailList = getSendMailFromAD(p_startModel.FunctionId, p_startModel.UserUuid);
                List<string[]> emailList = new List<string[]>();
                NotifyService process = new NotifyService();

                NotifySendModel sendModel = new NotifySendModel();
                sendModel.NotifyCodeId = "NT019"; //放行作業申請
                sendModel.UserUuid = p_startModel.UserUuid;
                sendModel.IsSubscription = false;
                for (int i = 0; i < emailList.Count; i++)
                {
                    sendModel.DataXml = string.Format(
                                "<Notify><Name>{0}</Name><Sender>{1}</Sender><FlowName>{2}</FlowName><Url>{3}</Url></Notify>",
                                emailList[i][1],
                                p_startModel.UserName,
                                p_programName,
                                m_be.VarChar01);

                    sendModel.ContactUserUuid = Guid.Empty;
                    sendModel.ContactAddrList = new List<string> { emailList[i][0]+ m_be.VarChar02 };
                    m_return = process.NotifySend(sendModel);
                    if (!m_return)
                    {
                        logger.Error("呼叫 NotifySend 失敗");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }

            return m_return;
        }

        /// <summary>
        /// 放行作業完成
        /// </summary>
        /// <param name="job"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool notifyNT020(FlwJobBE job, Guid userId, string p_programName)
        {
            bool m_return = false;

            try
            {
                SysService sysProcess = new SysService();
                SysCodeInfoBE m_be = sysProcess.QuerySysCodeInfoForFile("FILE_PATH", "CSFM_WEB");

                AaSysService userProcess = new AaSysService();
                //執行者查詢
                AaUser m_beExecutor = userProcess.QueryAaUserByUid(userId);
                //發起者查詢
                AaUser m_beSender = userProcess.QueryAaUserByUid(job.created_by);

                DateTime m_now = DateTime.Now;
                NotifySendModel sendModel = new NotifySendModel();
                sendModel.NotifyCodeId = "NT020"; //放行作業完成
                sendModel.UserUuid = userId;
                sendModel.ContactUserUuid = job.created_by;
                sendModel.IsSubscription = false;
                sendModel.DataXml = string.Format("<Notify><Date>{0}</Date><Time>{1}</Time><SendDate>{2}</SendDate><SendTime>{3}</SendTime><Name>{4}</Name><Executor>{5}</Executor><FlowName>{6}</FlowName><Url>{7}</Url></Notify>",
                    m_now.ToShortDateString(), m_now.ToString("HH:mm:ss"), job.created_date.ToShortDateString(), job.created_date.ToString("HH:mm:ss"),
                    m_beSender.user_name, m_beExecutor.user_name, p_programName, m_be.VarChar01);

                NotifyService process = new NotifyService();
                m_return = process.NotifySend(sendModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return m_return;
        }

        /// <summary>
        /// 放行作業完成退回
        /// </summary>
        /// <param name="job"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool notifyNT202(FlwJobBE job, Guid userId, string p_programName)
        {
            bool m_return = false;

            try
            {
                SysService sysProcess = new SysService();
                SysCodeInfoBE m_be = sysProcess.QuerySysCodeInfoForFile("FILE_PATH", "CSFM_WEB");

                AaSysService userProcess = new AaSysService();
                //執行者查詢
                AaUser m_beExecutor = userProcess.QueryAaUserByUid(userId);
                //發起者查詢
                AaUser m_beSender = userProcess.QueryAaUserByUid(job.created_by);

                DateTime m_now = DateTime.Now;
                NotifySendModel sendModel = new NotifySendModel();
                sendModel.NotifyCodeId = "NT202"; //放行作業完成退回
                sendModel.UserUuid = userId;
                sendModel.ContactUserUuid = job.created_by;
                sendModel.IsSubscription = false;
                sendModel.DataXml = string.Format("<Notify><Date>{0}</Date><Time>{1}</Time><SendDate>{2}</SendDate><SendTime>{3}</SendTime><Name>{4}</Name><Executor>{5}</Executor><FlowName>{6}</FlowName><Url>{7}</Url></Notify>",
                    m_now.ToShortDateString(), m_now.ToString("HH:mm:ss"), job.created_date.ToShortDateString(), job.created_date.ToString("HH:mm:ss"),
                    m_beSender.user_name, m_beExecutor.user_name, p_programName, m_be.VarChar01);

                NotifyService process = new NotifyService();
                m_return = process.NotifySend(sendModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return m_return;
        }

        public bool UpdateDataStatus(string p_funcId, Guid p_dataUuid, string p_status)
        {
            bool m_result = false;
            try
            {
                SysProgramBL m_programBl = new SysProgramBL();
                SysProgramModel m_queryModel = new SysProgramModel();
                m_queryModel.ProgramID = p_funcId;
                List<SysProgramBE> m_proList = m_programBl.SysProgramQuery(m_queryModel).ToList();
                string m_sql = "UPDATE " + m_proList[0].table_name + " SET status_flag = @flag WHERE " + m_proList[0].key_name + " = @key";
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@flag", "V"));
                m_paraList.Add(new DBParameter("@key", p_dataUuid));

                g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                m_result = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                //new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
            }
            return m_result;
        }

        public string CheckDataAvailable(string p_funcId, Guid p_dataUuid)
        {
            string m_ErrorMsg = string.Empty;
            string m_StatusFlag;
            try
            {
                SysProgramBL m_programBl = new SysProgramBL();
                SysProgramModel m_queryModel = new SysProgramModel();
                m_queryModel.ProgramID = p_funcId;
                List<SysProgramBE> m_proList = m_programBl.SysProgramQuery(m_queryModel).ToList();
                string m_sql = "SELECT TOP 1 'X' FROM " + m_proList[0].table_name + " WHERE " + m_proList[0].key_name + " = @key AND status_flag = 'V' ";
                List<SqlParameter> m_paraList = new List<SqlParameter>();
                m_paraList.Add(new SqlParameter("@key", p_dataUuid));
                m_StatusFlag = g_dba.GetData(m_sql, m_paraList.ToArray());
                if (false == string.IsNullOrWhiteSpace(m_StatusFlag))
                {
                    m_ErrorMsg = "送審資料狀態已被更新，請重新確認";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                //new LogService().InsertMonitorLog(MethodBase.GetCurrentMethod().Name, "ERROR", ex.Message, new Guid());
                m_ErrorMsg = ex.Message;
            }
            return m_ErrorMsg;
        }

    }
}
