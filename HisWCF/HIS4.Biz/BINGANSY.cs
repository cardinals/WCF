using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using System.Configuration;
using SWSoft.Framework;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;
using HIS4.Schemas;


namespace HIS4.Biz
{
    public class BINGANSY : IMessage<BINGANSY_IN, BINGANSY_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new BINGANSY_OUT();

            string zhuYuanBRID = InObject.ZHUYUANBRID;//住院病人ID

            #region 基本入参判断
            if (string.IsNullOrEmpty(zhuYuanBRID))
            {
                throw new Exception("住院病人编号不能为空！");
            }
            #endregion

            string bingAnShouYe = "select * from V_IPT_MEDICALRECORDPAGE_NEW where jylsh = '{0}'";

            DataTable dtBASY = DBVisitor.ExecuteTable(string.Format(bingAnShouYe, zhuYuanBRID));

            if (dtBASY != null && dtBASY.Rows.Count > 0)
            {
                for (int i = 0; i < dtBASY.Rows.Count; i++)
                {
                    BINGANXX baxx = new BINGANXX();
                    baxx.JGDM = dtBASY.Rows[i]["JGDM"].ToString();                                                    //机构代码                           
                    baxx.JYLSH = dtBASY.Rows[i]["JYLSH"].ToString();                                                  //就诊流水号                         
                    baxx.KH = dtBASY.Rows[i]["KH"].ToString();                                                        //卡号                               
                    baxx.KLX = dtBASY.Rows[i]["KLX"].ToString();                                                      //卡类型                             
                    baxx.MEDICALPAYMENT = dtBASY.Rows[i]["MEDICALPAYMENT"].ToString();                                //医疗付费方式                       
                    baxx.HEALTHCARDID = dtBASY.Rows[i]["HEALTHCARDID"].ToString();                                    //居民健康卡号                       
                    baxx.HOSPIZATIONID = dtBASY.Rows[i]["HOSPIZATIONID"].ToString();                                  //住院号                             
                    baxx.MEDICALRECORDID = dtBASY.Rows[i]["MEDICALRECORDID"].ToString();                              //病案号                             
                    baxx.XM = dtBASY.Rows[i]["XM"].ToString();                                                        //姓名                               
                    baxx.SEX = dtBASY.Rows[i]["SEX"].ToString();                                                      //性别                               
                    baxx.NL = dtBASY.Rows[i]["NL"].ToString();                                                        //年龄                               
                    baxx.HYZKDM = dtBASY.Rows[i]["HYZKDM"].ToString();                                                //婚姻状况代码                       
                    baxx.ZYLBDM = dtBASY.Rows[i]["ZYLBDM"].ToString();                                                //职业类别代码                       
                    baxx.GJ = dtBASY.Rows[i]["GJ"].ToString();                                                        //国籍                               
                    baxx.CSRQSJ = dtBASY.Rows[i]["CSRQSJ"].ToString();                                                //出生日期时间                       
                    baxx.MONTHSAGE = dtBASY.Rows[i]["MONTHSAGE"].ToString();                                          //月龄                               
                    baxx.NEONATALBIRTHWEIGHT = dtBASY.Rows[i]["NEONATALBIRTHWEIGHT"].ToString();                      //新生儿出生体重                     
                    baxx.NEONATALADMISSIONWEIGHT = dtBASY.Rows[i]["NEONATALADMISSIONWEIGHT"].ToString();              //新生儿入院体重                     
                    baxx.PROVINCE = dtBASY.Rows[i]["PROVINCE"].ToString();                                            //出生地-省（区、市）                
                    baxx.CITY = dtBASY.Rows[i]["CITY"].ToString();                                                    //出生地-市                          
                    baxx.COUNTY = dtBASY.Rows[i]["COUNTY"].ToString();                                                //出生地-县                          
                    baxx.ORIGINPROVINCE = dtBASY.Rows[i]["ORIGINPROVINCE"].ToString();                                //籍贯-省（区、市）                  
                    baxx.ORIGINCITY = dtBASY.Rows[i]["ORIGINCITY"].ToString();                                        //籍贯-市                            
                    baxx.PRESENTADDRPROVINCE = dtBASY.Rows[i]["PRESENTADDRPROVINCE"].ToString();                      //现住址-省（区、市）                
                    baxx.PRESENTADDRCITY = dtBASY.Rows[i]["PRESENTADDRCITY"].ToString();                              //现住址-市                          
                    baxx.PRESENTADDRCOUNTY = dtBASY.Rows[i]["PRESENTADDRCOUNTY"].ToString();                          //现住址-县                          
                    baxx.PRESENTADDROTHER = dtBASY.Rows[i]["PRESENTADDROTHER"].ToString();                            //现住址-其它                        
                    baxx.PRESENTADDRPOSTALCODE = dtBASY.Rows[i]["PRESENTADDRPOSTALCODE"].ToString();                  //现住址-邮编                        
                    baxx.PATIENTPHONE = dtBASY.Rows[i]["PATIENTPHONE"].ToString();                                    //患者电话号码                       
                    baxx.REGISTEREDPERMANENTRESIDENCEPR = dtBASY.Rows[i]["REGISTEREDPERMANENTRESIDENCEPR"].ToString();//户口地址-省（区、市）              
                    baxx.REGISTEREDPERMANENTRESIDENCECI = dtBASY.Rows[i]["REGISTEREDPERMANENTRESIDENCECI"].ToString();//户口地址-市                        
                    baxx.REGISTEREDPERMANENTRESIDENCETO = dtBASY.Rows[i]["REGISTEREDPERMANENTRESIDENCETO"].ToString();//户口地址-县                        
                    baxx.REGISTEREDPERMANENTRESIDENCEOT = dtBASY.Rows[i]["REGISTEREDPERMANENTRESIDENCEOT"].ToString();//户口地址-其它                      
                    baxx.REGISTEREDPERMANENTRESIDENCEPO = dtBASY.Rows[i]["REGISTEREDPERMANENTRESIDENCEPO"].ToString();//户口地址-邮编                      
                    baxx.WORKADDRPHONE = dtBASY.Rows[i]["WORKADDRPHONE"].ToString();                                  //工作单位电话号码                   
                    baxx.ADMISSIONWAY = dtBASY.Rows[i]["ADMISSIONWAY"].ToString();                                    //入院途径                           
                    baxx.ADMISSIONDATETIME = dtBASY.Rows[i]["ADMISSIONDATETIME"].ToString();                          //入院日期时间                       
                    baxx.ADMISSIONDEPTNAME = dtBASY.Rows[i]["ADMISSIONDEPTNAME"].ToString();                          //入院科室名称                       
                    baxx.ADMISSIONSICKROOM = dtBASY.Rows[i]["ADMISSIONSICKROOM"].ToString();                          //入院病房                           
                    baxx.TRANSFERREDDEPTNAME = dtBASY.Rows[i]["TRANSFERREDDEPTNAME"].ToString();                      //转科科室名称                       
                    baxx.DISCHARGEDATETIME = dtBASY.Rows[i]["DISCHARGEDATETIME"].ToString();                          //出院日期时间                       
                    baxx.DISCHARGEDEPARTMENT = dtBASY.Rows[i]["DISCHARGEDEPARTMENT"].ToString();                      //出院科室名称                       
                    baxx.DISCHARGESICKROOM = dtBASY.Rows[i]["DISCHARGESICKROOM"].ToString();                          //出院病房                           
                    baxx.ACTUALHOSPITALIZATIONDAYS = dtBASY.Rows[i]["ACTUALHOSPITALIZATIONDAYS"].ToString();          //实际住院天数                       
                    baxx.CLINICDIAGNOSIS = dtBASY.Rows[i]["CLINICDIAGNOSIS"].ToString();                              //门（急）诊诊断名称                 
                    baxx.CLINICWESTERNDIAGNOSISCODE = dtBASY.Rows[i]["CLINICWESTERNDIAGNOSISCODE"].ToString();        //门（急）诊诊断（西医诊断）-疾病编码
                    baxx.MASTERDISEASENAME = dtBASY.Rows[i]["MASTERDISEASENAME"].ToString();                          //出院诊断-主要诊断-疾病名称         
                    baxx.MASTERDISEASECODE = dtBASY.Rows[i]["MASTERDISEASECODE"].ToString();                          //出院诊断-主要诊断-疾病编码         
                    baxx.MASTERADMISSIONCONDITION = dtBASY.Rows[i]["MASTERADMISSIONCONDITION"].ToString();            //出院诊断-主要诊断-入院病情         
                    baxx.MASTERDISCHARGEPROGNOSISCONDIT = dtBASY.Rows[i]["MASTERDISCHARGEPROGNOSISCONDIT"].ToString();//出院诊断-主要诊断-出院情况         
                    baxx.OUTSIDEREASONOFINJURYANDPOISON = dtBASY.Rows[i]["OUTSIDEREASONOFINJURYANDPOISON"].ToString();//损伤中毒的外部原因                 
                    baxx.OUTSIDEREASONOFINJURYANDPOCODE = dtBASY.Rows[i]["OUTSIDEREASONOFINJURYANDPOCODE"].ToString();//损伤中毒的外部原因-疾病编码        
                    baxx.PATHOLOGYDISEASENAME = dtBASY.Rows[i]["PATHOLOGYDISEASENAME"].ToString();                    //病理诊断-疾病名称                  
                    baxx.PATHOLOGYDISEASECODE = dtBASY.Rows[i]["PATHOLOGYDISEASECODE"].ToString();                    //病理诊断-疾病编码                  
                    baxx.PATHOLOGYID = dtBASY.Rows[i]["PATHOLOGYID"].ToString();                                      //病理号                             
                    baxx.DRUGALLERGY = dtBASY.Rows[i]["DRUGALLERGY"].ToString();                                      //药物过敏                           
                    baxx.ALLERGICDRUG = dtBASY.Rows[i]["ALLERGICDRUG"].ToString();                                    //过敏药物                           
                    baxx.AUTOPSYSIGN = dtBASY.Rows[i]["AUTOPSYSIGN"].ToString();                                      //死亡患者尸检标志                   
                    baxx.ABOXX = dtBASY.Rows[i]["ABOXX"].ToString();                                                  //ABO 血型代码                       
                    baxx.RHXX = dtBASY.Rows[i]["RHXX"].ToString();                                                    //RH血型代码                         
                    baxx.DEPTMANAGER = dtBASY.Rows[i]["DEPTMANAGER"].ToString();                                      //科主任签名                         
                    baxx.CHIEFDOCTORSIGN = dtBASY.Rows[i]["CHIEFDOCTORSIGN"].ToString();                              //主任（副主任）医师签名             
                    baxx.ATTENDINGDOCTOR = dtBASY.Rows[i]["ATTENDINGDOCTOR"].ToString();                              //主治医师签名                       
                    baxx.HOSPIZATIONDOCTOR = dtBASY.Rows[i]["HOSPIZATIONDOCTOR"].ToString();                          //住院医师签名                       
                    baxx.RESPONSIBILITYNURSE = dtBASY.Rows[i]["RESPONSIBILITYNURSE"].ToString();                      //责任护士签名                       
                    baxx.REFRESHERDOCTORS = dtBASY.Rows[i]["REFRESHERDOCTORS"].ToString();                            //进修医师签名                       
                    baxx.INTERNDOCTOR = dtBASY.Rows[i]["INTERNDOCTOR"].ToString();                                    //实习医师签名                       
                    baxx.MEDICALRECORDCODERSIGN = dtBASY.Rows[i]["MEDICALRECORDCODERSIGN"].ToString();                //病案编码员签名                     
                    baxx.MEDICALRECORDQUALITY = dtBASY.Rows[i]["MEDICALRECORDQUALITY"].ToString();                    //病案质量                           
                    baxx.QUALITYCONTROLDOCTOR = dtBASY.Rows[i]["QUALITYCONTROLDOCTOR"].ToString();                    //质控医师签名                       
                    baxx.QUALITYCONTROLSIGN = dtBASY.Rows[i]["QUALITYCONTROLSIGN"].ToString();                        //质控护士签名                       
                    baxx.QUALITYCONTROLDATE = dtBASY.Rows[i]["QUALITYCONTROLDATE"].ToString();                        //质控日期                           
                    baxx.DISCHARGEMETHODS = dtBASY.Rows[i]["DISCHARGEMETHODS"].ToString();                            //离院方式                           
                    baxx.PREPAREACCEPTHOSPITALNAME = dtBASY.Rows[i]["PREPAREACCEPTHOSPITALNAME"].ToString();          //拟接受医疗机构名称                 
                    baxx.DAY31INPATIENTMK = dtBASY.Rows[i]["DAY31INPATIENTMK"].ToString();                            //出院 31 天内再住院标志             
                    baxx.DAY31INPATIENTAIM = dtBASY.Rows[i]["DAY31INPATIENTAIM"].ToString();                          //出院 31 天内再住院目的             
                    baxx.STUPORTIMEBEFOREADMISSION = dtBASY.Rows[i]["STUPORTIMEBEFOREADMISSION"].ToString();          //颅脑损伤患者入院前昏迷时间         
                    baxx.STUPORTIMEAFTERADMISSION = dtBASY.Rows[i]["STUPORTIMEAFTERADMISSION"].ToString();            //颅脑损伤患者入院后昏迷时间         
                    baxx.HOSPIZATIONTOTALCOST = dtBASY.Rows[i]["HOSPIZATIONTOTALCOST"].ToString();                    //住院总费用（元）                   
                    baxx.HOSPIZATIONTOTALPERSONALCOST = dtBASY.Rows[i]["HOSPIZATIONTOTALPERSONALCOST"].ToString();    //住院总费用-自付金额（元）          
                    baxx.DIAGCOINOUTPATIENTVSDISCHARGE = dtBASY.Rows[i]["DIAGCOINOUTPATIENTVSDISCHARGE"].ToString();  //诊断符合情况-门诊和出院            
                    baxx.DIAGCOINADMSSIONVSDISCHARGE = dtBASY.Rows[i]["DIAGCOINADMSSIONVSDISCHARGE"].ToString();      //诊断符合情况-入院和出院            
                    baxx.DIAGCOINPREOPERATIVEVSPOST = dtBASY.Rows[i]["DIAGCOINPREOPERATIVEVSPOST"].ToString();        //诊断符合情况-术前和术后            
                    baxx.DIAGCOINCLINICALVSPATHOLOGICA = dtBASY.Rows[i]["DIAGCOINCLINICALVSPATHOLOGICA"].ToString();  //诊断符合情况-临床和病理            
                    baxx.DIAGCOINRADIOLOGYVSPATHOLOGY = dtBASY.Rows[i]["DIAGCOINRADIOLOGYVSPATHOLOGY"].ToString();    //诊断符合情况-放射和病理            
                    baxx.SALVAGECONDITIONSALVAGETIMES = dtBASY.Rows[i]["SALVAGECONDITIONSALVAGETIMES"].ToString();    //抢救情况-抢救次数                  
                    baxx.SALVAGECONDITIONSUCCESSTIMES = dtBASY.Rows[i]["SALVAGECONDITIONSUCCESSTIMES"].ToString();    //抢救情况-成功次数                  
                    baxx.CLINICALPATHMANAGEMENT = dtBASY.Rows[i]["CLINICALPATHMANAGEMENT"].ToString();                //临床路径管理                       
                    baxx.GENERMEDISERVCHARGE = dtBASY.Rows[i]["GENERMEDISERVCHARGE"].ToString();                      //一般医疗服务费                     
                    baxx.GENERTREATHANDLINGFEE = dtBASY.Rows[i]["GENERTREATHANDLINGFEE"].ToString();                  //一般治疗操作费                     
                    baxx.NURSE = dtBASY.Rows[i]["NURSE"].ToString();                                                  //护理费                             
                    baxx.GENERMEDISERVCHARGEOTHER = dtBASY.Rows[i]["GENERMEDISERVCHARGEOTHER"].ToString();            //综合医疗服务类其他费用             
                    baxx.PATHOLOGICALFEE = dtBASY.Rows[i]["PATHOLOGICALFEE"].ToString();                              //病理诊断费                         
                    baxx.LABORATORYFEE = dtBASY.Rows[i]["LABORATORYFEE"].ToString();                                  //实验室诊断费                       
                    baxx.IMAGINGFEE = dtBASY.Rows[i]["IMAGINGFEE"].ToString();                                        //影像学诊断费                       
                    baxx.CLINICALDIAGNOSISFEE = dtBASY.Rows[i]["CLINICALDIAGNOSISFEE"].ToString();                    //临床诊断项目费                     
                    baxx.NONOPERATIVETREATFEE = dtBASY.Rows[i]["NONOPERATIVETREATFEE"].ToString();                    //非手术治疗项目费                   
                    baxx.CLINICALPHYSICALTREATMENT = dtBASY.Rows[i]["CLINICALPHYSICALTREATMENT"].ToString();          //临床物理治疗费                     
                    baxx.SURGICALTREATMENT = dtBASY.Rows[i]["SURGICALTREATMENT"].ToString();                          //手术治疗费                         
                    baxx.ESTHETICFEE = dtBASY.Rows[i]["ESTHETICFEE"].ToString();                                      //麻醉费                             
                    baxx.OPERATIONFEE = dtBASY.Rows[i]["OPERATIONFEE"].ToString();                                    //手术费                             
                    baxx.REHABILITATIONFEE = dtBASY.Rows[i]["REHABILITATIONFEE"].ToString();                          //康复费                             
                    baxx.ZYZLFEE = dtBASY.Rows[i]["ZYZLFEE"].ToString();                                              //中医治疗费                         
                    baxx.XYFEE = dtBASY.Rows[i]["XYFEE"].ToString();                                                  //西药费                             
                    baxx.ANTIBACTERIALDRUGEXP = dtBASY.Rows[i]["ANTIBACTERIALDRUGEXP"].ToString();                    //抗菌药物费用                       
                    baxx.MEDICINECHINA = dtBASY.Rows[i]["MEDICINECHINA"].ToString();                                  //中成药费                           
                    baxx.HERBALMEDICINEFEE = dtBASY.Rows[i]["HERBALMEDICINEFEE"].ToString();                          //中草药费                           
                    baxx.BLOODFEE = dtBASY.Rows[i]["BLOODFEE"].ToString();                                            //血费                               
                    baxx.ACPFEE = dtBASY.Rows[i]["ACPFEE"].ToString();                                                //白蛋白类制品费                     
                    baxx.GCPFEE = dtBASY.Rows[i]["GCPFEE"].ToString();                                                //球蛋白类制品费                     
                    baxx.NXYZFEE = dtBASY.Rows[i]["NXYZFEE"].ToString();                                              //凝血因子类制品费                   
                    baxx.XBYZFEE = dtBASY.Rows[i]["XBYZFEE"].ToString();                                              //细胞因子类制品费                   
                    baxx.YCYYCXFEE = dtBASY.Rows[i]["YCYYCXFEE"].ToString();                                          //检查用一次性医用材料费             
                    baxx.ZLYYCXFEE = dtBASY.Rows[i]["ZLYYCXFEE"].ToString();                                          //治疗用一次性医用材料费             
                    baxx.SSYYCXZLFEE = dtBASY.Rows[i]["SSYYCXZLFEE"].ToString();                                      //手术用一次性医用材料费             
                    baxx.OTHERFEE = dtBASY.Rows[i]["OTHERFEE"].ToString();                                            //其它费用                           
                    baxx.UPLOADDATE = dtBASY.Rows[i]["UPLOADDATE"].ToString();                                        //上传时间                           
                    baxx.UPLOADFLAG = dtBASY.Rows[i]["UPLOADFLAG"].ToString();                                        //采集标志                           
                    baxx.CREATEDATE = dtBASY.Rows[i]["CREATEDATE"].ToString();                                        //新增时间                           
                    baxx.ZYCS = dtBASY.Rows[i]["ZYCS"].ToString();                                                    //住院次数                           
                    baxx.GZDWDZ = dtBASY.Rows[i]["GZDWDZ"].ToString();                                                //工作单位地址                       
                    baxx.DWYB = dtBASY.Rows[i]["DWYB"].ToString();                                                    //工作单位邮编                       
                    baxx.LXRXM = dtBASY.Rows[i]["LXRXM"].ToString();                                                  //联系人姓名                         
                    baxx.LXRGX = dtBASY.Rows[i]["LXRGX"].ToString();                                                  //联系人关系                         
                    baxx.LXRDZ = dtBASY.Rows[i]["LXRDZ"].ToString();                                                  //联系人地址                         
                    baxx.LXRDH = dtBASY.Rows[i]["LXRDH"].ToString();                                                  //联系人电话                         
                    baxx.RYHZHMSJT = dtBASY.Rows[i]["RYHZHMSJT"].ToString();                                          //入院颅脑损伤患者昏迷时间几天       
                    baxx.RYHZHMSJXS = dtBASY.Rows[i]["RYHZHMSJXS"].ToString();                                        //入院颅脑损伤患者昏迷时间几小时     
                    baxx.RYHZHMSJFZ = dtBASY.Rows[i]["RYHZHMSJFZ"].ToString();                                        //入院颅脑损伤患者昏迷时间几分钟     
                    baxx.CYHZHMSJT = dtBASY.Rows[i]["CYHZHMSJT"].ToString();                                          //出院颅脑损伤患者昏迷时间几天       
                    baxx.CYHZHMSJXS = dtBASY.Rows[i]["CYHZHMSJXS"].ToString();                                        //出院颅脑损伤患者昏迷时间几小时     
                    baxx.CYHZHMSJFZ = dtBASY.Rows[i]["CYHZHMSJFZ"].ToString();                                        //出院颅脑损伤患者昏迷时间几分钟     
                    baxx.YL1 = dtBASY.Rows[i]["YL1"].ToString();                                                      //预留一                             
                    baxx.YL2 = dtBASY.Rows[i]["YL2"].ToString();                                                      //预留二                             
                    baxx.SFZH = dtBASY.Rows[i]["SFZH"].ToString();                                                    //身份证号                           
                    baxx.CYZDQTZDJBMC = dtBASY.Rows[i]["CYZDQTZDJBMC"].ToString();                                    //出院诊断-其他诊断-疾病名称         
                    baxx.CYZDQTZDJBBM = dtBASY.Rows[i]["CYZDQTZDJBBM"].ToString();                                    //出院诊断-其他诊断-疾病编码         
                    baxx.CYZDQTZDCYQK = dtBASY.Rows[i]["CYZDQTZDCYQK"].ToString();                                    //出院诊断-其他诊断-出院情况         
                    baxx.SSJLLSH = dtBASY.Rows[i]["SSJLLSH"].ToString();                                              //手术记录流水号                     
                    baxx.MZYSGH = dtBASY.Rows[i]["MZYSGH"].ToString();                                                //麻醉医生工号                       
                    baxx.SSZSIIGH = dtBASY.Rows[i]["SSZSIIGH"].ToString();                                            //手术助手II工号                     
                    baxx.SSQKYHDJ = dtBASY.Rows[i]["SSQKYHDJ"].ToString();                                            //手术切口愈合等级                   
                    baxx.SSCZRQSJ = dtBASY.Rows[i]["SSCZRQSJ"].ToString();                                            //手术/操作日期时间                  
                    baxx.SSZSIGH = dtBASY.Rows[i]["SSZSIGH"].ToString();                                              //手术助手I工号                      
                    baxx.SSYSGH = dtBASY.Rows[i]["SSYSGH"].ToString();                                                //手术医生工号                       
                    baxx.MZYSXM = dtBASY.Rows[i]["MZYSXM"].ToString();                                                //麻醉医生姓名                       
                    baxx.MZFFMC = dtBASY.Rows[i]["MZFFMC"].ToString();                                                //麻醉方法名称                       
                    baxx.SSYSXM = dtBASY.Rows[i]["SSYSXM"].ToString();                                                //手术医生姓名                       
                    baxx.MZFFDM = dtBASY.Rows[i]["MZFFDM"].ToString();                                                //麻醉方法代码                       
                    baxx.SSCZDM = dtBASY.Rows[i]["SSCZDM"].ToString();                                                //手术/操作代码                      
                    baxx.SSCZMC = dtBASY.Rows[i]["SSCZMC"].ToString();                                                //手术/操作名称                      
                    OutObject.BINGANSYMX.Add(baxx);
                }
            }
            else {
                throw new Exception("未找到该病人的病案信息！");
            }

        }
    }
}
