using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Monitor
{
    [Serializable]
    public class NotifytypeBE
    {
        #region 成員變數

        private Guid g_uuid;
        private string g_notify_code;
        private string g_notify_name;
        private string g_type;
        private string g_typeUI;
        private string g_created_by;
        private DateTime g_created_date;
        private string g_updated_by;
        private DateTime g_updated_date;
        private DateTime g_CreDate;
        private string g_CreUser;
        private string g_cate;
        private string g_code_id;
        private string g_code_name;
        private string g_address;
        private string g_id;

        #endregion

        #region Property

        [Display(Name = "id")]
        public string id
        {
            get { return g_id; }
            set { g_id = value; }
        }

        [Display(Name = "cate")]
        public string cate
        {
            get { return g_cate; }
            set { g_cate = value; }
        }
        [Display(Name = "uuid")]
        public Guid Uuid
        {
            get { return g_uuid; }
            set { g_uuid = value; }
        }

        [Display(Name = "g_notify_code")]
        public string notify_code
        {
            get { return g_notify_code; }
            set { g_notify_code = value; }
        }
        [Display(Name = "notify_name")]
        public string notify_name
        {
            get { return g_notify_name; }
            set { g_notify_name = value; }
        }
        [Display(Name = "type")]
        public string type
        {
            get { return g_type; }
            set { g_type = value; }
        }
        [Display(Name = "typeUI")]
        public string typeUI
        {
            get { return g_typeUI; }
            set { g_typeUI = value; }
        }
        [Display(Name = "created_by")]
        public string created_by
        {
            get { return g_created_by; }
            set { g_created_by = value; }
        }
        [Display(Name = "created_date")]
        public DateTime created_date
        {
            get { return g_created_date; }
            set { g_created_date = value; }
        }
        [Display(Name = "updated_by")]
        public string updated_by
        {
            get { return g_updated_by; }
            set { g_updated_by = value; }
        }
        [Display(Name = "updated_date")]
        public DateTime updated_date
        {
            get { return g_updated_date; }
            set { g_updated_date = value; }
        }
        [Display(Name = "新增時間")]
        public DateTime CreDate
        {
            get { return g_CreDate; }
            set { g_CreDate = value; }
        }
        [Display(Name = "新增人")]
        public string CreUser
        {
            get { return g_CreUser; }
            set { g_CreUser = value; }
        }
        [Display(Name = "code_id")]
        public string code_id
        {
            get { return g_code_id; }
            set { g_code_id = value; }
        }
        [Display(Name = "code_name")]
        public string code_name
        {
            get { return g_code_name; }
            set { g_code_name = value; }
        }
        [Display(Name = "address")]
        public string address
        {
            get { return g_address; }
            set { g_address = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_uuid = Guid.Empty;
            g_notify_code = string.Empty;
            g_notify_name = string.Empty;
            g_type = string.Empty;
            g_created_by = string.Empty;
            g_created_date = new DateTime();
            g_updated_by = string.Empty;
            g_updated_date = new DateTime();
            g_CreDate = new DateTime();
            g_CreUser = string.Empty;
            g_cate = string.Empty;
            g_code_id = string.Empty;
            g_code_name = string.Empty;
            g_address = string.Empty;
            g_id = string.Empty;
            g_typeUI = string.Empty;
        }

        #endregion

        #region 建構函數/Dispose

        public NotifytypeBE()
        {
            resetVariables();
        }

        #endregion      
    }
}
