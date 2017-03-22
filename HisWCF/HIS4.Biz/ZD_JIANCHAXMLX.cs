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
    public class ZD_JIANCHAXMLX : IMessage<ZD_JIANCHAXMLX_IN, ZD_JIANCHAXMLX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new ZD_JIANCHAXMLX_OUT();
            DataTable dtJianChaXM = DBVisitor.ExecuteTable("select  JIANCHALX,jianchaxmmc From gy_jianchaxm a where a.zuofeibz ='0' and a.mojibz ='0' and menzhensy = '1' and substr(a.yuanqusy,1,1) ='1' ");

             for (int i = 0; i < dtJianChaXM.Rows.Count; i++) {
                 JIANCHAXMLXXX temp = new JIANCHAXMLXXX();
                 temp.JIANCHAXMLX = dtJianChaXM.Rows[i]["JIANCHALX"].ToString();
                 temp.JIANCHAXMLXMC = dtJianChaXM.Rows[i]["jianchaxmmc"].ToString();;
                 OutObject.JIANCHAXMLXMX.Add(temp);
             }
             
        }
    }
}
