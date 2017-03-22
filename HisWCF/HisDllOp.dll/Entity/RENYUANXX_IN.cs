using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class RENYUANXX_IN : BaseInEntity
    {
        public int? JIUZHENKLX { get; set; }
        public string JIUZHENKH { get; set; }
        public int? BINGRENLB { get; set; }
        public string YIBAOKLX { get; set; }
        public string YIBAOKMM { get; set; }
        public string YIBAOKXX { get; set; }
        public string YILIAOLB { get; set; }
        public string JIESUANLB { get; set; }
        public DateTime? JIUZHENRQ { get; set; }
        public string QIANFEIKZ { get; set; }
     
    }
}