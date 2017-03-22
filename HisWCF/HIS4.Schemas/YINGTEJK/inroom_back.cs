using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas.YINGTEJK
{
    public class Inroom_Back_In
    {
        public List<inroomBackInfo> dataList { get; set; }

        public Inroom_Back_In() {
            this.dataList = new List<inroomBackInfo>();
        }
    }

    public class Inroom_Back_Out : yingtejkBASE
    {

    }

    public class inroomBackInfo
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 退货单号
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 退货时间
        /// </summary>
        public string return_date { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public string amount { get; set; }

        public List<inroomBackItem> itemList { get; set; }

        public inroomBackInfo() {
            this.itemList = new List<inroomBackItem>();
        }
    }

    public class inroomBackItem
    {
        /// <summary>
        /// 退货细单单号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 退货总单单号
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string medic_id { get; set; }
        /// <summary>
        /// 商品批号
        /// </summary>
        public string medic_lot { get; set; }
        /// <summary>
        /// 退货数量
        /// </summary>
        public string nums { get; set; }
        /// <summary>
        /// 退货含税价格
        /// </summary>
        public string tax_price { get; set; }
        /// <summary>
        /// 退货无税价格
        /// </summary>
        public string no_tax_price { get; set; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public string provider_id { get; set; }
    }
}
