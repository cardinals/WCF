using JYCS.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class JINRONGJYRZ_IN : MessageIn
    {
        ///<summary>  
        ///   就诊卡号                                   
        ///</summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 病案号
        /// </summary>
        public string BINGANHAO { get; set; }
        ///<summary>  
        ///   交易类型（现金银行卡智慧医疗支付宝微信） 
        ///</summary>
        public string JIAOYILX { get; set; }
        ///<summary>  
        ///   交易方式（充值消费冲销取现）               
        ///</summary>
        public string JIAOYIFS { get; set; }
        ///<summary>  
        ///   商户号                                     
        ///</summary>
        public string SHANGHUH { get; set; }
        ///<summary>  
        ///   终端号                                     
        ///</summary>
        public string ZHONGDUANH { get; set; }
        ///<summary>  
        ///   银行卡号                                   
        ///</summary>
        public string YINHANGKH { get; set; }
        ///<summary>  
        ///   交易批次号                                 
        ///</summary>
        public string JIAOYIPCH { get; set; }
        ///<summary>  
        ///   交易流水号                                 
        ///</summary>
        public string JIAOYILSH { get; set; }
        ///<summary>  
        ///   交易参考号                                 
        ///</summary>
        public string JIAOYICKH { get; set; }
        ///<summary>  
        ///   订单号                                     
        ///</summary>
        public string DINGDANH { get; set; }
        ///<summary>  
        ///   交易金额                                   
        ///</summary>
        public string JIAOYIJE { get; set; }
        ///<summary>  
        ///   100元纸币张数                              
        ///</summary>
        public string ZHIBIZS100 { get; set; }
        ///<summary>  
        ///   50元纸币张数                               
        ///</summary>
        public string ZHIBIZS50 { get; set; }
        ///<summary>  
        ///   20元纸币张数                               
        ///</summary>
        public string ZHIBIZS20 { get; set; }
        ///<summary>  
        ///   10元纸币张数                               
        ///</summary>
        public string ZHIBIZS10 { get; set; }
        ///<summary>  
        ///   5元纸币张数                                
        ///</summary>
        public string ZHIBIZS5 { get; set; }
        ///<summary>  
        ///   1元纸币张数                                
        ///</summary>
        public string ZHIBIZS1 { get; set; }
        /// <summary>
        /// 交易状态 1 成功 2 失败
        /// </summary>
        public string JIAOYIZT { get; set; }
        /// <summary>
        /// 关联交易ID
        /// </summary>
        public string GUANLIANJYID { get; set; }
    }

    /// <summary>
    /// 交易出参
    /// </summary>
    public class JINRONGJYRZ_OUT : MessageOUT
    {
        /// <summary>
        /// 交易编号
        /// </summary>
        public string JIAOYIID { get; set; }
    }
}
