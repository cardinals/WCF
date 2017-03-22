using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;
using System.Data;

namespace HIS4.Biz
{
    public class MENZHENTBZLMX : IMessage<MENZHENTBZLMX_IN, MENZHENTBZLMX_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new MENZHENTBZLMX_OUT();

            string BingRenID = InObject.BINGRENID; //病人ID

            if (string.IsNullOrEmpty(BingRenID)) {
                throw new Exception("病人ID不能为空！");
            }

            ///人员信息确认
            var dtBingRenXX = DBVisitor.ExecuteTable(string.Format("select * from gy_v_bingrenxx where bingrenid = '{0}' ",BingRenID));
            if (dtBingRenXX == null || dtBingRenXX.Rows.Count <= 0) {
                throw new Exception(string.Format("未找到病人ID为[{0}]的病人!",BingRenID));
            }
            ///特殊病处方医技信息查询
            DataTable dtTeBingZLXX = DBVisitor.ExecuteTable(string.Format("select * from mz_v_teBingZhenliaoXX where bingrenid = '{0}' ", BingRenID));
            if (dtTeBingZLXX != null && dtTeBingZLXX.Rows.Count > 0) {
                for(int i = 0 ;i< dtTeBingZLXX.Rows.Count;i++){
                TEBINGZLXX temp = new TEBINGZLXX();
                    temp.ZHENLIAOLX = dtTeBingZLXX.Rows[i]["ZHENLIAOLX"].ToString();
                    temp.ZHENLIAOID = dtTeBingZLXX.Rows[i]["ZHENLIAOID"].ToString();
                    temp.BINGRENID = dtTeBingZLXX.Rows[i]["bingrenid"].ToString();
                    temp.SHUXINGZT = dtTeBingZLXX.Rows[i]["SHUXINGZT"].ToString();
                    OutObject.TEBINGZLMX.Add(temp);
                }
            }
        }

    }
}
