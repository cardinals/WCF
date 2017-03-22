using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class JIANCHAJGCX_IN : MessageIn 
    {
        /// <summary>
        /// 申请单ID
        /// </summary>
        public string SHENQINDANID { get; set; }

    }

    public class JIANCHAJGCX_OUT : MessageOUT 
    {
        /// <summary>
        /// 检查结果明细
        /// </summary>
        public List<JIANCHAJGXX> JIANCHAJGMX { get; set; }

        public JIANCHAJGCX_OUT() {
            this.JIANCHAJGMX = new List<JIANCHAJGXX>();
        }
    }
}
