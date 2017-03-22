using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class PAIBANTZXX_IN :MessageIn
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public string KAISHIRQ { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string JIESHURQ { get; set; }
       
    }

    public class PAIBANTZXX_OUT : MessageOUT
    {
       public  List<TINGZHENXX> TINGZHENMX { get; set; }

       public PAIBANTZXX_OUT() {
           this.TINGZHENMX = new List<TINGZHENXX>();
       }
    }

    public class TINGZHENXX
    {
        public string TINGZHENKSRQ { get; set; } //停诊开始日期
        public string TINGZHENJSRQ { get; set; } //停诊结束日期
        public string YIZHOUPBID { get; set; }   //一周排版id
        public string DANGTIANPBID { get; set; } //当天排版id
        public string SHANGWUTZZT { get; set; }  //上午停诊状态
        public string XIAWUTZZT { get; set; }    //下午停诊状态
        public string GUAHAOLB { get; set; }     //挂号类别
        public string KESHIDM { get; set; }      //科室代码
        public string YISHENGDM { get; set; }    //医生代码
        public string KESHIMC { get; set; }
        public string YISHENGXM { get; set; }
    }
}
