using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class JIANCHAJLCX_IN : MessageIn
    {

        /// <summary>
        /// 病人ID
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
        /// 就诊来源
        /// </summary>
        public string JIUZHENLY { get; set; }


    }

    public class JIANCHAJLCX_OUT : MessageOUT
    {
        /// <summary>
        /// 检查记录条数
        /// </summary>
        public string JIANCHAJLTS { get; set; }
        /// <summary>
        /// 检查记录明细
        /// </summary>
        public List<JIANCHAJLXX> JIANCHAJLMX { get; set; }

        public JIANCHAJLCX_OUT() {
            this.JIANCHAJLMX = new List<JIANCHAJLXX>();
        }
    }
}
