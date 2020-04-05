using CommonLibrary.Service;
using Entity;
using Entity.SYS;
using log4net;
using PortalService.Contract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalWeb.Controllers
{
    public class AccountController : Controller
    {
        private static ILog logger = LogManager.GetLogger(typeof(AccountController));
        private string g_isInternet = ConfigurationManager.AppSettings["IsInternet"];
        private string g_loginType = ConfigurationManager.AppSettings["LoginType"];
        private int g_retryMax = Convert.ToInt32(ConfigurationManager.AppSettings["RetryMax"]);
        private Service<IBasService> g_basService = new WCFService<IBasService>("wsHttpBinding_IBasService");
        private Service<ISysService> g_sysService = new WCFService<ISysService>("wsHttpBinding_ISysService");
        private Service<IAaSysService> g_service = new WCFService<IAaSysService>("wsHttpBinding_IAaSysService");

        public ActionResult Login()
        {
            Session.Clear();
            Session.RemoveAll();
            LoginViewModel m_viewModel = new LoginViewModel();
            
            initQuery(m_viewModel);

            if (ViewBag.IsInternet == "Y")
            {
                return View("Login", m_viewModel);
            }
            else
            {
                return View("InternalLogin", m_viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Exclude = "")]LoginViewModel p_viewModel)
        {
            string m_ValidateCode = Session["ValidateCode"] == null ? "" : Session["ValidateCode"].ToString();

            Session.Clear();
            Session.RemoveAll();
            
            p_viewModel.IsNew = string.Empty;

            initQuery(p_viewModel);
            if (!checkFields(p_viewModel))
            {
                if (ViewBag.IsInternet == "Y")
                {
                    return View("Login", p_viewModel);
                }
                else
                {
                    return View("InternalLogin", p_viewModel);
                }
            }

            UserData m_userData = new UserData();
            string m_adminId = ConfigurationManager.AppSettings["AdminId"] == null ? "" : ConfigurationManager.AppSettings["AdminId"];
            bool isAdminDept = false;
            if (g_loginType == "LDAP" && !m_adminId.Contains(p_viewModel.LoginId))
            {
                
            }
            else
            {
                //內網 (For公司測試)
                {
                    LoginViewModel m_model = new LoginViewModel();
                    m_model.LoginId = p_viewModel.LoginId;
                    m_model.Password = p_viewModel.Password;
                    m_userData = g_sysService.Use(s => s.Login(m_model));

                    if (m_userData.UserInfo == null)
                    {
                        p_viewModel.Message = "驗證失敗!";
                        initQuery(p_viewModel);
                        return View("InternalLogin", p_viewModel);
                    }
                    else if (m_userData.UserInfo.pwd_status == "L")
                    {
                        p_viewModel.Message = string.Format("密碼連續失敗{0}次, 已被鎖定!", g_retryMax);
                        initQuery(p_viewModel);
                        return View("InternalLogin", p_viewModel);
                    }
                    else if (m_userData.UserInfo.pwd_status == "N")
                    {
                        p_viewModel.Message = "驗證失敗!";
                        initQuery(p_viewModel);
                        return View("InternalLogin", p_viewModel);
                    }
                    else if (m_userData.UserInfo.pwd_status == "M")
                    {
                        p_viewModel.Message = "密碼已過期!";
                        initQuery(p_viewModel);
                        return View("InternalLogin", p_viewModel);
                    }
                    else if (m_userData.UserInfo.pwd != p_viewModel.Password)
                    {
                        p_viewModel.Message = "驗證失敗!";
                        g_sysService.Use(s => s.UpdatePwdStatus(m_userData.UserInfo.user_uuid, m_userData.UserInfo.retry + 1, g_retryMax));
                        initQuery(p_viewModel);
                        return View("InternalLogin", p_viewModel);
                    }
                    else if (m_userData.ProgramXml == null || m_userData.GroupList.Count == 0)
                    {
                        p_viewModel.Message = "查無權限!";
                        initQuery(p_viewModel);
                        return View("InternalLogin", p_viewModel);
                    }
                    else if (m_userData.UserInfo.pwd_status == "I")
                    {
                        p_viewModel.IsNew = "Y";
                        p_viewModel.UserUuid = m_userData.UserInfo.user_uuid.ToString();
                        initQuery(p_viewModel);
                        return View("InternalLogin", p_viewModel);
                    }
                }
            }

            g_sysService.Use(s => s.UpdatePwdStatus(m_userData.UserInfo.user_uuid, 0, g_retryMax));

            
            logger.Debug("User Uuid + " + m_userData.UserInfo.user_uuid);
           
            HttpContext.Session["UserData"] = m_userData;
            string guid = Guid.NewGuid().ToString();
            Session["AuthToken"] = guid;
            Response.Cookies.Add(new HttpCookie("AuthToken", guid));

            if (ViewBag.IsInternet == "Y")
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                return Redirect("~/Home/InternalIndex");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            return Redirect("~/Account/Login");
        }

        private void initQuery(LoginViewModel p_viewModel)
        {
            ViewBag.IsInternet = g_isInternet;
            ViewBag.LoginType = g_loginType;
        }

        private bool checkFields(LoginViewModel p_viewModel)
        {
            bool m_return = false;

            if (ViewBag.IsInternet == "Y" && ViewBag.LoginType == "DB" && p_viewModel.TMId == null)
            {
                ViewBag.ErrorMsg = "<font color='red'>請輸入統編!</font>";
            }
            else if (p_viewModel.LoginId == null)
            {
                ViewBag.ErrorMsg = "<font color='red'>請輸入帳號!</font>";
            }
            else if (p_viewModel.Password == null)
            {
                ViewBag.ErrorMsg = "<font color='red'>請輸入密碼!</font>";
            }
            else
            {
                m_return = true;
            }

            return m_return;
        }

        [HttpPost]
        public ActionResult ChangePwd([Bind(Exclude = "")]LoginViewModel p_viewModel)
        {
            if (ModelState.IsValid)
            {
                if (checkDialogFields(p_viewModel))
                {
                    p_viewModel.EditChangePwd.Pwd3 = string.Empty;
                    p_viewModel.EditChangePwd.UpdatedBy = p_viewModel.EditChangePwd.UserUid;
                    p_viewModel.EditChangePwd.UpdatedDate = DateTime.Now;
                    bool m_success = g_service.Use(s => s.changePwdChangePwdBE(p_viewModel.EditChangePwd));
                    if (m_success)
                    {
                        p_viewModel.Message = "修改成功!請重新登入!";
                    }
                    else
                    {
                        p_viewModel.Message = "修改失敗!";
                    }
                }

                p_viewModel.AnnounceInfoList = g_basService.Use(a => a.AnnounceInfoQuery()).ToList();
                p_viewModel.Ann_Count = p_viewModel.AnnounceInfoList.Count;
                p_viewModel.Ann_Path = g_sysService.Use(a => a.QuerySysCodeInfoForFile("File_PATH", "ANN_HTML")).VarChar01;
                p_viewModel.Ann_File = g_sysService.Use(a => a.QuerySysCodeInfoForFile("File_PATH", "ANN_HTML_OLD")).VarChar01;

                initQuery(p_viewModel);
                return View("Login", p_viewModel);
            }
            else
            {
                p_viewModel.AnnounceInfoList = g_basService.Use(a => a.AnnounceInfoQuery()).ToList();
                p_viewModel.Ann_Count = p_viewModel.AnnounceInfoList.Count;
                p_viewModel.Ann_Path = g_sysService.Use(a => a.QuerySysCodeInfoForFile("File_PATH", "ANN_HTML")).VarChar01;
                p_viewModel.Ann_File = g_sysService.Use(a => a.QuerySysCodeInfoForFile("File_PATH", "ANN_HTML_OLD")).VarChar01;

                initQuery(p_viewModel);
                return View("Login", p_viewModel);
            }
        }

        private bool checkDialogFields(LoginViewModel p_viewModel)
        {
            bool m_return = false;
            bool m_isExist = g_service.Use(s => s.CheckPwd(p_viewModel.EditChangePwd.UserUid, p_viewModel.EditChangePwd.OldPwd));
            if (!m_isExist)
            {
                p_viewModel.Message = "舊密碼輸入錯誤";
            }
            else
            {
                m_return = true;
            }

            return m_return;
        }

    }
}