using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class RENYUANZC_IN :MessageIn
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
        /// 医保卡类型
        /// </summary>
        public string YIBAOKLX { get; set; }
        /// <summary>
        /// 医保卡信息
        /// </summary>
        public string YIBAOKXX { get; set; }
        /// <summary>
        /// 医保卡号
        /// </summary>
        public string YIBAOKH { get; set; }
        /// <summary>
        /// 个人编号
        /// </summary>
        public string GERENBH { get; set; }
        /// <summary>
        /// 病历本号
        /// </summary>
        public string BINGLIBH { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string XINGBIE { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string MINZU { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string CHUSHENGRQ { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZHENGJIANLX { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public string DANWEILX { get; set; }
        /// <summary>
        /// 单位编号
        /// </summary>
        public string DANWEIBH { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string DANWEIMC { get; set; }
        /// <summary>
        /// 家庭住址
        /// </summary>
        public string JIATINGZZ { get; set; }
        /// <summary>
        /// 人员类别
        /// </summary>
        public string RENYUANLB { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string LIANXIDH { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string YINHANGKH { get; set; }
        /// <summary>
        /// 签约标志
        /// </summary>
        public string QIANYUEBZ { get; set; }
        /// <summary>
        /// 医疗类别
        /// </summary>
        public string YILIAOLB { get; set; }
        /// <summary>
        /// 结算类别
        /// </summary>
        public string JIESUANLB { get; set; }
        /// <summary>
        /// 医保卡密码
        /// </summary>
        public string YIBAOKMM { get; set; }
        /// <summary>
        /// 照片
        /// </summary>
        public string PHOTO { get; set; }
        /// <summary>
        /// 是否有卡
        /// </summary>
        public string SHIFOUYK { get; set; }
        /// <summary>
        /// 绑定银行卡
        /// </summary>
        public string BANGDINGYHK { get; set; }
        /// <summary>
        /// 终端设备信息
        /// </summary>
        public string ZHONGDUANSBXX { get; set; }
        /// <summary>
        /// 病人性质
        /// </summary>
        public string BINGRENXZ { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public string CHONGZHIJE { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string ZHIFUFS { get; set; }
        /// <summary>
        /// empiID 秀洲区 实名制认证标识字段
        /// </summary>
        public string EMPIID { get; set; }
    }

    public class RENYUANZC_OUT : MessageOUT {
       
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 虚拟账户
        /// </summary>
        public string XUNIZH{ get; set; }
        /// <summary>
        /// 病人id
        /// </summary>
        public string BINGRENID { get; set; }
    }
}
