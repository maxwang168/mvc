using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    public class SysCodeInfoBE
    {
        #region 成員變數

        private Guid g_codeUuid;
        private Guid g_superUuid;
        private string g_codeType;
        private string g_codeId;
        private string g_codeName;
        private string g_cate;
        private int? g_seq;
        private string g_codeNameP;
        private string g_codeSNameP;
        private string g_codeNameS;
        private string g_codeSNameS;
        private string g_modifyStatus;
        private string g_statusFlag;
        private string g_description;
        private string g_orgVarChar01;
        private string g_varChar01;
        private bool g_encrypt01;
        private string g_orgVarChar02;
        private string g_varChar02;
        private bool g_encrypt02;
        private string g_orgVarChar03;
        private string g_varChar03;
        private bool g_encrypt03;
        private string g_orgVarChar04;
        private string g_varChar04;
        private bool g_encrypt04;
        private string g_orgVarChar05;
        private string g_varChar05;
        private bool g_encrypt05;
        private string g_orgVarChar06;
        private string g_varChar06;
        private bool g_encrypt06;
        private string g_orgVarChar07;
        private string g_varChar07;
        private bool g_encrypt07;
        private string g_orgVarChar08;
        private string g_varChar08;
        private bool g_encrypt08;
        private string g_orgVarChar09;
        private string g_varChar09;
        private bool g_encrypt09;
        private string g_orgVarChar10;
        private string g_varChar10;
        private bool g_encrypt10;
        private Guid g_createdBy;
        private string g_createdByName;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private string g_updatedByName;
        private DateTime g_updatedDate;

        private string g_groupId;
        private string g_groupName;
        private string g_subGroupId;
        private string g_subGroupName;

        #endregion

        #region Property

        [Display(Name = "uuid")]
        public Guid CodeUuid
        {
            get { return g_codeUuid; }
            set { g_codeUuid = value; }
        }
        [Display(Name = "superUuid")]
        public Guid SuperUuid
        {
            get { return g_superUuid; }
            set { g_superUuid = value; }
        }

        [Display(Name = "代碼類別")]
        [StringLength(1)]
        public string CodeType
        {
            get { return g_codeType; }
            set { g_codeType = value; }
        }

        [Display(Name = "代碼")]
        [StringLength(30)]
        public string CodeId
        {
            get { return g_codeId; }
            set { g_codeId = value; }
        }

        [Display(Name = "代碼名稱")]
        [Required]
        [StringLength(100)]
        public string CodeName
        {
            get { return g_codeName; }
            set { g_codeName = value; }
        }

        [Display(Name = "類別代碼")]
        [StringLength(50)]
        public string Cate
        {
            get { return g_cate; }
            set { g_cate = value; }
        }

        [Display(Name = "顯示順序")]
        [Required]
        public int? Seq
        {
            get { return g_seq; }
            set { g_seq = value; }
        }

        [Display(Name = "主要名稱")]
        [StringLength(500)]
        public string CodeNameP
        {
            get { return g_codeNameP; }
            set { g_codeNameP = value; }
        }

        [Display(Name = "主要名稱簡稱")]
        [StringLength(50)]
        public string CodeSNameP
        {
            get { return g_codeSNameP; }
            set { g_codeSNameP = value; }
        }

        [Display(Name = "次要名稱")]
        [StringLength(100)]
        public string CodeNameS
        {
            get { return g_codeNameS; }
            set { g_codeNameS = value; }
        }

        [Display(Name = "次要名稱簡稱")]
        [StringLength(50)]
        public string CodeSNameS
        {
            get { return g_codeSNameS; }
            set { g_codeSNameS = value; }
        }

        [Display(Name = "可修改(Y/N)")]
        [StringLength(1)]
        public string ModifyStatus
        {
            get { return g_modifyStatus; }
            set { g_modifyStatus = value; }
        }

        [Display(Name = "有效(Y/N)")]
        [StringLength(1)]
        public string StatusFlag
        {
            get { return g_statusFlag; }
            set { g_statusFlag = value; }
        }

        [Display(Name = "說明")]
        [StringLength(128)]
        public string Description
        {
            get { return g_description; }
            set { g_description = value; }
        }

        [Display(Name = "原參數1")]
        [StringLength(100)]
        public string OrgVarChar01
        {
            get { return g_orgVarChar01; }
            set { g_orgVarChar01 = value; }
        }

        [Display(Name = "參數1")]
        [StringLength(100)]
        public string VarChar01
        {
            get { return g_varChar01; }
            set { g_varChar01 = value; }
        }
        
        [Display(Name = "是否加密參數1")]
        public bool Encrypt01
        {
            get { return g_encrypt01; }
            set { g_encrypt01 = value; }
        }

        [Display(Name = "原參數2")]
        [StringLength(100)]
        public string OrgVarChar02
        {
            get { return g_orgVarChar02; }
            set { g_orgVarChar02 = value; }
        }

        [Display(Name = "參數2")]
        [StringLength(100)]
        public string VarChar02
        {
            get { return g_varChar02; }
            set { g_varChar02 = value; }
        }

        [Display(Name = "是否加密參數2")]
        public bool Encrypt02
        {
            get { return g_encrypt02; }
            set { g_encrypt02 = value; }
        }

        [Display(Name = "原參數3")]
        [StringLength(100)]
        public string OrgVarChar03
        {
            get { return g_orgVarChar03; }
            set { g_orgVarChar03 = value; }
        }

        [Display(Name = "參數3")]
        [StringLength(100)]
        public string VarChar03
        {
            get { return g_varChar03; }
            set { g_varChar03 = value; }
        }

        [Display(Name = "是否加密參數3")]
        public bool Encrypt03
        {
            get { return g_encrypt03; }
            set { g_encrypt03 = value; }
        }

        [Display(Name = "原參數4")]
        [StringLength(100)]
        public string OrgVarChar04
        {
            get { return g_orgVarChar04; }
            set { g_orgVarChar04 = value; }
        }

        [Display(Name = "參數4")]
        [StringLength(100)]
        public string VarChar04
        {
            get { return g_varChar04; }
            set { g_varChar04 = value; }
        }

        [Display(Name = "是否加密參數4")]
        public bool Encrypt04
        {
            get { return g_encrypt04; }
            set { g_encrypt04 = value; }
        }

        [Display(Name = "原參數5")]
        [StringLength(100)]
        public string OrgVarChar05
        {
            get { return g_orgVarChar05; }
            set { g_orgVarChar05 = value; }
        }

        [Display(Name = "參數5")]
        [StringLength(100)]
        public string VarChar05
        {
            get { return g_varChar05; }
            set { g_varChar05 = value; }
        }

        [Display(Name = "是否加密參數5")]
        public bool Encrypt05
        {
            get { return g_encrypt05; }
            set { g_encrypt05 = value; }
        }

        [Display(Name = "原參數6")]
        [StringLength(100)]
        public string OrgVarChar06
        {
            get { return g_orgVarChar06; }
            set { g_orgVarChar06 = value; }
        }

        [Display(Name = "參數6")]
        [StringLength(100)]
        public string VarChar06
        {
            get { return g_varChar06; }
            set { g_varChar06 = value; }
        }

        [Display(Name = "是否加密參數6")]
        public bool Encrypt06
        {
            get { return g_encrypt06; }
            set { g_encrypt06 = value; }
        }

        [Display(Name = "原參數7")]
        [StringLength(100)]
        public string OrgVarChar07
        {
            get { return g_orgVarChar07; }
            set { g_orgVarChar07 = value; }
        }

        [Display(Name = "參數7")]
        [StringLength(100)]
        public string VarChar07
        {
            get { return g_varChar07; }
            set { g_varChar07 = value; }
        }

        [Display(Name = "是否加密參數1")]
        public bool Encrypt07
        {
            get { return g_encrypt07; }
            set { g_encrypt07 = value; }
        }

        [Display(Name = "原參數8")]
        [StringLength(100)]
        public string OrgVarChar08
        {
            get { return g_orgVarChar08; }
            set { g_orgVarChar08 = value; }
        }

        [Display(Name = "參數8")]
        [StringLength(100)]
        public string VarChar08
        {
            get { return g_varChar08; }
            set { g_varChar08 = value; }
        }

        [Display(Name = "是否加密參數8")]
        public bool Encrypt08
        {
            get { return g_encrypt08; }
            set { g_encrypt08 = value; }
        }

        [Display(Name = "原參數9")]
        [StringLength(100)]
        public string OrgVarChar09
        {
            get { return g_orgVarChar09; }
            set { g_orgVarChar09 = value; }
        }

        [Display(Name = "參數9")]
        [StringLength(100)]
        public string VarChar09
        {
            get { return g_varChar09; }
            set { g_varChar09 = value; }
        }

        [Display(Name = "是否加密參數9")]
        public bool Encrypt09
        {
            get { return g_encrypt09; }
            set { g_encrypt09 = value; }
        }

        [Display(Name = "原參數10")]
        [StringLength(100)]
        public string OrgVarChar10
        {
            get { return g_orgVarChar10; }
            set { g_orgVarChar10 = value; }
        }

        [Display(Name = "參數10")]
        [StringLength(100)]
        public string VarChar10
        {
            get { return g_varChar10; }
            set { g_varChar10 = value; }
        }

        [Display(Name = "是否加密參數10")]
        public bool Encrypt10
        {
            get { return g_encrypt10; }
            set { g_encrypt10 = value; }
        }

        [Display(Name = "建立者代號")]
        public Guid CreatedBy
        {
            get { return g_createdBy; }
            set { g_createdBy = value; }
        }

        [Display(Name = "建立者姓名")]
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

        [Display(Name = "異動者姓名")]
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

        [Display(Name = "群組代碼")]
        public string GroupId
        {
            get { return g_groupId; }
            set { g_groupId = value; }
        }

        [Display(Name = "群組名稱")]
        public string GroupName
        {
            get { return g_groupName; }
            set { g_groupName = value; }
        }

        [Display(Name = "子群組代碼")]
        public string SubGroupId
        {
            get { return g_subGroupId; }
            set { g_subGroupId = value; }
        }

        [Display(Name = "子群組名稱")]
        public string SubGroupName
        {
            get { return g_subGroupName; }
            set { g_subGroupName = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_codeUuid = new Guid();
            g_superUuid = new Guid();
            g_codeType = string.Empty;
            g_codeId = string.Empty;
            g_codeName = string.Empty;
            g_cate = string.Empty;
            g_seq = 0;
            g_codeNameP = string.Empty;
            g_codeSNameP = string.Empty;
            g_codeNameS = string.Empty;
            g_codeSNameS = string.Empty;
            g_modifyStatus = string.Empty;
            g_statusFlag = string.Empty;
            g_description = string.Empty;
            g_orgVarChar01 = string.Empty;
            g_varChar01 = string.Empty;
            g_encrypt01 = false;
            g_orgVarChar02 = string.Empty;
            g_varChar02 = string.Empty;
            g_encrypt02 = false;
            g_orgVarChar03 = string.Empty;
            g_varChar03 = string.Empty;
            g_encrypt03 = false;
            g_orgVarChar04 = string.Empty;
            g_varChar04 = string.Empty;
            g_encrypt04 = false;
            g_orgVarChar05 = string.Empty;
            g_varChar05 = string.Empty;
            g_encrypt05 = false;
            g_orgVarChar06 = string.Empty;
            g_varChar06 = string.Empty;
            g_encrypt06 = false;
            g_orgVarChar07 = string.Empty;
            g_varChar07 = string.Empty;
            g_encrypt07 = false;
            g_orgVarChar08 = string.Empty;
            g_varChar08 = string.Empty;
            g_encrypt08 = false;
            g_orgVarChar09 = string.Empty;
            g_varChar09 = string.Empty;
            g_encrypt09 = false;
            g_orgVarChar10 = string.Empty;
            g_varChar10 = string.Empty;
            g_encrypt10 = false;
            g_createdBy = new Guid();
            g_createdByName = string.Empty;
            g_createdDate = new DateTime();
            g_updatedBy = new Guid();
            g_updatedByName = string.Empty;
            g_updatedDate = new DateTime();

            g_groupId = string.Empty;
            g_groupName = string.Empty;
            g_subGroupId = string.Empty;
            g_subGroupName = string.Empty;
        }

        #endregion

        #region 建構函數/Dispose

        public SysCodeInfoBE()
        {
            resetVariables();
        }

        #endregion
    }
}
