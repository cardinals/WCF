using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;


namespace YXPTJK.Biz
{
    public class YAOXIEHQJKTYPJ : IMessage<YAOXIEHQJKTYPJ_IN, YAOXIEHQJKTYPJ_OUT>
    {

        ///// <summary>
        ///// 对象转Json
        ///// </summary>
        ///// <typeparam name="T"><peparam>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static string Serialize(object obj)
        //{
        //    return JsonConvert.SerializeObject(obj);
        //}

        ///// <summary>
        ///// Json转对象
        ///// </summary>
        ///// <typeparam name="T"><peparam>
        ///// <param name="json"></param>
        ///// <returns></returns>
        //public static T Deserialize<T>(string json)
        //{
        //    return JsonConvert.DeserializeObject<T>(json);
        //}

        ///// <summary>
        ///// 反序列化
        ///// </summary>
        ///// <param name="type">类型</param>
        ///// <param name="xml">XML字符串</param>
        ///// <returns></returns>
        //public static object Deserialize(Type type, string xml)
        //{
        //    try
        //    {
        //        using (StringReader sr = new StringReader(xml))
        //        {
        //            XmlSerializer xmldes = new XmlSerializer(type);
        //            return xmldes.Deserialize(sr);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        ///// <summary>
        ///// 反序列化
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="xml"></param>
        ///// <returns></returns>
        //public static object Deserialize(Type type, Stream stream)
        //{
        //    XmlSerializer xmldes = new XmlSerializer(type);
        //    return xmldes.Deserialize(stream);
        //}
        ///// <summary>
        ///// 序列化
        ///// </summary>
        ///// <param name="type">类型</param>
        ///// <param name="obj">对象</param>
        ///// <returns></returns>
        //public static string Serializer(object obj)
        //{
        //    MemoryStream Stream = new MemoryStream();
        //    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        //    ns.Add("", "");
        //    XmlSerializer xml = new XmlSerializer(obj.GetType());
        //    try
        //    {
        //        //序列化对象
        //        xml.Serialize(Stream, obj, ns);
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        throw;
        //    }
        //    Stream.Position = 0;
        //    StreamReader sr = new StreamReader(Stream);
        //    string str = sr.ReadToEnd();
        //    sr.Dispose();
        //    Stream.Dispose();
        //    return str;
        //}


        //public class TokenInfo
        //{
        //    public string returnCode { get; set; }
        //}

        /// <summary>
        /// json字符串转换为Xml对象
        /// </summary>
        /// <param name="sJson"></param>
        /// <returns></returns>
        public static XmlDocument Json2Xml(string sJson)
        {
            //XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(sJson), XmlDictionaryReaderQuotas.Max);
            //XmlDocument doc = new XmlDocument();
            //doc.Load(reader);

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            Dictionary<string, object> Dic = (Dictionary<string, object>)oSerializer.DeserializeObject(sJson);
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDec;
            //xmlDec = doc.CreateXmlDeclaration("1.0", "gb2312", "yes");

            xmlDec = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");

            doc.InsertBefore(xmlDec, doc.DocumentElement);
            XmlElement nRoot = doc.CreateElement("root");
            doc.AppendChild(nRoot);
            foreach (KeyValuePair<string, object> item in Dic)
            {
                XmlElement element = doc.CreateElement(item.Key);
                KeyValue2Xml(element, item);
                nRoot.AppendChild(element);
            }
            return doc;
        }

        private static void KeyValue2Xml(XmlElement node, KeyValuePair<string, object> Source)
        {
            object kValue = Source.Value;
            if (kValue.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (KeyValuePair<string, object> item in kValue as Dictionary<string, object>)
                {
                    XmlElement element = node.OwnerDocument.CreateElement(item.Key);
                    KeyValue2Xml(element, item);
                    node.AppendChild(element);
                }
            }
            else if (kValue.GetType() == typeof(object[]))
            {
                object[] o = kValue as object[];
                for (int i = 0; i < o.Length; i++)
                {
                    XmlElement xitem = node.OwnerDocument.CreateElement("Item");
                    KeyValuePair<string, object> item = new KeyValuePair<string, object>("Item", o[i]);
                    KeyValue2Xml(xitem, item);
                    node.AppendChild(xitem);
                }

            }
            else
            {
                XmlText text = node.OwnerDocument.CreateTextNode(kValue.ToString());
                node.AppendChild(text);
            }
        }


        public override void ProcessMessage()
        {

            OutObject = new YAOXIEHQJKTYPJ_OUT();
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            //InObject.BUMENBM = "H0100010xyk";
            string cl = "departmentUserName="+InObject.BUMENBM + "&secret="+ InObject.BUMENMM +"&logtime="+ InObject.BASEINFO.CAOZUORQ;

            string pwd ="";
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 

                pwd = pwd + s[i].ToString("x2");

            }


            InObject.BUMENMM = cl + "&md5=" + pwd;
            var ypptdz = System.Configuration.ConfigurationManager.AppSettings["YXPTDZ"];
            ypptdz = ypptdz + "/hospitalInterface/accessToken/getToken";

            System.Net.WebClient webClientObj = new System.Net.WebClient();
            //1、获取令牌
            System.Collections.Specialized.NameValueCollection postHosGetToken = new System.Collections.Specialized.NameValueCollection();
            postHosGetToken.Add("departmentUserName", InObject.BUMENBM);
            postHosGetToken.Add("params", InObject.BUMENMM);
            try
            {
                byte[] byRemoteInfo = webClientObj.UploadValues(ypptdz, "POST", postHosGetToken);
                string strTokenInfo = System.Text.Encoding.UTF8.GetString(byRemoteInfo);

                var xmldom = new XmlDocument();
                var Xml = Json2Xml(strTokenInfo);
                string xmlstr = Xml.InnerXml;
                string newxmlstr = xmlstr.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>", "");
                newxmlstr = newxmlstr.ToUpper();
                xmldom.LoadXml(newxmlstr);

                var returncode = xmldom.GetElementsByTagName("RETURNCODE").Item(0).FirstChild.InnerText;

                if (returncode == "1")
                {
                    OutObject.JIEKOUDYPJ = xmldom.GetElementsByTagName("ACCESSTOKEN").Item(0).FirstChild.InnerText;
                    OutObject.PINGZHENGYXQ = xmldom.GetElementsByTagName("EXPIRESIN").Item(0).FirstChild.InnerText;
                    OutObject.DANGQIANSJ = xmldom.GetElementsByTagName("CURRENTTIME").Item(0).FirstChild.InnerText;
                    return;
                }
                else if (returncode == "2")
                {
                    OutObject.OUTMSG.ERRMSG = "-1";
                    OutObject.OUTMSG.ERRMSG = xmldom.GetElementsByTagName("RETURNMSG").Item(0).FirstChild.InnerText;
                }
                else
                {
                    OutObject.JIEKOUDYPJ = xmldom.GetElementsByTagName("ACCESSTOKEN").Item(0).FirstChild.InnerText;
                    if (OutObject.JIEKOUDYPJ != "")
                    {
                        return;
                    };
                    var returnmsg = xmldom.GetElementsByTagName("RETURNMSG").Item(0).FirstChild.InnerText;
                    OutObject.OUTMSG.ERRNO = "-1";
                    OutObject.OUTMSG.ERRMSG = returnmsg;
                    return;
                }

            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                OutObject.OUTMSG.ERRNO = "-1";
                OutObject.OUTMSG.ERRMSG = errorMessage;
                return;
            }
        }
    }
}
