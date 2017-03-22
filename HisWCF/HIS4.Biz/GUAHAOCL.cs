using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;
using log4net;
using System.Configuration;

namespace HIS4.Biz
{
    public class GUAHAOCL : IMessage<GUAHAOCL_IN, GUAHAOCL_OUT>
    {
        ILog log = log4net.LogManager.GetLogger("SqlInfo");
        public override void ProcessMessage()
        {
            this.OutObject = new GUAHAOCL_OUT();


            string jiuzhenkLx = InObject.JIUZHENKLX;//就诊卡类型
            string jiuzhenKh = InObject.JIUZHENKH;//就诊卡号
            string bingrenLb = InObject.BINGRENLB;//病人类别 "BINGRENLB");
            string bingrenXz = InObject.BINGRENXZ;// "BINGRENXZ");
            string yibaokLx = InObject.YIBAOKLX;// "YIBAOKLX");
            string yibaokMm = InObject.YIBAOKMM;// "YIBAOKMM");
            string yibaoXx = InObject.YIBAOKXX;// "YIBAOKXX");
            string yibaobrXx = InObject.YIBAOBRXX;// "YIBAOBRXX");
            string yiliaoLb = InObject.YILIAOLB;// "YILIAOLB");
            string jiesuanLb = InObject.JIESUANLB;// "JIESUANLB");
            string yizhoupbId = InObject.YIZHOUPBID;// "YIZHOUPBID");
            string dangtianpbId = InObject.DANGTIANPBID;// "DANGTIANPBID");
            string riQi = InObject.RIQI;// "RIQI");
            string guahaoBc = InObject.GUAHAOBC;// "GUAHAOBC");
            string guahaoLb = InObject.GUAHAOLB;// "GUAHAOLB");
            string keshiDm = InObject.KESHIDM;//     "KESHIDM");
            string yishengDm = InObject.YISHENGDM;// "YISHENGDM");
            string guahaoXh = InObject.GUAHAOXH;// "GUAHAOXH");
            string guahaoId = InObject.GUAHAOID;// "GUAHAOID");
            string daishouFy = InObject.DAISHOUFY;// "DAISHOUFY");
            string yuyueLy = InObject.YUYUELY;// "YUYUELY");
            string binglibH = InObject.BINGLIBH;// "BINGLIBH");
            string hisbrXx = InObject.HISBRXX;// "HISBRXX");
            string jiesuanId = InObject.JIESUANID;// "JIESUANID");
            string caozuoyDm = InObject.BASEINFO.CAOZUOYDM;// "CAOZUOYDM");
            string caozuoyXm = InObject.BASEINFO.CAOZUOYXM;// "CAOZUOYXM");
            string caozuoRq = InObject.BASEINFO.CAOZUORQ;// "CAOZUORQ");
            string fenyuanDm = InObject.BASEINFO.FENYUANDM;// "FENYUANDM");
            string jiaoyiLsh = InObject.BASEINFO.ZHONGDUANLSH;// "ZHONGDUANLSH");//终端流水号即交易流水号？？ 


            #region 基本入参判断
            if (string.IsNullOrEmpty(caozuoRq))
            {
                caozuoRq = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(jiuzhenKh))
            {
                throw new Exception("就诊卡号获取失败！");
            }

            if (string.IsNullOrEmpty(dangtianpbId))
            {
                throw new Exception("挂号排班编号获取失败！");
            }
            if (string.IsNullOrEmpty(guahaoBc))
            {
                throw new Exception("挂号班次获取失败！");
            }
            if (string.IsNullOrEmpty(daishouFy))
            {
                daishouFy = "0";
            }
            #endregion

            if (daishouFy == "0")
            {
                if (InObject.ZHIFUMX.Count <= 0)
                {
                    throw new Exception("支付明细不能为空!");
                }
                if (string.IsNullOrEmpty(InObject.ZHIFUMX[0].ZHIFULX))
                {
                    throw new Exception("支付类型不能为空!");
                }
                if (string.IsNullOrEmpty(InObject.ZHIFUMX[0].ZHIFUJE))
                {
                    throw new Exception("支付金额不能为空!");
                }
                if (string.IsNullOrEmpty(InObject.ZHIFUMX[0].YINHANGKH))
                {
                    throw new Exception("支付帐号不能为空!");
                }
                if (InObject.ZHIFUMX[0].ZHIFULX != "7")
                {
                    throw new Exception("暂时不支持该支付类型!");
                }
                string ZHIFFS = "1";
                switch (InObject.ZHIFUMX[0].ZHIFULX)
                {
                    case "7":
                        ZHIFFS = "19";
                        break;
                    default:
                        ZHIFFS = "1";
                        break;
                }


                string brxxSql = "select bingrenid from gy_bingrenxx where jiuzhenkh='" + jiuzhenKh + "'";
                string bingrenId = DBVisitor.ExecuteScalar(brxxSql).ToString();
                string jiaoyiMsg = bingrenId + "|" + dangtianpbId + "|" + caozuoyDm + "|" + caozuoyXm + "|" + (guahaoBc == "1" ? "0" : "1") + "|"
                   + caozuoRq + "|" + jiaoyiLsh + "|" + jiuzhenKh + "|" + "1" + "|" + string.Empty + "|" + string.Empty + "|" + ZHIFFS + "|";
                //挂号支付-------------------------------------------------------------------------------------------
                OracleParameter[] paramJiaoYi = new OracleParameter[3];
                paramJiaoYi[0] = new OracleParameter("PRM_MSG", OracleType.VarChar);
                paramJiaoYi[0].Value = jiaoyiMsg;
                paramJiaoYi[0].Direction = ParameterDirection.Input;
                paramJiaoYi[1] = new OracleParameter("PRM_APPCODE", OracleType.Number);
                paramJiaoYi[1].Value = 0;
                paramJiaoYi[1].Direction = ParameterDirection.Output;
                paramJiaoYi[2] = new OracleParameter("PRM_OUTBUFFER", OracleType.VarChar);
                paramJiaoYi[2].Value = null;
                paramJiaoYi[2].Size = 2000;
                paramJiaoYi[2].Direction = ParameterDirection.Output;

                log.InfoFormat("{0}", "执行存储过程：PKG_GY_YINYIJK.PRC_GUAHAOZF \r\nPRM_MSG：" + jiaoyiMsg + "\r\n");

                string returnValue = string.Empty;
                DbTransaction transaction = null;
                DbConnection conn = DBVisitor.Connection;
                try
                {
                    transaction = conn.BeginTransaction();
                    DBVisitor.ExecuteProcedure("PKG_GY_YINYIJK.PRC_GUAHAOZF", paramJiaoYi, transaction);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                        conn.Close();
                    }
                    throw new Exception(ex.Message);
                }
                returnValue = paramJiaoYi[1].Value.ToString();
                string returnMsg = paramJiaoYi[2].Value.ToString();
                // LogHelper.WriteLog(typeof(GG_JiaoYiBLL), "挂号支付存储过程返回值：" + returnValue + "|" + returnMsg);
                if (returnValue == "1")//交易成功
                {
                    try
                    {
                        

                        #region 诊疗费用信息
                        string ZhenLiaoXMSql = "select * from gy_shoufeixm where shoufeixmid in "
                        + "( select zhenliaofxm from mz_v_guahaopb_ex_zzj where paibanid = '{0}' )";
                        DataTable dtZhenLiaoMX = DBVisitor.ExecuteTable(string.Format(ZhenLiaoXMSql, dangtianpbId));

                        for (int i = 0; i < dtZhenLiaoMX.Rows.Count; i++)
                        {
                            FEIYONGXX fyxx = new FEIYONGXX();
                            fyxx.XIANGMUXH = dtZhenLiaoMX.Rows[i]["shoufeixmid"].ToString();//收费项目ID
                            fyxx.XIANGMUMC = dtZhenLiaoMX.Rows[i]["shoufeixmmc"].ToString();
                            fyxx.XIANGMUGL = dtZhenLiaoMX.Rows[i]["xiangmulx"].ToString();
                            fyxx.DANJIA = dtZhenLiaoMX.Rows[i]["danjia1"].ToString();//
                            fyxx.SHULIANG = "1";
                            fyxx.JINE = dtZhenLiaoMX.Rows[i]["danjia1"].ToString();
                            OutObject.FEIYONGMX.Add(fyxx);
                            OutObject.ZHENLIAOFEI = fyxx.DANJIA;
                        }
                        #endregion

                        #region 挂号费用信息
                        string GuaHaoXMSql = "select * from gy_shoufeixm where shoufeixmid in "
                               + "( select guahaofxm from mz_v_guahaopb_ex_zzj where paibanid = '{0}' )";
                        DataTable dtGuaHaoMX = DBVisitor.ExecuteTable(string.Format(GuaHaoXMSql, dangtianpbId));

                        for (int i = 0; i < dtGuaHaoMX.Rows.Count; i++)
                        {
                            FEIYONGXX fyxx = new FEIYONGXX();
                            fyxx.XIANGMUXH = dtGuaHaoMX.Rows[i]["shoufeixmid"].ToString();//收费项目ID
                            fyxx.XIANGMUMC = dtGuaHaoMX.Rows[i]["shoufeixmmc"].ToString();
                            fyxx.XIANGMUGL = dtGuaHaoMX.Rows[i]["xiangmulx"].ToString();
                            fyxx.DANJIA = dtGuaHaoMX.Rows[i]["danjia1"].ToString();//
                            fyxx.SHULIANG = "1";
                            fyxx.JINE = dtGuaHaoMX.Rows[i]["danjia1"].ToString();
                            OutObject.FEIYONGMX.Add(fyxx);
                            OutObject.GUAHAOFEI = fyxx.DANJIA;
                        }
                        #endregion

                        double fyze = 0.0;
                        for (int i = 0; i < OutObject.FEIYONGMX.Count; i++)
                        {
                            fyze += Convert.ToDouble(OutObject.FEIYONGMX[i].JINE);
                        }
                        if (fyze != double.Parse(InObject.ZHIFUMX[0].ZHIFUJE))
                        {
                            throw new Exception("支付金额:" + InObject.ZHIFUMX[0].ZHIFUJE + "与实际金额:" + fyze + "不符!");
                        }

                        string[] list = returnMsg.Split('|');
                        OutObject.GUAHAOID = list[8];//挂号ID
                        OutObject.GUAHAOXH = list[0];
                        OutObject.JIUZHENSJ = list[2];
                        OutObject.JIUZHENDD = list[1];
                        OutObject.YIJIID = list[5];
                        OutObject.JIESUANJG.FEIYONGZE = (Convert.ToDouble(list[6]) + Convert.ToDouble(list[7])).ToString();
                    //    string[] yiJiSpl = list[5].ToString().Split('^');

                        if (ConfigurationManager.AppSettings["XianShiHZSJ"] == "1")//显示候诊时间
                        {
                            OutObject.HOUZHENSJ = getJiuZhenSJD(yishengDm, keshiDm, list[0], "", Convert.ToInt32((guahaoBc == "1" ? "0" : "1")), guahaoLb, DateTime.Now.ToString());//候诊时间
                        }
                       
                        OutObject.JIESUANJG.FEIYONGZE = fyze.ToString();
                        transaction.Commit();//提交
                        conn.Close();
                    }
                    catch (Exception er)
                    {
                        transaction.Rollback();//回滚
                        conn.Close();
                        throw new Exception(er.Message.ToString());
                    }


                }
                else
                {
                    transaction.Rollback();//回滚
                    conn.Close();
                    throw new Exception(returnMsg);
                }

            }
            //代收模式
            else
            {
                string brxxSql = "select bingrenid from gy_bingrenxx where jiuzhenkh='" + jiuzhenKh + "'";
                string bingrenId = DBVisitor.ExecuteScalar(brxxSql).ToString();
                string jiaoyiMsg = bingrenId + "|" + dangtianpbId + "|" + caozuoyDm + "|" + caozuoyXm + "|" + (guahaoBc == "1" ? "0" : "1") + "|"
                   + caozuoRq + "|" + jiaoyiLsh + "|" + jiuzhenKh + "|" + "1" + "|" + string.Empty + "|" + string.Empty + "|";//交易类型默认为1
                //挂号支付-------------------------------------------------------------------------------------------
                OracleParameter[] paramJiaoYi = new OracleParameter[3];
                paramJiaoYi[0] = new OracleParameter("PRM_MSG", OracleType.VarChar);
                paramJiaoYi[0].Value = jiaoyiMsg;
                paramJiaoYi[0].Direction = ParameterDirection.Input;
                paramJiaoYi[1] = new OracleParameter("PRM_APPCODE", OracleType.Number);
                paramJiaoYi[1].Value = 0;
                paramJiaoYi[1].Direction = ParameterDirection.Output;
                paramJiaoYi[2] = new OracleParameter("PRM_OUTBUFFER", OracleType.VarChar);
                paramJiaoYi[2].Value = null;
                paramJiaoYi[2].Size = 2000;
                paramJiaoYi[2].Direction = ParameterDirection.Output;

                string returnValue = string.Empty;
                DbTransaction transaction = null;
                DbConnection conn = DBVisitor.Connection;
                try
                {
                    transaction = conn.BeginTransaction();
                    DBVisitor.ExecuteProcedure("PKG_GY_YINYIJK.PRC_GUAHAOZFDS", paramJiaoYi, transaction);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                        conn.Close();
                    }
                    throw new Exception(ex.Message);
                }


                //--------------------------------------------------------------------------------------------------
                returnValue = paramJiaoYi[1].Value.ToString();
                string returnMsg = paramJiaoYi[2].Value.ToString();
                //LogHelper.WriteLog(typeof(GG_JiaoYiBLL), "挂号支付存储过程返回值：" + returnValue + "|" + returnMsg);
                if (returnValue == "1")//交易成功
                {
                    transaction.Commit();//提交
                    conn.Close();
                    string[] list = returnMsg.Split('|');
                    OutObject.GUAHAOID = list[8];//挂号ID
                    OutObject.GUAHAOXH = list[0];
                    OutObject.JIUZHENSJ = list[2];
                    OutObject.JIUZHENDD = list[1];
                    OutObject.YIJIID = list[5];
                    OutObject.JIESUANJG.FEIYONGZE = (Convert.ToDouble(list[6]) + Convert.ToDouble(list[7])).ToString();
                    string[] yiJiSpl = list[5].ToString().Split('^');

                    #region 诊疗费用信息
                    string ZhenLiaoXMSql = "select * from gy_shoufeixm where shoufeixmid in "
                    + "( select zhenliaofxm from mz_v_guahaopb_ex_zzj where paibanid = '{0}' )";
                    DataTable dtZhenLiaoMX = DBVisitor.ExecuteTable(string.Format(ZhenLiaoXMSql, dangtianpbId));

                    for (int i = 0; i < dtZhenLiaoMX.Rows.Count; i++)
                    {
                        FEIYONGXX fyxx = new FEIYONGXX();
                        fyxx.XIANGMUXH = dtZhenLiaoMX.Rows[i]["shoufeixmid"].ToString();//收费项目ID
                        fyxx.XIANGMUMC = dtZhenLiaoMX.Rows[i]["shoufeixmmc"].ToString();
                        fyxx.XIANGMUGL = dtZhenLiaoMX.Rows[i]["xiangmulx"].ToString();
                        fyxx.DANJIA = dtZhenLiaoMX.Rows[i]["danjia1"].ToString();//
                        fyxx.SHULIANG = "1";
                        fyxx.JINE = dtZhenLiaoMX.Rows[i]["danjia1"].ToString();
                        OutObject.FEIYONGMX.Add(fyxx);
                        OutObject.ZHENLIAOFEI = fyxx.DANJIA;
                    }
                    #endregion

                    #region 挂号费用信息
                    string GuaHaoXMSql = "select * from gy_shoufeixm where shoufeixmid in "
                           + "( select guahaofxm from mz_v_guahaopb_ex_zzj where paibanid = '{0}' )";
                    DataTable dtGuaHaoMX = DBVisitor.ExecuteTable(string.Format(GuaHaoXMSql, dangtianpbId));

                    for (int i = 0; i < dtGuaHaoMX.Rows.Count; i++)
                    {
                        FEIYONGXX fyxx = new FEIYONGXX();
                        fyxx.XIANGMUXH = dtGuaHaoMX.Rows[i]["shoufeixmid"].ToString();//收费项目ID
                        fyxx.XIANGMUMC = dtGuaHaoMX.Rows[i]["shoufeixmmc"].ToString();
                        fyxx.XIANGMUGL = dtGuaHaoMX.Rows[i]["xiangmulx"].ToString();
                        fyxx.DANJIA = dtGuaHaoMX.Rows[i]["danjia1"].ToString();//
                        fyxx.SHULIANG = "1";
                        fyxx.JINE = dtGuaHaoMX.Rows[i]["danjia1"].ToString();
                        OutObject.FEIYONGMX.Add(fyxx);
                        OutObject.GUAHAOFEI = fyxx.DANJIA;
                    }
                    #endregion

                    double fyze = 0.0;
                    for (int i = 0; i < OutObject.FEIYONGMX.Count; i++)
                    {
                        fyze += Convert.ToDouble(OutObject.FEIYONGMX[i].JINE);
                    }

                    OutObject.JIESUANJG.FEIYONGZE = fyze.ToString();
                }
                else
                {
                    transaction.Rollback();//回滚
                    conn.Close();
                    throw new Exception(returnMsg);
                }
            }
        }

        private string getJiuZhenSJD(string yishengdm, string keshidm, string guahaoxh, string yuyuelx, int shangxiawpb, string guahaolb, string yuyuerq)
        {
            string MRJZSJD = ConfigurationManager.AppSettings["MRHaoYanJiuZhensjd"];
            if (string.IsNullOrEmpty(MRJZSJD))
            {
                MRJZSJD = "";
            }
            StringBuilder sbsql = new StringBuilder();
            sbsql.Append("select kaishisj||'-'||jieshusj as sjd from v_mz_houzhensj where keshiid = '" + keshidm + "' and " + guahaoxh + " >= qishighxh and " + guahaoxh + "<=jieshughxh " +
                "and shangxiawbz = " + shangxiawpb + " and nvl(yishengid,'*') = '" + yishengdm + "' ");

            DataTable dt = DBVisitor.ExecuteTable(sbsql.ToString());
            if (dt.Rows.Count > 0)//返回数据空
            {
                if (string.IsNullOrEmpty(dt.Rows[0]["SJD"].ToString()))
                    return MRJZSJD;
                return dt.Rows[0]["SJD"].ToString();
            }
            return MRJZSJD;
        }
    }
}
