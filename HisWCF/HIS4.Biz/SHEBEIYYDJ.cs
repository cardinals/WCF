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
    public class SHEBEIYYDJ : IMessage<SHEBEIYYDJ_IN, SHEBEIYYDJ_OUT>
    {
        ILog log = log4net.LogManager.GetLogger("SqlInfo");
        ILog logLaiDa = log4net.LogManager.GetLogger("LaiDaInfo");
        public override void ProcessMessage()
        {
           try{
                string list_JianChaDXH = InObject.YIZHUID;//医嘱ID
                if(string.IsNullOrEmpty(list_JianChaDXH)){
                    throw new Exception("医技申请单号为空或传入节点有误");
                }
                
                string[] JianChaDanID = list_JianChaDXH.Split('|');

                for (int i = 0; i < JianChaDanID.Length; i++)
                {
                    var resource = new HISYY_Register();
                    resource.HospitalCode = ConfigurationManager.AppSettings["HospitalCode_Fck"].ToString();
                    resource.HospitalName = ConfigurationManager.AppSettings["HospitalName_Fck"].ToString();
                    string jcsqdSql = @"SELECT A.BINGRENXM,
                                   A.BINGRENSFZH,
                                   A.Shenqingdid,
                                   E.BINGRENID,
                                   A.BINGRENXB,
                                   A.CHUSHENGRQ AS CHUSHENRQ,
                                   A.BINGRENNL AS NIANLING,
                                   A.BINGRENLXDH AS LIANXIDH,
                                   A.BINGRENLXDZ AS DIZHI,
                                   E.JIUZHENKH,
                                   A.YIJISBDM AS SHEBEIDM,
                                   A.YIJISBMC AS SHEBEIMC,
                                   B.JIANCHASTBW,
                                   B.JIANCHAXMMC,
                                   F.FEIYONGHJ,
                                   H.FAPIAOHM,
                                   G.ZHIXINGKS,
                                   G.ZHIXINGKSMC,
                                   A.JIESHOURQ,
                                   A.YIJIYYRQ,
                                   A.YIJISQDH,
                                   A.YIJIYYH,
                                   A.SONGJIANKSDM,
                                   A.SONGJIANYSGH,
                                   A.YIJIXXAPSJ
                                    FROM SXZZ_JIANCHASQD A,SXZZ_JIANCHASQDMX B,GY_BINGRENXX E,  
                                         YJ_SHENQINGDAN F,MZ_YIJI1 G,MZ_FAPIAO1 H,MZ_YIJI2 I
                                WHERE A.JIANCHASQDID = B.JIANCHASQDID 
                                    AND A.BINGRENSFZH = E.SHENFENZH 
                                    AND A.Shenqingdid = F.SHENQINDANID 
                                    AND G.FAPIAOID = H.FAPIAOID
                                    AND G.YIJIID = I.YIJIID
                                    AND I.YIZHUID = F.YIZHUID
                                    AND I.YIZHUID = {0}
                                    AND ROWNUM = 1";
                    //,SXZZ_JIANCHASQDZD D  AND A.JIANCHASQDID = D.JIANCHASQDID 
                    jcsqdSql = string.Format(jcsqdSql, JianChaDanID[i]);
                    
                    DataTable jcsqdDt = DBVisitor.ExecuteTable(jcsqdSql);
                    if (jcsqdDt.Rows.Count == 0)
                    {
                        throw new Exception( "HIS中检索不到相关记录：" + list_JianChaDXH);
                    }
                    foreach (DataRow dr in jcsqdDt.Rows)
                    {
                        resource.AdmissionSource = "50"; //病人类型
                        resource.PatientName = dr["BINGRENXM"].ToString();
                        resource.IdNumber = dr["BINGRENSFZH"].ToString();//身份证号
                        resource.RequestNo = dr["Shenqingdid"].ToString(); //申请单号
                        resource.AdmissionID = dr["BINGRENID"].ToString(); //门诊/住院 号
                        resource.ExaminePartTime = dr["YIJIXXAPSJ"].ToString(); //项目耗时
                        resource.PatientSex = dr["BINGRENXB"].ToString();
                        resource.PatientBorn = dr["CHUSHENRQ"].ToString();
                        resource.PatientAge = dr["NIANLING"].ToString();
                        resource.PatientTel = dr["LIANXIDH"].ToString();
                        resource.PatientAddress = dr["DIZHI"].ToString();
                        resource.PatientCard = dr["JIUZHENKH"].ToString(); //就诊卡号
                        resource.InPatientAreaName = ""; //病区名称
                        resource.InPatientAreaCode = ""; //病区代码
                        resource.BedNum = ""; //床号
                        resource.DeviceCode = dr["SHEBEIDM"].ToString(); //设备代码
                        resource.DeviceName = dr["SHEBEIMC"].ToString(); //设备名称
                        resource.StudiesExamine.Add(new StudiesExamine()
                        {
                            //HT  更改CODE取值
                            //ExamineCode = jcxmDt.Rows[i]["JIANCHAXMID"].ToString(),
                            ExamineCode = dr["JIANCHASTBW"].ToString(), //检查项目代码
                            ExamineName = XMLHandle.encodeString(dr["JIANCHAXMMC"].ToString()), //项目名称
                            Numbers = "1", //数量
                            ExaminePrice = dr["FEIYONGHJ"].ToString()//单价
                        });
                        resource.ReceiptNum = dr["FAPIAOHM"].ToString(); //发票号码
                        resource.ZxDepartmentId = dr["ZHIXINGKS"].ToString(); //执行科室代码
                        resource.ZxDepartmentName = dr["ZHIXINGKSMC"].ToString(); //执行科室名称
                        resource.Sqrq = dr["JIESHOURQ"].ToString(); //。申请日期
                        resource.ExamineFY = dr["FEIYONGHJ"].ToString(); //检查费用
                        resource.YjsbCode = ""; //设备代码
                        resource.BespeakDateTime = dr["YIJIYYRQ"].ToString(); //预约时间
                        resource.PDH = ""; //排队号
                        resource.JCH = dr["YIJISQDH"].ToString(); //检查号
                        resource.YYH = dr["YIJIYYH"].ToString(); //预约号
                        resource.RequestDepartmentId = dr["SONGJIANKSDM"].ToString(); //申请科室代码
                        resource.RequestDepartmentName = ""; //申请科室名称
                        resource.RequestDoctorId = dr["SONGJIANYSGH"].ToString(); //申请医生
                        resource.RequestDoctorName = "";

                        string url = System.Configuration.ConfigurationManager.AppSettings["LaiDa_Url"];
                        string xml = XMLHandle.EntitytoXML<HISYY_Register>(resource);

                        logLaiDa.InfoFormat(this.GetType().Name + "设备登记调用莱达XML入参：" + xml);
                        //LogHelper.WriteLog(typeof(GG_ShuangXiangZzBLL), "设备预约调用莱达XML入参：" + xml);
                        string outxml = WSServer.Call<HISYY_Register>(url, xml).ToString();

                        logLaiDa.InfoFormat(this.GetType().Name + "设备预约调用莱达XML出参：" + outxml);
                        HISYY_Register_Result result = XMLHandle.XMLtoEntity<HISYY_Register_Result>(outxml);

                        if (result.Success == "False")
                        {
                           throw new Exception("预约登记失败,错误原因：" + result.Message);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }

    
}
