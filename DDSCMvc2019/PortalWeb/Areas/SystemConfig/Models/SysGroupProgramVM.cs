using Entity.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.ViewModel;

namespace PortalWeb.Areas.SystemConfig.Models
{
    public class SysGroupProgramVM : BaseVM
    {
        public SysGroupProgramVM()
        {
            StoreQueryName = new string[] { "GroupId" };
        }

        public List<SelectListItem> GroupNameList { get; set; }
        public string GroupId { get; set; }
        public List<SysGroupBE> SysGroupList { get; set; }
        public List<SysGroupProgramBE> SysGroupProgramList { get; set; }
        /// <summary>
        /// 統一編號下拉
        /// </summary>
        public List<SelectListItem> AaOrgList { get; set; }
        /// <summary>
        /// 角色下拉
        /// </summary>
        public List<SelectListItem> RoleList { get; set; }
        public SysGroupBE EditSysGroup { get; set; }
        public string FuncUuidList { get; set; }
    }
}