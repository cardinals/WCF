using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HisWCFSVR.Entity
{
    public class YUYUEGH
    {
    }
    public class Body_YUYUEGH
    {
        public Result Result { get; set; }
        public GUAHAOYY GUAHAOYY { get; set; }
        public List<FEIYONGXX> FEIYONGMX { get; set; }

    }
    public class GUAHAOYY
    {
        public string YUYUEID { get; set; }
        public string QUHAOMM { get; set; }
        public string JIUZHENSJ { get; set; }
        public string GUAHAOXH { get; set; }
    }
    public class FEIYONGXX
    {
        public string XIANGMUXH { get; set; }
        public string XIANGMUMC { get; set; }
        public string FEIYONGLX { get; set; }
        public string DANJIA { get; set; }
        public string XIANGMUDW { get; set; }
        public string SHULIANG { get; set; }
        public string JINE { get; set; }
        public string YIBAODJ { get; set; }
        public string YIBAODM { get; set; }
        public string YIBAOZFBL { get; set; }
    }
}