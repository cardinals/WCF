using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
namespace HIS4.Schemas
{
    public class MENZHENJSMX_IN : MessageIn
    {
        /// <summary>
        /// 病人编号
        /// </summary>
        public string BINGRENID { get; set; }
        /// <summary>
        /// 收费ID
        /// </summary>
        public string SHOUFEIID { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string KAISHIRQ { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string JIESHURQ { get; set; }

    }

    public class MENZHENJSMX_OUT : MessageOUT
    {
        /// <summary>
        /// 费用明细条数
        /// </summary>
        public string FEIYONGMXTS { get; set; }
        /// <summary>
        /// 费用明细
        /// </summary>
        public List<MENZHENFYXX> FEIYONGMX { get; set; }

        public MENZHENJSMX_OUT() {
            this.FEIYONGMX = new List<MENZHENFYXX>();
        }
    }
}
