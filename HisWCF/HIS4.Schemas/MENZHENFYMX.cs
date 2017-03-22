using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class MENZHENFYMX_IN : MessageIn
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
        /// HIS病人信息
        /// </summary>
        public string HISBRXX { get; set; }
        /// <summary>
        /// 查询方式 1 通过医技和处方id查找费用记录
        /// </summary>
        public string CHAXUNFS { get; set; }
        /// <summary>
        /// 医技ID
        /// </summary>
        public string YIJIID { get; set; }
        /// <summary>
        /// 处方ID
        /// </summary>
        public string CHUFANGID { get; set; }
        /// <summary>
        /// 病人ID
        /// </summary>
        public string BINGRENID { get; set; }
    }

    public class MENZHENFYMX_OUT : MessageOUT {
        /// <summary>
        /// 医疗类别
        /// </summary>
        public string YILIAOLB { get; set; }
        /// <summary>
        /// 疾病明细
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


        public MENZHENFYMX_OUT() {
            this.FEIYONGMX = new List<MENZHENFYXX>();
            this.JIBINGMX = new List<JIBINGXX>();
        }

    }
}
