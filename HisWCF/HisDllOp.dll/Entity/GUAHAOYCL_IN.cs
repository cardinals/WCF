using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class GUAHAOYCL_IN : BaseInEntity
    {
        public int? JIUZHENKLX { get; set; }
        public string JIUZHENKH { get; set; }
        public int? BINGRENLB { get; set; }
        public string BINGRENXZ { get; set; }
        public string YIBAOKLX { get; set; }
        public string YIBAOKMM { get; set; }
        public string YIBAOKXX { get; set; }
        public string YIBAOBRXX { get; set; }
        public string YILIAOLB { get; set; }
        public string JIESUANLB { get; set; }
        public string YIZHOUPBID { get; set; }
        public string DANGTIANPBID { get; set; }
        public DateTime? RIQI { get; set; }
        public int? GUAHAOBC { get; set; }
        public int? GUAHAOLB { get; set; }
        public string KESHIDM { get; set; }
        public string YISHENGDM { get; set; }
        public string GUAHAOXH { get; set; }
        public string GUAHAOID { get; set; }
        public int? DAISHOUFY { get; set; }
        public string YUYUELY { get; set; }
        public string BINGLIBH { get; set; }
    }
}