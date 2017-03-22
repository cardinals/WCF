using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SWSoft.Framework
{
    /// <summary>
    /// 提供Json的相关处理方法
    /// </summary>
    public class Json
    {
        /// <summary>
        /// 返回对象的 JSON 数据交换格式
        /// </summary>
        public static string ToString(Entry entry = null, IList list = null)
        {
            StringBuilder json = new StringBuilder();
            if (entry != null)
            {
                foreach (var key in entry.Items.Keys)
                {
                    json.AppendFormat("\"{0}\":{1},", key, Parse(entry.Items[key]));
                }
                return string.Format("{{{0}}}", json.ToString().TrimEnd(','));
            }
            else if (list != null)
            {
                foreach (Entry item in list)
                {
                    var str = string.Empty;
                    foreach (var key in item.Items.Keys)
                    {
                        str += string.Format("\"{0}\":{1},", key, Parse(item.Items[key]));
                    }
                    json.AppendLine(string.Format("{{{0}}},", str.TrimEnd(',')));
                }
                return string.Format("[\r\n{0}\r\n]", json.ToString().TrimEnd(',', '\r', '\n'));
            }
            else
            {
                return "{}";
            }
        }

        private static string Parse(object property)
        {
            if (property is DateTime)
            {
                property = Convert.ToDateTime(property).ToString("yyyy-MM-dd HH:mm:ss");
            }
            var quotation = false;
            switch (Type.GetTypeCode(property.GetType()))
            {
                case TypeCode.Boolean:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Empty:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Object:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64: quotation = false; break;
                default: quotation = true; break;
            }
            return quotation ? string.Format("\"{0}\"", property) : property.ToString();
        }
    }
}
