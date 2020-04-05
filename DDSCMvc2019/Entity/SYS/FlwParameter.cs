using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    [Serializable]
    public static class FlwParameter
    {
        public static readonly string FLW_STATUS_GO = "G";
        public static readonly string FLW_STATUS_REJECT = "R";
        public static readonly string FLW_STATUS_FINISH = "F";

        public static readonly string FLW_APPROVE_APPROVE = "Y";
        public static readonly string FLW_APPROVE_RJECT = "R";
    }
}
