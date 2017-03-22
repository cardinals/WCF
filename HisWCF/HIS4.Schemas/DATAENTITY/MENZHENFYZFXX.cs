using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class MENZHENFYZFXX
    {
        /// <summary>
        /// 处方序号
        /// </summary>
        public string CHUFANGXH { get; set; }
        /// <summary>
        /// 明细序号
        /// </summary>
        public string MINGXIXH { get; set; }
        /// <summary>
        /// 医保代码
        /// </summary>
        public string YIBAODM { get; set; }
        /// <summary>
        /// 医保自付比例
        /// </summary>
        public string YIBAOZFBL { get; set; }
        /// <summary>
        /// 项目限价
        /// </summary>
        public string XIANGMUXJ { get; set; }
        /// <summary>
        /// 医保项目归类
        /// </summary>
        public string YIBAOXMGL { get; set; }
        /// <summary>
        /// 医保等级
        /// </summary>
        public string YIBAODJ { get; set; }
        /// <summary>
        /// 自费金额
        /// </summary>
        public string ZIFEIJE { get; set; }
        /// <summary>
        /// 自理金额
        /// </summary>
        public string ZILIJE { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string BEIZHUXX { get; set; }
    }
}
