using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class JIANYANJLXX
    {
        /// <summary>
        /// 病人ID
        /// </summary>
        public string BINGRENID { get; set; }
        /// <summary>
        /// 就诊来源
        /// </summary>
        public string JIUZHENLY { get; set; }
        /// <summary>
        /// 医嘱编号
        /// </summary>
        public string YIZHUID { get; set; }
        /// <summary>
        /// 申请单编号
        /// </summary>
        public string SHENQINGDID { get; set; }
        /// <summary>
        /// 门诊就诊编号
        /// </summary>
        public string MENZHENID { get; set; }
        /// <summary>
        /// 住院编号
        /// </summary>
        public string ZHUYUANID { get; set; }
        /// <summary>
        /// 病人床位
        /// </summary>
        public string BINGRENCW { get; set; }
        /// <summary>
        /// 病人科室
        /// </summary>
        public string BINGRENKS { get; set; }
        /// <summary>
        /// 病人病区
        /// </summary>
        public string BINGRENBQ { get; set; }
        /// <summary>
        /// 采集医生代码
        /// </summary>
        public string KAIDANYSDM { get; set; }
        /// <summary>
        /// 采集科室代码
        /// </summary>
        public string KAIDANKSDM { get; set; }
        /// <summary>
        /// 开单科室名称
        /// </summary>
        public string KAIDANKSMC { get; set; }
        /// <summary>
        /// 开单医生姓名
        /// </summary>
        public string KAIDANYSXM { get; set; }
        /// <summary>
        /// 开单时间
        /// </summary>
        public string KAIDANSJ { get; set; }
        /// <summary>
        /// 检验项目信息
        /// </summary>
        public List<JIANYANXMXX> JIANYANXMMX { get; set; }
        /// <summary>
        /// 检验科室代码
        /// </summary>
        public string JIANYANKSDM { get; set; }
        /// <summary>
        /// 检验科室名称
        /// </summary>
        public string JIANYANKSMC { get; set; }
        /// <summary>
        /// 采集日期
        /// </summary>
        public string CAIJIRQ { get; set; }
        /// <summary>
        /// 采集医生代码
        /// </summary>
        public string CAIJIYSDM { get; set; }
        /// <summary>
        /// 采集科室代码
        /// </summary>
        public string CAIJIKSDM { get; set; }
        /// <summary>
        /// 采集医生姓名
        /// </summary>
        public string CAIJIYSXM { get; set; }
        /// <summary>
        /// 采集科室名称
        /// </summary>
        public string CAIJIKSMC { get; set; }
        /// <summary>
        /// 条码号
        /// </summary>
        public string TIAOMAH { get; set; }
        /// <summary>
        /// 诊断
        /// </summary>
        public string ZHENDUAN { get; set; }


        

        public JIANYANJLXX(){
            this.JIANYANXMMX = new List<JIANYANXMXX>();
        }
        /// <summary>
        /// 审核标识
        /// </summary>
        public string SHENHEBZ { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string SHENHEREN { get; set; }
        /// <summary>
        /// 审核姓名
        /// </summary>
        public string SHENHERXM { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        public string SHENHERQ { get; set; }
    }
}
