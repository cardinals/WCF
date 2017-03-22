using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WSEntity
{
    public class StudiesExamine : WSEntityGroup
    {
        public string ExamineCode { get; set; }
        public string ExamineName { get; set; }
        public string Numbers { get; set; }
        public string ExaminePrice { get; set; }
    }
}
