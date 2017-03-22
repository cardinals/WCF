using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WSEntity
{
    public class HISYY_Cancel : WSEntityRequest
    {
        public string RequestNo { get; set; }
        public string YYH { get; set; }
        public string JCH { get; set; }
    }
}
