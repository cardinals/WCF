using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class YISHENGXX : BaseGroup
    {
        public string YISHENGDM { get; set; }
        public string YISHENGXM { get; set; }
        public string KESHIDM { get; set; }
        public string KESHIMC { get; set; }
        public string YISHENGZC { get; set; }
        public string YISHENGTC { get; set; }
        public string YISHENGJS { get; set; }
    }
}