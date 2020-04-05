using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.SYS
{
    public class SysGroupBE
    {
        #region 成員變數
        private Guid g_groupUuid;
        private string g_systemId;
        private string g_orgId;
        private string g_groupId;
        private string g_groupName;
        private bool g_adminGroup;
        private string g_statusFlag;
        private Guid g_createdBy;
        private string g_createdByName;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private string g_updatedByName;
        private DateTime g_updatedDate;

        private Guid g_orgUuid;
        private string g_orgName;
        private string g_orgIdName;
        private Guid g_roleUuid;
        private string g_groupIdName;

        #endregion

        #region Property

        [Display(Name = "GroupUuid")]
        public Guid GroupUuid
        {
            get { return g_groupUuid; }
            set { g_groupUuid = value; }
        }
        [Display(Name = "SystemId")]
        public string SystemId
        {
            get { return g_systemId; }
            set { g_systemId = value; }
        }

        [Display(Name = "OrgId")]
        public string OrgId
        {
            get { return g_orgId; }
            set { g_orgId = value; }
        }

        [Display(Name = "GroupId")]
        public string GroupId
        {
            get { return g_groupId; }
            set { g_groupId = value; }
        }

        [Display(Name = "GroupName")]
        public string GroupName
        {
            get { return g_groupName; }
            set { g_groupName = value; }
        }

        [Display(Name = "AdminGroup")]
        public bool AdminGroup
        {
            get { return g_adminGroup; }
            set { g_adminGroup = value; }
        }

        [Display(Name = "StatusFlag")]
        [StringLength(1)]
        public string StatusFlag
        {
            get { return g_statusFlag; }
            set { g_statusFlag = value; }
        }

        [Display(Name = "建立者代號")]
        public Guid CreatedBy
        {
            get { return g_createdBy; }
            set { g_createdBy = value; }
        }

        [Display(Name = "建立者名稱")]
        public string CreatedByName
        {
            get { return g_createdByName; }
            set { g_createdByName = value; }
        }

        [Display(Name = "建立日期")]
        public DateTime CreatedDate
        {
            get { return g_createdDate; }
            set { g_createdDate = value; }
        }

        [Display(Name = "異動者代號")]
        public Guid UpdatedBy
        {
            get { return g_updatedBy; }
            set { g_updatedBy = value; }
        }

        [Display(Name = "異動者代號")]
        public string UpdatedByName
        {
            get { return g_updatedByName; }
            set { g_updatedByName = value; }
        }

        [Display(Name = "異動日期")]
        public DateTime UpdatedDate
        {
            get { return g_updatedDate; }
            set { g_updatedDate = value; }
        }

        [Display(Name = "OrgUuid")]
        public Guid OrgUuid
        {
            get { return g_orgUuid; }
            set { g_orgUuid = value; }
        }

        [Display(Name = "OrgName")]
        public string OrgName
        {
            get { return g_orgName; }
            set { g_orgName = value; }
        }

        [Display(Name = "OrgIdName")]
        public string OrgIdName
        {
            get { return g_orgIdName; }
            set { g_orgIdName = value; }
        }

        [Display(Name = "RoleUuid")]
        public Guid RoleUuid
        {
            get { return g_roleUuid; }
            set { g_roleUuid = value; }
        }

        [Display(Name = "GroupIdName")]
        public string GroupIdName
        {
            get { return g_groupIdName; }
            set { g_groupIdName = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_groupUuid = new Guid();
            g_systemId = "CSFM";
            g_orgId = string.Empty;
            g_groupId = string.Empty;
            g_groupName = string.Empty;
            g_adminGroup = false;
            g_statusFlag = "Y";
            g_createdBy = new Guid();
            g_createdByName = string.Empty;
            g_createdDate = new DateTime();
            g_updatedBy = new Guid();
            g_updatedByName = string.Empty;
            g_updatedDate = new DateTime();

            g_orgUuid = new Guid();
            g_orgName = string.Empty;
            g_orgIdName = string.Empty;
            g_roleUuid = new Guid();
            g_groupIdName = string.Empty;
        }

        #endregion

        #region 建構函數/Dispose

        public SysGroupBE()
        {
            resetVariables();
        }

        #endregion

    }
}
