using System;
using System.Text;
using System.Diagnostics;
using System.Data.OracleClient;
using System.Data;
using System.Configuration;
using SWSoft.Framework;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace HIS4.Biz
{
    /// <summary>
    /// 工具类方法
    /// </summary>
    class ToolMethod
    {
        /// <summary>
        /// 获取配置文件参数
        /// </summary>
        /// <param name="inVar">参数名称</param>
        /// <returns></returns>
        public static string ConfigValue(string inVar)
        {
            string outVar = "";
            try
            {
                outVar = System.Configuration.ConfigurationManager.AppSettings[inVar];
                if (string.IsNullOrEmpty(outVar))
                {
                    outVar = "";
                }
                return outVar;
            }
            catch
            {
                return outVar;
            }
        }
        /// <summary>
        /// 获取配置文件参数
        /// </summary>
        /// <param name="inVar">参数名称</param>
        /// <param name="initFlag">初始值</param>
        /// <returns></returns>
        public static string ConfigValue(string inVar, string initFlag)
        {
            string outVar = "";
            try
            {
                outVar = System.Configuration.ConfigurationManager.AppSettings[inVar];
                if (string.IsNullOrEmpty(outVar))
                {
                    outVar = initFlag;
                }
                return outVar;
            }
            catch
            {
                return initFlag;
            }
        }
        /// <summary>
        /// 获取数据库参数配置
        /// </summary>
        /// <param name="sysNum">系统序号</param>
        /// <param name="varName">参数名称</param>
        /// <param name="varNum">参数序号</param>
        /// <param name="initValue">初始值</param>
        /// <returns>返回值</returns>
        public static string DbConfigValue(string sysNum, string varName, string varNum, string initValue)
        {
            string outVar = "";
            string dynamicSql = string.Format("Select Decode({0},1,Csz1,Csz2) Value From Gy_Xtcs Where Csmc = '{1}' And Xtxh = {2}", varNum, varName, sysNum);
            var Result = DBVisitor.ExecuteModel(dynamicSql);
            if (Result != null)
            {
                outVar = Result.Items["VALUE"].ToString();
            }
            else
            {
                outVar = initValue;
            }
            return outVar;
        }
        /// <summary>
        /// 验证特定格式日期
        /// </summary>
        /// <param name="varName">参数名</param>
        /// <param name="inDate">需验证参数值</param>
        /// <param name="dateFormat">格式</param>
        /// <returns>返回传入参数值</returns>
        public static string VerifyDate(string varName, string inDate, string dateFormat)
        {
            if (string.IsNullOrEmpty(inDate))
            {
                throw new Exception(string.Format("传入参数{0}为空", varName));
            }
            else
            {
                try
                {
                    DateTime.ParseExact(inDate, dateFormat, null);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("传入参数{0}格式不符合要求：[{1}]\r\n异常为：{2}", varName, inDate, ex));
                }

            }
            return inDate;
        }
        
        /// <summary>
        /// 解析字符串[双字符]
        /// </summary>
        /// <param name="inStringVar">需解析字符串</param>
        /// <param name="delimiter">分隔符</param>
        /// <returns>string数组</returns>
        public static string[] ParseStrings(string inStringVar, string delimiter)
        {
            string[] retArray = null;
            string[] sep = { string.Format("{0}", delimiter) };
            retArray = inStringVar.Split(sep, StringSplitOptions.None);
            return retArray;
        }

        /// <summary>
        /// Post方式Http接口调用
        /// </summary>
        /// <param name="httpUrl">Url</param>
        /// <param name="httpInVar">入参</param>
        /// <returns>请求结果返回</returns>
        public static string HttpClientRequestCall(string httpUrl, string httpInVar)
        {
            string rtnResult = "";
            try
            {
                // httpUrl = "http://122.224.110.186:8888/MediinfoOA/cms/AddressBook.aspx";
                Encoding e = Encoding.Default;
                byte[] buffer = e.GetBytes(httpInVar);

                HttpWebRequest hwReq = (HttpWebRequest)WebRequest.Create(httpUrl);
                hwReq.Timeout = 3000;
                hwReq.Method = "POST";
                hwReq.Accept = "text/html, application/xhtml+xml, */*";
                hwReq.ContentType = "application/x-www-form-urlencoded";
                hwReq.ContentLength = buffer.Length;
                hwReq.GetRequestStream().Write(buffer, 0, buffer.Length);

                HttpWebResponse hwRep = (HttpWebResponse)hwReq.GetResponse();
                StreamReader sr = new StreamReader(hwRep.GetResponseStream(), Encoding.UTF8);

                StringBuilder sb = new StringBuilder();
                while (sr.Peek() != -1)
                {
                    sb.Append(sr.ReadLine());
                }
                rtnResult = sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("发生异常:\r\n{0}", ex.Message));
            }
            return rtnResult;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  __胡谦
        /// </summary>  
        public static string CreatePostHttpResponse(string url, string strJson, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            byte[] postBytes = Encoding.UTF8.GetBytes(strJson);
            request.ContentType = "application/json;charset=UTF-8";
            request.ContentLength = Encoding.UTF8.GetByteCount(strJson);//strJson为json字符串

            Stream stream = request.GetRequestStream();
            stream.Write(postBytes, 0, postBytes.Length);
            request.Timeout = timeout;
            stream.Close();

            var response = request.GetResponse();
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            String responseString = streamRead.ReadToEnd();
            response.Close();
            streamRead.Close();

            return responseString;
        }
       /// <summary>
       /// 消息推送 返回信息处理
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="jsonString"></param>
       /// <returns></returns>
        public static T parse<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }
    }
}
