using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class JIANCHAJGXX_IN : MessageIn
    {
        public string KAISHIRQ { get; set; }
        public string JIESHURQ { get; set; }
        public string JIUZHENLY { get; set; }
        public string BINGRENID { get; set; }
        public string XINGMING { get; set; }
        public string JIANCHADH { get; set; }
    }

    public class JIANCHAJGXX_OUT : MessageOUT
    {
        public List<JIANCHAXXX> JIANCHAXMX { get; set; }

        public JIANCHAJGXX_OUT()
        {
            this.JIANCHAXMX = new List<JIANCHAXXX>();
        }
    }

    public class JIANCHAXXX
    {
        /// <summary>
        ///就诊卡号    
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        ///医疗序号    
        /// </summary>
        public string YILIAOXH { get; set; }
        /// <summary>
        ///医疗名称    
        /// </summary>
        public string YILIAOMC { get; set; }
        /// <summary>
        ///送检医生工号
        /// </summary>
        public string SONGJIANYSGH { get; set; }
        /// <summary>
        ///送检医生姓名
        /// </summary>
        public string SONGJIANYSXM { get; set; }
        /// <summary>
        ///送检时间
        /// </summary>
        public string SONGJIANSJ { get; set; }
        /// <summary>
        ///检查日期
        /// </summary>
        public string JIANCHARQ { get; set; }
        /// <summary>
        ///变更医生工号
        /// </summary>
        public string BIANGENGYSGH { get; set; }
        /// <summary>
        ///变更医生姓名
        /// </summary>
        public string BIANGENGYSXM { get; set; }
        /// <summary>
        ///变更日期
        /// </summary>
        public string BIANGENGRQ { get; set; }
        /// <summary>
        ///审核医生工号
        /// </summary>
        public string SHENHEYSGH { get; set; }
        /// <summary>
        ///审核医生姓名
        /// </summary>
        public string SHENHEYSXM { get; set; }
        /// <summary>
        ///检查状态
        /// </summary>
        public string JIANCHAZT { get; set; }
        /// <summary>
        ///检查描述
        /// </summary>
        public string JIANCHAMS { get; set; }
        /// <summary>
        ///检查结果
        /// </summary>
        public string JIANCHAJG { get; set; }
        /// <summary>
        ///审核日期
        /// </summary>
        public string SHENHERQ { get; set; }
        /// <summary>
        ///STUDYUID
        /// </summary>
        public string STUDYUID { get; set; }
        /// <summary>
        ///检查号码
        /// </summary>
        public string JIANCHAH { get; set; }
        /// <summary>
        ///病案号
        /// </summary>
        public string BINGANH { get; set; }
        /// <summary>
        ///病区名称
        /// </summary>
        public string BINGQUMC { get; set; }
        /// <summary>
        ///病区床号
        /// </summary>
        public string BINGQUCH { get; set; }
        /// <summary>
        ///病人性别
        /// </summary>
        public string BINGRENXB { get; set; }
        /// <summary>
        ///病人年龄
        /// </summary>
        public string BINGRENNL { get; set; }
        /// <summary>
        ///送检科室名称
        /// </summary>
        public string SONGJIANKSMC { get; set; }
        /// <summary>
        ///预约日期
        /// </summary>
        public string YUYUERQ { get; set; }
        /// <summary>
        ///检查部位代码
        /// </summary>
        public string JIANCHABWDM { get; set; } 

    }
}
