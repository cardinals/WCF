using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class GUAHAOXXCX_IN :MessageIn
    {
        /// <summary>
        /// 就诊卡类型
        /// </summary>
        public string JIUZHENKLX { get; set; }
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZHENGJIANLX { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
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
        /// 病人ID
        /// </summary>
        public string BINGRENID { get; set; }

    }

    public class GUAHAOXXCX_OUT : MessageOUT
    {
        /// <summary>
        /// 挂号信息列表
        /// </summary>
        public List<GUAHAOXX> GUAHAOXXLB { get; set; }

        public GUAHAOXXCX_OUT() {
            this.GUAHAOXXLB = new List<GUAHAOXX>();
        }
    }
}
