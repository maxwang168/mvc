using Entity.BAS;
using PortalService.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace PortalService.Contract
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IBasService"。
    [ServiceContract]
    public interface IBasService
    {
        #region NotifyRec

        [OperationContract]
        IEnumerable<NotifyRecBE> NotifyRecQuery(NotifyRecModel p_ViewModel);
        /*
        [OperationContract]
        bool insertBasWorkingBE(BasWorkingBE p_BE);

        [OperationContract]
        bool updateBasWorkingBE(BasWorkingBE p_BE);

        [OperationContract]
        bool deleteBasWorkingBE(Guid p_uuid);

        [OperationContract]
        int BasWorkingQryCnt(string p_work_date, Guid p_uuid);
        */
        [OperationContract]
        IEnumerable<string[]> QueryForChannel();

        [OperationContract]
        IEnumerable<string[]> QueryForStatus();

        [OperationContract]
        IEnumerable<NotifyRecBE> QueryNotifyRec(NotifyRecModel p_model);

        #endregion

        #region NotifyTemplate

        //[OperationContract]
        //IEnumerable<NotifyTemplateBE> NotifyTemplateQuery(NotifyTemplateModel p_ViewModel);

        //[OperationContract]
        //bool insertNotifyTemplateBE(NotifyTemplateBE p_BE);

        //[OperationContract]
        //bool updateNotifyTemplateBE(NotifyTemplateBE p_BE);

        //[OperationContract]
        //bool deleteNotifyTemplateBE(Guid p_uuid);

        //[OperationContract]
        //int NotifyTemplateQryCnt(string p_template_id, Guid p_uuid);

        //[OperationContract]
        //IEnumerable<string[]> QueryForTemplateChannel();

        //[OperationContract]
        //IEnumerable<NotifyTemplateBE> QueryNotifyTemplate(NotifyTemplateModel p_model)

        #endregion

        #region AnnounceInfo

        [OperationContract]
        IEnumerable<AnnounceInfoBE> AnnounceInfoQuery();

        [OperationContract]
        IEnumerable<AnnounceInfoBE> queryAnnounceInfo(AnnounceInfoModel p_model);

        [OperationContract]
        bool insertAnnounceInfoBE(AnnounceInfoBE p_be);

        [OperationContract]
        bool updateAnnounceInfoBE(AnnounceInfoBE p_be);

        [OperationContract]
        bool updateAnnounceInfoStatus(AnnounceInfoBE p_BE);

        [OperationContract]
        bool deleteAnnounceInfoBE(AnnounceInfoBE p_be);

        [OperationContract]
        AnnounceInfoModel GetAnnonuText(string uuid);

        /// <summary>
        /// 依照服務端點依序儲存檔案
        /// </summary>
        /// <returns>Item1: 是否執行成功@true, false, Item2: 錯誤訊息</returns>
        [OperationContract]
        Tuple<bool, string> UploadFileToAP(AnnounceInfoFileUpload p_UploadFile, bool p_IsFromService = false);
        #endregion

    }
}