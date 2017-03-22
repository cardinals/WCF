using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas.DATAENTITY;


namespace HIS4.Schemas
{
    public class ZHIFUBAOJSMXXZ_IN : MessageIn
    {
        /// <summary>
        /// 下载类型 1本地下载 2支付宝下载
        /// </summary>
        public string XIAZAILX { get; set; }
        /// <summary>
        /// 查询日期
        /// </summary>
        public string RIQI { get; set; }
        /// <summary>
        /// 页号
        /// </summary>
        public string PAGENO { get; set; }
        ///// <summary>
        ///// page_size分页大小
        ///// </summary>
        //public string PAGESIZE { get; set; }

    }
    public class ZHIFUBAOJSMXXZ_OUT : MessageOUT
    {
        /// <summary>
        /// 是否有下一页 T有 F没有
        /// </summary>
        public string HASNEXTPAGE { get; set; }
        /// <summary>
        /// 当前页号
        /// </summary>
        public string PAGENO { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public string PAGESIZE { get; set; }
        /// <summary>
        /// 支付宝对账明细
        /// </summary>
        public List<ZHIFUBAOMX> ZHIFUBAOXZMX { get; set; }

        public ZHIFUBAOJSMXXZ_OUT()
        {

            this.ZHIFUBAOXZMX = new List<ZHIFUBAOMX>();

        }
    }
}
