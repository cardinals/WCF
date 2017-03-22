using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class HAOYUANXX
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string RIQI { get; set; }
        /// <summary>
        /// 挂号班次
        /// </summary>
        public string GUAHAOBC { get; set; }
        /// <summary>
        /// 挂号类别
        /// </summary>
        public string GUAHAOLB { get; set; }
        /// <summary>
        /// 科室代码
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 医生代码
        /// </summary>
        public string YISHENGDM { get; set; }
        /// <summary>
        /// 挂号序号
        /// </summary>
        public string GUAHAOXH { get; set; }
        /// <summary>
        /// 就诊时间
        /// </summary>
        public string JIUZHENSJ { get; set; }
        /// <summary>
        /// 一周排班ID
        /// </summary>
        public string YIZHOUPBID { get; set; }
        /// <summary>
        /// 当天排班ID
        /// </summary>
        public string DANGTIANPBID { get; set; }
        /// <summary>
        /// 挂号费用
        /// </summary>
        public string GUAHAOFY { get; set; }
        /// <summary>
        /// 诊疗费用
        /// </summary>
        public string ZHENLIAOFY { get; set; }
        /// <summary>
        /// 剩余号源
        /// </summary>
        public string SHENGYUHYS { get; set; }
        /// <summary>
        /// 预约类型
        /// </summary>
        public string YUYUELX { get; set; }

        public HAOYUANXX() {
            this.DANGTIANPBID = string.Empty;
            this.GUAHAOBC = string.Empty;
            this.GUAHAOLB = string.Empty;
            this.GUAHAOXH = string.Empty;
            this.JIUZHENSJ = string.Empty;
            this.KESHIDM = string.Empty;
            this.RIQI = string.Empty;
            this.YISHENGDM = string.Empty;
            this.YIZHOUPBID = string.Empty;
            this.GUAHAOFY = string.Empty;
            this.ZHENLIAOFY = string.Empty;
        }
    }
}
