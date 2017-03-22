using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;


namespace HIS4.Schemas
{
    public class JIUZHENXXCX_IN : MessageIn
    {
        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZHENGJIANLX { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string KAISHIRQ { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string JIESHURQ { get; set; }
        /// <summary>
        /// 查询类型
        /// </summary>
        public string CHAXUNLX { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public string SHUJULY { get; set; }

        public JIUZHENXXCX_IN()
        {

           
        }
    }

    public class JIUZHENXXCX_OUT : MessageOUT
    {
        /// <summary>
        /// 就诊明细条数
        /// </summary>
        public string FEIYONGMXTS { get; set; }
        /// <summary>
        /// 就诊明细
        /// </summary>
        public List<JIUZHENXX> JIUZHENMX { get; set; }
        public JIUZHENXXCX_OUT()
        {
            this.JIUZHENMX = new List<JIUZHENXX>();
        }
    }

    public class JIUZHENXX {
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }

        public string BINGRENLB { get; set; }

        public string BINGRENXZ { get; set; }

        public string YIBAOKH { get; set; }

        public string XINGBIE { get; set; }

        public string GERENBH { get; set; }

        public string BINGLIBH { get; set; }

        public string XINGMING { get; set; }

        public string MINZU { get; set; }

        public string HUNYIN { get; set; }

        public string CHUSHENGRQ { get; set; }

        public string ZHENGJIANLX { get; set; }

        public string ZHENGJIANHM { get; set; }

        public string DANWEILX { get; set; }

        public string DANWEIBH { get; set; }

        public string DANWEIMC { get; set; }

        public string JIATINGZZ { get; set; }

        public string BINGANH { get; set; }

        public string BINGRENXZMC { get; set; }

        public string DANGQIANBQ { get; set; }

        public string DANGQIANKS { get; set; }

        public string ZHUZHIYISHENG { get; set; }

        public string CHUANGWEIH { get; set; }

        public string RUYUANRQ { get; set; }

        public string CHUYUANRQ { get; set; }

        public string LIANXIDH { get; set; }

        public string LIANXIR { get; set; }

        public List<JIBINGXX> JIBINGMX { get; set; }

        public string ZHILIAOJG { get; set; }

        public string CHUYUANQK { get; set; }

        public string CHUYUANZT { get; set; }

        public string SHUXIEYS { get; set; }

        public List<CHUYUANDYXX> CHUYUANDYMX { get; set; }

        public string BINGLIZD { get; set; }

        public string YUYUEYY { get; set; }

        public List<MENZHENYONGYAOXX> MENZHENYONGYAOMX { get; set; }

        public JIUZHENXX() {
            this.MENZHENYONGYAOMX = new List<MENZHENYONGYAOXX>();
        }

    }
    /// <summary>
    /// 门诊用药信息
    /// </summary>
    public class MENZHENYONGYAOXX
    {
        /// <summary>
        /// 药品名称
        /// </summary>
        public string YAOPINMC { get; set; }
        /// <summary>
        /// 药品频次
        /// </summary>
        public string YONGYAOPC { get; set; }
        /// <summary>
        /// 药品单位
        /// </summary>
        public string YAOPINDW { get; set; }
        /// <summary>
        /// 药品规格
        /// </summary>
        public string YAOPINGG { get; set; }
 
        }

    public class CHUYUANDYXX {
        
        public string JIFEIRQ { get; set; }

        public string FEIYONGLX { get; set; }

        public string XIANGMUXH { get; set; }

        public string XIANGMUCDDM { get; set; }

        public string XIANGMUMC { get; set; }

        public string XIANGMUGL { get; set; }

        public string XIANGMUGLMC { get; set; }

        public string XIANGMUGG { get; set; }

        public string XIANGMUJX { get; set; }

        public string XIANGMUDW { get; set; }

        public string XIANGMUCDMC { get; set; }

        public string DANJIA { get; set; }

        public string SHULIANG { get; set; }

        public string JINE { get; set; }

        public string YIBAODJ { get; set; }

        public string YIBAOZFBL { get; set; }

        public string XIANGMUXJ { get; set; }

        public string ZIFEIJE { get; set; }

        public string ZILIJE { get; set; }

        public string KAIDANKSDM { get; set; }

        public string KAIDANKSMC { get; set; }

        public string KAIDANYSDM { get; set; }

        public string KAIDANYSXM { get; set; }
    }
}
