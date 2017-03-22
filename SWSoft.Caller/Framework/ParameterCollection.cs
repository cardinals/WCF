using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace SWSoft.Framework
{
    /// <summary>
    /// 标识存储过程的参数集合
    /// </summary>
    public class ParameterCollection : IEnumerable<Parameter>
    {
        public ParameterCollection()
        {
            Items = new List<Parameter>();
        }
        /// <summary>
        /// 获取或设置参数集合
        /// </summary>
        public List<Parameter> Items { get; set; }
        /// <summary>
        /// 设置存储过程的参数
        /// </summary>
        /// <param name="name">参数名称</param>
        public object this[string name] { set { Items.Add(new Parameter { Name = name, Value = value, Direction = ParameterDirection.Input }); } }

        public IEnumerator<Parameter> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }

    /// <summary>
    /// 标识存储过程的参数
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数的值
        /// </summary>
        public object Value { get; set; }
        public ParameterDirection Direction { get; set; }
    }
}
