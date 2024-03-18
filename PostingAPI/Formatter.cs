using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Connection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Collections.Specialized.BitVector32;
using System.Security.Policy;

namespace PostingAPI
{
    public class Formatter
    {
        public static string CreateJSONMessage(string TranCode, string ServiceID)
        {
            //build header

            string resultHeader = "";
            string conditionHeader = " SOURCEID = '" + ServiceID + "'";
            conditionHeader += " AND TRANCODE = '" + TranCode + "'";
            DataRow[] drHeader = Common.DBIOUTPUTDEFINEHTTPHEADER.Select(conditionHeader, "FIELDNO");
            for (int i = 0; i < drHeader.Length; i++)
            {
                string FieldNo = drHeader[i]["FIELDNO"].ToString();
                string FieldName = drHeader[i]["FIELDNAME"].ToString();
                string FieldValue = drHeader[i]["FIELDVALUE"].ToString();
                //Get header Value 
                object value = null;
                //siti - 18032024 - sẽ cập nhật khi đủ năng lực

                resultHeader += value.ToString() + ",";
            }
            if (!string.IsNullOrEmpty(resultHeader))
                resultHeader = "{" + resultHeader.Substring(0, resultHeader.Length - 1) + "}";

            string conditionBody = " SOURCEID = '" + ServiceID + "'";
            conditionBody += " AND TRANCODE = '" + TranCode + "'";
            DataRow[] drBody = Common.DBIOUTPUTDEFINEHTTP.Select(conditionBody, "FIELDNO");
            JObject jBody = new JObject();
            for (int i = 0; i < drBody.Length; i++)
            {
                string FieldNo = drBody[i]["FIELDNO"].ToString();
                string FieldName = drBody[i]["FIELDNAME"].ToString();
                string FieldValue = drBody[i]["FIELDVALUE"].ToString();
                //Get Body Value
                object value = null;
                //siti - 18032024 - sẽ cập nhật khi đủ năng lực tạm thời sẽ để value là rỗng 


                JObject jCurrent = jBody;
                string[] arrJsonPath = FieldName.Trim().Split('.');
                for (int j = 0; j < arrJsonPath.Length; j++)
                {
                    string strPath = arrJsonPath[j].Trim();
                    JObject jSelect = (JObject)jCurrent.SelectToken(strPath);
                    if (jSelect == null)
                    {
                        if (strPath.EndsWith("]"))
                        {
                            string pathKey = strPath.Split('[')[0];
                            int pathPos = int.Parse(strPath.Split('[')[1].Substring(0, strPath.Split('[')[1].Length - 1));
                            if (jCurrent.SelectToken(pathKey) != null)
                            {
                                JArray jArr = (JArray)jCurrent.SelectToken(pathKey);
                                if (jArr.Count <= pathPos)
                                {
                                    for (int kk = jArr.Count; kk <= pathPos; kk++)
                                    {
                                        if (j != arrJsonPath.Length - 1)
                                        {
                                            jArr.Add(new JObject());
                                        }
                                        else
                                        {
                                            jArr.Add(value);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                jSelect = jCurrent;
                                jSelect.Add(pathKey, new JArray());
                                JArray jArr = (JArray)jSelect.SelectToken(pathKey);
                                if (jArr.Count <= pathPos)
                                {
                                    for (int kk = jArr.Count; kk <= pathPos; kk++)
                                    {
                                        if (j != arrJsonPath.Length - 1)
                                        {
                                            jArr.Add(new JObject());
                                        }
                                        else
                                        {
                                            jArr.Add(value);
                                        }
                                    }

                                }
                            }

                            if (j != arrJsonPath.Length - 1)
                            {
                                jSelect = (JObject)jCurrent.SelectToken(strPath);
                            }
                        }
                        else
                        {
                            jSelect = jCurrent;
                            if (arrJsonPath.Length == j + 1)
                            {
                                jSelect.Add(new JProperty(strPath, value));
                            }
                            else
                            {
                                jSelect.Add(strPath, new JObject());
                                jSelect = (JObject)jSelect.SelectToken(strPath);
                            }
                        }
                    }

                    jCurrent = jSelect;
                }


            }
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            dicResult.Add("HTTPHEADER", string.IsNullOrEmpty(resultHeader) ? new JObject() : JObject.Parse(resultHeader));
            dicResult.Add("HTTPBODY", jBody);
            return JsonConvert.SerializeObject(dicResult);
        }
        public static JObject NewParse(string value)
        {
            return JObject.Load(new JsonTextReader(new StringReader(value)) { FloatParseHandling = FloatParseHandling.Decimal }, null);
        }
        public static HttpWebRequest initConnection(string TranCode, string ServiceID)
        {
            string _Url = string.Empty;
            string _Action = string.Empty;
            string _ContentType = string.Empty;
            string condition = "TRANCODE = '" + TranCode + "'";
            DataRow[] row = Common.DBICONNECTIONWS.Select(condition);
            _Url = row[0]["URLWEBSERVICE"].ToString().Trim();
            _Action = row[0]["SOAPACTION"].ToString().Trim();
            _ContentType = row[0]["CONTENTTYPE"].ToString().Trim();
            //By pass certificate error
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (Object sender1, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_Url);
            request.Method = _Action;
            request.ContentType = _ContentType;
            return request;
        }
        public static bool PostJSONMessage(string TranCode, string ServiceID)
        {

            //Prepare 
            HttpWebRequest request = initConnection(TranCode, ServiceID);
            JObject jo = new JObject();
            JObject jHeader = new JObject();
            jo = NewParse(CreateJSONMessage(TranCode, ServiceID));
            jHeader = NewParse(jo.SelectToken("HTTPHEADER").ToString());
            List<string> keys = jHeader.Properties().Select(p => p.Name).ToList();
            foreach (string k in keys)
            {
                request.Headers.Add(k, jHeader.SelectToken(k).ToString());
            }
            string postData = string.Empty;

            if (request.Method.Equals("POST"))
            {
                postData = jo.SelectToken("HTTPBODY").ToString();
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Dispose();
            }
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            var result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();
            return true;
        }

    }
}
