using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械获取配送信息
    /// </summary>
    public class YAOXIEHQPSXX_IN : MessageIn
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
        public List<CAIGOUDDPSXX> CAIGOUDDMX { get; set; }

        public YAOXIEHQPSXX_IN()
        {
            CAIGOUDDMX = new List<CAIGOUDDPSXX>();
        }
    }

    public class YAOXIEHQPSXX_OUT : MessageOUT
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
    public class CAIGOUDDPSXX
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string DINDANMXBH { get; set; }


    }

}
