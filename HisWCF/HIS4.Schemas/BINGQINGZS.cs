using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
namespace HIS4.Schemas
{/// <summary>
///  病情自述 　小宝
/// </summary>
    public class BINGQINGZS_IN: MessageIn
    {
        /// <summary>
        /// 医疗机构代码
        /// </summary>
        public string YILIAOJGDM { get; set; }
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
        /// 预约ID
        /// </summary>
        public string YUYUEID { get; set; }
        /// <summary>
        /// 挂号序号
        /// </summary>
        public string GUAHAOXH { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string RIQI { get; set; }
        /// <summary>
        /// 预约来源
        /// </summary>
        public string YUYUELY { get; set; }
        /// <summary>
        /// 过敏药物描述
        /// </summary>
        public string GUOMINYW { get; set; }
        /// <summary>
        /// 病情描述
        /// </summary>
        public string BINGQINGMS { get; set; }
    }
}
