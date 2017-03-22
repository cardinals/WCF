using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class YISHENGXX
    {
        /// <summary>
        /// 医生代码
        /// </summary>
        public string YISHENGDM { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string YISHENGXM { get; set; }
        /// <summary>
        /// 科室代码
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string KESHIMC { get; set; }
        /// <summary>
        /// 医生职称
        /// </summary>
        public string YISHENGZC { get; set; }
        /// <summary>
        /// 医生特长
        /// </summary>
        public string YISHENGTC { get; set; }
        /// <summary>
        /// 医生介绍
        /// </summary>
        public string YISHENGJS { get; set; }
        /// <summary>
        /// 挂号类别
        /// </summary>
        public string GUAHAOLB { get; set; }
        /// <summary>
        /// 挂号方式 1 挂号 2 预约
        /// </summary>
        public string GUAHAOFS { get; set; }
        /// <summary>
        /// 预约类型
        /// </summary>
        public string YUYUELX { get; set; }


        public YISHENGXX() {
            this.GUAHAOFS = string.Empty;
            this.GUAHAOLB = string.Empty;
            this.KESHIDM = string.Empty;
            this.KESHIMC = string.Empty;
            this.YISHENGDM = string.Empty;
            this.YISHENGJS = string.Empty;
            this.YISHENGTC = string.Empty;
            this.YISHENGXM = string.Empty;
            this.YISHENGZC = string.Empty;
            this.YUYUELX = string.Empty;
        }
    }
}
