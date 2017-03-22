using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    /// <summary>
    /// 疾病信息
    /// </summary>
    public class JIBINGXX : BaseGroup
    {
        /// <summary>
        ///疾病代码
        /// </summary>
        public string JIBINGDM { set; get; }
        /// <summary>
        ///疾病ICD
        /// </summary>
        public string JIBINGICD { set; get; }
        /// <summary>
        ///疾病名称
        /// </summary>
        public string JIBINGMC { set; get; }
        /// <summary>
        ///疾病描述
        /// </summary>
        public string JIBINGMS { set; get; }
    }
}
