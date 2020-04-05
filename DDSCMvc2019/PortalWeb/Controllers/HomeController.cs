using CommonLibrary.Service;
using Entity;
using Entity.SYS;
using PortalService.Contract;
using PortalService.Contract.ViewModel.System;
using PortalWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Attribute;

namespace PortalWeb.Controllers
{
    public class HomeController : Controller
    {
        private Service<ISysService> g_sysService = new WCFService<ISysService>("wsHttpBinding_ISysService");
        private Service<IFlowService> g_flowService = new WCFService<IFlowService>("wsHttpBinding_IFlowService");
        private Service<INotifyService> g_notifyService = new WCFService<INotifyService>("wsHttpBinding_INotifyService");

        private string g_isInternet = ConfigurationManager.AppSettings["IsInternet"];
        private string g_loginType = ConfigurationManager.AppSettings["LoginType"];

        [DDSCAuthorizeAttribute]
        public ActionResult Index(string Id)
        {
            Init();
            if (string.IsNullOrEmpty(Id))
            {

            }
            else
            {
                UserData userData = (UserData)Session["UserData"];
                userData.SelectedSystem = Id;
            }
            return RedirectToAction("InternalIndex");
        }

        [DDSCAuthorizeAttribute]
        public ActionResult InternalIndex(string Id)
        {
            InternalVM m_vm = new InternalVM();
            Init();

            int m_keepDay = 0;
            SysCodeInfoBE m_be = g_sysService.Use(s => s.QueryByCateCodeId("notify_keep", "Notify"));
            int.TryParse(m_be.VarChar01, out m_keepDay);

            if (string.IsNullOrEmpty(Id))
            {
                UserData userData = (UserData)Session["UserData"];
                DateTime m_now = DateTime.Now;
                NotifyQueryModel m_notifyModel = new NotifyQueryModel();
                m_notifyModel.SendDateS = m_now.AddDays(-m_keepDay);
                m_notifyModel.SendDateE = m_now;
                //如果使用者在LDAP沒有MAIL, 通知服務會是5-失敗（因為寄不到信），所以主畫面通知區塊就會沒有資料
                //m_notifyModel.PorcStatus = 4;
                m_notifyModel.UserUuid = userData.UserInfo.user_uuid;
                m_vm.NotificationList = g_notifyService.Use(s => s.QueryNotifyRec(m_notifyModel)).ToList();
                m_vm.TodoList = g_flowService.Use(s => s.QueryTodoList(userData.UserInfo.role_uuid, userData.UserInfo.user_uuid)).ToList();
            }
            else
            {
                UserData userData = (UserData)Session["UserData"];
                userData.SelectedSystem = Id;
                DateTime m_now = DateTime.Now;
                NotifyQueryModel m_notifyModel = new NotifyQueryModel();
                m_notifyModel.SendDateS = m_now.AddDays(-m_keepDay);
                m_notifyModel.SendDateE = m_now;
                //如果使用者在LDAP沒有MAIL, 通知服務會是5-失敗（因為寄不到信），所以主畫面通知區塊就會沒有資料
                //m_notifyModel.PorcStatus = 4;
                m_notifyModel.UserUuid = userData.UserInfo.user_uuid;
                m_vm.NotificationList = g_notifyService.Use(s => s.QueryNotifyRec(m_notifyModel)).ToList();
                m_vm.TodoList = g_flowService.Use(s => s.QueryTodoList(userData.UserInfo.role_uuid, userData.UserInfo.user_uuid)).ToList();
            }
            return View(m_vm);
        }

        [DDSCAuthorize]
        public ActionResult GoHome()
        {
            if (g_isInternet == "Y")
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                return Redirect("~/Home/InternalIndex");
            }
        }

        [DDSCAuthorizeAttribute]
        public JsonResult ChangeLab(string m_currency_type)
        {
            UserData userData = (UserData)Session["UserData"];
            if (m_currency_type != "")
            {
                userData.CurrencyType = m_currency_type;
            }
            return Json(m_currency_type);
        }

        public ActionResult Error()
        {
            ViewBag.IsInternet = g_isInternet;
            return View();
        }

        private void Init()
        {
            UserData userData = (UserData)Session["UserData"];
            ViewBag.IsInternet = g_isInternet;
            ViewBag.LoginType = g_loginType;
            //LoginViewModel m_login = new LoginViewModel();
            ViewBag.CurrencyType = userData.CurrencyType;
        }

        [DDSCAuthorizeAttribute]
        public JsonResult NotifyData(string Id)
        {
            NotifyQueryModel m_notifyModel = new NotifyQueryModel();
            m_notifyModel.RecUuid = Guid.Parse(Id);
            List<NotifyRecBE> NotificationList = g_notifyService.Use(s => s.QueryNotifyRec(m_notifyModel)).ToList();

            return Json(NotificationList[0].notify_data);
        }

    }
}