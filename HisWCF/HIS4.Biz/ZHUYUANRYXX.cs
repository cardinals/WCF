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
    public class ZHUYUANRYXX : IMessage<ZHUYUANRYXX_IN, ZHUYUANRYXX_OUT>
    {

        public override void ProcessMessage()
        {    //modify   by 沈报  --控制医院的参数
            string KZYY = System.Configuration.ConfigurationManager.AppSettings["YYKZ"];
            OutObject = new ZHUYUANRYXX_OUT();
            string jiuzhenkLx = InObject.JIUZHENKLX;//就诊卡类型
            string jiuzhenKh = InObject.JIUZHENKH;//就诊卡号
            string bingquDm = InObject.BINGQUDM;//病区代码
            string chuangweiH = InObject.CHUANGWEIH;//床位号
            string bingrenLb = InObject.BINGRENLB;//病人类别
            string yibaokLx = InObject.YIBAOKLX;//医保卡类型
            string yibaokMm = InObject.YIBAOKMM;//医保卡密码
            string yibaokXx = InObject.YIBAOKXX;//医保卡信息
            string yiliaoLb = InObject.YILIAOLB;//医疗类别
            string jiesuanLb = InObject.JIESUANLB;//结算类别
            string jiuzhenRq = InObject.JIUZHENRQ;//就诊日期
            string qianfeiKz = InObject.QIANFEIKZ;//欠费控制
            string zhengjianLx = InObject.ZHENGJIANLX;//诊间类型
            string zhengjianHm = InObject.ZHENGJIANHM;//证件号码
            string zaiyuanZt = InObject.ZAIYUANZT;//在院状态
            string zhuyuanHao = InObject.ZHUYUANHAO;//住院号
            string yuanQuId = InObject.BASEINFO.FENYUANDM;//分院代码

            #region 基本入参判断

            if (string.IsNullOrEmpty(jiuzhenKh) && string.IsNullOrEmpty(zhengjianHm) && string.IsNullOrEmpty(zhuyuanHao))
            {
                throw new Exception("就诊卡号,住院号和证件号码不能同时为空！");
            }
            #endregion
            StringBuilder sqlZYBRXX;
            if (KZYY == "000001")
            {
                sqlZYBRXX = new StringBuilder("select a.jiuzhenkh,a.feiyonglb bingrenlb,a.feiyongxz bingrenxz,a.yibaokh, ")
                   .Append("a.gerenbh,a.zhuyuanhao binglibh,a.xingming,a.xingbie,a.minzu,to_char(a.chushengrq,'yyyy-mm-dd') chushengrq, ")
                   .Append("'1' zhengjianlx,a.shenfenzh zhengjianhm,'' danweilx,a.danweiyb danweibh, ")
                   .Append("a.gongzuodw danweimc,a.jiatingdz jiatingzz,''renyuanlb,a.danbaoje dangnianzhye, ")
                   .Append("a.danbaoje linianzhye,a.teshubzbz,a.teshubzbm teshubzspbh,a.yibaobrxx, ")
                   .Append("'' tishixx,a.yibaoyllb teshudylb,''qianfeije,''hisbrxx,a.lianxirdh lianxidh, ")
                   .Append("a.shenhebz qianyuebz,a.dangqianbq,a.dangqianks,a.zhuzhiys zhuzhiyisheng, ")
                   .Append("a.dangqiancw chuangweih,to_char(a.ruyuanrq,'yyyy-mm-dd')ruyuanrq, ")
                   .Append("to_char(a.chuyuanrq,'yyyy-mm-dd') chuyuanrq,a.bingrenid,a.zaiyuanzt, ")
                   .Append("a.dangqianbqmc bingqumc,a.binganhao binganh ,b.xingzhimc bingrenxzmc ")
                   //modify by xiaobao    2017/3/22 加了个费用总额的字段
                   .Append(" ,a.ruyuanzddm, a.ruyuanzdmc ,a.bingrenzyid ,a.zhuyuanhao ,a.yuanquid,c.FEIYONGZE ")
                   .Append("from v_zy_bingrenxx a ,gy_feiyongxz b ,V_BINGRENFYZE c  where a.feiyongxz = b.xingzhiid and a.bingrenzyid=c.bingrenzyid and ((a.jiuzhenkh = '{0}' or a.yibaokh = '{0}') or (a.shenfenzh = '{1}') or a.zhuyuanhao = '{2}') and a.yuanquid = '{3}'");


            }
            else
            {
                sqlZYBRXX = new StringBuilder("select a.jiuzhenkh,a.feiyonglb bingrenlb,a.feiyongxz bingrenxz,a.yibaokh, ")
                  .Append("a.gerenbh,a.zhuyuanhao binglibh,a.xingming,a.xingbie,a.minzu,to_char(a.chushengrq,'yyyy-mm-dd') chushengrq, ")
                  .Append("'1' zhengjianlx,a.shenfenzh zhengjianhm,'' danweilx,a.danweiyb danweibh, ")
                  .Append("a.gongzuodw danweimc,a.jiatingdz jiatingzz,''renyuanlb,a.danbaoje dangnianzhye, ")
                  .Append("a.danbaoje linianzhye,a.teshubzbz,a.teshubzbm teshubzspbh,a.yibaobrxx, ")
                  .Append("'' tishixx,a.yibaoyllb teshudylb,''qianfeije,''hisbrxx,a.lianxirdh lianxidh, ")
                  .Append("a.shenhebz qianyuebz,a.dangqianbq,a.dangqianks,a.zhuzhiys zhuzhiyisheng, ")
                  .Append("a.dangqiancw chuangweih,to_char(a.ruyuanrq,'yyyy-mm-dd')ruyuanrq, ")
                  .Append("to_char(a.chuyuanrq,'yyyy-mm-dd') chuyuanrq,a.bingrenid,a.zaiyuanzt, ")
                  .Append("a.dangqianbqmc bingqumc,a.binganhao binganh ,b.xingzhimc bingrenxzmc ")
                  .Append(" ,a.ruyuanzddm, a.ruyuanzdmc ,a.bingrenzyid ,a.zhuyuanhao ,a.yuanquid ")
                  .Append("from v_zy_bingrenxx a ,gy_feiyongxz b  where a.feiyongxz = b.xingzhiid and ((a.jiuzhenkh = '{0}' or a.yibaokh = '{0}') or (a.shenfenzh = '{1}') or a.zhuyuanhao = '{2}') and a.yuanquid = '{3}'");

            }
            if (!string.IsNullOrEmpty(zaiyuanZt) && zaiyuanZt == "0")
            {
                sqlZYBRXX.Append(" and zaiyuanzt = 0 ");
            }
            else if (!string.IsNullOrEmpty(zaiyuanZt) && zaiyuanZt == "1")
            {
                sqlZYBRXX.Append(" and zaiyuanzt != 0 ");
            }

            DataTable dtZYBRXX = DBVisitor.ExecuteTable(string.Format(sqlZYBRXX.ToString(), jiuzhenKh, zhengjianHm, zhuyuanHao, yuanQuId));

            if (dtZYBRXX.Rows.Count <= 0)
            {
                if (!string.IsNullOrEmpty(zaiyuanZt) && zaiyuanZt == "0")
                    throw new Exception("未找到在院病人信息！");
                throw new Exception("未找到相关住院病人信息！");
            }
            else
            {
                for (int i = 0; i < dtZYBRXX.Rows.Count; i++)
                {
                    #region 拼装住院人员信息
                    ZHUYUANXX zyxx = new ZHUYUANXX();
                    zyxx.JIUZHENKH = dtZYBRXX.Rows[i]["JIUZHENKH"].ToString();//就诊卡号
                    zyxx.BINGRENLB = dtZYBRXX.Rows[i]["BINGRENLB"].ToString();//病人类别
                    zyxx.BINGRENXZ = dtZYBRXX.Rows[i]["BINGRENXZ"].ToString();//病人性质
                    zyxx.YIBAOKH = dtZYBRXX.Rows[i]["YIBAOKH"].ToString();//医保卡号
                    zyxx.GERENBH = dtZYBRXX.Rows[i]["GERENBH"].ToString();//个人编号
                    zyxx.BINGLIBH = dtZYBRXX.Rows[i]["BINGLIBH"].ToString();//病历本号
                    zyxx.XINGMING = dtZYBRXX.Rows[i]["XINGMING"].ToString();//姓名
                    zyxx.XINGBIE = dtZYBRXX.Rows[i]["XINGBIE"].ToString();//性别
                    zyxx.MINZU = dtZYBRXX.Rows[i]["MINZU"].ToString();//民族
                    zyxx.CHUSHENGRQ = dtZYBRXX.Rows[i]["CHUSHENGRQ"].ToString();//出生日期
                    zyxx.ZHENGJIANLX = dtZYBRXX.Rows[i]["ZHENGJIANLX"].ToString();//证件类型
                    zyxx.ZHENGJIANHM = dtZYBRXX.Rows[i]["ZHENGJIANHM"].ToString();//证件号码
                    zyxx.DANWEILX = dtZYBRXX.Rows[i]["DANWEILX"].ToString();//单位类型
                    zyxx.DANWEIBH = dtZYBRXX.Rows[i]["DANWEIBH"].ToString();//单位编号
                    zyxx.DANWEIMC = dtZYBRXX.Rows[i]["DANWEIMC"].ToString().Replace("<", "(").Replace(">", ")").Replace("&", "|");//单位名称
                    zyxx.JIATINGZZ = dtZYBRXX.Rows[i]["JIATINGZZ"].ToString().Replace("<", "(").Replace(">", ")").Replace("&", "|");//家庭地址
                    zyxx.RENYUANLB = dtZYBRXX.Rows[i]["RENYUANLB"].ToString();//人员类别
                    zyxx.DANGNIANZHYE = dtZYBRXX.Rows[i]["DANGNIANZHYE"].ToString();//当年帐户余额
                    zyxx.LINIANZHYE = dtZYBRXX.Rows[i]["LINIANZHYE"].ToString();//历年帐户余额
                    zyxx.TESHUBZBZ = dtZYBRXX.Rows[i]["TESHUBZBZ"].ToString();//特殊病种标志
                    zyxx.TESHUBZSPBH = dtZYBRXX.Rows[i]["TESHUBZSPBH"].ToString();//特殊病种审批编号
                    zyxx.YIBAOBRXX = dtZYBRXX.Rows[i]["YIBAOBRXX"].ToString().Replace("<", "(").Replace(">", ")").Replace("&", "|");//医保病人信息
                    zyxx.TISHIXX = dtZYBRXX.Rows[i]["TISHIXX"].ToString().Replace("<", "(").Replace(">", ")").Replace("&", "|");//提示信息
                    //zyxx.DAIYULB = dtZYBRXX.Rows[i]["DAIYULB"].ToString();//待遇类别
                    //zyxx.CANBAOXZDM = dtZYBRXX.Rows[i]["CANBAOXZDM"].ToString();//参保行政代码
                    zyxx.TESHUDYLB = dtZYBRXX.Rows[i]["TESHUDYLB"].ToString();//特殊待遇类别
                    zyxx.QIANFEIJE = dtZYBRXX.Rows[i]["QIANFEIJE"].ToString();//欠费金额
                    zyxx.HISBRXX = dtZYBRXX.Rows[i]["HISBRXX"].ToString().Replace("<", "(").Replace(">", ")").Replace("&", "|");//HIS病人信息
                    zyxx.LIANXIDH = dtZYBRXX.Rows[i]["LIANXIDH"].ToString();//联系电话
                    zyxx.QIANYUEBZ = dtZYBRXX.Rows[i]["QIANYUEBZ"].ToString();//签约标志
                    zyxx.DANGQIANBQ = dtZYBRXX.Rows[i]["DANGQIANBQ"].ToString();//当前病区
                    zyxx.DANGQIANKS = dtZYBRXX.Rows[i]["DANGQIANKS"].ToString();//当前科室
                    zyxx.ZHUZHIYISHENG = dtZYBRXX.Rows[i]["ZHUZHIYISHENG"].ToString();//主治医生
                    zyxx.CHUANGWEIH = dtZYBRXX.Rows[i]["CHUANGWEIH"].ToString();//床位号
                    zyxx.RUYUANRQ = dtZYBRXX.Rows[i]["RUYUANRQ"].ToString();//入院日期
                    zyxx.CHUYUANRQ = dtZYBRXX.Rows[i]["CHUYUANRQ"].ToString();//出院日期
                    zyxx.BINGRENID = dtZYBRXX.Rows[i]["BINGRENID"].ToString();//病人唯一号
                    zyxx.ZAIYUANZT = dtZYBRXX.Rows[i]["ZAIYUANZT"].ToString();//在院状态
                    zyxx.BINGQUMC = dtZYBRXX.Rows[i]["BINGQUMC"].ToString().Replace("<", "(").Replace(">", ")").Replace("&", "|");//病区名称
                    zyxx.BINGANH = dtZYBRXX.Rows[i]["BINGANH"].ToString();//病案号
                    zyxx.BINGRENXZMC = dtZYBRXX.Rows[i]["BINGRENXZMC"].ToString();//病人性质名称
                    zyxx.BINGRENZYID = dtZYBRXX.Rows[i]["BINGRENZYID"].ToString();//病人住院id
                    zyxx.ZHUYUANHAO = dtZYBRXX.Rows[i]["zhuyuanhao"].ToString();//住院号
                    zyxx.YUANQUID = dtZYBRXX.Rows[i]["yuanquid"].ToString();//院区id
                    if (KZYY == "000001")
                    {
                        zyxx.FEIYONGZE = dtZYBRXX.Rows[i]["FEIYONGZE"].ToString();//住院费用总额；
                    }
                    OutObject.ZHUYUANRYMX.Add(zyxx);
                    #endregion

                    #region 特殊病种信息
                    if (zyxx.TESHUBZBZ == "1")
                    {
                        BINGZHONGXX bzxx = new BINGZHONGXX();
                        bzxx.JIBINGDM = dtZYBRXX.Rows[i]["ruyuanzddm"].ToString();//疾病代码
                        bzxx.JIBINGMC = dtZYBRXX.Rows[i]["ruyuanzdmc"].ToString();//疾病名称

                        string sqlICD = "select ICD10 AS ICD from gy_jibingdm where zhuyuansy =1 and zuofeibz = 0 and jibingid = '{0}' ";
                        bzxx.JIBINGICD = (string)DBVisitor.ExecuteScalar(string.Format(sqlICD, bzxx.JIBINGDM));

                        OutObject.TESHUBZXX.Add(bzxx);
                    }
                    #endregion


                }
            }
        }
    }
}
