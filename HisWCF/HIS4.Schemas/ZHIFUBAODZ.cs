using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZHIFUBAODZ_IN : MessageIn
    {
    
        /// <summary>
        /// 对账类型 1操作员对账 2日对账
        /// </summary>
        public string DZLX { get; set; }
        /// <summary>
        /// 操作员对账
        /// </summary>
        public string CZYDM { get; set; }
        /// <summary>
        /// 对账日期
        /// </summary>
        public string DZRQ { get; set; }
        /// <summary>
        /// 结算总额
        /// </summary>
        public string JIESUANZE { get; set; }
    }
    public class ZHIFUBAODZ_OUT : MessageOUT
    {
        /// <summary>
        /// 0对账平 1平台多2 HIS多
        /// </summary>
        public string DZJG { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string BZXX { get; set; }
    }
}
