using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class ZD_CHUANGWEIXX_IN : MessageIn
    {
        public string BINGQUDM { get; set; }//病区代码
    }
    public class ZD_CHUANGWEIXX_OUT : MessageOUT
    {
        /// <summary>
        /// 床位信息列表
        /// </summary>
        public List<CHUANGWEIXX> CHUANGWEIMX { get; set; }
        public ZD_CHUANGWEIXX_OUT()
        {
            CHUANGWEIMX = new List<CHUANGWEIXX>();
        }
    }

    public class CHUANGWEIXX
    {
        /// <summary>
        /// 床位号
        /// </summary>
        public string CHUANGWEIH { get; set; }
        /// <summary>
        /// 床位说明
        /// </summary>
        public string CHUANGWEISM { get; set; }

    }
}
