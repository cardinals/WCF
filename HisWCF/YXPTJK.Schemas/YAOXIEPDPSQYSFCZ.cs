using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 判断配送企业是否存在
    /// </summary>
    public class YAOXIEPDPSQYSFCZ_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 配送企业明细
        /// </summary>
        public List<PEISONGQYXX> PEISONGQYMX { get; set; }

        public YAOXIEPDPSQYSFCZ_IN()
        {
            PEISONGQYMX = new List<PEISONGQYXX>();
        }
    }

    public class YAOXIEPDPSQYSFCZ_OUT : MessageOUT
    {
        /// <summary>
        /// 商品返回明细
        /// </summary>
        public string PEISONGQYFHMX { get; set; }
    }
    public class PEISONGQYXX
    {
        /// <summary>
        /// 配送企业编号
        /// </summary>
        public string PEISONGQYBH { get; set; }


    }

}
