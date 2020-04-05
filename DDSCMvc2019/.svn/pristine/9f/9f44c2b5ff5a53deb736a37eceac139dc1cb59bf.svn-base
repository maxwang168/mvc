using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Attribute;
using WebLibrary.Controller;
using CommonLibrary.Service;
using PortalService.Contract;
using PortalService.Contract.ViewModel;
using Entity.SYS;
using log4net;
using PortalWeb.Areas.SystemConfig.Models;

namespace PortalWeb.Areas.SystemConfig.Controllers
{
    public class SysCodeInfoController : DDSCController
    {
        private Service<ISysService> g_service = new WCFService<ISysService>("wsHttpBinding_ISysService");
        private static ILog logger = LogManager.GetLogger(typeof(SysCodeInfoController));

        public SysCodeInfoController()
        {
            SessionName = "SysCodeInfo";
        }

        [DDSCAuthorize]
        public ActionResult Index()
        {
            SysCodeInfoVM m_viewModel = TempData[SessionName] as SysCodeInfoVM;
            if (m_viewModel == null)
            {
                m_viewModel = new SysCodeInfoVM();
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
                m_viewModel.SysCodeInfoList = ((SysCodeInfoVM)SessionObject).SysCodeInfoList;
            }
            return View(m_viewModel);
        }

        [HttpPost]
        [DDSCAuthorize, Log]
        public ActionResult Index([Bind(Exclude = "")]SysCodeInfoVM p_viewModel)
        {
            SysCodeInfoModel m_model = new SysCodeInfoModel();
            m_model.Type = p_viewModel.Type;
            m_model.Code = p_viewModel.Code;
            m_model.CodeName = p_viewModel.CodeName;
            if (!string.IsNullOrEmpty(p_viewModel.SelectedCategory))
            {
                m_model.Group = p_viewModel.SelectedCategory.Split('|')[1];
            }
            if (!string.IsNullOrEmpty(p_viewModel.SelectedSubCategory))
            {
                m_model.SubGroup = p_viewModel.SelectedSubCategory.Split('|')[1];
            }

            List<SysCodeInfoBE> m_codeList = g_service.Use(a => a.SysCodeInfoQuery(m_model)).ToList();
            p_viewModel.SysCodeInfoList = m_codeList;
            if (m_codeList == null || m_codeList.Count == 0)
            {
                p_viewModel.Message = "該條件查無資料";
            }
            SessionObject = p_viewModel;
            FormToSession(p_viewModel, SysCodeInfoVM.StoreQueryName);
            initQuery(p_viewModel);
            return View(p_viewModel);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            SysCodeInfoVM m_viewModel = (SysCodeInfoVM)SessionObject;
            return PartialView("_GridViewPartial", m_viewModel);
        }

        [DDSCAuthorize, Log]
        public ActionResult Show(string Id)
        {
            SysCodeInfoVM m_viewModel = new SysCodeInfoVM();
            if (Id != null)
            {
                List<SysCodeInfoBE> m_codeList = ((SysCodeInfoVM)SessionObject).SysCodeInfoList;
                List<SysCodeInfoBE> m_result = (from code in m_codeList
                                                    //where code.CodeUuid == new Guid(Ids[0])
                                                where code.CodeUuid == new Guid(Id)
                                                select code).ToList();
                m_viewModel.EditSysCodeInfo = m_result[0];
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
            SysCodeInfoVM m_viewModel = new SysCodeInfoVM();
            if (Id != null)
            {
                List<SysCodeInfoBE> m_codeList = ((SysCodeInfoVM)SessionObject).SysCodeInfoList;
                List<SysCodeInfoBE> m_result = (from code in m_codeList
                                                where code.CodeUuid == new Guid(Id)
                                                select code).ToList();
                m_viewModel.EditSysCodeInfo = m_result[0];
                m_viewModel.IsModify = true;

                if (m_viewModel.EditSysCodeInfo.Cate == "PARM|00000000-0000-0000-0000-000000000001")
                {
                    m_viewModel.Type = "R";
                }
                else if (string.IsNullOrWhiteSpace(m_viewModel.EditSysCodeInfo.SubGroupName))
                {
                    m_viewModel.Type = "S";
                }
                else
                {
                    m_viewModel.Type = "P";
                }
            }
            else
            {
                DateTime m_now = DateTime.Now;
                m_viewModel.EditSysCodeInfo = new SysCodeInfoBE();
                m_viewModel.EditSysCodeInfo.CreatedBy = LoginUser.UserInfo.user_uuid;
                m_viewModel.EditSysCodeInfo.CreatedByName = LoginUser.UserInfo.created_by_name;
                m_viewModel.EditSysCodeInfo.CreatedDate = m_now;
                m_viewModel.EditSysCodeInfo.UpdatedBy = LoginUser.UserInfo.user_uuid;
                m_viewModel.EditSysCodeInfo.UpdatedByName = LoginUser.UserInfo.user_name;
                m_viewModel.EditSysCodeInfo.UpdatedDate = m_now;
                m_viewModel.IsModify = false;
                m_viewModel.Type = "P";
            }
            TempData[SessionName] = m_viewModel;
            initQuery(m_viewModel);
            return View(m_viewModel);
        }

        [HttpPost]
        [DDSCAuthorize, Log]
        public ActionResult Edit([Bind(Exclude = "")]SysCodeInfoVM p_viewModel)
        {
            if (ModelState.IsValid)
            {
                SysCodeInfoVM m_viewModel = new SysCodeInfoVM();
                string Message = string.Empty;
                SessionToForm(p_viewModel);

                if (!p_viewModel.IsModify)
                {
                    if (CheckKey(p_viewModel.EditSysCodeInfo.CodeId, p_viewModel.EditSysCodeInfo.GroupId, p_viewModel.EditSysCodeInfo.Cate, p_viewModel.Type) > 0)
                    {
                        initQuery(p_viewModel);
                        p_viewModel.Message = "代碼重覆! 請重新輸入";
                        return View(p_viewModel);
                    }

                    p_viewModel.EditSysCodeInfo.CodeUuid = Guid.NewGuid();
                    p_viewModel.EditSysCodeInfo.CodeType = p_viewModel.Type;
                    switch (p_viewModel.Type)
                    {
                        case "R":
                            p_viewModel.EditSysCodeInfo.Cate = "PARM";
                            p_viewModel.EditSysCodeInfo.SuperUuid = new Guid("00000000-0000-0000-0000-000000000001");
                            break;
                        case "S":
                            p_viewModel.EditSysCodeInfo.Cate = p_viewModel.EditSysCodeInfo.GroupId.Split('|')[0].ToString();
                            p_viewModel.EditSysCodeInfo.SuperUuid = new Guid(p_viewModel.EditSysCodeInfo.GroupId.Split('|')[1].ToString());
                            break;
                        case "P":
                            p_viewModel.EditSysCodeInfo.SuperUuid = new Guid(p_viewModel.EditSysCodeInfo.Cate.Split('|')[1].ToString());
                            p_viewModel.EditSysCodeInfo.Cate = p_viewModel.EditSysCodeInfo.Cate.Split('|')[0].ToString();
                            break;
                    }

                    p_viewModel.EditSysCodeInfo.VarChar01 = getEncryptStr("", p_viewModel.EditSysCodeInfo.VarChar01, p_viewModel.EditSysCodeInfo.Encrypt01);
                    p_viewModel.EditSysCodeInfo.VarChar02 = getEncryptStr("", p_viewModel.EditSysCodeInfo.VarChar02, p_viewModel.EditSysCodeInfo.Encrypt02);
                    p_viewModel.EditSysCodeInfo.VarChar03 = getEncryptStr("", p_viewModel.EditSysCodeInfo.VarChar03, p_viewModel.EditSysCodeInfo.Encrypt03);
                    p_viewModel.EditSysCodeInfo.VarChar04 = getEncryptStr("", p_viewModel.EditSysCodeInfo.VarChar04, p_viewModel.EditSysCodeInfo.Encrypt04);
                    p_viewModel.EditSysCodeInfo.VarChar05 = getEncryptStr("", p_viewModel.EditSysCodeInfo.VarChar05, p_viewModel.EditSysCodeInfo.Encrypt05);
                    p_viewModel.EditSysCodeInfo.VarChar06 = getEncryptStr("", p_viewModel.EditSysCodeInfo.VarChar06, p_viewModel.EditSysCodeInfo.Encrypt06);
                    p_viewModel.EditSysCodeInfo.VarChar07 = getEncryptStr("", p_viewModel.EditSysCodeInfo.VarChar07, p_viewModel.EditSysCodeInfo.Encrypt07);
                    p_viewModel.EditSysCodeInfo.VarChar08 = getEncryptStr("", p_viewModel.EditSysCodeInfo.VarChar08, p_viewModel.EditSysCodeInfo.Encrypt08);
                    p_viewModel.EditSysCodeInfo.VarChar09 = getEncryptStr("", p_viewModel.EditSysCodeInfo.VarChar09, p_viewModel.EditSysCodeInfo.Encrypt09);
                    p_viewModel.EditSysCodeInfo.VarChar10 = getEncryptStr("", p_viewModel.EditSysCodeInfo.VarChar10, p_viewModel.EditSysCodeInfo.Encrypt09);

                    bool m_success = g_service.Use(s => s.insertSysCodeInfoBE(p_viewModel.EditSysCodeInfo));
                    if (m_success)
                    {
                        Message = "新增成功!";

                        SysCodeInfoModel m_model = new SysCodeInfoModel();
                        m_model.Type = p_viewModel.Type;
                        m_model.Code = p_viewModel.Code;
                        m_model.CodeName = p_viewModel.CodeName;
                        if (!string.IsNullOrEmpty(p_viewModel.SelectedCategory))
                        {
                            m_model.Group = p_viewModel.SelectedCategory.Split('|')[1];
                        }
                        if (!string.IsNullOrEmpty(p_viewModel.SelectedSubCategory))
                        {
                            m_model.SubGroup = p_viewModel.SelectedSubCategory.Split('|')[1];
                        }
                        List<SysCodeInfoBE> m_codeList = g_service.Use(a => a.SysCodeInfoQuery(m_model)).ToList();
                        p_viewModel.SysCodeInfoList = m_codeList;
                        SessionObject = p_viewModel;
                    }
                    else
                    {
                        Message = "新增失敗!";
                    }
                }
                else
                {
                    if (p_viewModel.Type != "P")
                    {
                        List<SysCodeInfoBE> m_codeList = ((SysCodeInfoVM)SessionObject).SysCodeInfoList;
                        List<SysCodeInfoBE> m_result = (from code in m_codeList
                                                        where code.CodeUuid == p_viewModel.EditSysCodeInfo.CodeUuid
                                                        select code).ToList();
                        p_viewModel.EditSysCodeInfo.CodeId = m_result[0].CodeId;
                    }
                    p_viewModel.EditSysCodeInfo.VarChar01 = getEncryptStr(p_viewModel.EditSysCodeInfo.OrgVarChar01, p_viewModel.EditSysCodeInfo.VarChar01, p_viewModel.EditSysCodeInfo.Encrypt01);
                    p_viewModel.EditSysCodeInfo.VarChar02 = getEncryptStr(p_viewModel.EditSysCodeInfo.OrgVarChar02, p_viewModel.EditSysCodeInfo.VarChar02, p_viewModel.EditSysCodeInfo.Encrypt02);
                    p_viewModel.EditSysCodeInfo.VarChar03 = getEncryptStr(p_viewModel.EditSysCodeInfo.OrgVarChar03, p_viewModel.EditSysCodeInfo.VarChar03, p_viewModel.EditSysCodeInfo.Encrypt03);
                    p_viewModel.EditSysCodeInfo.VarChar04 = getEncryptStr(p_viewModel.EditSysCodeInfo.OrgVarChar04, p_viewModel.EditSysCodeInfo.VarChar04, p_viewModel.EditSysCodeInfo.Encrypt04);
                    p_viewModel.EditSysCodeInfo.VarChar05 = getEncryptStr(p_viewModel.EditSysCodeInfo.OrgVarChar05, p_viewModel.EditSysCodeInfo.VarChar05, p_viewModel.EditSysCodeInfo.Encrypt05);
                    p_viewModel.EditSysCodeInfo.VarChar06 = getEncryptStr(p_viewModel.EditSysCodeInfo.OrgVarChar06, p_viewModel.EditSysCodeInfo.VarChar06, p_viewModel.EditSysCodeInfo.Encrypt06);
                    p_viewModel.EditSysCodeInfo.VarChar07 = getEncryptStr(p_viewModel.EditSysCodeInfo.OrgVarChar07, p_viewModel.EditSysCodeInfo.VarChar07, p_viewModel.EditSysCodeInfo.Encrypt07);
                    p_viewModel.EditSysCodeInfo.VarChar08 = getEncryptStr(p_viewModel.EditSysCodeInfo.OrgVarChar08, p_viewModel.EditSysCodeInfo.VarChar08, p_viewModel.EditSysCodeInfo.Encrypt08);
                    p_viewModel.EditSysCodeInfo.VarChar09 = getEncryptStr(p_viewModel.EditSysCodeInfo.OrgVarChar09, p_viewModel.EditSysCodeInfo.VarChar09, p_viewModel.EditSysCodeInfo.Encrypt09);
                    p_viewModel.EditSysCodeInfo.VarChar10 = getEncryptStr(p_viewModel.EditSysCodeInfo.OrgVarChar10, p_viewModel.EditSysCodeInfo.VarChar10, p_viewModel.EditSysCodeInfo.Encrypt09);
                    p_viewModel.EditSysCodeInfo.UpdatedBy = LoginUser.UserInfo.user_uuid;
                    p_viewModel.EditSysCodeInfo.UpdatedDate = DateTime.Now;

                    bool m_success = g_service.Use(s => s.updateSysCodeInfoBE(p_viewModel.EditSysCodeInfo));
                    if (m_success)
                    {
                        Message = "修改成功!";

                        SysCodeInfoModel m_model = new SysCodeInfoModel();
                        m_model.Type = p_viewModel.Type;
                        m_model.Code = p_viewModel.Code;
                        m_model.CodeName = p_viewModel.CodeName;
                        if (!string.IsNullOrEmpty(p_viewModel.SelectedCategory))
                        {
                            m_model.Group = p_viewModel.SelectedCategory.Split('|')[1];
                        }
                        if (!string.IsNullOrEmpty(p_viewModel.SelectedSubCategory))
                        {
                            m_model.SubGroup = p_viewModel.SelectedSubCategory.Split('|')[1];
                        }
                        List<SysCodeInfoBE> m_codeList = g_service.Use(a => a.SysCodeInfoQuery(m_model)).ToList();
                        p_viewModel.SysCodeInfoList = m_codeList;
                        SessionObject = p_viewModel;
                    }
                    else
                    {
                        Message = "修改失敗!";
                    }
                }

                m_viewModel.Message = Message;
                TempData[SessionName] = m_viewModel;
                initQuery(m_viewModel);
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
            SysCodeInfoVM m_viewModel = new SysCodeInfoVM();
            List<SysCodeInfoBE> m_codeList = ((SysCodeInfoVM)SessionObject).SysCodeInfoList;
            string Message = string.Empty;
            SessionToForm(m_viewModel);
            if (Id != null)
            {
                List<SysCodeInfoBE> m_result = (from code in m_codeList
                                                where code.CodeUuid == new Guid(Id)
                                                select code).ToList();
                string m_type = "P";
                //群組 R
                if (string.IsNullOrEmpty(m_result[0].GroupName) && string.IsNullOrEmpty(m_result[0].SubGroupName))
                {
                    m_type = "R";
                }
                //子群組 S
                else if (string.IsNullOrEmpty(m_result[0].SubGroupName))
                {
                    m_type = "S";
                }

                bool m_success = g_service.Use(s => s.deleteSysCodeInfoBE(m_type, m_result[0].CodeUuid.ToString()));
                if (m_success)
                {
                    m_codeList.Remove(m_result[0]);
                    m_viewModel.SysCodeInfoList = m_codeList;
                }
                if (m_success)
                {
                    Message = "刪除成功!";
                }
                else
                {
                    Message = "刪除失敗!";
                }
            }
            m_viewModel.Message = Message;
            TempData[SessionName] = m_viewModel;
            initQuery(m_viewModel);
            return RedirectToAction("Index");
        }

        public JsonResult SubGroup(string p_cate)
        {
            List<string[]> m_subList = g_service.Use(s => s.QuerySecondLevel(p_cate.Split('|')[1])).ToList();
            return Json(m_subList);
        }

        /// <summary>
        /// 頁面jquery檢查Key不重覆用
        /// </summary>
        /// <param name="p_code_id"></param>
        /// <param name="p_group_id"></param>
        /// <param name="p_cate"></param>
        /// <returns></returns>
        [DDSCAuthorize]
        public int CheckKey(string p_code_id, string p_group_id, string p_cate, string p_type)
        {
            string m_cate = string.Empty;
            if (p_cate != null && p_type == "P")
            {
                m_cate = p_cate.Split('|').Length == 2 ? p_cate.Split('|')[0] : p_cate;
            }
            else if (p_group_id != null && p_type == "S")
            {
                m_cate = p_group_id.Split('|').Length == 2 ? p_group_id.Split('|')[0] : p_group_id;
            }

            int m_cnt = 0;
            m_cnt = g_service.Use(a => a.SysCodeInfoQryCnt(p_code_id, m_cate));
            return m_cnt;
        }

        private string getEncryptStr(string p_orgStr, string p_str, bool p_isEncrypt)
        {
            string m_return = string.Empty;
            string m_ErrMsg = string.Empty;

            try
            {
                //勾選加密 & 不為空值 & 值有異動
                if (p_isEncrypt && !string.IsNullOrEmpty(p_str) && string.Compare(p_orgStr, p_str, false) != 0)
                {
                    m_return = CommonLibrary.DES.DESCode.desEncryptBase64(p_str, ref m_ErrMsg);

                    if (string.IsNullOrEmpty(m_ErrMsg))
                    {
                        logger.Error(m_ErrMsg);
                    }
                }
                else
                {
                    m_return = p_str;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }

            return m_return;
        }

        private void initQuery(SysCodeInfoVM p_viewModel)
        {
            List<SelectListItem> m_cat = new List<SelectListItem>();
            List<string[]> m_catStringList = g_service.Use(s => s.QueryFirstLevel()).ToList();
            for (int i = 0; i < m_catStringList.Count; i++)
            {
                SelectListItem m_item = new SelectListItem();
                m_item.Text = m_catStringList[i][0];
                m_item.Value = m_catStringList[i][1] + "|" + m_catStringList[i][2];
                m_cat.Add(m_item);
            }
            p_viewModel.CategoryList = m_cat;

            //修改時預設
            List<string[]> m_subList = new List<string[]>();
            if (p_viewModel.IsModify)
            {
                if (p_viewModel.Type == "P")
                {
                    m_subList = g_service.Use(s => s.QuerySecondLevel(p_viewModel.EditSysCodeInfo.GroupId.Split('|')[1])).ToList();
                }
            }
            else
            {
                //Index PostBack
                if (!string.IsNullOrEmpty(p_viewModel.SelectedCategory))
                {
                    m_subList = g_service.Use(s => s.QuerySecondLevel(p_viewModel.SelectedCategory.Split('|')[1])).ToList();
                }
            }

            List<SelectListItem> m_subCat = new List<SelectListItem>();
            if (m_subList.Count > 0)
            {
                for (int i = 0; i < m_subList.Count; i++)
                {
                    SelectListItem m_item = new SelectListItem();
                    m_item.Text = m_subList[i][0];
                    m_item.Value = m_subList[i][1] + "|" + m_subList[i][2];
                    m_subCat.Add(m_item);
                }
            }
            p_viewModel.SubCategory = m_subCat;
        }
    }
}