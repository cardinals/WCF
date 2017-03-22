using System;
using System.Collections.Generic;
using System.Text;

namespace SWSoft.Reflector
{
    public enum SqlVersion
    {
        SQL7 = 7, SQL2000 = 8, SQL2005 = 9, SQL2008 = 10
    }

    [Serializable]
    public class Options
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        public SqlVersion SqlVersion { get; set; }
    }
}
