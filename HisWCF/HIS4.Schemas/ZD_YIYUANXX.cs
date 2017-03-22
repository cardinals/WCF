using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZD_YIYUANXX_IN : MessageIn 
    {
        
    }

    public class ZD_YIYUANXX_OUT : MessageOUT 
    {
        /// <summary>
        /// 医院名称
        /// </summary>
        public string YIYUANMC { get; set; }
        /// <summary>
        /// 医院介绍
        /// </summary>
        public string YIYUANJS { get; set; }
        /// <summary>
        /// 医院地址
        /// </summary>
        public string YIYUANDZ { get; set; }
    }

}
