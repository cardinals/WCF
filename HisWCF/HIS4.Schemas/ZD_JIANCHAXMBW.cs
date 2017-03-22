using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZD_JIANCHAXMBW_IN : MessageIn 
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string CAOZUOLX { get; set; }
        /// <summary>
        /// 检查项目明细
        /// </summary>
        public List<JIANCHAXMBWXX> JIANCHAXMBWMX { get; set; }

        public ZD_JIANCHAXMBW_IN() {
            this.JIANCHAXMBWMX = new List<JIANCHAXMBWXX>();
        }
    }

    public class ZD_JIANCHAXMBW_OUT : MessageOUT 
    {
        /// <summary>
        /// 检查项目明细
        /// </summary>
        public List<JIANCHAXMBWXX> JIANCHAXMBWMX { get; set; }

        public ZD_JIANCHAXMBW_OUT()
        {
            this.JIANCHAXMBWMX = new List<JIANCHAXMBWXX>();
        }
    }
    /// <summary>
    /// 检查项目部位明细
    /// </summary>
    public class JIANCHAXMBWXX
    {
        /// <summary>
        /// 检查项目部位代码
        /// </summary>
        public string JIANCHAXMBWDM { get; set; }
        /// <summary>
        /// 检查项目部位名称
        /// </summary>
        public string JIANCHAXMBWMC { get; set; }
        /// <summary>
        /// 检查项目类型
        /// </summary>
        public string JIANCHAXMLX { get; set; }
       
    }

   
}


