using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.BAS
{
    [Serializable]
    public class ForeignExchangeBE
    {
        #region 成員變數
        private string g_currType;
        private string g_cashInd;
        private string g_buyingRate;
        private string g_sellingRate;
        private Guid g_createdBy;
        private string g_createdByName;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private string g_updatedByName;
        private DateTime g_updatedDate;

        #region for GridView
        private string g_currName;
        private string g_cashBuyingRate;
        private string g_cashSellingRate;
        private string g_indBuyingRate;
        private string g_indSellingRate;
        #endregion
        #endregion

        #region Property
        [Display(Name = "幣別")]
        [StringLength(3)]
        public string CurrType
        {
            get { return g_currType; }
            set { g_currType = value; }
        }

        [Display(Name = "外匯類型")]
        [StringLength(1)]
        public string CashInd
        {
            get { return g_cashInd; }
            set { g_cashInd = value; }
        }

        [Display(Name = "買匯")]
        public string BuyingRate
        {
            get { return g_buyingRate; }
            set { g_buyingRate = value; }
        }

        [Display(Name = "賣匯")]
        public string SellingRate
        {
            get { return g_sellingRate; }
            set { g_sellingRate = value; }
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

        #region for GridView
        [Display(Name = "外幣幣別")]
        public string CurrName
        {
            get { return g_currName; }
            set { g_currName = value; }
        }
        [Display(Name = "現金買匯")]
        public string CashBuyingRate
        {
            get { return g_cashBuyingRate; }
            set { g_cashBuyingRate = value; }
        }
        [Display(Name = "現金賣匯")]
        public string CashSellingRate
        {
            get { return g_cashSellingRate; }
            set { g_cashSellingRate = value; }
        }
        [Display(Name = "買匯匯率")]
        public string IndBuyingRate
        {
            get { return g_indBuyingRate; }
            set { g_indBuyingRate = value; }
        }
        [Display(Name = "賣匯匯率")]
        public string IndSellingRate
        {
            get { return g_indSellingRate; }
            set { g_indSellingRate = value; }
        }
        #endregion
        #endregion

        #region Public Method
        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_currType = string.Empty;
            g_cashInd = string.Empty;
            g_buyingRate = string.Empty;
            g_sellingRate = string.Empty;
            g_createdBy = new Guid();
            g_createdByName = string.Empty;
            g_createdDate = new DateTime();
            g_updatedBy = new Guid();
            g_updatedByName = string.Empty;
            g_updatedDate = new DateTime();
        }
        #endregion

        #region 建構函數/Dispose
        public ForeignExchangeBE()
        {
            resetVariables();
        }
        #endregion
    }
}
