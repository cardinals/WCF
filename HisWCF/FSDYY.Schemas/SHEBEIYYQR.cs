using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 设备预约确认入参
    /// </summary>
    public class SHEBEIYYQR_IN : MessageIn
    {
        /// <summary>
        /// 预约申请单编号
        /// </summary>
        public string YUYUESQDBH { get; set; }

        /// <summary>
        /// 预约确认类型
        /// </summary>
        public string YUYUEQRLX { get; set; }

        
    }

    /// <summary>
    /// 设备预约确认出参
    /// </summary>
    public class SHEBEIYYQR_OUT : MessageOUT
    {
        /// <summary>
        /// 预约号
        /// </summary>
        public string YUYUEH { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public string YUYUERQ { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public string YUYUESJ { get; set; }
    }
}
