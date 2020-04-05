using System;

namespace WebLibrary.ViewModel
{
    /// <summary>
    /// 基礎VM，(目前為)_lightLayout.cshtml與ContentLeftPartialView.cshtml的Model之參考型別。
    /// </summary>
    [Serializable]
    public abstract class BaseVM
    {
        /// <summary>
        /// WebLibrary.Controller.DDSCController.FormToSession 所需之屬性字串陣列。
        /// </summary>
        public static string[] StoreQueryName { get; set; }

        /// <summary>
        /// 是否修改。
        /// </summary>
        public bool IsModify { get; set; }

        /// <summary>
        /// 訊息。
        /// </summary>
        public string Message { get; set; } = "";
    }
}
