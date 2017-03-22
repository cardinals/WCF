using JYCS.Schemas;
using HIS4.Schemas;
using System;
using System.Data;
using SWSoft.Framework;

namespace HIS4.Biz
{
    public class YIZHUXX : IMessage<YIZHUXX_IN, YIZHUXX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new YIZHUXX_OUT();
            string bingRenId = InObject.BINGRENID;//病人住院ID

            #region 基本入参判断
            if (string.IsNullOrEmpty(bingRenId))
            {
                throw new Exception("病人ID不能为空！");
            }
            #endregion

            string pinci = string.Empty;
            DataTable dtYIZHUXX = DBVisitor.ExecuteTable(string.Format("SELECT * FROM V_YZ_BINGRENYZ WHERE BINGRENZYID='{0}'", bingRenId));
            foreach (DataRow dr in dtYIZHUXX.Rows)
            {
                BINGRENYZXX yzxx = new BINGRENYZXX();
                yzxx.STARTTIME = dr["START_TIME"].ToString();
                yzxx.ORDERNAME = dr["ORDER_NAME"].ToString() + dr["PSJG"].ToString();
                yzxx.PHYSICIANNAME = dr["PHYSICIAN_NAME"].ToString();
                pinci = dr["PINCI"].ToString();
                if (pinci == "ONCE" || pinci == "ST")
                {
                    yzxx.ZHIXINGSJ = dr["ZXSJ"].ToString();
                    yzxx.ZHIXINGRENNAME = dr["ZXR1"].ToString();
                }
                else
                {
                    yzxx.ZHIXINGSJ = dr["ZHIXINGSJ"].ToString();
                    yzxx.ZHIXINGRENNAME = dr["ZHIXINGREN_NAME"].ToString();
                }
                OutObject.BINGRENYZXX.Add(yzxx);
            }
        }
    }
}
