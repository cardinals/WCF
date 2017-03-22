using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKC.Schemas
{
    public class DISP_PREPARE_IN :BASEIN
    {
        /// <summary>
        /// 患者编号
        /// </summary>
        public string patientid;
        /// <summary>
        /// 配药单号
        /// </summary>
        public string orderno;
        /// <summary>
        /// 窗口号
        /// </summary>
        public string winno;
    }

    public class DISP_PREPARE_OUT : BASEOUT {

        public PREPARE_RESULT Result;

        public DISP_PREPARE_OUT() {
            this.Result = new PREPARE_RESULT();
        }
    }
}
