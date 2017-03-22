using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas
{
    public class JIANYANXMXX
    {
        /// <summary>
        /// 检验项目编号
        /// </summary>
        public string JIANYANXMID { get; set; }
        /// <summary>
        /// 检验项目名称
        /// </summary>
        public string JIANYANXMMC { get; set; }

        public JIANYANXMXX() { 
        
        }

        public JIANYANXMXX(string id,string mc) {
            this.JIANYANXMID = id;
            this.JIANYANXMMC = mc;
        }
    }
}
