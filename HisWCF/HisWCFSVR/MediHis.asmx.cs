using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using log4net;
using SWSoft.Framework;
using JYCS.Schemas;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Configuration;
using System.Xml;
using System.Reflection;
namespace HisWCFSVR
{
    /// <summary>
    /// MediHis 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class MediHis : System.Web.Services.WebService
    {


        [WebMethod]
        public int RunService(string TradeType, string TradeMsg, string YYDM, ref string TradeMsgOut)
        {
            ILog log = log4net.LogManager.GetLogger("XmlLog");
            string guid = Guid.NewGuid().ToString("N").ToUpper();
            log.InfoFormat("[{2}][{0}].IN  {1}", TradeType, LogUnity.I.ShowXml(TradeMsg), guid);     
            //var asm = Assembly.LoadFile(@"F:\1.服务平台\3.HIS4\服务平台\HisWCF\HisWCFSVR\bin\HIS4.Schemas.dll");      
            //Type tp = asm.GetType("HIS4.Schemas.ZHIFUBAOTF_IN");
            //dynamic obj = Activator.CreateInstance(tp);
            
 

            var listpbxx = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00002));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(HIS4.Schemas.ZHIFUBAOTF_IN));
            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(TradeMsg));
            HIS4.Schemas.ZHIFUBAOTF_IN p = xmlSerializer.Deserialize(stream) as HIS4.Schemas.ZHIFUBAOTF_IN;

           



         

            YuHangYY.HisApplayClient yhyy = new YuHangYY.HisApplayClient();
            int i = yhyy.RunService(TradeType, TradeMsg, ref   TradeMsgOut);
            log.InfoFormat("[{2}][{0}].OUT {1}", TradeType, TradeMsgOut, guid);
            return i;
        }

        public static T ToXmlObject<T>(string xml, bool isoverride = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(xml);
                ms.Write(buffer, 0, buffer.Length);
                ms.Flush();
                ms.Position = 0;
                XmlSerializer xs;
                if (isoverride)
                {
                    XmlAttributeOverrides overrides = GetOvverides(typeof(T));
                    xs = overrides == null ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), overrides);
                }
                else
                {
                    xs = new XmlSerializer(typeof(T));
                }
                return (T)xs.Deserialize(ms);
            }
        }
        static FileInfo xmlovveride = null;
        static Dictionary<string, List<OverrideNode>> dict;
        public static XmlAttributeOverrides GetOvverides(Type type, bool isSerialize = true)
        {
            var path = ConfigurationManager.AppSettings["xmlovveride"];
            if (path == null)
            {
                return null;
            }
            var fileinfo = new FileInfo(path);

            if (xmlovveride != null)
            {
                if (fileinfo.LastWriteTime > xmlovveride.LastWriteTime)
                {
                    dict.Clear();
                    goto loadfile;
                }
                else
                {
                    goto getconfig;
                }
            }

        loadfile:
            {

                var xmldom = new XmlDocument();
                xmldom.LoadXml(File.ReadAllText(fileinfo.FullName, Encoding.UTF8));
                var nodes = xmldom.SelectNodes("/configuration/item");
                if (nodes != null)
                {
                    foreach (XmlNode item in nodes)
                    {
                        var list = new List<OverrideNode>();
                        foreach (XmlNode prop in item.ChildNodes)
                        {
                            var node = new OverrideNode();
                            node.Name = prop.Attributes["name"].Value.ToUpper();
                            node.OverName = prop.Attributes["overname"].Value.ToUpper();
                            if (prop.Attributes["type"] != null)
                            {
                                node.Type = prop.Attributes["type"].Value;
                            }
                            list.Add(node);
                        }
                        if (!dict.ContainsKey(item.Attributes["class"].Value))
                        {
                            dict.Add(item.Attributes["class"].Value, list);
                        }
                    }
                    xmlovveride = fileinfo;
                }
            }

        getconfig:
            {
                if (!dict.ContainsKey(type.FullName))
                {
                    return null;
                }
                var xabo = new XmlAttributeOverrides();
                var onl = dict[type.FullName];
                foreach (var item in onl)
                {
                    XmlAttributes attrs = new XmlAttributes();
                    attrs.XmlElements.Add(new XmlElementAttribute(isSerialize ? item.OverName : item.Name));
                    if (string.IsNullOrEmpty(item.Type))
                    {
                        xabo.Add(type, isSerialize ? item.Name : item.OverName, attrs);
                    }
                    else
                    {
                        var ptype = type.Assembly.GetType(item.Type, false, true);
                        if (ptype != null)
                        {
                            xabo.Add(ptype, isSerialize ? item.Name : item.OverName, attrs);
                        }
                        else
                        {
                            ptype = Assembly.GetAssembly(typeof(BASEINFO)).GetType(item.Type, false, true);
                            if (ptype != null)
                            {
                                xabo.Add(ptype, isSerialize ? item.Name : item.OverName, attrs);
                            }
                        }
                    }
                }
                return xabo;
            }
        }


    }
}
