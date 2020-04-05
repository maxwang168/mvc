using Entity;
using Entity.SYS;
using PortalService.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PortalService.Contract
{
    [ServiceContract]
    public interface IAaSysService
    {
        #region ZT_AaUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_ViewModel"></param>
        /// <param name="p_PwdStatus">欲查詢的密碼狀態，中間用逗號隔開，ex: L,V</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<AaUser> AaUserQuery(AaUserModel p_ViewModel, string p_PwdStatus = "", string p_StatusFlag = "");
        [OperationContract]
        AaUserModel AaUserQryBE(string p_user_id, string p_user_pwd);
        [OperationContract]
        bool insertAaUserBE(AaUser p_BE);
        [OperationContract]
        bool updateAaUserBE(AaUser p_BE);
        [OperationContract]
        bool updateStatus(AaUser p_BE);
        [OperationContract]
        bool deleteAaUserBE(Guid p_uuid);
        [OperationContract]
        bool resetAaUserBE(AaUser p_be);
        [OperationContract]
        int AaUserQryCnt(string p_user_id, string p_org_id);
        [OperationContract]
        bool changePwdChangePwdBE(ChangePwdBE p_be);
        [OperationContract]
        bool CheckPwd(Guid p_userUuid, string p_pwd);
        [OperationContract]
        AaUser QueryAaUserByUid(Guid p_uuid);
        [OperationContract]
        AaUser QueryAaUserById(string p_id);
        [OperationContract]
        IEnumerable<AaUser> QueryAaUserByRoleUid(Guid p_uuid);
        [OperationContract]
        bool UnlockUserData(AaUser p_BE);
        [OperationContract]
        AaUser QueryAaUserByUidForResetPw(Guid p_UserUuid);
        #endregion

        #region ZT_AaOrg
        [OperationContract]
        IEnumerable<string[]> QueryOrgList(bool p_isInternal);

        [OperationContract]
        IEnumerable<string[]> QueryAaOrgRole(string p_uuid);

        [OperationContract]
        IEnumerable<AaOrgModel> AaOrgQuery(AaOrgModel p_ViewModel);

        [OperationContract]
        IEnumerable<string[]> QueryAllRole(string p_varChar01, bool p_isAdminGroup, bool p_isAdmin);

        [OperationContract]
        string GetOrgId(string p_groupId);
        #endregion
    }
}
