using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;


namespace HIS4.Schemas
{
    public class ZHIFUBAOTF_IN : MessageIn
    {
        /// <summary>
        /// 结算类型 1手机网页退费
        /// </summary>
        public string TUIFEILX { get; set; }
        /// <summary>
        /// 服务器异步通知页面路径
        /// </summary>
        public string NOTIFYURL { get; set; }
        /// <summary>
        /// 退款请求时间 yyyy-MM-dd hh:mm:ss
        /// </summary>
        public string REFUNDDATA { get; set; }
        /// <summary>
        ///  批次号  都必须保证唯一性  格式为：退款日期（8位）+流水号（3～24位）。不可重复，且退款日期必须是当天日期。流水号可以接受数字或英文字符，建议使用数字，但不可接受“000”
        /// </summary>
        public string BATCHNO { get; set; }
        /// <summary>
        /// 退款笔数
        /// </summary>
        public string BATCHNUM { get; set; }
        /// <summary>
        /// 单笔数据集
        /// </summary>
        public string DETAILDATA { get; set; }


        /// <summary>
        /// 退款请求订单号
        /// </summary>
        public string WIDOUTREQUESTNO { get; set; }
        /// <summary>
        /// 结算时的订单号
        /// </summary>
        public string WIDOUTTRADENO { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string WIDTOTALFEE { get; set; }
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string TRADENO { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string REFUNDREASON { get; set; }
    }
    public class ZHIFUBAOTF_OUT : MessageOUT
    {
        /// <summary>
        /// 手机WEB退费URL
        /// </summary>
        public string TUIFEIURL { get; set; }

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
        /// fund_change	本次退款请求是否发生资金变动
        /// </summary>
        public string FUNDCHANGE { set; get; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string REFUNDFEE { set; get; }
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
