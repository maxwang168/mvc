using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.DBA
{
    public class DBParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public SqlDbType SqlDataType { get; set; }
        public int DBType { get; set; }

        public static int DB_TYPE_SQL = 1;
        public static int DB_TYPE_ORACLE = 2;

        public DBParameter (string p_name, object p_value)
        {
            Name = p_name;
            Value = p_value;
            DBType = DB_TYPE_SQL;
        }
        
        public DBParameter(string p_name, object p_value, SqlDbType p_dataType)
        {
            Name = p_name;
            Value = p_value;
            SqlDataType = p_dataType;
            DBType = DB_TYPE_ORACLE;
        }
    }
}
