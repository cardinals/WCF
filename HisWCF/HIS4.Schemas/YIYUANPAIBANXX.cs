using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class YIYUANPAIBANXX_IN : MessageIn
    {
        /// <summary>
        /// 挂号方式
        /// </summary>
        public string GUAHAOFS { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string RIQI { get; set; }
        /// <summary>
        /// 挂号班次
        /// </summary>
        public string GUAHAOBC { get; set; }
        /// <summary>
        /// 科室代码
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 医生代码
        /// </summary>
        public string YISHENGDM { get; set; }
        /// <summary>
        /// 挂号类别
        /// </summary>
        public string GUAHAOLB { get; set; }
        /// <summary>
        /// 预约类型
        /// </summary>
        public string YUYUELX { get; set; }
    }

    public class YIYUANPAIBANXX_OUT : MessageOUT
    {

        /// <summary>
        /// 排班信息
        /// </summary>
        public List<PAIBANXX> PAIBANMX { get; set; }

        public YIYUANPAIBANXX_OUT() {
            this.PAIBANMX = new List<PAIBANXX>();
        }
    }
}
