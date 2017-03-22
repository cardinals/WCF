using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械获取商品信息
    /// </summary>
    public class YAOXIEHQSPXX_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 商品明细
        /// </summary>
        public List<SHANGPINXX> SHANGPINMX { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public string YUEFEN { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public string DANGQIANYM { get; set; }

        public YAOXIEHQSPXX_IN()
        {
            SHANGPINMX = new List<SHANGPINXX>();
        }
    }

    public class YAOXIEHQSPXX_OUT : MessageOUT
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
        /// 商品返回明细
        /// </summary>
        public string SHANGPINFHMX { get; set; }
    }
    public class SHANGPINXX
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public string SHANGPINBH { get; set; }


    }

}
