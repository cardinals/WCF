using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;


namespace HIS4.Schemas
{
    public class MENZHENJSJL_IN : MessageIn
    {
        /// <summary>
        /// 病人编号
        /// </summary>
        public string BINGRENID { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string KAISHIRQ { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string JIESHURQ { get; set; }
    }

    public class MENZHENJSJL_OUT : MessageOUT
    {
        /// <summary>
        /// 结束明细条数
        /// </summary>
        public string JIESUANMXTS { get; set; }
        /// <summary>
        /// 门诊结算记录明细
        /// </summary>
        public List<MENZHENJSXX> MENZHENJSMX { get; set; }

        public MENZHENJSJL_OUT() {
            this.MENZHENJSMX = new List<MENZHENJSXX>();
        }
    }
}
