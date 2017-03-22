using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class HUANZHEJHCX_IN : MessageIn
    {
        /// <summary>
        /// 病人ID
        /// </summary>
        public string BINGRENID { get; set; }
    }

    public class HUANZHEJHCX_OUT : MessageOUT
    {
        /// <summary>
        /// 患者叫号明细
        /// </summary>
        public List<PAIDUIJHXX> HUANZHEJHMX { get; set; }

        public HUANZHEJHCX_OUT()
        {
            this.HUANZHEJHMX = new List<PAIDUIJHXX>();
        }
    }
}
