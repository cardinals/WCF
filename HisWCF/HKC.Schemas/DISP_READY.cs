using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKC.Schemas
{
    public class DISP_READY_IN : BASEIN
    {
        /// <summary>
        /// 窗口号
        /// </summary>
        public string winno;
        /// <summary>
        /// 患者编号
        /// </summary>
        public string patientid;
        /// <summary>
        /// 姓名
        /// </summary>
        public string name;
        /// <summary>
        /// 处方信息
        /// </summary>
        public List<Ready.order> orderinfo;

        public DISP_READY_IN() {
            this.orderinfo = new List<Ready.order>();
        }
    }

    public class DISP_READY_OUT : BASEOUT { 
    
    }
}
