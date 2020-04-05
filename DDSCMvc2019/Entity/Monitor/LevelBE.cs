using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Monitor
{
    [Serializable]
    public class LevelBE
    {
        #region 成員變數
        private string g_level_code;

        private string g_level_name;

        private string g_created_by;

        private string g_created_date;

        private string g_updated_by;

        private string g_updated_date;
        #endregion

        #region Property

        [Display(Name = "等級代號")]
        public string level_code
        {
            get { return g_level_code; }
            set { g_level_code = value; }
        }
        [Display(Name = "等級名稱")]
        public string level_name
        {
            get { return g_level_name; }
            set { g_level_name = value; }
        }
        [Display(Name = "建立者")]
        public string created_by
        {
            get { return g_created_by; }
            set { g_created_by = value; }
        }
        [Display(Name = "建立時間")]
        public string created_date
        {
            get { return g_created_date; }
            set { g_created_date = value; }
        }
        [Display(Name = "更新者")]
        public string updated_by
        {
            get { return g_updated_by; }
            set { g_updated_by = value; }
        }
        [Display(Name = "更新時間")]
        public string updated_date
        {
            get { return g_updated_date; }
            set { g_updated_date = value; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            level_code = string.Empty;
            level_name = string.Empty;
            created_by = string.Empty;
            created_date = string.Empty;
            updated_by = string.Empty;
            updated_date = string.Empty;
        }

        #endregion

        #region 建構函數/Dispose

        public LevelBE()
        {
            resetVariables();
        }

        #endregion      
    }
}
