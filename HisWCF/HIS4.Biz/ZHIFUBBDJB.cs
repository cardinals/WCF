using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;

namespace HIS4.Biz
{
    public class ZHIFUBBDJB : IMessage<ZHIFUBBDJB_IN, ZHIFUBBDJB_OUT>
    {
        public override void ProcessMessage()
        {
           
            //就诊卡类型
            if (string.IsNullOrEmpty(InObject.JIUZHENKLX)) throw new Exception(string.Format("入参[JIUZHENKLX]为空"));
            //就诊卡号
            if (string.IsNullOrEmpty(InObject.JIUZHENKH)) throw new Exception(string.Format("入参[JIUZHENKH]为空"));

            //姓名
            if (string.IsNullOrEmpty(InObject.XINGMING)) throw new Exception(string.Format("入参[XINGMING]为空"));
            //联系电话
            if (string.IsNullOrEmpty(InObject.LIANXIDH)) throw new Exception(string.Format("入参[LIANXIDH]为空"));
            //证件类型
            // if (string.IsNullOrEmpty(InObject.ZHENGJIANLX)) throw new Exception(string.Format("入参[ZHENGJIANLX]为空"));
            //证件号码
            if (string.IsNullOrEmpty(InObject.ZHENGJIANHM)) throw new Exception(string.Format("入参[ZHENGJIANHM]为空"));
            //支付宝协议号
            if (string.IsNullOrEmpty(InObject.ZHIFUBXYH)) throw new Exception(string.Format("入参[ZHIFUBXYH]为空"));
            //支付宝UserId
            if (string.IsNullOrEmpty(InObject.ZHIFUBYHH)) throw new Exception(string.Format("入参[ZHIFUBYHH]为空"));
            //操作日期
            if (string.IsNullOrEmpty(InObject.CAOZUORQ)) throw new Exception(string.Format("入参[CAOZUORQ]为空"));
            //操作类型
            if (string.IsNullOrEmpty(InObject.CAOZUOLX)) throw new Exception(string.Format("入参[CAOZUOLX]为空"));
            if (string.IsNullOrEmpty(InObject.JIUZHENKLX))
            {
                //  2	社保卡
                throw new Exception(string.Format("入参[JIUZHENKLX]为空"));
            }
            if (InObject.JIUZHENKLX != "2" && InObject.JIUZHENKLX != "3")
            {
                throw new Exception(string.Format("就诊卡类型不正确"));
            }
            if (string.IsNullOrEmpty(InObject.BASEINFO.FENYUANDM))
            {
                throw new Exception(string.Format("医院代码不能为空"));
            }
            var FyInfo = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.HIS00022, InObject.BASEINFO.FENYUANDM));
            if (FyInfo == null)
            {
                throw new Exception(string.Format("查询不到医院代码为【{0}】的医院信息", InObject.BASEINFO.FENYUANDM));
            }
            InObject.JIUZHENKH = System.Convert.ToString(InObject.JIUZHENKH.ToUpper()).PadLeft(10, '0');
            //判断是否存在传入身份证号的病人信息
            string fydmwhere = "";
            if (InObject.JIUZHENKLX != "2")//2 社保卡 3就诊卡
            { //如果是就诊卡 则增加医院条件

                fydmwhere = string.Format(" and  FENYUANDM='{0}'", InObject.BASEINFO.FENYUANDM);
            }
            var existInfo = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.HIS00004, InObject.ZHENGJIANHM.ToLower(), InObject.JIUZHENKH, InObject.JIUZHENKLX, fydmwhere));

            var tran = DBVisitor.Connection.BeginTransaction();
            try
            {


                switch (InObject.CAOZUOLX)
                {
                    case "1":
                        if (existInfo != null)
                        {
                            if (InObject.JIUZHENKLX == "3")
                            {//就诊卡
                                DBVisitor.ExecuteNonQuery(string.Format("Update Gy_BangDingGx Set ShenFenZh='{0}'," +
                                   "BingRenXm='{1}',XieYiHm='{2}',BinRenSb='{3}',ZuoFeiPb = 0," +
                                   "LianXiDh='{4}',BangDingRq=To_Date('{5}','yyyy-mm-dd hh24:mi:ss'),CaoZuoRq=Sysdate,CaoZuoGh='{6}'," +
                                   "BeiZhuXx='{7}',PARTNER='{9}',SELLEREMAIL='{10}',JIUZHENKLX='{12}' where ShenFenZh='{8}' and JIUZHENKH='{11}'" +
                                   " and FENYUANDM='{13}' and  JIUZHENKLX='{12}'",
                                   InObject.ZHENGJIANHM.ToLower(),
                                   InObject.XINGMING,
                                   InObject.ZHIFUBXYH,
                                    InObject.ZHIFUBYHH,
                                    InObject.LIANXIDH,
                                    InObject.CAOZUORQ,
                                    InObject.BASEINFO.CAOZUOYDM,
                                    InObject.BEIZHUXX,
                                    InObject.ZHENGJIANHM.ToLower(),
                                    InObject.PARTNER,
                                    InObject.SELLEREMAIL,
                                    InObject.JIUZHENKH,
                                    InObject.JIUZHENKLX,
                                    InObject.BASEINFO.FENYUANDM
                                     ), tran);
                            }
                            else
                            {
                                //社保卡 不需要保存/判断医院代码
                                DBVisitor.ExecuteNonQuery(string.Format("Update Gy_BangDingGx Set ShenFenZh='{0}'," +
                                      "BingRenXm='{1}',XieYiHm='{2}',BinRenSb='{3}',ZuoFeiPb = 0," +
                                      "LianXiDh='{4}',BangDingRq=To_Date('{5}','yyyy-mm-dd hh24:mi:ss'),CaoZuoRq=Sysdate,CaoZuoGh='{6}'," +
                                      "BeiZhuXx='{7}',PARTNER='{9}',SELLEREMAIL='{10}',JIUZHENKLX='{12}' where ShenFenZh='{8}' and JIUZHENKH='{11}' " +
                                      "and  JIUZHENKLX='{12}'",
                                      InObject.ZHENGJIANHM.ToLower(),
                                      InObject.XINGMING,
                                      InObject.ZHIFUBXYH,
                                       InObject.ZHIFUBYHH,
                                       InObject.LIANXIDH,
                                       InObject.CAOZUORQ,
                                       InObject.BASEINFO.CAOZUOYDM,
                                       InObject.BEIZHUXX,
                                       InObject.ZHENGJIANHM.ToLower(),
                                       InObject.PARTNER,
                                       InObject.SELLEREMAIL,
                                       InObject.JIUZHENKH,
                                       InObject.JIUZHENKLX
                                        ), tran);
                            }
                        }
                        else
                        {

                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00003, InObject.JIUZHENKH, InObject.ZHENGJIANHM.ToLower(), InObject.XINGMING, InObject.ZHIFUBXYH,
                                                                                    InObject.ZHIFUBYHH, InObject.LIANXIDH, InObject.CAOZUORQ, InObject.BASEINFO.CAOZUOYDM,
                                                                                    InObject.BEIZHUXX, InObject.PARTNER, InObject.SELLEREMAIL,
                                                                                    InObject.JIUZHENKLX, InObject.BASEINFO.FENYUANDM), tran);
                        }

                        break;
                    case "2":
                        if (existInfo == null)
                        {
                            throw new Exception(string.Format("查询不到就身份证号为[{0}]的绑定信息!", InObject.ZHENGJIANHM.ToLower()));
                        }
                        if (InObject.XINGMING.ToString() != existInfo.Items["BINGRENXM"].ToString())
                        {
                            throw new Exception(string.Format("传入病人姓名[{0}]与绑定账户姓名[{1}]不符", InObject.XINGMING, existInfo.Items["BINGRENXM"]));
                        }

                        //解绑更新作废判别
                        string dynamicSql = string.Format("Update Gy_BangDingGx Set ZuoFeiPb = 1,JieBangRq = To_Date('{2}','yyyy-mm-dd hh24:mi:ss') Where ShenFenZh = '{0}' And XieYiHm = '{1}' And ZuoFeiPb = 0 and JIUZHENKH='{3}' and FENYUANDM='{4}'", InObject.ZHENGJIANHM.ToLower(), InObject.ZHIFUBXYH, InObject.CAOZUORQ, InObject.JIUZHENKH, InObject.BASEINFO.FENYUANDM);
                        DBVisitor.ExecuteNonQuery(dynamicSql, tran);
                        break;
                    default:
                        tran.Rollback();
                        throw new Exception("传入未定义的操作类型:" + InObject.CAOZUOLX);
                }
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception(ex.ToString());
            }

        }
    }
}
