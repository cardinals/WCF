using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKC.Schemas
{
    public class DISP_START_IN : BASEIN 
    {
        /// <summary>
        /// 患者编号
        /// </summary>
        public string pid;
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string name;
        /// <summary>
        /// 处方信息
        /// </summary>
        public List<HKC.Schemas.Start.order> orderinfo;

        public DISP_START_IN() {
            this.orderinfo = new List<HKC.Schemas.Start.order>();
        }
    }

    public class DISP_START_OUT : BASEOUT { 
    
    }
}
