using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class JIANYANJGXX
    {
        /// <summary>
        /// 结果项目代码
        /// </summary>
        public string JIEGUOXMDM { get; set; }
        /// <summary>
        /// 结果项目名称
        /// </summary>
        public string JIEGUOXMMC { get; set; }
        /// <summary>
        /// 结果值
        /// </summary>
        public string JIEGUOZHI { get; set; }
        /// <summary>
        /// 描述单位
        /// </summary>
        public string MIAOSHUDW { get; set; }
        /// <summary>
        /// 参考值上限
        /// </summary>
        public string CANKAOZSX { get; set; }
        /// <summary>
        /// 参考值下限
        /// </summary>
        public string CANKAOZXX { get; set; }
        /// <summary>
        /// 结果 偏高 偏低 正常
        /// </summary>
        public string JIEGUO { get; set; }
    }
}
