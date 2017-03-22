using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class CHONGZHI_IN : MessageIn
    {
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 就诊卡类型
        /// </summary>
        public string JIUZHENKLX { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string LIANXIDH { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public string CHONGZHIJE { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string ZHIFUFS { get; set; }
        /// <summary>
        /// 银行卡信息
        /// </summary>
        public string YINGHANGKXX { get; set; }
    }

    public class CHONGZHI_OUT : MessageOUT {
        /// <summary>
        /// 账户余额
        /// </summary>
        public string ZHANGHUYE { get; set; }
    }
}
