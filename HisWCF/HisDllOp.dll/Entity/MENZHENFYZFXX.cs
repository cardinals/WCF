using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    /// <summary>
    /// 门诊费用自负信息
    /// </summary>
    public class MENZHENFYZFXX : BaseGroup
    {
        /// <summary>
        ///处方序号
        /// </summary>
        public string CHUFANGXH { set; get; }
        /// <summary>
        ///明细序号
        /// </summary>
        public string MINGXIXH { set; get; }
        /// <summary>
        ///医保代码
        /// </summary>
        public string YIBAODM { set; get; }
        /// <summary>
        ///医保自付比例
        /// </summary>
        public string YIBAOZFBL { set; get; }
        /// <summary>
        ///项目限价
        /// </summary>
        public string XIANGMUXJ { set; get; }
        /// <summary>
        ///医保项目归类
        /// </summary>
        public string YIBAOXMGL { set; get; }
        /// <summary>
        ///医保等级
        /// </summary>
        public string YIBAODJ { set; get; }
        /// <summary>
        ///自费金额
        /// </summary>
        public string ZIFEIJE { set; get; }
        /// <summary>
        ///自理金额
        /// </summary>
        public string ZILIJE { set; get; }
        /// <summary>
        ///备注信息
        /// </summary>
        public string BEIZHUXX { set; get; }

    }
}
