using CommonLibrary.DBA;
using Entity.Monitor;
using log4net;
using PortalService.Contract;
using PortalService.Contract.ViewModel.Monitor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace PortalService.Impl
{
    public class MonitorService : IMonitorService
    {
        private static ILog logger = LogManager.GetLogger(typeof(NotifyService));
        //private DBASqlLog g_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);
        private DBASqlLog g_nixmonitorConnection = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);

        #region NIXMONITOR

        #region function

        //#region create function
        //public ReturnMsg CreateFunction(string functionCode, string functionName, Guid createdBy)
        //{
        //    ReturnMsg msg = new ReturnMsg();
        //    msg.isSuccess = false;
        //    try
        //    {
        //        //檢查是否重複                
        //        string str_sql = @"select COUNT(function_code) as functionCount 
        //                           from ZT_MonitorFunction 
        //                           where function_code = @function_code 
        //                           group by function_code";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@function_code", functionCode.Trim()));
        //        DataTable dt_functionCount = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());
        //        if (dt_functionCount.Rows.Count > 0)
        //        {
        //            msg.isSuccess = false;
        //            msg.errorMsg = "資料重複!";
        //            return msg;
        //        }

        //        //新增
        //        str_sql = @"insert INTO  ZT_MonitorFunction(function_code,function_name,created_by,updated_by) values(@function_code,@function_name,@created_by,@updated_by)";
        //        m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@function_code", functionCode.Trim()));
        //        m_paraList.Add(new DBParameter("@function_name", functionName));
        //        m_paraList.Add(new DBParameter("@created_by", createdBy));
        //        m_paraList.Add(new DBParameter("@updated_by", createdBy));
        //        g_nixmonitorConnection.ExecNonQuery(str_sql, m_paraList.ToArray());
        //        msg.isSuccess = true;
        //        msg.successMsg = "新增成功";

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //        msg.isSuccess = false;
        //        msg.errorMsg = "新增失敗";
        //    }
        //    return msg;
        //}
        //#endregion

        //#region update function
        //public ReturnMsg UpdateFunction(string functionCode, string functionName, Guid updatedBy)
        //{
        //    ReturnMsg msg = new ReturnMsg();
        //    msg.isSuccess = false;
        //    try
        //    {
        //        string str_sql = @"UPDATE ZT_MonitorFunction SET function_name = @function_name ,updated_by = @updated_by,updated_date = @updated_date where function_code = @function_code ";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@function_code", functionCode.Trim()));
        //        m_paraList.Add(new DBParameter("@function_name", functionName));
        //        m_paraList.Add(new DBParameter("@updated_by", updatedBy));
        //        m_paraList.Add(new DBParameter("@updated_date", DateTime.Now));
        //        g_nixmonitorConnection.ExecNonQuery(str_sql, m_paraList.ToArray());
        //        msg.isSuccess = true;
        //        msg.successMsg = "更新成功";
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //        msg.isSuccess = false;
        //        msg.errorMsg = "更新失敗";
        //    }
        //    return msg;
        //}
        //#endregion

        //#region query function
        //public IEnumerable<FunctionBE> QueryFunction(string functionCode, string functionName)
        //{
        //    List<FunctionBE> functionList = new List<FunctionBE>();
        //    try
        //    {
        //        string str_sql = @"select function_code,function_name,created_by,created_date,updated_by,updated_date
        //                           from ZT_MonitorFunction
        //                           where 1=1 ";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        if (!string.IsNullOrEmpty(functionName))
        //        {
        //            str_sql += " and function_name like @function_name ";
        //            m_paraList.Add(new DBParameter("@function_name", functionName + "%"));
        //        }
        //        if (!string.IsNullOrEmpty(functionCode))
        //        {
        //            str_sql += " and function_code like @function_code ";
        //            m_paraList.Add(new DBParameter("@function_code", functionCode + "%"));
        //        }

        //        DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());

        //        foreach (DataRow dr in m_dataTable.Rows)
        //        {
        //            FunctionBE function = new FunctionBE();
        //            function.function_code = dr["function_code"] == null ? "" : dr["function_code"].ToString();
        //            function.function_name = dr["function_name"] == null ? "" : dr["function_name"].ToString();
        //            function.created_by = dr["created_by"] == null ? "" : dr["created_by"].ToString();
        //            function.created_date = dr["created_date"] == null ? "" : DateTime.Parse(dr["created_date"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
        //            function.updated_by = dr["updated_by"] == null ? "" : dr["updated_by"].ToString();
        //            function.updated_date = dr["updated_date"] == null ? "" : DateTime.Parse(dr["updated_date"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
        //            functionList.Add(function);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //    }
        //    return functionList;
        //}
        //#endregion

        //#region delete function
        //public ReturnMsg DeleteFunction(string functionCode)
        //{
        //    ReturnMsg msg = new ReturnMsg();
        //    msg.isSuccess = false;
        //    try
        //    {
        //        string str_sql = @"Delete from ZT_MonitorFunction where function_code = @function_code";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@function_code", functionCode.Trim()));
        //        g_nixmonitorConnection.ExecNonQuery(str_sql, m_paraList.ToArray());
        //        msg.isSuccess = true;
        //        msg.successMsg = "刪除成功";
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //        msg.isSuccess = false;
        //        msg.errorMsg = "刪除失敗";
        //    }
        //    return msg;
        //}
        //#endregion

        #region functionSelectItem
        public IEnumerable<string[]> functionSelectItem()
        {
            List<string[]> functionList = new List<string[]>();
            try
            {
                string str_sql = @"select function_code,function_name,created_by,created_date,updated_by,updated_date
                                   from ZT_MonitorFunction
                                   where 1=1 ";

                List<DBParameter> m_paraList = new List<DBParameter>();
                DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());
                string[] first = new string[2];
                first[0] = "";
                first[1] = "請選擇";
                functionList.Add(first);
                foreach (DataRow dr in m_dataTable.Rows)
                {
                    string[] function = new string[2];
                    function[0] = dr["function_code"] == null ? "" : dr["function_code"].ToString();
                    function[1] = dr["function_name"] == null ? "" : dr["function_name"].ToString();
                    functionList.Add(function);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return functionList;
        }
        #endregion

        #endregion

        #region level

        //#region create level
        //public ReturnMsg CreateLevel(string levelCode, string levelName, string creadeBy)
        //{
        //    ReturnMsg msg = new ReturnMsg();
        //    msg.isSuccess = false;
        //    try
        //    {
        //        //檢查是否重複               
        //        string str_sql = @"select COUNT(level_code) as levelCount 
        //                        from ZT_MonitorLevel 
        //                        where level_code = @level_code 
        //                        group by level_code";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@level_code", levelCode.Trim()));
        //        DataTable dt_functionCount = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());
        //        if (dt_functionCount.Rows.Count > 0)
        //        {
        //            msg.isSuccess = false;
        //            msg.errorMsg = "資料重複!";
        //            return msg;

        //        }

        //        //新增
        //        str_sql = @"insert INTO  ZT_MonitorLevel(level_code,level_name,created_by,updated_by) values(@level_code,@level_name,@created_by,@updated_by)";
        //        m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@level_code", levelCode.Trim()));
        //        m_paraList.Add(new DBParameter("@level_name", levelName));
        //        m_paraList.Add(new DBParameter("@created_by", creadeBy.Trim()));
        //        m_paraList.Add(new DBParameter("@updated_by", creadeBy.Trim()));
        //        g_nixmonitorConnection.ExecNonQuery(str_sql, m_paraList.ToArray()); //回傳異動筆數
        //        msg.isSuccess = true;
        //        msg.successMsg = "新增成功";

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //        msg.isSuccess = false;
        //        msg.errorMsg = "新增失敗";
        //    }
        //    return msg;
        //}
        //#endregion

        //#region update level
        //public ReturnMsg UpdateLevel(string levelCode, string levelName, string userNo)
        //{
        //    ReturnMsg msg = new ReturnMsg();
        //    msg.isSuccess = false;
        //    try
        //    {
        //        string str_sql = @"UPDATE ZT_MonitorLevel SET level_name = @level_name ,updated_by = @updated_by,updated_date = @updated_date where level_code = @level_code ";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@level_code", levelCode.Trim()));
        //        m_paraList.Add(new DBParameter("@level_name", levelName));
        //        m_paraList.Add(new DBParameter("@updated_by", userNo.Trim()));
        //        m_paraList.Add(new DBParameter("@updated_date", DateTime.Now));
        //        g_nixmonitorConnection.ExecNonQuery(str_sql, m_paraList.ToArray());
        //        msg.isSuccess = true;
        //        msg.successMsg = "更新成功";
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //        msg.isSuccess = false;
        //        msg.errorMsg = "更新失敗";
        //    }
        //    return msg;
        //}
        //#endregion

        //#region query level
        //public IEnumerable<LevelBE> QueryLevel(string levelCode, string levelName)
        //{
        //    List<LevelBE> levelList = new List<LevelBE>();
        //    try
        //    {
        //        string str_sql = @"select level_code,level_name,created_by,created_date,updated_by,updated_date
        //                           from ZT_MonitorLevel
        //                           where 1=1 ";

        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        if (!string.IsNullOrEmpty(levelName))
        //        {
        //            str_sql += "and level_name like @level_name ";
        //            m_paraList.Add(new DBParameter("@level_name", levelName + "%"));
        //        }
        //        if (!string.IsNullOrEmpty(levelCode))
        //        {
        //            str_sql += "and level_code like @level_code ";
        //            m_paraList.Add(new DBParameter("@level_code", levelCode + "%"));
        //        }
        //        DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());

        //        foreach (DataRow dr in m_dataTable.Rows)
        //        {
        //            LevelBE level = new LevelBE();
        //            level.level_code = dr["level_code"] == null ? "" : dr["level_code"].ToString();
        //            level.level_name = dr["level_name"] == null ? "" : dr["level_name"].ToString();
        //            level.created_by = dr["created_by"] == null ? "" : dr["created_by"].ToString();
        //            level.created_date = dr["created_date"] == null ? "" : DateTime.Parse(dr["created_date"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
        //            level.updated_by = dr["updated_by"] == null ? "" : dr["updated_by"].ToString();
        //            level.updated_date = dr["updated_date"] == null ? "" : DateTime.Parse(dr["updated_date"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
        //            levelList.Add(level);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //    }
        //    return levelList;
        //}
        //#endregion

        //#region delete level
        //public ReturnMsg DeleteLevel(string levelCode)
        //{
        //    ReturnMsg msg = new ReturnMsg();
        //    msg.isSuccess = false;
        //    try
        //    {
        //        string str_sql = @"Delete from ZT_MonitorLevel where level_code = @level_code ";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@level_code", levelCode.Trim()));
        //        g_nixmonitorConnection.ExecNonQuery(str_sql, m_paraList.ToArray());
        //        msg.isSuccess = true;
        //        msg.successMsg = "刪除成功";
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //        msg.isSuccess = false;
        //        msg.errorMsg = "刪除失敗";
        //    }
        //    return msg;
        //}
        //#endregion

        #region levelSelectItem
        public IEnumerable<string[]> levelSelectItem()
        {
            List<string[]> levelList = new List<string[]>();
            try
            {
                string str_sql = @"select level_code,level_name,created_by,created_date,updated_by,updated_date
                                   from ZT_MonitorLevel
                                   where 1=1 ";

                List<DBParameter> m_paraList = new List<DBParameter>();
                DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());
                string[] first = new string[2];
                first[0] = "";
                first[1] = "請選擇";
                levelList.Add(first);
                foreach (DataRow dr in m_dataTable.Rows)
                {
                    string[] level = new string[2];
                    level[0] = dr["level_code"] == null ? "" : dr["level_code"].ToString();
                    level[1] = dr["level_name"] == null ? "" : dr["level_name"].ToString();
                    levelList.Add(level);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return levelList;
        }
        #endregion

        #endregion

        #region MonitorLog
        public IEnumerable<MonitorLogBE> QueryMonitorLog(string level, string function, DateTime sDate, DateTime eDate)
        {
            List<MonitorLogBE> MonitorLogList = new List<MonitorLogBE>();
            try
            {
                string sql = @"SELECT a.seq as seq,b.level_name as level_name,
c.function_name as function_name,a.[message] as [message],a.created_date as created_date,
u.user_id, u.user_name
FROM ZT_MonitorLog a
inner JOIN ZT_MonitorLevel b on a.level_code = b.level_code
inner JOIN ZT_MonitorFunction c on a.function_code = c.function_code 
LEFT JOIN ZT_AaUser AS u ON a.user_uuid = u.user_uuid
where a.created_date between @sDate and @eDate ";
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@sDate", sDate));
                m_paraList.Add(new DBParameter("@eDate", eDate));
                if (!string.IsNullOrEmpty(level))
                {
                    sql += " and a.level_code = @level_code";
                    m_paraList.Add(new DBParameter("@level_code", level));
                }
                if (!string.IsNullOrEmpty(function))
                {
                    sql += " and a.function_code = @function_code";
                    m_paraList.Add(new DBParameter("@function_code", function));
                }
                DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(sql, m_paraList.ToArray());
                foreach (DataRow dr in m_dataTable.Rows)
                {
                    MonitorLogBE monitor = new MonitorLogBE();
                    monitor.seq = dr["seq"].ToString();
                    monitor.level_name = dr["level_name"].ToString();
                    monitor.function_name = dr["function_name"].ToString();
                    monitor.message = dr["message"].ToString();
                    monitor.created_date = dr["created_date"].ToString();
                    if (dr["user_id"] != DBNull.Value)
                    {
                        monitor.user_id = (string)dr["user_id"].ToString();
                    }
                    else
                    {
                        monitor.user_id = string.Empty;
                    }
                    if (dr["user_name"] != DBNull.Value)
                    {
                        monitor.user_name = (string)dr["user_name"].ToString();
                    }
                    else
                    {
                        monitor.user_name = string.Empty;
                    }
                    MonitorLogList.Add(monitor);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return MonitorLogList;
        }
        #endregion

        #region notify

        //#region create notify
        //public ReturnMsg CreateNotify(string functionCode, string levelCode, string notifyNotify, string creadeBy)
        //{
        //    ReturnMsg msg = new ReturnMsg();
        //    msg.isSuccess = false;
        //    try
        //    {
        //        //檢查是否重複                
        //        string str_sql = @"select COUNT(function_code) as functionCount 
        //                           from ZT_MonitorSetting 
        //                           where function_code = @function_code and level_code = @level_code and notify_code = @notify_code 
        //                           group by function_code,level_code,notify_code";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@function_code", functionCode.Trim()));
        //        m_paraList.Add(new DBParameter("@level_code", levelCode.Trim()));
        //        m_paraList.Add(new DBParameter("@notify_code", notifyNotify.Trim()));
        //        DataTable dt_notifyCount = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());
        //        if (dt_notifyCount.Rows.Count > 0)
        //        {
        //            msg.isSuccess = false;
        //            msg.errorMsg = "資料重複!";
        //            return msg;
        //        }

        //        //新增
        //        str_sql = @"insert INTO  ZT_MonitorSetting(function_code,level_code,notify_code,created_by,updated_by) values(@function_code,@level_code,@notify_code,@created_by,@updated_by)";
        //        m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@function_code", functionCode.Trim()));
        //        m_paraList.Add(new DBParameter("@level_code", levelCode.Trim()));
        //        m_paraList.Add(new DBParameter("@notify_code", notifyNotify.Trim()));
        //        m_paraList.Add(new DBParameter("@created_by", creadeBy.Trim()));
        //        m_paraList.Add(new DBParameter("@updated_by", creadeBy.Trim()));
        //        g_nixmonitorConnection.ExecNonQuery(str_sql, m_paraList.ToArray());
        //        msg.isSuccess = true;
        //        msg.successMsg = "新增成功";

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //        msg.isSuccess = false;
        //        msg.errorMsg = "新增失敗";
        //    }
        //    return msg;
        //}
        //#endregion

        //#region query notify
        //public IEnumerable<MonitorBE> QueryNotify(string functionCode, string levelCode, string notifyCode)
        //{
        //    List<MonitorBE> notifyList = new List<MonitorBE>();
        //    try
        //    {
        //        string str_sql = @"select A.function_code,B.function_name AS function_name ,A.level_code,C.level_name AS level_name,A.notify_code,D.notify_name AS notify_name,A.created_by,A.created_date,A.updated_by,A.updated_date 
        //                           from ZT_MonitorSetting A 
        //                           inner join ZT_MonitorFunction B on A.function_code = B.function_code 
        //                           inner join ZT_MonitorLevel C on A.level_code = C.level_code 
        //                           inner join ZT_MonitorNotify D on A.notify_code = D.notify_code 
        //                           where 1=1 ";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        if (!string.IsNullOrEmpty(functionCode))
        //        {
        //            str_sql += " and A.function_code like @function_code ";
        //            m_paraList.Add(new DBParameter("@function_code", functionCode + "%"));
        //        }
        //        if (!string.IsNullOrEmpty(levelCode))
        //        {
        //            str_sql += " and A.level_code like @level_code ";
        //            m_paraList.Add(new DBParameter("@level_code", levelCode + "%"));
        //        }
        //        if (!string.IsNullOrEmpty(notifyCode))
        //        {
        //            str_sql += " and A.notify_code like @notify_code ";
        //            m_paraList.Add(new DBParameter("@notify_code", notifyCode + "%"));
        //        }
        //        DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());

        //        foreach (DataRow dr in m_dataTable.Rows)
        //        {
        //            MonitorBE notify = new MonitorBE();
        //            notify.function_code = dr["function_name"] == null ? "" : dr["function_code"].ToString() + "-" + dr["function_name"].ToString();
        //            notify.level_code = dr["level_name"] == null ? "" : dr["level_code"].ToString() + "-" + dr["level_name"].ToString();
        //            notify.notify_code = dr["notify_name"] == null ? "" : dr["notify_code"].ToString() + "-" + dr["notify_name"].ToString();
        //            notify.created_by = dr["created_by"] == null ? "" : dr["created_by"].ToString();
        //            notify.created_date = dr["created_date"] == null ? "" : DateTime.Parse(dr["created_date"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
        //            notify.updated_by = dr["updated_by"] == null ? "" : dr["updated_by"].ToString();
        //            notify.updated_date = dr["updated_date"] == null ? "" : DateTime.Parse(dr["updated_date"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
        //            notifyList.Add(notify);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //    }
        //    return notifyList;
        //}
        //#endregion

        //#region delete notify
        //public ReturnMsg DeleteNotify(string functionCode, string levelCode, string notifyCode)
        //{
        //    ReturnMsg msg = new ReturnMsg();
        //    msg.isSuccess = false;
        //    try
        //    {
        //        string str_sql = @"Delete from ZT_MonitorSetting where function_code = @function_code and level_code = @level_code and notify_code = @notify_code";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        m_paraList.Add(new DBParameter("@function_code", functionCode.Trim()));
        //        m_paraList.Add(new DBParameter("@level_code", levelCode.Trim()));
        //        m_paraList.Add(new DBParameter("@notify_code", notifyCode.Trim()));
        //        g_nixmonitorConnection.ExecNonQuery(str_sql, m_paraList.ToArray());
        //        msg.isSuccess = true;
        //        msg.successMsg = "刪除成功";
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //        msg.isSuccess = false;
        //        msg.errorMsg = "刪除失敗";
        //    }
        //    return msg;
        //}
        //#endregion

        //#region notify setting use DropdownList Item
        //public List<string[]> GetFunctionItem()
        //{
        //    List<string[]> itemList = new List<string[]>();
        //    try
        //    {
        //        string str_sql = @"select function_code,function_name  
        //                           from ZT_MonitorFunction";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());
        //        string[] firstitem = new string[2];
        //        firstitem[0] = "請選擇";
        //        firstitem[1] = "";
        //        itemList.Add(firstitem);
        //        foreach (DataRow dr in m_dataTable.Rows)
        //        {
        //            string[] item = new string[2];
        //            item[0] = dr["function_code"] == null ? "" : dr["function_code"].ToString();
        //            item[1] = dr["function_name"] == null ? "" : dr["function_code"].ToString() + "-" + dr["function_name"].ToString();
        //            itemList.Add(item);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //    }
        //    return itemList;
        //}

        //public List<string[]> GetLevelItem()
        //{
        //    List<string[]> itemList = new List<string[]>();
        //    try
        //    {
        //        string str_sql = @"select level_code,level_name  
        //                           from ZT_MonitorLevel";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());
        //        string[] firstitem = new string[2];
        //        firstitem[0] = "請選擇";
        //        firstitem[1] = "";
        //        itemList.Add(firstitem);
        //        foreach (DataRow dr in m_dataTable.Rows)
        //        {
        //            string[] item = new string[2];
        //            item[0] = dr["level_code"] == null ? "" : dr["level_code"].ToString();
        //            item[1] = dr["level_name"] == null ? "" : dr["level_code"].ToString() + "-" + dr["level_name"].ToString();
        //            itemList.Add(item);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //    }
        //    return itemList;
        //}

        //public List<string[]> GetNotifyItem()
        //{
        //    List<string[]> itemList = new List<string[]>();
        //    try
        //    {
        //        string str_sql = @"select notify_code,notify_name  
        //                           from ZT_MonitorNotify";
        //        List<DBParameter> m_paraList = new List<DBParameter>();
        //        DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(str_sql, m_paraList.ToArray());
        //        string[] firstitem = new string[2];
        //        firstitem[0] = "請選擇";
        //        firstitem[1] = "";
        //        itemList.Add(firstitem);
        //        foreach (DataRow dr in m_dataTable.Rows)
        //        {
        //            string[] item = new string[2];
        //            item[0] = dr["notify_code"] == null ? "" : dr["notify_code"].ToString();
        //            item[1] = dr["notify_name"] == null ? "" : dr["notify_code"].ToString() + "-" + dr["notify_name"].ToString();
        //            itemList.Add(item);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //    }
        //    return itemList;
        //}

        //#endregion

        #endregion

        #region NotifyType

//        /// <summary>
//        /// 測試通知系統查詢 octo.Class By genus = 'notify'
//        /// </summary>
//        /// <returns></returns>
//        public IEnumerable<string[]> Querynotify_type()
//        {
//            List<string[]> m_list = new List<string[]>();

//            string m_sql = @" select code_id,code_name from [dbo].[sys_code_info]where cate='NOTIFY_TYPE' ";

//            List<DBParameter> m_paraList = new List<DBParameter>();

//            DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
//            for (int i = 0; i < m_dataTable.Rows.Count; i++)
//            {
//                string[] m_string = new string[2];
//                m_string[0] = m_dataTable.Rows[i]["code_id"].ToString().Trim();
//                m_string[1] = m_dataTable.Rows[i]["code_id"].ToString().Trim() + "-" + m_dataTable.Rows[i]["code_name"].ToString();
//                m_list.Add(m_string);
//            }
//            return m_list;
//        }

//        /// <summary>octo.Class
//        /// </summary>
//        /// <param name="p_be"></param>
//        /// <returns></returns>
//        public bool insertNotifytype(NotifytypeBE p_be)
//        {

//            bool m_success = false;
//            string m_sql = @"INSERT INTO ZT_MonitorNotify (notify_code,notify_name,type,created_by,created_date,updated_by,updated_date) 
//VALUES (@notify_code,@notify_name,@type,@created_by,@created_date,@updated_by,@updated_date)";
//            try
//            {
//                List<DBParameter> m_paraList = new List<DBParameter>();
//                m_paraList.Add(new DBParameter("@notify_code", p_be.notify_code));
//                m_paraList.Add(new DBParameter("@notify_name", p_be.notify_name));
//                m_paraList.Add(new DBParameter("@type", p_be.type));
//                m_paraList.Add(new DBParameter("@created_by", p_be.created_by));
//                m_paraList.Add(new DBParameter("@created_date", p_be.created_date));
//                m_paraList.Add(new DBParameter("@updated_by", p_be.updated_by));
//                m_paraList.Add(new DBParameter("@updated_date", p_be.updated_date));

//                g_nixmonitorConnection.ExecNonQuery(m_sql, m_paraList.ToArray());
//                m_success = true;
//            }
//            catch (Exception ex)
//            {
//                logger.Error(ex.Message, ex);
//                m_success = false;
//            }
//            return m_success;
//        }

//        /// <summary>
//        /// 測試通知系統新增dtl 
//        /// </summary>
//        /// <param name="p_be"></param>
//        /// <returns></returns>
//        public bool insertNotifytypedtl(NotifytypeBE p_be)
//        {

//            bool m_success = false;
//            string m_sql = @"INSERT INTO ZT_MonitorNotifyDetail (notify_code,address,created_by,created_date,updated_by,updated_date) 
//VALUES (@notify_code,@address,@created_by,@created_date,@updated_by,@updated_date) ";
//            try
//            {
//                List<DBParameter> m_paraList = new List<DBParameter>();
//                m_paraList.Add(new DBParameter("@notify_code", p_be.notify_code));
//                m_paraList.Add(new DBParameter("@address", p_be.address));
//                m_paraList.Add(new DBParameter("@created_by", p_be.created_by));
//                m_paraList.Add(new DBParameter("@created_date", p_be.created_date));
//                m_paraList.Add(new DBParameter("@updated_by", p_be.updated_by));
//                m_paraList.Add(new DBParameter("@updated_date", p_be.updated_date));

//                g_nixmonitorConnection.ExecNonQuery(m_sql, m_paraList.ToArray());
//                m_success = true;
//            }
//            catch (Exception ex)
//            {
//                logger.Error(ex.Message, ex);
//                m_success = false;
//            }
//            return m_success;
//        }

//        /// <summary>
//        /// 測試通知系統刪除 octo.Class
//        /// </summary>
//        /// <param name="p_be"></param>
//        /// <returns></returns>
//        public bool deleteNotifytype(string p_notify_code)
//        {

//            bool m_success = false;
//            string m_sql = @"delete ZT_MonitorNotifyDetail where notify_code=@notify_code  
//delete ZT_MonitorNotify where notify_code=@notify_code ";
//            try
//            {
//                List<DBParameter> m_paraList = new List<DBParameter>();
//                m_paraList.Add(new DBParameter("@notify_code", p_notify_code));

//                g_nixmonitorConnection.ExecNonQuery(m_sql, m_paraList.ToArray());
//                m_success = true;
//            }
//            catch (Exception ex)
//            {
//                logger.Error(ex.Message, ex);
//                m_success = false;
//            }
//            return m_success;
//        }

//        /// <summary>
//        /// 測試通知系統dtl 刪除 octo.Class
//        /// </summary>
//        /// <param name="p_be"></param>
//        /// <returns></returns>
//        public bool deleteNotifytypedtl(string p_notify_code, string p_address, string p_id)
//        {

//            bool m_success = false;
//            string m_sql = @" delete ZT_MonitorNotifyDetail where notify_code=@notify_code and address=@address and id=@id ";
//            try
//            {
//                List<DBParameter> m_paraList = new List<DBParameter>();
//                m_paraList.Add(new DBParameter("@id", p_id));
//                m_paraList.Add(new DBParameter("@notify_code", p_notify_code));
//                m_paraList.Add(new DBParameter("@address", p_address));

//                g_nixmonitorConnection.ExecNonQuery(m_sql, m_paraList.ToArray());
//                m_success = true;
//            }
//            catch (Exception ex)
//            {
//                logger.Error(ex.Message, ex);
//                m_success = false;
//            }
//            return m_success;
//        }

//        /// <summary>
//        /// 測試通知系統查詢 Notifytype
//        /// </summary>
//        /// <param name="p_notifytypeVM"></param>
//        /// <returns></returns>
//        public IEnumerable<NotifytypeBE> selectNotifytype(NotifytypeViewModel p_notifytypeVM)
//        {
//            List<NotifytypeBE> m_result = new List<NotifytypeBE>();
//            List<DBParameter> m_paraList = new List<DBParameter>();

//            string m_sql = @"select * from ZT_MonitorNotify(nolock) where 1=1 ";

//            try
//            {

//                if (p_notifytypeVM.type != "")
//                {
//                    m_sql += " AND type=@type ";
//                    m_paraList.Add(new DBParameter("@type", p_notifytypeVM.type));
//                }
//                if (p_notifytypeVM.notify_name != "" && p_notifytypeVM.notify_name != null)
//                {
//                    m_sql += " and notify_name=@notify_name ";
//                    m_paraList.Add(new DBParameter("@notify_name", p_notifytypeVM.notify_name));
//                }


//                m_sql += " order by type desc ";

//                DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(m_sql, m_paraList.ToArray());
//                for (int i = 0; i < m_dataTable.Rows.Count; i++)
//                {
//                    NotifytypeBE m_be = genNotifytypeBE(m_dataTable.Rows[i]);
//                    m_result.Add(m_be);
//                }
//            }
//            catch (Exception ex)
//            {
//                logger.Error(ex.Message, ex);
//            }
//            return m_result;
//        }
//        private NotifytypeBE genNotifytypeBE(DataRow p_row)
//        {
//            NotifytypeBE m_be = new NotifytypeBE();

//            m_be.notify_code = p_row["notify_code"] == null ? "" : p_row["notify_code"].ToString().Trim();
//            m_be.notify_name = p_row["notify_name"] == null ? "" : p_row["notify_name"].ToString().Trim();
//            m_be.type = p_row["type"] == null ? "" : p_row["type"].ToString().Trim();
//            m_be.created_by = p_row["created_by"] == null ? "" : p_row["created_by"].ToString().Trim();
//            m_be.created_date = DateTime.Parse(p_row["created_date"].ToString());
//            m_be.updated_by = p_row["updated_by"] == null ? "" : p_row["updated_by"].ToString().Trim();
//            m_be.updated_date = DateTime.Parse(p_row["updated_date"].ToString());

//            return m_be;
//        }

//        /// <summary>
//        /// 測試通知系統dtl查詢 Notifytypedtl
//        /// </summary>
//        /// <param name="p_notifytypeVM"></param>
//        /// <returns></returns>
//        public IEnumerable<NotifytypeBE> selectNotifytypedtl(NotifytypeViewModel p_notifytypeVM)
//        {
//            List<NotifytypeBE> m_result = new List<NotifytypeBE>();
//            List<DBParameter> m_paraList = new List<DBParameter>();

//            string m_sql = @"select * from ZT_MonitorNotifyDetail(nolock) where 1=1 ";

//            try
//            {

//                if (p_notifytypeVM.notify_code != "" && p_notifytypeVM.notify_code != null)
//                {
//                    m_sql += " AND notify_code=@notify_code ";
//                    m_paraList.Add(new DBParameter("@notify_code", p_notifytypeVM.notify_code));
//                }
//                if (p_notifytypeVM.addresss != "" && p_notifytypeVM.addresss != null)
//                {
//                    m_sql += " AND addresss=@addresss ";
//                    m_paraList.Add(new DBParameter("@addresss", p_notifytypeVM.addresss));
//                }


//                m_sql += " order by notify_code desc ";

//                DataTable m_dataTable = g_nixmonitorConnection.GetDataTable(m_sql, m_paraList.ToArray());
//                for (int i = 0; i < m_dataTable.Rows.Count; i++)
//                {
//                    NotifytypeBE m_be = genNotifytypedtlBE(m_dataTable.Rows[i]);
//                    m_result.Add(m_be);
//                }
//            }
//            catch (Exception ex)
//            {
//                logger.Error(ex.Message, ex);
//            }
//            return m_result;
//        }
//        private NotifytypeBE genNotifytypedtlBE(DataRow p_row)
//        {
//            NotifytypeBE m_be = new NotifytypeBE();

//            m_be.notify_code = p_row["notify_code"] == null ? "" : p_row["notify_code"].ToString().Trim();
//            m_be.address = p_row["address"] == null ? "" : p_row["address"].ToString().Trim();
//            m_be.created_by = p_row["created_by"] == null ? "" : p_row["created_by"].ToString().Trim();
//            m_be.created_date = DateTime.Parse(p_row["created_date"].ToString());
//            m_be.updated_by = p_row["updated_by"] == null ? "" : p_row["updated_by"].ToString().Trim();
//            m_be.updated_date = DateTime.Parse(p_row["updated_date"].ToString());
//            m_be.id = p_row["id"] == null ? "" : p_row["id"].ToString().Trim();

//            return m_be;
//        }

//        /// <summary>
//        /// 測試通知管理修改 octo.Notifytype
//        /// </summary>
//        /// <param name="p_be"></param>
//        /// <returns></returns>
//        public bool updateNotifytype(NotifytypeBE p_be)
//        {
//            bool m_success = false;

//            DateTime now = DateTime.Now;
//            string sqlUpdate = string.Empty;
//            //string sqlSubjectUpdate = string.Empty;

//            if (string.IsNullOrEmpty(p_be.notify_name))
//            {
//                sqlUpdate = " UPDATE ZT_MonitorNotify SET type=@type,updated_by=@updated_by,updated_date=getdate() WHERE notify_code=@notify_code ";
//            }
//            if (string.IsNullOrEmpty(p_be.type))
//            {
//                sqlUpdate = " UPDATE ZT_MonitorNotify SET notify_name=@notify_name,updated_by=@updated_by,updated_date=getdate() WHERE notify_code=@notify_code ";
//            }
//            if (!string.IsNullOrEmpty(p_be.notify_name) && !string.IsNullOrEmpty(p_be.type))
//            {
//                sqlUpdate = " UPDATE ZT_MonitorNotify SET type=@type,notify_name=@notify_name,updated_by=@updated_by,updated_date=getdate() WHERE notify_code=@notify_code ";
//            }

//            try
//            {
//                List<DBParameter> m_paraList = new List<DBParameter>();
//                m_paraList.Add(new DBParameter("@type", p_be.type));
//                m_paraList.Add(new DBParameter("@notify_name", p_be.notify_name));
//                m_paraList.Add(new DBParameter("@notify_code", p_be.notify_code));
//                m_paraList.Add(new DBParameter("@updated_by", p_be.updated_by));
//                m_paraList.Add(new DBParameter("@updated_date", p_be.updated_date));

//                g_nixmonitorConnection.ExecNonQuery(sqlUpdate, m_paraList.ToArray());

//                m_success = true;
//            }
//            catch (Exception ex)
//            {
//                logger.Error(ex.Message, ex);
//                m_success = false;
//            }
//            return m_success;
//        }

//        /// <summary>
//        /// 測試通知管理dtl修改 octo.Notifytypedtl
//        /// </summary>
//        /// <param name="p_be"></param>
//        /// <returns></returns>
//        public bool updateNotifytypedtl(NotifytypeBE p_be)
//        {
//            bool m_success = false;

//            DateTime now = DateTime.Now;
//            string sqlUpdate = string.Empty;
//            //string sqlSubjectUpdate = string.Empty;

//            sqlUpdate = " update ZT_MonitorNotifyDetail set address=@address, updated_by = @updated_by, updated_date = getdate() where notify_code = @notify_code and id = @id ";

//            try
//            {
//                List<DBParameter> m_paraList = new List<DBParameter>();
//                m_paraList.Add(new DBParameter("@id", p_be.id));
//                m_paraList.Add(new DBParameter("@notify_code", p_be.notify_code));
//                m_paraList.Add(new DBParameter("@address", p_be.address));
//                m_paraList.Add(new DBParameter("@updated_by", p_be.updated_by));
//                m_paraList.Add(new DBParameter("@updated_date", p_be.updated_date));

//                g_nixmonitorConnection.ExecNonQuery(sqlUpdate, m_paraList.ToArray());

//                m_success = true;
//            }
//            catch (Exception ex)
//            {
//                logger.Error(ex.Message, ex);
//                m_success = false;
//            }
//            return m_success;
//        }

        #endregion

        #endregion
    }
}
