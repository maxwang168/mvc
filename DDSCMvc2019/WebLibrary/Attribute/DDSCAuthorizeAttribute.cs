using CommonLibrary.Service;
using Entity;
using PortalService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;

namespace WebLibrary.Attribute
{
    public class DDSCAuthorizeAttribute : AuthorizeAttribute
    {
        private Service<ISysService> g_service = new WCFService<ISysService>("wsHttpBinding_ISysService");
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
 

            UserData userData = (UserData)filterContext.HttpContext.Session["UserData"];
            string authToken = (string)filterContext.HttpContext.Session["AuthToken"];
            if (userData == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Logout");
            }
            else
            {
                if (filterContext.HttpContext.Request.Cookies["AuthToken"] != null && authToken != null)
                {
                    if (filterContext.HttpContext.Request.Cookies["AuthToken"].Value == authToken)
                    {
                        
                        if (!g_service.Use(s=>s.CheckLoginStatus(userData.LoginToken)))
                        {
                            filterContext.Result = new RedirectResult("~/Account/Logout");
                        }
                        string controlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                        string actionName = filterContext.ActionDescriptor.ActionName;
                        string areaName = (string)filterContext.RouteData.DataTokens["area"];
                        string edType = filterContext.RouteData.Values["edType"] == null ?
                            string.Empty : filterContext.RouteData.Values["edType"].ToString();
                        if (areaName == null && controlName == "Home")
                        {
                            //skip Home
                        }
                        else
                        {
                            if (!findProgramId(areaName, controlName, edType, userData.ProgramXml))
                            {
                                filterContext.Result = new RedirectResult("~/Account/Login");
                            }
                        }
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/Account/Logout");
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Account/Logout");
                }
            }
        }

        private bool findProgramId(string areaName, string controlName, string edType, string programXml)
        {
            XmlDocument xml = new XmlDocument();
            xml.XmlResolver = null;
            xml.LoadXml(programXml);
            bool isFind = false;
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
                                        isFind = true;
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
                                    isFind = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return isFind;
        }

    }
}
