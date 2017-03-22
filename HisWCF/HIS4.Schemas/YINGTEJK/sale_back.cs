using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas.YINGTEJK
{
    public class Sale_Back_In
    {
        public List<saleBackInfo> dataList { get; set; }

        public Sale_Back_In() {

            this.dataList = new List<saleBackInfo>();
        }

    }

    public class Sale_Back_Out : yingtejkBASE
    {
    }

    public class saleBackInfo { 
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
        /// <summary>
        /// 收银现金金额
        /// </summary>
        public string cash { get; set; }
        /// <summary>
        /// 银联卡金额
        /// </summary>
        public string unionpay { get; set; }
        /// <summary>
        /// 支票金额
        /// </summary>
        public string checks { get; set; }
        /// <summary>
        /// 转帐金额
        /// </summary>
        public string transfer { get; set; }
        /// <summary>
        /// 市民卡
        /// </summary>
        public string city_card { get; set; }
        /// <summary>
        /// 城市通
        /// </summary>
        public string city_pass { get; set; }
        /// <summary>
        /// 现金券
        /// </summary>
        public string cash_coupon { get; set; }
        /// <summary>
        /// 储值卡
        /// </summary>
        public string stored_card { get; set; }
        /// <summary>
        /// 市医保
        /// </summary>
        public string city_insurance { get; set; }
        /// <summary>
        /// 省医保
        /// </summary>
        public string province_insurance { get; set; }

        public List<saleBackItem> itemList { get; set; }

        public saleBackInfo()
        {
            this.itemList = new List<saleBackItem>();
        }
    }

    public class saleBackItem
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
        /// 药品编号
        /// </summary>
        public string medic_id { get; set; }
        /// <summary>
        /// 药品批号
        /// </summary>
        public string medic_lot { get; set; }
        /// <summary>
        /// 退货数量
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
    }

}
