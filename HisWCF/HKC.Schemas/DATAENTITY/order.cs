using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKC.Schemas.Start
{
    public class order
    {
        /// <summary>
        /// 配药单号
        /// </summary>
        public string no;
        /// <summary>
        /// 日期
        /// </summary>
        public string odate;
        /// <summary>
        /// 窗口号
        /// </summary>
        public string winno;
    }
}

namespace HKC.Schemas.Ready {
    public class order {
        /// <summary>
        /// 确认完成处方号码
        /// </summary>
        public string orderno;
        /// <summary>
        /// 配药药师工号
        /// </summary>
        public string pyid;
    }
}
