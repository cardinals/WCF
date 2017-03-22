using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;


namespace HIS4.Schemas
{
    public class GUAHAOCL_IN : MessageIn
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
        /// 一周排班ID
        /// </summary>
        public string YIZHOUPBID { get; set; }
        /// <summary>
        /// 当天排班ID
        /// </summary>
        public string DANGTIANPBID { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string RIQI { get; set; }
        /// <summary>
        /// 挂号班次
        /// </summary>
        public string GUAHAOBC { get; set; }
        /// <summary>
        /// 挂号类别
        /// </summary>
        public string GUAHAOLB { get; set; }
        /// <summary>
        /// 科室代码
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 医生代码
        /// </summary>
        public string YISHENGDM { get; set; }
        /// <summary>
        /// 挂号序号
        /// </summary>
        public string GUAHAOXH { get; set; }
        /// <summary>
        /// 挂号ID
        /// </summary>
        public string GUAHAOID { get; set; }
        /// <summary>
        /// 代收费用
        /// </summary>
        public string DAISHOUFY { get; set; }
        /// <summary>
        /// 预约来源
        /// </summary>
        public string YUYUELY { get; set; }
        /// <summary>
        /// 病历本号
        /// </summary>
        public string BINGLIBH { get; set; }
        /// <summary>
        /// 支付明细
        /// </summary>
        public List<ZHIFUXX> ZHIFUMX { get; set; }
        /// <summary>
        /// 重复交易信息
        /// </summary>
        public List<CHONGFUJYXX> CHONGFUJYMX { get; set; }
        /// <summary>
        /// HIS病人信息
        /// </summary>
        public string HISBRXX { get; set; }
        /// <summary>
        /// 结算ID
        /// </summary>
        public string JIESUANID { get; set; }

        public GUAHAOCL_IN() {
            this.ZHIFUMX = new List<ZHIFUXX>();
            this.CHONGFUJYMX = new List<CHONGFUJYXX>();
        }
    }

    public class GUAHAOCL_OUT : MessageOUT
    {
        /// <summary>
        /// 挂号ID
        /// </summary>
        public string GUAHAOID { get; set; }
        /// <summary>
        /// 挂号序号
        /// </summary>
        public string GUAHAOXH { get; set; }
        /// <summary>
        /// 就诊时间
        /// </summary>
        public string JIUZHENSJ { get; set; }
        /// <summary>
        /// 就诊地点
        /// </summary>
        public string JIUZHENDD { get; set; }
        /// <summary>
        /// 费用明细
        /// </summary>
        public List<FEIYONGXX> FEIYONGMX { get; set; }
        /// <summary>
        /// 结算结果
        /// </summary>
        public JIESUANJG JIESUANJG { get; set; }
        /// <summary>
        /// 详细结算结果
        /// </summary>
        public List<XIANGXIJSJGXX> XIANGXIJSJG { get; set; }
        /// <summary>
        /// 重复交易信息
        /// </summary>
        public List<CHONGFUJYXX> CHONGFUJYMX { get; set; }
        /// <summary>
        /// 结算ID
        /// </summary>
        public string JIESUANID { get; set; }
        /// <summary>
        /// 医技ID
        /// </summary>
        public string YIJIID { get; set; }
        /// <summary>
        /// 挂号费
        /// </summary>
        public string GUAHAOFEI { get; set; }
        /// <summary>
        /// 诊疗费
        /// </summary>
        public string ZHENLIAOFEI { get; set; }
        /// <summary>
        /// 候诊时间
        /// </summary>
        public string HOUZHENSJ { get; set; }

        public GUAHAOCL_OUT() {
            this.JIESUANJG = new JIESUANJG();
            this.XIANGXIJSJG = new List<XIANGXIJSJGXX>();
            this.CHONGFUJYMX = new List<CHONGFUJYXX>();
            this.FEIYONGMX = new List<FEIYONGXX>();
        }
    }
}
