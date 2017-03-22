using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    /// <summary>
    /// 支付宝绑定查询
    /// </summary>
    public class ZHIFUBAOBDCX_IN : MessageIn
    {
        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZHENGJIANLX { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
        /// <summary>
        /// 就诊卡类型2社保卡 3就诊卡
        /// </summary>
        public string JIUZHENKLX { get; set; }
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
    }
    public class ZHIFUBAOBDCX_OUT : MessageOUT
    {
        /// <summary>
        /// 支付宝协议号
        /// </summary>
        public string ZHIFUBXYH { get; set; }
        //public string ZHIFUBYHH { get; set; }
        //public string CAOZUORQ { get; set; }
    }
}
