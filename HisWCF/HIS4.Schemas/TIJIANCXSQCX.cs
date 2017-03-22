using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class TIJIANCXSQCX_IN : MessageIn
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

    public class TIJIANCXSQCX_OUT : MessageOUT
    {
        /// <summary>
        /// 申请单ID
        /// </summary>
        public string SHENQINGDID { get; set; }
        /// <summary>
        /// 申请状态
        /// </summary>
        public string SHENQINGZT { get; set; }
    }

   
}
