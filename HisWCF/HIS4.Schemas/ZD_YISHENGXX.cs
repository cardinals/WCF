using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class ZD_YISHENGXX_IN : MessageIn 
    {
        /// <summary>
        /// 科室ID
        /// </summary>
        public string KESHIID { get; set; }

    }

    public class ZD_YISHENGXX_OUT : MessageOUT 
    {
        /// <summary>
        /// 医生明细
        /// </summary>
        public List<HIS4.Schemas.ZD.YISHENGXX> YISHENGMX { get; set; }

        public ZD_YISHENGXX_OUT()
        {
            this.YISHENGMX = new List<HIS4.Schemas.ZD.YISHENGXX>();
        }

    }

    
}

namespace HIS4.Schemas.ZD
{
    public class YISHENGXX
    {
        /// <summary>
        /// 医生ID
        /// </summary>
        public string YISHENGID { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string YISHENGXM { get; set; }
        /// <summary>
        /// 医生性别
        /// </summary>
        public string YISHENGXB { get; set; }
        /// <summary>
        /// 医生职称
        /// </summary>
        public string YISHENGZC { get; set; }
        /// <summary>
        /// 医生介绍
        /// </summary>
        public string YISHENGJS { get; set; }
        /// <summary>
        /// 所属科室
        /// </summary>
        public string SUOSUKS { get; set; }
    }
}