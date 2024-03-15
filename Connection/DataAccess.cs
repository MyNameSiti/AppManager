using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Collections;


namespace Connection
{
    /// <summary>
    /// Data access helper class, which controls the complete database interaction with the database for all objects.
    /// SqlServer specific.
    /// </summary>
    public static class DataAccess
    {
        #region Encrypt Dencrypt Connect String
        public static string EncryptData(string Data)
        {
            try
            {
                byte[] PP = Encoding.Unicode.GetBytes("IPC");
                HashAlgorithm HashPassword = HashAlgorithm.Create("MD5");
                byte[] V = { 0, 9, 0, 4, 4, 9, 5, 9, 4, 2, 2, 2, 0, 6, 8, 2 };
                RijndaelManaged Encrypt = new RijndaelManaged();
                Encrypt.Key = HashPassword.ComputeHash(PP);
                ICryptoTransform encryptor = Encrypt.CreateEncryptor(Encrypt.Key, V);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                byte[] DataBytes = Encoding.Unicode.GetBytes(Data);
                cs.Write(DataBytes, 0, DataBytes.Length);
                cs.Close();
                byte[] EncryptedData = ms.ToArray();
                ms.Close();
                return Convert.ToBase64String(EncryptedData);
            }
            catch
            {
                return "";
            }
        }
        public static string DecryptData(string Data)
        {
            try
            {
                byte[] PP = Encoding.Unicode.GetBytes("IPC");
                byte[] DataEncryptedByte = Convert.FromBase64String(Data);
                HashAlgorithm HashPassword = HashAlgorithm.Create("MD5");
                byte[] V = { 0, 9, 0, 4, 4, 9, 5, 9, 4, 2, 2, 2, 0, 6, 8, 2 };
                RijndaelManaged Decrypt = new RijndaelManaged();
                Decrypt.Key = HashPassword.ComputeHash(PP);
                ICryptoTransform decryptor = Decrypt.CreateDecryptor(Decrypt.Key, V);
                MemoryStream ms = new MemoryStream(DataEncryptedByte);
                CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                byte[] Result = new byte[DataEncryptedByte.Length];
                cs.Read(Result, 0, Result.Length);
                ms.Close();
                cs.Close();
                return Encoding.Unicode.GetString(Result).Replace("\0", "");
            }
            catch
            {
                return "";
            }
        }
        #endregion
        private static string ConnectionString
        {
            get
            {
                try
                {
                    return DecryptData(ConfigurationManager.AppSettings["Connection"].ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to get Database Connection string from Web Config File. Contact the site Administrator" + ex);
                }
            }
        }

        #region SQL Excute
        public static DataTable GetFromDataTable(string SPName, params SqlParameter[] Parameters)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(SPName, cn);
            DataTable dt = new DataTable();
            IDataReader dr;

            try
            {
                cmd.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["ConTimeout"].ToString());
            }
            catch { };

            cmd.CommandType = CommandType.StoredProcedure;

            if (Parameters != null)
                foreach (SqlParameter item in Parameters)
                    cmd.Parameters.Add(item);

            cn.Open();

            try
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr != null)
                {
                    dt.Load(dr);
                }
            }
            catch (SqlException ex)
            {
                if (cn != null) cn.Close();
                throw ex;
            }

            cmd = null;
            cn = null;

            return dt;
        }
        public static int Execute(string SPName, params SqlParameter[] Parameters)
        {
            int intErr = 0;
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(SPName, cn);
            cmd.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["ConTimeout"].ToString());
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            if (Parameters != null)
                foreach (SqlParameter item in Parameters)
                    cmd.Parameters.Add(item);

            cn.Open();
            try
            {
                intErr = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (cn != null) cn.Close();
                throw ex;
            }
            cn.Close();

            cmd = null;
            cn = null;

            return intErr;
        }
        public static int ExecuteSQL(string sqlString)
        {
            int intErr = 0;
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sqlString, cn);

            try
            {
                cmd.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["ConTimeout"].ToString());
            }
            catch { };

            cn.Open();
            try
            {
                intErr = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (cn != null) cn.Close();
                throw ex;
            }
            cn.Close();

            cmd = null;
            cn = null;

            return intErr;
        }
        public static DataTable FillDataTable(string p_strSPname, params object[] p_arrValue)
        {
            DataTable p_dtData = new DataTable();
            if ((p_arrValue != null) && (p_arrValue.Length > 0))
            {
                // Tạo danh sách SqlParameter
                SqlHelperParameterCache objOHPC = new SqlHelperParameterCache();
                SqlParameter[] arrSQLParameter = objOHPC.GetSpParameterSet(ConnectionString, p_strSPname);

                // Gán dữ liệu từ các mãng value vô mảng command parameter
                AssignParameterValues(arrSQLParameter, p_arrValue);

                // gọi hàm overload
                FillDataTable(ConnectionString, p_dtData, p_strSPname, arrSQLParameter);
            }

            else
            {
                FillDataTable(ConnectionString, p_dtData, p_strSPname, null);
            }

            return p_dtData;
        }
        public static DataTable FillDataTableSQL(string p_strSQLCommand)
        {
            //bool includeReturnValueParameter = true;
            DataTable p_dtData = new DataTable();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            try
            {
                cmd.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["ConTimeout"].ToString());
            }
            catch { };

            try
            {
                PrepareCommandSQL(cmd, conn, (SqlTransaction)null, p_strSQLCommand);
                da.Fill(p_dtData);
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                cmd.Dispose();
                da.Dispose();
                p_dtData.Dispose();
            }

            return p_dtData;
        }
        private static void AssignParameterValues(SqlParameter[] p_arrSQLParameter, object[] p_arrValue)
        {
            if ((p_arrSQLParameter == null) || (p_arrValue == null))
            {
                return;
            }

            if (p_arrSQLParameter.Length != p_arrValue.Length)
            {
                throw new Exception("Parameter count does not match Parameter Value count.");
            }

            for (int i = 0, j = p_arrSQLParameter.Length; i < j; i++)
            {
                p_arrSQLParameter[i].Value = p_arrValue[i];
            }
        }
        public static DataSet FillDataSet(string sql)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception)
            {
                return ds;
            }
            return ds;
        }
        private static void PrepareCommandSQL(SqlCommand p_cmd, SqlConnection p_conn, SqlTransaction p_trans, string p_strSQLCommand)
        {
            //if the provided connection is not open, we will open it
            if (p_conn.State != ConnectionState.Open)
            {
                p_conn.Open();
            }

            //associate the connection with the command
            p_cmd.Connection = p_conn;

            //set the command text (SQL statement)
            p_cmd.CommandText = p_strSQLCommand;

            //if we were provided a transaction, assign it.
            if (p_trans != null)
            {
                p_cmd.Transaction = p_trans;
            }

            //set the command type
            p_cmd.CommandType = CommandType.Text;
        }
        #endregion
    }
    public sealed class SqlHelperParameterCache
    {

        public SqlHelperParameterCache() { }

        private Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
        private SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(spName, cn);
            SqlParameter[] discoveredParameters;

            try
            {
                cmd.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["ConTimeout"].ToString());
            }
            catch { };

            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlCommandBuilder.DeriveParameters(cmd);

                if (!includeReturnValueParameter)
                {
                    cmd.Parameters.RemoveAt(0);
                }

                discoveredParameters = new SqlParameter[cmd.Parameters.Count];

                cmd.Parameters.CopyTo(discoveredParameters, 0);

                //vutran 29/09
                foreach (SqlParameter parameter in cmd.Parameters)
                {
                    if (parameter.SqlDbType != SqlDbType.Structured)
                    {
                        continue;
                    }
                    string name = parameter.TypeName;
                    int index = name.IndexOf(".");
                    if (index == -1)
                    {
                        continue;
                    }
                    name = name.Substring(index + 1);
                    if (name.Contains("."))
                    {
                        parameter.TypeName = name;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                cn.Close();
                cmd.Dispose();
            }

            return discoveredParameters;
        }
        private SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            //deep copy of cached SqlParameter array
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }
        public void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }
        public SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = connectionString + ":" + commandText;

            SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];

            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }
        public SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }
        public SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            string hashKey = connectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            SqlParameter[] cachedParameters;

            cachedParameters = (SqlParameter[])paramCache[hashKey];

            if (cachedParameters == null)
            {
                cachedParameters = (SqlParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
            }

            return CloneParameters(cachedParameters);
        }
    }
}
