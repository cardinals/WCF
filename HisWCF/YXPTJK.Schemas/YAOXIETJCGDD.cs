using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 提交采购订单
    /// </summary>
    public class YAOXIETJCGDD_IN : MessageIn
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

    public class YAOXIETJCGDD_OUT : MessageOUT
    {
    }


}
