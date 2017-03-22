using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Schemas.YINGTEJK
{
    public class Provider_Search_In
    {
        public List<providerInfo> dataList { get; set; }

        public Provider_Search_In() {
            this.dataList = new List<providerInfo>();
        }
    }

    public class Provider_Search_Out : yingtejkBASE
    { 
    
    }

    /// <summary>
    /// 供应商基础信息
    /// </summary>
    public class providerInfo{
    
    }
}
