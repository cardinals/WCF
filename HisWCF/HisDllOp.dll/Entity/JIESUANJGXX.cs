using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    /// <summary>
    /// 结算结算信息
    /// </summary>
    public class JIESUANJGXX : BaseGroup
    {
        /// <summary>
        ///结果序号
        /// </summary>
        public string JIEGUOXH { set; get; }
        /// <summary>
        ///结果类型
        /// </summary>
        public string JIEGUOLX { set; get; }
        /// <summary>
        ///结果代码
        /// </summary>
        public string JIEGUODM { set; get; }
        /// <summary>
        ///结果名称
        /// </summary>
        public string JIEGUOMC { set; get; }
        /// <summary>
        ///结果内容
        /// </summary>
        public string JIEGUONR { set; get; }

    }
}
