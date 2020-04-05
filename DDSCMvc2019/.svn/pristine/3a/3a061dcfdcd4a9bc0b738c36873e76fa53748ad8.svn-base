using System;

namespace Entity.FileImport
{
    /// <summary>
    /// 資料庫schema
    /// </summary>
    [Serializable]
    public partial class DataXBE
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid DataUuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid LogUuid { get; set; }
        /// <summary>
        /// 序號
        /// </summary>
        public string Seq { get; set; }
        /// <summary>
        /// 期交所帳號
        /// </summary>
        public string StkSettleAc { get; set; }
        /// <summary>
        /// 結算會員/結算銀行代號
        /// </summary>
        public string MemNo { get; set; }
        /// <summary>
        /// 類別->0:客戶帳,9:自有帳
        /// </summary>
        public string AcType { get; set; }
        /// <summary>
        /// 結算會員結算保證金專戶帳號
        /// </summary>
        public string MemSettleAc { get; set; }
        /// <summary>
        /// 應追繳/撥出金額
        /// </summary>
        public decimal AllocatedAmount { get; set; }
        /// <summary>
        /// 試算金額
        /// </summary>
        public decimal QueryAmount { get; set; }
        /// <summary>
        /// 實際追繳/撥出金額
        /// </summary>
        public decimal ExecuteAmount { get; set; }
        /// <summary>
        /// 當次逐筆交易金額
        /// </summary>
        public decimal SingleExecuteAmount { get; set; }
        /// <summary>
        /// 交易後尚不足金額
        /// </summary>
        public decimal ExecuteInsuffAmount { get; set; }
        /// <summary>
        /// 送出交易電文時的金額，為了萬一ESC回傳錯誤但主機確實有交易成功的狀況。
        /// </summary>
        public decimal AttemptExecuteAmount { get; set; }
        /// 幣別
        /// </summary>
        public string CurrencyType { get; set; }
        /// <summary>
        /// a:本日帳, 1:次日帳
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public string StatusFlag { get; set; }
        /// <summary>
        /// 上傳時間
        /// </summary>
        public string WorkTime { get; set; }
        /// <summary>
        /// 上傳主機時間
        /// </summary>
        public string UloadTime { get; set; }
        /// <summary>
        /// 試算時間
        /// </summary>
        public string QueryTime { get; set; }
        /// <summary>
        /// 執行交易時間
        /// </summary>
        public string ExecuteTime { get; set; }
        /// <summary>
        /// 交易序號
        /// </summary>
        public string TrnCode { get; set; }
        /// <summary>
        /// 主機回傳碼
        /// </summary>
        public string EsbMsg { get; set; }
        /// <summary>
        /// 查詢餘額之交易序號
        /// </summary>
        public string QueryTrnCode { get; set; }
        /// <summary>
        /// 查詢餘額之主機回傳碼
        /// </summary>
        public string QueryEsbMsg { get; set; }
        /// <summary>
        /// 統一編號(11)
        /// </summary>
        public string BankIdNo { get; set; }
        /// <summary>
        /// 電話區域號碼(3)
        /// </summary>
        public string BankTelZone { get; set; }
        /// <summary>
        /// 電話號碼(7)
        /// </summary>
        public string BankTelNo { get; set; }
        /// <summary>
        /// 保留1
        /// </summary>
        public string Reserve1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid CreatedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; }
        ///
        public Guid UpdatedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdatedDate { get; set; }
    }

    /// <summary>
    /// 額外資訊
    /// </summary>
    public partial class DataXBE
    {
        /// <summary>
        /// 銀行代號
        /// </summary>
        public string BankNo { get; set; }
        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 報表代號
        /// </summary>
        public string ReportId { get; set; }
        /// <summary>
        /// 檔案類型
        /// </summary>
        public string FileRule { get; set; }
        /// <summary>
        /// 檔案類型名稱
        /// </summary>
        public string FileDesc { get; set; }
        /// <summary>
        /// 作業日期
        /// </summary>
        public string WorkDate { get; set; }
        /// <summary>
        /// 處理批次
        /// </summary>
        public string BatchId { get; set; }
        /// <summary>
        /// 目前處理狀態（數字）
        /// </summary>
        public DataLogInfoBE.ImportStatus? StatusStep { get; set; }
        /// <summary>
        /// 目前處理狀態（中文）
        /// </summary>
        public string StatusStepName { get; set; }
        /// <summary>
        /// 幣別名稱
        /// </summary>
        public string CurrencyTypeDesc { get; set; }
        /// <summary>
        /// 多幣別排序用
        /// </summary>
        public int CurrencySeq { get; set; }
        /// <summary>
        /// 帳戶類別名稱
        /// </summary>
        public string AcTypeName { get; set; }
        /// <summary>
        /// 試算尚不足金額
        /// </summary>
        public decimal QueryInsuffAmount { get; set; }
        /// <summary>
        /// 結算會員名稱(40)
        /// </summary>
        public string MemName { get; set; }
        /// <summary>
        /// 金額顯示用
        /// </summary>
        public string AllocatedAmountToDisplay
        {
            get
            {
                if (CurrencyType == "TWD")
                {
                    return string.Format("{0:N0}", AllocatedAmount);
                }
                else
                {
                    return string.Format("{0:N}", AllocatedAmount);
                }
            }
        }
        /// <summary>
        /// 處理格式名稱
        /// </summary>
        public string DataTypeDesc { get; set; }
    }
}