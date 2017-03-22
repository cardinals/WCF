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

namespace HIS4.Biz
{
    public class SHEBEIYYZTCX : IMessage<SHEBEIYYZTCX_IN, SHEBEIYYZTCX_OUT>
    {
        public override void ProcessMessage()
        {
            string jianChaXMDM = InObject.JIANCHAXMDM;  //检查项目代码
            string yuYueRQ = InObject.YUYUERQ;          //预约日期
            string yuYueSJ = InObject.YUYUESJ;          //预约时间
            string chaXunLX = InObject.CHAXUNLX;        //查询类型
            string yeWuLY = InObject.YEWULY;            //业务来源
            string yeWuLX = InObject.YEWULX;            //业务类型
            string jiChuSJLY = InObject.JICHUSJLY;      //基础数据来源

            #region 基础数据判断
            if (string.IsNullOrEmpty(jianChaXMDM)) {
                throw new Exception("检查项目代码不能为空！");
            }

            if (string.IsNullOrEmpty(yuYueRQ))
            {
                throw new Exception("预约日期不能为空！");
            }
            else if (Convert.ToDateTime(yuYueRQ) < DateTime.Now.Date)
            {
                throw new Exception( "预约日期必须大于等于当前日期！");
            }

            if (string.IsNullOrEmpty(chaXunLX)) {
                throw new Exception("查询类型不能为空！");
            }

            if (string.IsNullOrEmpty(yeWuLY)) {
                throw new Exception("业务来源不能为空！");
            }
            if (string.IsNullOrEmpty(yeWuLX))
            {
                throw new Exception("业务类型不能为空！");
            }

            #endregion

            var resource = new HISYY_GetResource();
            resource.HospitalCode = ConfigurationManager.AppSettings["HospitalCode_Fck"].ToString();
            resource.HospitalName = ConfigurationManager.AppSettings["HospitalName_Fck"].ToString();
            //ht     SQL语句更改测试用
            //string jcxmSql = string.Format("select a.*,b.keshimc from GY_JIANCHAXM a,gy_keshi b where b.keshiid=a.zhixingks and a.zuofeibz=0 and a.jianchaxmid in({0})", jianchaxmDm);
            //string jcxmSql = string.Format("select A.*,B.DAIMAID,D.KESHIMC from GY_JIANCHAXM A, GY_JIANCHABW B, GY_JIANCHAXMBWDY C,  gy_keshi D    WHERE A.JIANCHAXMID = C.JIANCHAXMID   AND B.DAIMAID = C.JIANCHABWID  AND D.keshiid=a.zhixingks and d.yuanquid = 3 and a.zuofeibz = 0 and b.ZuoFeiBZ = 0 and b.daimaid in({0})", jianchaxmDm);
            
            string jcxmSql = @"select * from v_jianchaxmbwxx where DAIMAID IN({0}) AND YUANQUMC = '{1}' ";
            jcxmSql = string.Format(jcxmSql, jianChaXMDM, ConfigurationManager.AppSettings["HospitalName_Fck"].ToString());

            DataTable jcxmDt = DBVisitor.ExecuteTable(jcxmSql);

            //ht 没有对应检查项目返回
            if (jcxmDt.Rows.Count < 1)
            {
                throw new Exception("找不到检查项目: " + jianChaXMDM);
            }

            for (int i = 0; i < jcxmDt.Rows.Count; i++)
            {
                resource.BespeakExamine.Add(new BespeakExamine()
                {
                    //HT  更改CODE取值
                    //ExamineCode = jcxmDt.Rows[i]["JIANCHAXMID"].ToString(),
                    ExamineCode = jcxmDt.Rows[i]["DAIMAID"].ToString(),
                    ExamineName = XMLHandle.encodeString(jcxmDt.Rows[i]["DAIMAMC"].ToString())
                });
            }
            resource.BespeakDate = yuYueRQ.Replace("-", string.Empty);
            if (yeWuLX == "2")
                resource.AdmissionSource = "10";
            else
                resource.AdmissionSource = "50";
            resource.StudiesDepartMentCode = jcxmDt.Rows[0]["zhixingks"].ToString();//科室代码
            resource.StudiesDepartMentName = jcxmDt.Rows[0]["keshimc"].ToString();//科室名称
            //resource.IsJZ = jiZhen;
            //resource.IsZQ = zengQiang;
            //resource.IsLS = LinShi;

            //调用莱达WebService------------------------------------------------------------
            string url = System.Configuration.ConfigurationManager.AppSettings["LaiDa_Url"];
            string xml = XMLHandle.EntitytoXML<HISYY_GetResource>(resource);
            string outxml = WSServer.Call<HISYY_GetResource>(url, xml).ToString();//调用莱达webservice
            HISYY_GetResource_Result result = XMLHandle.XMLtoEntity<HISYY_GetResource_Result>(outxml);
            //------------------------------------------------------------------------------
            if (result.Success == "False")
            {
                throw new Exception( "取号源信息失败,错误原因：" + result.Message);
            }
            OutObject = new SHEBEIYYZTCX_OUT();
            string[] separators = { ",", " " };
            foreach (var time in result.BespeakDatePart.Split(separators, StringSplitOptions.RemoveEmptyEntries))
            {
                SHEBEIYYXX temp = new SHEBEIYYXX();
                
                temp.JIANCHASBDM = result.DeviceCode;
                temp.JIANCHASBMC = result.DeviceName;
                temp.JIANCHASBDD = result.DeviceLocation;
                temp.YUYUERQ = yuYueRQ;
                temp.YUYUEKSSJ = time.Split('-')[0];
                temp.YUYUEJSSJ = time.Split('-')[1];
                temp.JIANCHAYYLX = "1";
                temp.XIANGMUHS = result.ExaminePartTime;
                OutObject.SHEBEIYYXXXX.Add(temp);
            }
        }
        
    }

    
}
