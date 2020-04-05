using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    public class SysProgramBE
    {
        private string g_funcUUID;
        private string g_systemID;
        private string g_statusFlag;

        private string g_programID;
        private string g_parentID;
        private string g_programDesc;
        private string g_programUrl;
        private string g_menu;
        private string g_subMenu;

        public string flow_name { get; set; }
        public string func_uuid
        {
            get
            {
                return g_funcUUID;
            }
            set
            {
                if (value == null)
                    g_funcUUID = Guid.NewGuid().ToString();
                else
                    g_funcUUID = value.Trim();
            }
        }
        public string system_id
        {
            get
            {
                return g_systemID;
            }
            set
            {
                if (value == null)
                    g_systemID = "trade";
                else
                    g_systemID = value.Trim();
            }
        }
        public string status_flag
        {
            get
            {
                return g_statusFlag;
            }
            set
            {
                if (value == null)
                    g_statusFlag = "Y";
                else
                    g_statusFlag = value.Trim();
            }
        }

        [Display(Name = "程式代碼")]
        [Required]
        [StringLength(15)]
        public string program_id
        {
            get
            {
                return g_programID;
            }
            set
            {
                if (value == null)
                    g_programID = string.Empty;
                else
                    g_programID = value.Trim();
            }
        }
        public string program_type { get; set; }

        [Display(Name = "程式名稱")]
        [Required]
        [StringLength(40)]
        public string program_name { get; set; }
        public string program_desc
        {
            get
            {
                return g_programDesc;
            }
            set
            {
                if (value == null)
                    g_programDesc = string.Empty;
                else
                    g_programDesc = value.Trim();
            }
        }
        public string parent_id
        {
            get
            {
                return g_parentID;
            }
            set
            {
                if (value == null)
                    g_parentID = string.Empty;
                else
                    g_parentID = value.Trim();
            }
        }

        [Display(Name = "顯示順序")]
        [Required]
        public int seq_no { get; set; }
        public string func_source { get; set; }

        [Display(Name = "程式URL")]
        [StringLength(200)]
        public string program_url
        {
            get
            {
                if (g_programUrl == null)
                {
                    g_programUrl = string.Empty;
                }
                return g_programUrl;
            }
            set
            {
                g_programUrl = value;
            }
        }
        public bool isValid { get; set; }
        public bool isAddNew { get; set; }
        public bool isUpdate { get; set; }
        public bool isDelete { get; set; }
        public bool isQuery { get; set; }
        public bool isView { get; set; }
        public bool isPrint { get; set; }
        public bool isImport { get; set; }
        public bool isExport { get; set; }
        public bool isExecute { get; set; }
        public bool isShow { get; set; }
        public string CREATE_USER_UUID { get; set; }
        public string CREATE_USER_NAME { get; set; }
        public string CREATE_ORG_ID { get; set; }
        public System.DateTime CREATE_DATE { get; set; }
        public string MODIFY_USER_UUID { get; set; }
        public string MODIFY_USER_NAME { get; set; }
        public string MODIFY_ORG_ID { get; set; }
        public System.DateTime MODIFY_DATE { get; set; }
        public string SubProgramId { get; set; }
        public string SubProgramName { get; set; }
        public string RootProgramId { get; set; }
        public string RootProgramName { get; set; }
        public string GRootProgramName { get; set; }

        [Display(Name = "主選單")]
        public string Menu
        {
            get
            {
                return g_menu;
            }
            set
            {
                if (value == null)
                    g_menu = string.Empty;
                else
                    g_menu = value.Trim();
            }
        }

        [Display(Name = "子選單")]
        public string SubMenu
        {
            get
            {
                return g_subMenu;
            }
            set
            {
                if (value == null)
                    g_subMenu = string.Empty;
                else
                    g_subMenu = value.Trim();
            }
        }
        public string func_entity { get; set; }
        public string table_name { get; set; }
        public string key_name { get; set; }
        public string func_class { get; set; }
    }
}
