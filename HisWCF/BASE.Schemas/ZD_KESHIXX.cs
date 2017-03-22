using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class ZD_KESHIXX_IN : MessageIn
    {

    }
    public class ZD_KESHIXX_OUT : MessageOUT
    {
        /// <summary>
        /// 病区信息列表
        /// </summary>
        public List<KESHIXX> KESHIMX { get; set; }
        public ZD_KESHIXX_OUT()
        {
            KESHIMX = new List<KESHIXX>();
        }
    }

    public class KESHIXX
    {
        /// <summary>
        /// 科室代码
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string KESHIMC { get; set; }

    }
}
