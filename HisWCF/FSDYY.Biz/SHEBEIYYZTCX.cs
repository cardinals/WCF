using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using Common.WSEntity;
using Common.WSCall;

namespace FSDYY.Biz
{
    public class SHEBEIYYZTCX : IMessage<SHEBEIYYZTCX_IN, SHEBEIYYZTCX_OUT>
    {
        public override void ProcessMessage()
        {
            var sqlsb = "";
            OutObject = new SHEBEIYYZTCX_OUT();
            OutObject.SHEBEIYYXXXX = new List<SHEBEIYYXX>();
            var xmlIn = "";

            //var ldweb = new LaiDaWebService.WebServiceSoapClient();
            //var xmlOut = ldweb.HISYY_GetResource(xmlIn);
            if (string.Compare(InObject.YUYUERQ.ToString(), DateTime.Now.ToString("yyyy-MM-dd")) < 0)
            {
                throw new Exception(string.Format("预约日期必须大于等于今天！"));
            }
            if (InObject.JIANCHAXMDM == null || InObject.JIANCHAXMDM == "")
            {
                throw new Exception(string.Format("预约项目不能为空！"));
            }

            if (System.Configuration.ConfigurationManager.AppSettings["JianChaJKMS"] == "1")
            {
                var resource = new HISYY_GetResource();
                //var result = new HISYY_GetResource_Result();

                resource.HospitalCode = InObject.BASEINFO.JIGOUDM;
                resource.HospitalName = "杭州市第一人民医院";
                var codes = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.FSD00027, InObject.JIANCHAXMDM));
                foreach (var code in codes)
                {
                    resource.BespeakExamine.Add(new BespeakExamine()
                    {
                        ExamineCode = code["LBXH"].ToString(),
                        ExamineName = Unity.encodeString(code["LBMC"].ToString())
                    });
                }
                resource.BespeakDate = InObject.YUYUERQ.Replace("-", string.Empty);
                if (InObject.YEWULX == "2")
                    resource.AdmissionSource = "10";
                else
                    resource.AdmissionSource = "50";

                codes = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.FSD00027, InObject.JIANCHAXMDM));
                resource.StudiesDepartMentCode = codes[0]["JCKS"].ToString();
                resource.StudiesDepartMentName = codes[0]["KSMC"].ToString();

                resource.IsJZ = InObject.JIZHEN;
                resource.IsZQ = InObject.ZENGQIANG;
                resource.IsLS = InObject.LINSHI;

                ///调用WEBSERVICE
                string url = System.Configuration.ConfigurationManager.AppSettings["LAIDAURL"];
                string xml = XMLHandle.EntitytoXML<HISYY_GetResource>(resource);
                HISYY_GetResource_Result result = XMLHandle.XMLtoEntity<HISYY_GetResource_Result>(WSServer.Call<HISYY_GetResource>(url, xml).ToString());

                if (result.Success == "False")
                {
                    throw new Exception("取号源信息失败,错误原因：" + result.Message);
                }
                string[] separators = { ",", " " };
                foreach (var time in result.BespeakDatePart.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    var pbxx = new SHEBEIYYXX();
                    pbxx.JIANCHASBDM = result.DeviceCode;
                    pbxx.JIANCHASBMC = result.DeviceName;
                    pbxx.JIANCHASBDD = result.DeviceLocation;
                    pbxx.YUYUERQ = InObject.YUYUERQ;
                    pbxx.YUYUEKSSJ = time.Split('-')[0];
                    pbxx.YUYUEJSSJ = time.Split('-')[1];
                    pbxx.JIANCHAYYLX = 1;
                    pbxx.XIANGMUHS = Convert.ToInt16(result.ExaminePartTime);
                    OutObject.SHEBEIYYXXXX.Add(pbxx);
                }
            }
            else
            {

                #region 市2模式
                InObject.JIANCHAXMDM = InObject.JIANCHAXMDM.ToString().Replace("，", ",");
                if (InObject.JIANCHAXMDM.IndexOf(',') >= 0)
                {
                    sqlsb += " having( count(distinct a.jcxmdm) >= " + InObject.JIANCHAXMDM.Split(',').Length.ToString() + ") group by a.jcsbdm ";
                }
                //取得对应设备
                var listdysb = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.FSD00001, InObject.JIANCHAXMDM, sqlsb));

                if (listdysb.Count == 0)
                {
                    throw new Exception(string.Format("找不到项目对应的检查设备信息:项目代码[{0}]", InObject.JIANCHAXMDM));
                }
                else
                {
                    foreach (var item in listdysb)
                    {
                        var jcsbdm = item.Get("jcsbdm");
                        var sypb = 0;
                        //取得设备当前日期和时间的排班信息--
                        //若查询类型为空或0，则查询当前日期当前时间之后的所有排班信息
                        if (InObject.CHAXUNLX == null || InObject.CHAXUNLX == "0")
                        {
                            sypb = 1;
                        }
                        var listsbpbxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.FSD00002, jcsbdm, InObject.YUYUERQ, InObject.YUYUESJ, sypb));
                        if (listsbpbxx.Count == 0)
                        {
                            //throw new Exception(string.Format("未找到排班信息!"));
                        }
                        else
                        {
                            foreach (var item_sb in listsbpbxx)
                            {
                                var pbxx = new SHEBEIYYXX();
                                pbxx.JIANCHASBDM = item_sb.Get("jcsbdm");
                                pbxx.JIANCHASBMC = item_sb.Get("jcsbmc");
                                pbxx.JIANCHASBDD = item_sb.Get("jcsbdd");
                                pbxx.YUYUERQ = item_sb.Get("pbrq");
                                pbxx.YUYUEKSSJ = item_sb.Get("kssj");
                                pbxx.YUYUEJSSJ = item_sb.Get("jssj");
                                pbxx.YUYUEJCBW = item_sb.Get("yyjcbw");
                                pbxx.JIANCHAYYLX = Convert.ToInt16(item_sb.Get("jcyylx"));
                                int xcyy = 0;//现场预约值为2,检索所有数据

                                if (InObject.YEWULY == "3")
                                {
                                    pbxx.YUYUEHZS = int.Parse(item_sb.Get("sqkyys"));
                                    pbxx.YIYUYUES = int.Parse(item_sb.Get("sqyyys"));
                                }
                                else if (InObject.YEWULY == "2")
                                {
                                    pbxx.YUYUEHZS = int.Parse(item_sb.Get("zykyys"));
                                    pbxx.YIYUYUES = int.Parse(item_sb.Get("zyyyys"));
                                }
                                else if (InObject.YEWULY == "1")
                                {
                                    pbxx.YUYUEHZS = int.Parse(item_sb.Get("mzkyys"));
                                    pbxx.YIYUYUES = int.Parse(item_sb.Get("mzyyys"));
                                }
                                else
                                {
                                    if (item_sb.Get("pbrq") == DateTime.Now.ToString("yyyy-MM-dd"))
                                    {
                                        pbxx.YUYUEHZS = int.Parse(item_sb.Get("yyzs"));
                                        xcyy = 2;
                                    }
                                    else
                                    {
                                        pbxx.YUYUEHZS = int.Parse(item_sb.Get("kyys"));
                                    }
                                    pbxx.YIYUYUES = int.Parse(item_sb.Get("yyys"));
                                }
                                //当天预约，查询所有预约号
                                //非当天预约，查询可预约号（去除预留号）
                                if (pbxx.YUYUEHZS > pbxx.YIYUYUES)
                                {
                                    var listyyhxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.FSD00003, item_sb.Get("yyhxx"), xcyy));
                                    var yyhxx_list = new List<YUYUEHXX>();

                                    foreach (var item_yyh in listyyhxx)
                                    {
                                        var yyhxx = new YUYUEHXX();
                                        yyhxx.YUYUEH = item_yyh.Get("yyh");
                                        yyhxx.YUYUEZT = int.Parse(item_yyh.Get("yyzt"));
                                        yyhxx_list.Add(yyhxx);
                                    }
                                    pbxx.YUYUEHXXXX = yyhxx_list;
                                    //OutObject.SHEBEIYYXXXX.Add(pbxx);
                                }
                                else
                                {
                                    var yyhxx_list = new List<YUYUEHXX>();
                                    pbxx.YUYUEHXXXX = yyhxx_list;
                                }
                                OutObject.SHEBEIYYXXXX.Add(pbxx);
                            }
                        }

                    }
                }
                #endregion
            }
        }
    }
}
