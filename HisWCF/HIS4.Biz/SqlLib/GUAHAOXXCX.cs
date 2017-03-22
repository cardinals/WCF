using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;
using System.Data;
using HIS4.Schemas;

namespace HIS4.Biz
{
    public class GUAHAOXXCX : IMessage<GUAHAOXXCX_IN,GUAHAOXXCX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new GUAHAOXXCX_OUT();
            string jiuzhenkLx = InObject.JIUZHENKLX;//就诊卡类型
            string jiuzhenKh = InObject.JIUZHENKH;//就诊卡号
            string zhengjianLx = InObject.ZHENGJIANLX;//证件类型
            string zhengjianHm = InObject.ZHENGJIANHM;//证件号码
            string xingMing = InObject.XINGMING;//姓名
            string riQi = InObject.RIQI;//日期
            string guahaoBc = InObject.GUAHAOBC;//挂号班次
            string keshiDm = InObject.KESHIDM;//科室代码
            string yishengDm = InObject.YISHENGDM;//医生代码
            string bingRenId = InObject.BINGRENID;//病人ID
            string yuanQuID = InObject.BASEINFO.FENYUANDM;//院区ID

            #region 基本入参判断
            //就诊卡号
            if (string.IsNullOrEmpty(jiuzhenKh) && string.IsNullOrEmpty(zhengjianHm))
            {
                throw new Exception("就诊卡号获取失败，请重新尝试！");
            }
            //日期
            if (string.IsNullOrEmpty(riQi))
            {
                throw new Exception("日期获取失败，请重新尝试！");
            }
            //挂号班次
            //if (string.IsNullOrEmpty(guahaoBc))
            //{
            //    throw new Exception("挂号班次获取失败，请重新尝试！");
            //}
            //科室代码
            if (string.IsNullOrEmpty(keshiDm))
            {
                keshiDm = "*";
            }
            //医生代码
            if (string.IsNullOrEmpty(yishengDm))
            {
                yishengDm = "*";
            }

            #endregion

            #region 基础信息查询语句
            if (string.IsNullOrEmpty(bingRenId) && !string.IsNullOrEmpty(jiuzhenKh)){
                bingRenId = DBVisitor.ExecuteScalar("select bingrenid from gy_bingrenxx where jiuzhenkh='" + jiuzhenKh + "' or shenfenzh = '" + zhengjianHm + "'").ToString();
            }
             

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" select  guahaoid ,guahaolb,guahaoks keshidm,guahaoksmc keshimc,guahaoys yishengdm,to_char(guahaorq,'yyyy-mm-dd') riqi, ");
            sbSql.Append(" decode(shangxiawbz,0,1,1,2) guahaobc,guahaoxh,nvl(yuyuebz,0) shifouyy,yuyueid  from mz_guahao1 where nvl(zuofeibz,0) = 0 ");
            sbSql.Append(" and yuanquid ='" + yuanQuID + "' ");
            sbSql.Append(" and (bingrenid='" + bingRenId + "') ");
            sbSql.Append(" and (nvl(guahaoks,'*') = '" + keshiDm + "' or '*' = '" + keshiDm + "')  and (nvl(guahaoys,'*') = '" + yishengDm + "' or '*' = '" + yishengDm +"') ");
            //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "挂号信息查询：" + sbSql.ToString(), messageId);
            if (!string.IsNullOrEmpty(riQi)) {
                sbSql.Append(" and to_char(guahaorq,'yyyy-mm-dd') = '"+riQi+"' ");
            }
            #endregion

            #region 基础信息拼装
            DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
            if (dt.Rows.Count <= 0)
            {
                throw new Exception("未找到挂号记录!");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GUAHAOXX ghxx = new GUAHAOXX();
                    ghxx.GUAHAOID = dt.Rows[i]["GUAHAOID"].ToString();
                    ghxx.GUAHAOLB = dt.Rows[i]["GUAHAOLB"].ToString();
                    ghxx.KESHIDM = dt.Rows[i]["KESHIDM"].ToString();
                    ghxx.KESHIMC = dt.Rows[i]["KESHIMC"].ToString();
                    ghxx.YISHENGDM = dt.Rows[i]["YISHENGDM"].ToString();
                    ghxx.RIQI = dt.Rows[i]["RIQI"].ToString();
                    ghxx.GUAHAOBC = dt.Rows[i]["GUAHAOBC"].ToString();
                    ghxx.GUAHAOXH = dt.Rows[i]["GUAHAOXH"].ToString();
                    ghxx.SHIFOUYY = dt.Rows[i]["SHIFOUYY"].ToString();

                    //获取医生姓名
                    if (!string.IsNullOrEmpty(ghxx.YISHENGDM))
                    {
                        DataTable dtYs = DBVisitor.ExecuteTable("select xm from gy_zgxx where zgid ='" + ghxx.YISHENGDM + "' ");
                        //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "挂号信息查询：" + ghxx.YISHENGDM + ":医生信息：" + "select xm from gy_zgxx where zgid ='" + ghxx.YISHENGDM + "' ", messageId);
                        if (dtYs.Rows.Count > 0)
                        {
                            ghxx.YISHENGXM = dtYs.Rows[0]["XM"].ToString();
                        }
                    }
                    //诊间 就诊信息
                    DataTable dtJZ = DBVisitor.ExecuteTable("select to_char(a.jiuzhenrq,'yyyy-mm-dd hh24:mm:dd') jiuzhensj,b.weizhism weizhi,decode(a.jiuzhenzt,0,0,1) jiuzhenbs from zj_jiuzhenxx a, gy_keshi b where a.guahaoks = b.keshiid and a.guahaoid ='" + ghxx.GUAHAOID + "'");
                    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "挂号信息查询：" + ghxx.YISHENGDM + ":诊间信息：" + "select to_char(a.jiuzhenrq,'yyyy-mm-dd hh24:mm:dd') jiuzhensj,b.weizhism weizhi,decode(a.jiuzhenzt,0,0,1) jiuzhenbs from zj_jiuzhenxx a, gy_keshi b where a.guahaoks = b.keshiid and a.guahaoid ='" + ghxx.GUAHAOID + "'", messageId);
                    if (dt.Rows.Count > 0)
                    {
                        ghxx.JIUZHENSJ = dtJZ.Rows[0]["JIUZHENSJ"].ToString();
                        ghxx.JIUZHENDD = dtJZ.Rows[0]["WEIZHI"].ToString();
                        ghxx.JIUZHENBS = dtJZ.Rows[0]["JIUZHENBS"].ToString();
                    }
                    //预约信息
                    if (!string.IsNullOrEmpty(ghxx.SHIFOUYY) && ghxx.SHIFOUYY !="0")
                    {
                        DataTable dtYY = DBVisitor.ExecuteTable(" select yuyuehao，yuyuely from mz_guahaoyy where yuyueid ='" + dt.Rows[i]["YUYUEID"].ToString() + "' ");
                        //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "挂号信息查询：" + dt.Rows[i]["YUYUEID"].ToString() + ":预约信息：" + " select yuyuehao，yuyuely from mz_guahaoyy where yuyueid ='" + dt.Rows[i]["YUYUEID"].ToString() + "' ", messageId);
                        if (dtYY.Rows.Count > 0)
                        {
                            ghxx.QUHAOMM = dtYY.Rows[0]["YUYUEHAO"].ToString();
                            ghxx.YUYUELY = dtYY.Rows[0]["YUYUELY"].ToString();
                        }
                    }
                    OutObject.GUAHAOXXLB.Add(ghxx);
                }
            }
            #endregion

            if (OutObject.GUAHAOXXLB.Count <= 0) {
                throw new Exception("未找到相关的挂号信息！");
            }
        }
    }
}
