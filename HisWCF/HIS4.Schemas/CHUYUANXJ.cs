using JYCS.Schemas;
using System.Collections.Generic;

namespace HIS4.Schemas
{
    public class CHUYUANXJ_IN : MessageIn
    {
        /// <summary>
        /// 病人住院ID
        /// </summary>
        public string BINGRENID { get; set; }
    }

    public class CHUYUANXJ_OUT : MessageOUT
    {
        /// <summary>
        /// 出院小结
        /// </summary>
        public List<BINGRENCYXX> CHUYUANXJXX { get; set; }
        public CHUYUANXJ_OUT()
        {
            CHUYUANXJXX = new List<BINGRENCYXX>();
        }
    }

    public class BINGRENCYXX
    {
        /// <summary>
        /// 病案号
        /// </summary>
        public string BIGNANH { get; set; }
        /// <summary>
        /// 在院状态：1在院0出院
        /// </summary>
        public string ZAIYUANZT { get; set; }
        /// <summary>
        /// 病人姓名
        /// </summary>
        public string BINGRENXM { get; set; }
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string SHENFENZH { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string ZHIYE { get; set; }
        /// <summary>
        /// 性别：1男2女
        /// </summary>
        public string XINGBIE { get; set; }
        /// <summary>
        /// 出生地
        /// </summary>
        public string CHUSHENGGD { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string NIANLING { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string LIANXIR { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string LIANXIRDH { get; set; }
        /// <summary>
        /// 现住址
        /// </summary>
        public string XIANZHUZ { get; set; }
        /// <summary>
        /// 婚姻：1未婚2已婚3丧偶4离婚
        /// </summary>
        public string HUNYIN { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string MINZU { get; set; }
        /// <summary>
        /// 入院日期
        /// </summary>
        public string RUYUANRQ { get; set; }
        /// <summary>
        /// 出院日期
        /// </summary>
        public string CHUYUANRQ { get; set; }
        /// <summary>
        /// 床位
        /// </summary>
        public string CHUANWEI { get; set; }
        /// <summary>
        /// 主治医生
        /// </summary>
        public string ZHUZHIYS { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string KESHIMC { get; set; }
        /// <summary>
        /// 病理诊断
        /// </summary>
        public string BINGLIZD { get; set; }
        /// <summary>
        /// 入院情况
        /// </summary>
        public string RUYUNAQK { get; set; }
        /// <summary>
        /// 出院情况
        /// </summary>
        public string CHUYUANQK { get; set; }
        /// <summary>
        /// 入院诊断
        /// </summary>
        public string RUYUANZD { get; set; }
        /// <summary>
        /// 出院诊断
        /// </summary>
        public string CHUYUANZD { get; set; }
        /// <summary>
        /// 出院医嘱
        /// </summary>
        public string CHUYUANYZ { get; set; }
    }
}