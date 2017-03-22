﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械获取订单明细信息
    /// </summary>
    public class YAOXIEHQDDMXXX_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public string DANGQIANYM { get; set; }

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public List<CAIGOUDDMXXX> CAIGOUDDMX { get; set; }

        public YAOXIEHQDDMXXX_IN()
        {
            CAIGOUDDMX = new List<CAIGOUDDMXXX>();
        }
    }


    public class YAOXIEHQDDMXXX_OUT : MessageOUT
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
        /// 采购订单返回明细
        /// </summary>
        public string CAIGOUDDFHMX { get; set; }

    }

    public class CAIGOUDDMXXX
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string DINDANMXBH { get; set; }


    }

}