using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class JIAOYIZF
    {  /// <summary>
        /// 交易转发入参
        /// </summary>
        public class JIAOYIZF_IN : MessageIn
        {
            public string JIEKOULX { get; set; }
            public string JIAOYILX { get; set; }
            public string JIAOYIRC { get; set; }
        }
        /// <summary>
        /// 交易转发出参
        /// </summary>
        public class JIAOYIZF_OUT : MessageOUT
        {
            public string JIEKOULX { get; set; }
            public string JIAOYILX { get; set; }
            public string JIAOYICC { get; set; }
        }
    }
}
