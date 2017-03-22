using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 设备预约取消入参
    /// </summary>
    public class SHEBEIYYQX_IN : MessageIn
    {
        /// <summary>
        /// 预约申请单编号
        /// </summary>
        public string YUYUESQDBH { get; set; }
    }

    /// <summary>
    /// 设备预约取消出参
    /// </summary>
    public class SHEBEIYYQX_OUT : MessageOUT
    {

    }
}
