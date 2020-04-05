using CommonLibrary.Service;
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
    public class SysGroupProgramController : DDSCController
    {
        private Service<ISysService> g_service = new WCFService<ISysService>("wsHttpBinding_ISysService");
        private Service<IAaSysService> g_service2 = new WCFService<IAaSysService>("wsHttpBinding_IAaSysService");

        public SysGroupProgramController()
        {
            SessionName = "SysGroupProgram";
        }

        [DDSCAuthorize]
        public ActionResult Index()
        {
            SysGroupProgramVM m_viewModel = (SysGroupProgramVM)TempData[SessionName];
            if (m_viewModel == null)
            {
                m_viewModel = new SysGroupProgramVM();
                SessionObject = null;
            }
            else
            {
                SessionToForm(m_viewModel);
            }
            initQuery(m_viewModel);
            if (SessionObject != null)
            {
                m_viewModel.SysGroupList = ((SysGroupProgramVM)SessionObject).SysGroupList;
            }
            return View("Index", m_viewModel);
        }

        [HttpPost]
        [DDSCAuthorize]
        public ActionResult Index([Bind(Exclude = "")]SysGroupProgramVM p_viewModel)
        {
            SysGroupModel m_model = new SysGroupModel();
            m_model.GroupId = p_viewModel.GroupId;
            m_model.IsAdminGroup = LoginUser.UserInfo.admin_user;
            m_model.IsAdmin = LoginUser.UserInfo.role_id.ToLower() == "admin" ? true : false;

            List<SysGroupBE> m_groupList = g_service.Use(a => a.SysGroupQuery(m_model)).ToList();
            p_viewModel.SysGroupList = m_groupList;
            if (m_groupList == null || m_groupList.Count == 0)
            {
                p_viewModel.Message = "該條件查無資料";
            }
            SessionObject = p_viewModel;
            initQuery(p_viewModel);
            FormToSession(p_viewModel, SysGroupProgramVM.StoreQueryName);
            return View("Index", p_viewModel);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            SysGroupProgramVM m_model = (SysGroupProgramVM)SessionObject;
            return PartialView("_GridViewPartial", m_model);
        }

        [DDSCAuthorize]
        public ActionResult Edit(string Id)
        {
            SysGroupProgramVM m_viewModel = new SysGroupProgramVM();
            if (Id != null)
            {
                List<SysGroupBE> m_qryList = ((SysGroupProgramVM)SessionObject).SysGroupList;
                List<SysGroupBE> m_result = (from qry in m_qryList
                                             where qry.GroupUuid == new Guid(Id)
                                             select qry).ToList();
                m_viewModel.EditSysGroup = m_result[0];
                m_viewModel.IsModify = true;
            }
            else
            {
                m_viewModel.EditSysGroup = new SysGroupBE();
                m_viewModel.IsModify = false;
            }
            initQuery(m_viewModel);
            TempData[SessionName] = m_viewModel;
            return View("Edit", m_viewModel);
        }

        [HttpPost]
        [DDSCAuthorize]
        public ActionResult Edit([Bind(Exclude = "")]SysGroupProgramVM p_viewModel)
        {
            if (ModelState.IsValid)
            {
                SessionToForm(p_viewModel);
                if (CheckKey(p_viewModel.EditSysGroup.GroupId, p_viewModel.EditSysGroup.GroupUuid) > 0) //, p_viewModel.EditSysGroup.OrgId, p_viewModel.EditSysGroup.GroupUuid) > 0)
                {
                    initQuery(p_viewModel);
                    p_viewModel.Message = "角色重覆! 請重新輸入";
                    return View("Edit", p_viewModel);
                }

                SysGroupProgramVM m_viewModel = new SysGroupProgramVM();
                string Message = string.Empty;

                if (!p_viewModel.IsModify)
                {
                    p_viewModel.EditSysGroup.GroupUuid = Guid.NewGuid();
                    p_viewModel.EditSysGroup.CreatedBy = LoginUser.UserInfo.user_uuid;
                    p_viewModel.EditSysGroup.CreatedDate = DateTime.Now;
                    p_viewModel.EditSysGroup.UpdatedBy = LoginUser.UserInfo.user_uuid;
                    p_viewModel.EditSysGroup.UpdatedDate = DateTime.Now;
                    bool m_success = g_service.Use(s => s.insertSysGroupBE(p_viewModel.EditSysGroup));
                    if (m_success)
                    {
                        SysGroupModel m_model = new SysGroupModel();
                        m_model.GroupId = p_viewModel.GroupId;
                        m_model.IsAdminGroup = LoginUser.UserInfo.admin_user;
                        m_model.IsAdmin = LoginUser.UserInfo.role_id.ToLower() == "admin" ? true : false;

                        List<SysGroupBE> m_groupList = g_service.Use(a => a.SysGroupQuery(m_model)).ToList();
                        p_viewModel.SysGroupList = m_groupList;
                        SessionObject = p_viewModel;
                    }
                    if (m_success)
                    {
                        Message = "新增成功!";
                    }
                    else
                    {
                        Message = "新增失敗!";
                    }
                }
                else
                {
                    p_viewModel.EditSysGroup.UpdatedBy = LoginUser.UserInfo.user_uuid;
                    p_viewModel.EditSysGroup.UpdatedDate = DateTime.Now;
                    bool m_success = g_service.Use(s => s.updateSysGroupBE(p_viewModel.EditSysGroup));
                    if (m_success)
                    {
                        SysGroupModel m_model = new SysGroupModel();
                        m_model.GroupId = p_viewModel.GroupId;
                        m_model.IsAdminGroup = LoginUser.UserInfo.admin_user;
                        m_model.IsAdmin = LoginUser.UserInfo.role_id.ToLower() == "admin" ? true : false;

                        List<SysGroupBE> m_groupList = g_service.Use(a => a.SysGroupQuery(m_model)).ToList();
                        p_viewModel.SysGroupList = m_groupList;
                        SessionObject = p_viewModel;
                    }
                    if (m_success)
                    {
                        Message = "修改成功!";
                    }
                    else
                    {
                        Message = "修改失敗!";
                    }
                }
                p_viewModel.Message = Message;
                initQuery(p_viewModel);
                TempData[SessionName] = p_viewModel;
                return RedirectToAction("Index");
            }
            else
            {
                initQuery(p_viewModel);
                return View("Edit", p_viewModel);
            }
        }

        [DDSCAuthorize]
        public ActionResult Show(string Id)
        {
            SysGroupProgramVM m_viewModel = new SysGroupProgramVM();
            if (Id != null)
            {
                List<SysGroupBE> m_qryList = ((SysGroupProgramVM)SessionObject).SysGroupList;
                List<SysGroupBE> m_result = (from qry in m_qryList
                                             where qry.GroupUuid == new Guid(Id)
                                             select qry).ToList();
                m_viewModel.EditSysGroup = m_result[0];
                TempData[SessionName] = m_viewModel;
                return View("Show", m_viewModel);
            }
            else
            {
                m_viewModel.Message = "找不到該筆資料!";
                initQuery(m_viewModel);
                return RedirectToAction("Index");
            }
        }

        [DDSCAuthorize]
        public ActionResult EditProgram(string Id)
        {
            SysGroupProgramVM m_viewModel = ((SysGroupProgramVM)SessionObject);
            m_viewModel.Message = string.Empty;
            if (Id != null)
            {
                List<SysGroupBE> m_result = (from qry in m_viewModel.SysGroupList
                                             where qry.GroupUuid == new Guid(Id)
                                             select qry).ToList();

                m_viewModel.GroupId = m_result[0].GroupId;

                SysGroupProgramModel m_model = new SysGroupProgramModel();
                m_model.GroupUuid = new Guid(Id);
                List<SysGroupProgramBE> m_groupProgramsList = g_service.Use(a => a.SysGroupProgramQuery(m_model)).ToList();
                m_viewModel.SysGroupProgramList = m_groupProgramsList;
                SessionObject = m_viewModel;

                string m_selectedRows = string.Empty;
                for (int i = 0; i < m_groupProgramsList.Count; i++)
                {
                    if (m_groupProgramsList[i].StatusFlag)
                    {
                        if (!string.IsNullOrEmpty(m_selectedRows))
                        {
                            m_selectedRows += "|";
                        }
                        m_selectedRows += m_groupProgramsList[i].FuncUuid;
                    }
                }
                ViewData["selectedRows"] = m_selectedRows.Split('|');
                TempData["ViewModel"] = m_viewModel;
                return View("EditProgram", m_viewModel);
            }
            else
            {
                m_viewModel.Message = "找不到該筆資料!";
                initQuery(m_viewModel);
                //return View("Index", m_viewModel);
                return RedirectToAction("Index");
            }
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartialEdit()
        {
            SysGroupProgramVM m_model = ((SysGroupProgramVM)SessionObject);
            return PartialView("_GridViewPartialEdit", m_model);
        }

        [DDSCAuthorize]
        public JsonResult Save(string p_funcUuidList)
        {
            if (p_funcUuidList != null)
            {
                string[] m_list = p_funcUuidList.Split('|');
                SysGroupProgramVM m_viewModel = ((SysGroupProgramVM)SessionObject);
                List<SysGroupProgramBE> m_qryList = m_viewModel.SysGroupProgramList;
                List<SysGroupProgramBE> m_result = (from qry in m_qryList
                                                    where m_list.Contains(qry.FuncUuid.ToString())
                                                    select qry).ToList();

                bool m_success = g_service.Use(s => s.modifySysGroupProgramBE(m_result));

                if (m_success)
                {
                    m_viewModel.Message = "儲存成功!";
                }
                else
                {
                    m_viewModel.Message = "儲存失敗!";
                }

                initQuery(m_viewModel);
                TempData["ViewModel"] = m_viewModel;
                return Json(string.Empty);
            }
            else
            {
                //bool m_success = g_service.Use(s => s.deleteSysGroupBE(m_result[0].GroupUuid));
                return Json(string.Empty);
            }
        }

        /// <summary>
        /// 頁面jquery檢查Key不重覆用
        /// </summary>
        /// <param name="p_user_id"></param>
        /// <returns></returns>
        [DDSCAuthorize]
        public int CheckKey(string p_groupId, Guid p_groupUuid) //, string p_orgId, Guid p_groupUuid)
        {
            int m_cnt = 0;
            m_cnt = g_service.Use(a => a.SysGroupProgramQryCnt(p_groupId, p_groupUuid)); //, p_orgId, p_groupUuid));
            return m_cnt;
        }

        /// <summary>
        /// 取得 OrgId
        /// </summary>
        /// <param name="p_groupId"></param>
        /// <returns></returns>
        [DDSCAuthorize]
        public JsonResult getOrgId(string p_groupId)
        {
            string m_orgId = string.Empty;
            m_orgId = g_service2.Use(a => a.GetOrgId(p_groupId));
            return Json(m_orgId);
        }

        private void initQuery(SysGroupProgramVM p_viewModel)
        {
            bool isAdmin = LoginUser.UserInfo.role_id.ToLower() == "admin" ? true : false;

            List<string[]> m_groupList = g_service.Use(s => s.QueryGroupName(LoginUser.UserInfo.admin_user, isAdmin)).ToList();
            List<SelectListItem> m_role = new List<SelectListItem>();
            SelectListItem m_fist = new SelectListItem();
            m_fist.Text = "請選擇";
            m_fist.Value = string.Empty;
            m_role.Add(m_fist);
            for (int i = 0; i < m_groupList.Count; i++)
            {
                SelectListItem m_item = new SelectListItem();
                m_item.Text = string.Format("{0}-{1}", m_groupList[i][0].Trim(), m_groupList[i][1].Trim());
                m_item.Value = m_groupList[i][0].Trim();
                m_role.Add(m_item);
            }
            p_viewModel.GroupNameList = m_role;

            List<SelectListItem> m_sliList = new List<SelectListItem>();
            List<string[]> m_sList = g_service2.Use(s => s.QueryAllRole("ROLE", LoginUser.UserInfo.admin_user, isAdmin).ToList());
            for (int i = 0; i < m_sList.Count; i++)
            {
                SelectListItem m_item = new SelectListItem();
                m_item.Text = m_sList[i][1] + "-" + m_sList[i][2]; //id-name
                m_item.Value = m_sList[i][0]; //uuid
                m_sliList.Add(m_item);
            }
            p_viewModel.RoleList = m_sliList;
        }
    }
}