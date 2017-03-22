using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HKC.Schemas;
using System.Data;
using SWSoft.Framework;

namespace HKC.Biz
{
    public class DISP_PREPARE : HKCMessage<DISP_PREPARE_IN, DISP_PREPARE_OUT>
    {
        public override void ProcessMessage()
        {
            if (string.IsNullOrEmpty(InObject.pharmacyid) || !InObject.pharmacyid.Contains('*')) {
                throw new Exception("传入参数格式错误");
            }
            string chuFangHM = InObject.pharmacyid.Split('*')[1];//处方id 多个时使用#分割
            string bingRenID = InObject.pharmacyid.Split('*')[0];//病人id

            string pharmacyid = string.Empty;//药房id
            string winno = string.Empty;//窗口id

            #region 入参判断
            if (string.IsNullOrEmpty(chuFangHM))
            {
                throw new Exception("处方号码不能为空");
            }
            if (string.IsNullOrEmpty(bingRenID))
            {
                throw new Exception("病人id不能为空");
            }
            #endregion

            string sqlCKXX = "select a.chufangid, a.chuangkouid chuangkouid,b.yingyongid yaofangid from mz_chufang1 a ,my_chuangkou b where a.chuangkouid = b.chuangkouid and a.chufangid in(select max(chufangid) from mz_chufang1 where chufanghm in('{0}') group by chufanghm)  and a.bingrenid = '{1}' ";

            DataTable dtCKXX = DBVisitor.ExecuteTable(string.Format(sqlCKXX, chuFangHM, bingRenID));

            if (dtCKXX.Rows.Count > 0)
            {
                pharmacyid = dtCKXX.Rows[0]["yaofangid"].ToString();
                winno = dtCKXX.Rows[0]["chuangkouid"].ToString();
                InObject.pharmacyid = pharmacyid;
                InObject.winno = winno;
                InObject.orderno = dtCKXX.Rows[0]["chufangid"].ToString();
                string sqlJiuZhenKH = "select jiuzhenkh from gy_bingrenxx where bingrenid ='{0}'";
                InObject.patientid = (string)DBVisitor.ExecuteScalar(string.Format(sqlJiuZhenKH, bingRenID));
                //InObject.patientid = bingRenID;
                OutObject = Unity.runService<DISP_PREPARE_IN, DISP_PREPARE_OUT>(InObject);
                if (OutObject.ResultCode == "0")
                {
                    if (OutObject.Result.state == "2")
                    {
                        OutObject.ResultContent = "已配药完成";
                        string sqlPYBZ = "update mz_chufang1 set peiyaobz = 1 where chufangid = '{0}'";
                        DBVisitor.ExecuteNonQuery(string.Format(sqlPYBZ, InObject.orderno));
                    }
                    else
                    {
                        //OutObject.ResultContent = "正在配药中，请稍后!";
                    }
                }
            }
            else
            {
                throw new Exception("未找到相应的处方信息");
            }
        }
    }
}
