using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class ZD_BINGQUXX_IN : MessageIn
    {

    }
    public class ZD_BINGQUXX_OUT : MessageOUT
    {
        /// <summary>
        /// 病区信息列表
        /// </summary>
        public List<BINGQUXX> BINGQUMX { get; set; }
        public ZD_BINGQUXX_OUT()
        {
            BINGQUMX = new List<BINGQUXX>();
        }
    }

    public class BINGQUXX
    {
        /// <summary>
        /// 病区代码
        /// </summary>
        public string BINGQUDM { get; set; }
        /// <summary>
        /// 病区名称
        /// </summary>
        public string BINGQUMC { get; set; }

    }
}
