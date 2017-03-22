using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class GUAHAOTH_IN : BaseInEntity
    {
        public int? JIUZHENKLX { get; set; }
        public string JIUZHENKH { get; set; }
        public string XINGMING { get; set; }
        public string GUAHAOID { get; set; }
        public int? TUIHAOLX { get; set; }
    }
}