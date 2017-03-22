using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZHUYUANRYXX_IN : MessageIn
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
        /// 病区代码
        /// </summary>
        public string BINGQUDM { get; set; }
        /// <summary>
        /// 床位号
        /// </summary>
        public string CHUANGWEIH { get; set; }
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
        /// 证件类型
        /// </summary>
        public string ZHENGJIANLX { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 在院状态
        /// </summary>
        public string ZAIYUANZT { get; set; }
        /// <summary>
        /// 住院号
        /// </summary>
        public string ZHUYUANHAO { get; set; }
    }

    public class ZHUYUANRYXX_OUT : MessageOUT
    {
        /// <summary>
        /// 住院人员信息
        /// </summary>
        public List<ZHUYUANXX> ZHUYUANRYMX { get; set; }
        /// <summary>
        /// 病种信息
        /// </summary>
        public List<BINGZHONGXX> TESHUBZXX { get; set; }

        public ZHUYUANRYXX_OUT() {
            ZHUYUANRYMX = new List<ZHUYUANXX>();
            TESHUBZXX = new List<BINGZHONGXX>();
        }
    }
}
