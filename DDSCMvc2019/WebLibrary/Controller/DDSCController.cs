using CommonLibrary.Service;
using Entity;
using Entity.SYS;
using log4net;
using PortalService.Contract;
using PortalService.Contract.ViewModel.System;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace WebLibrary.Controller
{
    public class DDSCController : System.Web.Mvc.Controller
    {
        private static ILog logger = LogManager.GetLogger(typeof(DDSCController));
        private Service<IFlowService> g_flowService = new WCFService<IFlowService>("wsHttpBinding_IFlowService");
        private Service<ISysService> g_sysService = new WCFService<ISysService>("wsHttpBinding_ISysService");
        private Service<INotifyService> g_notifyService = new WCFService<INotifyService>("wsHttpBinding_INotifyService");
        public static string g_isInternet = ConfigurationManager.AppSettings["IsInternet"];

        #region Properties
        /// <summary>
        /// 使用者登入資訊
        /// </summary>
        public UserData LoginUser
        {
            get
            {
                return Session["UserData"] as UserData;
            }
        }
        /// <summary>
        /// 暫存結果的Key Name
        /// </summary>
        public string SessionName { get; set; }
        /// <summary>
        /// 儲存暫存結果於Session中
        /// </summary>
        public object SessionObject
        {
            get
            {
                return Session[SessionName];
            }
            set
            {
                Session[SessionName] = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 將畫面的查詢條件儲存至Session
        /// </summary>
        /// <param name="p_vm"></param>
        /// <param name="sessionProperties">需要暫存的viewmodel資料</param>
        protected virtual void FormToSession<T>(T p_vm, string[] sessionProperties)
        {
            List<string> m_val = new List<string>();
            string m_sessionName = SessionName + "_FORM";
            try
            {
                PropertyInfo[] vmProperties = p_vm.GetType().GetProperties();
                foreach (string sessProp in sessionProperties)
                {
                    foreach (PropertyInfo vmProp in vmProperties)
                    {

                        if (sessProp == vmProp.Name)
                        {
                            var val = vmProp.GetValue(p_vm);
                            if (val == null)
                            {
                                m_val.Add(string.Format("{0},{1}", sessProp, ""));
                            }
                            else if (val.GetType().Name != typeof(List<T>).Name)
                            {

                                m_val.Add(string.Format("{0},{1}", sessProp, val == null ? "" : val.ToString()));

                            }
                            break;
                        }

                    }
                }

                Session.Remove(m_sessionName);
                Session[m_sessionName] = m_val;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 將Session的查詢條件放入Model
        /// </summary>
        /// <returns></returns>
        protected virtual void SessionToForm<T>(T p_vm)
        {
            //T m_retVal = p_vm;
            string m_sessionName = SessionName + "_FORM";
            try
            {
                if (Session[m_sessionName] != null)
                {
                    List<string> m_queryCriteria = (List<string>)Session[m_sessionName];
                    string m_key = string.Empty;
                    string m_val = string.Empty;
                    PropertyInfo[] DestProperties = p_vm.GetType().GetProperties();
                    Type type;
                    for (int i = 0; i < m_queryCriteria.Count; i++)
                    {
                        m_key = m_queryCriteria[i].Split(',')[0];
                        m_val = m_queryCriteria[i].Split(',')[1];

                        foreach (PropertyInfo destProp in DestProperties)
                        {
                            if (m_key == destProp.Name)
                            {
                                type = destProp.PropertyType;
                                if (type == typeof(string) || type == typeof(int))
                                {
                                    destProp.SetValue(p_vm, m_val);
                                }
                                else if (type == typeof(DateTime))
                                {
                                    DateTime tmp;
                                    if (DateTime.TryParse(m_val, out tmp))
                                    {
                                        if (tmp.Year > 1)
                                        {
                                            destProp.SetValue(p_vm, tmp);
                                        }
                                    }

                                }
                                break;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return m_retVal;
        }

        /// <summary>
        /// Object To Xml String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Object2Xml(object value)
        {
            StringWriter stringwriter = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(value.GetType());
            serializer.Serialize(stringwriter, value);
            return stringwriter.ToString();
        }

        /// <summary>
        /// Xml String To Object
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Xml2Object(string xml, Type type)
        {
            XmlSerializer m_xmlSear = new XmlSerializer(type);
            StringReader m_sr = new StringReader(xml);
            XmlTextReader reader = new XmlTextReader(m_sr);
            object transferObj = m_xmlSear.Deserialize(reader);

            return transferObj;
        }

        /// <summary>
        /// 送出流程
        /// </summary>
        /// <param name="p_dataUuid">Key</param>
        /// <param name="p_flwId">流程代碼</param>
        /// <param name="p_flwType">操作行為 Ex: A:新增, M:修改, D:刪除</param>
        /// <param name="p_dataXml">放行內容</param>
        /// <param name="p_funcId">程式代碼</param>
        /// <param name="p_msg">錯誤訊息</param>
        /// <returns></returns>
        public bool Sent2Flow(Guid p_dataUuid, string p_flwId, string p_flwType, string p_dataXml, string p_beforeXml, string p_funcId, ref string p_msg)
        {
            bool m_return = false;

            try
            {
                if (p_flwType == "M" || p_flwType == "D" || p_flwType == "U")
                {
                    //TODO: 檢核USER_UUID是否已經為V，如果是，則不能送審
                    p_msg = g_flowService.Use(s => s.CheckDataAvailable(p_funcId, p_dataUuid));
                    if (false == string.IsNullOrWhiteSpace(p_msg))
                    {
                        return false;
                    }

                    m_return = g_flowService.Use(s => s.UpdateDataStatus(p_funcId, p_dataUuid, "V"));
                    if (!m_return)
                    {
                        p_msg = "更新資料發生錯誤!";
                        return false;
                    }
                }

                FlowStartModel model = new FlowStartModel();
                model.FlwId = p_flwId;
                model.FlwType = p_flwType;
                model.BeforeDataXml = p_beforeXml;
                model.DataXml = p_dataXml;
                model.DataUuid = p_dataUuid;
                model.FunctionId = p_funcId;
                model.RoleUuid = LoginUser.UserInfo.role_uuid;
                model.OrgUuid = LoginUser.UserInfo.org_uuid;
                model.SystemId = "CSFM";
                model.UserUuid = LoginUser.UserInfo.user_uuid;
                model.UserName = LoginUser.UserInfo.user_name;

                FlwRtn rtn = g_flowService.Use(s => s.FlowStart(model));

                if (!rtn.isSuccess)
                {
                    if (p_flwType == "M" || p_flwType == "D" || p_flwType == "U")
                    {
                        m_return = g_flowService.Use(s => s.UpdateDataStatus(p_funcId, p_dataUuid, "Y"));
                    }
                }

                m_return = rtn.isSuccess;
                p_msg = rtn.rtnMessage;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return m_return;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.IsInternet = g_isInternet;
            base.OnActionExecuting(filterContext);
        }

        #endregion 
    }
}