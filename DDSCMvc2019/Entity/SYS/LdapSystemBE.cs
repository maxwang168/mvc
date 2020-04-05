using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    [Serializable]
    public class LdapSystemBE
    {
        #region 建構函數
        public LdapSystemBE()
        {
            initProperty();
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void initProperty()
        {
            query_result = false;
            response_message = string.Empty;
            ldap_system = string.Empty;
            ldap_server = string.Empty;
            ldap_server_port = 0;
            system_user = string.Empty;
            system_user_dn = string.Empty;
            system_pwd = string.Empty;
            role = string.Empty;
            pwd_expired_days = 0;
        }
        #endregion

        #region Property
        /// <summary>
        /// 查詢結果
        /// </summary>
        public bool query_result { get; set; }

        /// <summary>
        /// 回應訊息
        /// </summary>
        public string response_message { get; set; }

        /// <summary>
        /// Ldap連線模式：CTBC=中信Ldap連線模式、DDSC=中菲Ldap連線模式
        /// </summary>
        public string ldap_system { get; set; }

        /// <summary>
        /// Ldap Server
        /// </summary>
        public string ldap_server { get; set; }

        /// <summary>
        /// Ldap Server Port
        /// </summary>
        public short ldap_server_port { get; set; }

        /// <summary>
        /// 連線系統帳號
        /// </summary>
        public string system_user { get; set; }

        /// <summary>
        /// 連線系統帳號DN
        /// </summary>
        public string system_user_dn { get; set; }

        /// <summary>
        /// 連線系統密碼
        /// </summary>
        public string system_pwd { get; set; }

        /// <summary>
        /// 角色代碼
        /// </summary>
        public string role { get; set; }

        /// <summary>
        /// 密碼逾期天數
        /// </summary>
        public short pwd_expired_days { get; set; }
        #endregion
    }
}
