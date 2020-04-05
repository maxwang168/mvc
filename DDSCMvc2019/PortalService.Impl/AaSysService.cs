using CommonLibrary.DBA;
using Entity;
using Entity.SYS;
using PortalService.Contract;
using PortalService.Contract.ViewModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PortalService.Impl.BL;

namespace PortalService.Impl
{
    public class AaSysService : IAaSysService
    {
        private static ILog logger = LogManager.GetLogger(typeof(AaSysService));
        private DBASqlLog g_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);
        private AaUserBL g_AaUserBL = new AaUserBL();
        private AaOrgBL g_AaOrgBL = new AaOrgBL();

        #region ZT_AaUser
        public IEnumerable<AaUser> AaUserQuery(AaUserModel p_ViewModel, string p_PwdStatus, string p_StatusFlag)
        {
            return g_AaUserBL.AaUserQuery(p_ViewModel, p_PwdStatus, p_StatusFlag);
        }

        public AaUserModel AaUserQryBE(string p_user_id, string p_user_pwd)
        {
            return g_AaUserBL.AaUserQryBE(p_user_id, p_user_pwd);
        }

        public bool insertAaUserBE(AaUser p_BE)
        {
            return g_AaUserBL.insertAaUserBE(p_BE);
        }

        public bool updateAaUserBE(AaUser p_BE)
        {
            return g_AaUserBL.updateAaUserBE(p_BE);
        }

        public bool updateStatus(AaUser p_BE)
        {
            return g_AaUserBL.updateStatus(p_BE);
        }

        public bool deleteAaUserBE(Guid p_uuid)
        {
            return g_AaUserBL.deleteAaUserBE(p_uuid);
        }

        public bool resetAaUserBE(AaUser p_be)
        {
            return g_AaUserBL.resetAaUserBE(p_be);
        }

        public int AaUserQryCnt(string p_user_id, string p_org_id)
        {
            return g_AaUserBL.AaUserQryCnt(p_user_id, p_org_id);
        }

        public bool changePwdChangePwdBE(ChangePwdBE p_be)
        {
            return g_AaUserBL.changePwdChangePwdBE(p_be);
        }

        public bool CheckPwd(Guid p_userUuid, string p_pwd)
        {
            return g_AaUserBL.CheckPwd(p_userUuid, p_pwd);
        }

        public AaUser QueryAaUserByUid(Guid p_uuid)
        {
            return g_AaUserBL.QueryAaUserByUid(p_uuid);
        }

        public AaUser QueryAaUserById(string p_id)
        {
            return g_AaUserBL.QueryAaUserById(p_id);
        }

        public IEnumerable<AaUser> QueryAaUserByRoleUid(Guid p_uuid)
        {
            return g_AaUserBL.QueryAaUserByRoleUid(p_uuid);
        }

        public bool UnlockUserData(AaUser p_BE)
        {
            return g_AaUserBL.UnlockUserData(p_BE);
        }

        public AaUser QueryAaUserByUidForResetPw(Guid p_UserUuid)
        {
            return g_AaUserBL.QueryAaUserByUidForResetPw(p_UserUuid);
        }

        #endregion

        #region ZT_AaOrg

        public IEnumerable<string[]> QueryOrgList(bool p_isInternal)
        {
            return g_AaOrgBL.QueryOrgList(p_isInternal);
        }

        public IEnumerable<string[]> QueryAaOrgRole(string p_uuid)
        {
            return g_AaOrgBL.QueryAaOrgRole(p_uuid);
        }

        public IEnumerable<AaOrgModel> AaOrgQuery(AaOrgModel p_ViewModel)
        {
            return g_AaOrgBL.AaOrgQuery(p_ViewModel);
        }

        public IEnumerable<string[]> QueryAllRole(string p_varChar01, bool p_isAdminGroup, bool p_isAdmin)
        {
            AaOrgBL m_bl = new AaOrgBL();
            return m_bl.QueryAllRole(p_varChar01, p_isAdminGroup, p_isAdmin);
        }

        public string GetOrgId(string p_groupId)
        {
            AaOrgBL m_bl = new AaOrgBL();
            return m_bl.GetOrgId(p_groupId);
        }

        #endregion

    }
}
