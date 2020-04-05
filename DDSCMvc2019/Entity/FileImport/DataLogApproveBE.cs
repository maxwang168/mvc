using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.FileImport
{
    [Serializable]
    public class DataLogApproveBE : DataLogInfoBE
    {
        public void cloneDataLogInfo(DataLogInfoBE p_dataBE)
        {
            FileRule = p_dataBE.FileRule;
            WorkDate = p_dataBE.WorkDate;
            BatchId = p_dataBE.BatchId;
            DataType = p_dataBE.DataType;
            CurrencyType = p_dataBE.CurrencyType;
            UpdatedBy = p_dataBE.UpdatedBy;
            UpdatedDate = p_dataBE.UpdatedDate;
            DataXList = p_dataBE.DataXList;
            ImportStatusStep = p_dataBE.ImportStatusStep;

            DataUuidList = p_dataBE.DataUuidList;
        }
    }
}
