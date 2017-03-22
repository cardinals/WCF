using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class OUTMSG
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ERRNO { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ERRMSG { get; set; }
        /// <summary>
        /// 错误信息源
        /// </summary>
        public string ERRMSGEX { get; set; }
        /// <summary>
        /// 终端流水号
        /// </summary>
        public string ZHONGDUANLSH { get; set; }
        /// <summary>
        /// 终端机编号
        /// </summary>
        public string ZHONGDUANJBH { get; set; }
        /// <summary>
        /// 发送方报文
        /// </summary>
        public string MSGNO { get; set; }
        /// <summary>
        /// 接收方报文GUID
        /// </summary>
        public string MessageID { get; set; }
    }
}
