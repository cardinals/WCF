using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas.DATAENTITY
{
    /// <summary>
    /// 支付宝账单明细
    /// </summary>
    public class ZHIFUBAOZDMX
    {
        /// <summary>
        /// 余额
        /// </summary>
        public string BALANCE { get; set; }
        /// <summary>
        /// 收入金额
        /// </summary>
        public string INCOME { get; set; }
        /// <summary>
        /// 支出金额
        /// </summary>
        public string OUTCOME { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string TRANSDATE { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string TRANSCODEMSG { get; set; }// trans_code_msg
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string MERCHANTOUTORDERNO { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string TRANSOUTERORDERNO { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BANKNAME { get; set; }//   bank_name
        /// <summary>
        /// bank_account_no 银行账号
        /// </summary>
        public string BANKACCOUNTNO { get; set; }
        /// <summary>
        /// 银行账户名字
        /// </summary>
        public string BANKACCOUNTNAME { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string MEMO { get; set; }
        /// <summary>
        /// 买家支付宝人民币资金账号
        /// </summary>
        public string BUYERACCOUNT { get; set; }
        /// <summary>
        /// 卖家支付宝人民币资金账号
        /// </summary>
        public string SELLERACCOUNT { get; set; }
        /// <summary>
        /// 卖家姓名
        /// </summary>
        public string SELLERFULLNAME { get; set; }
        /// <summary>
        /// 货币代码 156 人民币
        /// </summary>
        public string CURRENCY { get; set; }
        /// <summary>
        /// 充值网银流水号
        /// </summary>
        public string DEPOSITBANKNO { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GOODSTITLE { get; set; }
        /// <summary>
        /// 财务序列号
        /// </summary>
        public string IWACCOUNTLOGID { get; set; }
        /// <summary>
        /// 账务本方支付宝人民币资金账号
        /// </summary>
        public string TRANSACCOUNT { get; set; }
        /// <summary>
        /// 账务对方邮箱
        /// </summary>
        public string OUTHERACCOUNTEMAIL { get; set; }
        /// <summary>
        /// 账务对方全称
        /// </summary>
        public string OTHERACCOUNTFULLNAME { get; set; }
        /// <summary>
        /// 账务对方支付宝用户号
        /// </summary>
        public string OTHERUSERID { get; set; }
        /// <summary>
        /// 合作者身份ID
        /// </summary>
        public string PARTNERID { get; set; }
        /// <summary>
        /// 交易服务费
        /// </summary>
        public string SERVICFEE { get; set; }
        /// <summary>
        /// 交易服务费率
        /// </summary>
        public string SERVICEFEERATIO { get; set; }
        /// <summary>
        /// 交易总金额
        /// </summary>
        public string TOTALFEE { get; set; }
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string TRADENO { get; set; }
        /// <summary>
        /// 累积退款金额
        /// </summary>
        public string TRADEREFUNDAMOUNT { get; set; }



    }
}
