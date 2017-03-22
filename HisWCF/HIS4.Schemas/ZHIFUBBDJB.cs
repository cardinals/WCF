using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    /// <summary>
    /// 支付宝绑定解绑入参
    /// </summary>
    public class ZHIFUBBDJB_IN : MessageIn
    {
   
        /// <summary>
        /// 就诊卡类型2社保卡 3就诊卡
        /// </summary>
        public string JIUZHENKLX { get; set; }
        public string JIUZHENKH { get; set; }
        public string XINGMING { get; set; }
        public string LIANXIDH { get; set; }
        public string ZHENGJIANLX { get; set; }
        public string ZHENGJIANHM { get; set; }
        public string ZHIFUBXYH { get; set; }
        public string ZHIFUBYHH { get; set; }
        public string CAOZUORQ { get; set; }
        public string CAOZUOLX { get; set; }
        public string BEIZHUXX { get; set; }
        public string PARTNER { get; set; }
        public string SELLEREMAIL { get; set; }
    }
    /// <summary>
    /// 支付宝绑定解绑出参
    /// </summary>
    public class ZHIFUBBDJB_OUT : MessageOUT
    {

    }
}
