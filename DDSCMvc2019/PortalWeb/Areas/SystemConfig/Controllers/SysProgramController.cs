using CommonLibrary.Service;
using Entity;
using Entity.SYS;
using PortalService.Contract;
using PortalService.Contract.ViewModel;
using PortalWeb.Areas.SystemConfig.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Attribute;
using WebLibrary.Controller;

namespace PortalWeb.Areas.SystemConfig.Controllers
{
    public class SysProgramController : DDSCController
    {
        private Service<ISysService> g_service = new WCFService<ISysService>("wsHttpBinding_ISysService");

        public SysProgramController()
        {
            SessionName = "SysProgram";
        }

        [DDSCAuthorize]
        public ActionResult Index()
        {
            SysProgramVM m_viewModel = TempData[SessionName] as SysProgramVM;
            if (m_viewModel == null)
            {
                m_viewModel = new SysProgramVM();
                SessionObject = null;
            }
            else
            {
                SessionToForm(m_viewModel);
            }
            m_viewModel.IsModify = false;
            initQuery(m_viewModel);

            if (SessionObject != null)
            {
                m_viewModel.SysProgramList = ((SysProgramVM)SessionObject).SysProgramList;
            }

            return View(m_viewModel);
        }

        [HttpPost]
        [DDSCAuthorize, Log]
        public ActionResult Index([Bind(Exclude = "")]SysProgramVM p_viewModel)
        {
            SysProgramModel m_model = new SysProgramModel();
            m_model.ProgramType = p_viewModel.ProgramType;
            m_model.Menu = p_viewModel.SelectedMenu;
            m_model.SubMenu = p_viewModel.SelectedSubMenu;
            m_model.ProgramID = p_viewModel.ProgramID;
            m_model.ProgramName = p_viewModel.ProgramName;
            List<SysProgramBE> m_programList = g_service.Use(s => s.SysProgramQuery(m_model)).ToList();
            p_viewModel.SysProgramList = m_programList;
            SessionObject = p_viewModel;
            if (m_programList == null || m_programList.Count == 0)
            {
                p_viewModel.Message = "該條件查無資料";
            }
            //將畫面上的查詢條件存入Session
            FormToSession(p_viewModel, SysProgramVM.StoreQueryName);

            initQuery(p_viewModel);
            return View(p_viewModel);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            List<SysProgramBE> m_codeList = ((SysProgramVM)SessionObject).SysProgramList;
            SysProgramVM m_model = new SysProgramVM();
            m_model.SysProgramList = m_codeList;
            return PartialView("_GridViewPartial", m_model);
        }

        [DDSCAuthorize, Log]
        public ActionResult Show(string Id)
        {
            SysProgramVM m_viewModel = new SysProgramVM();
            if (Id != null)
            {
                List<SysProgramBE> m_programList = ((SysProgramVM)SessionObject).SysProgramList;
                List<SysProgramBE> m_result = (from program in m_programList
                                               where program.func_uuid == Id
                                               select program).ToList();
                m_viewModel.EditSysProgram = m_result[0];
                m_viewModel.IsModify = true;
                TempData[SessionName] = m_viewModel;
                return View(m_viewModel);
            }
            else
            {
                m_viewModel.Message = "找不到該筆資料!";
                initQuery(m_viewModel);
                return RedirectToAction("Index");
            }
        }

        [DDSCAuthorize]
        public ActionResult Edit(string Id)
        {
            SysProgramVM m_viewModel = new SysProgramVM();
            if (Id != null)
            {
                List<SysProgramBE> m_programList = ((SysProgramVM)SessionObject).SysProgramList;
                List<SysProgramBE> m_result = (from program in m_programList
                                               where program.func_uuid == Id
                                               select program).ToList();
                m_viewModel.EditSysProgram = m_result[0];
                m_viewModel.IsModify = true;
            }
            else
            {
                m_viewModel.IsModify = false;
            }
            initQuery(m_viewModel);
            return View(m_viewModel);
        }

        [HttpPost]
        [DDSCAuthorize, Log]
        public ActionResult Edit([Bind(Exclude = "")]SysProgramVM p_viewModel)
        {
            SysProgramVM m_viewModel = new SysProgramVM();
            UserData m_userData = (UserData)Session["UserData"];
            string m_msg = string.Empty;
            if (ModelState.IsValid)
            {
                switch (p_viewModel.EditSysProgram.program_type)
                {
                    case "F":  //parent_id = 子選單
                        if (p_viewModel.EditSysProgram.SubMenu == "")
                            p_viewModel.EditSysProgram.parent_id = p_viewModel.EditSysProgram.Menu;
                        else
                            p_viewModel.EditSysProgram.parent_id = p_viewModel.EditSysProgram.SubMenu;
                        break;

                    case "M":  //parent.ID = 主選單
                        p_viewModel.EditSysProgram.parent_id = p_viewModel.EditSysProgram.Menu;
                        break;

                    case "A":  //parent.ID = MAIN
                        //p_viewModel.EditSysProgram.parent_id = "MAIN";
                        p_viewModel.EditSysProgram.parent_id = "1AF0ED79-B6DE-45F9-9B39-2FC87240F087";
                        p_viewModel.EditSysProgram.isView = true;
                        break;
                }
                if (!p_viewModel.IsModify)
                {
                    p_viewModel.EditSysProgram.CREATE_USER_UUID = m_userData.UserInfo.user_uuid.ToString(); //"415B07D5-5645-4B02-A3AE-D953E85D75DE";//m_userData.SysUserInfo.uid.ToString();//"415B07D5-5645-4B02-A3AE-D953E85D75DE"; //m_userData.SysUserInfo.uid.ToString();
                    p_viewModel.EditSysProgram.CREATE_DATE = DateTime.Now;
                    p_viewModel.EditSysProgram.MODIFY_USER_UUID = m_userData.UserInfo.user_uuid.ToString(); //"415B07D5-5645-4B02-A3AE-D953E85D75DE";//m_userData.SysUserInfo.uid.ToString();// "415B07D5-5645-4B02-A3AE-D953E85D75DE"; //m_userData.SysUserInfo.uid.ToString();
                    p_viewModel.EditSysProgram.MODIFY_DATE = DateTime.Now;

                    bool m_success = g_service.Use(s => s.insertSysProgramBE(p_viewModel.EditSysProgram));
                    if (m_success)
                    {
                        m_msg = "新增成功!";
                    }
                    else
                    {
                        m_msg = "新增失敗!";
                    }
                }
                else
                {
                    p_viewModel.EditSysProgram.MODIFY_USER_UUID = m_userData.UserInfo.user_uuid.ToString(); //"415B07D5-5645-4B02-A3AE-D953E85D75DE";//m_userData.SysUserInfo.uid.ToString();
                    p_viewModel.EditSysProgram.MODIFY_DATE = DateTime.Now;

                    bool m_success = g_service.Use(s => s.updateSysProgramBE(p_viewModel.EditSysProgram));
                    if (m_success)
                    {
                        m_msg = "修改成功!";
                    }
                    else
                    {
                        m_msg = "修改失敗!";
                    }
                }
                SessionToForm(m_viewModel);
                m_viewModel.Message = m_msg;
                TempData["ViewModel"] = m_viewModel;
                return RedirectToAction("Index");
            }
            else
            {
                initQuery(p_viewModel);
                return View(p_viewModel);
            }
        }

        [HttpPost]
        [DDSCAuthorize, Log]
        public ActionResult Delete(string Id)
        {
            SysProgramVM m_viewModel = new SysProgramVM();
            List<SysProgramBE> m_programList = (List<SysProgramBE>)Session["Result"];
            string m_msg = string.Empty;
            if (Id != null)
            {
                List<SysProgramBE> m_result = (from program in m_programList
                                               where program.func_uuid == Id
                                               select program).ToList();
                bool m_success = g_service.Use(s => s.deleteSysProgramBE(m_result[0].func_uuid));
                //if (m_success)
                //{
                //    m_programList.Remove(m_result[0]);
                //}
                if (m_success)
                {
                    m_msg = "刪除成功!";
                }
                else
                {
                    m_msg = "刪除失敗!";
                }
            }

            SessionToForm(m_viewModel);
            m_viewModel.Message = m_msg;
            //initQuery(m_viewModel);
            TempData["ViewModel"] = m_viewModel;
            return RedirectToAction("Index");
        }

        public JsonResult SubMenu(string p_parent)
        {
            List<string[]> m_subList = g_service.Use(s => s.QuerySubMenu(p_parent)).ToList();
            return Json(m_subList);
        }


        private void initQuery(SysProgramVM p_viewModel)
        {
            List<string[]> m_menuStringList = g_service.Use(s => s.QueryMenu("")).ToList();
            List<SelectListItem> m_menu = new List<SelectListItem>();
            SelectListItem m_fist = new SelectListItem();
            m_fist.Text = "請選擇";
            m_fist.Value = string.Empty;
            m_menu.Add(m_fist);
            for (int i = 0; i < m_menuStringList.Count; i++)
            {
                if (m_menuStringList[i][2] == "M")
                    continue;

                SelectListItem m_item = new SelectListItem();
                m_item.Value = m_menuStringList[i][0];
                m_item.Text = m_menuStringList[i][1];
                m_menu.Add(m_item);
            }
            p_viewModel.MenuList = m_menu;
            List<SelectListItem> m_subMenu = new List<SelectListItem>();
            m_subMenu.Add(m_fist);

            //修改時預設
            List<string[]> m_subList = new List<string[]>();
            if (p_viewModel.IsModify)
            {
                if (p_viewModel.EditSysProgram.Menu.ToUpper() == "1AF0ED79-B6DE-45F9-9B39-2FC87240F087")
                {
                    p_viewModel.EditSysProgram.Menu = p_viewModel.EditSysProgram.parent_id;
                }
                if (p_viewModel.EditSysProgram.program_type == "F" || p_viewModel.EditSysProgram.program_type == "M")
                {
                    m_subList = g_service.Use(s => s.QuerySubMenu(p_viewModel.EditSysProgram.Menu)).ToList();
                }

            }
            else
            {
                //Index PostBack
                if (!string.IsNullOrEmpty(p_viewModel.SelectedMenu))
                {
                    m_subList = g_service.Use(s => s.QuerySubMenu(p_viewModel.SelectedMenu)).ToList();
                }
                //Edit PostBack
                if (p_viewModel.EditSysProgram != null && !string.IsNullOrEmpty(p_viewModel.EditSysProgram.SubMenu))
                {
                    m_subList = g_service.Use(s => s.QuerySubMenu(p_viewModel.EditSysProgram.Menu)).ToList();
                }
            }

            if (m_subList.Count > 0)
            {
                for (int i = 0; i < m_subList.Count; i++)
                {
                    SelectListItem m_item = new SelectListItem();
                    m_item.Value = m_subList[i][0];
                    m_item.Text = m_subList[i][1];
                    m_subMenu.Add(m_item);
                }
            }
            p_viewModel.SubMenuList = m_subMenu;
        }
    }
}