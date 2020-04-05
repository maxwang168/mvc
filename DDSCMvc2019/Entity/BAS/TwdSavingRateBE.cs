using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.BAS
{
    [Serializable]
    public class TwdSavingRateBE
    {
        #region 成員變數
        private string g_dpTerm;
        private string g_rateType;
        private string g_dpRate;
        private Guid g_createdBy;
        private string g_createdByName;
        private DateTime g_createdDate;
        private Guid g_updatedBy;
        private string g_updatedByName;
        private DateTime g_updatedDate;

        #region for GridView
        private string g_term;
        private string g_largeRate;
        private string g_fixedRate;
        private string g_floatingRate;
        #endregion
        #endregion

        #region Property
        [Display(Name = "期別")]
        [StringLength(3)]
        public string DpTerm
        {
            get { return g_dpTerm; }
            set { g_dpTerm = value; }
        }

        [Display(Name = "利率類型")]
        [StringLength(1)]
        public string RateType
        {
            get { return g_rateType; }
            set { g_rateType = value; }
        }

        [Display(Name = "台幣存款利率")]
        public string DpRate
        {
            get { return g_dpRate; }
            set { g_dpRate = value; }
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
        [Display(Name = "期別")]
        public string Term
        {
            get { return g_term; }
            set { g_term = value; }
        }
        [Display(Name = "大額定期利率")]
        public string LargeRate
        {
            get { return g_largeRate; }
            set { g_largeRate = value; }
        }
        [Display(Name = "固定利率")]
        public string FixedRate
        {
            get { return g_fixedRate; }
            set { g_fixedRate = value; }
        }
        [Display(Name = "機動利率")]
        public string FloatingRate
        {
            get { return g_floatingRate; }
            set { g_floatingRate = value; }
        }
        #endregion
        #endregion

        #region Public Method
        /// <summary>
        /// 重新設定成員變數的預設值
        /// </summary>
        public void resetVariables()
        {
            g_dpTerm = string.Empty;
            g_rateType = string.Empty;
            g_dpRate = string.Empty;
            g_createdBy = new Guid();
            g_createdByName = string.Empty;
            g_createdDate = new DateTime();
            g_updatedBy = new Guid();
            g_updatedByName = string.Empty;
            g_updatedDate = new DateTime();

            g_term = string.Empty;
            g_largeRate = string.Empty;
            g_fixedRate = string.Empty;
            g_floatingRate = string.Empty;
        }
        #endregion

        #region 建構函數/Dispose
        public TwdSavingRateBE()
        {
            resetVariables();
        }
        #endregion
    }
}
