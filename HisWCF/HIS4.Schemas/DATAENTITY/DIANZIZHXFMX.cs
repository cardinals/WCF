using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas.DATAENTITY
{
    public class DIANZIZHXFMX
    {
        /// <summary>
        /// 操作日期
        /// </summary>
        public string CAOZUORQ { get; set; }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string ZHIFUMC { get; set; }
        /// <summary>
        /// 发生金额
        /// </summary>
        public string FASHENGJE { get; set; }
        /// <summary>
        /// 本次金额
        /// </summary>
        public string BENCIJE { get; set; }
        /// <summary>
        /// 使用金额
        /// </summary>
        public string SHIYONGJE { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string CAOZUOYUAN { get; set; }
        /// <summary>
        /// 交易编号
        /// </summary>
        public string JIAOYIBH { get; set; }
    }
}
