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
    public class Permission
    {
        public string Login(string userName, string password)
        {
            string Result;
            try
            {
                DataTable strError;
 
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "@userName";
                p1.Value = userName;
                p1.SqlDbType = SqlDbType.VarChar;

                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "@password";
                p2.Value = password;
                p2.SqlDbType = SqlDbType.VarChar;

                strError = DataAccess.GetFromDataTable("App_Login", p1, p2);
                Result = strError.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public DataTable GetPermisson(string UserID)
        {
            DataTable iRead;
            SqlParameter p1 = new SqlParameter();
            p1.ParameterName = "@userID";
            p1.Value = UserID;
            p1.SqlDbType = SqlDbType.VarChar;


            iRead = DataAccess.GetFromDataTable("App_CheckPermison", p1);
            return iRead;
        }
        public DataTable LoadMenu()
        {
            DataTable iRead;
            SqlParameter p1 = new SqlParameter();
            p1.ParameterName = "@ServiceID";
            p1.Value = "APP";
            p1.SqlDbType = SqlDbType.VarChar;


            iRead = DataAccess.GetFromDataTable("App_LoadMenu", p1);
            return iRead;
        }

    }
}
