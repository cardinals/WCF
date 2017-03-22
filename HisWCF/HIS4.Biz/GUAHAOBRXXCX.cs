using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Data;
using HIS4.Schemas;

namespace HIS4.Biz
{
    public class GUAHAOBRXXCX : IMessage<GUAHAOBRXXCX_IN,GUAHAOBRXXCX_OUT>
    {
        public override void ProcessMessage()
        {
            string kaishiRq = InObject.KAISHIRQ;
            string jieshuRq = InObject.JIESHURQ;
            string guahaoBc = InObject.GUAHAOBC;
            string keshiDm = InObject.KESHIDM;
            string yishengDm = InObject.YISHENGDM;

            #region 基本入参信息判断
            //if (string.IsNullOrEmpty(kaishiRq)) {
            //    return WcfCommon.returnMessage<GUAHAOBRXXCX_OUT>(-1, messageId, "开始时间获取失败", OutObject, ref tradeOut);
            //}
            //if (string.IsNullOrEmpty(jieshuRq))
            //{
            //    return WcfCommon.returnMessage<GUAHAOBRXXCX_OUT>(-1, messageId, "结束时间获取失败", OutObject, ref tradeOut);
            //}
            #endregion

            StringBuilder sbSql = new StringBuilder("select b.jiuzhenkh,b.xingming,b.xingbie,b.chushengrq,b.lianxirdh lianxidh,a.guahaolb, ");
            sbSql.Append("a.guahaoks keshidm,a.guahaoksmc keshimc,a.guahaoys yishengdm,a.guahaoysxm yishengxm, ");//
            sbSql.Append("a.guahaorq riqi,decode(a.shangxiawbz,0,1,1,2,0) guahaobc,a.guahaoxh,'' jiuzhensj, ");//
            sbSql.Append("c.weizhi jiuzhendd,d.yuyuely,d.yuyuehao quhaomm,decode(a.yuyueid,null,0,1) shifouyy, ");//
            sbSql.Append("e.jiuzhenzt jiuzhenbs,a.guahaoid   ");//
            sbSql.Append("from mz_guahao1 a,GY_BINGRENXX b,mz_guahaopb_ex c,mz_guahaoyy d,zj_jiuzhenxx e ");//
            sbSql.Append("where b.bingrenid=a.bingrenid and c.paibanid=a.paibanid ");
            sbSql.Append("and d.yuyueid(+)= a.yuyueid and e.guahaoid(+)=a.guahaoid ");
            if (kaishiRq != string.Empty)
                sbSql.Append("and a.guahaorq>=to_date('" + kaishiRq + "','yyyy-mm-dd')' ");
            if (jieshuRq != string.Empty)
                sbSql.Append("and a.guahaorq<=to_date('" + jieshuRq + "','yyyy-mm-dd')' ");
            if (guahaoBc != string.Empty)
                sbSql.Append("and a.shangxiawbz='" + guahaoBc + "' ");//？
            if (keshiDm != string.Empty)
                sbSql.Append("and a.guahaoks ='" + keshiDm + "' ");
            if (yishengDm != string.Empty)
                sbSql.Append("and a.guahaoys ='" + yishengDm + "' ");

            DataTable dt = DBVisitor .ExecuteTable(sbSql.ToString());
            if (dt.Rows.Count <= 0)
            {
                throw new Exception("未找到挂号信息！");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GUAHAOBRXX ghbrxx = new GUAHAOBRXX();
                    ghbrxx.JIUZHENKH = dt.Rows[i]["JIUZHENKH"].ToString();
                    ghbrxx.XINGMING = dt.Rows[i]["XINGMING"].ToString();
                    ghbrxx.XINGBIE = dt.Rows[i]["XINGBIE"].ToString();
                    ghbrxx.CHUSHENGRQ = dt.Rows[i]["CHUSHENGRQ"].ToString();
                    ghbrxx.LIANXIDH = dt.Rows[i]["LIANXIDH"].ToString();
                    ghbrxx.GUAHAOLB = dt.Rows[i]["GUAHAOLB"].ToString();
                    ghbrxx.KESHIDM = dt.Rows[i]["KESHIDM"].ToString();
                    ghbrxx.KESHIMC = dt.Rows[i]["KESHIMC"].ToString();
                    ghbrxx.YISHENGDM = dt.Rows[i]["YISHENGDM"].ToString();
                    ghbrxx.YISHENGXM = dt.Rows[i]["YISHENGXM"].ToString();
                    ghbrxx.RIQI = dt.Rows[i]["RIQI"].ToString();
                    ghbrxx.GUAHAOBC = dt.Rows[i]["GUAHAOBC"].ToString();
                    ghbrxx.GUAHAOXH = dt.Rows[i]["GUAHAOXH"].ToString();
                    ghbrxx.JIUZHENDD = dt.Rows[i]["JIUZHENDD"].ToString();
                    ghbrxx.YUYUELY = dt.Rows[i]["YUYUELY"].ToString();
                    ghbrxx.QUHAOMM = dt.Rows[i]["QUHAOMM"].ToString();
                    ghbrxx.SHIFOUYY = dt.Rows[i]["SHIFOUYY"].ToString();
                    ghbrxx.JIUZHENBS = dt.Rows[i]["JIUZHENBS"].ToString();
                    ghbrxx.GUAHAOID = dt.Rows[i]["GUAHAOID"].ToString();
                    //ghbrxx.JIUZHENSJ =
                    OutObject.GUAHAOBRXXLB.Add(ghbrxx);
                }
            }
        }
    }
}
