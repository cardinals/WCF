using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKC.Schemas
{
    public class BASECLASS
    {

    }

    public class BASEIN : BASECLASS
    {
        /// <summary>
        /// 药房ID
        /// </summary>
        public string pharmacyid;
        /// <summary>
        /// 业务关键字
        /// </summary>
        public string codekey;
    }

    public class BASEOUT : BASECLASS
    {
        /// <summary>
        /// 响应代码
        /// </summary>
        public string ResultCode;
        /// <summary>
        /// 响应信息
        /// </summary>
        public string ResultContent;
    }
}
