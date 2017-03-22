using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using SWSoft.Framework;
using System.Xml.Serialization;
using System.IO;
using HKC.Schemas;
using log4net;

namespace HKC.Biz
{
    
    public class Unity
    {
        /// <summary>
        ///  Md5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMD5(string value)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string secStr = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(value)));
            secStr = secStr.Replace("-", "");
            return secStr;
        }

        /// <summary>
        /// xml反序列化工具方法：可以将xml文本反序列化成一个指定的类对象，并返回。
        /// 注：类对象需要和xml中的字段匹配，否则将造成数据丢失
        /// </summary>
        /// <typeparam name="T">将要生成的类对象类型</typeparam>
        /// <param name="xmlString">需要被反序列化的xml文本</param>
        /// <returns>返回数据对象</returns>
        public static T DeXMLSerialize<T>(string xmlString)
            where T : BASECLASS, new()
        {
            T cloneObject = new T();

            //反序列化语句
            StringBuilder buffer = new StringBuilder();
            buffer.Append(xmlString);

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (TextReader reader = new StringReader(buffer.ToString()))
            {
                Object obj = serializer.Deserialize(reader);
                cloneObject = (T)obj;
            }

            return cloneObject;
        }

        /// <summary>
        /// 类 转 xml序列化工具方法：可以将一个类对象直接序列化成xml文本并返回
        /// </summary>
        /// <typeparam name="T">指定类类型</typeparam>
        /// <param name="obj">将要被序列化的类实体</param>
        /// <returns>类被xml序列化后的xml文本</returns>
        public static string XMLSerialize<T>(T obj)
        {

            StringBuilder buffer = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StringWriter(buffer))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(writer, obj, ns);
            }

            return buffer.ToString();

        }

        /// <summary>
        /// 调用省平台服务
        /// </summary>
        /// <typeparam name="IN"></typeparam>
        /// <typeparam name="OUT"></typeparam>
        /// <param name="InObejct"></param>
        /// <param name="OutObject"></param>
        /// <returns></returns>
        public static OUT runService<IN, OUT>(IN InObejct, string messageId = "")
        where OUT : BASEOUT, new()
        {
            ILog log = log4net.LogManager.GetLogger("TxLog");
            string InMSG = XMLSerialize<IN>(InObejct);
            InMSG = InMSG.Replace("<" + InObejct.GetType().Name + ">", "<code>").Replace("</" + InObejct.GetType().Name + ">", "</code>");
            log.InfoFormat("[{2}][{0}].IN  {1}", InObejct.GetType().Name, LogUnity.I.ShowXml(InMSG), "");
            HKCSERVICE.HkcInterfaceClient ss = new HKCSERVICE.HkcInterfaceClient();
            var OutMsg = ss.sendToHkc(InMSG);
            OUT o = new OUT();
            OutMsg = OutMsg.Replace("<Response>", "<" + o.GetType().Name + ">").Replace("</Response>", "</" + o.GetType().Name + ">");
            log.InfoFormat("[{2}][{0}].IN  {1}", o.GetType().Name, LogUnity.I.ShowXml(OutMsg), "");
            return DeXMLSerialize<OUT>(OutMsg);
        }
    }
}
