using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZD_JIANCHAXMLX_IN : MessageIn 
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string CAOZUOLX { get; set; }
        /// <summary>
        /// 检查项目类型明细
        /// </summary>
        public List<JIANCHAXMLXXX> JIANCHAXMLXMX { get; set; }

        public ZD_JIANCHAXMLX_IN() {
            this.JIANCHAXMLXMX = new List<JIANCHAXMLXXX>();
        }
    }

    public class ZD_JIANCHAXMLX_OUT : MessageOUT 
    {
        /// <summary>
        /// 检查项目类型明细
        /// </summary>
        public List<JIANCHAXMLXXX> JIANCHAXMLXMX { get; set; }

         public ZD_JIANCHAXMLX_OUT() {
            this.JIANCHAXMLXMX = new List<JIANCHAXMLXXX>();
        }
    }

    public class JIANCHAXMLXXX {

        /// <summary>
        /// 检查项目类型
        /// </summary>
        public string JIANCHAXMLX { get; set; }
        /// <summary>
        /// 检查项目类型名称
        /// </summary>
        public string JIANCHAXMLXMC { get; set; }
    }

   
}


