using System;

namespace Entity.SYS
{
    public class EtlTableBE
    {
        /// <summary>
        /// Table UUID
        /// </summary>
        public Guid TABLE_UUID { get; set; }

        /// <summary>
        /// Table名稱
        /// </summary>
        public string TABLE_NAME { get; set; }

        /// <summary>
        /// Table說明
        /// </summary>
        public string TABLE_DESC { get; set; }

        /// <summary>
        /// 資料狀態
        /// </summary>
        public char STATUS_FLAG { get; set; }

        /// <summary>
        /// 建立使用者 UUID
        /// </summary>
        public string CREATE_USER_UUID { get; set; }

        public string CREATE_USER_NAME { get; set; }

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

        public string MODIFY_USER_NAME { get; set; }

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
