using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;
using Common.WSEntity;
using System.Configuration;
using Common.WSCall;
using System.Data.OracleClient;
using log4net;

namespace HIS4.Biz
{
    public class SHEBEIYY : IMessage<SHEBEIYY_IN, SHEBEIYY_OUT>
    {
        ILog log = log4net.LogManager.GetLogger("SqlInfo");
        public override void ProcessMessage()
        {

            string brxxcx = "select * from gy_bingrenxx where bingrenid = '{0}'";
            DataTable dtBingRenXX = DBVisitor.ExecuteTable(string.Format(brxxcx, InObject.BINGRENID));
            if (dtBingRenXX.Rows.Count <= 0)
            {
                throw new Exception("未找到该病人信息，无法完成预约！");
            }

            //判断入参合法性
            if (InObject.YUYUERQ == null)
            {
                throw new Exception("预约日期不能为空！");
            }
            if (InObject.YUYUESJ == null)
            {
                throw new Exception("预约时间不能为空！");
            }
            if (InObject.BINGRENLX == null || Convert.ToInt32(InObject.BINGRENLX) < 1 || Convert.ToInt32(InObject.BINGRENLX) > 3)
            {
                throw new Exception("病人类型代码不对！");
            }
            if (InObject.SHENFENZH == null || InObject.SHENFENZH == string.Empty)
            {
                throw new Exception("病人身份证号不能为空！");
            }
            if (InObject.YEWULY == null)
            {
                InObject.YEWULY = InObject.BINGRENLX.ToString();
            }
            var jcsbdm = InObject.JIANCHASBDM.ToString();//检查设备代码
            var yyrq = InObject.YUYUERQ.ToString();//预约日期
            var yysj = InObject.YUYUESJ.ToString();//预约时间
            var yyh = "";//预约号



            //不能预约以前的日期
            if (string.Compare(yyrq, DateTime.Now.ToString("yyyy-MM-dd")) < 0)
            {
                throw new Exception("预约日期必须大于等于今天！");
            }
            var SONGJIANKS = System.Configuration.ConfigurationManager.AppSettings["SXZZ_SONGJIANKS"];//送检科室
            var SONGJIANYS = System.Configuration.ConfigurationManager.AppSettings["SXZZ_SONGJIANYS"];//送检医生
            HISYY_Submit resource = new HISYY_Submit();

            //获取预约申请单编号
            var yysqdBh = DBVisitor.ExecuteTable("select seq_fsdyy_sq_yysqdbh.nextval yysqdbh from dual");
            //返回字段定义
            OutObject = new SHEBEIYY_OUT();
            //业务类型(1检查，2检验暂时不做)       
            if (InObject.YEWULX == "1")
            {
                //调用包，判断入参是否正确
                var jyjcdxx = "";
                var jyjcmx = "";
                var zdmx = "";
                var jcxmdm = "";
                int i = 0;
                if (InObject.JIANCHALB.Count < 1)
                {
                    throw new Exception("检查项目不能为空！");
                }
                //检查单信息
                jyjcdxx += InObject.JIUZHENKLX + "|";//	就诊卡类型
                jyjcdxx += InObject.JIUZHENKH + "|";//	就诊卡号
                jyjcdxx += InObject.SHENQINGYSGH + "|";//	送检医生
                jyjcdxx += InObject.SHENQINGYYDM + "|";//	送检科室
                jyjcdxx += InObject.YUYUESF + "|";//	收费识别
                jyjcdxx += InObject.BINGQINGMS + "|";//	病情描述
                jyjcdxx += InObject.ZHENDUAN + "|";//	诊断
                jyjcdxx += InObject.BINGRENTZ + "|";//	病人体征
                jyjcdxx += InObject.QITAJC + "|";//	其它检查
                jyjcdxx += InObject.BINGRENZS + "|";//	病人主诉
                jyjcdxx += InObject.YEWULY + "|";//	检查来源
                jyjcdxx += InObject.BINGRENXM + "|";//	病人姓名
                jyjcdxx += InObject.SHENFENZH + "|";//	病人身份证号
                jyjcdxx += "0" + "|";//	接收方式
                jyjcdxx += "|";//	检查申请单号
                jyjcdxx += InObject.YUYUERQ + " " + InObject.YUYUESJ + "|";//	检查日期
                jyjcdxx += InObject.BINGRENXB + "|";//	病人性别
                jyjcdxx += InObject.BINGRENNL;//	病人年龄

                //检验检查明细
                foreach (var item in InObject.JIANCHALB)
                {
                    if (i == 0)
                    {
                        jcxmdm = item.JIANCHAXMBH;
                    }
                    else
                    {
                        jcxmdm += "," + item.JIANCHAXMBH;
                    }
                    ++i;
                    jyjcmx += item.JIANCHAXMBH + "|";//	检查项目编号
                    jyjcmx += item.JIANCHAXMMC + "|";//	检查项目名称
                    jyjcmx += item.JIANCHAFLBM + "|";//	检查分类编码
                    jyjcmx += item.JIANCHASTBW + "|";//	检查身体部位
                    jyjcmx += item.JIANCHAFXDM + "|";//	检查方向代码
                    jyjcmx += item.JIANCHAZYDM + "|";//	检查肢位代码
                    jyjcmx += item.JIANCHATS + "^";//	检查提示
                }
                //疾病明细
                foreach (var item in InObject.ZHENDUANLB)
                {
                    zdmx += item.ICD10 + "|";//	ICD10
                    zdmx += item.ZHENDUANMC + "^";//诊断名称
                }

                #region 调用莱达接口
                if (InObject.YEWULX == "2")
                    resource.AdmissionSource = "10";
                else
                    resource.AdmissionSource = "50";
                resource.HospitalCode = InObject.SHENQINGYYDM;// InObject.BASEINFO.JIGOUDM;
                resource.HospitalName = InObject.SHENQINGYYMC;
                resource.PatientName = InObject.BINGRENXM;
                resource.IdNumber = InObject.SHENFENZH;
                resource.RequestNo = "";//预约的时候传空
                if (InObject.YEWULY == "2")
                    resource.AdmissionID = InObject.BINGRENZYH;
                else
                    resource.AdmissionID = InObject.BINGRENMZH;
                resource.ExaminePartTime = InObject.XIANGMUHS;
                resource.PatientSex = InObject.BINGRENXB.ToString();
                resource.PatientBorn = InObject.BINGRENCSRQ;
                resource.PatientAge = InObject.BINGRENNL;
                resource.PatientTel = InObject.BINGRENLXDH;
                resource.PatientAddress = InObject.BINGRENLXDZ;
                resource.PatientCard = InObject.BINGRENID + "|" + InObject.JIUZHENKH;
                resource.InPatientAreaName = InObject.BINGRENBQMC;
                resource.InPatientAreaCode = InObject.BINGRENBQDM;
                resource.BedNum = InObject.BINGRENCWH;
                resource.DeviceCode = InObject.JIANCHASBDM.ToString();
                resource.DeviceName = InObject.JIANCHASBMC;
                //,B.DAIMAID,B.DAIMAMC,D.KESHIMC
                string jcxmSql = @"SELECT A.*,B.DAIMAID,B.DAIMAMC,D.KESHIMC FROM GY_JIANCHAXM A, GY_JIANCHABW B, GY_JIANCHAXMBWDY C,  
                            GY_KESHI D,GY_YUANQU E  WHERE A.JIANCHAXMID = C.JIANCHAXMID AND B.DAIMAID = C.JIANCHABWID  
                            AND D.KESHIID=A.ZHIXINGKS AND A.ZUOFEIBZ = 0 AND B.ZUOFEIBZ = 0 AND D.YUANQUID = E.YUANQUID
                            AND A.JIANCHAXMID IN({0}) AND B.DAIMAID = {1} AND E.YUANQUMC = '{2}'";
                jcxmSql = string.Format(jcxmSql, jcxmdm, InObject.JIANCHABWDM.ToString(), ConfigurationManager.AppSettings["HospitalName_Fck"].ToString());

                DataTable jcxmDt = DBVisitor.ExecuteTable(jcxmSql);
                foreach (DataRow dr in jcxmDt.Rows)
                {
                    resource.StudiesExamine.Add(new StudiesExamine()
                    {
                        //HT  更改CODE取值 检查厂商使用的是HIS的部位ID和部位的名称，而没有检查项目ID
                        //ExamineCode = dr["JIANCHAXMID"].ToString(),
                        ExamineCode = dr["DAIMAID"].ToString(),
                        ExamineName = XMLHandle.encodeString(dr["DAIMAMC"].ToString()),
                        Numbers = "1",
                        ExaminePrice = "0"//这里默认给个0

                    });
                }
                resource.StudiesExamine = resource.StudiesExamine.Distinct().ToList();
                resource.ExamineFY = resource.StudiesExamine.Sum<StudiesExamine>(group => { return Convert.ToDecimal(group.ExaminePrice); }).ToString();
                resource.ReceiptNum = InObject.BINGRENFPH;
                resource.ZxDepartmentId = jcxmDt.Rows[0]["zhixingks"].ToString();
                resource.ZxDepartmentName = jcxmDt.Rows[0]["keshimc"].ToString();
                resource.Sqrq = InObject.BASEINFO.CAOZUORQ;
                resource.BespeakDateTime = InObject.YUYUERQ + " " + InObject.YUYUESJ;
                resource.JZ = "0";// InObject.JIZHEN;
                resource.ZQ = "0";//InObject.ZENGQIANG;
                resource.LS = "0";//InObject.LINSHI;
                resource.PF = "1";

                //调用莱达WEBSERVICE---------------------------------------------------------------
                string url = System.Configuration.ConfigurationManager.AppSettings["LaiDa_Url"];
                string xml = XMLHandle.EntitytoXML<HISYY_Submit>(resource);

                //LogHelper.WriteLog(typeof(GG_ShuangXiangZzBLL), "设备预约调用莱达XML入参：" + xml);
                string outxml = WSServer.Call<HISYY_Submit>(url, xml).ToString();

                //LogHelper.WriteLog(typeof(GG_ShuangXiangZzBLL), "设备预约调用莱达XML出参：" + outxml);
                HISYY_Submit_Result result = XMLHandle.XMLtoEntity<HISYY_Submit_Result>(outxml);
                //-------------------------------------------------------------------------------
                if (result.Success == "False")
                {
                    throw new Exception("预约失败,错误原因：" + result.Message);
                }
                #endregion

                //定义数据库transaction
                var tran = DBVisitor.Connection.BeginTransaction();
                var xh = DBVisitor.ExecuteScalar("select seq_sxzz_jianchasqd.nextval from dual").ToString();
                //保存预约数据                               
                try
                {
                    //先插入到临时表                   

                    var JCSQDH = result.JCH;
                    //var jiuzhenid = DBVisitor.ExecuteScalar("select SEQ_ZJ_JIUZHENXX.NEXTVAL from dual").ToString();
                    int k = 0;
                    //插入检查单信息
                    string insJcd = @"insert into SXZZ_JIANCHASQD(JIANCHASQDID,JIUZHENKLX,JIUZHENKH,BINGRENXM,
                                      BINGRENSFZH,SONGJIANYSGH,SONGJIANKSDM,SHOUFEIBZ,
                                      BINGQINGMS,ZHENDUANMC,BINGRENTZ,QITAJC,
                                      BINGRENZS,JIANCHALY,JIESHOUFS,JIESHOURQ,
                                      JIESHOUBZ,YIJISQDH,JIANCHARQ,BINGRENXB,BINGRENNL,
                                        JIANCHASQDZT,YIJIZXKS,YIJIZXKSMC,BINGRENLX,
                                         BINGRENLXMC,ZHUYUANHAO,SONGJIANBQDM,SONGJIANBQMC,
                                         CHAUNGWEIID,CHUSHENGRQ,BINGRENLXDZ,BINGRENLXDH,
                                        SONGJIANYSXM,SONGJIANKSMC,SONGJIANYYDM,SONGJIANYYMC,
                                        YIJISBDM,YIJISBMC,YIJISBDZ,YIJIYYH,
                                        YIJIYYSJD,YIJIXXAPSJ,YIJIYYRQ)
                                      values({0},'{1}','{2}','{3}',
                                            '{4}','{5}','{6}','{7}',
                                            '{8}','{9}','{10}','{11}',
                                            '{12}','{13}','{14}',sysdate,
                                            0,'{15}',to_date('{16}','yyyy-mm-dd'),'{17}','{18}',
                                            '{19}','{20}','{21}','{22}',
                                            '{23}','{24}','{25}','{26}',
                                            '{27}',to_date('{28}','yyyy-mm-dd'),'{29}','{30}',
                                            '{31}','{32}','{33}','{34}',
                                            '{35}','{36}','{37}','{38}',
                                            '{39}','{40}',to_date('{41}','yyyy-mm-dd hh24:mi'))";

                    DBVisitor.ExecuteNonQuery(string.Format(insJcd, xh, "", InObject.JIUZHENKH, InObject.BINGRENXM,
                                InObject.SHENFENZH, SONGJIANYS, SONGJIANKS, 0,
                                InObject.BINGQINGMS, InObject.ZHENDUAN, InObject.BINGRENTZ, InObject.QITAJC,
                                InObject.BINGRENZS, InObject.YEWULX, 0, JCSQDH, InObject.YUYUERQ,
                                InObject.BINGRENXB, InObject.BINGRENNL,
                                InObject.YUYUEZT,
                                InObject.JIANCHAKSDM,
                                InObject.JIANCHAKSMC,
                                InObject.BINGRENLX,
                                InObject.BINGRENLXMC,
                                InObject.BINGRENZYH,
                                InObject.BINGRENBQDM,
                                InObject.BINGRENBQMC,
                                InObject.BINGRENCWH,
                                InObject.BINGRENCSRQ,
                                InObject.BINGRENLXDZ,
                                InObject.BINGRENLXDH,
                                InObject.SHENQINGYSMC,//申请医生姓名
                                "",//申请科室名称
                                InObject.SHENQINGYYDM,//申请医院代码
                                InObject.SHENQINGYYMC,//申请医院名称
                        //InObject.YUYUESJ,//检查时间
                        //InObject.JIANCHAXMLX,//检查项目类型
                                InObject.JIANCHASBDM,//检查设备代码
                                InObject.JIANCHASBMC,//检查设备名称
                                InObject.JIANCHASBDD,//检查设备地点
                                result.YYH,//预约号
                                result.PDH,//时间段  预约申请时间段
                                InObject.XIANGMUHS,//详细安排时间
                                InObject.YUYUERQ + " " + InObject.YUYUESJ//预约日期
                                ), tran);

                    //插入检查明细
                    foreach (var item in InObject.JIANCHALB)
                    {
                        var mxxh = DBVisitor.ExecuteScalar("select seq_sxzz_jianchasqdmx.nextval from dual").ToString();
                        string insJcmx = @"insert into SXZZ_JIANCHASQDMX(JIANCHASQDID,JIANCHASQDMXID,JIANCHAXMBH,
                                           JIANCHAXMMC,JIANCHAFLBM,JIANCHASTBW,
                                           JIANCHAFXDM,JIANCHAZYDM,JIANCHATS)
                                           values({0},{1},'{2}',
                                           '{3}','{4}','{5}',
                                           '{6}','{7}','{8}')";

                        DBVisitor.ExecuteNonQuery(string.Format(insJcmx, xh, mxxh, item.JIANCHAXMBH,
                            item.JIANCHAXMMC, item.JIANCHAFLBM, item.JIANCHASTBW,
                            item.JIANCHAFXDM, item.JIANCHAZYDM, item.JIANCHATS),tran);
                    }
                    k = 0;
                    //插入诊断明细
                    foreach (var item in InObject.ZHENDUANLB)
                    {
                        var zdxh = DBVisitor.ExecuteScalar("select seq_sxzz_jianchasqdzd.nextval from dual").ToString();

                        string insZdmx = @"insert into SXZZ_JIANCHASQDZD(JIANCHASQDID,JIANCHADZDID,icd10,zhenduanmc)
                                           values({0},{1},'{2}','{3}')";

                        DBVisitor.ExecuteNonQuery(string.Format(insZdmx, xh, zdxh, item.ICD10, item.ZHENDUANMC), tran);
                    }

                    tran.Commit();

                    OutObject.YUYUERQ = InObject.YUYUERQ;
                    OutObject.YUYUESJ = InObject.YUYUESJ;
                    OutObject.YUYUEH = result.YYH;
                    OutObject.JIANCHAH = result.JCH;
                    OutObject.YUYUESQDBH = xh;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    var resource1 = new HISYY_Cancel();
                    resource1.RequestNo = "";// listyyxx.Items["YYH"].ToString();
                    resource1.YYH = result.YYH;
                    resource1.JCH = result.PDH;
                    string url1 = System.Configuration.ConfigurationManager.AppSettings["LaiDa_Url"];
                    string xml1 = XMLHandle.EntitytoXML<HISYY_Cancel>(resource1);
                    string outxml1 = WSServer.Call<HISYY_Cancel>(url, xml).ToString();
                    HISYY_Cancel_Result result1 = XMLHandle.XMLtoEntity<HISYY_Cancel_Result>(outxml);
                    throw new Exception(ex.Message);
                }


                var trans = DBVisitor.Connection.BeginTransaction();
                try
                {
                    //拼装交易字符串 
                    //--1 病人id
                    //--2 检查项目id
                    //--3 检查部位id列表
                    //--4 临床诊断
                    //--5 院区id
                    //--6 应用id
                    //--7 操作员工号
                    //--8 中间表申请单序号
                    string jianChaBWDM = string.Empty;
                    InObject.JIANCHALB.ForEach(o => jianChaBWDM += o.JIANCHASTBW + "#");
                    jianChaBWDM = jianChaBWDM.Trim('#');
                    string jiaoyiZfc = InObject.BINGRENID;//病人id
                    jiaoyiZfc += "|" + InObject.JIANCHAXMDM;//检查项目id
                    jiaoyiZfc += "|" + jianChaBWDM;//检查部位id列表
                    jiaoyiZfc += "|" + InObject.ZHENDUAN;//临床诊断
                    jiaoyiZfc += "|" + InObject.BASEINFO.FENYUANDM;//院区id
                    jiaoyiZfc += "|" + "2301";//应用id
                    jiaoyiZfc += "|" + InObject.BASEINFO.CAOZUOYDM;//操作员工号
                    jiaoyiZfc += "|" + xh;//中间表申请单序号

                    //提交预约号-------------------------------------------------------------------------------------------
                    OracleParameter[] paramJiaoYi = new OracleParameter[3];
                    paramJiaoYi[0] = new OracleParameter("Prm_Msg", OracleType.VarChar);
                    paramJiaoYi[0].Value = jiaoyiZfc;
                    paramJiaoYi[0].Direction = ParameterDirection.Input;
                    paramJiaoYi[1] = new OracleParameter("prm_AppCode", OracleType.Number);
                    paramJiaoYi[1].Value = null;
                    paramJiaoYi[1].Direction = ParameterDirection.Output;
                    paramJiaoYi[2] = new OracleParameter("prm_DataBuffer", OracleType.VarChar);
                    paramJiaoYi[2].Value = null;
                    paramJiaoYi[2].Size = 2000;
                    paramJiaoYi[2].Direction = ParameterDirection.Output;

                    log.InfoFormat("{0}", "执行存储过程：PKG_YJ_YUYUE_YH.Prc_YJ_ShuangXiangZZJC_JieKou \r\nPRM_JIAOYIZFC：" + jiaoyiZfc + "\r\n");

                    DBVisitor.ExecuteProcedure("PKG_YJ_YUYUE_YH.Prc_YJ_ShuangXiangZZJC_JieKou", paramJiaoYi, trans);

                    if (paramJiaoYi[1].Value.ToString() != "1")
                    {
                        throw new Exception("his系统计费失败！");
                    }

                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    var resource1 = new HISYY_Cancel();
                    resource1.RequestNo = "";// listyyxx.Items["YYH"].ToString();
                    resource1.YYH = result.YYH;
                    resource1.JCH = result.PDH;
                    string url1= System.Configuration.ConfigurationManager.AppSettings["LaiDa_Url"];
                    string xml1 = XMLHandle.EntitytoXML<HISYY_Cancel>(resource1);
                    string outxml1 = WSServer.Call<HISYY_Cancel>(url, xml).ToString();
                    HISYY_Cancel_Result result1 = XMLHandle.XMLtoEntity<HISYY_Cancel_Result>(outxml);
                    throw ex;
                }

            }
            //检验，暂时先不考虑----------------------------------------------------------------------------
            #region 检验设备预约
            else
            {
                //                //调用包，判断入参是否正确
                //                var jyjcdxx = "";
                //                var jyjcmx = "";
                //                var zdmx = "";
                //                var jcxmdm = "";
                //                int i = 0;
                //                if (InObject.JIANCHALB.Count < 1)
                //                {
                //                    throw new Exception(string.Format("检查项目不能为空！"));
                //                }
                //                //检查单信息
                //                jyjcdxx += InObject.JIUZHENKLX + "|";//	就诊卡类型
                //                jyjcdxx += InObject.JIUZHENKH + "|";//	就诊卡号
                //                jyjcdxx += SONGJIANKS + "|";//	送检医生
                //                jyjcdxx += SONGJIANYS + "|";//	送检科室
                //                jyjcdxx += InObject.YUYUESF + "|";//	收费识别
                //                jyjcdxx += InObject.BINGQINGMS + "|";//	病情描述
                //                jyjcdxx += InObject.ZHENDUAN + "|";//	诊断
                //                jyjcdxx += InObject.BINGRENTZ + "|";//	病人体征
                //                jyjcdxx += InObject.QITAJC + "|";//	其它检查
                //                jyjcdxx += InObject.BINGRENZS + "|";//	病人主诉
                //                jyjcdxx += InObject.YEWULY + "|";//	检查来源
                //                jyjcdxx += InObject.BINGRENXM + "|";//	病人姓名
                //                jyjcdxx += InObject.SHENFENZH + "|";//	病人身份证号
                //                jyjcdxx += "0" + "|";//	接收方式
                //                jyjcdxx += "|";//	检查申请单号
                //                jyjcdxx += InObject.YUYUERQ + " " + InObject.YUYUESJ + "|";//	检查日期
                //                jyjcdxx += InObject.BINGRENXB + "|";//	病人性别
                //                jyjcdxx += InObject.BINGRENNL;//	病人年龄

                //                //检验检查明细
                //                foreach (var item in InObject.JIANCHALB)
                //                {
                //                    if (i == 0)
                //                        jcxmdm = item.JIANCHAXMBH;
                //                    else
                //                        jcxmdm += "," + item.JIANCHAXMBH;
                //                    ++i;
                //                    jyjcmx += item.JIANCHAXMBH + "|";//	检查项目编号
                //                    jyjcmx += item.JIANCHAXMMC + "|";//	检查项目名称
                //                    jyjcmx += item.JIANCHAFLBM + "|";//	检查分类编码
                //                    jyjcmx += item.JIANCHASTBW + "|";//	检查身体部位
                //                    jyjcmx += item.JIANCHAFXDM + "|";//	检查方向代码
                //                    jyjcmx += item.JIANCHAZYDM + "|";//	检查肢位代码
                //                    jyjcmx += item.JIANCHATS + "^";//	检查提示

                //                }
                //                //疾病明细
                //                foreach (var item in InObject.ZHENDUANLB)
                //                {
                //                    zdmx += item.ICD10 + "|";//	ICD10
                //                    zdmx += item.ZHENDUANMC + "^";//	诊断名称
                //                }

                //                DBServer db = new DBServer();
                //                //调用检查开单包-------------------------------------------------------------------------------------------
                //                OracleParameter[] jiaoYi ={
                //                                new OracleParameter("ywlx",OracleDbType.VarChar,ParameterDirection.Input), 
                //                                new OracleParameter("jyjcdxx",OracleDbType.VarChar,ParameterDirection.Input), 
                //                                new OracleParameter("jyjcmx",OracleDbType.VarChar,ParameterDirection.Input), 
                //                                new OracleParameter("zdmx",OracleDbType.VarChar,ParameterDirection.Input), 
                //                                new OracleParameter("jylx",OracleDbType.VarChar,ParameterDirection.Input),                              
                //                                new OracleParameter("errno",OracleDbType.Integer,ParameterDirection.Output),
                //                                new OracleParameter("errmsg",OracleDbType.Integer,ParameterDirection.Output),
                //                                new OracleParameter("outdata",OracleDbType.VarChar,ParameterDirection.Output)
                //                                                };
                //                jiaoYi[0].Value = 2;
                //                jiaoYi[1].Value = jyjcdxx;
                //                jiaoYi[2].Value = jyjcmx;
                //                jiaoYi[3].Value = zdmx;
                //                jiaoYi[4].Value = 1;
                //                jiaoYi[5].Value = -1;
                //                jiaoYi[6].Value = string.Empty.PadRight(1024);
                //                jiaoYi[7].Value = string.Empty.PadRight(1024);
                //                try
                //                {
                //                    db.DbProOption("pkg_sxzz_jckd.prc_jsjcd", jiaoYi, 5);
                //                }
                //                catch (Exception ex)
                //                {
                //                    tradeOut = WcfCommon.GetXmlString(tradeHead, tradeType, tradeDetail, new DataTable(), -1, ex.Message);
                //                    return -1;
                //                }
                //                if (jiaoYi[5].Value.ToString() != "0")
                //                {
                //                    throw new Exception("更新预约信息失败:" + jiaoYi[6].Value.ToString());
                //                }

                //                OracleConnection conn = new OracleConnection(MediTrade.Common.ConfigInfo.SqlConn);
                //                OracleCommand comm = new OracleCommand();
                //                comm.Connection = conn;
                //                if (conn.State != System.Data.ConnectionState.Open)
                //                {
                //                    conn.Open();
                //                }
                //                OracleTransaction tran = conn.BeginTransaction();
                //                comm.Transaction = tran;
                //                try
                //                {
                //                    //先插入到临时表
                //                    var ID = new DBServer().GetCurrData("select seq_sxzz_jianchad.nextval from dual");
                //                    var xh = "";
                //                    var JCSQDH = "";
                //                    int k = 0;
                //                    if (ID == null)
                //                    {
                //                        throw new Exception(string.Format("检查单序列获取失败！"));
                //                    }
                //                    else
                //                    {
                //                        xh = ID.ToString();
                //                    }
                //                    var SQDH = new DBServer().GetCurrData("select SEQ_GY_YXSQD_SQDH.nextval from dual");
                //                    if (SQDH == null)
                //                    {
                //                        throw new Exception(string.Format("申请单号获取失败！"));
                //                    }
                //                    else
                //                    {
                //                        JCSQDH = SQDH.ToString();
                //                    }
                //                    //插入检查单信息
                //                    string insJcd = @"insert into sxzz_jianchad(JIANCHADXH,JIUZHENKLX,JIUZHENKH,BINGRENXM,
                //                                      BINGRENSFZH,SONGJIANYS,SONGJIANKS,SHOUFEISB,
                //                                      BINGQINGMS,ZHENDUAN,BINGRENTZ,QITAJC,
                //                                      BINGRENZS,JIANCHALY,JIESHOUFS,JIESHOURQ,
                //                                      JIESHOUBZ,JIANCHASQDH,JIANCHARQ,BINGRENXB,BINGRENNL,YYSQDBH)
                //                                      values({0},'{1}','{2}','{3}',
                //                                            '{4}','{5}','{6}','{7}',
                //                                            '{8}','{9}','{10}','{11}',
                //                                            '{12}','{13}','{14}',sysdate,
                //                                            0,'{15}','{16}','{17}','{18}','{19}')";
                //                    insJcd = string.Format(insJcd, xh, "", InObject.BINGRENMZH, InObject.BINGRENXM,
                //                                InObject.SHENFENZH, InObject.SHENQINGYSGH, InObject.JIANCHAKSDM, 0,
                //                                InObject.BINGQINGMS, InObject.ZHENDUAN, InObject.BINGRENTZ, InObject.QITAJC,
                //                                InObject.BINGRENZS, InObject.YEWULX, 0, JCSQDH, InObject.YUYUERQ,
                //                                InObject.BINGRENXB, InObject.BINGRENNL, yysqdBh);
                //                    comm.CommandText = insJcd;
                //                    comm.ExecuteNonQuery();
                //                    //插入检查明细
                //                    foreach (var item in InObject.JIANCHALB)
                //                    {
                //                        ++k;
                //                        string insJcmx = @"insert into sxzz_jianchadxm (JIANCHADXH,JIANCHADMXXH,JIANCHAXMBH,
                //                                          JIANCHAXMMC,JIANCHAFLBM,JIANCHASTBW,
                //                                          JIANCHAFXDM,JIANCHAZYDM,JIANCHATS)
                //                                          values({0},{1},'{2}',
                //                                          '{3}','{4}','{5}',
                //                                          '{6}','{7}','{8}')";
                //                        insJcmx = string.Format(insJcmx, xh, k, item.JIANCHAXMBH,
                //                            item.JIANCHAXMMC, item.JIANCHAFLBM, item.JIANCHASTBW,
                //                            item.JIANCHAFXDM, item.JIANCHAZYDM, item.JIANCHATS);
                //                        comm.CommandText = insJcmx;
                //                        comm.ExecuteNonQuery();
                //                    }
                //                    k = 0;
                //                    //插入诊断明细
                //                    foreach (var item in InObject.ZHENDUANLB)
                //                    {
                //                        ++k;
                //                        string insZdMx = @"insert into sxzz_jianchadzd (jianchadxh,jianchadzdxh,icd10,zhenduanmc)
                //                                           values({0},{1},'{2}','{3}')";
                //                        insZdMx = string.Format(insZdMx, xh, k, item.ICD10, item.ZHENDUANMC);
                //                        comm.CommandText = insZdMx;
                //                        comm.ExecuteNonQuery();
                //                    }

                //                    var yysqlsh = new DBServer().GetCurrData("select seq_fsdyy_sq_yysqlsh.nextval yysqlsh from dual");
                //                    //插入预约申请信息fdsyy_sq
                //                    string insYySqd = @"Insert into fsdyy_sq (yysqlsh,yysqdbh,yysqdmc,yysqdzt,jcksdm,jcksmc,brfph,brlx,brlxmc,brkh,brmzh,brzyh,brbqdm,brbqmc,brcwh,brxm,
                //                                        brxb,brnl,brcsrq,brlxdz,brlxdh,sqysgh,sqysmc,sqksdm,sqksmc,sqyydm,sqyymc,sqsj,jch,jcrq,jcsj,jcxmdm,jcxmmc
                //                                        ,jcxmlx,jcbwdm,jcbwmc,jcsbdm,jcsbmc,jcsbdd,yyh,sfzh,yysf,jcsqdbh,yxfx,yysjd,xxapsj,yyly,ywlx,sfzq,sfjz,sfls,yyrq,yyhxx)
                //                                        values({42}, --预约申请流水号
                //                                               {43},     --预约申请单编号
                //                                               '{0}',     --预约申请单名称
                //                                               {1},     --预约申请单状态(0未确认,1已确认,9作废)
                //                                               '{2}',     --检查科室代码
                //                                               '{3}',     --检查科室名称
                //                                               '{4}',     --病人发票号
                //                                               {5},     --病人类型
                //                                               '{6}',     --病人类型名称
                //                                               '{7}',     --病人卡号
                //                                               '{8}',     --病人门诊号
                //                                               '{9}',     --病人住院号
                //                                               '{10}',     --病人病区代码
                //                                               '{11}',     --病人病区名称
                //                                               '{12}',     --病人床位号
                //                                               '{13}',     --病人姓名
                //                                               {14},     --病人性别
                //                                               '{15}',     --病人年龄
                //                                               to_date('{16}','yyyy-MM-dd'),     --病人出生日期
                //                                               '{17}',     --病人联系地址
                //                                               '{18}',     --病人联系电话
                //                                               '{19}',     --申请医生工号
                //                                               '{20}',     --申请医生姓名
                //                                               '{21}',     --申请科室代码
                //                                               '{22}',     --申请科室名称
                //                                               '{23}',     --申请医院代码
                //                                               '{24}',     --申请医院名称
                //                                               sysdate,     --申请时间
                //                                               '{25}',     --检查号
                //                                               '{26}',     --检查日期
                //                                               '{27}',     --检查时间
                //                                               '{28}',     --检查项目代码
                //                                               '{29}',     --检查项目名称
                //                                               '{30}',     --检查项目类型
                //                                               '{31}',     --检查部位代码
                //                                               '{32}',     --检查部位名称
                //                                               '{33}',     --检查设备代码
                //                                               '{34}',     --检查设备名称
                //                                               '{35}',     --检查设备地点
                //                                               '{36}',     --预约号
                //                                               '{37}',     --身份证号
                //                                               {38},     --预约收费(0未收费，1已收费)
                //                                               '{39}',     --检查申请单编号
                //                                               '{40}',      --影像方向
                //                                               '{41}',      --预约时间段
                //                                               '{44}',      --详细安排时间
                //                                               '{45}',        --预约来源
                //                                               '{46}',       --业务类型
                //                                               '{47}',      --是否增强
                //                                               '{48}',      --是否急诊
                //                                               '{49}',      --是否临时
                //                                               to_date('{50}','yyyy-mm-dd HH24:mi:ss'),--预约日期 
                //                                               '{51}')     --预约号信息";
                //                    insYySqd = string.Format(insYySqd, "申请单",//预约申请单名称
                //                       InObject.YUYUEZT,//预约申请单状态(0未确认,1已确认,9作废)
                //                       InObject.JIANCHAKSDM,//检查科室代码
                //                       InObject.JIANCHAKSMC,//检查科室名称
                //                       InObject.BINGRENFPH,//病人发票号
                //                       InObject.BINGRENLX,//病人类型
                //                       InObject.BINGRENLXMC,//病人类型名称
                //                       InObject.BINGRENKH,//病人卡号
                //                       InObject.BINGRENMZH,//病人门诊号
                //                       InObject.BINGRENZYH,//病人住院号
                //                       InObject.BINGRENBQDM,//病人病区代码
                //                       InObject.BINGRENBQMC,//病人病区名称
                //                       InObject.BINGRENCWH,//病人床位号
                //                       InObject.BINGRENXM,//病人姓名
                //                       InObject.BINGRENXB,//病人性别
                //                       InObject.BINGRENNL,//病人年龄
                //                       InObject.BINGRENCSRQ,//病人出生日期
                //                       InObject.BINGRENLXDZ,//病人联系地址
                //                       InObject.BINGRENLXDH,//病人联系电话
                //                       InObject.SHENQINGYSGH,//申请医生工号
                //                       InObject.SHENQINGYSMC,//申请医生姓名
                //                       "",//申请科室代码
                //                       "",//申请科室名称
                //                       InObject.SHENQINGYYDM,//申请医院代码
                //                       InObject.SHENQINGYYMC,//申请医院名称
                //                       JCSQDH,//检查号
                //                       InObject.YUYUERQ,//检查日期
                //                       InObject.YUYUESJ,//检查时间
                //                       jcxmdm,//InObject.JIANCHAXMDM,//检查项目代码
                //                       InObject.JIANCHAXMMC,//检查项目名称
                //                       InObject.JIANCHAXMLX,//检查项目类型
                //                       InObject.JIANCHABWDM,//检查部位代码
                //                       InObject.JIANCHABWMC,//检查部位名称
                //                       InObject.JIANCHASBDM,//检查设备代码
                //                       InObject.JIANCHASBMC,//检查设备名称
                //                       InObject.JIANCHASBDD,//检查设备地点
                //                       "",//预约号
                //                       InObject.SHENFENZH,//身份证号
                //                       InObject.YUYUESF,//预约收费(0未收费，1已收费)
                //                       InObject.JIANCHASQDBH,//检查申请单编号
                //                       InObject.YINGXIANGFX,//影像方向
                //                       "",//时间段
                //                       yysqlsh,//预约申请流水号
                //                       yysqdBh,//预约申请单编号
                //                       InObject.XIANGMUHS,//详细安排时间
                //                       InObject.YEWULY,
                //                       InObject.YEWULX,
                //                       InObject.ZENGQIANG,
                //                       InObject.JIZHEN,
                //                       InObject.LINSHI,
                //                       InObject.YUYUERQ + " " + InObject.YUYUESJ,//预约日期
                //                       0);
                //                    comm.CommandText = insYySqd;
                //                    comm.ExecuteNonQuery();

                //                    tran.Commit();
                //                    conn.Close();

                //                    OutObject = new SHEBEIYY_OUT();
                //                    //OutObject.YUYUERQ = InObject.YUYUERQ;
                //                    //OutObject.YUYUESJ = InObject.YUYUESJ;
                //                    //OutObject.YUYUEH = result.YYH;
                //                    //OutObject.JIANCHAH = result.JCH;
                //                    OutObject.YUYUESQDBH = yysqdBh;
                //                }
                //                catch (Exception ex)
                //                {
                //                    tran.Rollback();
                //                    conn.Close();
                //                    throw ex;
                //                }
            }
            #endregion
            //---------------------------------------------------------------------------------------------            

        }

    }


}
