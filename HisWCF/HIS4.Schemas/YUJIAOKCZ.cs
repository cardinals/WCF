using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class YUJIAOKCZ_IN : MessageIn
    {
        /// <summary>
        /// 病人ID
        /// </summary>
        public string BINGRENID { get; set; }
        /// <summary>
        /// 支付明细
        /// </summary>
        public List<ZHIFUXX> ZHIFUMX { get; set; }

        public YUJIAOKCZ_IN() {
            this.ZHIFUMX = new List<ZHIFUXX>();
        }

    }


    public class YUJIAOKCZ_OUT : MessageOUT
    {
    }
}
