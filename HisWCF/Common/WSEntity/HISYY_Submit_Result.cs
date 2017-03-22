using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WSEntity
{
    public class HISYY_Submit_Result : WSEntityResponse
    {
        public string Success { get; set; }
        public string Message { get; set; }
        public string YYH { get; set; }
        public string JCH { get; set; }
        public string PDH { get; set; }
    }
}
