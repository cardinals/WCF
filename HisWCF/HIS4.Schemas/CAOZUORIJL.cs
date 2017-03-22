using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    /// <summary>
    /// 记录操作日志 add by Hu.Q 2017年3月9日 20:08:12
    /// </summary>
    public class CAOZUORIJL_IN : MessageIn
    {
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { set; get; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { set; get; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string CONTEXT { set; get; }
        /// <summary>
        /// 日志类型 1市民卡充值日志
        /// </summary>
        public string LEIXING { set; get; }
        /// <summary>
        /// 错误标志
        /// </summary>
        public string ERRBZ { get; set; }

    }
    public class CAOZUORIJL_OUT : MessageOUT
    {

    }
}
