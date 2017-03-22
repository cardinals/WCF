using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class ZD_JIANCHAJYMX_IN : MessageIn
    {
        /// <summary>
        ///下载类型
        /// </summary>
        public string XIAZAILX { get; set; }

    }
    public class ZD_JIANCHAJYMX_OUT : MessageOUT
    {
        /// <summary>
        /// 检验检查模版列表
        /// </summary>
        public List<JIANCHAJYXX> JIANCHAJYMX { get; set; }
        public ZD_JIANCHAJYMX_OUT()
        {
            JIANCHAJYMX = new List<JIANCHAJYXX>();
        }
    }

    public class JIANCHAJYXX
    {
        /// <summary>
        ///模版代码
        /// </summary>
        public string MOBANDM { get; set; }
        /// <summary>
        ///明细代码
        /// </summary>
        public string JIANCHAJYDM { get; set; }
        /// <summary>
        ///明细名称
        /// </summary>
        public string JIANCHAJYMC { get; set; }
        /// <summary>
        ///父类序号
        /// </summary>
        public string FULEIXH { get; set; }
        /// <summary>
        ///末节判别
        /// </summary>
        public string MOJIEPB { get; set; }
        /// <summary>
        ///执行科室
        /// </summary>
        public string ZHIXINGKS { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        public string BEIZHU { get; set; }
    }
}
