using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SWSoft.Framework
{
    public class ProcedureItem
    {
        /// <summary>
        /// 表示存储过程的完整名称
        /// </summary>
        public string ProcedureName { get; set; }
        /// <summary>
        /// 表示存储过程参数的集合
        /// </summary>
        public Dictionary<string, ProcedureParameter> Parameters { get; set; }
        /// <summary>
        /// 表示存储过程参数名称的集合，按顺序排列
        /// </summary>
        public List<string> ParametersKeys { get; set; }

        public ProcedureItem()
        {
            Parameters = new Dictionary<string, ProcedureParameter>();
            ParametersKeys = new List<string>();
        }

        public object this[string name]
        {
            get
            {
                name = name.ToUpper();
                if (Parameters.ContainsKey(name))
                {
                    return Parameters[name].Value;
                }
                return string.Empty;
            }
            set
            {
                name = name.ToUpper();
                if (Parameters.ContainsKey(name))
                {
                    Parameters[name].Value = value;
                }
            }
        }

        public void AddParameter(ProcedureParameter p)
        {
            if (!Parameters.ContainsKey(p.ParameterName))
            {
                Parameters.Add(p.ParameterName, p);
                ParametersKeys.Add(p.ParameterName);
            }
        }
    }

    public class ProcedureParameter
    {
        public string ParameterName { get; set; }
        public DbType DataType { get; set; }
        public ParameterDirection Direction { get; set; }
        public object Value { get; set; }
    }
}
