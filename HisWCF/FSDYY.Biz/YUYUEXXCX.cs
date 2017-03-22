using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;

namespace FSDYY.Biz
{
    public class YUYUEXXCX:IMessage<YUYUEXXCX_IN,YUYUEXXCX_OUT>
    {
        public override void ProcessMessage()
        {
            var sql = "";
            var yyrq = "";
            int jl = 0;
            OutObject = new YUYUEXXCX_OUT();
            OutObject.YUYUEXXXX = new List<YUYUEXX>();

            if (InObject.YUYUESQDBH.ToString() != "")
            {
                sql = " and a.yysqdbh = '" + InObject.YUYUESQDBH.ToString() + "'";
            }
            if (InObject.BINGRENKH.ToString() != "")
            {
                sql = sql + " and a.brkh = '" + InObject.BINGRENKH.ToString() + "'";
            }
            if (InObject.BINGRENMZH.ToString() != "")
            {
                sql = sql + " and a.brmzh = '" + InObject.BINGRENMZH.ToString() + "'";
            }
            if (InObject.BINGRENZYH.ToString() != "")
            {
                sql = sql + " and a.brzyh = '" + InObject.BINGRENZYH.ToString() + "'";
            }
            if (InObject.SHENFENZH.ToString() != "")
            {
                sql = sql + " and a.sfzh = '" + InObject.SHENFENZH.ToString() + "'";
            }
            if (InObject.JIANCHASQDBH.ToString() != "")
            {
                sql = sql + " and a.jcsqdbh = '" + InObject.JIANCHASQDBH.ToString() + "'";
            }
            int dtrq = 0;//如果是当天则为0,非当天为1
            if (InObject.YUYUEKSRQ.ToString() == "")
            {
                dtrq = 1;
                yyrq = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (InObject.YUYUEKSRQ == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    dtrq = 0;
                }
                else
                {
                    dtrq = 1;
                }
                yyrq = InObject.YUYUEKSRQ.ToString();
            }
            if (string.IsNullOrEmpty(InObject.CHAXUNLX))//0是所有，1是已预约，2未预约
            {
                InObject.CHAXUNLX = "0";
            }
            if (InObject.CHAXUNLX == "0" || InObject.CHAXUNLX == "1")
            {
                var listyyxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.FSD00006, sql, yyrq, dtrq));
                if (listyyxx.Count == 0)
                {
                    jl = 2;
                    //OutObject = new YUYUEXXCX_OUT();
                    //OutObject.OUTMSG.ERRNO = "-2";
                    //OutObject.OUTMSG.ERRMSG = "找不到预约信息";
                    //return;
                    //throw new Exception(string.Format("找不到预约信息"));
                }
                else
                {
                    jl = 1;
                    foreach (var item in listyyxx)
                    {
                        var yyxx = new YUYUEXX();
                        var csrq = "";
                        if (string.IsNullOrEmpty(item.Get("brcsrq")))
                            csrq = "1901-01-01";
                        else
                            csrq = item.Get("brcsrq").ToString();
                        yyxx.JIANCHAKSDM = string.Empty;// (item.Get("jcksdm") == "" ? string.Empty : item.Get("jcksdm"));
                        yyxx.JIANCHAKSMC = item.Get("jcksmc");
                        yyxx.BINGRENFPH = item.Get("brfph");
                        yyxx.BINGRENLX = int.Parse(item.Get("brlx"));
                        yyxx.BINGRENLXMC = item.Get("brlxmc");
                        yyxx.BINGRENKH = item.Get("brkh");
                        yyxx.BINGRENMZH = item.Get("brmzh");
                        yyxx.BINGRENZYH = item.Get("brzyh");
                        yyxx.BINGRENBQDM = item.Get("brbqdm");
                        yyxx.BINGRENBQMC = item.Get("brbqmc");
                        yyxx.BINGRENCWH = item.Get("brcwh");
                        yyxx.BINGRENXM = item.Get("brxm");
                        yyxx.BINGRENXB = int.Parse(item.Get("brxb"));
                        yyxx.BINGRENNL = item.Get("brnl").ToString();
                        yyxx.BINGRENCSRQ = Convert.ToDateTime(csrq).ToString("yyyy-MM-dd HH:mm:ss");
                        yyxx.BINGRENLXDZ = item.Get("brlxdz");
                        yyxx.BINGRENLXDH = item.Get("brlxdh");
                        yyxx.SHENQINGYSGH = item.Get("sqysgh");
                        yyxx.SHENQINGYSMC = item.Get("sqysmc");
                        yyxx.SHENQINGYYDM = item.Get("sqyydm");
                        yyxx.SHENQINGYYMC = item.Get("sqyymc");
                        yyxx.JIANCHAXMDM = item.Get("jcxmdm");
                        yyxx.JIANCHAXMMC = item.Get("jcxmmc");
                        yyxx.JIANCHAXMLX = item.Get("jcxmlx");
                        yyxx.JIANCHABWDM = item.Get("jcbwdm");
                        yyxx.JIANCHABWMC = item.Get("jcbwmc");
                        yyxx.JIANCHASBDM = int.Parse(item.Get("jcsbdm"));
                        yyxx.JIANCHASBMC = item.Get("jcsbmc");
                        yyxx.JIANCHASBDD = item.Get("jcsbdd");
                        yyxx.YUYUEH = item.Get("yyh");
                        yyxx.SHENFENZH = item.Get("sfzh");
                        yyxx.YUYUESF = int.Parse(item.Get("yysf"));
                        yyxx.YUYUESQDBH = item.Get("yysqdbh");
                        yyxx.YUYUESQDZT = int.Parse(item.Get("yysqdzt"));
                        yyxx.SHENQINGSJ = item.Get("sqsj");
                        yyxx.JIANCHAH = item.Get("jch");
                        yyxx.YUYUERQ = item.Get("jcrq");
                        yyxx.YUYUESJ = item.Get("jcsj");
                        yyxx.JIANCHASQDBH = item.Get("jcsqdbh");
                        yyxx.YINGXIANGFX = item.Get("yxfx");
                        yyxx.YUYUSJD = item.Get("yysjd");
                        OutObject.YUYUEXXXX.Add(yyxx);
                    }
                }
            }
            if (InObject.CHAXUNLX == "0" || InObject.CHAXUNLX == "2")
            {
                //查询未预约信息
                var listwyyxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.FSD00019, sql));
                if (listwyyxx.Count == 0)
                {
                    if (jl != 1)
                    {
                        jl = 2;
                    }
                    //OutObject = new YUYUEXXCX_OUT();
                    //OutObject.OUTMSG.ERRNO = "-2";
                    //OutObject.OUTMSG.ERRMSG = "找不到预约信息";
                    //return;
                    //throw new Exception(string.Format("找不到预约信息"));
                }
                else
                {
                    jl = 1;
                    //OutObject = new YUYUEXXCX_OUT();
                    //OutObject.YUYUEXXXX = new List<YUYUEXX>();
                    foreach (var item in listwyyxx)
                    {
                        var yyxx = new YUYUEXX();
                        var csrq = "";
                        if (string.IsNullOrEmpty(item.Get("brcsrq")))
                            csrq = "1901-01-01";
                        else
                            csrq = item.Get("brcsrq").ToString();
                        yyxx.JIANCHAKSDM = item.Get("jcksdm");
                        yyxx.JIANCHAKSMC = item.Get("jcksmc");
                        yyxx.BINGRENFPH = item.Get("brfph");
                        yyxx.BINGRENLX = int.Parse(item.Get("brlx"));
                        yyxx.BINGRENLXMC = item.Get("brlxmc");
                        yyxx.BINGRENKH = item.Get("brkh");
                        yyxx.BINGRENMZH = item.Get("brmzh");
                        yyxx.BINGRENZYH = item.Get("brzyh");
                        yyxx.BINGRENBQDM = item.Get("brbqdm");
                        yyxx.BINGRENBQMC = item.Get("brbqmc");
                        yyxx.BINGRENCWH = item.Get("brcwh");
                        yyxx.BINGRENXM = item.Get("brxm");
                        yyxx.BINGRENXB = int.Parse(item.Get("brxb"));
                        yyxx.BINGRENNL = item.Get("brnl").ToString();
                        yyxx.BINGRENCSRQ = Convert.ToDateTime(csrq).ToString("yyyy-MM-dd HH:mm:ss");
                        yyxx.BINGRENLXDZ = item.Get("brlxdz");
                        yyxx.BINGRENLXDH = item.Get("brlxdh");
                        yyxx.SHENQINGYSGH = item.Get("sqysgh");
                        yyxx.SHENQINGYSMC = item.Get("sqysmc");
                        yyxx.SHENQINGYYDM = item.Get("sqyydm");
                        yyxx.SHENQINGYYMC = item.Get("sqyymc");
                        yyxx.JIANCHAXMDM = item.Get("jcxmdm");
                        yyxx.JIANCHAXMMC = item.Get("jcxmmc");
                        yyxx.JIANCHAXMLX = item.Get("jcxmlx");
                        yyxx.JIANCHABWDM = item.Get("jcbwdm");
                        yyxx.JIANCHABWMC = item.Get("jcbwmc");
                        //yyxx.JIANCHASBDM = int.Parse(item.Get("jcsbdm"));
                        yyxx.JIANCHASBMC = "";
                        yyxx.JIANCHASBDD = "";
                        yyxx.YUYUEH = "";
                        yyxx.SHENFENZH = item.Get("sfzh");
                        //yyxx.YUYUESF = int.Parse(item.Get("yysf"));
                        yyxx.YUYUESQDBH = "";
                        //yyxx.YUYUESQDZT = int.Parse(item.Get("yysqdzt"));
                        yyxx.SHENQINGSJ = item.Get("jcsqrq");
                        yyxx.JIANCHAH = "";
                        yyxx.YUYUERQ = "";
                        yyxx.YUYUESJ = "";
                        yyxx.JIANCHASQDBH = item.Get("jcsqdbh");
                        yyxx.YINGXIANGFX = item.Get("yxfx");
                        yyxx.YUYUSJD = "";
                        OutObject.YUYUEXXXX.Add(yyxx);
                    }
                }
            }
            if (jl != 1)
            {
                OutObject.OUTMSG.ERRNO = "-2";
                OutObject.OUTMSG.ERRMSG = "找不到预约信息";
            }
        }
    }
}
