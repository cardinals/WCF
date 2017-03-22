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
    public class YAOXIEHQSPXX : IMessage<YAOXIEHQSPXX_IN, YAOXIEHQSPXX_OUT>
    {
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
            OutObject = new YAOXIEHQSPXX_OUT();


            var ypptdz = System.Configuration.ConfigurationManager.AppSettings["YXPTDZ"];
            ypptdz = ypptdz + "/hospitalInterface/drug/procurecatalog/getProcurecatalog";

            string inJason = "{\"list\":[";

            //采购订单明细
            var i = 0;
            foreach (var item in InObject.SHANGPINMX)
            {
                inJason = inJason + "{" +
                                "\"procurecatalogId\":\"" + item.SHANGPINBH + "\"" +
                                 "}";

                inJason = inJason + ",";
                i++;
            }
            if (i > 0)
            {
                inJason = inJason.Substring(0, inJason.LastIndexOf(","));
            };
            inJason = inJason + "]}";


            System.Net.WebClient webClientObj = new System.Net.WebClient();
            //1、获取令牌
            System.Collections.Specialized.NameValueCollection postHosGetProcurecatalog = new System.Collections.Specialized.NameValueCollection();
            postHosGetProcurecatalog.Add("accessToken", InObject.JIEKOUDYPJ);
            postHosGetProcurecatalog.Add("procurecatalogIds", inJason);
            postHosGetProcurecatalog.Add("month", InObject.YUEFEN);
            postHosGetProcurecatalog.Add("currentPageNumber", InObject.DANGQIANYM);
           



            try
            {
                byte[] byRemoteInfo = webClientObj.UploadValues(ypptdz, "POST", postHosGetProcurecatalog);
                string strGetProcurecatalog = System.Text.Encoding.UTF8.GetString(byRemoteInfo);
                strGetProcurecatalog = strGetProcurecatalog.Replace(":null", ":0");
                strGetProcurecatalog = strGetProcurecatalog.Replace("&lt;", "");
                strGetProcurecatalog = strGetProcurecatalog.Replace("&gt;", "");
                strGetProcurecatalog = strGetProcurecatalog.Replace("&;", "");
                strGetProcurecatalog = strGetProcurecatalog.Replace("&", "");
                //strGetProcurecatalog = "{\"dataList\":[{\"goodsId\":4.00,\"materialName\":\"玻璃瓶\"}],\"totalRecordCount\":15275,\"returnCode\":1,\"totalPageCount\":1528,\"returnMsg\":\"业务执行成功\",\"currentPageNumber\":\"1\"}";

                var xmldom = new XmlDocument();
                var Xml = Json2Xml(strGetProcurecatalog);
                string xmlstr = Xml.InnerXml;
                string newxmlstr = xmlstr.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>", "");
                newxmlstr = newxmlstr.ToUpper();
                xmldom.LoadXml(newxmlstr);


                var returncode = xmldom.GetElementsByTagName("RETURNCODE").Item(0).FirstChild.InnerText;

                if (returncode == "1")
                {
                    OutObject.DANGQIANYM = xmldom.GetElementsByTagName("CURRENTPAGENUMBER").Item(0).FirstChild.InnerText;
                    OutObject.ZONGYEM = xmldom.GetElementsByTagName("TOTALPAGECOUNT").Item(0).FirstChild.InnerText;
                    OutObject.ZONGHANGS = xmldom.GetElementsByTagName("TOTALRECORDCOUNT").Item(0).FirstChild.InnerText;
                   // OutObject.SHANGPINFHMX = xmldom.GetElementsByTagName("DATALIST").Item(0).InnerXml;
                    OutObject.SHANGPINFHMX = "<![CDATA[" + xmldom.GetElementsByTagName("DATALIST").Item(0).InnerXml+ "]]>";
                    return;
                }
                else
                {
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
