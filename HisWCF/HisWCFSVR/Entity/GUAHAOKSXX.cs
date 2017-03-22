using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HisWCFSVR.Entity
{
    public class GUAHAOKSXX
    {

    }

    public class Body_GUAHAOKSXX
    {
        public Result Result { get; set; }
        public List<KESHIXX> KESHIMX { get; set; }

    }
    public class KESHIXX
    {
        /// <summary>
        /// 科室代码	VA50	Y	HIS系统内部唯一标识。【实际就诊科室代码】
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string KESHIMC { get; set; }
        /// <summary>
        /// 就诊地点
        /// </summary>
        public string JIUZHENDD { get; set; }
        /// <summary>
        /// 科室介绍
        /// </summary>
        public string KESHIJS { get; set; }
        /// <summary>
        /// 科室排序
        /// </summary>
        public string KESHIPX { get; set; }
    }

}