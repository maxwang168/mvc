using Entity;
using Entity.SYS;
using PortalService.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Entity.FileImport;
using PortalService.Contract.ViewModel.System;
using System.Data;

namespace PortalService.Contract
{
    [ServiceContract]
    public interface ISysService
    {
        #region SysCodeInfo
        [OperationContract]
        IEnumerable<SysCodeInfoBE> SysCodeInfoQuery(SysCodeInfoModel p_sysCodeVM);
        [OperationContract]
        IEnumerable<string[]> QueryByCate(string p_cate);
        [OperationContract]
        bool insertSysCodeInfoBE(SysCodeInfoBE p_code);
        [OperationContract]
        bool updateSysCodeInfoBE(SysCodeInfoBE p_code);
        [OperationContract]
        bool deleteSysCodeInfoBE(string p_codeType, string p_codeUuid);
        [OperationContract]
        IEnumerable<string[]> QueryFirstLevel();
        [OperationContract]
        IEnumerable<string[]> QuerySecondLevel(string p_cate);
        [OperationContract]
        SysCodeInfoBE QuerySysCodeInfo(string uuid);
        [OperationContract]
        SysCodeInfoBE QueryByCateCodeId(string p_codeId, string p_cate);
        
        [OperationContract]
        IEnumerable<string[]> QueryFirstLevelForUser();
        [OperationContract]
        SysCodeInfoBE QuerySysCodeInfoForFile(string cate, string code_id);

        [OperationContract]
        IEnumerable<string[]> QueryUuidByCate(string p_cate);

        [OperationContract]
        int SysCodeInfoQryCnt(string p_code_id, string p_cate);

        [OperationContract]
        bool IsDataModified(Guid p_CodeUuid, Guid p_UpdatedBy, DateTime p_UpdatedDate);
        #endregion

        #region SysProgram
        [OperationContract]
        IEnumerable<SysProgramBE> SysProgramQuery(SysProgramModel p_sysProgramVM);
        [OperationContract]
        bool insertSysProgramBE(SysProgramBE p_code);
        [OperationContract]
        bool updateSysProgramBE(SysProgramBE p_code);
        [OperationContract]
        bool deleteSysProgramBE(string p_programId);
        [OperationContract]
        IEnumerable<string[]> QueryMenu(string p_type);
        [OperationContract]
        IEnumerable<string[]> QuerySubMenu(string p_parent);
        #endregion

        #region SysGroupProgram
        [OperationContract]
        IEnumerable<SysGroupProgramBE> SysGroupProgramQuery(SysGroupProgramModel p_viewModel);
        [OperationContract]
        bool modifySysGroupProgramBE(List<SysGroupProgramBE> p_listVM);
        #endregion

        #region SysGroup

        [OperationContract]
        IEnumerable<SysGroupBE> SysGroupQuery(SysGroupModel p_viewModel);

        [OperationContract]
        bool insertSysGroupBE(SysGroupBE p_be);

        [OperationContract]
        bool updateSysGroupBE(SysGroupBE p_be);

        [OperationContract]
        bool deleteSysGroupBE(Guid p_groupUuid);

        [OperationContract]
        int SysGroupProgramQryCnt(string p_groupId, Guid p_groupUuid); //, string p_orgId, Guid p_groupUuid);

        [OperationContract]
        IEnumerable<string[]> QueryGroupName(bool p_isAdminGroup, bool p_isAdmin);

        #endregion

        #region SysUserLog

        [OperationContract]
        IEnumerable<SysUserLogBE> SysUserLogQuery(SysUserLogModel p_sysUserLogVM, Guid p_UserUuid);

        [OperationContract]
        IEnumerable<UserLogTaiFexBE> UserLogTaifexQuery(UserLogTaiFexModel p_vm);

        #endregion

        #region FlowJob

        [OperationContract]
        IEnumerable<FlowJobBE> FlowJobQuery(FlowJobModel p_sysUserLogVM);

        [OperationContract]
        IEnumerable<string[]> QueryRecProgram(Guid p_UserUuid);

        [OperationContract]
        IEnumerable<string[]> QueryRecStatus();


        #endregion

        #region Login

        [OperationContract]
        UserData Login(LoginViewModel loginVM);

        [OperationContract]
        bool CheckLoginStatus(Guid p_loginToken);

        [OperationContract]
        bool UpdatePwdStatus(Guid p_userUuid, int p_retry, int p_retryMax);

        [OperationContract]
        bool ModifyUserInfo(string p_userId, string p_userName, string p_roleId, string p_mail);

        #endregion


        #region ZTClrList

        [OperationContract]
        IEnumerable<ZtClrlistBE> ZTClrListQuery(ZTClrListViewModel p_viewModel, bool p_IsSearch);

        [OperationContract]
        bool insertZTClrListBE(ZtClrlistBE p_clear);

        [OperationContract]
        bool insertZTClrListBatch(string p_clearUuid, string p_tableListUuid, string p_createdBy);

        [OperationContract]
        bool updateZTClrListBE(ZtClrlistBE p_clear);

        [OperationContract]
        bool deleteZTClrListBE(string p_clearUuid);

        #endregion ZTClrList

        #region ZT_EtlTable
        /// <summary>
        /// 取得ZT_EtlTable資料
        ///     2016-12-23 主要針對 is_houskeeping='1'
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<EtlTableBE> QueryHouskeepingTableList(EtlTableViewModel etlTableViewModel);

        [OperationContract]
        EtlTableBE ZTEtlTableQuery(ZTClrTableListViewModel p_viewModel);

        ///// <summary>
        ///// 取得ZT_EtlTable資料(只取得第一筆)並將IS_HOUSKEEPING改成'' 及 IS_ETL改成'1'
        /////     2016-12-23 主要是針對 IS_ETL<>'1' AND IS_HOUSKEEPING='1'
        ///// </summary>
        ///// <param name="table_name"></param>
        ///// <returns></returns>
        //[OperationContract]
        //IEnumerable<EtlTableBE> QueryHouskeepingTableListForHouseKeeping(string table_name);

        /// <summary>
        /// 新增一筆資料 ZT_EtlTable
        /// </summary>
        /// <param name="etlTableBE"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertHouskeepingTableBE(EtlTableBE p_table);

        /// <summary>
        /// 修改資料 ZT_EtlTable
        /// </summary>
        /// <param name="etlTableBE"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateHouskeepingTableBE(EtlTableBE p_table);

        /// <summary>
        /// 刪除資料 ZT_EtlTable
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteHouskeepingTableBE(string p_tableUuid);
        #endregion

        #region ZtClrtablelist


        [OperationContract]
        string[] SelectedClrtablelistQuery(string p_clearUuid);

        #endregion ZtClrtablelist

        #region ZT_EtlColumn
        /// <summary>
        /// 取得ZT_EtlColumn資料
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<EtlColumnBE> QueryEtlColumnList(EtlColumnViewModel etlColumnViewModel);

        /// <summary>
        /// 新增一筆資料 ZT_EtlColumn
        /// </summary>
        /// <param name="etlColumnViewModel"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertEtlColumnBE(EtlColumnViewModel etlColumnViewModel);

        /// <summary>
        /// 修改資料 ZT_EtlColumn
        /// </summary>
        /// <param name="etlColumnViewModel"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateEtlColumnBE(EtlColumnViewModel etlColumnViewModel);

        /// <summary>
        /// 刪除一筆資料 ZT_EtlColumn
        /// </summary>
        /// <param name="column_name"></param>
        /// <param name="table_uuid"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteEtlColumnBE(string table_uuid, string column_name);

        /// <summary>
        /// 刪除整筆資料 ZT_EtlColumn
        /// </summary>
        /// <param name="table_uuid"></param>
        /// <returns></returns>
        //[OperationContract]
        //bool DeleteEtlColumnBE(Guid table_uuid);
        #endregion


        [OperationContract]
        IEnumerable<SysRoleProgramsBE> SysRoleProgramsQuery(SysRoleProgramsViewModel p_viewModel);
        [OperationContract]
        bool modifySysRoleProgramsBE(List<SysRoleProgramsBE> p_listVM);
        [OperationContract]
        IEnumerable<SysRoleInfoBE> QueryRoleCodeName();
        //[OperationContract]
        //IEnumerable<SysRoleInfoBE> SysRoleInfoQuery(SysRoleInfoViewModel p_viewModel);

        [OperationContract]
        IEnumerable<string[]> QueryProgram(Guid p_UserUuid, bool p_IsAllQueriable = false);

        [OperationContract]
        IEnumerable<SysRoleInfoBE> SysRoleInfoQuery(SysRoleInfoViewModel p_sysRoleVM);

        [OperationContract]
        bool insertSysRoleInfo(SysRoleInfoBE p_role);

        [OperationContract]
        bool updateSysRoleInfo(SysRoleInfoBE p_role);

        [OperationContract]
        bool deleteSysRoleInfo(string p_roleCode);

        [OperationContract]
        IEnumerable<string> GetProgramInfo(string p_funcID);

        [OperationContract]
        IEnumerable<WorkFlowBE> WorkFlowQuery(WorkFlowViewModel p_viewModel);
        [OperationContract]
        bool updateWorkFlowBE(WorkFlowBE p_wf);

        
        [OperationContract]
        Tuple<string, DataSet> SysQueryData(SysQueryModel p_queryData);
        [OperationContract]
        string SysQueryUpdate(SysQueryModel p_queryData);
    }
}
