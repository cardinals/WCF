using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 判断商品是否存在
    /// </summary>
    public class YAOXIEPDYPSFCZ_IN : MessageIn
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string JIEKOUDYPJ { get; set; }
        /// <summary>
        /// 商品明细
        /// </summary>
        public List<SHANGPINSFCZXX> SHANGPINMX { get; set; }

        public YAOXIEPDYPSFCZ_IN()
        {
            SHANGPINMX = new List<SHANGPINSFCZXX>();
        }
    }

    public class YAOXIEPDYPSFCZ_OUT : MessageOUT
    {
        /// <summary>
        /// 商品返回明细
        /// </summary>
        public string SHANGPINFHMX { get; set; }
    }
    public class SHANGPINSFCZXX
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public string SHANGPINBH { get; set; }


    }

}
