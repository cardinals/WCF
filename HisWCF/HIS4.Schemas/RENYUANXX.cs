using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class RENYUANXX_IN:MessageIn
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
        /// 医保卡密码
        /// </summary>
        public string YIBAOKMM { get; set; }
        /// <summary>
        /// 医保卡信息
        /// </summary>
        public string YIBAOKXX { get; set; }
        /// <summary>
        /// 医疗类别
        /// </summary>
        public string YILIAOLB { get; set; }
        /// <summary>
        /// 结算类别
        /// </summary>
        public string JIESUANLB { get; set; }
        /// <summary>
        /// 就诊日期
        /// </summary>
        public string JIUZHENRQ { get; set; }
        /// <summary>
        /// 欠费控制
        /// </summary>
        public string QIANFEIKZ { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string YINHANGKH { get; set; }
        /// <summary>
        /// 重复交易信息
        /// </summary>
        public List<CHONGFUJYXX> CHONGFUJYMX { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 病人ID
        /// </summary>
        public string BINGRENID { get; set; }

        public RENYUANXX_IN() {
            this.CHONGFUJYMX = new List<CHONGFUJYXX>();
        }

    }

    public class RENYUANXX_OUT:MessageOUT
    {
        /// <summary>
        /// 病人ID
        /// </summary>
        public string BINGRENID { get; set; }
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
        /// 家庭地址
        /// </summary>
        public string JIATINGZZ { get; set; }
        /// <summary>
        /// 人员类别
        /// </summary>
        public string RENYUANLB { get; set; }
        /// <summary>
        /// 当年帐户余额
        /// </summary>
        public string DANGNIANZHYE { get; set; }
        /// <summary>
        /// 历年帐户余额
        /// </summary>
        public string LINIANZHYE { get; set; }
        /// <summary>
        /// 特殊病种标志
        /// </summary>
        public string TESHUBZBZ { get; set; }
        /// <summary>
        /// 特殊病种审批编号
        /// </summary>
        public string TESHUBZSPBH { get; set; }
        /// <summary>
        /// 医保病人信息
        /// </summary>
        public string YIBAOBRXX { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string TISHIXX { get; set; }
        /// <summary>
        /// 待遇类别
        /// </summary>
        public string DAIYULB { get; set; }
        /// <summary>
        /// 参保行政代码
        /// </summary>
        public string CANBAOXZDM { get; set; }
        /// <summary>
        /// 特殊待遇类别
        /// </summary>
        public string TESHUDYLB { get; set; }
        /// <summary>
        /// 特殊病种信息
        /// </summary>
        public List<TESHUBZXX> TESHUBZMX { get; set; }
        /// <summary>
        /// 欠费金额 
        /// </summary>
        public string QIANFEIJE { get; set; }
        /// <summary>
        /// 重复交易信息
        /// </summary>
        public List<CHONGFUJYXX> CHONGFUJYMX { get; set; }
        /// <summary>
        /// HIS病人信息
        /// </summary>
        public string HISBRXX { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string LIANXIDH { get; set; }
        /// <summary>
        /// 签约标志
        /// </summary>
        public string QIANYUEBZ { get; set; }
        /// <summary>
        /// 开户状态
        /// </summary>
        public string KAIHUZT { get; set; }
        /// <summary>
        /// 虚拟帐户
        /// </summary>
        public string XUNIZH { get; set; }
        /// <summary>
        /// 挂号黑名单  空或0正常 >0黑名单次数
        /// </summary>
        public string GUAHAOHMD { get; set; }
        /// <summary>
        /// 虚拟账户余额
        /// </summary>
        public string ZHANGHUYE { get; set; }
        /// <summary>
        /// 交易密码
        /// </summary>
        public string JIAOYIMM { get; set; }
        /// <summary>
        /// 医保保险类型
        /// </summary>
        public string YIBAOBXLX { get; set; }
        /// <summary>
        /// empi ID 嘉兴实名制认证标志字段
        /// </summary>
        public string EMPIID { get; set; }

        public RENYUANXX_OUT() {
            this.TESHUBZMX = new List<TESHUBZXX>();
            this.CHONGFUJYMX = new List<CHONGFUJYXX>();
        }
    }
}
