using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Web.Hosting;
using System.Data.Common;
using SWSoft.Caller.Framework;
using System.Data;

namespace SWSoft.Framework
{
    public class SqlLoad
    {
        static Dictionary<string, string> Items { get; set; }
        static Dictionary<string, ProcedureItem> ProcItems { get; set; }
        static SqlLoad()
        {
            var workpath = HostingEnvironment.MapPath("~");//WCF中
            if (workpath == null)
            {
                workpath = Environment.CurrentDirectory;
            }
            Items = new Dictionary<string, string>();
            ProcItems = new Dictionary<string, ProcedureItem>();
            string path = workpath + "\\sqllib.ini";
            foreach (var item in File.ReadAllLines(path))
            {
                var arr = item.Split('=');
                if (arr.Length >= 2)
                {
                    Load(workpath + arr[1]);
                }
            }
        }

        /// <summary>
        /// 加载一个SQL脚本文件对象
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        static bool Load(string path)
        {
            var xml = new XmlDocument();
            xml.Load(path);
            foreach (XmlNode item in xml.LastChild.ChildNodes)
            {
                if (item.Name.ToUpper() == "SQI")
                {
                    var id = item.Attributes["id"].Value;
                    Debug.WriteLine("SWSoft.Framework.SqlLoad -> [Sql][" + id + "]");
                    if (Items.ContainsKey(id))
                    {
                        //throw new Exception("已存在相同名称的脚本:" + id);
                    }
                    else
                    {
                        Items.Add(item.Attributes["id"].Value, item.InnerText);
                    }
                }
                else if (item.Name.ToUpper() == "PROC")
                {
                    var id = item.Attributes["id"].Value;
                    if (ProcItems.ContainsKey(id))
                    {
                        //throw new Exception("已存在相同名称的存储过程配置:" + id);
                    }
                    else
                    {
                        Debug.WriteLine("[Proc][" + id + "]");
                        var pitem = new ProcedureItem();
                        pitem.ProcedureName = item["name"].InnerText.ToUpper();
                        foreach (XmlNode paramnode in item.ChildNodes)
                        {
                            if (paramnode.Name.ToUpper() == "PARAM")
                            {
                                var p = new ProcedureParameter();
                                p.ParameterName = paramnode.Attributes["name"].Value.ToUpper();
                                p.DataType = (DbType)Enum.Parse(typeof(DbType), paramnode.Attributes["type"].Value, true);
                                p.Direction = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), paramnode.Attributes["Direction"].Value, true);
                                pitem.AddParameter(p);
                            }
                        }
                        ProcItems.Add(id, pitem);
                    }
                }
            }
            return true;
        }

        public static ProcedureItem GetProcedure(string key)
        {
            if (ProcItems.ContainsKey(key))
            {
                return ProcItems[key];
            }
            else
            {
                Debug.WriteLine("找不到存储过程配置:" + key);
                return null;
            }
        }

        /// <summary>
        /// 获得格式化的SQL语句
        /// </summary>
        /// <param name="key">SQL语句的ID名称,XML配置中</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        /// <returns></returns>
        public static string GetFormat(string key, params object[] args)
        {
            if (Items.ContainsKey(key))
            {
                return args.Length == 0 ? Items[key] : string.Format(Items[key], args);
            }
            else
            {
                throw new Exception("找不到SQL脚本对象:" + key);
            }
        }
    }
}
