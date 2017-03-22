using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class TIJIANCXSQ_IN : MessageIn
    {
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 体检编码
        /// </summary>
        public string TIJIANBM { get; set; }
        /// <summary>
        /// 申请类型
        /// </summary>
        public string SHENQINGLX { get; set; }
        
    }

    public class TIJIANCXSQ_OUT : MessageOUT
    {
               
    }

   
}
