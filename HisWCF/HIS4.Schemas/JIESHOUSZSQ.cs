using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class JIESHOUSZSQ_IN : MessageIn
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
        /// 医保卡号
        /// </summary>
        public string YIBAOKLX { get; set;}
        /// <summary>
        /// 医保卡号
        /// </summary>
        public string YIBAOKH { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string YEWULX { get; set; }
        /// <summary>
        /// 病人姓名
        /// </summary>
        public string BINGRENXM { get; set; }
        /// <summary>
        /// 病人性别
        /// </summary>
        public string BINGRENXB { get; set; }
        /// <summary>
        /// 病人出生日期
        /// </summary>
        public string BINGRENCSRQ { get; set; }
        /// <summary>
        /// 病人年龄
        /// </summary>
        public string BINGRENNL { get; set; }
        /// <summary>
        /// 病人身份证号
        /// </summary>
        public string BINGRENSFZH { get; set; }
        /// <summary>
        /// 病人联系电话
        /// </summary>
        public string BINGRENLXDH { get; set; }
        /// <summary>
        /// 病人联系地址
        /// </summary>
        public string BINGRENLXDZ { get; set; }
        /// <summary>
        /// 病人费用类别
        /// </summary>
        public string BINGRENFYLB { get; set; }
        /// <summary>
        /// 申请机构代码
        /// </summary>
        public string SHENQINGJGDM { get; set; }
        /// <summary>
        /// 申请机构名称
        /// </summary>
        public string SHENQINGJGMC { get; set; }
        /// <summary>
        /// 申请机构联系电话
        /// </summary>
        public string SHENQINGJGLXDH { get; set; }
        /// <summary>
        /// 申请医生
        /// </summary>
        public string SHENQINGYS { get; set; }
        /// <summary>
        /// 申请医生电话
        /// </summary>
        public string SHENQINGYSDH { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
        public string SHENQINGRQ { get; set; }
        /// <summary>
        /// 转诊原因
        /// </summary>
        public string ZHUANZHENYY { get; set; }
        /// <summary>
        /// 病情描述
        /// </summary>
        public string BINQINGMS { get; set; }
        /// <summary>
        /// 转诊注意事项
        /// </summary>
        public string ZHUANZHENZYSX { get; set; }
        /// <summary>
        /// 转入科室代码
        /// </summary>
        public string ZHUANRUKSDM { get; set; }
        /// <summary>
        /// 转入科室名称
        /// </summary>
        public string ZHUANRUKSMC { get; set; }
        /// <summary>
        /// 转诊单号
        /// </summary>
        public string ZHUANZHENDH { get; set; }
        /// <summary>
        /// 上转接收联系人
        /// </summary>
        public string SZJSLXR { get; set; }
        /// <summary>
        /// 上转接收联系人电话
        /// </summary>
        public string SZJSLXRDH { get; set; }
        /// <summary>
        /// 处方信息
        /// </summary>
        public List<CHUFANGXX> CHUFANGMX { get; set; }
        /// <summary>
        /// 检验信息
        /// </summary>
        public List<JIANYANXX> JIANYANMX { get; set; }
        /// <summary>
        /// 检验信息
        /// </summary>
        public List<JIANCHAXXXX> JIANCHAMX { get; set; }
        /// <summary>
        /// 检验信息
        /// </summary>
        public List<ZHUYUANYZXX> ZHUYUANYZMX { get; set; }

        public JIESHOUSZSQ_IN()
        {
            CHUFANGMX = new List<CHUFANGXX>();
            JIANYANMX = new List<JIANYANXX>();
            JIANCHAMX = new List<JIANCHAXXXX>();
            ZHUYUANYZMX = new List<ZHUYUANYZXX>();
        }
    }

    public class CHUFANGXX
    {
        public string CHUFANGID { get; set; }
        public string CHUFANGLY { get; set; }
        public string CHUFANGLX { get; set; }
        public string KAIFANGRQ { get; set; }
        public string BEIZHU { get; set; }
        /// <summary>
        /// 处方详细信息
        /// </summary>
        public List<CHUFANGXXXX> CHUFANGXXMX { get; set; }
    }

    public class CHUFANGXXXX
    {
        public string FEIYONGLX { get; set; }
        public string XIANGMUMC { get; set; }
        public string YAOPINTYM { get; set; }
        public string YAOPINSPM { get; set; }
        public string CHANGDIMC { get; set; }
        public string YAOPINGG { get; set; }
        public string DANGWEI { get; set; }
        public string SHULIANG { get; set; }
        public string PINLV { get; set; }
        public string GEIYAOTJ { get; set; }
        public string YONGYAOTS { get; set; }
        public string DANCIYL { get; set; }
        public string YONGLIANGDW { get; set; }
        public string PISHIJG { get; set; }
        public string ZHONGCHAOYTS { get; set; }
        public string FAYAORQ { get; set; }
    }

    public class JIANYANXX
    {
        public string JIANYANID { get; set; }
        public string XIANGMUMC { get; set; }
        public string KAIDANRQ { get; set; }
        public string BEIZHU { get; set; }
        public string JIANYANJG { get; set; }
        public string JIANCEFF { get; set; }
        public string WEIJIZBZ { get; set; }
        public string JIANCHAZD { get; set; }
        public string JIANCHARQ { get; set; }
        public string YANGBENLXMC { get; set; }
        public string YANGBENGH { get; set; }
        public string SHENGHEYS { get; set; }
        public string JIANYANRQ { get; set; }

        /// <summary>
        /// 处方详细信息
        /// </summary>
        public List<JIANYANXXXX> JIANYANXXMX { get; set; }
    }

    public class JIANYANXXXX
    {
        public string XIANGMUMC { get; set; }
        public string JIANYANZ { get; set; }
        public string DINGXING { get; set; }
        public string FANWEI { get; set; }
        public string DANWEI { get; set; }
        public string DAYINXH { get; set; }
    }

    public class JIANCHAXXXX
    {
        public string JIANCHAID { get; set; }
        public string JIANCHALX { get; set; }
        public string KAIDANRQ { get; set; }
        public string XIANGMUMC { get; set; }
        public string YIXIANGSJ { get; set; }
        public string ZHENDUAMJG { get; set; }
        public string BAOGAODZ { get; set; }
        public string KAIDANYS { get; set; }
        public string BEIZHU { get; set; }
    }

    public class ZHUYUANYZXX
    {
        public string YIZHUXH { get; set; }
        public string FUYIZXH { get; set; }
        public string YIZHULX { get; set; }
        public string YIZHUZH { get; set; }
        public string YIZHUMC { get; set; }
        public string KAISHISJ { get; set; }
        public string TINGZHISJ { get; set; }
        public string YICISL { get; set; }
        public string YONGLIANGDW { get; set; }
        public string ZHIXINGRQ { get; set; }
        public string PINGLV { get; set; }
        public string YIZHULB { get; set; }
        public string KAIDANYS { get; set; }
        public string PISHIJG { get; set; }
        public string GEIYAOTJ { get; set; }

    }

    public class JIESHOUSZSQ_OUT : MessageOUT
    {
        /// <summary>
        /// 转诊单号
        /// </summary>
        public string ZHUANZHENDH { get; set; }
    }
}
