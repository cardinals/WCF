using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 药械收货
    /// </summary>
    public class YAOXIESH_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string JIEKOUDYPJ { get; set; }

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public List<CAIGOUDDSHXX> CAIGOUDDMX { get; set; }

        public YAOXIESH_IN()
        {
            CAIGOUDDMX = new List<CAIGOUDDSHXX>();
        }
    }

    public class YAOXIESH_OUT : MessageOUT
    {

    }
    public class CAIGOUDDSHXX
    {
        /// <summary>
        /// 配送明细编号
        /// </summary>
        public string PEISONGMXBH { get; set; }
        /// <summary>
        /// 收货数量
        /// </summary>
        public string SHOUHUOSL { get; set; }
        /// <summary>
        /// 自定义收货信息
        /// </summary>
        public string ZIDINGYSHXX { get; set; }
    }

}
