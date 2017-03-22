using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class KESHIXX
    {
        /// <summary>
        /// 科室代码
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string KESHIMC { get; set; }
        /// <summary>
        /// 就诊地点
        /// </summary>
        public string JIUZHENDD { get; set; }
        /// <summary>
        /// 科室介绍
        /// </summary>
        public string KESHIJS { get; set; }
        /// <summary>
        /// 状态标识
        /// </summary>
        public string ZHUANGTAIBZ { get; set; }

        public KESHIXX() {

            this.KESHIDM = string.Empty;
            this.KESHIMC = string.Empty;
            this.JIUZHENDD = string.Empty;
            this.KESHIJS = string.Empty;
            this.ZHUANGTAIBZ = string.Empty;
        }
    }
}
