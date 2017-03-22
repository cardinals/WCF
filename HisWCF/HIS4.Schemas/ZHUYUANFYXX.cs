using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZHUYUANFYXX_IN : MessageIn
    {
        /// <summary>
        /// 病人唯一号
        /// </summary>
        public string BINGRENZYID { get; set; }
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
        /// HIS病人信息
        /// </summary>
        public string HISBRXX { get; set; }
        /// <summary>
        /// 校验密码
        /// </summary>
        public string JIAOYANMM { get; set; }
        /// <summary>
        /// 在院状态
        /// </summary>
        public string ZAIYUANZT { get; set; }
    }

    public class ZHUYUANFYXX_OUT : MessageOUT
    {
        /// <summary>
        /// 病人住院ID
        /// </summary>
        public string BINGRENZYID { get; set; }
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
        /// 费用归类
        /// </summary>
        public List<FEIYONGGLXX> FEIYONGGLMX { get; set; }
        /// <summary>
        /// 预交款信息
        /// </summary>
        public List<YUJIAOKXX> YUJIAOKMX { get; set; }

        public ZHUYUANFYXX_OUT() {
            this.JIESUANJG = new JIESUANJG();
            this.XIANGXIJSJG = new List<XIANGXIJSJGXX>();
            this.FEIYONGZFMX = new List<MENZHENFYZFXX>();
            this.FEIYONGGLMX = new List<FEIYONGGLXX>();
            this.YUJIAOKMX = new List<YUJIAOKXX>();
        
        }
    }
}
