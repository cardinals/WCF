using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZHIFUBAOJS_IN : MessageIn
    {
        /// <summary>
        /// 结算类型 1手机网页支付
        /// </summary>
        public string JIESUANLX { get; set; }
        /// <summary>
        /// 请求单号 每次都必须唯一
        /// </summary>
        public string QINGQIUDH { get; set; }
        /// <summary>
        /// 服务器异步通知页面路径
        /// </summary>
        public string NOTIFYURL { get; set; }
        /// <summary>
        /// 页面跳转同步通知页面路径
        /// </summary>
        public string CALLBACKURL { get; set; }
        /// <summary>
        /// 操作中断返回地址
        /// </summary>
        public string MERCHANTURL { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string WIDOUTTRADENO { get; set; }
        /// <summary>
        /// 订单名称
        /// </summary>
        public string WIDSUBJECT { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string WIDTOTALFEE { get; set; }
        /// <summary>
        /// 订单描述
        /// </summary>
        public string WIDBODY { get; set; }
        /// <summary>
        /// 超时时间取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（无论交易何时创建，都在0点关闭）。该参数数值不接受小数点，如1.5h，可转换为90m。
        /// </summary>
        public string OUTTIME { get; set; }
        /// <summary>
        /// 买家支付宝用户号 商户代扣模式用到
        /// </summary>
        public string BUYERID { get; set; }
        /// <summary>
        /// 买家支付宝帐号 商户代扣模式用到
        /// </summary>
        public string BUYEREMAIL { get; set; }
        /// <summary>
        /// 授权号
        /// </summary>
        public string AUTHNO { get; set; }
        /// <summary>
        /// 协议号
        /// </summary>
        public string AGREENNO { get; set; }
        /// <summary>
        /// 就诊卡类型2社保卡 3就诊卡
        /// </summary>
        public string JIUZHENKLX { get; set; }
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
    }
    public class ZHIFUBAOJS_OUT : MessageOUT
    {
        /// <summary>
        /// 手机WEB结算URL
        /// </summary>
        public string JIESUANURL { get; set; }
        /// <summary>
        /// 二维码码串
        /// </summary>
        public string QRCODE { get; set; }
        /// <summary>
        /// pic_url	二维码图片地址
        /// </summary>
        public string PICURL { get; set; }
        /// <summary>
        /// small_pic_url	二维码小图地址
        /// </summary>
        public string SMALLPICURL { get; set; }
        /// <summary>
        /// 服务平台图片地址
        /// </summary>
        public string PICLOCAL { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string WIDOUTTRADENO { get; set; }


        /// <summary>
        /// 响应码  参考字典
        /// </summary>
        public string RESULTCODE { get; set; }
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string TRADENO { get; set; }
        /// <summary>
        /// 买家支付宝用户号 商户代扣模式用到
        /// </summary>
        public string BUYERID { get; set; }
        /// <summary>
        /// 买家支付宝帐号 商户代扣模式用到
        /// </summary>
        public string BUYEREMAIL { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string TOTALFREE { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string PAYMENTTIME { get; set; }
        /// <summary>
        /// 返回扩展信息
        /// </summary>
        public string EXTENDINFO { get; set; }
        /// <summary>
        /// 本次交易支付单据信息集合
        /// </summary>
        public string FUNDBILLLIST { get; set; }
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
