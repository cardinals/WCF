using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKC.Schemas
{
    public class DISP_FINISH_IN :BASEIN
    {
        /// <summary>
        /// 窗口id
        /// </summary>
        public string winno;
        /// <summary>
        /// 发药药师工号
        /// </summary>
        public string dispoper;
        /// <summary>
        /// 患者信息
        /// </summary>
        public List<patientid> patientidlist;

        public DISP_FINISH_IN() {
            this.patientidlist = new List<patientid>();
        }
    }

    public class DISP_FINISH_OUT : BASEOUT {
        /// <summary>
        /// 完成信息
        /// </summary>
        List<Result> Resultlist;

        public DISP_FINISH_OUT() {
            this.Resultlist = new List<Result>();
        }
    }
}
