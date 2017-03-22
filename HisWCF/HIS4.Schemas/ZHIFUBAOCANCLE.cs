using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZHIFUBAOCANCLE_IN : MessageIn
    {
        /// 商户订单号
        /// </summary>
        public string WIDOUTTRADENO { get; set; }

        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string TRADENO { get; set; }
    }
    public class ZHIFUBAOCANCLE_OUT : MessageOUT
    {
        /// <summary>
        ///   result_code	响应码	String(32)	退款处理结果响应码。
        /// </summary>
        public string RESULTCODE { get; set; }

        /// <summary>
        /// trade_no	支付宝交易号
        /// </summary>
        public string TRADENO { get; set; }
        /// <summary>
        /// 商户网站唯一订单号  原样返回
        /// </summary>
        public string WIDOUTREQUESTNO { get; set; }

        /// <summary>
        /// 是否可重试标志
        /// </summary>
        public string RETRYFLAG { set; get; }

        /// <summary>
        /// detail_error_code	详细错误码
        /// </summary>
        public string ERRORCODE { get; set; }
        /// <summary>
        /// detail_error_des	详细错误描述
        /// </summary>
        public string ERRORDES { set; get; }

    }
}
