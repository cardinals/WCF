using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class MENZHENJSXX
    {
        /// <summary>
        /// 收费ID
        /// </summary>
        public string SHOUFEIID { get; set; }
        /// <summary>
        /// 费用类别
        /// </summary>
        public string FEIYONGLB { get; set; }
        /// <summary>
        /// 费用类别名称
        /// </summary>
        public string FEIYONGLBMC { get; set; }
        /// <summary>
        /// 费用合计
        /// </summary>
        public string FEIYONGHJ { get; set; }
        /// <summary>
        /// 实付金额
        /// </summary>
        public string SHIFUJE { get; set; }
        /// <summary>
        /// 交易类型 1正交易 -1反交易
        /// </summary>
        public string JIAOYILX { get; set; }
        /// <summary>
        /// 急诊标志 0普通1急诊
        /// </summary>
        public string JIZHENBZ { get; set; }
        /// <summary>
        /// 收费类别 1挂号2收费
        /// </summary>
        public string SHOUFEILB { get; set; }
        /// <summary>
        /// 收费类别名称
        /// </summary>
        public string SHOUFEILBMC { get; set; }
        /// <summary>
        /// 收费类型 10门诊挂号 11急诊挂号 20门诊收费 21急诊收费 30规定病种 31单病种 41其他收费 
        /// </summary>
        public string SHOUFEILX { get; set; }
        /// <summary>
        /// 收费类型名称
        /// </summary>
        public string SHOUFEILXMC { get; set; }
        /// <summary>
        /// 收费日期 yyyy-mm-dd hh24:mm:ss
        /// </summary>
        public string SHOUFEIRQ { get; set; }
        /// <summary>
        /// 记录来源 0正常 1退费 2作废
        /// </summary>
        public string JILULY { get; set; }
        /// <summary>
        /// 记录来源 0正常 1退费 2作废
        /// </summary>
        public string JILULYMC { get; set; }

    }
}
