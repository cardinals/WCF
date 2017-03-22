using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class BINGANSY_IN : MessageIn
    {
        /// <summary>
        /// 住院病人ID
        /// </summary>
        public string ZHUYUANBRID { get; set; }


    }

    public class BINGANSY_OUT : MessageOUT
    {
        public  List<BINGANXX> BINGANSYMX { get; set; }

        public BINGANSY_OUT() {
            this.BINGANSYMX = new List<BINGANXX>();
        }
    }

    public class BINGANXX {
        /// <summary>
        /// 机构代码 
        /// </summary>
        public string JGDM { get; set; }
        /// <summary>
        /// 就诊流水号 
        /// </summary>
        public string JYLSH { get; set; }
        /// <summary>
        /// 卡号 
        /// </summary>
        public string KH { get; set; }
        /// <summary>
        /// 卡类型 
        /// </summary>
        public string KLX { get; set; }
        /// <summary>
        /// 医疗付费方式 
        /// </summary>
        public string MEDICALPAYMENT { get; set; }
        /// <summary>
        /// 居民健康卡号 
        /// </summary>
        public string HEALTHCARDID { get; set; }
        /// <summary>
        /// 住院号 
        /// </summary>
        public string HOSPIZATIONID { get; set; }
        /// <summary>
        /// 病案号 
        /// </summary>
        public string MEDICALRECORDID { get; set; }
        /// <summary>
        /// 姓名 
        /// </summary>
        public string XM { get; set; }
        /// <summary>
        /// 性别 
        /// </summary>
        public string SEX { get; set; }
        /// <summary>
        /// 年龄 
        /// </summary>
        public string NL { get; set; }
        /// <summary>
        /// 婚姻状况代码 
        /// </summary>
        public string HYZKDM { get; set; }
        /// <summary>
        /// 职业类别代码 
        /// </summary>
        public string ZYLBDM { get; set; }
        /// <summary>
        /// 国籍 
        /// </summary>
        public string GJ { get; set; }
        /// <summary>
        /// 出生日期时间 
        /// </summary>
        public string CSRQSJ { get; set; }
        /// <summary>
        /// 月龄 
        /// </summary>
        public string MONTHSAGE { get; set; }
        /// <summary>
        /// 新生儿出生体重 
        /// </summary>
        public string NEONATALBIRTHWEIGHT { get; set; }
        /// <summary>
        /// 新生儿入院体重 
        /// </summary>
        public string NEONATALADMISSIONWEIGHT { get; set; }
        /// <summary>
        /// 出生地-省（区、市） 
        /// </summary>
        public string PROVINCE { get; set; }
        /// <summary>
        /// 出生地-市 
        /// </summary>
        public string CITY { get; set; }
        /// <summary>
        /// 出生地-县 
        /// </summary>
        public string COUNTY { get; set; }
        /// <summary>
        /// 籍贯-省（区、市） 
        /// </summary>
        public string ORIGINPROVINCE { get; set; }
        /// <summary>
        /// 籍贯-市 
        /// </summary>
        public string ORIGINCITY { get; set; }
        /// <summary>
        /// 现住址-省（区、市） 
        /// </summary>
        public string PRESENTADDRPROVINCE { get; set; }
        /// <summary>
        /// 现住址-市 
        /// </summary>
        public string PRESENTADDRCITY { get; set; }
        /// <summary>
        /// 现住址-县 
        /// </summary>
        public string PRESENTADDRCOUNTY { get; set; }
        /// <summary>
        /// 现住址-其它 
        /// </summary>
        public string PRESENTADDROTHER { get; set; }
        /// <summary>
        /// 现住址-邮编 
        /// </summary>
        public string PRESENTADDRPOSTALCODE { get; set; }
        /// <summary>
        /// 患者电话号码 
        /// </summary>
        public string PATIENTPHONE { get; set; }
        /// <summary>
        /// 户口地址-省（区、市） 
        /// </summary>
        public string REGISTEREDPERMANENTRESIDENCEPR { get; set; }
        /// <summary>
        /// 户口地址-市 
        /// </summary>
        public string REGISTEREDPERMANENTRESIDENCECI { get; set; }
        /// <summary>
        /// 户口地址-县 
        /// </summary>
        public string REGISTEREDPERMANENTRESIDENCETO { get; set; }
        /// <summary>
        /// 户口地址-其它 
        /// </summary>
        public string REGISTEREDPERMANENTRESIDENCEOT { get; set; }
        /// <summary>
        /// 户口地址-邮编 
        /// </summary>
        public string REGISTEREDPERMANENTRESIDENCEPO { get; set; }
        /// <summary>
        /// 工作单位电话号码 
        /// </summary>
        public string WORKADDRPHONE { get; set; }
        /// <summary>
        /// 入院途径 
        /// </summary>
        public string ADMISSIONWAY { get; set; }
        /// <summary>
        /// 入院日期时间 
        /// </summary>
        public string ADMISSIONDATETIME { get; set; }
        /// <summary>
        /// 入院科室名称 
        /// </summary>
        public string ADMISSIONDEPTNAME { get; set; }
        /// <summary>
        /// 入院病房 
        /// </summary>
        public string ADMISSIONSICKROOM { get; set; }
        /// <summary>
        /// 转科科室名称 
        /// </summary>
        public string TRANSFERREDDEPTNAME { get; set; }
        /// <summary>
        /// 出院日期时间 
        /// </summary>
        public string DISCHARGEDATETIME { get; set; }
        /// <summary>
        /// 出院科室名称 
        /// </summary>
        public string DISCHARGEDEPARTMENT { get; set; }
        /// <summary>
        /// 出院病房 
        /// </summary>
        public string DISCHARGESICKROOM { get; set; }
        /// <summary>
        /// 实际住院天数 
        /// </summary>
        public string ACTUALHOSPITALIZATIONDAYS { get; set; }
        /// <summary>
        /// 门（急）诊诊断名称 
        /// </summary>
        public string CLINICDIAGNOSIS { get; set; }
        /// <summary>
        /// 门（急）诊诊断（西医诊断）-疾病编码 
        /// </summary>
        public string CLINICWESTERNDIAGNOSISCODE { get; set; }
        /// <summary>
        /// 出院诊断-主要诊断-疾病名称 
        /// </summary>
        public string MASTERDISEASENAME { get; set; }
        /// <summary>
        /// 出院诊断-主要诊断-疾病编码 
        /// </summary>
        public string MASTERDISEASECODE { get; set; }
        /// <summary>
        /// 出院诊断-主要诊断-入院病情 
        /// </summary>
        public string MASTERADMISSIONCONDITION { get; set; }
        /// <summary>
        /// 出院诊断-主要诊断-出院情况 
        /// </summary>
        public string MASTERDISCHARGEPROGNOSISCONDIT { get; set; }
        /// <summary>
        /// 损伤中毒的外部原因 
        /// </summary>
        public string OUTSIDEREASONOFINJURYANDPOISON { get; set; }
        /// <summary>
        /// 损伤中毒的外部原因-疾病编码 
        /// </summary>
        public string OUTSIDEREASONOFINJURYANDPOCODE { get; set; }
        /// <summary>
        /// 病理诊断-疾病名称 
        /// </summary>
        public string PATHOLOGYDISEASENAME { get; set; }
        /// <summary>
        /// 病理诊断-疾病编码 
        /// </summary>
        public string PATHOLOGYDISEASECODE { get; set; }
        /// <summary>
        /// 病理号 
        /// </summary>
        public string PATHOLOGYID { get; set; }
        /// <summary>
        /// 药物过敏 
        /// </summary>
        public string DRUGALLERGY { get; set; }
        /// <summary>
        /// 过敏药物 
        /// </summary>
        public string ALLERGICDRUG { get; set; }
        /// <summary>
        /// 死亡患者尸检标志 
        /// </summary>
        public string AUTOPSYSIGN { get; set; }
        /// <summary>
        /// ABO 血型代码 
        /// </summary>
        public string ABOXX { get; set; }
        /// <summary>
        /// RH血型代码 
        /// </summary>
        public string RHXX { get; set; }
        /// <summary>
        /// 科主任签名 
        /// </summary>
        public string DEPTMANAGER { get; set; }
        /// <summary>
        /// 主任（副主任）医师签名 
        /// </summary>
        public string CHIEFDOCTORSIGN { get; set; }
        /// <summary>
        /// 主治医师签名 
        /// </summary>
        public string ATTENDINGDOCTOR { get; set; }
        /// <summary>
        /// 住院医师签名 
        /// </summary>
        public string HOSPIZATIONDOCTOR { get; set; }
        /// <summary>
        /// 责任护士签名 
        /// </summary>
        public string RESPONSIBILITYNURSE { get; set; }
        /// <summary>
        /// 进修医师签名 
        /// </summary>
        public string REFRESHERDOCTORS { get; set; }
        /// <summary>
        /// 实习医师签名 
        /// </summary>
        public string INTERNDOCTOR { get; set; }
        /// <summary>
        /// 病案编码员签名 
        /// </summary>
        public string MEDICALRECORDCODERSIGN { get; set; }
        /// <summary>
        /// 病案质量 
        /// </summary>
        public string MEDICALRECORDQUALITY { get; set; }
        /// <summary>
        /// 质控医师签名 
        /// </summary>
        public string QUALITYCONTROLDOCTOR { get; set; }
        /// <summary>
        /// 质控护士签名 
        /// </summary>
        public string QUALITYCONTROLSIGN { get; set; }
        /// <summary>
        /// 质控日期 
        /// </summary>
        public string QUALITYCONTROLDATE { get; set; }
        /// <summary>
        /// 离院方式 
        /// </summary>
        public string DISCHARGEMETHODS { get; set; }
        /// <summary>
        /// 拟接受医疗机构名称 
        /// </summary>
        public string PREPAREACCEPTHOSPITALNAME { get; set; }
        /// <summary>
        /// 出院 31 天内再住院标志 
        /// </summary>
        public string DAY31INPATIENTMK { get; set; }
        /// <summary>
        /// 出院 31 天内再住院目的 
        /// </summary>
        public string DAY31INPATIENTAIM { get; set; }
        /// <summary>
        /// 颅脑损伤患者入院前昏迷时间 
        /// </summary>
        public string STUPORTIMEBEFOREADMISSION { get; set; }
        /// <summary>
        /// 颅脑损伤患者入院后昏迷时间 
        /// </summary>
        public string STUPORTIMEAFTERADMISSION { get; set; }
        /// <summary>
        /// 住院总费用（元） 
        /// </summary>
        public string HOSPIZATIONTOTALCOST { get; set; }
        /// <summary>
        /// 住院总费用-自付金额（元） 
        /// </summary>
        public string HOSPIZATIONTOTALPERSONALCOST { get; set; }
        /// <summary>
        /// 诊断符合情况-门诊和出院 
        /// </summary>
        public string DIAGCOINOUTPATIENTVSDISCHARGE { get; set; }
        /// <summary>
        /// 诊断符合情况-入院和出院 
        /// </summary>
        public string DIAGCOINADMSSIONVSDISCHARGE { get; set; }
        /// <summary>
        /// 诊断符合情况-术前和术后 
        /// </summary>
        public string DIAGCOINPREOPERATIVEVSPOST { get; set; }
        /// <summary>
        /// 诊断符合情况-临床和病理 
        /// </summary>
        public string DIAGCOINCLINICALVSPATHOLOGICA { get; set; }
        /// <summary>
        /// 诊断符合情况-放射和病理 
        /// </summary>
        public string DIAGCOINRADIOLOGYVSPATHOLOGY { get; set; }
        /// <summary>
        /// 抢救情况-抢救次数 
        /// </summary>
        public string SALVAGECONDITIONSALVAGETIMES { get; set; }
        /// <summary>
        /// 抢救情况-成功次数 
        /// </summary>
        public string SALVAGECONDITIONSUCCESSTIMES { get; set; }
        /// <summary>
        /// 临床路径管理 
        /// </summary>
        public string CLINICALPATHMANAGEMENT { get; set; }
        /// <summary>
        /// 一般医疗服务费 
        /// </summary>
        public string GENERMEDISERVCHARGE { get; set; }
        /// <summary>
        /// 一般治疗操作费 
        /// </summary>
        public string GENERTREATHANDLINGFEE { get; set; }
        /// <summary>
        /// 护理费 
        /// </summary>
        public string NURSE { get; set; }
        /// <summary>
        /// 综合医疗服务类其他费用 
        /// </summary>
        public string GENERMEDISERVCHARGEOTHER { get; set; }
        /// <summary>
        /// 病理诊断费 
        /// </summary>
        public string PATHOLOGICALFEE { get; set; }
        /// <summary>
        /// 实验室诊断费 
        /// </summary>
        public string LABORATORYFEE { get; set; }
        /// <summary>
        /// 影像学诊断费 
        /// </summary>
        public string IMAGINGFEE { get; set; }
        /// <summary>
        /// 临床诊断项目费 
        /// </summary>
        public string CLINICALDIAGNOSISFEE { get; set; }
        /// <summary>
        /// 非手术治疗项目费 
        /// </summary>
        public string NONOPERATIVETREATFEE { get; set; }
        /// <summary>
        /// 临床物理治疗费 
        /// </summary>
        public string CLINICALPHYSICALTREATMENT { get; set; }
        /// <summary>
        /// 手术治疗费 
        /// </summary>
        public string SURGICALTREATMENT { get; set; }
        /// <summary>
        /// 麻醉费 
        /// </summary>
        public string ESTHETICFEE { get; set; }
        /// <summary>
        /// 手术费 
        /// </summary>
        public string OPERATIONFEE { get; set; }
        /// <summary>
        /// 康复费 
        /// </summary>
        public string REHABILITATIONFEE { get; set; }
        /// <summary>
        /// 中医治疗费 
        /// </summary>
        public string ZYZLFEE { get; set; }
        /// <summary>
        /// 西药费 
        /// </summary>
        public string XYFEE { get; set; }
        /// <summary>
        /// 抗菌药物费用 
        /// </summary>
        public string ANTIBACTERIALDRUGEXP { get; set; }
        /// <summary>
        /// 中成药费 
        /// </summary>
        public string MEDICINECHINA { get; set; }
        /// <summary>
        /// 中草药费 
        /// </summary>
        public string HERBALMEDICINEFEE { get; set; }
        /// <summary>
        /// 血费 
        /// </summary>
        public string BLOODFEE { get; set; }
        /// <summary>
        /// 白蛋白类制品费 
        /// </summary>
        public string ACPFEE { get; set; }
        /// <summary>
        /// 球蛋白类制品费 
        /// </summary>
        public string GCPFEE { get; set; }
        /// <summary>
        /// 凝血因子类制品费 
        /// </summary>
        public string NXYZFEE { get; set; }
        /// <summary>
        /// 细胞因子类制品费 
        /// </summary>
        public string XBYZFEE { get; set; }
        /// <summary>
        /// 检查用一次性医用材料费 
        /// </summary>
        public string YCYYCXFEE { get; set; }
        /// <summary>
        /// 治疗用一次性医用材料费 
        /// </summary>
        public string ZLYYCXFEE { get; set; }
        /// <summary>
        /// 手术用一次性医用材料费 
        /// </summary>
        public string SSYYCXZLFEE { get; set; }
        /// <summary>
        /// 其它费用 
        /// </summary>
        public string OTHERFEE { get; set; }
        /// <summary>
        /// 上传时间 
        /// </summary>
        public string UPLOADDATE { get; set; }
        /// <summary>
        /// 采集标志 
        /// </summary>
        public string UPLOADFLAG { get; set; }
        /// <summary>
        /// 新增时间 
        /// </summary>
        public string CREATEDATE { get; set; }
        /// <summary>
        /// 住院次数 
        /// </summary>
        public string ZYCS { get; set; }
        /// <summary>
        /// 工作单位地址 
        /// </summary>
        public string GZDWDZ { get; set; }
        /// <summary>
        /// 工作单位邮编 
        /// </summary>
        public string DWYB { get; set; }
        /// <summary>
        /// 联系人姓名 
        /// </summary>
        public string LXRXM { get; set; }
        /// <summary>
        /// 联系人关系 
        /// </summary>
        public string LXRGX { get; set; }
        /// <summary>
        /// 联系人地址 
        /// </summary>
        public string LXRDZ { get; set; }
        /// <summary>
        /// 联系人电话 
        /// </summary>
        public string LXRDH { get; set; }
        /// <summary>
        /// 入院颅脑损伤患者昏迷时间几天 
        /// </summary>
        public string RYHZHMSJT { get; set; }
        /// <summary>
        /// 入院颅脑损伤患者昏迷时间几小时 
        /// </summary>
        public string RYHZHMSJXS { get; set; }
        /// <summary>
        /// 入院颅脑损伤患者昏迷时间几分钟 
        /// </summary>
        public string RYHZHMSJFZ { get; set; }
        /// <summary>
        /// 出院颅脑损伤患者昏迷时间几天 
        /// </summary>
        public string CYHZHMSJT { get; set; }
        /// <summary>
        /// 出院颅脑损伤患者昏迷时间几小时 
        /// </summary>
        public string CYHZHMSJXS { get; set; }
        /// <summary>
        /// 出院颅脑损伤患者昏迷时间几分钟 
        /// </summary>
        public string CYHZHMSJFZ { get; set; }
        /// <summary>
        /// 预留一 
        /// </summary>
        public string YL1 { get; set; }
        /// <summary>
        /// 预留二 
        /// </summary>
        public string YL2 { get; set; }
        /// <summary>
        /// 身份证号 
        /// </summary>
        public string SFZH { get; set; }
        /// <summary>
        /// 出院诊断-其他诊断-疾病名称 
        /// </summary>
        public string CYZDQTZDJBMC { get; set; }
        /// <summary>
        /// 出院诊断-其他诊断-疾病编码 
        /// </summary>
        public string CYZDQTZDJBBM { get; set; }
        /// <summary>
        /// 出院诊断-其他诊断-出院情况 
        /// </summary>
        public string CYZDQTZDCYQK { get; set; }
        /// <summary>
        /// 手术记录流水号 
        /// </summary>
        public string SSJLLSH { get; set; }
        /// <summary>
        /// 麻醉医生工号 
        /// </summary>
        public string MZYSGH { get; set; }
        /// <summary>
        /// 手术助手II工号 
        /// </summary>
        public string SSZSIIGH { get; set; }
        /// <summary>
        /// 手术切口愈合等级 
        /// </summary>
        public string SSQKYHDJ { get; set; }
        /// <summary>
        /// 手术/操作日期时间 
        /// </summary>
        public string SSCZRQSJ { get; set; }
        /// <summary>
        /// 手术助手I工号 
        /// </summary>
        public string SSZSIGH { get; set; }
        /// <summary>
        /// 手术医生工号 
        /// </summary>
        public string SSYSGH { get; set; }
        /// <summary>
        /// 麻醉医生姓名 
        /// </summary>
        public string MZYSXM { get; set; }
        /// <summary>
        /// 麻醉方法名称 
        /// </summary>
        public string MZFFMC { get; set; }
        /// <summary>
        /// 手术医生姓名 
        /// </summary>
        public string SSYSXM { get; set; }
        /// <summary>
        /// 麻醉方法代码 
        /// </summary>
        public string MZFFDM { get; set; }
        /// <summary>
        /// 手术/操作代码 
        /// </summary>
        public string SSCZDM { get; set; }
        /// <summary>
        /// 手术/操作名称 
        /// </summary>
        public string SSCZMC { get; set; }
    }
}
