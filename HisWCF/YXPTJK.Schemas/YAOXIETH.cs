using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械退货
    /// </summary>
    public class YAOXIETH_IN : MessageIn
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
        public List<TUIHUOXX> TUIHUOMX { get; set; }

        public YAOXIETH_IN()
        {
            TUIHUOMX = new List<TUIHUOXX>();
        }
    }

    public class YAOXIETH_OUT : MessageOUT
    {
        /// <summary>
        /// 退货返回明细
        /// </summary>
        public string TUIHUOFHMX { get; set; }
    }

    public class TUIHUOXX
    {
        /// <summary>
        /// 医院退货明细主键
        /// </summary>
        public string YIYUANTHMXZJ { get; set; }
        /// <summary>
        /// 配送明细编号
        /// </summary>
        public string PEISONGMXBH { get; set; }
        /// <summary>
        /// 退货数量
        /// </summary>
        public string TUIHUOSL { get; set; }
        /// <summary>
        /// 退货原因
        /// </summary>
        public string TUIHUOYY { get; set; }
        /// <summary>
        /// 自定义退货信息
        /// </summary>
        public string ZIDINGYTHXX { get; set; }


    }

}
