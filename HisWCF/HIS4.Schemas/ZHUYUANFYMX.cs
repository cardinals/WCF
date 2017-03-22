using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZHUYUANFYMX_IN : MessageIn
    {
        /// <summary>
        /// 病人唯一号
        /// </summary>
        public string BINGRENZYID { get; set; }
        /// <summary>
        /// 开始日期 默认入院开始日期 （YYYY-MM-DD）
        /// </summary>
        public string KAISHIRQ { get; set; }
        /// <summary>
        /// 结束日期 默认出院日期 （YYYY-MM-DD）
        /// </summary>
        public string JIESHURQ { get; set; }
        /// <summary>
        /// 校验密码 身份证号后6位
        /// </summary>
        public string JIAOYANMM { get; set; }
        /// <summary>
        /// 统计方式 1：日清单；2：汇总清单
        /// </summary>
        public string TONGJIFS { get; set; }
        /// <summary>
        /// 统计分类
        /// </summary>
        public string TONGJIFL { get; set; }

    }

    public class ZHUYUANFYMX_OUT : MessageOUT
    {

        public string FEIYONGMXTS { get; set; }

        public List<FEIYONGXX_ZY> FEIYONGMX_ZY { get; set; }

        public ZHUYUANFYMX_OUT(){
            this.FEIYONGMX_ZY = new List<FEIYONGXX_ZY>();
        }
    }
}
