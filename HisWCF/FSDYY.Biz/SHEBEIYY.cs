using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;
using Common.WSCall;
using Common.WSEntity;
//using FSDYY.Biz.WSEntity;
//using FSDYY.Biz.WSCall;

namespace FSDYY.Biz
{
    public class SHEBEIYY : IMessage<SHEBEIYY_IN, SHEBEIYY_OUT>
    {
        public override void ProcessMessage()
        {
            if (InObject.YUYUERQ == null)
            {
                throw new Exception(string.Format("预约日期不能为空！"));
            }
            if (InObject.YUYUESJ == null)
            {
                throw new Exception(string.Format("预约时间不能为空！"));
            }
            //if (InObject.JIANCHAXMDM == null || InObject.JIANCHAXMDM == "")
            //{
            //    throw new Exception(string.Format("检查项目不能为空！"));
            //}
            if (InObject.BINGRENLX == null || InObject.BINGRENLX < 1 || InObject.BINGRENLX > 3)
            {
                throw new Exception(string.Format("病人类型代码不对！"));
            }
            if (InObject.YEWULY == null)
            {
                InObject.YEWULY = InObject.BINGRENLX.ToString();
            }
            var jcsbdm = InObject.JIANCHASBDM.ToString();
            var yyrq = InObject.YUYUERQ.ToString();
            var yysj = InObject.YUYUESJ.ToString();
            var yyh = "";

            //不能预约以前的日期
            if (string.Compare(yyrq, DateTime.Now.ToString("yyyy-MM-dd")) < 0)
            {
                throw new Exception(string.Format("预约日期必须大于等于今天！"));
            }

            if (System.Configuration.ConfigurationManager.AppSettings["JianChaJKMS"] == "1")
            {
                HISYY_Submit resource = new HISYY_Submit();
                var yysqdbh = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00018)).Items["YYSQDBH"].ToString();
                //检查
                if (InObject.YEWULY == "1")
                {
                    #region 调用包，判断入参是否正确
                    var jyjcdxx = "";
                    var jyjcmx = "";
                    var zdmx = "";
                    var jcxmdm = "";
                    int i = 0;
                    if (InObject.JIANCHALB.Count < 1)
                    {
                        throw new Exception(string.Format("检查项目不能为空！"));
                    }
                    #region 检查单信息
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
                    #endregion
                    #region 检验检查明细
                    foreach (var item in InObject.JIANCHALB)
                    {
                        if (i == 0)
                            jcxmdm = item.JIANCHAXMBH;
                        else
                            jcxmdm += "," + item.JIANCHAXMBH;
                        ++i;
                        jyjcmx += item.JIANCHAXMBH + "|";//	检查项目编号
                        jyjcmx += item.JIANCHAXMMC + "|";//	检查项目名称
                        jyjcmx += item.JIANCHAFLBM + "|";//	检查分类编码
                        jyjcmx += item.JIANCHASTBW + "|";//	检查身体部位
                        jyjcmx += item.JIANCHAFXDM + "|";//	检查方向代码
                        jyjcmx += item.JIANCHAZYDM + "|";//	检查肢位代码
                        jyjcmx += item.JIANCHATS + "^";//	检查提示

                    }
                    #endregion
                    #region 疾病明细
                    foreach (var item in InObject.ZHENDUANLB)
                    {
                        zdmx += item.ICD10 + "|";//	ICD10
                        zdmx += item.ZHENDUANMC + "^";//	诊断名称
                    }
                    #endregion
                    #region 调用包
                    var jianchakd = SqlLoad.GetProcedure(SQ.P_FSD00030);
                    jianchakd["ywlx"] = 1;
                    jianchakd["jyjcdxx"] = jyjcdxx;
                    jianchakd["jyjcmx"] = jyjcmx;
                    jianchakd["zdmx"] = zdmx;
                    jianchakd["jylx"] = 1;
                    jianchakd["errno"] = -1;
                    jianchakd["errmsg"] = string.Empty.PadRight(1024);
                    jianchakd["outdata"] = string.Empty.PadRight(1024);
                    DBVisitor.ExecuteProcedure(jianchakd);

                    if (jianchakd["errno"].ToString() != "0")
                    {
                        throw new Exception("更新库存失败:" + jianchakd["errmsg"]);
                    }
                    #endregion
                    #endregion

                    if (InObject.YEWULX == "2")
                        resource.AdmissionSource = "10";
                    else
                        resource.AdmissionSource = "50";
                    resource.HospitalCode = InObject.BASEINFO.JIGOUDM;
                    resource.HospitalName = "余杭三院";
                    resource.PatientName = InObject.BINGRENXM;
                    resource.IdNumber = InObject.SHENFENZH;
                    resource.RequestNo = yysqdbh;
                    if (InObject.YEWULY == "2")
                    {
                        resource.AdmissionID = InObject.BINGRENZYH;
                    }
                    else
                    {
                        resource.AdmissionID = InObject.BINGRENMZH;
                    }
                    resource.ExaminePartTime = InObject.XIANGMUHS;
                    resource.PatientSex = InObject.BINGRENXB.ToString();
                    resource.PatientBorn = InObject.BINGRENCSRQ;
                    resource.PatientAge = InObject.BINGRENNL;
                    resource.PatientTel = InObject.BINGRENLXDH;
                    resource.PatientAddress = InObject.BINGRENLXDZ;
                    resource.PatientCard = InObject.BINGRENKH;
                    resource.InPatientAreaName = InObject.BINGRENBQMC;
                    resource.InPatientAreaCode = InObject.BINGRENBQDM;
                    resource.BedNum = InObject.BINGRENCWH;
                    resource.DeviceCode = InObject.JIANCHASBDM.ToString();
                    resource.DeviceName = InObject.JIANCHASBMC;
                    var codes = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.FSD00027, jcxmdm));
                    foreach (var code in codes)
                    {
                        resource.StudiesExamine.Add(new StudiesExamine()
                        {
                            ExamineCode = code["LBXH"].ToString(),
                            ExamineName = Unity.encodeString(code["LBMC"].ToString()),
                            Numbers = "1",
                            ExaminePrice = DBVisitor.ExecuteScalar(SqlLoad.GetFormat(SQ.FSD00029, code["LBXH"].ToString())).ToString()
                        });
                    }
                    resource.ExamineFY = resource.StudiesExamine.Sum<StudiesExamine>(group => { return Convert.ToDecimal(group.ExaminePrice); }).ToString();
                    resource.ReceiptNum = InObject.BINGRENFPH;
                    codes = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.FSD00027, jcxmdm));
                    resource.ZxDepartmentId = codes[0]["JCKS"].ToString();
                    resource.ZxDepartmentName = codes[0]["KSMC"].ToString();
                    resource.Sqrq = InObject.BASEINFO.CAOZUORQ;
                    resource.BespeakDateTime = InObject.YUYUERQ + " " + InObject.YUYUESJ;
                    resource.JZ = "0";// InObject.JIZHEN;
                    resource.ZQ = "0";//InObject.ZENGQIANG;
                    resource.LS = "0";//InObject.LINSHI;
                    resource.PF = "1";

                    ///调用WEBSERVICE
                    string url = System.Configuration.ConfigurationManager.AppSettings["LAIDAURL"];
                    string xml = XMLHandle.EntitytoXML<HISYY_Submit>(resource);
                    HISYY_Submit_Result result = XMLHandle.XMLtoEntity<HISYY_Submit_Result>(WSServer.Call<HISYY_Submit>(url, xml).ToString());

                    if (result.Success == "False")
                    {
                        throw new Exception("预约失败,错误原因：" + result.Message);
                    }

                    var tran = DBVisitor.Connection.BeginTransaction();
                    try
                    {

                        #region 先插入到临时表
                        var ID = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00024, " seq_sxzz_jianchad.nextval "));
                        var xh = "";
                        var JCSQDH = result.JCH;
                        int k = 0;
                        if (ID == null)
                        {
                            throw new Exception(string.Format("检查单序列获取失败！"));
                        }
                        else
                        {
                            xh = ID.Items["MAXID"].ToString();
                        }
                        //var SQDH = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00023, " SEQ_GY_YXSQD_SQDH.NEXTVAL "));
                        //if (SQDH == null)
                        //{
                        //    throw new Exception(string.Format("申请单号获取失败！"));
                        //}
                        //else
                        //{
                        //    JCSQDH = SQDH.Items["MAXID"].ToString();
                        //}
                        //插入检查单信息
                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00022,
                                    xh, "", InObject.BINGRENMZH, InObject.BINGRENXM,
                                    InObject.SHENFENZH, InObject.SHENQINGYSGH, InObject.JIANCHAKSDM, 0,
                                    InObject.BINGQINGMS, InObject.ZHENDUAN, InObject.BINGRENTZ, InObject.QITAJC,
                                    InObject.BINGRENZS, InObject.YEWULX, 0, JCSQDH, InObject.YUYUERQ,
                                    InObject.BINGRENXB, InObject.BINGRENNL), tran);
                        //插入检查明细
                        foreach (var item in InObject.JIANCHALB)
                        {
                            ++k;
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00023,
                                xh, k, item.JIANCHAXMBH,
                                item.JIANCHAXMMC, item.JIANCHAFLBM, item.JIANCHASTBW,
                                item.JIANCHAFXDM, item.JIANCHAZYDM, item.JIANCHATS), tran);
                        }
                        k = 0;
                        //插入诊断明细
                        foreach (var item in InObject.ZHENDUANLB)
                        {
                            ++k;
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00025,
                                xh, k, item.ICD10, item.ZHENDUANMC), tran);
                        }


                        #endregion

                        var listyylsh = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00017));
                        var yysqlsh = listyylsh.Items["YYSQLSH"].ToString();

                        //插入预约申请信息fdsyy_sq
                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00011,
                           "申请单",//预约申请单名称
                           InObject.YUYUEZT,//预约申请单状态(0未确认,1已确认,9作废)
                           InObject.JIANCHAKSDM,//检查科室代码
                           InObject.JIANCHAKSMC,//检查科室名称
                           InObject.BINGRENFPH,//病人发票号
                           InObject.BINGRENLX,//病人类型
                           InObject.BINGRENLXMC,//病人类型名称
                           InObject.BINGRENKH,//病人卡号
                           InObject.BINGRENMZH,//病人门诊号
                           InObject.BINGRENZYH,//病人住院号
                           InObject.BINGRENBQDM,//病人病区代码
                           InObject.BINGRENBQMC,//病人病区名称
                           InObject.BINGRENCWH,//病人床位号
                           InObject.BINGRENXM,//病人姓名
                           InObject.BINGRENXB,//病人性别
                           InObject.BINGRENNL,//病人年龄
                           InObject.BINGRENCSRQ,//病人出生日期
                           InObject.BINGRENLXDZ,//病人联系地址
                           InObject.BINGRENLXDH,//病人联系电话
                           InObject.SHENQINGYSGH,//申请医生工号
                           InObject.SHENQINGYSMC,//申请医生姓名
                           "",//申请科室代码
                           "",//申请科室名称
                           InObject.SHENQINGYYDM,//申请医院代码
                           InObject.SHENQINGYYMC,//申请医院名称
                           result.JCH,//检查号
                           InObject.YUYUERQ,//检查日期
                           InObject.YUYUESJ,//检查时间

                           jcxmdm,//InObject.JIANCHAXMDM,//检查项目代码
                           InObject.JIANCHAXMMC,//检查项目名称

                           InObject.JIANCHAXMLX,//检查项目类型
                           InObject.JIANCHABWDM,//检查部位代码
                           InObject.JIANCHABWMC,//检查部位名称
                           InObject.JIANCHASBDM,//检查设备代码
                           InObject.JIANCHASBMC,//检查设备名称
                           InObject.JIANCHASBDD,//检查设备地点
                           result.YYH,//预约号
                           InObject.SHENFENZH,//身份证号
                           InObject.YUYUESF,//预约收费(0未收费，1已收费)
                           InObject.JIANCHASQDBH,//检查申请单编号
                           InObject.YINGXIANGFX,
                           "",//影像方向
                           yysqlsh,//预约申请流水号
                           yysqdbh,//预约申请单编号
                           InObject.XIANGMUHS,//详细安排时间
                           InObject.YEWULY,
                           InObject.YEWULX,
                           InObject.ZENGQIANG,
                           InObject.JIZHEN,
                           InObject.LINSHI
                        ), tran);//业务来源
                        tran.Commit();

                        OutObject = new SHEBEIYY_OUT();
                        OutObject.YUYUERQ = InObject.YUYUERQ;
                        OutObject.YUYUESJ = InObject.YUYUESJ;
                        OutObject.YUYUEH = result.YYH;
                        OutObject.JIANCHAH = result.JCH;
                        OutObject.YUYUESQDBH = yysqdbh;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
                //检验
                else
                {

                }

            }
            else
            {
                #region 市二
                if (InObject.JIANCHAXMDM == null || InObject.JIANCHAXMDM == "")
                {
                    throw new Exception(string.Format("检查项目不能为空！"));
                }
                var listjcsb = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00007, jcsbdm));
                if (listjcsb == null)
                {
                    throw new Exception(string.Format("未找到预约设备:设备编号[{0}]", jcsbdm));
                }
                if (listjcsb.Items.Count == 0)//判断有无该设备
                {
                    throw new Exception(string.Format("未找到预约设备:设备编号[{0}]", jcsbdm));
                }
                else
                {

                    if (listjcsb.Items["JCSBZT"].ToString() != "0")//设备是否可用
                    {
                        throw new Exception(string.Format("预约设备故障或者已停用:设备编号[{0}]", jcsbdm));
                    }
                    else
                    {
                        if (listjcsb.Items["SBYYBZ"].ToString() != "0")//设备是否可预约
                        {
                            throw new Exception(string.Format("预约设备为不可预约状态:设备编号[{0}]", jcsbdm));
                        }
                        else
                        {
                            //获取设备某天预约排班信息
                            var listsbyyxx = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00002, jcsbdm, yyrq, yysj, 0));
                            if (listsbyyxx == null)
                            {
                                throw new Exception(string.Format("未找到预约号信息!"));
                            }
                            //取得预约类型，再去取预约号及状态
                            var jcyylx = int.Parse(listsbyyxx.Items["JCYYLX"].ToString());
                            var yyhxx = int.Parse(listsbyyxx.Items["YYHXX"].ToString());
                            var yyzs = int.Parse(listsbyyxx.Items["YYZS"].ToString());
                            var kyys = int.Parse(listsbyyxx.Items["KYYS"].ToString());
                            var xcyls = int.Parse(listsbyyxx.Items["XCYLS"].ToString());
                            var yyys = int.Parse(listsbyyxx.Items["YYYS"].ToString());
                            int xcyy = 0;//现场预约值为2,则检索所有数据
                            var zykyys = int.Parse(listsbyyxx.Items["ZYKYYS"].ToString());
                            var zyyyys = int.Parse(listsbyyxx.Items["ZYYYYS"].ToString());
                            var mzkyys = int.Parse(listsbyyxx.Items["MZKYYS"].ToString());
                            var mzyyys = int.Parse(listsbyyxx.Items["MZYYYS"].ToString());
                            var sqkyys = int.Parse(listsbyyxx.Items["SQKYYS"].ToString());
                            var sqyyys = int.Parse(listsbyyxx.Items["SQYYYS"].ToString());
                            var tran = DBVisitor.Connection.BeginTransaction();
                            var yysjd = listsbyyxx.Items["KSSJ"].ToString() + "-" + listsbyyxx.Items["JSSJ"].ToString();
                            if (string.Compare(listsbyyxx.Items["PBRQ"].ToString(), DateTime.Now.ToString("yyyy-MM-dd")) == 0)
                            {
                                xcyy = 2;
                            }
                            if (xcyy == 0)//非现场预约，则判断门诊住院预约的比例值
                            {
                                if (InObject.BINGRENLX == 2)
                                {
                                    if (InObject.YEWULY == "3")
                                    {
                                        if (sqkyys <= sqyyys)
                                        {
                                            throw new Exception(string.Format("当前社区可预约数已预约完，不能再预约!"));
                                        }
                                        ++sqyyys;
                                    }
                                    else
                                    {
                                        if (zykyys <= zyyyys)
                                        {
                                            throw new Exception(string.Format("当前住院可预约数已预约完，不能再预约!"));
                                        }
                                        ++zyyyys;
                                    }
                                }
                                else
                                {
                                    if (InObject.YEWULY == "3")
                                    {
                                        if (sqkyys <= sqyyys)
                                        {
                                            throw new Exception(string.Format("当前社区可预约数已预约完，不能再预约!"));
                                        }
                                        ++sqyyys;
                                    }
                                    else
                                    {
                                        if (mzkyys <= mzyyys)
                                        {
                                            throw new Exception(string.Format("当前门诊可预约数已预约完，不能再预约!"));
                                        }
                                        ++mzyyys;
                                    }
                                }
                            }
                            if (jcyylx == 1)//有预约号模式
                            {
                                if (InObject.YUYUEH == null || InObject.YUYUEH == "")
                                {
                                    throw new Exception(string.Format("预约号不能为空！"));
                                }
                                yyh = InObject.YUYUEH.ToString();
                                var listsbyyhxx = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00010, yyhxx, yyh));
                                if (listsbyyhxx == null)
                                {
                                    throw new Exception(string.Format("未找到预约号：预约号：[{0}]!", yyh));
                                }
                                if (listsbyyhxx.Items["YYZT"].ToString() == "1")
                                {
                                    throw new Exception(string.Format("该预约号已被预约：预约号：[{0}]!", yyh));
                                }
                                else
                                {
                                    try
                                    {
                                        if (InObject.YEWULY == "3")
                                        {
                                            //更新预约排班表 社区
                                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00021, yyhxx, yyys + 1, sqyyys), tran);
                                        }
                                        else if (InObject.BINGRENLX == 2)
                                        {
                                            //更新预约排班表 住院
                                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00008, yyhxx, yyys + 1, zyyyys), tran);
                                        }
                                        else
                                        {
                                            //更新预约排班表 门诊
                                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00020, yyhxx, yyys + 1, mzyyys), tran);
                                        }
                                        //更新预约排班表
                                        //DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00008, yyhxx, yyys + 1, zyyyys), tran);
                                        //更新预约号状态
                                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00009, yyhxx, yyh, 1), tran);
                                        //tran.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        tran.Rollback();
                                        throw ex;
                                    }
                                }
                            }
                            else//无预约号模式
                            {

                                try
                                {
                                    //取得当前预约信息未预约的最小号码
                                    var listsbyyhxx = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00013, yyhxx, xcyy));
                                    if (listsbyyhxx == null)
                                    {
                                        throw new Exception(string.Format("预约已满！"));
                                    }
                                    yyh = listsbyyhxx.Items["YYH"].ToString();
                                    if (InObject.YEWULY == "3")
                                    {
                                        //更新预约排班表 社区
                                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00021, yyhxx, yyys + 1, sqyyys), tran);
                                    }
                                    else if (InObject.BINGRENLX == 2)
                                    {
                                        //更新预约排班表 住院
                                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00008, yyhxx, yyys + 1, zyyyys), tran);
                                    }
                                    else
                                    {
                                        //更新预约排班表 门诊
                                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00020, yyhxx, yyys + 1, mzyyys), tran);
                                    }
                                    //更新预约排班表
                                    //DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00008, yyhxx, yyys + 1, zyyyys), tran);
                                    //更新预约号状态
                                    DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00009, yyhxx, yyh, 1), tran);
                                    //tran.Commit();
                                }
                                catch (Exception ex)
                                {
                                    tran.Rollback();
                                    throw ex;
                                }
                            }
                            try
                            {
                                var listyylsh = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00017));
                                var yysqlsh = listyylsh.Items["YYSQLSH"].ToString();
                                var listyysqdbh = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00018));
                                var yysqdbh = listyysqdbh.Items["YYSQDBH"].ToString();
                                //插入预约申请信息fdsyy_sq
                                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00011,
                                   "申请单",//预约申请单名称
                                   InObject.YUYUEZT,//预约申请单状态(0未确认,1已确认,9作废)
                                   InObject.JIANCHAKSDM,//检查科室代码
                                   InObject.JIANCHAKSMC,//检查科室名称
                                   InObject.BINGRENFPH,//病人发票号
                                   InObject.BINGRENLX,//病人类型
                                   InObject.BINGRENLXMC,//病人类型名称
                                   InObject.BINGRENKH,//病人卡号
                                   InObject.BINGRENMZH,//病人门诊号
                                   InObject.BINGRENZYH,//病人住院号
                                   InObject.BINGRENBQDM,//病人病区代码
                                   InObject.BINGRENBQMC,//病人病区名称
                                   InObject.BINGRENCWH,//病人床位号
                                   InObject.BINGRENXM,//病人姓名
                                   InObject.BINGRENXB,//病人性别
                                   InObject.BINGRENNL,//病人年龄
                                   InObject.BINGRENCSRQ,//病人出生日期
                                   InObject.BINGRENLXDZ,//病人联系地址
                                   InObject.BINGRENLXDH,//病人联系电话
                                   InObject.SHENQINGYSGH,//申请医生工号
                                   InObject.SHENQINGYSMC,//申请医生姓名
                                   "",//申请科室代码
                                   "",//申请科室名称
                                   InObject.SHENQINGYYDM,//申请医院代码
                                   InObject.SHENQINGYYMC,//申请医院名称
                                   "",//检查号
                                   InObject.YUYUERQ,//检查日期
                                   InObject.YUYUESJ,//检查时间
                                   InObject.JIANCHAXMDM,//检查项目代码
                                   InObject.JIANCHAXMMC,//检查项目名称
                                   InObject.JIANCHAXMLX,//检查项目类型
                                   InObject.JIANCHABWDM,//检查部位代码
                                   InObject.JIANCHABWMC,//检查部位名称
                                   InObject.JIANCHASBDM,//检查设备代码
                                   InObject.JIANCHASBMC,//检查设备名称
                                   InObject.JIANCHASBDD,//检查设备地点
                                   yyh,//预约号
                                   InObject.SHENFENZH,//身份证号
                                   InObject.YUYUESF,//预约收费(0未收费，1已收费)
                                   InObject.JIANCHASQDBH,//检查申请单编号
                                   InObject.YINGXIANGFX,
                                   yysjd,//影像方向
                                   yysqlsh,//预约申请流水号
                                   yysqdbh,//预约申请单编号
                                   InObject.XIANGXIAPSJ,//详细安排时间
                                   InObject.YEWULY,
                                   1,
                                   0,
                                   0,
                                   0), tran);//业务来源
                                tran.Commit();
                                OutObject = new SHEBEIYY_OUT();
                                OutObject.YUYUERQ = yyrq;
                                OutObject.YUYUESJ = yysj;
                                OutObject.YUYUEH = yyh;
                                OutObject.YUYUESQDBH = yysqdbh;
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw ex;
                            }
                        }

                    }
                }
                #endregion
            }
        }
    }
}
