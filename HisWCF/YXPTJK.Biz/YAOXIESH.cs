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
    public class YAOXIESH : IMessage<YAOXIESH_IN, YAOXIESH_OUT>
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
            OutObject = new YAOXIESH_OUT();


            var ypptdz = System.Configuration.ConfigurationManager.AppSettings["YXPTDZ"];
            ypptdz = ypptdz + "/hospitalInterface/drug/wareHouse/warehouse";

            string inJason = "{\"list\":[";

            //采购订单明细
            var i = 0;
            foreach (var item in InObject.CAIGOUDDMX)
            {
                inJason = inJason + "{" +
                                "\"distributeId\":\"" + item.PEISONGMXBH + "\"," +  //配送明细编号”,  //必填
                                "\"warehouseCount\":\""+ item.SHOUHUOSL + "\"," + //收货数量”,  //必填; 0表示拒绝收货
                                "\"warehouseCustomInfo\":\"" + item.ZIDINGYSHXX + "\"" + //自定义收货信息”   //可填
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
            System.Collections.Specialized.NameValueCollection postHosWareHouse = new System.Collections.Specialized.NameValueCollection();
            postHosWareHouse.Add("accessToken", InObject.JIEKOUDYPJ);
            postHosWareHouse.Add("distributeInfo", inJason);



            try
            {
                byte[] byRemoteInfo = webClientObj.UploadValues(ypptdz, "POST", postHosWareHouse);
                string strWareHouse = System.Text.Encoding.UTF8.GetString(byRemoteInfo);

                strWareHouse = strWareHouse.Replace(":null", ":0");
                strWareHouse = strWareHouse.Replace("&lt;", "");
                strWareHouse = strWareHouse.Replace("&gt;", "");
                strWareHouse = strWareHouse.Replace("&;", "");


                var xmldom = new XmlDocument();
                var Xml = Json2Xml(strWareHouse);
                string xmlstr = Xml.InnerXml;
                string newxmlstr = xmlstr.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>", "");
                newxmlstr = newxmlstr.ToUpper();
                xmldom.LoadXml(newxmlstr);


                var returncode = xmldom.GetElementsByTagName("RETURNCODE").Item(0).FirstChild.InnerText;

                if (returncode == "1")
                {
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
