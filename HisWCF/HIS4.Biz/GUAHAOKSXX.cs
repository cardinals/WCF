using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using System.Configuration;
using SWSoft.Framework;
using System.Data;
using HIS4.Schemas;

namespace HIS4.Biz
{
    public class GUAHAOKSXX : IMessage<GUAHAOKSXX_IN,GUAHAOKSXX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new GUAHAOKSXX_OUT();
            string guahaoFs = InObject.GUAHAOFS;//挂号方式
            string riQi = InObject.RIQI;//日期
            string guahaoBc = InObject.GUAHAOBC;//挂号班次
            string guahaoLb = InObject.GUAHAOLB;//挂号类别
            string yuanquID = InObject.BASEINFO.FENYUANDM;//分院代码
            string YuYueLx = ConfigurationManager.AppSettings["GuaHaoYYLX"];
            string guaHaoHYMS = ConfigurationManager.AppSettings["GuaHaoYMS"];//挂号号源模式
            string WuPaiBanKSXS = ConfigurationManager.AppSettings["MZKSWPBXSKSXX"];//无排班时科室显示

            #region 基本信息有效性判断
            //排班号源模式 0或空 有具体号源模式 1 无具体号源模式 
            if (string.IsNullOrEmpty(guaHaoHYMS)) {
                guaHaoHYMS = "0";
            }

            //挂号方式
            if (string.IsNullOrEmpty(guahaoFs))
            {
                throw new Exception("挂号方式获取失败，请重新尝试挂号！");
            }
            else if (!(guahaoFs == "0" || guahaoFs == "1" || guahaoFs == "2"))
            {
                throw new Exception("挂号方式信息错误，必须符合：1挂号 2预约 ！");
            }
            //挂号班次
            if(string.IsNullOrEmpty(guahaoBc))
            {
                throw new Exception("挂号方式信息错误，必须符合：0全部 1挂号 2预约 ！");
            }
            else if (!(guahaoBc == "0" || guahaoBc == "1" || guahaoBc == "2"))
            {
                throw new Exception("号源时间信息错误，必须符合：0全天 1上午 2下午！");
            }
            //挂号类别
            if (string.IsNullOrEmpty(guahaoLb))
            {
                throw new Exception("挂号类别获取失败，请重新尝试挂号！");
            }
            
            //挂号日期
            if (string.IsNullOrEmpty(riQi))
            {
                riQi = string.Empty;
            }
            //预约类型
            if (!(string.IsNullOrEmpty(YuYueLx)))
            {
                YuYueLx = YuYueLx.Replace("|", ",");
            }
            //无排班时科室显示
            if (string.IsNullOrEmpty(WuPaiBanKSXS))
            {
                WuPaiBanKSXS = "0";
            }

            #endregion

            #region 获取科室信息语句拼装
            StringBuilder sbSql = new StringBuilder();
            if (guaHaoHYMS == "0")//有具体号源模式
            {
                #region 有具体号源

                if (guahaoFs == "1" )
                {
                    #region 普通挂号
                    sbSql.Append("select distinct a.keshiid keshidm,b.keshimc,a.weizhi jiuzhendd, b.keshizl keshijs from mz_v_guahaopb_ex_zzj a,v_gy_keshi b where b.keshiid=a.keshiid");
                    //分院代码
                    sbSql.Append(" and a.yuanquid = '" + yuanquID + "' ");
                    //挂号日期
                    if (string.IsNullOrEmpty(riQi))
                    {
                        sbSql.Append("and to_char(a.riqi,'yyyy-mm-dd') ='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    }
                    else
                    {
                        sbSql.Append("and to_char(a.riqi,'yyyy-mm-dd') ='" + riQi + "' ");
                    }

                    if (ConfigurationManager.AppSettings["XHKSXXXS"] == "1")
                    {
                        //挂号班次
                        if (guahaoBc == "0")//全天
                        {
                            sbSql.Append(" and ((a.shangwuxh>0 ) or (a.xiawuxh>0 )) ");
                        }
                        else if (guahaoBc == "1")//上午
                        {
                            sbSql.Append(" and a.shangwuxh>0 ");
                        }
                        else if (guahaoBc == "2")//下午
                        {
                            sbSql.Append(" and a.xiawuxh>0 ");
                        }
                    }
                    else
                    {
                        //挂号班次
                        if (guahaoBc == "0")//全天
                        {
                            sbSql.Append(" and ((a.shangwuxh>0 and a.shangwuxh > a.shangwuygh) or (a.xiawuxh>0 and a.xiawuxh > a.xiawuygh)) ");
                        }
                        else if (guahaoBc == "1")//上午
                        {
                            sbSql.Append(" and a.shangwuxh>0 and a.shangwuxh > a.shangwuygh ");
                        }
                        else if (guahaoBc == "2")//下午
                        {
                            sbSql.Append(" and a.xiawuxh>0 and a.xiawuxh > a.xiawuygh ");
                        }
                    }
                    //挂号类别
                    if (guahaoLb != "0")
                        sbSql.Append(" and a.guahaolb='" + guahaoLb + "' ");
                    #endregion
                }



                if (guahaoFs == "2" )
                {
                    #region 预约挂号
                    sbSql.Append("select distinct a.keshiid keshidm,b.keshimc,a.weizhi jiuzhendd, b.keshizl keshijs from mz_v_guahaopb_ex_zzj a,v_gy_keshi b ,v_mz_guahaoyyxh c where b.keshiid=a.keshiid and c.paibanid=a.paibanid");
                    //分院代码
                    sbSql.Append(" and a.yuanquid = '" + yuanquID + "' ");
                    //预约类型
                    sbSql.Append(" and (c.yuyuelx in (" + YuYueLx + ")) ");
                    //挂号日期
                    if (!string.IsNullOrEmpty(riQi))
                    {
                        sbSql.Append(" and to_char(a.riqi,'yyyy-mm-dd') ='" + riQi + "' ");
                    }
                    else
                    {
                        //挂号预约可预约当天号源 0 不启用 1 启用
                        if (ConfigurationManager.AppSettings["GuaHaoYYDTHY"] == "1") {
                            sbSql.Append(" and a.riqi >= trunc(sysdate) ");
                        }
                        else
                        {
                            sbSql.Append(" and a.riqi > trunc(sysdate) ");
                        }
                    }
                    //挂号班次
                    if (guahaoBc == "0")//全天
                    {
                        sbSql.Append(" and (c.shangwuyyxh>0 or c.xiawuyyxh>0) ");
                    }
                    else if (guahaoBc == "1")//上午
                    {
                        sbSql.Append(" and c.shangwuyyxh>0 ");
                    }
                    else if (guahaoBc == "2")//下午
                    {
                        sbSql.Append(" and c.xiawuyyxh>0 ");
                    }
                    //挂号类别
                    if (guahaoLb != "0")
                        sbSql.Append(" and a.guahaolb='" + guahaoLb + "' ");
                    #endregion
                }


                #endregion
            }
            else
            {
                #region 无具体号源

                if (guahaoFs == "1" || guahaoFs == "0")
                {
                    #region 普通挂号
                    sbSql.Append("select distinct a.keshiid keshidm,b.keshimc,a.weizhi jiuzhendd, b.keshizl keshijs from mz_v_guahaopb_ex_zzj a,v_gy_keshi b where b.keshiid=a.keshiid");
                    //分院代码
                    sbSql.Append(" and a.yuanquid = '" + yuanquID + "' ");
                    //挂号日期
                    if (string.IsNullOrEmpty(riQi))
                    {
                        sbSql.Append("and to_char(a.riqi,'yyyy-mm-dd') ='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    }
                    else
                    {
                        sbSql.Append("and to_char(a.riqi,'yyyy-mm-dd') ='" + riQi + "' ");
                    }
                    //挂号班次
                    if (guahaoBc == "0")//全天
                    {
                        sbSql.Append(" and ((a.shangwuxh>0 and a.shangwuxh > a.shangwuygh) or (a.xiawuxh>0 and a.xiawuxh > a.xiawuygh)) ");
                    }
                    else if (guahaoBc == "1")//上午
                    {
                        sbSql.Append(" and a.shangwuxh>0 and a.shangwuxh > a.shangwuygh ");
                    }
                    else if (guahaoBc == "2")//下午
                    {
                        sbSql.Append(" and a.xiawuxh>0 and a.xiawuxh > a.xiawuygh ");
                    }
                    //挂号类别
                    if (guahaoLb != "0")
                        sbSql.Append(" and a.guahaolb='" + guahaoLb + "' ");
                    #endregion
                }



                if (guahaoFs == "2" )
                {
                    #region 预约挂号
                    sbSql.Append("select distinct a.keshiid keshidm,b.keshimc,a.weizhi jiuzhendd, b.keshizl keshijs from mz_v_guahaopb_ex_zzj a,v_gy_keshi b ,v_mz_guahaoyyxh c where b.keshiid=a.keshiid and c.paibanid=a.paibanid");
                    //分院代码
                    sbSql.Append(" and a.yuanquid = '" + yuanquID + "' ");
                    //预约类型
                    //sbSql.Append(" and (c.yuyuelx in (" + YuYueLx + ")) ");
                    //挂号日期
                    if (!string.IsNullOrEmpty(riQi))
                    {
                        sbSql.Append(" and to_char(a.riqi,'yyyy-mm-dd') ='" + riQi + "' ");
                    }
                    else
                    {
                        sbSql.Append(" and a.riqi > trunc(sysdate) ");

                    }
                    //挂号班次
                    if (guahaoBc == "0")//全天
                    {
                        sbSql.Append(" and ((a.shangwuxh>0 and a.shangwuxh > a.shangwuygh and c.shangwuyyxh>0 ) or (a.xiawuxh>0 and a.xiawuxh > a.xiawuygh and c.xiawuyyxh>0)) ");
                    }
                    else if (guahaoBc == "1")//上午
                    {
                        sbSql.Append(" and a.shangwuxh>0 and a.shangwuxh > a.shangwuygh ");
                        sbSql.Append(" and c.shangwuyyxh>0 ");
                    }
                    else if (guahaoBc == "2")//下午
                    {
                        sbSql.Append(" and a.xiawuxh>0 and a.xiawuxh > a.xiawuygh ");
                        sbSql.Append(" and c.xiawuyyxh>0 ");
                    }
                    //挂号类别
                    if (guahaoLb != "0")
                        sbSql.Append(" and a.guahaolb='" + guahaoLb + "' ");
                    #endregion
                }

                #endregion
            }
            //sql日志记录
            //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "挂号科室信息查询：" + sbSql.ToString(), messageId);
            #endregion

            #region 获取科室信息
            DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
            if (dt.Rows.Count > 0)
            {
                string guahaoFspb = ConfigurationManager.AppSettings["GUAHAOFSPB"];//无排班时科室显示
                //设置固定科室列表显示模式 为1时 所有列表中的科室无论有没有排班都想被返回
                if (WuPaiBanKSXS == "1" && (guahaoFs == "1" || guahaoFspb == "0"))
                {
                    #region 固定科室模式
                    Dictionary<string, KESHIXX> dicKeShiPaiBanXX = new Dictionary<string, KESHIXX>();

                    #region 有排班的科室
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        KESHIXX ksxx = new KESHIXX();
                        ksxx.KESHIDM = dt.Rows[i]["KESHIDM"].ToString();
                        ksxx.KESHIMC = dt.Rows[i]["KESHIMC"].ToString();
                        ksxx.JIUZHENDD = dt.Rows[i]["JIUZHENDD"].ToString();
                        ksxx.KESHIJS = dt.Rows[i]["KESHIJS"].ToString();
                        if (!dicKeShiPaiBanXX.ContainsKey(ksxx.KESHIDM))
                        {
                            dicKeShiPaiBanXX.Add(ksxx.KESHIDM, ksxx);
                        }
                    }
                    #endregion
                    

                    //检索固定科室信息列表
                    string sqlGuDingKSLB = " select a.keshiid,b.keshimc, b.keshizl keshijs,nvl(a.xianshims,2) xianshims ,a.shunxuhao From zzgh_zhichiks a,v_gy_keshi b where a.KESHIID=b.keshiid and  a.zuofeibz = 0 and a.yuanquid = '{0}' order by xianshims asc,shunxuhao asc,b.keshimc asc ";
                    DataTable dtGuDingKSLB = DBVisitor.ExecuteTable(string.Format(sqlGuDingKSLB, yuanquID));
                    for (int i = 0; i < dtGuDingKSLB.Rows.Count; i++) { 
                        string guDingKeShiID = dtGuDingKSLB.Rows[i]["keshiid"].ToString(); //固定科室代码
                        string guDingKeShiMC = dtGuDingKSLB.Rows[i]["keshimc"].ToString();//固定科室名称
                        string guDingKeShiMS = dtGuDingKSLB.Rows[i]["keshijs"].ToString();//固定科室简述
                        string xianShiMS = dtGuDingKSLB.Rows[i]["xianshims"].ToString();//显示模式 1 固定显示，2有排班显示
                        string xianShiSXh = dtGuDingKSLB.Rows[i]["shunxuhao"].ToString();//顺序号
                        KESHIXX gdksxx = new KESHIXX();
                        gdksxx.KESHIDM = guDingKeShiID;
                        gdksxx.KESHIMC = guDingKeShiMC;
                        gdksxx.KESHIJS = guDingKeShiMS;
                        if (dicKeShiPaiBanXX.ContainsKey(guDingKeShiID))//固定科室是否存在排班
                        {//存在排班
                            KESHIXX ksxx = dicKeShiPaiBanXX[guDingKeShiID];
                            gdksxx.JIUZHENDD = ksxx.JIUZHENDD;
                            gdksxx.ZHUANGTAIBZ = "0";
                        }
                        else {//不存在排班
                            gdksxx.ZHUANGTAIBZ = "1";
                            if (xianShiMS == "2") {
                                continue;
                            }
                        }

                        //科室位置信息
                        if (string.IsNullOrEmpty(gdksxx.JIUZHENDD))
                        {
                            string sqlJZDD = "select weizhism from v_gy_keshi where keshiid ='{0}'";
                            DataTable dtJZDD = DBVisitor.ExecuteTable(string.Format(sqlJZDD, gdksxx.KESHIDM));
                            if (dtJZDD.Rows.Count > 0)
                            {
                                gdksxx.JIUZHENDD = dtJZDD.Rows[0][0].ToString();
                            }
                        }
                        OutObject.KESHIMX.Add(gdksxx);
                    }
                    #endregion
                }
                else {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        KESHIXX ksxx = new KESHIXX();
                        ksxx.KESHIDM = dt.Rows[i]["KESHIDM"].ToString();
                        ksxx.KESHIMC = dt.Rows[i]["KESHIMC"].ToString();
                        ksxx.JIUZHENDD = dt.Rows[i]["JIUZHENDD"].ToString();
                        ksxx.KESHIJS = dt.Rows[i]["KESHIJS"].ToString();
                        ksxx.ZHUANGTAIBZ = "0";
                        if (string.IsNullOrEmpty(ksxx.JIUZHENDD))
                        {
                            string sqlJZDD = "select weizhism from v_gy_keshi where keshiid ='{0}'";
                            DataTable dtJZDD = DBVisitor.ExecuteTable(string.Format(sqlJZDD, ksxx.KESHIDM));
                            if (dtJZDD.Rows.Count > 0)
                            {
                                ksxx.JIUZHENDD = dtJZDD.Rows[0][0].ToString();
                            }
                        }
                        OutObject.KESHIMX.Add(ksxx);
                    }
                }
            }
            #endregion

            if (OutObject.KESHIMX.Count <= 0) {
                throw new Exception("未找到相关科室排班!");
            }
        }
    }
}
