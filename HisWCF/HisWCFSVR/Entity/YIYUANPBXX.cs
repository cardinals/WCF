using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HisWCFSVR.Entity
{
    public class YIYUANPBXX
    {

    }
    public class Body_YIYUANPBXX
    {
        public Result Result { get; set; }
        public List<PAIBANMX> PAIBANLB { get; set; }
    }
    public class PAIBANMX
    {
        public string HAOYUANFPLX { get; set; }
        public string KESHIDM { get; set; }
        public string KESHIMC { get; set; }
        public string JIUZHENDD { get; set; }
        public string KESHIJS { get; set; }
        public string KESHIPX { get; set; }
        public string YISHENGDM { get; set; }
        public string YISHENGXM { get; set; }
        public string ZHENLIAOJSF { get; set; }
        public string ZHENLIAOF { get; set; }
        public string PAIBANRQ { get; set; }
        public string GUAHAOBC { get; set; }
        public string GUAHAOLB { get; set; }
        public string TINGZHENBZ { get; set; }
        public string YIZHOUPBID { get; set; }
        public string DANGTIANPBID { get; set; }
    }
}