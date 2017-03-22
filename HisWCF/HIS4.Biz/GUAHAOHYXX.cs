using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Data;
using HIS4.Schemas;
using System.Configuration;

namespace HIS4.Biz
{
    public class GUAHAOHYXX : IMessage<GUAHAOHYXX_IN, GUAHAOHYXX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new GUAHAOHYXX_OUT();
            string guahaoFs = InObject.GUAHAOFS;//挂号方式
            string riQi = InObject.RIQI;//日期
            string guahaoBc = InObject.GUAHAOBC;//挂号班次
            string keshiDm = InObject.KESHIDM;//科室代码
            string yishengDm = InObject.YISHENGDM;//医生代码
            string guahaolb = InObject.GUAHAOLB;//挂号类别
            string yuyueLx = InObject.YUYUELX;//预约类型
            string FenYuanDM = InObject.BASEINFO.FENYUANDM;//分院代码
            string HaoYuanXSMS = InObject.HAOYUANXSMS;//号源明细聚合方式 0 关闭 1 开启
            string guaHaoHYMS = ConfigurationManager.AppSettings["GuaHaoYMS"];//挂号号源模式
            string WuPaiBanKSXS = ConfigurationManager.AppSettings["MZKSWPBXSKSXX"];//无排班时科室显示
            
            #region 基本入参判断
            //挂号号源模式
            if (string.IsNullOrEmpty(guaHaoHYMS))
            {
                guaHaoHYMS = "0";
            }

            if (string.IsNullOrEmpty(yuyueLx)) {
                yuyueLx = ConfigurationManager.AppSettings["GuaHaoYYLX"];//预约类型
            }

            //挂号方式
            if (string.IsNullOrEmpty(guahaoFs))
            {
                throw new Exception("挂号方式获取失败，请重新尝试挂号！");

            }
            else if (!(guahaoFs == "0" || guahaoFs == "1" || guahaoFs == "2"))
            {
                throw new Exception("挂号方式信息错误，必须符合：0全部 1挂号 2预约 ！");

            }
            //挂号班次
            if (string.IsNullOrEmpty(guahaoBc))
            {
                throw new Exception("号源时间获取失败，请重新尝试挂号！");
            }
            else if (!(guahaoBc == "0" || guahaoBc == "1" || guahaoBc == "2"))
            {
                throw new Exception("号源时间信息错误，必须符合：0全天 1上午 2下午！");
            }
            //挂号类别
            if (string.IsNullOrEmpty(guahaolb))
            {
                throw new Exception("挂号类别获取失败，请重新尝试挂号！");
            }
            
            //挂号日期
            if (string.IsNullOrEmpty(riQi))
            {
                riQi = string.Empty;
            }
            else if (ConfigurationManager.AppSettings["GuaHaoYYDTHY"] != "1" &&(DateTime.ParseExact(riQi + " 00:00:00", "yyyy-MM-dd HH:mm:ss", null) < DateTime.Now) && guahaoFs == "2")
            {
                throw new Exception("预约日期必须大于当天日期！");
            }
            //科室代码
            if (string.IsNullOrEmpty(keshiDm))
            {
                throw new Exception("科室信息获取失败，请重新尝试挂号！");
            }

            //医生代码
            if (string.IsNullOrEmpty(yishengDm))
            {
                yishengDm = "*";
            }

            //号源明细聚合
            if (string.IsNullOrEmpty(HaoYuanXSMS))
            {
                HaoYuanXSMS = "0";
            }

            if (string.IsNullOrEmpty(WuPaiBanKSXS)) {
                WuPaiBanKSXS = "0";
            }
            #endregion

            if (guaHaoHYMS == "0")//有具体号源模式
            {
                #region 挂号号源信息 -- 当日挂号
                //if (guahaoFs == "1")
                //{
                //    if (string.IsNullOrEmpty(riQi))
                //    {
                //        riQi = DateTime.Now.ToString("yyyy-MM-dd");
                //    }
                //    #region 查询语句拼装
                //    StringBuilder sbSql = new StringBuilder();
                //    sbSql.Append("select to_char(a.riqi,'yyyy-mm-dd') riqi,'' guahaobc, ");
                //    sbSql.Append("a.guahaolb guahaolb,a.keshiid keshidm,a.yishengid yishengdm, ");
                //    sbSql.Append("'0' guahaoxh,''jiuzhensj,''yizhoupbid,a.paibanid dangtianpbid, ");
                //    sbSql.Append("nvl(d1.danjia1,0) as zhenliaofje,nvl(d2.danjia1,0) as guahaofje ");
                //    sbSql.Append("from mz_v_guahaopb_ex_zzj a , gy_keshi b, gy_zhigongxx c,gy_shoufeixm d1,gy_shoufeixm d2,gy_menzhenlb e  ");
                //    sbSql.Append("where b.keshiid = a.keshiid and c.zhigongid(+)=a.yishengid and e.leibieid(+)=a.guahaolb ");
                //    sbSql.Append("and d1.shoufeixmid(+)=a.zhenliaofxm and d2.shoufeixmid(+)=a.guahaofxm ");
                //    sbSql.Append("and a.yuanquid = '" + FenYuanDM + "' ");
                //    sbSql.Append(" and a.guahaolb = " + guahaolb + " ");
                //    sbSql.Append("and to_char(a.riqi,'yyyy-mm-dd')='" + riQi + "' ");

                //    //挂号班次
                //    if (guahaoBc == "0")
                //    {
                //        sbSql.Append(" and ((a.shangwuxh>0 and a.shangwuxh > a.shangwuygh) or (a.xiawuxh>0 and a.xiawuxh > a.xiawuygh)) ");
                //    }
                //    else if (guahaoBc == "1")
                //    {
                //        sbSql.Append(" and a.shangwuxh>0 and a.shangwuxh > a.shangwuygh ");
                //    }
                //    else if (guahaoBc == "2")
                //    {
                //        sbSql.Append(" and a.xiawuxh>0 and a.xiawuxh > a.xiawuygh ");
                //    }

                //    sbSql.Append(" and a.keshiid='" + keshiDm + "' ");
                //    sbSql.Append(" and nvl(a.yishengid ,'*') ='" + yishengDm + "' ");
                //    sbSql.Append("order by b.keshimc,c.zhigongxm,a.yishengid ");
                //    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "普通挂号号源信息查询：" + sbSql.ToString(), messageId);
                //    #endregion

                //    #region 号源信息数据拼装
                //    DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
                //    if (dt.Rows.Count > 0)
                //    {

                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            HAOYUANXX hyxx = new HAOYUANXX();
                //            hyxx.RIQI = dt.Rows[i]["RIQI"].ToString();
                //            hyxx.GUAHAOBC = dt.Rows[i]["GUAHAOBC"].ToString();
                //            hyxx.GUAHAOLB = dt.Rows[i]["GUAHAOLB"].ToString();
                //            hyxx.KESHIDM = dt.Rows[i]["KESHIDM"].ToString();
                //            hyxx.YISHENGDM = dt.Rows[i]["YISHENGDM"].ToString();
                //            hyxx.GUAHAOXH = dt.Rows[i]["GUAHAOXH"].ToString();
                //            hyxx.JIUZHENSJ = dt.Rows[i]["JIUZHENSJ"].ToString();
                //            hyxx.YIZHOUPBID = dt.Rows[i]["DANGTIANPBID"].ToString();
                //            hyxx.DANGTIANPBID = dt.Rows[i]["DANGTIANPBID"].ToString();
                //            hyxx.GUAHAOFY = dt.Rows[i]["guahaofje"].ToString();
                //            hyxx.ZHENLIAOFY = dt.Rows[i]["zhenliaofje"].ToString();
                //            OutObject.HAOYUANMX.Add(hyxx);
                //        }
                //    }
                //    #endregion
                //}
                #endregion

                #region 挂号号源信息 -- 当日挂号
                if (guahaoFs == "1" || guahaoFs == "0")
                {
                    string RQ = "";
                    if (string.IsNullOrEmpty(riQi))
                    {
                        RQ = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        RQ = riQi;
                    }
                    #region 查询语句拼装
                    StringBuilder sbSql = new StringBuilder();

                    if (guahaoBc == "1" || guahaoBc == "0")
                    {

                        sbSql.Append("select a.shangwuxh as xianhaos,a.shangwuygh as yiguahaos,to_char(a.riqi,'yyyy-mm-dd') riqi,'1' guahaobc, ");
                        sbSql.Append("a.guahaolb guahaolb,a.keshiid keshidm,a.yishengid yishengdm, ");
                        sbSql.Append("'0' guahaoxh,''jiuzhensj,''yizhoupbid,a.paibanid dangtianpbid, ");
                        sbSql.Append("nvl(d1.danjia1,0) as zhenliaofje,nvl(d2.danjia1,0) as guahaofje ");
                        sbSql.Append("from mz_v_guahaopb_ex_zzj a , gy_keshi b, gy_zhigongxx c,gy_shoufeixm d1,gy_shoufeixm d2,gy_menzhenlb e  ");
                        sbSql.Append("where b.keshiid = a.keshiid and c.zhigongid(+)=a.yishengid and e.leibieid(+)=a.guahaolb ");
                        sbSql.Append("and d1.shoufeixmid(+)=a.zhenliaofxm and d2.shoufeixmid(+)=a.guahaofxm ");
                        sbSql.Append("and a.yuanquid = '" + FenYuanDM + "' ");
                        if (!string.IsNullOrEmpty(guahaolb) && guahaolb != "0")
                            sbSql.Append(" and a.guahaolb = " + guahaolb + " ");
                        sbSql.Append("and to_char(a.riqi,'yyyy-mm-dd')='" + RQ + "' ");
                        if (WuPaiBanKSXS != "1")
                        {
                            //挂号班次
                            sbSql.Append(" and a.shangwuxh>0 and a.shangwuxh > a.shangwuygh ");
                        }

                        sbSql.Append(" and a.keshiid='" + keshiDm + "' ");
                        sbSql.Append(" and nvl(a.yishengid ,'*') ='" + yishengDm + "' ");
                    }

                    if (guahaoBc == "0")
                    {
                        sbSql.Append(" union all ");
                    }

                    if (guahaoBc == "2" || guahaoBc == "0")
                    {
                        sbSql.Append("select a.xiawuxh as xianhaos,a.xiawuygh as yiguahaos,to_char(a.riqi,'yyyy-mm-dd') riqi,'2' guahaobc, ");
                        sbSql.Append("a.guahaolb guahaolb,a.keshiid keshidm,a.yishengid yishengdm, ");
                        sbSql.Append("'0' guahaoxh,''jiuzhensj,''yizhoupbid,a.paibanid dangtianpbid, ");
                        sbSql.Append("nvl(d1.danjia1,0) as zhenliaofje,nvl(d2.danjia1,0) as guahaofje ");
                        sbSql.Append("from mz_v_guahaopb_ex_zzj a , gy_keshi b, gy_zhigongxx c,gy_shoufeixm d1,gy_shoufeixm d2,gy_menzhenlb e  ");
                        sbSql.Append("where b.keshiid = a.keshiid and c.zhigongid(+)=a.yishengid and e.leibieid(+)=a.guahaolb ");
                        sbSql.Append("and d1.shoufeixmid(+)=a.zhenliaofxm and d2.shoufeixmid(+)=a.guahaofxm ");
                        sbSql.Append("and a.yuanquid = '" + FenYuanDM + "' ");
                        if (!string.IsNullOrEmpty(guahaolb) && guahaolb != "0")
                            sbSql.Append(" and a.guahaolb = " + guahaolb + " ");
                        sbSql.Append("and to_char(a.riqi,'yyyy-mm-dd')='" + RQ + "' ");
                        if (WuPaiBanKSXS != "1")
                        {
                            sbSql.Append(" and a.xiawuxh>0 and a.xiawuxh > a.xiawuygh ");
                        }

                        sbSql.Append(" and a.keshiid='" + keshiDm + "' ");
                        sbSql.Append(" and nvl(a.yishengid ,'*') ='" + yishengDm + "' ");
                    }
                    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "普通挂号号源信息查询：" + sbSql.ToString(), messageId);
                    #endregion

                    #region 号源信息数据拼装
                    DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
                    if (dt.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            HAOYUANXX hyxx = new HAOYUANXX();
                            hyxx.RIQI = dt.Rows[i]["RIQI"].ToString();
                            hyxx.GUAHAOBC = dt.Rows[i]["GUAHAOBC"].ToString();
                            hyxx.GUAHAOLB = dt.Rows[i]["GUAHAOLB"].ToString();
                            hyxx.KESHIDM = dt.Rows[i]["KESHIDM"].ToString();
                            hyxx.YISHENGDM = dt.Rows[i]["YISHENGDM"].ToString();
                            hyxx.GUAHAOXH = dt.Rows[i]["GUAHAOXH"].ToString();
                            hyxx.JIUZHENSJ = dt.Rows[i]["JIUZHENSJ"].ToString();
                            hyxx.YIZHOUPBID = dt.Rows[i]["DANGTIANPBID"].ToString();
                            hyxx.DANGTIANPBID = dt.Rows[i]["DANGTIANPBID"].ToString();
                            hyxx.GUAHAOFY = dt.Rows[i]["guahaofje"].ToString();
                            hyxx.ZHENLIAOFY = dt.Rows[i]["zhenliaofje"].ToString();

                            var xhs = Convert.ToInt32( dt.Rows[i]["xianhaos"].ToString()??"0");
                            var yghs = Convert.ToInt32( dt.Rows[i]["yiguahaos"].ToString()??"0");
                            string YuYueHYSL = "select guahaoxh from mz_guahaoyy where paibanid = '{0}' and to_char(yuyuesj,'yyyy-mm-dd') = '{1}' and shangxiawbz = {2} and yuyuelx in ({3}) and yuyuezt in (0)";
                            DataTable YuYueHYSLTable = DBVisitor.ExecuteTable(string.Format(YuYueHYSL, hyxx.DANGTIANPBID, hyxx.RIQI, hyxx.GUAHAOBC, yuyueLx));
                            var yyhs = YuYueHYSLTable.Rows.Count;
                            hyxx.SHENGYUHYS = xhs <= 0 ? "-1" : (xhs == 9999 ? "9999" : (xhs - yghs - yyhs).ToString());

                            OutObject.HAOYUANMX.Add(hyxx);
                        }
                    }
                    #endregion
                }
                #endregion

                #region 挂号号源信息 -- 预约挂号
                if (guahaoFs == "2")
                {
                    #region 查询语句拼装
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append("select to_char(a.riqi,'yyyy-mm-dd') as riqi,'' guahaobc,a.guahaolb,a.keshiid keshidm,a.yishengid yishengdm, ");
                    sbSql.Append("b.shangwuyyxh as shangwuxh,b.xiawuyyxh as xiawuxh,b.shangwuyyxhmx,b.xiawuyyxhmx,'' jiuzhensj, ");
                    sbSql.Append("'' yizhoupbid,a.paibanid dangtianpbid , a.zhenliaofxm,a.guahaofxm ,b.yuyuelx ");
                    sbSql.Append(" from mz_v_guahaopb_ex_zzj a,v_mz_guahaoyyxh b where b.paibanid=a.paibanid ");
                    sbSql.Append("and b.yuyuelx in (" + yuyueLx + ") ").Append("and a.yuanquid = '" + FenYuanDM + "' ");

                    //日期
                    if (string.IsNullOrEmpty(riQi))
                    {
                        //挂号预约可预约当天号源 0 不启用 1 启用
                        if (ConfigurationManager.AppSettings["GuaHaoYYDTHY"] == "1")
                        {
                            sbSql.Append(" and a.riqi >= trunc(sysdate) ");
                        }
                        else
                        {
                            sbSql.Append(" and a.riqi > trunc(sysdate) ");
                        }
                    }
                    else
                    {
                        sbSql.Append(" and to_char(a.riqi,'yyyy-mm-dd') ='" + riQi + "' ");
                    }
                    //挂号班次
                    if (guahaoBc == "0")
                    {
                        sbSql.Append(" and (b.shangwuyyxh>0 or b.xiawuyyxh > 0) ");
                    }
                    else if (guahaoBc == "1")
                    {
                        sbSql.Append(" and b.shangwuyyxh > 0 ");
                    }
                    else if (guahaoBc == "2")
                    {
                        sbSql.Append(" and b.xiawuyyxh > 0 ");
                    }
                    //科室代码
                    sbSql.Append(" and a.keshiid ='" + keshiDm + "' ");
                    //医生代码
                    sbSql.Append(" and nvl(a.yishengid,'*') ='" + yishengDm + "' ");


                    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "预约挂号号源信息查询：" + sbSql.ToString(), messageId);
                    #endregion

                    #region 号源信息数据拼装
                    DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
                    int swXianHao = 0, xwXianHao = 0; //限号
                    int swShengYuHS = 0, xwShengYuHS = 0;//剩余号数
                    string swGuahaoXh = "", xwGuahaoXh = "";  //序号明细
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        riQi = dt.Rows[i]["riqi"].ToString();

                        swXianHao = int.Parse(dt.Rows[i]["shangwuxh"].ToString() == "" ? "0" : dt.Rows[i]["shangwuxh"].ToString());
                        xwXianHao = int.Parse(dt.Rows[i]["xiawuxh"].ToString() == "" ? "0" : dt.Rows[i]["xiawuxh"].ToString());
                        swGuahaoXh = dt.Rows[i]["shangwuyyxhmx"].ToString();//上午明细
                        xwGuahaoXh = dt.Rows[i]["xiawuyyxhmx"].ToString();//下午明细

                        if (swGuahaoXh != string.Empty || xwGuahaoXh != string.Empty)//预约明细不为空时
                        {
                            #region 上午号源明细
                            if (guahaoBc.Trim() == "1" || guahaoBc.Trim() == "0")
                            {
                                if (swGuahaoXh != string.Empty)
                                {
                                    #region 获取已挂号、预约信息
                                    string xhStr = "^";
                                    if (guahaoFs.Trim() == "1" || guahaoFs == "0")
                                    {
                                        #region 上午已挂序号
                                        string xhSql1 = "select guahaoxh from mz_guahao1 where paibanid = '" + dt.Rows[i]["dangtianpbid"].ToString()
                                             + "' and to_char(guahaorq,'yyyy-mm-dd') = '" + riQi + "' and shangxiawbz = 0";
                                        DataTable xhTable = DBVisitor.ExecuteTable(xhSql1.ToString());
                                        for (int xh = 0; xh < xhTable.Rows.Count; xh++)
                                        {
                                            xhStr += xhTable.Rows[xh]["guahaoxh"] + "^";
                                        }
                                        #endregion
                                    }
                                    if (guahaoFs.Trim() == "2" || guahaoFs == "0")
                                    {
                                        #region 上午已预约序号
                                        string xhSql2 = "select guahaoxh from mz_guahaoyy where paibanid = '" + dt.Rows[i]["dangtianpbid"].ToString()
                                             + "' and to_char(yuyuesj,'yyyy-mm-dd') = '" + riQi.Trim() + "' and shangxiawbz = 0 and yuyuelx in ( "+yuyueLx+") and yuyuezt in (0,1)";
                                        DataTable xhTable = DBVisitor.ExecuteTable(xhSql2.ToString());
                                        swShengYuHS = swXianHao - xhTable.Rows.Count; //上午剩余号源数量
                                        
                                        for (int xh = 0; xh < xhTable.Rows.Count; xh++)
                                        {
                                            xhStr += xhTable.Rows[xh]["guahaoxh"].ToString().Trim() + "^";
                                        }
                                        #endregion
                                    }
                                    #endregion
                                    string[] swGhxhSplit = swGuahaoXh.Split('^');
                                    string swSingleXh = string.Empty;
                                    if (HaoYuanXSMS == "0")
                                    {
                                        for (int j = 0; j < swGhxhSplit.Length; j++)
                                        {
                                            swSingleXh = swGhxhSplit[j];
                                            if (xhStr.IndexOf("^" + swSingleXh.Trim() + "^") != -1)
                                                continue;
                                            if (swSingleXh != string.Empty)
                                            {
                                                HAOYUANXX hyxx = new HAOYUANXX();
                                                hyxx.RIQI = dt.Rows[i]["riqi"].ToString();
                                                hyxx.GUAHAOBC = "1";
                                                hyxx.GUAHAOLB = dt.Rows[i]["guahaolb"].ToString(); ;
                                                hyxx.KESHIDM = dt.Rows[i]["keshidm"].ToString();
                                                hyxx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();
                                                hyxx.GUAHAOXH = swSingleXh;
                                                hyxx.JIUZHENSJ = Unity.getJiuZhenSJD(yishengDm, dt.Rows[i]["keshidm"].ToString(), swSingleXh, yuyueLx, 0, hyxx.GUAHAOLB, riQi);
                                                hyxx.YIZHOUPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                                hyxx.DANGTIANPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                                hyxx.YUYUELX = dt.Rows[i]["yuyuelx"].ToString();

                                                string guahaofydm = dt.Rows[i]["guahaofxm"].ToString();
                                                string zhenliaofydm = dt.Rows[i]["zhenliaofxm"].ToString();
                                                hyxx.GUAHAOFY = Unity.GetXiangMuFY(guahaofydm);
                                                hyxx.ZHENLIAOFY = Unity.GetXiangMuFY(zhenliaofydm);
                                                hyxx.SHENGYUHYS = swShengYuHS.ToString();
                                                OutObject.HAOYUANMX.Add(hyxx);
                                            }
                                        }
                                    }
                                    else if (HaoYuanXSMS == "1")
                                    {
                                        HAOYUANXX hyxx = new HAOYUANXX();
                                        for (int j = 0; j < swGhxhSplit.Length; j++)
                                        {
                                            swSingleXh = swGhxhSplit[j];
                                            if (xhStr.IndexOf("^" + swSingleXh.Trim() + "^") != -1)
                                                continue;
                                            hyxx.RIQI = dt.Rows[i]["riqi"].ToString();
                                            hyxx.GUAHAOBC = "1";
                                            hyxx.GUAHAOLB = dt.Rows[i]["guahaolb"].ToString(); 
                                            hyxx.KESHIDM = dt.Rows[i]["keshidm"].ToString();
                                            hyxx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();
                                            hyxx.YUYUELX = dt.Rows[i]["yuyuelx"].ToString();

                                            hyxx.YIZHOUPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                            hyxx.DANGTIANPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                            string guahaofydm = dt.Rows[i]["guahaofxm"].ToString();
                                            string zhenliaofydm = dt.Rows[i]["zhenliaofxm"].ToString();
                                            hyxx.GUAHAOFY = Unity.GetXiangMuFY(guahaofydm);
                                            hyxx.ZHENLIAOFY = Unity.GetXiangMuFY(zhenliaofydm);

                                            if (swSingleXh != string.Empty)
                                            {
                                                hyxx.JIUZHENSJ += "|" + Unity.getJiuZhenSJD(yishengDm, dt.Rows[i]["keshidm"].ToString(), swSingleXh, yuyueLx, 0, hyxx.GUAHAOLB, riQi);
                                                hyxx.GUAHAOXH += "|" + swSingleXh;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(hyxx.GUAHAOXH))
                                        {
                                            //hyxx.JIUZHENSJ = hyxx.JIUZHENSJ.Trim('|');
                                            hyxx.GUAHAOXH = hyxx.GUAHAOXH.Trim('|');
                                            if (hyxx.JIUZHENSJ[0] == '|')
                                            {
                                                hyxx.JIUZHENSJ = hyxx.JIUZHENSJ.Substring(1);
                                                //hyxx.JIUZHENSJ = hyxx.JIUZHENSJ.TrimEnd('|');
                                            }
                                            //if (hyxx.GUAHAOXH[1] == '|')
                                            //{
                                            //    hyxx.GUAHAOXH = hyxx.GUAHAOXH.Substring(1);
                                            //    //hyxx.GUAHAOXH = hyxx.GUAHAOXH.TrimEnd('|');
                                            //}
                                            hyxx.SHENGYUHYS = swShengYuHS.ToString();
                                            OutObject.HAOYUANMX.Add(hyxx);
                                        }
                                    }

                                }
                            }
                            #endregion

                            #region 下午号源明细
                            if (guahaoBc.Trim() == "2" || guahaoBc.Trim() == "0")
                            {
                                if (xwGuahaoXh != string.Empty)
                                {
                                    #region 获取已挂号、预约信息
                                    string xhStr = "^";
                                    if (guahaoFs.Trim() == "2" || guahaoFs == "0")
                                    {
                                        #region 下午已预约序号
                                        string xhSql2 = "select guahaoxh from mz_guahaoyy where paibanid = '" + dt.Rows[i]["dangtianpbid"].ToString()
                                             + "' and to_char(yuyuesj,'yyyy-mm-dd') = '" + riQi + "' and shangxiawbz = 1 and yuyuelx in ( " + yuyueLx + ")  and yuyuezt in (0,1)";
                                        DataTable xhTable = DBVisitor.ExecuteTable(xhSql2.ToString());
                                        xwShengYuHS = xwXianHao - xhTable.Rows.Count;//下午剩余号源
                                        for (int xh = 0; xh < xhTable.Rows.Count; xh++)
                                        {
                                            xhStr += xhTable.Rows[xh]["guahaoxh"].ToString().Trim() + "^";
                                        }
                                        #endregion
                                    }
                                    #endregion
                                    string[] xwGhxhSplit = xwGuahaoXh.Split('^');
                                    string xwSingleXh = string.Empty;
                                    if (HaoYuanXSMS == "0")
                                    {
                                        for (int j = 0; j < xwGhxhSplit.Length; j++)
                                        {
                                            xwSingleXh = xwGhxhSplit[j].Trim();
                                            if (xhStr.IndexOf("^" + xwSingleXh + "^") != -1)
                                                continue;
                                            if (xwSingleXh != string.Empty)
                                            {
                                                HAOYUANXX hyxx = new HAOYUANXX();
                                                hyxx.RIQI = dt.Rows[i]["riqi"].ToString();
                                                hyxx.GUAHAOBC = "2";
                                                hyxx.GUAHAOLB = dt.Rows[i]["guahaolb"].ToString(); ;
                                                hyxx.KESHIDM = dt.Rows[i]["keshidm"].ToString();
                                                hyxx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();
                                                hyxx.GUAHAOXH = xwSingleXh;
                                                hyxx.JIUZHENSJ = Unity.getJiuZhenSJD(yishengDm, dt.Rows[i]["keshidm"].ToString(), xwSingleXh, yuyueLx, 1, hyxx.GUAHAOLB, riQi);
                                                hyxx.YIZHOUPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                                hyxx.DANGTIANPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                                hyxx.YUYUELX = dt.Rows[i]["yuyuelx"].ToString();
                                                string guahaofydm = dt.Rows[i]["guahaofxm"].ToString();
                                                string zhenliaofydm = dt.Rows[i]["zhenliaofxm"].ToString();
                                                hyxx.GUAHAOFY = Unity.GetXiangMuFY(guahaofydm);
                                                hyxx.ZHENLIAOFY = Unity.GetXiangMuFY(zhenliaofydm);
                                                hyxx.SHENGYUHYS = xwShengYuHS.ToString();
                                                OutObject.HAOYUANMX.Add(hyxx);
                                            }
                                        }
                                    }
                                    else if (HaoYuanXSMS == "1")
                                    {
                                        HAOYUANXX hyxx = new HAOYUANXX();
                                        for (int j = 0; j < xwGhxhSplit.Length; j++)
                                        {

                                            hyxx.RIQI = dt.Rows[i]["riqi"].ToString();
                                            hyxx.GUAHAOBC = "2";
                                            hyxx.GUAHAOLB = dt.Rows[i]["guahaolb"].ToString(); ;
                                            hyxx.KESHIDM = dt.Rows[i]["keshidm"].ToString();
                                            hyxx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();
                                            hyxx.YUYUELX = dt.Rows[i]["yuyuelx"].ToString();
                                            hyxx.YIZHOUPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                            hyxx.DANGTIANPBID = dt.Rows[i]["dangtianpbid"].ToString();

                                            string guahaofydm = dt.Rows[i]["guahaofxm"].ToString();
                                            string zhenliaofydm = dt.Rows[i]["zhenliaofxm"].ToString();
                                            hyxx.GUAHAOFY = Unity.GetXiangMuFY(guahaofydm);
                                            hyxx.ZHENLIAOFY = Unity.GetXiangMuFY(zhenliaofydm);

                                            xwSingleXh = xwGhxhSplit[j].Trim();
                                            if (xhStr.IndexOf("^" + xwSingleXh + "^") != -1)
                                                continue;
                                            if (xwSingleXh != string.Empty)
                                            {
                                                hyxx.JIUZHENSJ += "|" + Unity.getJiuZhenSJD(yishengDm, dt.Rows[i]["keshidm"].ToString(), xwSingleXh, yuyueLx, 1, hyxx.GUAHAOLB, riQi);
                                                hyxx.GUAHAOXH += "|" + xwSingleXh;


                                            }
                                        }
                                        if (!string.IsNullOrEmpty(hyxx.GUAHAOXH))
                                        {
                                            hyxx.GUAHAOXH = hyxx.GUAHAOXH.Trim('|');
                                            if (hyxx.JIUZHENSJ[0] == '|')
                                            {
                                                hyxx.JIUZHENSJ = hyxx.JIUZHENSJ.Substring(1);
                                                //hyxx.JIUZHENSJ = hyxx.JIUZHENSJ.TrimEnd('|');
                                            }
                                            //if (hyxx.GUAHAOXH[1] == '|')
                                            //{
                                            //    hyxx.GUAHAOXH = hyxx.GUAHAOXH.Substring(1);
                                            //    //hyxx.GUAHAOXH = hyxx.GUAHAOXH.TrimEnd('|');
                                            //}
                                            hyxx.SHENGYUHYS = xwShengYuHS.ToString();
                                            OutObject.HAOYUANMX.Add(hyxx);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else//预约明细为空时
                        {
                            #region 预约明细为空时
                            if (swXianHao >= 100) swXianHao = 100;
                            if (xwXianHao >= 100) xwXianHao = 100;//限制最高限号为100
                            if (swXianHao > 0 && (guahaoBc == "1" || guahaoBc == "0"))
                            {
                                string xhStr = "^";
                                string xwSingleXh = string.Empty;
                                #region 上午已预约序号
                                string xhSql2 = "select guahaoxh from mz_guahaoyy where paibanid = '" + dt.Rows[i]["dangtianpbid"].ToString()
                                     + "' and to_char(yuyuesj,'yyyy-mm-dd') = '" + riQi.Trim() + "' and shangxiawbz = 0 and yuyuezt in (0,1)";
                                DataTable xhTable = DBVisitor.ExecuteTable(xhSql2.ToString());
                                for (int xh = 0; xh < xhTable.Rows.Count; xh++)
                                {
                                    xhStr += xhTable.Rows[xh]["guahaoxh"].ToString().Trim() + "^";
                                }
                                #endregion
                                for (int a = 1; a < swXianHao; a++)
                                {
                                    xwSingleXh = a.ToString();
                                    if (xhStr.IndexOf("^" + xwSingleXh + "^") != -1)
                                        continue;
                                    HAOYUANXX hyxx = new HAOYUANXX();
                                    hyxx.RIQI = dt.Rows[i]["riqi"].ToString();
                                    hyxx.GUAHAOBC = "上午";
                                    hyxx.GUAHAOLB = dt.Rows[i]["guahaolb"].ToString(); ;
                                    hyxx.KESHIDM = dt.Rows[i]["keshidm"].ToString();
                                    hyxx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();
                                    hyxx.GUAHAOXH = xwSingleXh;
                                    hyxx.JIUZHENSJ = Unity.getJiuZhenSJD(yishengDm, dt.Rows[i]["keshidm"].ToString(), xwSingleXh, yuyueLx, 0, hyxx.GUAHAOLB, riQi);
                                    hyxx.YIZHOUPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                    hyxx.DANGTIANPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                    OutObject.HAOYUANMX.Add(hyxx);
                                }
                            }
                            if (xwXianHao > 0 && (guahaoBc == "2" || guahaoBc == "0"))
                            {
                                string xhStr = "^";
                                string xwSingleXh = string.Empty;
                                #region 下午已预约序号
                                string xhSql2 = "select guahaoxh from mz_guahaoyy where paibanid = '" + dt.Rows[i]["dangtianpbid"].ToString()
                                     + "' and to_char(yuyuesj,'yyyy-mm-dd') = '" + riQi + "' and shangxiawbz = 1 and yuyuezt in (0,1)";
                                DataTable xhTable = DBVisitor.ExecuteTable(xhSql2.ToString());
                                for (int xh = 0; xh < xhTable.Rows.Count; xh++)
                                {
                                    xhStr += xhTable.Rows[xh]["guahaoxh"].ToString().Trim() + "^";
                                }
                                #endregion
                                for (int a = 1; a < xwXianHao; a++)
                                {

                                    xwSingleXh = a.ToString();
                                    if (xhStr.IndexOf("^" + xwSingleXh + "^") != -1)
                                        continue;
                                    xwSingleXh = a.ToString();
                                    if (xhStr.IndexOf("^" + xwSingleXh + "^") != -1)
                                        continue;
                                    HAOYUANXX hyxx = new HAOYUANXX();
                                    hyxx.RIQI = dt.Rows[i]["riqi"].ToString();
                                    hyxx.GUAHAOBC = "上午";
                                    hyxx.GUAHAOLB = dt.Rows[i]["guahaolb"].ToString(); ;
                                    hyxx.KESHIDM = dt.Rows[i]["keshidm"].ToString();
                                    hyxx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();
                                    hyxx.GUAHAOXH = xwSingleXh;
                                    hyxx.JIUZHENSJ = Unity.getJiuZhenSJD(yishengDm, dt.Rows[i]["keshidm"].ToString(), xwSingleXh, yuyueLx, 0, hyxx.GUAHAOLB,riQi);
                                    hyxx.YIZHOUPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                    hyxx.DANGTIANPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                    OutObject.HAOYUANMX.Add(hyxx);
                                }
                            }
                            #endregion
                        }

                    }
                    #endregion
                }
                #endregion
            }
            else if (guaHaoHYMS == "1")
            {
            
            }
            else {//无具体号源模式 - 有好换序号明细范围
                #region 挂号号源信息 -- 当日挂号
                if (guahaoFs == "1" || guahaoFs == "0")
                {
                    string RQ = "";
                    if (string.IsNullOrEmpty(riQi))
                    {
                        RQ = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else {
                        RQ = riQi;
                    }
                    #region 查询语句拼装
                    StringBuilder sbSql = new StringBuilder();

                    if (guahaoBc == "1" || guahaoBc == "0")
                    {

                        sbSql.Append("select to_char(a.riqi,'yyyy-mm-dd') riqi,'1' guahaobc, ");
                        sbSql.Append("a.guahaolb guahaolb,a.keshiid keshidm,a.yishengid yishengdm, ");
                        sbSql.Append("'0' guahaoxh,''jiuzhensj,''yizhoupbid,a.paibanid dangtianpbid, ");
                        sbSql.Append("nvl(d1.danjia1,0) as zhenliaofje,nvl(d2.danjia1,0) as guahaofje ");
                        sbSql.Append("from mz_v_guahaopb_ex_zzj a , gy_keshi b, gy_zhigongxx c,gy_shoufeixm d1,gy_shoufeixm d2,gy_menzhenlb e  ");
                        sbSql.Append("where b.keshiid = a.keshiid and c.zhigongid(+)=a.yishengid and e.leibieid(+)=a.guahaolb ");
                        sbSql.Append("and d1.shoufeixmid(+)=a.zhenliaofxm and d2.shoufeixmid(+)=a.guahaofxm ");
                        sbSql.Append("and a.yuanquid = '" + FenYuanDM + "' ");
                        if (!string.IsNullOrEmpty(guahaolb) && guahaolb != "0")
                            sbSql.Append(" and a.guahaolb = " + guahaolb + " ");
                        sbSql.Append("and to_char(a.riqi,'yyyy-mm-dd')='" + RQ + "' ");
                        
                        //挂号班次
                        sbSql.Append(" and a.shangwuxh>0 and a.shangwuxh > a.shangwuygh ");


                        sbSql.Append(" and a.keshiid='" + keshiDm + "' ");
                        sbSql.Append(" and nvl(a.yishengid ,'*') ='" + yishengDm + "' ");
                    }

                    if (guahaoBc == "0") {
                        sbSql.Append(" union all ");
                    }

                    if (guahaoBc == "2" || guahaoBc == "0")
                    {
                        sbSql.Append("select to_char(a.riqi,'yyyy-mm-dd') riqi,'2' guahaobc, ");
                        sbSql.Append("a.guahaolb guahaolb,a.keshiid keshidm,a.yishengid yishengdm, ");
                        sbSql.Append("'0' guahaoxh,''jiuzhensj,''yizhoupbid,a.paibanid dangtianpbid, ");
                        sbSql.Append("nvl(d1.danjia1,0) as zhenliaofje,nvl(d2.danjia1,0) as guahaofje ");
                        sbSql.Append("from mz_v_guahaopb_ex_zzj a , gy_keshi b, gy_zhigongxx c,gy_shoufeixm d1,gy_shoufeixm d2,gy_menzhenlb e  ");
                        sbSql.Append("where b.keshiid = a.keshiid and c.zhigongid(+)=a.yishengid and e.leibieid(+)=a.guahaolb ");
                        sbSql.Append("and d1.shoufeixmid(+)=a.zhenliaofxm and d2.shoufeixmid(+)=a.guahaofxm ");
                        sbSql.Append("and a.yuanquid = '" + FenYuanDM + "' ");
                        if (!string.IsNullOrEmpty(guahaolb) && guahaolb != "0")
                            sbSql.Append(" and a.guahaolb = " + guahaolb + " ");
                        sbSql.Append("and to_char(a.riqi,'yyyy-mm-dd')='" + RQ + "' ");

                        sbSql.Append(" and a.xiawuxh>0 and a.xiawuxh > a.xiawuygh ");

                        sbSql.Append(" and a.keshiid='" + keshiDm + "' ");
                        sbSql.Append(" and nvl(a.yishengid ,'*') ='" + yishengDm + "' ");
                    }
                    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "普通挂号号源信息查询：" + sbSql.ToString(), messageId);
                    #endregion

                    #region 号源信息数据拼装
                    DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
                    if (dt.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            HAOYUANXX hyxx = new HAOYUANXX();
                            hyxx.RIQI = dt.Rows[i]["RIQI"].ToString();
                            hyxx.GUAHAOBC = dt.Rows[i]["GUAHAOBC"].ToString();
                            hyxx.GUAHAOLB = dt.Rows[i]["GUAHAOLB"].ToString();
                            hyxx.KESHIDM = dt.Rows[i]["KESHIDM"].ToString();
                            hyxx.YISHENGDM = dt.Rows[i]["YISHENGDM"].ToString();
                            hyxx.GUAHAOXH = dt.Rows[i]["GUAHAOXH"].ToString();
                            hyxx.JIUZHENSJ = dt.Rows[i]["JIUZHENSJ"].ToString();
                            hyxx.YIZHOUPBID = dt.Rows[i]["DANGTIANPBID"].ToString();
                            hyxx.DANGTIANPBID = dt.Rows[i]["DANGTIANPBID"].ToString();
                            hyxx.GUAHAOFY = dt.Rows[i]["guahaofje"].ToString();
                            hyxx.ZHENLIAOFY = dt.Rows[i]["zhenliaofje"].ToString();
                            OutObject.HAOYUANMX.Add(hyxx);
                        }
                    }
                    #endregion
                }
                #endregion

                #region 挂号号源信息 -- 预约挂号
                if (guahaoFs == "2"||guahaoFs == "0")
                {
                    #region 查询语句拼装
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append("select to_char(a.riqi,'yyyy-mm-dd') as riqi,'' guahaobc,a.guahaolb,a.keshiid keshidm,a.yishengid yishengdm, ");
                    sbSql.Append("a.shangwuxh,a.xiawuxh,b.shangwuyyxhmx,b.xiawuyyxhmx,b.shangwuyyyh,b.xiawuyyyh,'' jiuzhensj, ");
                    sbSql.Append("'' yizhoupbid,a.paibanid dangtianpbid , a.zhenliaofxm,a.guahaofxm ,b.QiShiHYXH ");
                    sbSql.Append("from mz_v_guahaopb_ex_zzj a,v_mz_guahaoyyxh b where b.paibanid=a.paibanid ");
                    //sbSql.Append("and b.yuyuelx = " + yuyueLx + " ");
                    sbSql.Append("and a.yuanquid = '" + FenYuanDM + "' ");

                    //日期
                    if (string.IsNullOrEmpty(riQi))
                    {
                        sbSql.Append(" and a.riqi > trunc(sysdate) ");
                    }
                    else
                    {
                        sbSql.Append(" and to_char(a.riqi,'yyyy-mm-dd') ='" + riQi + "' ");
                    }
                    //挂号班次
                    if (guahaoBc == "0")
                    {
                        sbSql.Append(" and (b.shangwuyyxh>0 or b.xiawuyyxh > 0) ");
                    }
                    else if (guahaoBc == "1")
                    {
                        sbSql.Append(" and b.shangwuyyxh > 0 ");
                    }
                    else if (guahaoBc == "2")
                    {
                        sbSql.Append(" and b.xiawuyyxh > 0 ");
                    }
                    //科室代码
                    sbSql.Append(" and a.keshiid ='" + keshiDm + "' ");
                    //医生代码
                    sbSql.Append(" and nvl(a.yishengid ,'*') ='" + yishengDm + "' ");


                    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "预约挂号号源信息查询：" + sbSql.ToString(), messageId);
                    #endregion

                    #region 号源信息数据拼装
                    DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
                    int swXianHao = 0, xwXianHao = 0; //限号
                    int swGuahaoXh = 0, xwGuahaoXh = 0;  //序号明细
                    int swYGuahaoXhs, xwYGuahaoXhs;//已挂号个数
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        riQi = dt.Rows[i]["riqi"].ToString();

                        swXianHao = int.Parse(dt.Rows[i]["shangwuxh"].ToString() == "" ? "0" : dt.Rows[i]["shangwuxh"].ToString());
                        xwXianHao = int.Parse(dt.Rows[i]["xiawuxh"].ToString() == "" ? "0" : dt.Rows[i]["xiawuxh"].ToString());
                        swGuahaoXh = int.Parse(dt.Rows[i]["shangwuyyxhmx"].ToString());//上午明细
                        xwGuahaoXh = int.Parse(dt.Rows[i]["xiawuyyxhmx"].ToString());//下午明细
                        swYGuahaoXhs = int.Parse(dt.Rows[i]["shangwuyyyh"].ToString());//下午已挂号数量
                        xwYGuahaoXhs = int.Parse(dt.Rows[i]["xiawuyyyh"].ToString());//下午已挂号数量
                        //string QiShiHYXH = ConfigurationManager.AppSettings["QiShiHYXH"];//起始号源
                        string QiShiHYXH = dt.Rows[i]["QiShiHYXH"].ToString();//起始号源
                        int qishixh = 1;

                        string yuYueHYXSMS = ConfigurationManager.AppSettings["YYHYXSMS"];//预约号源显示模式

                        if (string.IsNullOrEmpty(yuYueHYXSMS)) {

                            yuYueHYXSMS = "0";
                        }
                        
                        try
                        {
                            if (!string.IsNullOrEmpty(QiShiHYXH))
                            {
                                qishixh = int.Parse(QiShiHYXH);
                            }
                        }
                        catch (Exception ex)
                        {
                            qishixh = 1;
                        }
                        #region 预约明细为空时
                        if (swXianHao > 0 && (guahaoBc == "1" || guahaoBc == "0"))
                        {
                            string xhStr = "^";
                            string swSingleXh = string.Empty;
                            #region 上午已预约序号
                            string xhSql2 = "select guahaoxh from mz_guahaoyy where paibanid = '" + dt.Rows[i]["dangtianpbid"].ToString()
                                 + "' and to_char(yuyuesj,'yyyy-mm-dd') = '" + riQi.Trim() + "' and shangxiawbz = 0 and yuyuezt in (0,1)";
                            DataTable xhTable = DBVisitor.ExecuteTable(xhSql2.ToString());
                            for (int xh = 0; xh < xhTable.Rows.Count; xh++)
                            {
                                xhStr += xhTable.Rows[xh]["guahaoxh"].ToString().Trim() + "^";
                            }
                            #endregion
                            if (HaoYuanXSMS == "0")
                            {
                                for (int a = qishixh; a < qishixh + swGuahaoXh; a++)
                                {
                                    swSingleXh = a.ToString();
                                    //if (xhStr.IndexOf("^" + swSingleXh + "^") != -1)
                                    //    continue;
                                    if (xhStr.IndexOf("^" + swSingleXh + "^") != -1)
                                    {
                                        if (yuYueHYXSMS == "0")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            swSingleXh = "*" + swSingleXh;
                                        }
                                    }
                                    HAOYUANXX hyxx = new HAOYUANXX();
                                    hyxx.RIQI = dt.Rows[i]["riqi"].ToString();
                                    hyxx.GUAHAOBC = "1";
                                    hyxx.GUAHAOLB = dt.Rows[i]["guahaolb"].ToString();
                                    hyxx.KESHIDM = dt.Rows[i]["keshidm"].ToString();
                                    hyxx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();
                                    hyxx.GUAHAOXH = swSingleXh;
                                    hyxx.JIUZHENSJ = ConfigurationManager.AppSettings["ShangWuQHSJ"];//上午预约取号时间

                                    hyxx.DANGTIANPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                    hyxx.YIZHOUPBID = GetYiZhouPBID(hyxx.DANGTIANPBID);
                                    OutObject.HAOYUANMX.Add(hyxx);
                                }
                            }
                            else {
                                HAOYUANXX hyxx = new HAOYUANXX();
                                for (int a = qishixh; a < qishixh + swGuahaoXh; a++)
                                {
                                    swSingleXh = a.ToString();
                                    if (xhStr.IndexOf("^" + swSingleXh + "^") != -1)
                                    {
                                        if (yuYueHYXSMS == "0")
                                        {
                                            continue;
                                        }
                                        else {
                                            swSingleXh = "*" + swSingleXh;
                                        }
                                    }
                                    
                                    hyxx.RIQI = dt.Rows[i]["riqi"].ToString();
                                    hyxx.GUAHAOBC = "1";
                                    hyxx.GUAHAOLB = dt.Rows[i]["guahaolb"].ToString();
                                    hyxx.KESHIDM = dt.Rows[i]["keshidm"].ToString();
                                    hyxx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();
                                   
                                    if (swSingleXh != string.Empty && swSingleXh != "*" )
                                    {
                                        hyxx.JIUZHENSJ += "|" + ConfigurationManager.AppSettings["ShangWuQHSJ"];//上午预约取号时间
                                        hyxx.GUAHAOXH += "|" + swSingleXh;
                                    }
                                    hyxx.DANGTIANPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                    hyxx.YIZHOUPBID = GetYiZhouPBID(hyxx.DANGTIANPBID);
                                    
                                }
                                if (!string.IsNullOrEmpty(hyxx.GUAHAOXH))
                                {
                                    hyxx.JIUZHENSJ = hyxx.JIUZHENSJ.Trim('|');
                                    hyxx.GUAHAOXH = hyxx.GUAHAOXH.Trim('|');
                                    OutObject.HAOYUANMX.Add(hyxx);
                                }
                            }
                        }
                        if (xwXianHao > 0 && (guahaoBc == "2" || guahaoBc == "0"))
                        {
                            string xhStr = "^";
                            string xwSingleXh = string.Empty;
                            #region 下午已预约序号
                            string xhSql2 = "select guahaoxh from mz_guahaoyy where paibanid = '" + dt.Rows[i]["dangtianpbid"].ToString()
                                 + "' and to_char(yuyuesj,'yyyy-mm-dd') = '" + riQi + "' and shangxiawbz = 1 and yuyuezt in (0,1)";
                            DataTable xhTable = DBVisitor.ExecuteTable(xhSql2.ToString());
                            for (int xh = 0; xh < xhTable.Rows.Count; xh++)
                            {
                                xhStr += xhTable.Rows[xh]["guahaoxh"].ToString().Trim() + "^";
                            }
                            #endregion
                            if (HaoYuanXSMS == "0")
                            {
                                for (int a = qishixh; a < qishixh + xwGuahaoXh; a++)
                                {

                                    xwSingleXh = a.ToString();
                                    //if (xhStr.IndexOf("^" + xwSingleXh + "^") != -1)
                                    //    continue;

                                    if (xhStr.IndexOf("^" + xwSingleXh + "^") != -1)
                                    {
                                        if (yuYueHYXSMS == "0")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            xwSingleXh = "*" + xwSingleXh;
                                        }
                                    }

                                    HAOYUANXX hyxx = new HAOYUANXX();
                                    hyxx.RIQI = dt.Rows[i]["riqi"].ToString();
                                    hyxx.GUAHAOBC = "2";
                                    hyxx.GUAHAOLB = dt.Rows[i]["guahaolb"].ToString();
                                    hyxx.KESHIDM = dt.Rows[i]["keshidm"].ToString();
                                    hyxx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();
                                    hyxx.GUAHAOXH = xwSingleXh;
                                    hyxx.JIUZHENSJ = ConfigurationManager.AppSettings["XiaWuQHSJ"];//下午预约取号时间

                                    hyxx.DANGTIANPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                    hyxx.YIZHOUPBID = GetYiZhouPBID(hyxx.DANGTIANPBID);
                                    OutObject.HAOYUANMX.Add(hyxx);
                                }
                            }
                            else
                            {
                                HAOYUANXX hyxx = new HAOYUANXX();
                                for (int a = qishixh; a < qishixh + swGuahaoXh; a++)
                                {
                                    xwSingleXh = a.ToString();
                                    //if (xhStr.IndexOf("^" + xwSingleXh + "^") != -1)
                                    //    continue;
                                    if (xhStr.IndexOf("^" + xwSingleXh + "^") != -1)
                                    {
                                        if (yuYueHYXSMS == "0")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            xwSingleXh = "*" + xwSingleXh;
                                        }
                                    }
                                    hyxx.RIQI = dt.Rows[i]["riqi"].ToString();
                                    hyxx.GUAHAOBC = "2";
                                    hyxx.GUAHAOLB = dt.Rows[i]["guahaolb"].ToString();
                                    hyxx.KESHIDM = dt.Rows[i]["keshidm"].ToString();
                                    hyxx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();

                                    if (xwSingleXh != string.Empty)
                                    {
                                        hyxx.JIUZHENSJ += "|" + ConfigurationManager.AppSettings["XiaWuQHSJ"];//上午预约取号时间
                                        hyxx.GUAHAOXH += "|" + xwSingleXh;
                                    }
                                    hyxx.DANGTIANPBID = dt.Rows[i]["dangtianpbid"].ToString();
                                    hyxx.YIZHOUPBID = GetYiZhouPBID(hyxx.DANGTIANPBID);

                                }
                                if (!string.IsNullOrEmpty(hyxx.GUAHAOXH))
                                {
                                    hyxx.JIUZHENSJ = hyxx.JIUZHENSJ.Trim('|');
                                    hyxx.GUAHAOXH = hyxx.GUAHAOXH.Trim('|');
                                    OutObject.HAOYUANMX.Add(hyxx);
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            if (OutObject.HAOYUANMX.Count <= 0)//返回数据空
            {
                throw new Exception("该科室或医生号源没有排班或已挂完！");
            }
        }

        public string GetYiZhouPBID(string dangTianPaiBanID)
        {
            string paiBanIDSql = string.Format("select b.paibanid from mz_v_guahaopb_ex_zzj a,mz_yizhoupb b where a.paibanid = {0} and a.yuanquid=b.yuanquid and a.keshiid = b.keshiid and nvl(a.yishengid,'*') = nvl(b.yishengid,'*') and a.guahaolb = b.guahaolb and a.XINGQI = b.xingqi",dangTianPaiBanID);
            string dt = DBVisitor.ExecuteScalar(paiBanIDSql.ToString()).ToString();
            if (string.IsNullOrEmpty(dt))
            {
                return string.Empty;
            }
            return dt;
        }
    }
}
