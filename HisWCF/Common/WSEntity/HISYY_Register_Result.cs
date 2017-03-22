using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WSEntity
{
    public class HISYY_Register_Result : WSEntityResponse
    {
        public string Success { get; set; }//	是否成功
        public string Message{ get; set; }//	返回消息
        public string YYH{ get; set; }//	预约号
        public string JCH{ get; set; }//	检查号
        public string PDH { get; set; }//	排队号
    }
}
