using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class GUAHAOYY_IN : BaseInEntity
    {
        public int? JIUZHENKLX { get; set; }
        public string JIUZHENKH { get; set; }
        public string ZHENGJIANLX { get; set; }
        public string ZHENGJIANHM { get; set; }
        public string XINGMING { get; set; }
        public string YIZHOUPBID { get; set; }
        public string DANGTIANPBID { get; set; }
        public DateTime? RIQI { get; set; }
        public int? GUAHAOBC { get; set; }
        public int? GUAHAOLB { get; set; }
        public string KESHIDM { get; set; }
        public string YISHENGDM { get; set; }
        public string GUAHAOXH { get; set; }
        public string YUYUELY { get; set; }
    }
}