using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class SHEBEIYYQX_IN : MessageIn
    {
        /// <summary>
        /// 预约申请单编号
        /// </summary>
        public string YUYUESQDBH { get; set; }
        /// <summary>
        /// 业务类型 1检查 2检验
        /// </summary>
        public string YEWULX { get; set; }
       
    }

    public class SHEBEIYYQX_OUT : MessageOUT
    {

    }
}
