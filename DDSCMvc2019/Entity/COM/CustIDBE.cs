using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.COM
{
    [Serializable]
    public class CustIDBE
    {
        #region 成員變數
        private string g_custID;
        private string g_custIDName;
        private string g_description;
        private bool g_isExist;
        #endregion

        #region Property

        [Display(Name = "CustID")]
        [Required]
        public string CustID
        {
            get { return g_custID; }
            set { g_custID = value; }
        }

        [Display(Name = "CustIDName")]
        public string CustIDName
        {
            get { return g_custIDName; }
            set { g_custIDName = value; }
        }

        [Display(Name = "Description")]
        public string Description
        {
            get { return g_description; }
            set { g_description = value; }
        }

        [Display(Name = "IsExist")]
        public bool IsExist
        {
            get { return g_isExist; }
            set { g_isExist = value; }
        }

        #endregion

        #region Public Method
        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_custID = string.Empty;
            g_custIDName = string.Empty;
            g_description = string.Empty;
            g_isExist = false;
        }
        #endregion

        #region 建構函數/Dispose
        public CustIDBE()
        {
            resetVariables();
        }
        #endregion

    }
}
