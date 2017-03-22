using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class ZD_ZHENSHIXX_IN : MessageIn
    {

    }
    public class ZD_ZHENSHIXX_OUT : MessageOUT
    {
        /// <summary>
        /// 诊室信息列表
        /// </summary>
        public List<ZHENSHIXX> ZHENSHILB { get; set; }
        public ZD_ZHENSHIXX_OUT()
        {
            ZHENSHILB = new List<ZHENSHIXX>();
        }
    }

    public class ZHENSHIXX
    {
        /// <summary>
        /// 诊室代码
        /// </summary>
        public string ZHENSHIDM { get; set; }
        /// <summary>
        /// 诊室名称
        /// </summary>
        public string ZHENSHIMC { get; set; }

    }
}
