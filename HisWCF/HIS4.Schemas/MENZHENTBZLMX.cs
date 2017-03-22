using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class MENZHENTBZLMX_IN : MessageIn
    {
        /// <summary>
        /// 病人ID
        /// </summary>
        public string BINGRENID { get; set; }
    }

    public class MENZHENTBZLMX_OUT : MessageOUT
    {
        public List<TEBINGZLXX> TEBINGZLMX { get; set; }

        public MENZHENTBZLMX_OUT() {
            this.TEBINGZLMX = new List<TEBINGZLXX>();
        }
    }


    public class TEBINGZLXX {
        /// <summary>
        /// 诊疗类型 1 处方 2 医技
        /// </summary>
        public string ZHENLIAOLX { get; set; }
        /// <summary>
        /// 诊疗ID  类型1 处方id 类型2 医技id
        /// </summary>
        public string ZHENLIAOID { get; set; }
        /// <summary>
        /// 病人id
        /// </summary>
        public string BINGRENID { get; set; }
        /// <summary>
        /// 属性状态 0 普通 1 特病
        /// </summary>
        public string SHUXINGZT { get; set; }
    }
}
