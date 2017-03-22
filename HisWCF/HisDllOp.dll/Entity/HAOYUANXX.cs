using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class HAOYUANXX : BaseGroup
    {
        public string RIQI { get; set; }
        public string GUAHAOBC { get; set; }
        public int? GUAHAOLB { get; set; }
        public string KESHIDM { get; set; }
        public string YISHENGDM { get; set; }
        public string GUAHAOXH { get; set; }
        public string JIUZHENSJ { get; set; }
        public string YIZHOUPBID { get; set; }
        public string DANGTIANPBID { get; set; }
    }
}