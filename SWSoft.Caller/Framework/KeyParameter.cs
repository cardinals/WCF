using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SWSoft.Caller.Framework
{

    public class KeyParameter
    {
        public DataTable TableView
        {
            get
            {
                var table = new DataTable("paramview");
                table.Columns.Add("Name");
                table.Columns.Add("Value");
                foreach (var item in Items)
                {
                    var newrow = table.NewRow();
                    newrow["Name"] = item.Key;
                    newrow["Value"] = item.Value;
                    table.Rows.Add(newrow);
                }
                return table;
            }
        }
        /// <summary>
        /// 通过键获取值，自动转换为大写
        /// </summary>
        /// <param name="key">键的名称</param>
        /// <returns></returns>
        public string this[string key]
        {
            get { key = key.ToUpper().Trim(); return Items.ContainsKey(key.ToUpper()) ? Items[key] : string.Empty; }
            set
            {
                key = key.ToUpper();
                if (Items.ContainsKey(key))
                {
                    Items[key] = value;
                }
                else
                {
                    Items.Add(key, value);
                }
            }
        }
        public string Value { get; set; }
        public Dictionary<string, string> Items { get; set; }
        /// <summary>
        /// 键值对参数
        /// </summary>
        /// <param name="value">包含键值对的字符串</param>
        /// <param name="separator">分隔符</param>
        public KeyParameter(string value, char separator = '|')
        {
            Items = new Dictionary<string, string>();
            if (value == null)
            {
                return;
            }
            Value = value;
            foreach (var item in value.Split(separator))
            {
                var kv = item.Split('=');
                if (kv.Length > 1)
                {
                    this[kv[0]] = kv[1];
                }
            }
        }
    }
}
