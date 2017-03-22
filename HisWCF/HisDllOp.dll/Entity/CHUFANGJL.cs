using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class CHUFANGJL : BaseGroup
    {
        /// <summary>
        /// 处方ID
        /// </summary>
        public string CHUFANGID { get; set; }
        /// <summary>
        /// 处方类型  0 医技 1 处方
        /// </summary>
        public string CHUFANGLB { get; set; }
        /// <summary>
        /// 处方日期
        /// </summary>
        public DateTime? CHUFANGRQ { get; set; }
        /// <summary>
        /// 处方名称
        /// </summary>
        public string CHUFANGMC { get; set; }
        /// <summary>
        /// 就诊科室
        /// </summary>
        public string JIUZHENKS { get; set; }
        /// <summary>
        /// 就诊医生
        /// </summary>
        public string JIUZHENYS { get; set; }
    }
}
