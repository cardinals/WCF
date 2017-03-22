using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WSEntity
{
    public class HISYY_GetResource_Result : WSEntityResponse
    {
        public string Success { get; set; }
        public string Message { get; set; }
        public string DeviceCode { get; set; }
        public string DeviceName { get; set; }
        public string DeviceLocation { get; set; }
        public string ExaminePartTime { get; set; }
        public string BespeakDatePart { get; set; }
    }
}
