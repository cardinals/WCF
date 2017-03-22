using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class PAIDUIJHXX
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string RIQI { get; set; }
        /// <summary>
        /// 上下午标识
        /// </summary>
        public string SHANGXIAWBZ { get; set; }
        /// <summary>
        /// 排队编号
        /// </summary>
        public string PAIDUIID { get; set; }
        /// <summary>
        /// 科室代码
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string KESHIMC { get; set; }
        /// <summary>
        /// 医生代码
        /// </summary>
        public string YISHENGDM { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        public string YISHENGMC { get; set; }
        /// <summary>
        /// 当前号码
        /// </summary>
        public string DANGQIANHM { get; set; }
        /// <summary>
        /// 病人序号
        /// </summary>
        public string BINGRENXH { get; set; }
        /// <summary>
        /// 病人状态
        /// </summary>
        public string BINGRENZT { get; set; }
        /// <summary>
        /// 就诊位置
        /// </summary>
        public string SUOZAIZJ { get; set; }
    }
}
