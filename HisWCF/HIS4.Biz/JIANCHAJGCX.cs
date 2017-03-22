using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HIS4.Schemas;
using JYCS.Schemas;
using System.Data;
using SWSoft.Framework;

namespace HIS4.Biz
{
    public class JIANCHAJGCX : IMessage<JIANCHAJGCX_IN, JIANCHAJGCX_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new JIANCHAJGCX_OUT();
            string shenQingDID = InObject.SHENQINDANID;//检查申请单ID

            #region 基础入参判断
            if (string.IsNullOrEmpty(shenQingDID))
            {
                throw new Exception("检查申请单ID不能为空！");
            }

            string shenQingDQRSql = "select * from yj_shenqingdan where shenqindanid = '{0}'";
            DataTable dtShenQingDQR = DBVisitor.ExecuteTable(string.Format(shenQingDQRSql, shenQingDID));
            if (dtShenQingDQR == null || dtShenQingDQR.Rows.Count <= 0) {
                throw new Exception("未找到该申请单的相关信息，请确认！");
            }
            #endregion

            string shenQingBGSql = " select * from YJ_V_JIANCHABG where shenqindanid = '{0}' ";
            DataTable dtShenQingBG = DBVisitor.ExecuteTable(string.Format(shenQingBGSql, shenQingDID));
            if (dtShenQingBG == null || dtShenQingBG.Rows.Count <= 0)
            {
                throw new Exception("检查还未出报告！");
            }
            else {

                for (int i = 0; i < dtShenQingBG.Rows.Count; i++) {
                    HIS4.Schemas.JIANCHAJGXX jcjgxx = new HIS4.Schemas.JIANCHAJGXX();
                    jcjgxx.SHENQINDANID = dtShenQingBG.Rows[i]["shenqindanid"].ToString();//申请单id
                    jcjgxx.YIZHUID = dtShenQingBG.Rows[i]["yizhuid"].ToString();//医嘱id
                    jcjgxx.YIZHUXMID = dtShenQingBG.Rows[i]["yizhuxmid"].ToString();//医嘱项目id
                    jcjgxx.YIZHUMC = dtShenQingBG.Rows[i]["yizhumc"].ToString();//医嘱名称
                    jcjgxx.JIUZHENID = dtShenQingBG.Rows[i]["jiuzhenid"].ToString();//就诊id
                    jcjgxx.BINGRENID = dtShenQingBG.Rows[i]["bingrenid"].ToString();//病人id
                    jcjgxx.BINGRENZYID = dtShenQingBG.Rows[i]["BINGRENZYID"].ToString();//病人住院id
                    jcjgxx.JIANCHALX = dtShenQingBG.Rows[i]["JIANCHALX"].ToString();//检查类型
                    jcjgxx.JIANCHALXMC = dtShenQingBG.Rows[i]["JIANCHALXMC"].ToString();//检查类型名称
                    jcjgxx.KAIDANYS = dtShenQingBG.Rows[i]["kaidanys"].ToString();//开单医生
                    jcjgxx.KAIDANSJ = dtShenQingBG.Rows[i]["kaidansj"].ToString();//开单时间
                    jcjgxx.MOBANDM = dtShenQingBG.Rows[i]["mobandm"].ToString();//模板代码
                    jcjgxx.ZHUSU = dtShenQingBG.Rows[i]["zhusu"].ToString();//主诉
                    jcjgxx.LINCHUANGZD = dtShenQingBG.Rows[i]["linchuangzd"].ToString();//临床诊断
                    jcjgxx.JIANCHABW = dtShenQingBG.Rows[i]["jianchabw"].ToString();//检查部位
                    jcjgxx.JIANCHASJ = dtShenQingBG.Rows[i]["jianchasj"].ToString();//检查时间
                    jcjgxx.ZHENDUANJG = dtShenQingBG.Rows[i]["zhenduanjg"].ToString();//诊断结果
                    jcjgxx.BAOGAOSJ = dtShenQingBG.Rows[i]["baogaosj"].ToString();//报告时间
                    jcjgxx.MENZHENZYBZ = dtShenQingBG.Rows[i]["menzhenzybz"].ToString();//门诊住院标识
                    jcjgxx.DANGQIANZT = dtShenQingBG.Rows[i]["dangqianzt"].ToString();//当前状态
                    jcjgxx.DANGQIANZTMC = dtShenQingBG.Rows[i]["dangqianztmc"].ToString();//当前状态名称
                    OutObject.JIANCHAJGMX.Add(jcjgxx);
                }
            }
            

        }
    }
}
