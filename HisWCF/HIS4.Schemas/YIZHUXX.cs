using JYCS.Schemas;
using System.Collections.Generic;

namespace HIS4.Schemas
{
    public class YIZHUXX_IN : MessageIn
    {
        /// <summary>
        /// 病人住院ID
        /// </summary>
        public string BINGRENID { get; set; }
    }

    public class YIZHUXX_OUT : MessageOUT
    {
        /// <summary>
        /// 医嘱列表
        /// </summary>
        public List<BINGRENYZXX> BINGRENYZXX { get; set; }
        public YIZHUXX_OUT()
        {
            BINGRENYZXX = new List<BINGRENYZXX>();
        }
    }

    public class BINGRENYZXX
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        public string STARTTIME { get; set; }
        /// <summary>
        /// 医嘱内容
        /// </summary>
        public string ORDERNAME { get; set; }
        /// <summary>
        /// 医师签名
        /// </summary>
        public string PHYSICIANNAME { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public string ZHIXINGSJ { get; set; }
        /// <summary>
        /// 执行者签名
        /// </summary>
        public string ZHIXINGRENNAME { get; set; }
    }
}
