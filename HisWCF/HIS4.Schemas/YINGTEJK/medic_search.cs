using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas.YINGTEJK
{
    public class Medic_Search_In
    {
        public List<medicInfo> dataList { get; set;}

        public Medic_Search_In() {

            this.dataList = new List<medicInfo>();
        }
    }

    public class Medic_Search_Out : yingtejkBASE
    {

    }

    /// <summary>
    /// 商品基础信息
    /// </summary>
    public class medicInfo { 
        

    }
}
