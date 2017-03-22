using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class GUAHAOYY_OUT : BaseOutEntity
    {
        public string QUHAOMM { get; set; }
        public string GUAHAOXH { get; set; }
        public string JIUZHENSJ { get; set; }
        public IList<FEIYONGXX> FEIYONGMX { get; set; }
    }
}