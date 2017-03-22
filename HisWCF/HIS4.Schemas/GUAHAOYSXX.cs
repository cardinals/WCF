using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class GUAHAOYSXX_IN :MessageIn
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
        /// 科室代码
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 挂号类别
        /// </summary>
        public string GUAHAOLB { get; set; }
    }

    public class GUAHAOYSXX_OUT : MessageOUT {
        /// <summary>
        /// 医生明细
        /// </summary>
        public List<YISHENGXX> YISHENGMX { get; set; }

        public GUAHAOYSXX_OUT() {
            this.YISHENGMX = new List<YISHENGXX>();
        }
    
    }
}
