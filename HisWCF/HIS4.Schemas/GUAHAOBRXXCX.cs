using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class GUAHAOBRXXCX_IN : MessageIn
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public string KAISHIRQ { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string JIESHURQ { get; set; }
        /// <summary>
        /// 挂号班次
        /// </summary>
        public string GUAHAOBC { get; set; }
        /// <summary>
        /// 科室代码
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 医生代码
        /// </summary>
        public string YISHENGDM { get; set; }
        
    }

    public class GUAHAOBRXXCX_OUT : MessageOUT {
        /// <summary>
        /// 挂号病人信息列表
        /// </summary>
        public List<GUAHAOBRXX> GUAHAOBRXXLB { get; set; }

        public GUAHAOBRXXCX_OUT() {
            this.GUAHAOBRXXLB = new List<GUAHAOBRXX>();
        }
    }
}
