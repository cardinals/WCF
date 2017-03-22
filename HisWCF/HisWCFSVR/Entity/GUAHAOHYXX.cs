using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HisWCFSVR.Entity
{
    public class GUAHAOHYXX
    {
    }
    public class Body_GUAHAOHYXX
    {
        public Result Result { get; set; }
        public List<HAOYUANXX> HAOYUANMX { get; set; }

    }
    public class HAOYUANXX
    {

        public string HAOYUANID { get; set; }
        public string PAIBANRQ { get; set; }
        public string GUAHAOBC { get; set; }
        public string GUAHAOLB { get; set; }
        public string YISHENGDM { get; set; }
        public string KESHIDM { get; set; }
        public string GUAHAOXH { get; set; }
        public string JIUZHENSJ { get; set; }
        public string YIZHOUPBID { get; set; }
        public string DANGTIANPBID { get; set; }
        public string HAOYUANLB { get; set; }
    }
}