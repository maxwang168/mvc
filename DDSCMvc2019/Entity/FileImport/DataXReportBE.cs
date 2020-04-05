namespace Entity.FileImport
{
    public class DataXReportBE
    {
        /// <summary>
        /// 銀行代號
        /// </summary>
        public string bank_no { get; set; }
        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string bank_name { get; set; }
        /// <summary>
        /// 檔案類型名稱
        /// </summary>
        public string file_desc { get; set; }
        /// <summary>
        /// 幣別
        /// </summary>
        public string currency_type { get; set; }
        /// <summary>
        /// 幣別排序
        /// </summary>
        public int currency_seq { get; set; }
        /// <summary>
        /// 報表代號
        /// </summary>
        public string report_id { get; set; }
        /// <summary>
        /// 作業日期
        /// </summary>
        public string work_date { get; set; }
        /// <summary>
        /// 處理批次
        /// </summary>
        public string batch_id { get; set; }
        /// <summary>
        /// 期交所結算保證金專戶
        /// </summary>
        public string stk_settle_ac { get; set; }
        /// <summary>
        /// 金額單位
        /// </summary>
        public string currency_type_desc { get; set; }
        /// <summary>
        /// 劃撥轉帳結算會員代號
        /// </summary>
        public string mem_no { get; set; }
        /// <summary>
        /// 類別
        /// </summary>
        public string ac_type_desc { get; set; }
        /// <summary>
        /// 作業時間
        /// </summary>
        public string work_time { get; set; }
        /// <summary>
        /// 結算會員結算保證金專戶帳號
        /// </summary>
        public string mem_settle_ac { get; set; }
        /// <summary>
        /// 應追繳款項金額(元)
        /// </summary>
        public decimal allocated_amount { get; set; }
        /// <summary>
        /// 實際撥入款項金額(元)
        /// </summary>
        public decimal execute_amount { get; set; }
        /// <summary>
        /// 逐筆處理撥入款項金額（元）
        /// </summary>
        public decimal single_execute_amount { get; set; }
        /// <summary>
        /// 尚不足款項金額(元)
        /// </summary>
        public decimal insuff_amount { get; set; }
        /// <summary>
        /// 檔案類型
        /// </summary>
        public string file_rule { get; set; }
        /// <summary>
        /// 處理格式
        /// </summary>
        public string data_type { get; set; }
    }
}
