using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class PAIDUIJHCX_IN : MessageIn
    {
        /// <summary>
        /// 科室代码
        /// </summary>
        public string KESHIID { get; set; }
        
    }

    public class PAIDUIJHCX_OUT : MessageOUT
    {
        /// <summary>
        /// 排队叫号明细
        /// </summary>
        public List<PAIDUIJHXX> PAIDUIJHMX { get; set; }

        public PAIDUIJHCX_OUT() {
            this.PAIDUIJHMX = new List<PAIDUIJHXX>();
        }
    }
}
