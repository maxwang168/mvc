using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Entity.FileImport
{
    [Serializable]
    public partial class DataLogInfoBE : IFlowHandler
    {
        #region 成員變數
        private Guid g_logUuid;
        private string g_fileRule;
        private string g_workDate;
        private string g_batchId;
        private int g_dataCnt;
        private int g_successCnt;
        private int g_fileCnt;
        private string g_statusStep;
        private string g_statusStepName;
        private string g_statusFlag;
        private Guid g_createdBy;
        private string g_createdByName;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private string g_updatedByName;
        private DateTime g_updatedDate;

        private decimal g_amountTotal;
        private decimal g_amountSuccess;
        //幣別
        private string g_currencyType;
        /// <summary>
        /// 2017/11/14 Add By Richer
        /// 存放執行交易放行申請之多幣別
        /// </summary>
        private List<string> g_currencyTypeList;
        /// <summary>
        /// 2017/11/14 Add By Richer
        /// 存放執行交易放行申請之多幣別各別總金額
        /// </summary>
        private List<string> g_amountTotalToDisplayList;
        //首筆的作業時間
        private string g_workTime;
        //開始上傳主機的時間
        private string g_uloadTime;
        //開始執行的時間
        private string g_executeTime;
        //開始試算結果的時間
        private string g_queryTime;
        #endregion

        #region Property
        [Display(Name = "LogUuid")]
        public Guid LogUuid
        {
            get { return g_logUuid; }
            set { g_logUuid = value; }
        }

        [Display(Name = "FileRule")]
        public string FileRule
        {
            get { return g_fileRule; }
            set { g_fileRule = value; }
        }

        [Display(Name = "WorkDate")]
        public string WorkDate
        {
            get { return g_workDate; }
            set { g_workDate = value; }
        }

        [Display(Name = "BatchId")]
        public string BatchId
        {
            get { return g_batchId; }
            set { g_batchId = value; }
        }

        [Display(Name = "DataCnt")]
        public int DataCnt
        {
            get { return g_dataCnt; }
            set { g_dataCnt = value; }
        }

        public string DataCntToDisplay
        {
            get
            {
                return string.Format("{0:N0}", DataCnt);
            }
        }

        [Display(Name = "SuccessCnt")]
        public int SuccessCnt
        {
            get { return g_successCnt; }
            set { g_successCnt = value; }
        }

        [Display(Name = "FileCnt")]
        public int FileCnt
        {
            get { return g_fileCnt; }
            set { g_fileCnt = value; }
        }

        [Display(Name = "StatusStep")]
        public string StatusStep
        {
            get { return g_statusStep; }
            set { g_statusStep = value; }
        }

        [Display(Name = "StatusStepName")]
        public string StatusStepName
        {
            get { return g_statusStepName; }
            set { g_statusStepName = value; }
        }

        [Display(Name = "StatusFlag")]
        public string StatusFlag
        {
            get { return g_statusFlag; }
            set { g_statusFlag = value; }
        }

        [Display(Name = "建立者代號")]
        public Guid CreatedBy
        {
            get { return g_createdBy; }
            set { g_createdBy = value; }
        }

        [Display(Name = "建立者姓名")]
        public string CreatedByName
        {
            get { return g_createdByName; }
            set { g_createdByName = value; }
        }

        [Display(Name = "建立日期")]
        public DateTime CreatedDate
        {
            get { return g_createdDate; }
            set { g_createdDate = value; }
        }

        [Display(Name = "異動者代號")]
        public Guid UpdatedBy
        {
            get { return g_updatedBy; }
            set { g_updatedBy = value; }
        }

        [Display(Name = "異動者姓名")]
        public string UpdatedByName
        {
            get { return g_updatedByName; }
            set { g_updatedByName = value; }
        }

        [Display(Name = "異動日期")]
        public DateTime UpdatedDate
        {
            get { return g_updatedDate; }
            set { g_updatedDate = value; }
        }

        [Display(Name = "AmountTotal")]
        public decimal AmountTotal
        {
            get { return g_amountTotal; }
            set { g_amountTotal = value; }
        }

        /// <summary>
        /// 2017/11/14 Add By Richer
        /// 存放執行交易放行申請之多幣別
        /// </summary>
        [Display(Name = "CurrencyTypeList")]
        public List<string> CurrencyTypeList
        {
            get { return g_currencyTypeList; }
            set { g_currencyTypeList = value; }
        }

        /// <summary>
        /// 2017/11/14 Add By Richer
        /// 存放執行交易放行申請之多幣別各別總金額
        /// </summary>
        [Display(Name = "AmountTotalToDisplayList")]
        public List<string> AmountTotalToDisplayList
        {
            get { return g_amountTotalToDisplayList; }
            set { g_amountTotalToDisplayList = value; }
        }

        public string AmountTotalToDisplay
        {
            get
            {
                if (CurrencyType == "TWD")
                {
                    return string.Format("{0:N0}", AmountTotal);
                }
                else
                {
                    return string.Format("{0:N}", AmountTotal);
                }
            }
        }

        [Display(Name = "AmountSuccess")]
        public decimal AmountSuccess
        {
            get { return g_amountSuccess; }
            set { g_amountSuccess = value; }
        }

        public string AmountSuccessToDisplay
        {
            get
            {
                if (CurrencyType == "TWD")
                {
                    return string.Format("{0:N0}", AmountSuccess);
                }
                else
                {
                    return string.Format("{0:N}", AmountSuccess);
                }
            }
        }

        [Display(Name = "CurrencyType")]
        public string CurrencyType
        {
            get { return g_currencyType; }
            set { g_currencyType = value; }
        }

        [Display(Name = "WorkTime")]
        public string WorkTime
        {
            get { return g_workTime; }
            set { g_workTime = value; }
        }

        [Display(Name = "UloadTime")]
        public string UloadTime
        {
            get { return g_uloadTime; }
            set { g_uloadTime = value; }
        }

        [Display(Name = "ExecuteTime")]
        public string ExecuteTime
        {
            get { return g_executeTime; }
            set { g_executeTime = value; }
        }

        [Display(Name = "QueryTime")]
        public string QueryTime
        {
            get { return g_queryTime; }
            set { g_queryTime = value; }
        }

        /// <summary>
        /// 處理類型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 處理類型@本日帳/次日帳
        /// </summary>
        public string DataTypeDesc { get; set; }

        public ImportStatus ImportStatusStep { get; set; } = ImportStatus.None;

        /// <summary>
        /// 執行交易-逐筆處理用，紀錄勾選資料的DataUuid
        /// </summary>
        public string DataUuidList { get; set; }

        /// <summary>
        /// 與當前DataLogIngo物件相關聯的DataX
        /// </summary>
        public List<DataXBE> DataXList { get; set; }

        /// <summary>
        /// 檔案維護放行顯示專用
        /// </summary>
        public List<DataXMaintainBE> DataXMaintainList { get; set; }

        public List<DataLogApproveBE> ApproveData { get; set; }

        /// <summary>
        /// 檔案處理狀態
        /// </summary>
        public enum ImportStatus
        {
            /// <summary>
            /// 未指定狀態
            /// </summary>
            [Display(Name = "未指定狀態")]
            None = 0,
            /// <summary>
            /// 檔案匯入中
            /// </summary>
            [Display(Name = "檔案匯入中")]
            Importing = 10,
            /// <summary>
            /// 檔案匯入成功
            /// </summary>
            [Display(Name = "檔案匯入成功")]
            ImportSuccess = 11,
            /// <summary>
            /// 檔案匯入失敗
            /// </summary>
            [Display(Name = "檔案匯入失敗")]
            ImportFailed = 12,
            /// <summary>
            /// 上傳主機中
            /// </summary>
            [Display(Name = "上傳主機中")]
            Uploading = 20,
            /// <summary>
            /// 上傳主機成功
            /// </summary>
            [Display(Name = "上傳主機成功")]
            UpdloadSuccess = 21,
            /// <summary>
            /// 上傳主機失敗
            /// </summary>
            [Display(Name = "上傳主機失敗")]
            UploadFailed = 22,
            /// <summary>
            /// 檔案維護中
            /// </summary>
            [Display(Name = "檔案維護中")]
            Maintaining = 30,
            /// <summary>
            /// 執行交易中
            /// </summary>
            [Display(Name = "執行交易中")]
            TransactionExecuting = 40,
            /// <summary>
            /// 已執行交易
            /// </summary>
            [Display(Name = "已執行交易")]
            TransactionExecuted = 41,
            /// <summary>
            /// 查無資料(檔案類型+作業日期+批次)
            /// </summary>
            [Display(Name = "查無資料(檔案類型+作業日期+批次)")]
            DataNotFound = 100
        }
        #endregion

        #region Public Method
        public static string GetEnumDisplayName<T>(T p_Enum)
        {
            var type = p_Enum.GetType();
            if (!type.IsEnum) return p_Enum.ToString();

            var members = type.GetMember(p_Enum.ToString());
            if (members.Length == 0) return p_Enum.ToString();

            var attributes = members[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length == 0) return p_Enum.ToString();

            var attribute = (DisplayAttribute)attributes[0];
            return attribute.GetName();
        }

        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_logUuid = new Guid();
            g_fileRule = string.Empty;
            g_workDate = string.Empty;
            g_batchId = string.Empty;
            g_dataCnt = 0;
            g_successCnt = 0;
            g_fileCnt = 0;
            g_statusStep = string.Empty;
            g_statusStepName = string.Empty;
            g_statusFlag = string.Empty;
            g_createdBy = new Guid();
            g_createdByName = string.Empty;
            g_createdDate = new DateTime();
            g_updatedBy = new Guid();
            g_updatedByName = string.Empty;
            g_updatedDate = new DateTime();

            g_amountTotal = 0;
            g_amountSuccess = 0;
            g_currencyType = string.Empty;
            g_workTime = string.Empty;
            g_uloadTime = string.Empty;
            g_executeTime = string.Empty;
            g_queryTime = string.Empty;

            ImportStatusStep = ImportStatus.None;
            DataXList = null;
        }

        public string htmlStringHandler()
        {
            StringBuilder m_strHtml = new StringBuilder();

            if (DataXMaintainList != null && DataXMaintainList.Count > 0)
            {
                return htmlStringHandlerForMaintaining();
            }

            m_strHtml.Append("<table class='main_table' border='1' cellpadding='1' cellspacing='1' style='width:100%;'>");
            m_strHtml.Append("<thead><tr><td colspan='2'>資料內容</td></tr></thead>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel' width='15%'>檔案類型</th>");
            m_strHtml.Append(string.Format("<td class='editTable' width='85%'>{0}</td>", FileDesc));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>作業日期</th>");
            m_strHtml.Append("<td class='editTable'>");
            DateTime m_date = DateTime.Now;
            if (DateTime.TryParseExact(WorkDate, "yyyyMMdd",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out m_date))
            {
                m_strHtml.Append(m_date.ToString("yyyy/MM/dd"));
            }
            else
            {
                m_strHtml.Append(WorkDate);
            }
            m_strHtml.Append("</td>");
            m_strHtml.Append("</tr>");
            if (false == string.IsNullOrWhiteSpace(BatchId))
            {
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>處理批次</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", BatchId));
                m_strHtml.Append("</tr>");
            }
            if (false == string.IsNullOrWhiteSpace(DataType))
            {
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>處理類型</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", DataTypeDesc));
                m_strHtml.Append("</tr>");
            }
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>總筆數</th>");
            m_strHtml.Append("<td class='editTable'>");
            if (ApproveData != null && ApproveData.Count > 0)//將台幣與外幣的覆核資料放在一起
            {
                int m_totalCount = 0;
                for (int i = 0; i < ApproveData.Count; i++)
                {
                    m_totalCount += ApproveData[i].DataXList.Count;
                }
                m_strHtml.Append(string.Format("{0:N0}", m_totalCount));
            }
            else
            {
                m_strHtml.Append(DataCntToDisplay);
            }
            m_strHtml.Append("</td>");
            m_strHtml.Append("</tr>");
            // 2017/11/14 Add By Richer 顯示執行交易放行申請之多幣別各別總金額
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>總金額</th>");
            m_strHtml.Append("<td class='editTable'>");
            if (CurrencyTypeList != null && CurrencyTypeList.Count > 0)
            {
                for (int i = 0; i < CurrencyTypeList.Count; i++)
                {
                    if (AmountTotalToDisplayList[i] != null)
                    {
                        m_strHtml.Append(CurrencyTypeList[i]).Append("：").Append(AmountTotalToDisplayList[i]).Append("<br />");
                    }
                }
            }
            m_strHtml.Append("</td>");
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th>");
            m_strHtml.Append("</th>");
            m_strHtml.Append("<td>");
            m_strHtml.Append("<table>");

            if (ApproveData != null && ApproveData.Count > 0)//將台幣與外幣的覆核資料放在一起
            {
                m_strHtml.Append("<thead>");
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("序號");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("期交所帳號");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("會員代號");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("種類");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("會員帳號");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("金額");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("幣別");
                m_strHtml.Append("</td>");
                m_strHtml.Append("</tr>");
                m_strHtml.Append("</thead>");

                for (int i = 0; i < ApproveData.Count; i++)
                {
                    for (int j = 0; j < ApproveData[i].DataXList.Count; j++)
                    {
                        m_strHtml.Append("<tr>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(ApproveData[i].DataXList[j].Seq);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(ApproveData[i].DataXList[j].StkSettleAc);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(ApproveData[i].DataXList[j].MemNo);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(ApproveData[i].DataXList[j].AcTypeName);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(ApproveData[i].DataXList[j].MemSettleAc);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(ApproveData[i].DataXList[j].AllocatedAmountToDisplay);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(ApproveData[i].DataXList[j].CurrencyTypeDesc);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("</tr>");
                    }
                }
            }
            else
            {
                if (DataXList != null && DataXList.Count > 0)
                {

                    m_strHtml.Append("<thead>");
                    m_strHtml.Append("<tr>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append("序號");
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append("期交所帳號");
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append("會員代號");
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append("種類");
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append("會員帳號");
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append("金額");
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append("幣別");
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("</tr>");
                    m_strHtml.Append("</thead>");
                    foreach (var m_DataX in DataXList)
                    {
                        m_strHtml.Append("<tr>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(m_DataX.Seq);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(m_DataX.StkSettleAc);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(m_DataX.MemNo);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(m_DataX.AcTypeName);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(m_DataX.MemSettleAc);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(m_DataX.AllocatedAmountToDisplay);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("<td>");
                        m_strHtml.Append(m_DataX.CurrencyTypeDesc);
                        m_strHtml.Append("</td>");
                        m_strHtml.Append("</tr>");
                    }
                }
            }
            m_strHtml.Append("</table>");
            m_strHtml.Append("</td>");
            m_strHtml.Append("</tr>");
            m_strHtml.Append("</table>");

            return m_strHtml.ToString();
        }

        private string htmlStringHandlerForMaintaining()
        {
            StringBuilder m_strHtml = new StringBuilder();

            m_strHtml.Append("<table class='main_table' border='1' cellpadding='1' cellspacing='1' style='width:100%;'>");
            m_strHtml.Append("<thead><tr><td colspan='2'>資料內容</td></tr></thead>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel' width='15%'>檔案類型</th>");
            m_strHtml.Append(string.Format("<td class='editTable' width='85%'>{0}</td>", FileDesc));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>作業日期</th>");
            m_strHtml.Append("<td class='editTable'>");
            DateTime m_date = DateTime.Now;
            if (DateTime.TryParseExact(WorkDate, "yyyyMMdd",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out m_date))
            {
                m_strHtml.Append(m_date.ToString("yyyy/MM/dd"));
            }
            else
            {
                m_strHtml.Append(WorkDate);
            }
            m_strHtml.Append("</td>");
            m_strHtml.Append("</tr>");
            if (false == string.IsNullOrWhiteSpace(BatchId))
            {
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>處理批次</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", BatchId));
                m_strHtml.Append("</tr>");
            }
            if (false == string.IsNullOrWhiteSpace(DataType))
            {
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<th class='mainLabel'>處理類型</th>");
                m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", DataTypeDesc));
                m_strHtml.Append("</tr>");
            }
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th class='mainLabel'>幣別</th>");
            m_strHtml.Append(string.Format("<td class='editTable'>{0}</td>", CurrencyType));
            m_strHtml.Append("</tr>");
            m_strHtml.Append("<tr>");
            m_strHtml.Append("<th>");
            m_strHtml.Append("</th>");
            m_strHtml.Append("<td>");
            m_strHtml.Append("<table>");
            if (DataXMaintainList != null && DataXMaintainList.Count > 0)
            {

                m_strHtml.Append("<thead>");
                m_strHtml.Append("<tr>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("狀態");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("序號");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("期交所帳號");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("結算會員");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("結算會員/期交所(他行)帳號");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("類別");
                m_strHtml.Append("</td>");
                m_strHtml.Append("<td>");
                m_strHtml.Append("交易金額");
                m_strHtml.Append("</td>");
                m_strHtml.Append("</tr>");
                m_strHtml.Append("</thead>");
                foreach (var m_DataXMaintain in DataXMaintainList)
                {
                    m_strHtml.Append("<tr>");
                    m_strHtml.Append("<td>");
                    switch (m_DataXMaintain.StatusFlag)
                    {
                        case "I":
                            m_strHtml.Append("新增");
                            break;
                        case "D":
                            m_strHtml.Append("刪除");
                            break;
                        case "M":
                            if (m_DataXMaintain.OriginalFlag)
                            {
                                m_strHtml.Append("異動前");
                            }
                            else
                            {
                                m_strHtml.Append("異動後");
                            }
                            break;
                        default:
                            break;
                    }
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append(m_DataXMaintain.Seq);
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append(m_DataXMaintain.StkSettleAc);
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append(m_DataXMaintain.MemNo);
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append(m_DataXMaintain.MemSettleAc);
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append(m_DataXMaintain.AcTypeName);
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("<td>");
                    m_strHtml.Append(m_DataXMaintain.AllocatedAmountToDisplay);
                    m_strHtml.Append("</td>");
                    m_strHtml.Append("</tr>");
                }
            }
            m_strHtml.Append("</table>");
            m_strHtml.Append("</td>");
            m_strHtml.Append("</tr>");
            m_strHtml.Append("</table>");

            return m_strHtml.ToString();
        }
        #endregion

        #region 建構函數/Dispose
        public DataLogInfoBE()
        {
            resetVariables();
        }
        #endregion

    }

    public partial class DataLogInfoBE
    {
        /// <summary>
        /// 檔案類型名稱
        /// </summary>
        public string FileDesc { get; set; }
        /// <summary>
        /// 幣別名稱
        /// </summary>
        public string CurrencyTypeDesc { get; set; }
        /// <summary>
        /// 明細資料結束檢核/開始入檔時間
        /// </summary>
        public string DataWorkTime { get; set; }
        /// <summary>
        /// 明細資料結束入檔時間
        /// </summary>
        public string DataUloadTime { get; set; }
        /// <summary>
        /// 最後修改時間（主檔與維護暫存檔相比）
        /// </summary>
        public DateTime LastEditDate { get; set; }
        /// <summary>
        /// 最後修改時間-顯示用
        /// </summary>
        public string LastEditDateToDisplay => LastEditDate.ToString("yyyy/MM/dd HH:mm:ss");
        /// <summary>
        /// 幣別在系統參數檔中的seq，排序用
        /// </summary>
        public int CurrencySeq { get; set; }
    }
}
