using CommonLibrary.Service;
using Entity;
using Entity.SYS;
using PortalService.Contract;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace WebLibrary.Attribute
{
    public class LogAttribute : ActionFilterAttribute, IActionFilter
    {
        private Service<ILogService> g_service = new WCFService<ILogService>("wsHttpBinding_ILogService");

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            string edType = filterContext.RouteData.Values["edType"]?.ToString();

            UserData userData = (UserData)filterContext.HttpContext.Session["UserData"];
            if (userData != null)
            {
                ArrayList logList = new ArrayList();
                StringBuilder sb = new StringBuilder();
                foreach (var parameter in filterContext.ActionParameters)
                {
                    string key = parameter.Key;
                    object value = parameter.Value;
                    if (value != null)
                    {
                        if (value.GetType().IsSerializable)
                        {
                            ArrayList data = new ArrayList();
                            data.Add(value.GetType().ToString() + "," + value.GetType().Module.Name.Replace(".dll", ""));
                            data.Add(toXmlData(value));
                            logList.Add(data);
                        }
                    }
                }
                DateTime now = DateTime.Now;
                SysUserLogBE log = new SysUserLogBE();
                log.user_log_uuid = Guid.NewGuid();
                //log.user_info_uuid = 
                log.user_uuid = userData.UserInfo.user_uuid;
                log.exe_date = now;
                log.exe_btn = actionName;
                if (logList.Count > 0)
                {
                    log.exe_query = toXmlData(logList);
                }
                else
                {
                    log.exe_query = string.Empty;
                }
                log.exe_result = string.Empty;
                log.status_flag = "0";
                log.created_by = userData.UserInfo.user_uuid;
                log.created_date = now;
                log.updated_by = userData.UserInfo.user_uuid;
                log.updated_date = now;
                findProgramId((string)filterContext.RouteData.DataTokens["area"], controlName, edType, userData.ProgramXml, log);

                g_service.Use(s => s.insertLog(log));
            }
        }

        private string toXmlData(Object value)
        {
            StringWriter stringwriter = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(value.GetType());
            serializer.Serialize(stringwriter, value);
            return stringwriter.ToString();
        }

        private void findProgramId(string areaName, string controlName, string edType, string programXml, SysUserLogBE log)
        {
            XmlDocument xml = new XmlDocument();
            xml.XmlResolver = null;
            xml.LoadXml(programXml);
            XmlNodeList nodeList = xml.SelectSingleNode("/MAIN").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNodeList proNodeList = nodeList[i].ChildNodes;
                for (int j = 0; j < proNodeList.Count; j++)
                {
                    XmlNodeList itemNodeList = proNodeList[j].ChildNodes;
                    if (itemNodeList.Count > 0)
                    {
                        for (int k = 0; k < itemNodeList.Count; k++)
                        {
                            if (itemNodeList[k].Attributes["Link"] != null)
                            {
                                if (string.IsNullOrEmpty(areaName))
                                {
                                    //目前不會有area為空
                                }
                                else
                                {
                                    string matchUrl = areaName + "/" + controlName + (string.IsNullOrEmpty(edType) ? "" : "/" + edType);
                                    string linkUrl = itemNodeList[k].Attributes["Link"].InnerText;
                                    if (linkUrl.IndexOf(matchUrl) >= 0)
                                    {
                                        log.func_id = itemNodeList[k].Name;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (proNodeList[j].Attributes["Link"] != null)
                        {
                            if (string.IsNullOrEmpty(areaName))
                            {
                                //目前不會有area為空
                            }
                            else
                            {
                                string matchUrl = areaName + "/" + controlName + (string.IsNullOrEmpty(edType) ? "" : "/" + edType);
                                string linkUrl = proNodeList[j].Attributes["Link"].InnerText;
                                if (linkUrl.IndexOf(matchUrl) >= 0)
                                {
                                    log.func_id = proNodeList[j].Name;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
