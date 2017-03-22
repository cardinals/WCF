using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class BINGZHONGXX
    {
        /// <summary>
        /// 疾病代码
        /// </summary>
        public string JIBINGDM { get; set; }
        /// <summary>
        /// 疾病ICD
        /// </summary>
        public string JIBINGICD { get; set; }
        /// <summary>
        /// 疾病名称
        /// </summary>
        public string JIBINGMC { get; set; }
        /// <summary>
        /// 疾病审批编号
        /// </summary>
        public string JIBINGSPBH { get; set; }
        /// <summary>
        /// 疾病备注
        /// </summary>
        public string JIBINGBZ { get; set; }
        /// <summary>
        /// 疾病类型
        /// </summary>
        public string JIBINGLX { get; set; }
    }
}
