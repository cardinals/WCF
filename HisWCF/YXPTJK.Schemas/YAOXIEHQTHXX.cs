using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械获取退货信息
    /// </summary>
    public class YAOXIEHQTHXX_IN : MessageIn
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
        /// 退货明细
        /// </summary>
        public List<TUIHUOHQXX> TUIHUOMX { get; set; }

        public YAOXIEHQTHXX_IN()
        {
            TUIHUOMX = new List<TUIHUOHQXX>();
        }
    }


    public class YAOXIEHQTHXX_OUT : MessageOUT
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
        /// 退货返回明细
        /// </summary>
        public string TUIHUOFHMX { get; set; }

    }

    public class TUIHUOHQXX
    {
        /// <summary>
        /// 配送明细编号
        /// </summary>
        public string PEISONGMXBH { get; set; }

    }

}
