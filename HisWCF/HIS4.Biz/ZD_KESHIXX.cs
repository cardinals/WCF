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
    public class ZD_KESHIXX : IMessage<ZD_KESHIXX_IN, ZD_KESHIXX_OUT>
    {
        public override void ProcessMessage()
        {
            this.OutObject = new ZD_KESHIXX_OUT();

            string KeShiXXSql = "select keshiid,keshimc,keshims from web_keshixx ";

            DataTable dtKeShiXX = DBVisitor.ExecuteTable(KeShiXXSql);

            if (dtKeShiXX.Rows.Count > 0)
            {
                for(int i =0;i<dtKeShiXX.Rows.Count;i++){
                    HIS4.Schemas.ZD.KESHIXX ksxx = new HIS4.Schemas.ZD.KESHIXX();
                    ksxx.KESHIID = dtKeShiXX.Rows[i]["KESHIID"].ToString();
                    ksxx.KESHIMC = dtKeShiXX.Rows[i]["keshimc"].ToString();
                    ksxx.KESHIMS = dtKeShiXX.Rows[i]["keshims"].ToString();
                    OutObject.KESHIMX.Add(ksxx);
                }
            }
            else {
                throw new Exception("系统建设中……");
            }

        }

    }
}
