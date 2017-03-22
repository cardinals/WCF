using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZD_KESHIXX_IN : MessageIn 
    {
        
    }

    public class ZD_KESHIXX_OUT : MessageOUT 
    {
        /// <summary>
        /// 药品明细
        /// </summary>
        public List<HIS4.Schemas.ZD.KESHIXX> KESHIMX { get; set; }

        public ZD_KESHIXX_OUT()
        {
            this.KESHIMX = new List<HIS4.Schemas.ZD.KESHIXX>();
        }

    }

   
}

namespace HIS4.Schemas.ZD {
    public class KESHIXX
    {
        /// <summary>
        /// 科室ID
        /// </summary>
        public string KESHIID { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string KESHIMC { get; set; }
        /// <summary>
        /// 科室描述
        /// </summary>
        public string KESHIMS { get; set; }

    }
}
