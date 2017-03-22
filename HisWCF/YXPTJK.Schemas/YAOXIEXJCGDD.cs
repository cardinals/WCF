using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械新建采购订单
    /// </summary>
    public class YAOXIEXJCGDD_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 医院订单主键
        /// </summary>
        public string YIYUANDDZJ { get; set; }
        /// <summary>
        /// 医院订单类型
        /// </summary>
        public string YIYUANDDLX { get; set; }
        /// <summary>
        /// 医院订单备注
        /// </summary>
        public string YIYUANDDBZ { get; set; }
    }

    public class YAOXIEXJCGDD_OUT : MessageOUT
    {
        /// <summary>
        /// 医院订单主键
        /// </summary>
        public string YIYUANDDZJ { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string DINGDANBH { get; set; }
    }


}
