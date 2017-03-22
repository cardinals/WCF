using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWSoft.Framework;
using JYCS.Schemas;
using HKC.Schemas;
using System.Data;

namespace HKC.Biz
{
    public class DISP_READY :HKCMessage<DISP_READY_IN,DISP_READY_OUT>
    {

        public override void ProcessMessage()
        {
            string jiuZhenKH = InObject.patientid;

            if (string.IsNullOrEmpty(jiuZhenKH))
            {
                throw new Exception("患者编号不能为空");
            }

            string sqlBingRenID = "select bingrenid from gy_bingrenxx where jiuzhenkh ='{0}'";
            string bingRenID = (string)DBVisitor.ExecuteScalar(string.Format(sqlBingRenID, jiuZhenKH));

            string errChuFang = string.Empty;
            foreach (var item in InObject.orderinfo) {
                string orderno = item.orderno;
                if (string.IsNullOrEmpty(orderno)) {
                    errChuFang += "," + jiuZhenKH;
                    continue;
                }
                string sqlPYWC = "update mz_chufang1 set peiyaobz = 1 where chufangid = '{0}' and bingrenid = '{1}'";
                DBVisitor.ExecuteNonQuery(string.Format(sqlPYWC, orderno, jiuZhenKH));
            }
            if (!string.IsNullOrEmpty(errChuFang)) {
                errChuFang = errChuFang.Trim(',');
                throw new Exception("以下病人的处方id获取失败请检查数据：" + errChuFang);
            }
        }
    }
}
