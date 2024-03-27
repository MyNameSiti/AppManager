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
        public DataTable GetTable(string PageID)
        {
            DataTable iRead;
            SqlParameter p1 = new SqlParameter();
            p1.ParameterName = "@PageID";
            p1.Value = PageID;
            p1.SqlDbType = SqlDbType.VarChar;

            iRead = DataAccess.GetFromDataTable("App_SelectObject", p1);
            return iRead;
        }
    }
}
