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
    public class ZHUYUANFYXX : IMessage<ZHUYUANFYXX_IN, ZHUYUANFYXX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new ZHUYUANFYXX_OUT();
            string bingRenZYID = InObject.BINGRENZYID;//病人id
            string zaiYuanZT = InObject.ZAIYUANZT;//在院状态

            if (string.IsNullOrEmpty(bingRenZYID))
            {
                throw new Exception("病人住院ID不能为空！");
            }


            StringBuilder zhuYuanBRXXSQL = new StringBuilder();

            zhuYuanBRXXSQL.Append(" select * from zy_bingrenxx where bingrenZYid = '{0}' ");
            if(!string.IsNullOrEmpty(zaiYuanZT)){
                if (zaiYuanZT == "0")
                {
                    zhuYuanBRXXSQL.Append(" and zaiyuanzt = 0 ");
                }
                else {
                    zhuYuanBRXXSQL.Append(" and zaiyuanzt != 0 ");
                }
            }
            
            zhuYuanBRXXSQL.Append(" order by ruyuanrq desc ");
            DataTable dtZhuYuanBRXX = DBVisitor.ExecuteTable(string.Format(zhuYuanBRXXSQL.ToString(), bingRenZYID));

            if (dtZhuYuanBRXX.Rows.Count <= 0) {
                throw new Exception("未找到病人住院信息！");
            }

            //string bingRenZYID = dtZhuYuanBRXX.Rows[0]["bingrenzyid"].ToString();//病人住院id
            OutObject.BINGRENZYID = bingRenZYID;

            #region 结算信息
            //基本费用信息
            string sqlZhuYuanJSXX = " select feiyonghj,zifeije,zilije,zifuje from zy_jiesuan1 where jiesuanid in (select jiesuanid from zy_jiesuan1 where bingrenzyid  = '{0}' ) ";
            //预交款信息
            string sqlZhuYanYJK = " select  trim(to_char(nvl(sum(jiaokuanje),0),'999999999.99'))  from zy_yujiaokuan where bingrenzyid = '{0}' ";

            DataTable dtZhuYuanJSXX = DBVisitor.ExecuteTable(string.Format(sqlZhuYuanJSXX, bingRenZYID));

            if (dtZhuYuanJSXX.Rows.Count > 0)
            {
                OutObject.JIESUANJG.FEIYONGZE = dtZhuYuanJSXX.Rows[0]["feiyonghj"].ToString();//费用合计
                OutObject.JIESUANJG.ZILIJE = dtZhuYuanJSXX.Rows[0]["zilije"].ToString();//自理金额
                OutObject.JIESUANJG.ZIFUJE = dtZhuYuanJSXX.Rows[0]["zifuje"].ToString();//自负金额
                OutObject.JIESUANJG.ZIFEIJE = dtZhuYuanJSXX.Rows[0]["zifeije"].ToString();//自费金额
            }
            else {
                OutObject.JIESUANJG.FEIYONGZE = "0.00";//费用合计
                OutObject.JIESUANJG.ZILIJE = "0.00";//自理金额
                OutObject.JIESUANJG.ZIFUJE = "0.00";//自负金额
                OutObject.JIESUANJG.ZIFEIJE = "0.00";//自费金额
            }

            OutObject.JIESUANJG.YUJIAOKZE = DBVisitor.ExecuteScalar(string.Format(sqlZhuYanYJK, bingRenZYID)).ToString();//住院预交款总额


            #endregion


            #region 详细结算结果
            
            #endregion

            #region 费用支付明细
            
            #endregion

            #region 费用归并
            string sqlFeiYongGB = "select trim(to_Char(sum(a.jiesuanjia * a.shuliang),'999999999999999999.99')) je , b.hesuanxmid as xiangmugl ,b.hesuanxmmc as xiangmuglmc "
                + " FROM ZY_FEIYONG1 a,gy_hesuanxm b,zy_bingrenxx c "
                + " WHERE b.hesuanxmid(+)=a.hesuanxm and c.BINGRENZYID = a.bingrenzyid  "
                + " and a.shuliang > 0 and a.feiyongid not in (select yuanfeiyid from zy_feiyong1 where bingrenzyid = '{0}' and shuliang < 0) "
                + " and c.bingrenzyid = '{0}'  group by b.hesuanxmid,b.hesuanxmmc order by b.hesuanxmid ";
            DataTable dtFeiYongGB = DBVisitor.ExecuteTable(string.Format(sqlFeiYongGB, bingRenZYID));//费用归并信息检索
            
            for(int i =0;i<dtFeiYongGB.Rows.Count;i++){
                FEIYONGGLXX fyglxx = new FEIYONGGLXX();
                fyglxx.JINE = dtFeiYongGB.Rows[i]["je"].ToString();
                fyglxx.XIANGMUGL = dtFeiYongGB.Rows[i]["xiangmugl"].ToString();
                fyglxx.XIANGMUGLMC = dtFeiYongGB.Rows[i]["xiangmuglmc"].ToString();
                OutObject.FEIYONGGLMX.Add(fyglxx);
            }
            #endregion

            #region 预交款信息
            string sqlYuJiaoKuan = "select to_char(a.jiaokuanrq,'yyyy-mm-dd') riqi,a.jiaokuanje,b.ZHIFUMC from zy_yujiaokuan a,gy_zhifufs b where a.zhifufs = b.zhifufsid and  bingrenzyid = {0} ";
            DataTable dtYuJiaoKuan = DBVisitor.ExecuteTable(string.Format(sqlYuJiaoKuan, bingRenZYID));

            for (int i = 0; i < dtYuJiaoKuan.Rows.Count; i++) {
                YUJIAOKXX yjkxx = new YUJIAOKXX();
                yjkxx.JIAOKUANRQ = dtYuJiaoKuan.Rows[i]["riqi"].ToString();
                yjkxx.JIAOKUANJE = dtYuJiaoKuan.Rows[i]["jiaokuanje"].ToString();
                yjkxx.ZHIFULX = dtYuJiaoKuan.Rows[i]["zhifumc"].ToString();
                OutObject.YUJIAOKMX.Add(yjkxx);
            }
            #endregion
        }
    }
}
