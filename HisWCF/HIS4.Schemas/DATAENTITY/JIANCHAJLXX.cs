using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class JIANCHAJLXX
    {
        /// <summary>
        /// 申请单ID
        /// </summary>
        public string SHENQINDANID { get; set; }

        /// <summary>
        /// 医嘱ID
        /// </summary>
        public string YIZHUID { get; set; }
        /// <summary>
        /// 医嘱项目ID
        /// </summary>
        public string YIZHUXMID { get; set; }
        /// <summary>
        /// 医嘱名称
        /// </summary>
        public string YIZHUMC { get; set; }
        /// <summary>
        /// 门诊就诊ID
        /// </summary>
        public string JIUZHENID { get; set; }
        /// <summary>
        /// 病人住院ID
        /// </summary>
        public string BINGRENZYID { get; set; }
        /// <summary>
        /// 病人ID
        /// </summary>
        public string BINGRENID { get; set; }
        /// <summary>
        /// 病人姓名
        /// </summary>
        public string BINGRENXM { get; set; }
        /// <summary>
        /// 输入人
        /// </summary>
        public string SHURUREN { get; set; }
        /// <summary>
        /// 输入时间
        /// </summary>
        public string SHURUSJ { get; set; }
        /// <summary>
        /// 开单科室
        /// </summary>
        public string KAIDANKS { get; set; }
        /// <summary>
        /// 开单日期
        /// </summary>
        public string KAIDANRQ { get; set; }
        /// <summary>
        /// 检查科室
        /// </summary>
        public string JIANCHAKS { get; set; }
        /// <summary>
        /// 检查日期
        /// </summary>
        public string JIANCHARQ { get; set; }
        /// <summary>
        /// 门诊住院标识 0,门诊,1,住院
        /// </summary>
        public string MENZHENZYBZ { get; set; }
        /// <summary>
        /// 当前状态 1,新开单,9,已撤销,8,已打印,7,已报告,6,已完成,5,已安排,4,已预约,3,待登记,2,待划价,11,已发送未接收,10,已退单
        /// </summary>
        public string DANGQIANZT { get; set; }
        /// <summary>
        /// 当前状态 1,新开单,9,已撤销,8,已打印,7,已报告,6,已完成,5,已安排,4,已预约,3,待登记,2,待划价,11,已发送未接收,10,已退单
        /// </summary>
        public string DANGQIANZTMC { get; set; }
        /// <summary>
        /// 主诉
        /// </summary>
        public string ZHUSU { get; set; }
        /// <summary>
        /// 简要病史
        /// </summary>
        public string JIANYAOBS { get; set; }
        /// <summary>
        /// 检查部位
        /// </summary>
        public string JIANCHABW { get; set; }
        /// <summary>
        /// 检查目的
        /// </summary>
        public string JIANCHAMD { get; set; }
        /// <summary>
        /// 检查类型
        /// </summary>
        public string JIANCHALX { get; set; }
        /// <summary>
        /// 退单人
        /// </summary>
        public string TUIDANREN { get; set; }
        /// <summary>
        /// 退单日期
        /// </summary>
        public string TUIDANRQ { get; set; }
        /// <summary>
        /// 退单人姓名
        /// </summary>
        public string TUIDANRXM { get; set; }
    }
}
