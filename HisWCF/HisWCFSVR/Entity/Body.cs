using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HisWCFSVR.Entity
{
    public class Body
    {

        /// <summary>
        ///  医疗机构代码  VA12 Y   预约平台分配的医疗机构编码
        /// </summary>

        public string YILIAOJGDM { get; set; }

        /// <summary>
        ///  科室代码 VA50    N
        /// </summary>
        public string KESHIDM { get; set; }


        /// <summary>
        /// 医生代码	VA50	N	空值时返回全部
        /// </summary>
        public string YISHENGDM { get; set; }


        public string PAIBANKSRQ { get; set; }
        public string PAIBANJSRQ { get; set; }
        public string GUAHAOBC { get; set; }
        public string GUAHAOLB { get; set; }

        public string PAIBANRQ { get; set; }
        public string GUAHAOFS { get; set; }

        public string HAOYUANLB { get; set; }
        public YUYUEXX YUYUEXX { get; set; }

        public string JIUZHENKLX { get; set; }
        public string JIUZHENKH { get; set; }
        public string ZHENGJIANLX { get; set; }
        public string ZHENGJIANHM { get; set; }
        public string XINGMING { get; set; }
        public string YUYUEID { get; set; }
        public string GUAHAOXH { get; set; }
        public string RIQI { get; set; }
        public string QUHAOMM { get; set; }
        public string YUYUELY { get; set; }
        public  string GUOMINYW { get; set; }
        public  string BINGQINGMS { get; set; }
    }
    public class YUYUEXX
    {
        public string YILIAOJGDM { get; set; }
        public string JIUZHENKLX { get; set; }
        public string JIUZHENKH { get; set; }
        public string ZHENGJIANLX { get; set; }
        public string ZHENGJIANHM { get; set; }
        public string XINGMING { get; set; }
        public string XINGBIE { get; set; }
        public string CHUSHENGRQ { get; set; }
        public string YIZHOUPBID { get; set; }
        public string DANGTIANPBID { get; set; }
        public string PAIBANRQ { get; set; }
        public string GUAHAOXH { get; set; }
        public string HAOYUANID { get; set; }
        public string GUAHAOBC { get; set; }
        public string GUAHAOLB { get; set; }
        public string KESHIDM { get; set; }
        public string YISHENGDM { get; set; }
        public string YUYUELY { get; set; }
        public string LIANXIDH { get; set; }
    }
}