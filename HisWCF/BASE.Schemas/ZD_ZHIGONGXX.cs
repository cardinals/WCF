using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class ZD_ZHIGONGXX_IN : MessageIn
    {
        /// <summary>
        /// 职工类型
        /// </summary>
        public string ZHIGONGLX { get; set; }
        /// <summary>
        /// 职工工号
        /// </summary>
        public string ZHIGONGGH { get; set; }
        /// <summary>
        /// 职工姓名
        /// </summary>
        public string ZHIGONGXM { get; set; }
    }

    public class ZD_ZHIGONGXX_OUT : MessageOUT
    {
        /// <summary>
        /// 职工信息列表
        /// </summary>
        public List<ZHIGONGXX> ZHIGONGLB { get; set; }
        public ZD_ZHIGONGXX_OUT()
        {
            ZHIGONGLB = new List<ZHIGONGXX>();
        }
    }

    public class ZHIGONGXX
    {
        /// <summary>
        /// 职工工号
        /// </summary>
        public string ZHIGONGGH { get; set; }
        /// <summary>
        /// 职工姓名
        /// </summary>
        public string ZHIGONGXM { get; set; }
        /// <summary>
        /// 职工职称
        /// </summary>
        public string ZHIGONGZC { get; set; }
        /// <summary>
        /// 职工介绍
        /// </summary>
        public string ZHIGONGJS { get; set; }
        /// <summary>
        /// 职工特长
        /// </summary>
        public string ZHIGONGTC { get; set; }
    }
}
