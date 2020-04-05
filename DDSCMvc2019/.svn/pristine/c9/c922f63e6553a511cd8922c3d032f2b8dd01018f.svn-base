using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PortalService.Contract.ViewModel;

namespace PortalService.Impl.BL
{
    [Serializable]
    public static class BLHelper
    {

        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();
                    foreach (var prop in obj.GetType().GetProperties().Where(p => p.CanWrite))
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            //recInfo
                            if (propertyInfo.PropertyType.Name == "RecInfoModel")
                            {
                                propertyInfo.SetValue(obj, DataRowToBE<RecInfoModel>(row));
                            }

                            if (ContainsColumn(row, prop.Name))
                            {
                                //可能DB和class型別不同
                                if (row[prop.Name].GetType() == propertyInfo.PropertyType)
                                {
                                    //propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                                    propertyInfo.SetValue(obj, GetValue(row, prop.Name, null));
                                }
                                else
                                {
                                    //當DB和Class型別不同
                                    //DB=guid或datatime而class是String
                                    if (row.IsNull(prop.Name) == false
                                        && (string.IsNullOrWhiteSpace(propertyInfo.GetValue(obj).ToString())
                                            || propertyInfo.GetValue(obj).ToString() == Guid.Empty.ToString())
                                        && propertyInfo.PropertyType.ToString() == "System.String")
                                    {
                                        propertyInfo.SetValue(obj, row[prop.Name].ToString());
                                    }
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a DataTable to a object of generic object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T DataRowToBE<T>(this DataRow row) where T : class, new()
        {
            try
            {
                T obj = new T();
                foreach (var prop in obj.GetType().GetProperties().Where(p => p.CanWrite))
                {
                    try
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        //recInfo
                        if (propertyInfo.PropertyType.Name == "RecInfoModel")
                        {
                            propertyInfo.SetValue(obj, DataRowToBE<RecInfoModel>(row));
                        }

                        if (ContainsColumn(row, prop.Name))
                        {
                            //可能DB和class型別不同
                            if(row[prop.Name].GetType() == propertyInfo.PropertyType)
                            {
                                //propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                                propertyInfo.SetValue(obj, GetValue(row, prop.Name, null));
                            }
                            else
                            {
                                //當DB和Class型別不同
                                //DB=guid或datatime而class是String
                                if (row.IsNull(prop.Name) == false
                                    && (string.IsNullOrWhiteSpace(propertyInfo.GetValue(obj).ToString())
                                        || propertyInfo.GetValue(obj).ToString() == Guid.Empty.ToString())
                                    && propertyInfo.PropertyType.ToString() == "System.String")
                                {
                                    propertyInfo.SetValue(obj, row[prop.Name].ToString());
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }

                }

                return obj;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 取DB值-先判斷有指定欄位,再取值,無時給預設值(如null..等)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object GetValue(this DataRow row, string column, object defaultValue = null)
        {
            return row.Table.Columns.Contains(column) ? row[column] : defaultValue;
        }

        /// <summary>
        /// 判斷位是否存在DB
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool ContainsColumn(this DataRow row, string column)
        {
            return row.Table.Columns.Contains(column);
        }

    }
}
