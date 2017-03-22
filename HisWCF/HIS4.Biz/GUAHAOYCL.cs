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

namespace HIS4.Biz
{
    public class GUAHAOYCL : IMessage<GUAHAOYCL_IN, GUAHAOYCL_OUT>
    {
        public override void ProcessMessage()
        {
            this.OutObject = new GUAHAOYCL_OUT();

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

            if (daishouFy == "0") { }
            else
            {
                string PaiBanxxSql = "select * from mz_v_guahaopb_ex_zzj where paibanid = '{0}' ";
                DataTable dtPaiBanxx = DBVisitor.ExecuteTable(string.Format(PaiBanxxSql, dangtianpbId));
                if (dtPaiBanxx.Rows.Count > 0)
                {
                    OutObject.JIUZHENDD = dtPaiBanxx.Rows[0]["WEIZHI"].ToString();
                }
                else {
                    throw new Exception("未找到指定排班!");
                }


                OutObject.GUAHAOXH = "0";


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
                for (int i = 0; i < OutObject.FEIYONGMX.Count; i++) {
                    fyze += Convert.ToDouble(OutObject.FEIYONGMX[i].JINE);
                }

                OutObject.JIESUANJG.FEIYONGZE = fyze.ToString();
                
            }
        }
    }
}
