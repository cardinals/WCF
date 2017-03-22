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
    public class DISP_START : HKCMessage<DISP_START_IN,DISP_START_OUT>
    {
        public override void ProcessMessage()
        {
            string chuFangHM = InObject.pharmacyid.Split('*')[1];//处方id 多个时使用|分割
            string bingRenID = InObject.pharmacyid.Split('*')[0];//病人id

            #region 基本入参判断
            if (string.IsNullOrEmpty(chuFangHM))
            {
                throw new Exception("处方ID不能为空");
            }
            if (string.IsNullOrEmpty(bingRenID))
            {
                throw new Exception("病人ID不能为空");
            }
            #endregion
            
            chuFangHM = chuFangHM.Replace("#", "','");

            string sqlChuFang = "select chufangid,chufanghm,to_char(kaidanrq,'yyyy-mm-dd') kaidanrq,chuangkouid,bingrenxm from mz_chufang1 where bingrenid ='{0}' and chufangid in(select max(chufangid) from mz_chufang1 where chufanghm in('{1}') group by chufanghm) ";

            DataTable dtChuFang = DBVisitor.ExecuteTable(string.Format(sqlChuFang, bingRenID, chuFangHM));

            string name = string.Empty;
            string shiBaiCF = string.Empty;//发送失败的处方号码

            if (dtChuFang.Rows.Count > 0)
            {
                name = dtChuFang.Rows[0]["bingrenxm"].ToString();
                for(int i = 0;i<dtChuFang.Rows.Count;i++){
                    DISP_START_IN SCtemp = new DISP_START_IN();
                    HKC.Schemas.Start.order dtemp = new HKC.Schemas.Start.order();
                    string no = dtChuFang.Rows[i]["chufangid"].ToString();
                    string odate = dtChuFang.Rows[i]["kaidanrq"].ToString();
                    string winno = dtChuFang.Rows[i]["chuangkouid"].ToString();
                    string sqlJiuZhenKH = "select jiuzhenkh from gy_bingrenxx where bingrenid ='{0}'";
                    SCtemp.pid = (string)DBVisitor.ExecuteScalar(string.Format(sqlJiuZhenKH, bingRenID));
                    SCtemp.name = name;
                    SCtemp.codekey = InObject.codekey;
                    dtemp.no = no;
                    dtemp.odate = odate;
                    dtemp.winno = winno;
                    SCtemp.orderinfo.Add(dtemp);
                    string yfid = "select yingyongid from my_chuangkou where chuangkouid = '{0}' ";
                    DataTable dtYFID = DBVisitor.ExecuteTable(string.Format(yfid, winno));
                    if (dtYFID.Rows.Count > 0)
                    {
                        SCtemp.pharmacyid = dtYFID.Rows[0]["yingyongid"].ToString();
                    }
                    else {
                        //获取药房id失败
                        shiBaiCF += "," + no;
                        continue;
                    }

                    DISP_START_OUT outTemp = Unity.runService<DISP_START_IN, DISP_START_OUT>(SCtemp);
                    //上传失败
                    if (outTemp.ResultCode != "0") {
                        shiBaiCF += "," + no;
                        continue;
                    }
                }

                if (!string.IsNullOrEmpty(shiBaiCF)) {
                    //返回失败的处方号码
                    throw new Exception("处方号为：" + shiBaiCF.Trim(',').ToString() + "的处方上传失败！");
                }

            }
            else {
                throw new Exception("未找到相关的处方信息");
            }

        }
    }
}
