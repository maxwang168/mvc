using Entity.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebLibrary.ViewModel;

namespace PortalWeb.Areas.SystemConfig.Models
{
    [Serializable]
    public class SysCodeInfoVM : BaseVM
    {
        public string Type { get; set; }
        public string Code { get; set; } = string.Empty;
        public string CodeName { get; set; } = string.Empty;
        public List<SelectListItem> CategoryList { get; set; }
        public List<SelectListItem> SubCategory { get; set; }
        public string SelectedCategory { get; set; } = string.Empty;
        public string SelectedSubCategory { get; set; } = string.Empty;
        public List<SysCodeInfoBE> SysCodeInfoList { get; set; }
        public SysCodeInfoBE EditSysCodeInfo { get; set; }
        public List<SelectListItem> EditList { get; set; }
        public List<SelectListItem> StatusList { get; set; }

        public SysCodeInfoVM()
        {
            StoreQueryName = new string[] { "SelectedCategory", "SelectedSubCategory", "Code", "CodeName" };
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string[] m_item;

            if (EditSysCodeInfo == null)
            {
                sb.Append("條件:");
                sb.Append("群組:");
                
                sb.Append(";");
                sb.Append("子群組:");
                
                sb.Append(";");
                sb.Append("代碼:");
                sb.Append(Code);
                sb.Append(";");
                sb.Append("代碼名稱:");
                sb.Append(CodeName);
            }
            else
            {
                sb.Append("編輯:");
                sb.Append("種類：");
                switch (Type)
                {
                    case "P":
                        sb.Append("參數");
                        break;
                    case "S":
                        sb.Append("子群組");
                        break;
                    case "R":
                        sb.Append("群組");
                        break;
                    default:
                        sb.Append(Type);
                        break;
                }
                sb.Append(";");
                sb.Append("群組：");
                
                sb.Append(";");
                sb.Append("子群組：");
                
                sb.Append(";");
                sb.Append("代碼：");
                sb.Append(EditSysCodeInfo.CodeId);
                sb.Append(";");
                sb.Append("代碼名稱：");
                sb.Append(EditSysCodeInfo.CodeName);
                sb.Append(";");
                sb.Append("顯示順序：");
                sb.Append(EditSysCodeInfo.Seq);
                sb.Append(";");
                sb.Append("可修改：");
                switch (EditSysCodeInfo.ModifyStatus)
                {
                    case "Y":
                        sb.Append("是");
                        break;
                    case "N":
                        sb.Append("否");
                        break;
                    default:
                        sb.Append(EditSysCodeInfo.ModifyStatus);
                        break;
                }
                sb.Append(";");
                sb.Append("資料狀態：");
                switch (EditSysCodeInfo.StatusFlag)
                {
                    case "Y":
                        sb.Append("有效");
                        break;
                    case "N":
                        sb.Append("無效");
                        break;
                    default:
                        sb.Append(EditSysCodeInfo.StatusFlag);
                        break;
                }
                sb.Append(";");
                sb.Append("說明：");
                sb.Append(EditSysCodeInfo.Description);
                sb.Append(";");
                sb.Append("參數1：");
                sb.Append(EditSysCodeInfo.VarChar01);
                sb.Append(";");
                sb.Append("參數2：");
                sb.Append(EditSysCodeInfo.VarChar02);
                sb.Append(";");
                sb.Append("參數3：");
                sb.Append(EditSysCodeInfo.VarChar03);
                sb.Append(";");
                sb.Append("參數4：");
                sb.Append(EditSysCodeInfo.VarChar04);
                sb.Append(";");
                sb.Append("參數5：");
                sb.Append(EditSysCodeInfo.VarChar05);
                sb.Append(";");
                sb.Append("參數6：");
                sb.Append(EditSysCodeInfo.VarChar06);
                sb.Append(";");
                sb.Append("參數7：");
                sb.Append(EditSysCodeInfo.VarChar07);
                sb.Append(";");
                sb.Append("參數8：");
                sb.Append(EditSysCodeInfo.VarChar08);
                sb.Append(";");
                sb.Append("參數9：");
                sb.Append(EditSysCodeInfo.VarChar09);
                sb.Append(";");
                sb.Append("參數10：");
                sb.Append(EditSysCodeInfo.VarChar10);
                sb.Append(";");
                sb.Append("建立者：");
                sb.Append(EditSysCodeInfo.CreatedByName);
                sb.Append(";");
                sb.Append("建立時間：");
                sb.Append(EditSysCodeInfo.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss"));
                sb.Append(";");
                sb.Append("異動者：");
                sb.Append(EditSysCodeInfo.UpdatedByName);
                sb.Append(";");
                sb.Append("異動時間：");
                sb.Append(EditSysCodeInfo.UpdatedDate.ToString("yyyy/MM/dd HH:mm:ss"));
            }
            return sb.ToString();
        }
    }
}