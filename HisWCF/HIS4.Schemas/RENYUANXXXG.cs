using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class RENYUANXXXG_IN:MessageIn
    {
        /// <summary>
        /// 病人id
        /// </summary>
        public string BINGRENID { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string LIANXIDH { get; set; }

        public RENYUANXXXG_IN() {
        }

    }

    public class RENYUANXXXG_OUT:MessageOUT
    {

        public RENYUANXXXG_OUT() {
            
        }
    }
}
