using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械添加支付订单明细
    /// </summary>
    public class YAOXIETJZFDDMX_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 支付订单明细
        /// </summary>
        public List<ZHIFUDDXX> ZHIFUDDMX { get; set; }

        public YAOXIETJZFDDMX_IN()
        {
            ZHIFUDDMX = new List<ZHIFUDDXX>();
        }
    }

    public class YAOXIETJZFDDMX_OUT : MessageOUT
    {
        /// <summary>
        /// 支付订单返回明细
        /// </summary>
        public string ZHIFUDDFHMX { get; set; }

    }

    public class ZHIFUDDXX
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string DINDANBH { get; set; }
        /// <summary>
        /// 配送明细编号
        /// </summary>
        public string PEISONGMXBH { get; set; }
        /// <summary>
        /// 退货明细编号
        /// </summary>
        public string TUIHUOMXBH { get; set; }
        /// <summary>
        /// 自定义订单信息
        /// </summary>
        public string ZIDINGYDDXX { get; set; }

    }

}
