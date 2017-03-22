using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZD_JIANCHAXMXX_IN : MessageIn 
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string CAOZUOLX { get; set; }
        /// <summary>
        /// 检查项目明细
        /// </summary>
        public List<JIANCHAXMXX> JIANCHAXMMX { get; set; }

        public ZD_JIANCHAXMXX_IN() {
            this.JIANCHAXMMX = new List<JIANCHAXMXX>();
        }
    }

    public class ZD_JIANCHAXMXX_OUT : MessageOUT 
    {
        /// <summary>
        /// 检查项目明细
        /// </summary>
        public List<JIANCHAXMXX> JIANCHAXMMX { get; set; }

        public ZD_JIANCHAXMXX_OUT()
        {
            this.JIANCHAXMMX = new List<JIANCHAXMXX>();
        }
    }
    /// <summary>
    /// 检查项目明细
    /// </summary>
    public class JIANCHAXMXX
    {
        /// <summary>
        /// 检查项目代码
        /// </summary>
        public string JIANCHAXMDM { get; set; }
        /// <summary>
        /// 检查项目名称
        /// </summary>
        public string JIANCHAXMMC { get; set; }
        /// <summary>
        /// 检查项目类型
        /// </summary>
        public string JIANCHAXMLX { get; set; }
        /// <summary>
        /// 检查项目类型名称
        /// </summary>
        public string JIANCHAXMLXMC { get; set; }
        /// <summary>
        /// 检查部位代码
        /// </summary>
        public string JIANCHABWDM { get; set; }
        /// <summary>
        /// 检查部位名称
        /// </summary>
        public string JIANCHABWMC { get; set; }
        /// <summary>
        /// 检查项目方向
        /// </summary>
        public string JIANCHAXMFX { get; set; }
        /// <summary>
        /// 检查项目备注
        /// </summary>
        public string JIANCHAXMBZ { get; set; }

    }

   
}


