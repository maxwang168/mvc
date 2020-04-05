using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS {
    public class EtlColumnBE {
        /// <summary>
        /// Column UUID
        /// </summary>
        public Guid COLUMN_UUID { get; set; }

        /// <summary>
        /// Table UUID
        /// </summary>
        public Guid TABLE_UUID { get; set; }

        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string COLUMN_NAME { get; set; }

        /// <summary>
        /// 欄位描述
        /// </summary>
        public string COLUMN_DESC { get; set; }

        /// <summary>
        /// 欄位屬性
        /// </summary>
        public string COLUMN_TYPE { get; set; }

        /// <summary>
        /// 欄位長度
        /// </summary>
        public int COLUMN_LENGTH { get; set; }

        /// <summary>
        /// 小數點長度
        /// </summary>
        public int DIGITAL_LENGTH { get; set; }

        /// <summary>
        /// 資料狀態
        /// </summary>
        public char STATUS_FLAG { get; set; }

        /// <summary>
        /// 建立使用者 UUID
        /// </summary>
        public string CREATE_USER_UUID { get; set; }

        /// <summary>
        /// 建立機構代碼
        /// </summary>
        public string CREATE_ORG_ID { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CREATE_DATE { get; set; }

        /// <summary>
        /// 修改使用者 UUID
        /// </summary>
        public string MODIFY_USER_UUID { get; set; }

        /// <summary>
        /// 修改機構代碼
        /// </summary>
        public string MODIFY_ORG_ID { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime MODIFY_DATE { get; set; }

        public string SEQNO { get; set; }
    }
}
