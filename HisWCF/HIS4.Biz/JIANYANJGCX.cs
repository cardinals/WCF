using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;
using System.Data.OracleClient;
using System.Data;
using System.Data.Common;
using System.Configuration;


namespace HIS4.Biz
{
    public class JIANYANJGCX :IMessage<JIANYANJGCX_IN,JIANYANJGCX_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new JIANYANJGCX_OUT();
            string tiaoMaH = InObject.JIANYANTM;//检验条码

            string JianYanJGHQMS = ConfigurationManager.AppSettings["JianYanJGHQMS"];//检验结果获取模式

            #region 基本入参判断
            if (string.IsNullOrEmpty(JianYanJGHQMS)) {
                JianYanJGHQMS = "0";
            }

            if (string.IsNullOrEmpty(tiaoMaH)) {
                throw new Exception("检验条码号获取失败！");
            }
            #endregion
            
            #region 检验是否存在
            if (JianYanJGHQMS == "1") //嘉善县第一人民医院使用
            {
                //条码信息转换 
                string sqlTiaoMaXXZH = "select doctadviseno  from l_patientinfo where listmh = '{0}' or doctadviseno = '{0}'";
                string tiaoMaXX = DBVisitor.ExecuteScalar(string.Format(sqlTiaoMaXXZH, tiaoMaH)).ToString();
                if (string.IsNullOrEmpty(tiaoMaXX))
                {
                    throw new Exception("该检验还未出报告，请耐心等待");
                }
                else {
                    tiaoMaH = tiaoMaXX;
                }
            }
            
            string sqlJCSQD = "select kaidanks,kaidanys,kaidanysxm,kaidanksmc,jianyanxmid,jianyanxmmc,yangbenlx,yangbenlxmc,caijiren,caijirxm,to_char(caijirq,'yyyy-mm-dd hh24:mi:dd') caijisj,jieshouren,jieshourxm,to_char(jieshourq,'yyyy-mm-dd hh24:mi:dd') jieshousj,shenhebz,SHENHEREN,SHENHERXM,to_char(SHENHERQ,'yyyy-mm-dd hh24:mi:ss') as SHENHERQ  from yj_jianyansqd a where a.tiaoma='{0}' ";
            

            DataTable dtJCSQD = DBVisitor.ExecuteTable(string.Format(sqlJCSQD, tiaoMaH));

            if (dtJCSQD.Rows.Count <= 0) {
                throw new Exception("未找到该检验申请单");
            }

            OutObject.KAIDANKSDM = dtJCSQD.Rows[0]["kaidanks"].ToString();//开单科室代码
            OutObject.KAIDANYSDM = dtJCSQD.Rows[0]["kaidanys"].ToString();//开单医生代码
            OutObject.KAIDANYSXM = dtJCSQD.Rows[0]["kaidanysxm"].ToString();//开单医生姓名
            OutObject.KAIDANKSMC = dtJCSQD.Rows[0]["kaidanksmc"].ToString();//开单科室名称
            OutObject.JIANYANXMDM = dtJCSQD.Rows[0]["jianyanxmid"].ToString();//检验项目代码
            OutObject.JIANYANXMMC = dtJCSQD.Rows[0]["jianyanxmmc"].ToString();//检验项目名称
            OutObject.YANGBENLX = dtJCSQD.Rows[0]["yangbenlx"].ToString();//样本类型
            OutObject.YANGBENLXMC = dtJCSQD.Rows[0]["yangbenlxmc"].ToString();//样本类型名称
            OutObject.CAIJISJ = dtJCSQD.Rows[0]["caijisj"].ToString();//采集时间
            OutObject.JIESHOUSJ = dtJCSQD.Rows[0]["jieshousj"].ToString();//接收时间
            string shenHeBz = dtJCSQD.Rows[0]["shenhebz"].ToString();//审核标识
            OutObject.SHENHEBZ = dtJCSQD.Rows[0]["SHENHEBZ"].ToString();//审核标识
            OutObject.SHENHEREN = dtJCSQD.Rows[0]["SHENHEREN"].ToString();//审核人
            OutObject.SHENHERXM = dtJCSQD.Rows[0]["SHENHERXM"].ToString();//审核人姓名
            OutObject.SHENHERQ = dtJCSQD.Rows[0]["SHENHERQ"].ToString();//审核日期
            #endregion

            #region 检验是否已出报告
            string sqlJCBG="";
            
            
            sqlJCBG = "select sampleno,sampletype ,to_char(checktime,'yyyy-mm-dd hh24:mi:ss') baogaosj  from l_patientinfo where doctadviseno = '{0}'";
            

            DataTable dtJCBG = DBVisitor.ExecuteTable(string.Format(sqlJCBG, tiaoMaH));

            if (dtJCBG.Rows.Count <= 0 ) //|| shenHeBz == "0"
            {
                throw new Exception("该检验还未出报告，请耐心等待");
            }

            if (shenHeBz == "0" && ConfigurationManager.AppSettings["SHENHEHCBG"] == "1")
            {
                throw new Exception("该检验还未出报告，请耐心等待");
            }

            string sampleNo = dtJCBG.Rows[0]["sampleno"].ToString();//样本号
            OutObject.BAOGAOSJ = dtJCBG.Rows[0]["baogaosj"].ToString();//报告时间

            //string sampleTypeCode = dtJCBG.Rows[0]["sampletype"].ToString();//样本类型代码
            //string sampleType = "";//样本类型
            //string sqlYBLX = "select YANGBENMC from gy_yangbenlx where shuruma1 = '{0}' ";
            
            //if (string.IsNullOrEmpty(sampleTypeCode))
            //{
            //    DataTable dtYBLX = DBVisitor.ExecuteTable(string.Format(sqlYBLX, sampleTypeCode.ToUpper()));
            //    if (dtYBLX.Rows.Count > 0)
            //    {
            //        sampleType = dtYBLX.Rows[0]["YANGBENMC"].ToString();
            //    }
            //}
            #endregion

            #region 获取检验结果
            string sqlJYJGMX = "select b.chinesename,a.testid,a.testresult,a.resultflag,a.unit,a.reflo,a.refhi,A.HINT from l_testresult a,l_testdescribe b where a.testid = b.testid  and a.sampleno = '{0}' ";

            DataTable dtJYJGMX = DBVisitor.ExecuteTable(string.Format(sqlJYJGMX, sampleNo));
            for (int i = 0; i < dtJYJGMX.Rows.Count; i++) {

                JIANYANJGXX jyjgxx = new JIANYANJGXX();

                jyjgxx.JIEGUOXMDM = dtJYJGMX.Rows[i]["testid"].ToString().Replace("<", "＜").Replace(">", "＞");//结果项目代码
                jyjgxx.JIEGUOXMMC = dtJYJGMX.Rows[i]["chinesename"].ToString().Replace("<", "＜").Replace(">", "＞");//结果项目名称
                jyjgxx.JIEGUOZHI = dtJYJGMX.Rows[i]["testresult"].ToString().Replace("<", "＜").Replace(">", "＞");//结果值
                jyjgxx.MIAOSHUDW = dtJYJGMX.Rows[i]["unit"].ToString().Replace("<", "＜").Replace(">", "＞");//单位
                jyjgxx.CANKAOZSX = dtJYJGMX.Rows[i]["refhi"].ToString().Replace("<", "＜").Replace(">", "＞");//上限
                jyjgxx.CANKAOZXX = dtJYJGMX.Rows[i]["reflo"].ToString().Replace("<", "＜").Replace(">", "＞");//下限

                if (string.IsNullOrEmpty(jyjgxx.CANKAOZSX)) //嘉善人民医院检验结果上下限存放在下限字段中，添加特别处理
                {
                    if (!string.IsNullOrEmpty(jyjgxx.CANKAOZXX)) {
                        if (jyjgxx.CANKAOZXX.Contains("-")) {
                            jyjgxx.CANKAOZSX = jyjgxx.CANKAOZXX.Split('-')[1].ToString();
                            jyjgxx.CANKAOZXX = jyjgxx.CANKAOZXX.Split('-')[0].ToString();
                        }
                    }
                }
                
                string resultFlag = dtJYJGMX.Rows[i]["resultflag"].ToString();//细菌检验结果标识
                if (!string.IsNullOrEmpty(resultFlag))
                {
                    if (resultFlag.ToUpper() == "AAAAAA" || resultFlag.ToUpper() == "ABAAAA" || resultFlag.ToUpper() == "ACAAAA")
                    {
                        jyjgxx.JIEGUO = jyjgxx.JIEGUOZHI;
                    }
                    else if (resultFlag.ToUpper() == "AAA" || resultFlag.ToUpper() == "ABA" || resultFlag.ToUpper() == "ACA" || resultFlag.ToUpper() == "ADA")
                    {
                        jyjgxx.JIEGUO = "正常";
                    }
                    else if (resultFlag.ToUpper() == "BAA" || resultFlag.ToUpper() == "BBA" || resultFlag.ToUpper() == "BCA" || resultFlag.ToUpper() == "BDA")
                    {
                        jyjgxx.JIEGUO = "偏高";
                    }
                    else if (resultFlag.ToUpper() == "CAA" || resultFlag.ToUpper() == "CBA" || resultFlag.ToUpper() == "CCA" || resultFlag.ToUpper() == "CDA")
                    {
                        jyjgxx.JIEGUO = "偏低";
                    }
                    else
                    {
                        #region 细菌培养
                        if (!string.IsNullOrEmpty(resultFlag) && resultFlag == "AA"){
                        }else  if (!string.IsNullOrEmpty(resultFlag) && resultFlag[0] == 'A')
                        {
                            continue;
                        }
                        if (string.IsNullOrEmpty(jyjgxx.JIEGUOZHI))
                        {
                            if (resultFlag[0] == 'N')
                            {
                                jyjgxx.JIEGUOZHI = jyjgxx.JIEGUOXMMC + ";";
                            }

                            if (resultFlag[0] == 'B')
                            {

                                string sqlJYJGXJMX = "select b.chinesename,a.testid,a.testresult,a.resultflag,a.unit,a.reflo,a.refhi,A.HINT from l_testresult a,l_testdescribe b where a.testid = b.testid  and a.sampleno = '{0}' and  resultFlag = 'B" + resultFlag[1].ToString() + "'";
                                DataTable dtJYJGXJMX = DBVisitor.ExecuteTable(string.Format(sqlJYJGXJMX, sampleNo));

                                for (int j = 0; j < dtJYJGXJMX.Rows.Count; j++)
                                {
                                    jyjgxx.JIEGUOZHI = dtJYJGXJMX.Rows[j]["chinesename"].ToString() + ":" + dtJYJGXJMX.Rows[j]["HINT"].ToString();
                                    if (dtJYJGXJMX.Rows[j]["HINT"].ToString() == "阳性")
                                    {
                                        string sqlJYJGYWMX = "select b.chinesename,a.testid,a.testresult,a.resultflag,a.unit,a.reflo,a.refhi,A.HINT from l_testresult a,l_testdescribe b where a.testid = b.testid  and a.sampleno = '{0}' and  resultFlag = 'A" + resultFlag[1].ToString() + "'";
                                        DataTable dtJYJGYWMX = DBVisitor.ExecuteTable(string.Format(sqlJYJGYWMX, sampleNo));

                                        for (int z = 0; z < dtJYJGYWMX.Rows.Count; z++)
                                        {
                                            JIANYANJGXX jyjgxxym = new JIANYANJGXX();
                                            jyjgxxym.JIEGUOXMDM = dtJYJGYWMX.Rows[z]["testid"].ToString();//药敏测试代码
                                            jyjgxxym.JIEGUOXMMC = jyjgxx.JIEGUOXMMC + "[药敏]" + dtJYJGYWMX.Rows[z]["chinesename"].ToString();
                                            string naiYaoXDM = dtJYJGYWMX.Rows[z]["HINT"].ToString();//耐药性代码
                                            switch (naiYaoXDM)
                                            {
                                                case "R":
                                                    jyjgxxym.JIEGUOZHI += "R|耐药";
                                                    break;
                                                case "S":
                                                    jyjgxxym.JIEGUOZHI += "S|敏感";
                                                    break;
                                                case "I":
                                                    jyjgxxym.JIEGUOZHI += "I|中介";
                                                    break;
                                            }
                                            OutObject.JIANYANJGMX.Add(jyjgxxym);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }                    
                }
                OutObject.JIANYANJGMX.Add(jyjgxx);
            }
            #endregion

            if (OutObject.JIANYANJGMX.Count <= 0) {
                throw new Exception("该检验还未出报告，请耐心等待");
            }
        }
    }
}
