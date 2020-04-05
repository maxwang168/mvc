using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalService.Contract.ViewModel.Monitor
{
    public class ReturnMsg
    {
        /// <summary>
        /// 是否成功(true成功/false失敗)
        /// </summary>
        public bool isSuccess { get; set; }

        /// <summary>
        /// 失敗訊息
        /// </summary>
        public string errorMsg { get; set; }

        /// <summary>
        /// 成功訊息
        /// </summary>
        public string successMsg { get; set; }

    }
}
