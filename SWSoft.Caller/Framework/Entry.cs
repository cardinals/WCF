using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace SWSoft.Framework
{
    [Serializable]
    /// <summary>
    /// 为表示实体类特有对象提供的基类
    /// </summary>
    public class Entry : IXmlSerializable
    {
        /// <summary>
        /// 获取临时字段关联的值
        /// </summary>
        /// <param name="columnName">要获取字段的列名</param>
        public object this[string columnName]
        {
            get { return Items.ContainsKey(columnName.ToUpper()) ? Items[columnName.ToUpper()] : null; }
            set
            {
                columnName = columnName.ToUpper();
                if (Items.ContainsKey(columnName))
                {
                    Items[columnName] = value;
                }
                else
                {
                    Items.Add(columnName, value);
                }
            }
        }

        /// <summary>
        /// 获取临时字段关联的值
        /// </summary>
        /// <param name="index">索引位置</param>
        public object this[int index]
        {
            get { return Items.Values.GetEnumerator().Current; }
        }

        /// <summary>
        /// 包含临时属性的集合
        /// </summary>
        public Dictionary<string, object> Items;

        public Entry()
        {
            Items = new Dictionary<string, object>();
        }

        public string Get(string columnName)
        {
            return (this[columnName] ?? "").ToString();
        }

        public string ToString(string format = "")
        {
            switch (format)
            {
                case "json": return Json.ToString(this);
                default: return base.ToString();
            }
        }

        public XmlSchema GetSchema()
        {
            var schema = new XmlSchema();
            schema.Id = GetType().Name;
            return schema;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {

        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var type = GetType();
            writer.WriteStartElement(type.Name);
            foreach (var item in GetType().GetProperties())
            {
                if (item.Name != "Item")
                {
                    writer.WriteStartElement(item.Name);
                    writer.WriteValue(item.GetValue(this, null));
                    writer.WriteEndElement();
                }
            }
            if (Items.Count > 0)
            {
                foreach (var item in Items)
                {
                    writer.WriteStartElement(item.Key);
                    writer.WriteValue(item.Value == DBNull.Value ? "" : item.Value);
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// 获得对象的值拼接后的Sql Values
        /// </summary>
        /// <returns></returns>
        public string GetValues()
        {
            var cmd = string.Empty;
            foreach (var item in GetType().GetProperties())
            {
                if (item.Name == "Item")
                {
                    continue;
                }
                var value = item.GetValue(this, null);
                if (value != null)
                {
                    if (item.PropertyType == typeof(DateTime?))
                    {
                        cmd += "TO_DATE(" + value + "),";
                    }
                    else
                    {
                        cmd += "'" + value + "',";
                    }
                }
                else
                {
                    cmd += "null,";
                }
            }
            return cmd.TrimEnd(',');
        }
    }
}
