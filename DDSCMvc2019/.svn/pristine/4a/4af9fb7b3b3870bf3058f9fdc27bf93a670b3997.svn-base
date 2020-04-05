using System;

namespace Entity.FileImport
{
    public class DataXMaintainBE
    {
        public bool OriginalFlag { get; set; } = false;

        public string StatusFlag { get; set; } = string.Empty;

        public Guid LogUuid { get; set; } = Guid.Empty;

        public Guid DataUuid { get; set; } = Guid.Empty;

        public string Seq { get; set; } = string.Empty;

        public string StkSettleAc { get; set; } = string.Empty;

        public string MemNo { get; set; } = string.Empty;

        public string MemSettleAc { get; set; } = string.Empty;

        public string AcType { get; set; } = string.Empty;

        public string AcTypeName { get; set; } = string.Empty;

        public decimal AllocatedAmount { get; set; } = 0;

        public string AllocatedAmountToDisplay
        {
            get
            {
                if (CurrencyType == "TWD")
                {
                    return AllocatedAmount.ToString("N0");
                }
                else
                {
                    return AllocatedAmount.ToString("N2");
                }
            }
        }

        public string CurrencyType { get; set; } = string.Empty;
    }
}
