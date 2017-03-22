using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class MENZHENFYXX
    {

        public string CHUFANGLX { get; set; }//处方类型
        public string CHUFANGXH { get; set; }//处方序号
        public string MINGXIXH { get; set; }//明细序号
        public string FEIYONGLX { get; set; }//费用类型
        public string XIANGMUXH { get; set; }//项目序号
        public string XIANGMUCDDM { get; set; }//项目产品代码
        public string XIANGMUMC { get; set; }//项目名称
        public string XIANGMUGL { get; set; }//项目归类
        public string XIANGMUGLMC { get; set; }//项目归类名称
        public string XIANGMUGG { get; set; }//项目规格
        public string XIANGMUJX { get; set; }//项目剂型
        public string XIANGMUDW { get; set; }//项目单位
        public string XIANGMUCDMC { get; set; }//项目产地名称
        public string BAOZHUANGSL { get; set; }//包装数量
        public string BAOZHUANGDW { get; set; }//包装单位
        public string ZUIXIAOJLDW { get; set; }//最小剂量单位
        public string DANCIYL { get; set; }//单次用量
        public string YONGLIANGDW { get; set; }//用量单位
        public string MEITIANCS { get; set; }//每天次数
        public string YONGYAOTS { get; set; }//用药天数
        public string DANFUFBZ { get; set; }//单复方标志
        public string ZHONGCAOYTS { get; set; }//中草药贴数
        public string DANJIA { get; set; }//单价
        public string SHULIANG { get; set; }//数量
        public string JINE { get; set; }//金额
        public string YIBAODJ { get; set; }//医保等级
        public string YIBAODM { get; set; }//医保代码
        public string YIBAOZFBL { get; set; }//医保自负比例
        public string XIANGMUXJ { get; set; }//项目限价
        public string ZIFEIJE { get; set; }//自费金额
        public string ZILIJE { get; set; }//自理金额
        public string SHENGPIBH { get; set; }//审批编号
        public string ZIFEIBZ { get; set; }//自费标志
        public string TESHUYYBZ { get; set; }//特殊用药标志
        public string YIBAOXMFZXX { get; set; }//医保项目辅助信息
        public string DANCISL { get; set; }//单次数量
        public string PINLVSZ { get; set; }//频率数值
        public string KAIDANKSDM { get; set; }//开单科室代码
        public string KAIDANKSMC { get; set; }//开单科室名称
        public string KAIDANYSDM { get; set; }//开单医生代码
        public string KAIDANYSXM { get; set; }//开单医生姓名
        public string ZIFUBL { get; set; }//自负比例
        public string KAIDANRQ { get; set; }//开单日期
        public string YAOPINDGGXH { get; set; }//药品大规格序号
        public string YAOPINDGGCD { get; set; }//药品大规格产地
        public string YAOPINDGGSL { get; set; }//药品大规格数量
        public string KONGZHISX { get; set; }//控制属性 普通 特病 生育
        public string KONGZHISXMC { get; set; }//控制属性名称
        public string SHOUFEIRQ { get; set; }//收费日期
        /// <summary>
        /// 发票号码
        /// </summary>
        public string FAPIAOHM { get; set; }
        /// <summary>
        /// 发票ID 
        /// </summary>
        public string FAPIAOID { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string ZHIFUFS { get; set; }
        /// <summary>
        /// 操作工号
        /// </summary>
        public string CAOZUOGH { get; set; }
        /// <summary>
        /// 一张发票的医保金额
        /// </summary>
        public string YBJE { get; set; }
        /// <summary>
        /// 收费编号
        /// </summary>
        public string SHOUFEIID { get; set; }
    }
}
