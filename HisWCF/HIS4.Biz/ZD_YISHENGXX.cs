using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using System.Data;
using SWSoft.Framework;

namespace HIS4.Biz
{
    public class ZD_YISHENGXX : IMessage<ZD_YISHENGXX_IN, ZD_YISHENGXX_OUT>
    {
        public override void ProcessMessage()
        {
            this.OutObject = new ZD_YISHENGXX_OUT();
            string keShiId = InObject.KESHIID;

            string YiShengXXSql = "select yishengid,yishengxm,yishengxb,yishengzc,yishengjs,keshiid from web_yishengxx where 1=1  ";

            if (!string.IsNullOrEmpty(keShiId)) {
                YiShengXXSql += " and keshiid = '" + keShiId + "' ";   
            }
            DataTable dtYiShengXX = DBVisitor.ExecuteTable(YiShengXXSql);
            if (dtYiShengXX.Rows.Count > 0)
            {
                for (int i = 0; i < dtYiShengXX.Rows.Count; i++) {
                    HIS4.Schemas.ZD.YISHENGXX ysxx = new Schemas.ZD.YISHENGXX();
                    ysxx.YISHENGID = dtYiShengXX.Rows[i]["yishengid"].ToString();
                    ysxx.YISHENGXM = dtYiShengXX.Rows[i]["yishengxm"].ToString();
                    ysxx.YISHENGXB = dtYiShengXX.Rows[i]["yishengxb"].ToString();
                    ysxx.YISHENGZC = dtYiShengXX.Rows[i]["yishengzc"].ToString();
                    ysxx.YISHENGJS = dtYiShengXX.Rows[i]["yishengjs"].ToString();
                    OutObject.YISHENGMX.Add(ysxx);
                }
            }
            else {
                throw new Exception("系统建设中……");
            }
           
        }
    }
}
