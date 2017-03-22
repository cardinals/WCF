using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械新建支付订单
    /// </summary>
    public class YAOXIEXJZFDD_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 医院订单主键
        /// </summary>
        public string YIYUANDDZJ { get; set; }
        /// <summary>
        /// 企业编号
        /// </summary>
        public string QIYEBH { get; set; }
        /// <summary>
        /// 医院订单备注
        /// </summary>
        public string YIYUANDDBZ { get; set; }
    }

    public class YAOXIEXJZFDD_OUT : MessageOUT
    {
        /// <summary>
        /// 医院订单主键
        /// </summary>
        public string YIYUANDDZJ { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string DINGDANBH { get; set; }
    }


}
