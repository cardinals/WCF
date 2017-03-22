using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    ///药械获取接口调用凭据
    /// </summary>
    public class YAOXIEHQJKTYPJ_IN : MessageIn
    {
        /// <summary>
        /// 部门编码
        /// </summary>
        public string BUMENBM { get; set; }
        /// <summary>
        /// 部门密码
        /// </summary>
        public string BUMENMM { get; set; }
    }

    public class YAOXIEHQJKTYPJ_OUT : MessageOUT
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 凭证有效期
        /// </summary>
        public string PINGZHENGYXQ { get; set; }
        /// <summary>
        /// 当前时间
        /// </summary>
        public string DANGQIANSJ { get; set; }
        
    }



}