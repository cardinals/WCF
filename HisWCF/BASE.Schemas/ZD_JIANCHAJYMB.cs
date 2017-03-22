using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class ZD_JIANCHAJYMB_IN : MessageIn
    {
        /// <summary>
        ///下载类型
        /// </summary>
        public string XIAZAILX { get; set; }

    }
    public class ZD_JIANCHAJYMB_OUT : MessageOUT
    {
        /// <summary>
        /// 检验检查模版列表
        /// </summary>
        public List<MOBANXX> MOBANMX { get; set; }
        public ZD_JIANCHAJYMB_OUT()
        {
            MOBANMX = new List<MOBANXX>();
        }
    }

    public class MOBANXX
    {
        /// <summary>
        ///模版代码
        /// </summary>
        public string MOBANDM { get; set; }
        /// <summary>
        ///模版名称
        /// </summary>
        public string MOBANMC { get; set; }
    }
}
