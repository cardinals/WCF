using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Web.Hosting;

namespace SWSoft.Framework
{
    public class LogUnity
    {
        static LogUnity i;
        public static LogUnity I
        {
            get
            {
                i = i ?? new LogUnity();
                return i;
                ;
            }
            set { i = value; }
        }
        public string InSql { get; set; }
        public string Connect { get; set; }
        public string Enable { get; set; }
        public string runlevel { get; set; }
        public DateTime lasttime = DateTime.Now;
        public LogUnity()
        {
            var path = HostingEnvironment.ApplicationPhysicalPath ?? Application.StartupPath;
            var xml = new XmlDocument();
            xml.Load(path + "\\" + "log4db.xml");
            foreach (var item in this.GetType().GetProperties())
            {
                if (item.Name != "I")
                {
                    var value = xml.LastChild[item.Name.ToLower()].InnerText;
                    Convert.ChangeType(value, item.PropertyType);
                    item.SetValue(this, value, null);
                }
            }
        }

        public bool Insert(params object[] args)
        {
            if (Enable == "0")
            {
                return true;
            }
            try
            {
                if ((DateTime.Now - lasttime).TotalSeconds > 60)
                {
                    lasttime = DateTime.Now;
                    I = new LogUnity();
                }
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = ShowXml(args[i]);
                }
                if (runlevel == "1" || runlevel == "2")
                {
                    DBVisitor.ExecuteNonQuery(string.Format(InSql, args), null, Connect);
                }
                return false;
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// XML格式化为文本显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public object ShowXml(object str)
        {
            try
            {
                if (str != null && str.ToString().StartsWith("<?xml"))
                {
                    var xml = new XmlDocument();
                    xml.LoadXml(str.ToString());
                    return xml.InnerXml;
                }
            }
            catch (Exception)
            {
            }
            return str;
        }

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            foreach (XmlNode item in section.ChildNodes)
            {
                typeof(LogUnity).GetType().GetProperty(item.Name).SetValue(this, item.Value, null);
            }
            return section;
        }

        public void Info(params object[] args)
        {
            Insert(args);
        }

        public void Error(params object[] args)
        {
            Insert(args);
        }
    }
}
