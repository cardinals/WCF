using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace JYCS.Schemas
{
    public class MessageParse
    {
        static FileInfo xmlovveride = null;
        static Dictionary<string, List<OverrideNode>> dict;
        static MessageParse()
        {
            dict = new Dictionary<string, List<OverrideNode>>();
        }
        public static string GetXml(object xmlobj, bool isoverride = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add("", "");
                XmlSerializer xs;
                if (isoverride)
                {
                    XmlAttributeOverrides overrides = GetOvverides(xmlobj.GetType());
                    xs = overrides == null ? new XmlSerializer(xmlobj.GetType()) : new XmlSerializer(xmlobj.GetType(), overrides);
                }
                else
                {
                    xs = new XmlSerializer(xmlobj.GetType());
                }
                var xst = new XmlWriterSettings();
                xst.Encoding = Encoding.UTF8;
                var xmlw = XmlWriter.Create(ms, xst);
                xs.Serialize(xmlw, xmlobj, xmlSerializerNamespaces);

                ms.Flush();
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    var outxml = sr.ReadToEnd();
                    ///////
                    //加入转译字符
                    //////
                    var str = outxml;
                    var replace_str_start = "&lt;![CDATA[";
                    var replace_str_end = "]]&gt;";
                    var rep_i = str.IndexOf(replace_str_start);


                    if (rep_i >= 0)
                    {
                        var rep_e = str.LastIndexOf(replace_str_end);
                        var str1 = str.Substring(0, rep_i);
                        var str2 = str.Substring(rep_i, rep_e - rep_i + 10);
                        var str3 = str.Substring(rep_e + 10);
                        str2 = str2.Replace("&lt;", "<");
                        str2 = str2.Replace("&gt;", ">");
                        str2 = str2.Replace("\r\n", "");
                        str = str1 + str2 + str3;
                    }
                    replace_str_start = "&amp;";
                  
                    rep_i = str.IndexOf(replace_str_start);
                    if (rep_i >= 0)
                    {

                        str = str.Replace("&amp;", "&");
                    }
                    var replace_str_start2 = "&lt;";
                    rep_i = str.IndexOf(replace_str_start2);
                    if (rep_i >= 0)
                    {

                        str = str.Replace("&lt;", "<");
                    }
                    var replace_str_start3 = "&gt;";
                    rep_i = str.IndexOf(replace_str_start3);
                    if (rep_i >= 0)
                    {

                        str = str.Replace("&gt;", ">");
                    }

                    outxml = str;
                    return outxml;
                }
            }
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

    public class OverrideNode
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string OverName { get; set; }
    }
}
