﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械获取企业信息
    /// </summary>
    public class YAOXIEHQQYXX_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 企业明细
        /// </summary>
        public List<QIYEXX> QIYEMX { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public string YUEFEN { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public string DANGQIANYM { get; set; }


        public YAOXIEHQQYXX_IN()
        {
            QIYEMX = new List<QIYEXX>();
        }
    }

    public class YAOXIEHQQYXX_OUT : MessageOUT
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public string DANGQIANYM { get; set; }
        /// <summary>
        /// 总页码
        /// </summary>
        public string ZONGYEM { get; set; }
        /// <summary>
        /// 总行数
        /// </summary>
        public string ZONGHANGS { get; set; }
        /// <summary>
        /// 企业返回明细
        /// </summary>
        public string QIYEFHMX { get; set; }
    }
    public class QIYEXX
    {
        /// <summary>
        /// 企业编号
        /// </summary>
        public string QIYEBH { get; set; }


    }

}
