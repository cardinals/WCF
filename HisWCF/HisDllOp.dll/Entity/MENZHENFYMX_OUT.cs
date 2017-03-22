using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class MENZHENFYMX_OUT : BaseOutEntity
    {
        /// <summary>
        /// 医疗类别
        /// </summary>
        public string YILIAOLB { get; set; }
        /// <summary>
        /// 疾病明细
        /// </summary>
        public List<JIBINGXX> JIBINGMX { get; set; }
        /// <summary>
        /// 费用明细条数
        /// </summary>
        public int FEIYONGMXTS { get; set; }
        /// <summary>
        /// 费用明细
        /// </summary>
        public List<MENZHENFYXX> FEIYONGMX { get; set; }
       
       
    }
}
