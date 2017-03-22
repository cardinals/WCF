using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械添加采购订单明细
    /// </summary>
    public class YAOXIETJCGDDMX_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 采购订单明细
        /// </summary>
        public List<CAIGOUDDXX> CAIGOUDDMX { get; set; }

        public YAOXIETJCGDDMX_IN()
        {
            CAIGOUDDMX = new List<CAIGOUDDXX>();
        }
    }

    public class YAOXIETJCGDDMX_OUT : MessageOUT
    {
        /// <summary>
        /// 采购订单返回明细
        /// </summary>
        public string CAIGOUDDFHMX { get; set; }

    }

    public class CAIGOUDDXX
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string DINDANBH { get; set; }
        /// <summary>
        /// 医院订单明细主键
        /// </summary>
        public string YIYUANDDMXZJ { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string SHANGPINBH { get; set; }
        /// <summary>
        /// 采购数量
        /// </summary>
        public string CAIGOUSL { get; set; }
        /// <summary>
        /// 配送企业编号
        /// </summary>
        public string PEISONGQYBH { get; set; }
        /// <summary>
        /// 送货地址
        /// </summary>
        public string SONGHUODZ { get; set; }
        /// <summary>
        /// 备案申请编号
        /// </summary>
        public string BEIANSQBH { get; set; }
        /// <summary>
        /// 自定义订单信息
        /// </summary>
        public string ZIDINGYDDXX { get; set; }

    }

}
