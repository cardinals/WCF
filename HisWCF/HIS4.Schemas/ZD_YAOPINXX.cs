using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZD_YAOPINXX_IN : MessageIn 
    {
        /// <summary>
        /// 项目归类
        /// </summary>
        public string XIANGMUGL { get; set; }
        /// <summary>
        /// 输入码类型
        /// </summary>
        public string SHURUMLX { get; set; }
        /// <summary>
        /// 输入码
        /// </summary>
        public string SHURUM { get; set; }
        /// <summary>
        /// 门诊住院启动 0：全部 1：门诊 1：住院
        /// </summary>
        public string MENZHENZYQY { get; set; }
    }

    public class ZD_YAOPINXX_OUT : MessageOUT 
    {
        /// <summary>
        /// 药品明细
        /// </summary>
        public List<YAOPINXX> YAOPINMX { get; set; }

        public ZD_YAOPINXX_OUT() {
            this.YAOPINMX = new List<YAOPINXX>();
        }

    }
}
