using System;

namespace Entity.BAS
{
    [Serializable]
    public partial class QueryTransferBE
    {
        public Guid txn_uuid { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public string txn_date { get; set; }
        /// <summary>
        /// 交易時間
        /// </summary>
        public string txn_time { get; set; }

        public string txn_serno { get; set; }
        /// <summary>
        /// 結算會員代號
        /// </summary>
        public string txn_mem_code { get; set; }
        /// <summary>
        /// 轉帳行代號
        /// </summary>
        public string txn_out_no { get; set; }
        /// <summary>
        /// 對方帳號
        /// </summary>
        public string txn_out_ac { get; set; }
        /// <summary>
        /// 交易帳戶名稱
        /// </summary>
        public string txn_out_name { get; set; }
        /// <summary>
        /// 交易種類
        /// </summary>
        public string txn_kind { get; set; }
        /// <summary>
        /// 交易種類名稱
        /// </summary>
        public string txn_kind_name { get; set; }

        public decimal txn_amount { get; set; }
        /// <summary>
        /// 對方統一編號或身分證字號
        /// </summary>
        public string txn_ac_id_no { get; set; }

        public string txn_actype { get; set; }
        /// <summary>
        /// 撥入金額
        /// </summary>
        public decimal txn_amount_in { get; set; }
        /// <summary>
        /// 撥出金額
        /// </summary>
        public decimal txn_amount_out { get; set; }

        public string txn_ac_datatype { get; set; }

        public string txn_out_actype { get; set; }

        public string txn_applicant { get; set; }

        public string txn_reference { get; set; }

        public string txn_tran_type { get; set; }

        public string txn_query_account { get; set; }

        public DateTime? txn_query_date { get; set; }

        public string txn_query_curr { get; set; }

        public string txn_query_curr_desc { get; set; }

        public DateTime? created_date { get; set; }

        public Guid? created_by { get; set; }
    }

    public partial class QueryTransferBE
    {
        public string txn_balance { get; set; }
        /// <summary>
        /// 撥入金額（顯示用）
        /// </summary>
        public string txn_amount_in2display
        {
            get
            {
                return txn_amount_in.ToString((txn_query_curr ?? "") == "TWD" ? "N0" : "N2");
            }
        }
        /// <summary>
        /// 撥出金額（顯示用）
        /// </summary>
        public string txn_amount_out2display
        {
            get
            {
                return txn_amount_out.ToString((txn_query_curr ?? "") == "TWD" ? "N0" : "N2");
            }
        }
    }
}
