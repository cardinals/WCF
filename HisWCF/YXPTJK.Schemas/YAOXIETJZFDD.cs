using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 提交支付订单
    /// </summary>
    public class YAOXIETJZFDD_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string DINGDANBH { get; set; }
    }

    public class YAOXIETJZFDD_OUT : MessageOUT
    {
    }


}
