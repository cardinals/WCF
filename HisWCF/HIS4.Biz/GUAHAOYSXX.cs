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
    public class GUAHAOYSXX : IMessage<GUAHAOYSXX_IN,GUAHAOYSXX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new GUAHAOYSXX_OUT();
            string YuYueLx = ConfigurationManager.AppSettings["GuaHaoYYLX"];
            string guahaoFs = InObject.GUAHAOFS;//挂号方式
            string riQi = InObject.RIQI;//日期
            string guahaoBc = InObject.GUAHAOBC;//挂号班次
            string keshiDm = InObject.KESHIDM;//科室代码
            string guahaoLb = InObject.GUAHAOLB;//挂号类别
            string yuanquId = InObject.BASEINFO.FENYUANDM;//分院代码
            string guaHaoHYMS = ConfigurationManager.AppSettings["GuaHaoYMS"];//挂号号源模式
            string WuPaiBanKSXS = ConfigurationManager.AppSettings["MZKSWPBXSKSXX"];//无排班时科室显示

            #region 基本信息有效性判断
            //挂号号源模式
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
            if (string.IsNullOrEmpty(guahaoLb))
            {
                throw new Exception("挂号类别获取失败，请重新尝试挂号！");
            }
            
            //挂号日期
            if (string.IsNullOrEmpty(riQi))
            {
                riQi = string.Empty;
            }
            //科室代码
            if (string.IsNullOrEmpty(keshiDm))
            {
                keshiDm = "*";
            }
            //预约类型
            if (!(string.IsNullOrEmpty(YuYueLx)))
            {
                YuYueLx = YuYueLx.Replace("|", ",");
            }

            if (string.IsNullOrEmpty(WuPaiBanKSXS)) {
                WuPaiBanKSXS = "0";
            }
            #endregion
            if (guaHaoHYMS == "0")//有具体号源模式
            {
                #region 获取普通排班医生信息

                if (guahaoFs == "1" || guahaoFs == "0")
                {
                    #region 医生信息查询语句拼装
                    StringBuilder sqlPTYS = new StringBuilder();//普通排班
                    sqlPTYS.Append(" select a.keshiid,nvl(a.yishengid,'*') yishengdm,a.guahaolb,b.keshimc from mz_v_guahaopb_ex_zzj a,v_gy_keshi b where a.keshiid=b.keshiid  ");
                    //分院代码
                    sqlPTYS.Append(" and a.yuanquid = '" + yuanquId + "' ");
                    //挂号类别
                    if (guahaoLb != "0")
                        sqlPTYS.Append(" and a.guahaolb = " + guahaoLb + " ");
                    //挂号日期
                    if (string.IsNullOrEmpty(riQi))
                    {
                        sqlPTYS.Append(" and to_char(a.riqi,'yyyy-mm-dd') ='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    }
                    else
                    {
                        sqlPTYS.Append(" and to_char(a.riqi,'yyyy-mm-dd') ='" + riQi + "' ");
                    }
                    //挂号科室
                    if (keshiDm != "*")
                    {
                        sqlPTYS.Append(" and a.keshiid= '" + keshiDm + "' ");
                    }
                    if (WuPaiBanKSXS != "1")
                    {
                        //挂号班次
                        if (guahaoBc == "0")
                        {
                            sqlPTYS.Append(" and ((a.shangwuxh>0 and a.shangwuxh > a.shangwuygh) or (a.xiawuxh>0 and a.xiawuxh > a.xiawuygh)) ");
                        }
                        else if (guahaoBc == "1")
                        {
                            sqlPTYS.Append(" and a.shangwuxh>0 and a.shangwuxh > a.shangwuygh ");
                        }
                        else if (guahaoBc == "2")
                        {
                            sqlPTYS.Append(" and a.xiawuxh>0 and a.xiawuxh > a.xiawuygh ");
                        }
                    }
                    //日志记录
                    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "普通挂号医生信息查询：" + sqlPTYS.ToString(), messageId);
                    #endregion

                    #region 医生信息查询
                    DataTable dt = DBVisitor.ExecuteTable(sqlPTYS.ToString());
                    if (dt.Rows.Count > 0)
                    {
                        #region 获取医生信息拼装
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            YISHENGXX ysxx = new YISHENGXX();
                            ysxx.GUAHAOFS = "1";
                            ysxx.KESHIDM = dt.Rows[i]["KESHIID"].ToString();//科室ID
                            ysxx.KESHIMC = dt.Rows[i]["KESHIMC"].ToString();//科室名称
                            ysxx.YISHENGDM = dt.Rows[i]["YISHENGDM"].ToString();//医生代码
                            ysxx.GUAHAOLB = dt.Rows[i]["GUAHAOLB"].ToString();//挂号类别
                            if (ysxx.YISHENGDM == "*")
                            {
                                ysxx.YISHENGXM = "普通";//医生姓名
                                ysxx.YISHENGZC = "医生";//医生职称
                            }
                            else
                            {
                                //日志记录
                                //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "挂号医生详细信息查询：" + sqlPTYS.ToString(), messageId);
                                //获取医生详细信息
                                DataTable dtYSXX = DBVisitor.ExecuteTable(
                                    string.Format("select a.zhigongxm yishengxm,b.daimamc yishengzc from gy_zhigongxx a,(select * from gy_daima where daimalb='0072') b where b.daimaid(+)=a.zhicheng and a.zhigongid = '{0}' ",
                                    ysxx.YISHENGDM)
                                    );
                                if (dtYSXX.Rows.Count > 0)
                                {
                                    ysxx.YISHENGXM = dtYSXX.Rows[0]["YISHENGXM"].ToString();//医生姓名
                                    ysxx.YISHENGZC = dtYSXX.Rows[0]["YISHENGZC"].ToString();//医生职称
                                }
                            }
                            OutObject.YISHENGMX.Add(ysxx);
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #region 获取预约排班医生信息
                if (guahaoFs == "2" || guahaoFs == "0")
                {
                    #region 医生信息查询语句拼装
                    StringBuilder sqlYYYS = new StringBuilder();//预约排班
                    sqlYYYS.Append("select distinct nvl(a.yishengid,'*') YISHENGDM, a.keshiid KESHIDM,c.keshimc,a.guahaolb GUAHAOLB ,e.yuyuelx yuyuelx from  mz_v_guahaopb_ex_zzj a,gy_keshi c ,v_mz_guahaoyyxh e where  c.keshiid=a.keshiid and a.paibanid = e.paibanid ");
                    //分院代码
                    sqlYYYS.Append(" and a.yuanquid = '" + yuanquId + "' ");
                    //预约类型
                    sqlYYYS.Append(" and e.yuyuelx in (" + YuYueLx + ") ");
                    //挂号类别
                    if (guahaoLb != "0")
                        sqlYYYS.Append(" and a.guahaolb = " + guahaoLb + " ");
                    //挂号日期
                    if (!string.IsNullOrEmpty(riQi))
                    {
                        sqlYYYS.Append(" and to_char(a.riqi,'yyyy-mm-dd') ='" + riQi + "' ");
                    }
                    else
                    {
                        //挂号预约可预约当天号源 0 不启用 1 启用
                        if (ConfigurationManager.AppSettings["GuaHaoYYDTHY"] == "1")
                        {
                            sqlYYYS.Append(" and a.riqi >= trunc(sysdate) ");
                        }
                        else
                        {
                            sqlYYYS.Append(" and a.riqi > trunc(sysdate) ");
                        }
                    }
                    //挂号科室
                    if (keshiDm != "*")
                    {
                        sqlYYYS.Append(" and a.keshiid= " + keshiDm + " ");
                    }
                    //挂号班次
                    if (guahaoBc == "0")
                    {
                        sqlYYYS.Append(" and (e.shangwuyyxh>0  or e.xiawuyyxh > 0) ");
                    }
                    else if (guahaoBc == "1")
                    {
                        sqlYYYS.Append(" and e.shangwuyyxh > 0 ");
                    }
                    else if (guahaoBc == "2")
                    {
                        sqlYYYS.Append(" and e.xiawuyyxh > 0 ");
                    }
                    //日志记录
                    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "预约挂号医生信息查询：" + sqlYYYS.ToString(), messageId);
                    #endregion

                    #region 医生信息查询
                    DataTable dt = DBVisitor.ExecuteTable(sqlYYYS.ToString());
                    if (dt.Rows.Count > 0)
                    {
                        #region 获取医生信息拼装
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            YISHENGXX ysxx = new YISHENGXX();
                            ysxx.GUAHAOFS = "2";
                            ysxx.KESHIDM = dt.Rows[i]["KESHIDM"].ToString();//科室ID
                            ysxx.KESHIMC = dt.Rows[i]["KESHIMC"].ToString();//科室名称
                            ysxx.YISHENGDM = dt.Rows[i]["YISHENGDM"].ToString();//医生代码
                            ysxx.GUAHAOLB = dt.Rows[i]["GUAHAOLB"].ToString();//挂号类别
                            ysxx.YUYUELX = dt.Rows[i]["YUYUELX"].ToString();//预约类型
                            if (ysxx.YISHENGDM == "*")
                            {
                                ysxx.YISHENGXM = "普通";//医生姓名
                                ysxx.YISHENGZC = "医生";//医生职称
                            }
                            else
                            {
                                //日志记录
                                //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "挂号医生详细信息查询：" + sqlYYYS.ToString(), messageId);
                                //获取医生详细信息
                                DataTable dtYSXX = DBVisitor.ExecuteTable(
                                    string.Format("select a.zhigongxm yishengxm,b.daimamc yishengzc ,a.zhigongzl from gy_zhigongxx a,(select * from gy_daima where daimalb='0072') b where b.daimaid(+)=a.zhicheng and a.zhigongid = '{0}' ",
                                    ysxx.YISHENGDM)
                                    );
                                if (dtYSXX.Rows.Count > 0)
                                {
                                    ysxx.YISHENGXM = dtYSXX.Rows[0]["YISHENGXM"].ToString();//医生姓名
                                    ysxx.YISHENGZC = dtYSXX.Rows[0]["YISHENGZC"].ToString();//医生职称
                                    ysxx.YISHENGJS = dtYSXX.Rows[0]["zhigongzl"].ToString();//医生介绍
                                }
                            }

                            OutObject.YISHENGMX.Add(ysxx);
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            else { //无具体号源模式
                #region 获取普通排班医生信息

                //if (guahaoFs == "1" || guahaoFs == "0")
                //{
                //    #region 医生信息查询语句拼装
                //    StringBuilder sqlPTYS = new StringBuilder();//普通排班
                //    sqlPTYS.Append(" select a.keshiid,nvl(a.yishengid,'*') yishengdm,a.guahaolb,b.keshimc from mz_v_guahaopb_ex_zzj a,v_gy_keshi b where a.keshiid=b.keshiid  ");
                //    //分院代码
                //    sqlPTYS.Append(" and a.yuanquid = '" + yuanquId + "' ");
                //    //挂号类别
                //    if (guahaoLb != "0")
                //        sqlPTYS.Append(" and a.guahaolb = " + guahaoLb + " ");
                //    //挂号日期
                //    if (string.IsNullOrEmpty(riQi))
                //    {
                //        sqlPTYS.Append(" and to_char(a.riqi,'yyyy-mm-dd') ='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                //    }
                //    else
                //    {
                //        sqlPTYS.Append(" and to_char(a.riqi,'yyyy-mm-dd') ='" + riQi + "' ");
                //    }
                //    //挂号科室
                //    if (keshiDm != "*")
                //    {
                //        sqlPTYS.Append(" and a.keshiid= '" + keshiDm + "' ");
                //    }
                //    //挂号班次
                //    if (guahaoBc == "0")
                //    {
                //        sqlPTYS.Append(" and ((a.shangwuxh>0 and a.shangwuxh > a.shangwuygh) or (a.xiawuxh>0 and a.xiawuxh > a.xiawuygh)) ");
                //    }
                //    else if (guahaoBc == "1")
                //    {
                //        sqlPTYS.Append(" and a.shangwuxh>0 and a.shangwuxh > a.shangwuygh ");
                //    }
                //    else if (guahaoBc == "2")
                //    {
                //        sqlPTYS.Append(" and a.xiawuxh>0 and a.xiawuxh > a.xiawuygh ");
                //    }
                //    //日志记录
                //    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "普通挂号医生信息查询：" + sqlPTYS.ToString(), messageId);
                //    #endregion

                //    #region 医生信息查询
                //    DataTable dt = DBVisitor.ExecuteTable(sqlPTYS.ToString());
                //    if (dt.Rows.Count > 0)
                //    {
                //        #region 获取医生信息拼装
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            YISHENGXX ysxx = new YISHENGXX();
                //            ysxx.GUAHAOFS = "1";
                //            ysxx.KESHIDM = dt.Rows[i]["KESHIID"].ToString();//科室ID
                //            ysxx.KESHIMC = dt.Rows[i]["KESHIMC"].ToString();//科室名称
                //            ysxx.YISHENGDM = dt.Rows[i]["YISHENGDM"].ToString();//医生代码
                //            ysxx.GUAHAOLB = dt.Rows[i]["GUAHAOLB"].ToString();//挂号类别
                //            if (ysxx.YISHENGDM == "*")
                //            {
                //                ysxx.YISHENGXM = "普通";//医生姓名
                //                ysxx.YISHENGZC = "医生";//医生职称
                //            }
                //            else
                //            {
                //                //日志记录
                //                //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "挂号医生详细信息查询：" + sqlPTYS.ToString(), messageId);
                //                //获取医生详细信息
                //                DataTable dtYSXX = DBVisitor.ExecuteTable(
                //                    string.Format("select a.zhigongxm yishengxm,b.daimamc yishengzc ,a.zhigongzl from v_gy_zhigongxx a,(select * from gy_daima where daimalb='0072') b where b.daimaid(+)=a.zhicheng and a.zhigongid = '{0}' ",
                //                    ysxx.YISHENGDM)
                //                    );
                //                if (dtYSXX.Rows.Count > 0)
                //                {
                //                    ysxx.YISHENGXM = dtYSXX.Rows[0]["YISHENGXM"].ToString();//医生姓名
                //                    ysxx.YISHENGZC = dtYSXX.Rows[0]["YISHENGZC"].ToString();//医生职称
                //                    ysxx.YISHENGJS = dtYSXX.Rows[0]["zhigongzl"].ToString();//医生介绍
                //                }
                //            }
                //            OutObject.YISHENGMX.Add(ysxx);
                //        }
                //        #endregion
                //    }
                //    #endregion
                //}
                #endregion

                #region 获取预约排班医生信息
                if (guahaoFs == "2" || guahaoFs == "0")
                {
                    #region 医生信息查询语句拼装
                    StringBuilder sqlYYYS = new StringBuilder();//预约排班
                    sqlYYYS.Append("select distinct nvl(a.yishengid,'*') YISHENGDM, a.keshiid KESHIDM,c.keshimc,a.guahaolb GUAHAOLB ,e.yuyuelx yuyuelx from  mz_v_guahaopb_ex_zzj a,v_gy_keshi c ,v_mz_guahaoyyxh e where  c.keshiid=a.keshiid and a.paibanid = e.paibanid ");
                    //分院代码
                    sqlYYYS.Append(" and a.yuanquid = '" + yuanquId + "' ");
                    //预约类型
                    //sqlYYYS.Append(" and e.yuyuelx in (" + YuYueLx + ") ");
                    //挂号类别
                    if (guahaoLb != "0")
                        sqlYYYS.Append(" and a.guahaolb = " + guahaoLb + " ");
                    //挂号日期
                    if (!string.IsNullOrEmpty(riQi))
                    {
                        sqlYYYS.Append(" and to_char(a.riqi,'yyyy-mm-dd') ='" + riQi + "' ");
                    }
                    else {
                        sqlYYYS.Append(" and a.riqi > trunc(sysdate) ");
                    }
                    //挂号科室
                    if (keshiDm != "*")
                    {
                        sqlYYYS.Append(" and a.keshiid= " + keshiDm + " ");
                    }
                    //挂号班次
                    if (guahaoBc == "0")
                    {
                        sqlYYYS.Append(" and ((a.shangwuxh>0 and a.shangwuxh > a.shangwuygh and e.shangwuyyxh>0 ) or (a.xiawuxh>0 and a.xiawuxh > a.xiawuygh and e.xiawuyyxh > 0)) ");
                    }
                    else if (guahaoBc == "1")
                    {
                        sqlYYYS.Append(" and a.shangwuxh>0 and a.shangwuxh > a.shangwuygh ");
                        sqlYYYS.Append(" and e.shangwuyyxh > 0 ");
                    }
                    else if (guahaoBc == "2")
                    {
                        sqlYYYS.Append(" and a.xiawuxh>0 and a.xiawuxh > a.xiawuygh ");
                        sqlYYYS.Append(" and e.xiawuyyxh > 0 ");
                    }
                    //日志记录
                    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "预约挂号医生信息查询：" + sqlYYYS.ToString(), messageId);
                    #endregion

                    #region 医生信息查询
                    DataTable dt = DBVisitor.ExecuteTable(sqlYYYS.ToString());
                    if (dt.Rows.Count > 0)
                    {
                        #region 获取医生信息拼装
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            YISHENGXX ysxx = new YISHENGXX();
                            ysxx.GUAHAOFS = "2";
                            ysxx.KESHIDM = dt.Rows[i]["KESHIDM"].ToString();//科室ID
                            ysxx.KESHIMC = dt.Rows[i]["KESHIMC"].ToString();//科室名称
                            ysxx.YISHENGDM = dt.Rows[i]["YISHENGDM"].ToString();//医生代码
                            ysxx.GUAHAOLB = dt.Rows[i]["GUAHAOLB"].ToString();//挂号类别
                            ysxx.YUYUELX = dt.Rows[i]["YUYUELX"].ToString();//预约类型
                            if (ysxx.YISHENGDM == "*")
                            {
                                ysxx.YISHENGXM = "普通";//医生姓名
                                ysxx.YISHENGZC = "医生";//医生职称
                            }
                            else
                            {
                                //日志记录
                                //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "挂号医生详细信息查询：" + sqlYYYS.ToString(), messageId);
                                //获取医生详细信息
                                DataTable dtYSXX = DBVisitor.ExecuteTable(
                                    string.Format("select a.zhigongxm yishengxm,b.daimamc yishengzc ,a.zhigongzl from v_gy_zhigongxx a,(select * from gy_daima where daimalb='0072') b where b.daimaid(+)=a.zhicheng and a.zhigongid = '{0}' ",
                                    ysxx.YISHENGDM)
                                    );
                                if (dtYSXX.Rows.Count > 0)
                                {
                                    ysxx.YISHENGXM = dtYSXX.Rows[0]["YISHENGXM"].ToString();//医生姓名
                                    ysxx.YISHENGZC = dtYSXX.Rows[0]["YISHENGZC"].ToString();//医生职称
                                    ysxx.YISHENGJS = dtYSXX.Rows[0]["zhigongzl"].ToString();//医生介绍
                                }
                            }

                            OutObject.YISHENGMX.Add(ysxx);
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            if (OutObject.YISHENGMX.Count == 0)
            {
                if (ConfigurationManager.AppSettings["XHKSXXXS"] == "1")
                    throw new Exception("该科室号源已挂完！");
                throw new Exception("该科室未排班或号源已挂完！");
            }
        }
    }
}
