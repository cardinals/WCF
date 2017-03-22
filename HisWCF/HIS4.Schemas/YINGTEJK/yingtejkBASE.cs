using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas.YINGTEJK
{
    public class yingtejkBASE
    {
        /// <summary>
        /// 标识接口操作
        /// </summary>
        public string result { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public errorInfo errorInfo { get; set; }

        public yingtejkBASE() {
            this.errorInfo = new errorInfo();
        }
    }

    public class errorInfo{ 
        /// <summary>
        /// 具体错误数据
        /// </summary>
        public string id{get;set;}
        /// <summary>
        /// 错误编码
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errorMsg { get; set; }
    }
}
