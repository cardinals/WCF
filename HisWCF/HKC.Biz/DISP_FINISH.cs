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
    public class DISP_FINISH : HKCMessage<DISP_FINISH_IN,DISP_FINISH_OUT>
    {
        public override void ProcessMessage()
        {
            string chuFangHM = InObject.pharmacyid.Split('*')[1];//处方id 多个时使用|分割
            string bingRenID = InObject.pharmacyid.Split('*')[0];//病人id
            
            #region 基本入参判断
            if (string.IsNullOrEmpty(chuFangHM))
            {
                throw new Exception("处方号码不能为空");
            }
            if (string.IsNullOrEmpty(bingRenID))
            {
                throw new Exception("病人ID不能为空");
            }
            #endregion

            string sqlCFXX = "select a.chufangid, b.yingyongid yaofangid,a.zhidanren fayaoyaoshi,a.chuangkouid fayaochuangkou from mz_chufang1 a ,my_chuangkou b where a.chuangkouid = b.chuangkouid and a.chufangid in(select max(chufangid) from mz_chufang1 where chufanghm in('{0}') group by chufanghm)  and  bingrenid = '{1}' ";
            DataTable dtCFXX = DBVisitor.ExecuteTable(string.Format(sqlCFXX, chuFangHM, bingRenID));

            if (dtCFXX.Rows.Count > 0)
            {
                InObject.pharmacyid = dtCFXX.Rows[0]["yaofangid"].ToString();
                InObject.winno = dtCFXX.Rows[0]["fayaochuangkou"].ToString();
                InObject.dispoper = dtCFXX.Rows[0]["fayaoyaoshi"].ToString();
                patientid pattemp = new patientid();
                string sqlJiuZhenKH = "select jiuzhenkh from gy_bingrenxx where bingrenid ='{0}'";
                pattemp.pid = (string)DBVisitor.ExecuteScalar(string.Format(sqlJiuZhenKH, bingRenID));
                //pattemp.pid = bingRenID;
                pattemp.orderno = dtCFXX.Rows[0]["chufangid"].ToString();
                InObject.patientidlist.Add(pattemp);

                OutObject = Unity.runService<DISP_FINISH_IN, DISP_FINISH_OUT>(InObject);

            }
            else {
                throw new Exception("未找到相关的处方信息！");
            }
        }
    }
}
