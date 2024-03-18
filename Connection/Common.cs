using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection
{
    public class Common
    {
        //JSON Message
        public static DataTable DBIOUTPUTDEFINEHTTP;
        public static DataTable DBIOUTPUTDEFINEHTTPHEADER;
        public static DataTable DBIINPUTDEFINEHTTP;

        //XML Message
        public static DataTable DBIOUTPUTDEFINEXML;
        public static DataTable DBIINPUTDEFINEXML;

        //Connecttion
        public static DataTable DBICONNECTIONWS;
        public Common()
        {
            //Load Setting JSON Message
            DBIOUTPUTDEFINEHTTP = DataAccess.FillDataTableSQL("SELECT * FROM OUTPUTDEFINEHTTP");
            DBIOUTPUTDEFINEHTTPHEADER = DataAccess.FillDataTableSQL("SELECT * FROM OUTPUTDEFINEHTTPHEADER");
            DBIINPUTDEFINEHTTP = DataAccess.FillDataTableSQL("SELECT * FROM INPUTDEFINEHTTP");

            //Load Setting XML Message
            DBIOUTPUTDEFINEXML = DataAccess.FillDataTableSQL("SELECT * FROM OUTPUTDEFINEXML");
            DBIINPUTDEFINEXML = DataAccess.FillDataTableSQL("SELECT * FROM INPUTDEFINEXML");

            //Load Web Service Connection
            DBICONNECTIONWS = DataAccess.FillDataTableSQL("SELECT * FROM CONNECTIONWS");

        }
    }
}
