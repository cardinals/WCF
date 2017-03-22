using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    class MENZHENYJS_OUT : BaseOutEntity
    {
        /// <summary>
        /// 结算ID
        /// </summary>
        public string JIESUANID { get; set; }
        /// <summary>
        /// 结算结果
        /// </summary>
        public JIESUANJG JIESUANJG { get; set; }
        /// <summary>
        /// 详细结算结果
        /// </summary>
        public List<JIESUANJGXX> XIANGXIJSJG { get; set; }
        /// <summary>
        /// 费用自负明细
        /// </summary>
        public List<MENZHENFYZFXX> FEIYONGZFMX { get; set; }
        /// <summary>
        /// 重复交易信息
        /// </summary>
        public List<CHONGFUJYXX> CHONGFUJYMX { get; set; }
    }
}
