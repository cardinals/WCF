using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class YUYUEJLCX_IN :MessageIn
    {
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 就诊卡类型
        /// </summary>
        public string JIUZHENKLX { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string LIANXIDH { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public string YUYUERQ { get; set; }
        /// <summary>
        /// 取号状态
        /// </summary>
        public string QUHAOZT { get; set; }
        
    }

    public class YUYUEJLCX_OUT : MessageOUT {
        /// <summary>
        /// 预约明细
        /// </summary>
        public List<YUYUEXX> YUYUEMX { get; set; }

        public YUYUEJLCX_OUT() {
            this.YUYUEMX = new List<YUYUEXX>();
        }
    }
}
