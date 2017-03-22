using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class GUAHAOKSXX_IN : MessageIn
    {
        /// <summary>
        /// 挂号方式
        /// </summary>
        public string GUAHAOFS { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string RIQI { get; set; }
        /// <summary>
        /// 挂号班次
        /// </summary>
        public string GUAHAOBC { get; set; }
        /// <summary>
        /// 挂号类别
        /// </summary>
        public string GUAHAOLB { get; set; }
    }

    public class GUAHAOKSXX_OUT : MessageOUT {
        /// <summary>
        /// 科室明细
        /// </summary>
        public List<KESHIXX> KESHIMX { get; set; }

        public GUAHAOKSXX_OUT() {
            this.KESHIMX = new List<KESHIXX>();
        }

    }
}
