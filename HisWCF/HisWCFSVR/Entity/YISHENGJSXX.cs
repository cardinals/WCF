using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HisWCFSVR.Entity
{
    public class YISHENGJSXX
    {

    }
    public class Body_YISHENGJSXX
    {
        public Result Result { get; set; }
        public List<YISHENGXX> YISHENGMX { get; set; }

    }
    public class YISHENGXX
    {
        public string YISHENGDM { get; set; }
        public string YISHENGXM { get; set; }
        public string YISHENGXB { get; set; }
        public string ZHENGJIANLX { get; set; }
        public string ZHENGJIANHM { get; set; }
        public string YISHENGPX { get; set; }
        public string YISHENGZC { get; set; }
        public string YISHENGTC { get; set; }
        public string YISHENGJS { get; set; }
        public string ZHAOPIAN { get; set; }
    }

}