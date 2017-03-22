using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class GUAHAOYCL_OUT : BaseOutEntity
    {
        public string GUAHAOID { get; set; }
        public string GUAHAOXH { get; set; }
        public string JIUZHENSJ { get; set; }
        public string JIUZHENDD { get; set; }
        public JIESUANJG JIESUANJG { get; set; }
        public IList<FEIYONGXX> FEIYONGMX { get; set; }
    }
}