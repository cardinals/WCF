using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class KESHIXX : BaseGroup
    {
        public string KESHIDM { get; set; }
        public string KESHIMC { get; set; }
        public string JIUZHENDD { get; set; }
        public string KESHIJS { get; set; }
    }
}