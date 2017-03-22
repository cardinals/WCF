using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas.YINGTEJK
{
    public class Inroom_In
    {
        public List<inroomInfo> dataList { get; set; }

        public Inroom_In() {
            this.dataList = new List<inroomInfo>();
        }
    }

    public class Inroom_Out
    {
    }


    public class inroomInfo
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public string trace_code { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        public string inroom_date { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public string amount { get; set; }

        public List<inroomItem> itemList { get; set; }

        public inroomInfo() {

            this.itemList = new List<inroomItem>();
        }
    }

    public class inroomItem
    {
        /// <summary>
        /// 入库细单单号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public string trace_code { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string medic_id { get; set; }
        /// <summary>
        /// 商品批号
        /// </summary>
        public string medic_lot { get; set; }
        /// <summary>
        /// 商品生产日期
        /// </summary>
        public string produce_date { get; set; }
        /// <summary>
        /// 商品有效期至
        /// </summary>
        public string delay_date { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>
        public string nums { get; set; }
        /// <summary>
        /// 含税价格
        /// </summary>
        public string tax_price { get; set; }
        /// <summary>
        /// 无税价格
        /// </summary>
        public string no_tax_price { get; set; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public string provider_id { get; set; }
    }
}
