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
                int strError;
 
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "@userName";
                p1.Value = userName;
                p1.SqlDbType = SqlDbType.VarChar;

                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "@password";
                p2.Value = password;
                p2.SqlDbType = SqlDbType.VarChar;

                strError = DataAccess.Execute("App_Login", p1, p2);

                if (strError == 0)
                    Result = "Failed to Login";
                else
                    Result = "0";
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
            return Result;
        }
        public DataTable GetPermisson(string userName)
        {
            DataTable iRead;
            SqlParameter p1 = new SqlParameter();
            p1.ParameterName = "@userName";
            p1.Value = userName;
            p1.SqlDbType = SqlDbType.VarChar;


            iRead = DataAccess.FillDataTable("Main_CheckPermison", p1);
            return iRead;
        }
    
    }
}
