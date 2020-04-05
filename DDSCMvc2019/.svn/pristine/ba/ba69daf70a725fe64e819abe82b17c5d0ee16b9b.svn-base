using CommonLibrary.DBA;
using Entity.BAS;
using log4net;
using PortalService.Contract;
using PortalService.Contract.ViewModel;
using PortalService.Impl.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace PortalService.Impl
{
    public class BasService : IBasService
    {
        private static ILog logger = LogManager.GetLogger(typeof(BasService));
        private DBASqlLog g_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);
        
        private AnnounceInfoBL g_announceInfoBL = new AnnounceInfoBL();
        private CommonBL g_commonBL = new CommonBL();

      

       

   

        #region NotifyRec

        private NotifyRecBL g_NotifyRecBL = new NotifyRecBL();
        public IEnumerable<NotifyRecBE> NotifyRecQuery(NotifyRecModel p_ViewModel)
        {
            return g_NotifyRecBL.NotifyRecQuery(p_ViewModel);
        }
        /*
        public bool insertBasWorkingBE(BasWorkingBE p_BE)
        {
            return g_BasWorkingBL.insertBasWorkingBE(p_BE);
        }

        public bool updateBasWorkingBE(BasWorkingBE p_BE)
        {
            return g_BasWorkingBL.updateBasWorkingBE(p_BE);
        }

        public bool deleteBasWorkingBE(Guid p_uuid)
        {
            return g_BasWorkingBL.deleteBasWorkingBE(p_uuid);
        }

        public int BasWorkingQryCnt(string p_work_date, Guid p_uuid)
        {
            return g_BasWorkingBL.BasWorkingQryCnt(p_work_date, p_uuid);
        }
        */
        public IEnumerable<string[]> QueryForChannel()
        {
            return g_NotifyRecBL.QueryForChannel();
        }
        public IEnumerable<string[]> QueryForStatus()
        {
            return g_NotifyRecBL.QueryForStatus();
        }

        public IEnumerable<NotifyRecBE> QueryNotifyRec(NotifyRecModel p_model)
        {
            return g_NotifyRecBL.QueryNotifyRec(p_model);
        }
        #endregion

        #region NotifyTemplate

        //private NotifyTemplateBL g_NotifyTemplateBL = new NotifyTemplateBL();
        //public IEnumerable<NotifyTemplateBE> NotifyTemplateQuery(NotifyTemplateModel p_ViewModel)
        //{
            //return g_NotifyTemplateBL.NotifyTemplateQuery(p_ViewModel);
        //}

        //public bool insertNotifyTemplateBE(NotifyTemplateBE p_BE)
        //{
            //return g_NotifyTemplateBL.insertNotifyTemplateBE(p_BE);
        //}

        //public bool updateNotifyTemplateBE(NotifyTemplateBE p_BE)
        //{
            //return g_NotifyTemplateBL.updateNotifyTemplateBE(p_BE);
        //}

        //public bool deleteNotifyTemplateBE(Guid p_uuid)
        //{
            //return g_NotifyTemplateBL.deleteNotifyTemplateBE(p_uuid);
        //}

        //public int NotifyTemplateQryCnt(string p_template_id, Guid p_uuid)
        //{
            //return g_NotifyTemplateBL.NotifyTemplateQryCnt(p_template_id, p_uuid);
        //}

        //public IEnumerable<string[]> QueryForTemplateChannel()
        //{
            //return g_NotifyTemplateBL.QueryForTemplateChannel();
        //}
        /*
        public IEnumerable<NotifyTemplateBE> QueryNotifyTemplate(NotifyTemplateModel p_model)
        {
            return g_NotifyTemplateBL.QueryNotifyTemplate(p_model);
        }*/
        #endregion

        #region AnnounceInfo

        public IEnumerable<AnnounceInfoBE> AnnounceInfoQuery()
        {
            return g_announceInfoBL.AnnounceInfoQuery();
        }

        public IEnumerable<AnnounceInfoBE> queryAnnounceInfo(AnnounceInfoModel p_model)
        {
            return g_announceInfoBL.queryAnnounceInfo(p_model);
        }

        public bool insertAnnounceInfoBE(AnnounceInfoBE p_be)
        {
            return g_announceInfoBL.insertAnnounceInfoBE(p_be);
        }

        public bool updateAnnounceInfoBE(AnnounceInfoBE p_be)
        {
            return g_announceInfoBL.updateAnnounceInfoBE(p_be);
        }

        public bool updateAnnounceInfoStatus(AnnounceInfoBE p_BE)
        {
            return g_announceInfoBL.updateStatus(p_BE);
        }

        public bool deleteAnnounceInfoBE(AnnounceInfoBE p_be)
        {
            return g_announceInfoBL.deleteAnnounceInfoBE(p_be);
        }

        public AnnounceInfoModel GetAnnonuText(string uuid)
        {
            return g_announceInfoBL.GetAnnonuText(uuid);
        }

        public Tuple<bool, string> UploadFileToAP(AnnounceInfoFileUpload p_UploadFile, bool p_IsFromService)
        {
            return g_announceInfoBL.UploadFileToAP(p_UploadFile, p_IsFromService);
        }

        #endregion

  

      

     

  

 




    }
}
