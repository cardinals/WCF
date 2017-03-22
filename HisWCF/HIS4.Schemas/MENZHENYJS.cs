using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class MENZHENYJS_IN : MessageIn
    {
        /// <summary>
        /// 就诊卡类型
        /// </summary>
        public string JIUZHENKLX { get; set; }
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 病人类别
        /// </summary>
        public string BINGRENLB { get; set; }
        /// <summary>
        /// 病人性质
        /// </summary>
        public string BINGRENXZ { get; set; }
        /// <summary>
        /// 医保卡类型
        /// </summary>
        public string YIBAOKLX { get; set; }
        /// <summary>
        /// 医保卡密码
        /// </summary>
        public string YIBAOKMM { get; set; }
        /// <summary>
        /// 医保卡信息
        /// </summary>
        public string YIBAOKXX { get; set; }
        /// <summary>
        /// 医保病人信息
        /// </summary>
        public string YIBAOBRXX { get; set; }
        /// <summary>
        /// 医疗类别
        /// </summary>
        public string YILIAOLB { get; set; }
        /// <summary>
        /// 结算类别
        /// </summary>
        public string JIESUANLB { get; set; }
        /// <summary>
        /// 疾病明细信息
        /// </summary>
        public List<JIBINGXX> JIBINGMX { get; set; }
        /// <summary>
        /// 费用明细条数
        /// </summary>
        public long FEIYONGMXTS { get; set; }
        /// <summary>
        /// 费用明细
        /// </summary>
        public List<MENZHENFYXX> FEIYONGMX { get; set; }
        /// <summary>
        /// 重复交易明细
        /// </summary>
        public List<CHONGFUJYXX> CHONGFUJYMX { get; set; }
        /// <summary>
        /// HIS病人信息
        /// </summary>
        public string HISBRXX { get; set; }
    }


    public class MENZHENYJS_OUT : MessageOUT {
        /// <summary>
        /// 结算ID
        /// </summary>
        public string JIESUANID { get; set; }
        /// <summary>
        /// 结算结果
        /// </summary>
        public JIESUANJG JIESUANJG { get; set; }
        /// <summary>
        /// 详细结算结果
        /// </summary>
        public List<XIANGXIJSJGXX> XIANGXIJSJG { get; set; }
        /// <summary>
        /// 费用自负明细
        /// </summary>
        public List<MENZHENFYZFXX> FEIYONGZFMX { get; set; }
        /// <summary>
        /// 重复交易明细
        /// </summary>
        public List<CHONGFUJYXX> CHONGFUJYMX { get; set; }
    }
}
