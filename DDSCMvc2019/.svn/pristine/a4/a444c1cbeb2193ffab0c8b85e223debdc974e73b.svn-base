using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.SYS
{
    public class SysGroupProgramBE
    {
        #region 成員變數
        private Guid g_permissionUuid;
        private Guid g_groupUuid;
        private Guid g_funcUuid;
        private Guid g_createdBy;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private DateTime g_updatedDate;

        private int g_sn;
        private string g_groupId;
        private string g_funcId;
        private string g_funcName;
        private string g_funcType;
        private string g_funcTypeName;
        private bool g_statusFlag;

        #endregion

        #region Property

        [Display(Name = "PermissionUuid")]
        public Guid PermissionUuid
        {
            get { return g_permissionUuid; }
            set { g_permissionUuid = value; }
        }
        [Display(Name = "GroupUuid")]
        public Guid GroupUuid
        {
            get { return g_groupUuid; }
            set { g_groupUuid = value; }
        }

        [Display(Name = "FuncUuid")]
        public Guid FuncUuid
        {
            get { return g_funcUuid; }
            set { g_funcUuid = value; }
        }

        [Display(Name = "建立者代號")]
        public Guid CreatedBy
        {
            get { return g_createdBy; }
            set { g_createdBy = value; }
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

        [Display(Name = "異動日期")]
        public DateTime UpdatedDate
        {
            get { return g_updatedDate; }
            set { g_updatedDate = value; }
        }

        public int Sn
        {
            get { return g_sn; }
            set { g_sn = value; }
        }

        public string GroupId
        {
            get { return g_groupId; }
            set { g_groupId = value; }
        }

        public string FuncId
        {
            get { return g_funcId; }
            set { g_funcId = value; }
        }

        public string FuncName
        {
            get { return g_funcName; }
            set { g_funcName = value; }
        }

        public string FuncType
        {
            get { return g_funcType; }
            set { g_funcType = value; }
        }

        public string FuncTypeName
        {
            get { return g_funcTypeName; }
            set { g_funcTypeName = value; }
        }

        public bool StatusFlag
        {
            get { return g_statusFlag; }
            set { g_statusFlag = value; }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_permissionUuid = new Guid();
            g_groupUuid = new Guid();
            g_funcUuid = new Guid();
            g_createdBy = new Guid();
            g_createdDate = new DateTime();
            g_updatedBy = new Guid();
            g_updatedDate = new DateTime();

            g_sn = 0;
            g_groupId = string.Empty;
            g_funcId = string.Empty;
            g_funcName = string.Empty;
            g_funcType = string.Empty;
            g_funcTypeName = string.Empty;
            g_statusFlag = false;
        }

        #endregion

        #region 建構函數/Dispose

        public SysGroupProgramBE()
        {
            resetVariables();
        }

        #endregion

    }
}
