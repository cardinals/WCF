using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZHIFUBAOCX_IN : MessageIn
    {
        /// 商户订单号
        /// </summary>
        public string WIDOUTTRADENO { get; set; }

        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string TRADENO { get; set; }
    }
    public class ZHIFUBAOCX_OUT : MessageOUT
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
        ///   buyer_user_id	买家支付宝用户号
        /// </summary>
        public string BUYERUSERID { get; set; }
        /// <summary>
        /// buyer_logon_id	买家支付宝账号	
        /// </summary>
        public string BUYEREMAIL { get; set; }
        /// <summary>
        /// 合作者身份ID
        /// </summary>
        public string PARTNER { set; get; }
        /// <summary>
        /// 交易状态
        /// </summary>
        public string TRADESTATUS { set; get; }
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
