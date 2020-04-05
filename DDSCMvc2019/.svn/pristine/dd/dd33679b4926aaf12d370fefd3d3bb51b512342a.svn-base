using Entity.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.ViewModel;

namespace PortalWeb.Areas.SystemConfig.Models
{
    public class SysProgramVM : BaseVM
    {
        public string ProgramID { get; set; }
        public string ProgramName { get; set; }
        public string ProgramType { get; set; }
        public List<SelectListItem> MenuList { get; set; }
        public List<SelectListItem> SubMenuList { get; set; }
        public string SelectedMenu { get; set; } = string.Empty;
        public string SelectedSubMenu { get; set; } = string.Empty;
        public List<SysProgramBE> SysProgramList { get; set; }
        public SysProgramBE EditSysProgram { get; set; }
        public List<SelectListItem> EditList { get; set; }
        public List<SelectListItem> StatusList { get; set; }

        public SysProgramVM()
        {
            StoreQueryName = new string[] { "ProgramID", "ProgramName", "ProgramType", "SelectedMenu", "SelectedSubMenu" };
        }
    }
}