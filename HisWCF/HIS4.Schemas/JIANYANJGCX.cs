using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class JIANYANJGCX_IN :MessageIn
    {
        /// <summary>
        /// 检验条码
        /// </summary>
        public string JIANYANTM { get; set; }
    }

    public class JIANYANJGCX_OUT : MessageOUT
    {
        /// <summary>
        /// 样本号
        /// </summary>
        public string YANGBENH { get; set; }
        /// <summary>
        /// 样本类型
        /// </summary>
        public string YANGBENLX { get; set; }
        /// <summary>
        /// 样本类型名称
        /// </summary>
        public string YANGBENLXMC { get; set; }
        /// <summary>
        /// 开单科室代码
        /// </summary>
        public string KAIDANKSDM { get; set; }
        /// <summary>
        /// 开单科室名称
        /// </summary>
        public string KAIDANKSMC { get; set; }
        /// <summary>
        /// 开单医生代码
        /// </summary>
        public string KAIDANYSDM { get; set; }
        /// <summary>
        /// 开单医生姓名
        /// </summary>
        public string KAIDANYSXM { get; set; }
        /// <summary>
        /// 送检医生代码
        /// </summary>
        public string SONGJIANYSDM { get; set; }
        /// <summary>
        /// 送检医生姓名
        /// </summary>
        public string SONGJIANYSXM { get; set; }
        /// <summary>
        /// 执行人代码
        /// </summary>
        public string ZHIXINGYSDM { get; set; }
        /// <summary>
        /// 执行人姓名
        /// </summary>
        public string ZHIXINGYSXM { get; set; }
        /// <summary>
        /// 采集时间
        /// </summary>
        public string CAIJISJ { get; set; }
        /// <summary>
        /// 接收时间
        /// </summary>
        public string JIESHOUSJ { get; set; }
        /// <summary>
        /// 报告时间
        /// </summary>
        public string BAOGAOSJ { get; set; }
        /// <summary>
        /// 检验项目代码
        /// </summary>
        public string JIANYANXMDM { get; set; }
        /// <summary>
        /// 检验项目名称
        /// </summary>
        public string JIANYANXMMC { get; set; }
        /// <summary>
        /// 检验结果明细
        /// </summary>
        public List<JIANYANJGXX> JIANYANJGMX { get; set; }

        public JIANYANJGCX_OUT() {

            this.JIANYANJGMX = new List<JIANYANJGXX>();
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
        /// 审核人姓名
        /// </summary>
        public string SHENHERXM { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        public string SHENHERQ { get; set; }
    }
}
