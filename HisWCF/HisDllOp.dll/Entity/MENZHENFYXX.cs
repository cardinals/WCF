using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    /// <summary>
    /// 门诊费用信息
    /// </summary>
    public class MENZHENFYXX : BaseGroup
    {
        /// <summary>
        /// 处方类型  0 医技 1 处方
        /// </summary>
        public string CHUFANGLX { set; get; }
        /// <summary>
        ///处方序号
        /// </summary>
        public string CHUFANGXH { set; get; }
        /// <summary>
        ///明细序号
        /// </summary>
        public string MINGXIXH { set; get; }
        /// <summary>
        ///费用类型
        /// </summary>
        public string FEIYONGLX { set; get; }
        /// <summary>
        ///项目序号
        /// </summary>
        public string XIANGMUXH { set; get; }
        /// <summary>
        ///项目产品代码
        /// </summary>
        public string XIANGMUCDDM { set; get; }
        /// <summary>
        ///项目名称
        /// </summary>
        public string XIANGMUMC { set; get; }
        /// <summary>
        ///项目归类
        /// </summary>
        public string XIANGMUGL { set; get; }
        /// <summary>
        /// 项目归类名称
        /// </summary>
        public string XIANGMUGLMC { get; set; }
        /// <summary>
        ///项目规格
        /// </summary>
        public string XIANGMUGG { set; get; }
        /// <summary>
        ///项目剂型
        /// </summary>
        public string XIANGMUJX { set; get; }
        /// <summary>
        ///项目单位
        /// </summary>
        public string XIANGMUDW { set; get; }
        /// <summary>
        ///项目产地名称
        /// </summary>
        public string XIANGMUCDMC { set; get; }
        /// <summary>
        ///包装数量
        /// </summary>
        public string BAOZHUANGSL { set; get; }
        /// <summary>
        ///包装单位
        /// </summary>
        public string BAOZHUANGDW { set; get; }
        /// <summary>
        ///最小剂量单位
        /// </summary>
        public string ZUIXIAOJLDW { set; get; }
        /// <summary>
        ///单次用量
        /// </summary>
        public string DANCIYL { set; get; }
        /// <summary>
        ///用量单位
        /// </summary>
        public string YONGLIANGDW { set; get; }
        /// <summary>
        ///每天次数
        /// </summary>
        public string MEITIANCS { set; get; }
        /// <summary>
        ///用药天数
        /// </summary>
        public string YONGYAOTS { set; get; }
        /// <summary>
        ///单复方标志
        /// </summary>
        public string DANFUFBZ { set; get; }
        /// <summary>
        ///中草药贴数
        /// </summary>
        public string ZHONGCAOYTS { set; get; }
        /// <summary>
        ///单价
        /// </summary>
        public string DANJIA { set; get; }
        /// <summary>
        ///数量
        /// </summary>
        public string SHULIANG { set; get; }
        /// <summary>
        ///金额
        /// </summary>
        public string JINE { set; get; }
        /// <summary>
        ///医保等级
        /// </summary>
        public string YIBAODJ { set; get; }
        /// <summary>
        ///医保代码
        /// </summary>
        public string YIBAODM { set; get; }
        /// <summary>
        ///医保自负比例
        /// </summary>
        public string YIBAOZFBL { set; get; }
        /// <summary>
        ///项目限价
        /// </summary>
        public string XIANGMUXJ { set; get; }
        /// <summary>
        ///自费金额
        /// </summary>
        public string ZIFEIJE { set; get; }
        /// <summary>
        ///自理金额
        /// </summary>
        public string ZILIJE { set; get; }
        /// <summary>
        ///审批编号
        /// </summary>
        public string SHENGPIBH { set; get; }
        /// <summary>
        ///自费标志
        /// </summary>
        public string ZIFEIBZ { set; get; }
        /// <summary>
        ///特殊用药标志
        /// </summary>
        public string TESHUYYBZ { set; get; }
        /// <summary>
        ///医保项目辅助信息
        /// </summary>
        public string YIBAOXMFZXX { set; get; }
        /// <summary>
        ///单次数量
        /// </summary>
        public string DANCISL { set; get; }
        /// <summary>
        ///频率数值
        /// </summary>
        public string PINLVSZ { set; get; }
        /// <summary>
        ///开单科室代码
        /// </summary>
        public string KAIDANKSDM { set; get; }
        /// <summary>
        ///开单科室名称
        /// </summary>
        public string KAIDANKSMC { set; get; }
        /// <summary>
        ///开单医生代码
        /// </summary>
        public string KAIDANYSDM { set; get; }
        /// <summary>
        ///开单医生姓名
        /// </summary>
        public string KAIDANYSXM { set; get; }
        /// <summary>
        /// 院内自付比例
        /// </summary>
        public string ZIFUBL { get; set; }
        /// <summary>
        /// 开单日期
        /// </summary>
        public string KAIDANRQ { get; set; }
        /// <summary>
        /// 药品大规格序号
        /// </summary>
        public string YAOPINDGGXH { get; set; }
        /// <summary>
        /// 药品大规格产地
        /// </summary>
        public string YAOPINDGGCD { get; set; }
        /// <summary>
        /// 药品大规格数量
        /// </summary>
        public string YAOPINDGGSL { get; set; }
    }
}
