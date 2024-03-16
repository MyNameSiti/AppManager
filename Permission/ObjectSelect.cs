using Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission
{
    public class ObjectSelect
    {
        public DataTable GetTable(string tableName)
        {
            DataTable iRead;
            SqlParameter p1 = new SqlParameter();
            p1.ParameterName = "@tableName";
            p1.Value = tableName;
            p1.SqlDbType = SqlDbType.VarChar;

            SqlParameter p2 = new SqlParameter();
            p2.ParameterName = "@MenuID";
            p2.Value = tableName;
            p2.SqlDbType = SqlDbType.VarChar;

            iRead = DataAccess.GetFromDataTable("App_SelectObject", p1, p2);
            return iRead;
        }
    }
}
