using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas.DATAENTITY;

namespace HIS4.Schemas
{
    public class DIANZIZHXX_IN : MessageIn
    {
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 检索时间范围
        /// </summary>
        public string SHIJIANFW { get; set; }
        /// <summary>
        /// 业务类型0全部 1充值
        /// </summary>
        public string YWLX { get; set; }
    }

    public class DIANZIZHXX_OUT : MessageOUT
    {
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public string TOTALCOUNT { get; set; }
        /// <summary>
        /// 费用明细
        /// </summary>
        public List<DIANZIZHXFMX> FEIYONGMX { get; set; }

        public DIANZIZHXX_OUT() {
            this.FEIYONGMX = new List<DIANZIZHXFMX>();
        }
    }
}
