using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class JIANCHAJGXX
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
        /// 检查类型
        /// </summary>
        public string JIANCHALX { get; set; }
        /// <summary>
        /// 检查类型名称
        /// </summary>
        public string JIANCHALXMC { get; set; }
        /// <summary>
        /// 开单医生
        /// </summary>
        public string KAIDANYS { get; set; }
        /// <summary>
        /// 开单时间
        /// </summary>
        public string KAIDANSJ { get; set; }
        /// <summary>
        /// 主诉
        /// </summary>
        public string ZHUSU { get; set; }
        /// <summary>
        /// 临床诊断
        /// </summary>
        public string LINCHUANGZD { get; set; }
        /// <summary>
        /// 检查部位
        /// </summary>
        public string JIANCHABW { get; set; }
        /// <summary>
        /// 诊断结果
        /// </summary>
        public string ZHENDUANJG { get; set; }
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
        /// 报告时间
        /// </summary>
        public string BAOGAOSJ { get; set; }

        /// <summary>
        /// 模板代码
        /// </summary>
        public string MOBANDM { get; set; }

        /// <summary>
        /// 检查时间
        /// </summary>
        public string JIANCHASJ { get; set; }
        
    }
}
