using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 入参基本参数
    /// </summary>
    public class BASEINFO
    {
        /// <summary>
        /// 操作员代码
        /// </summary>
        public string CAOZUOYDM { get; set; }
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string CAOZUOYXM { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public string CAOZUORQ { get; set; }
        /// <summary>
        /// 系统标识
        /// </summary>
        public string XITONGBS { get; set; }
        /// <summary>
        /// 分院代码
        /// </summary>
        public string FENYUANDM { get; set; }
        /// <summary>
        /// 终端机编号
        /// </summary>
        public string ZHONGDUANJBH { get; set; }
        /// <summary>
        /// 终端流水号
        /// </summary>
        public string ZHONGDUANLSH { get; set; }
        /// <summary>
        /// 发送方报文
        /// </summary>
        public string MSGNO { get; set; }
        /// <summary>
        /// 机构代码
        /// </summary>
        public string JIGOUDM { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        public string MACDZ { get; set; }
    }
}
