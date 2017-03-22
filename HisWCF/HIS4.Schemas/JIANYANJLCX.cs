using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class JIANYANJLCX_IN : MessageIn
    {
        /// <summary>
        /// 病人唯一编号
        /// </summary>
        public string BINGRENID { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string KAISHIRQ { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string JIESHURQ { get; set; }
        /// <summary>
        /// 病人唯一编号
        /// </summary>
        public string JIUZHENLY { get; set; }
    }

    public class JIANYANJLCX_OUT : MessageOUT 
    {
        /// <summary>
        /// 检验记录条数
        /// </summary>
        public string JIANYANJLTS { get; set; }
        /// <summary>
        /// 检验记录明细
        /// </summary>
        public List<JIANYANJLXX> JIANYANJLMX { get; set; }

        public JIANYANJLCX_OUT() {
            this.JIANYANJLMX = new List<JIANYANJLXX>();
        }
    }
}
