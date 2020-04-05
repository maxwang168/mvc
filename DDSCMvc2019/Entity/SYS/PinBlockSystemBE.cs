using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    [Serializable]
    public class PinBlockSystemBE
    {
        #region 建構函數
        public PinBlockSystemBE()
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
            pin_block_system = string.Empty;
            tx_code = string.Empty;
            biz_id = string.Empty;
            des_url = string.Empty;
            pin_block_url = string.Empty;
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
        /// PINBLOCK連線模式：CTBC=中信PINBLOCK連線模式、DDSC=中菲PINBLOCK連線模式
        /// </summary>
        public string pin_block_system { get; set; }

        /// <summary>
        /// 電文代碼
        /// </summary>
        public string tx_code { get; set; }

        /// <summary>
        /// 連線系統帳號
        /// </summary>
        public string biz_id { get; set; }

        /// <summary>
        /// DES_URL
        /// </summary>
        public string des_url { get; set; }

        /// <summary>
        /// PINBLOCK_URL
        /// </summary>
        public string pin_block_url { get; set; }
        #endregion
    }
}
