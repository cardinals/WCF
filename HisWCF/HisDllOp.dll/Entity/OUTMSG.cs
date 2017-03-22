using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class OUTMSG : BaseGroup
    {
        public string ERRNO { get; set; }
        public string ERRMSG { get; set; }
        public string ZHONGDUANJBH { get; set; }
        public string ZHONGDUANLS { get; set; }
    }
}