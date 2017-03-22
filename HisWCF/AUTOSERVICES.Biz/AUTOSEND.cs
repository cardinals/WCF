using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWSoft.Framework;
using System.Configuration;
using System.Xml;

namespace AUTOSERVICES.Biz
{
    public class AUTOSEND
    {
        public static void send()
        {
            //是否启用定时发送
            var csxx = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.AUTO00013," and csmc = 'SZXX_DSFS' "));
            if (csxx == null)
            {
                return;
            }
            else
            {
                if (csxx.Items["CSZ2"] != "1")
                {
                    return;
                }
            }

            #region 发送审核数据
            sendSHSJ();
            #endregion

            #region 发送待入院数据
            sendDRYSJ();
            #endregion

            #region 发送入院数据
            sendRYSJ();
            #endregion

            #region 发送离院数据
            sendLYSJ();
            #endregion

            #region 发送查房通知数据
            sendCFTZ();
            #endregion

            #region 发送审核未不通数据
            sendSHBTG();
            #endregion

            #region 发送下转申请数据
            sendXZSQSJ();
            #endregion

            #region 发送检查项目分类
            sendJCFL();
            #endregion

            #region 发送检查项目
            sendJCXM();
            #endregion

            #region 发送检查方向
            sendJCFX();
            #endregion

            #region 发送检查部位
            sendJCBW();
            #endregion
        }

        //病人状态0或空接收，1待住院，2审核，3住院，4离院，5审核不通过
        public static void sendSHSJ()
        {
            var Brxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00001," and brzt in (1,2,3,4) and shfszt = 0 "));
            if (Brxx == null || Brxx.Count == 0)
            {
            }
            else
            {
                foreach (var mxxx in Brxx)
                {
                    
                    var str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<request action= \"sendInHospitalConfirm\" client=\"市二:192.168.1.10\" >" +
                    "<head><patient><mpiId></mpiId>" +
                    "<personName>" + mxxx.Get("brxm") + "</personName>" +
                    "<sexCode>" + mxxx.Get("brxb") + "</sexCode>" +
                    "<birthday>" + mxxx.Get("brcsrq") + "</birthday>" +
                    "<idCard>" + mxxx.Get("sfzh") + "</idCard>" +
                    "<idType>01</idType>" +
                    "<cardNo>" + mxxx.Get("jzkh") + "</cardNo>" +
                    "<cardType>" + mxxx.Get("jzklx") + "</cardType>" +
                    "</patient></head>" +
                    "<body>" +
                    "<hospitalCode>47011662233010511A1001</hospitalCode>"+
                    "<confirmResult>1</confirmResult>"+
                    "<refuseReason></refuseReason>"+
                    "<wardAreaCode>" + mxxx.Get("bqdm") + "</wardAreaCode>"+
                    "<wardArea>" + mxxx.Get("bqmc") + "</wardArea>" +
                    "<departmentCode>" + mxxx.Get("ksdm") + "</departmentCode>" +
                    "<department>" + mxxx.Get("ksmc") + "</department>" +
                    "<bedNo>" + mxxx.Get("curr_bed") + "</bedNo>" +
                    "<doctorName>" + mxxx.Get("zzys") + "</doctorName>" +
                    "<doctorPhone>" + mxxx.Get("zzyslxdh") + "</doctorPhone>" +
                    "<submitor>自动转发</submitor>" +
                    "<submitTime>" + System.DateTime.Now.ToString() + "</submitTime>" +
                    "<submitAgency>杭州市二医院</submitAgency></body></request>";

                    var rtnstr = sendJK(str);
                    var xmldom = new XmlDocument();
                    var xmlOut = "";
                    xmlOut = rtnstr;
                    xmldom.LoadXml(xmlOut);
                    var errno = xmldom.GetElementsByTagName("code").Item(0).FirstChild.InnerText;
                    if (errno == "200")
                    {
                        var tran = DBVisitor.Connection.BeginTransaction();
                        try
                        {
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.AUTO00007, mxxx.Get("zzsqdh")), tran);
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                        }
                    }
                }
            }
        }

        public static void sendDRYSJ()
        {
            var Brxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00001," and brzt in (1,3,4) and dryfszt = 0 "));
            if (Brxx == null || Brxx.Count == 0)
            {
            }
            else
            {
                foreach (var mxxx in Brxx)
                {
                    var str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<request action= \"sendInHospitalAcceptFlag\" client=\"市二:192.168.1.10\" >" +
                    "<head><patient><mpiId></mpiId>" +
                    "<personName>" + mxxx.Get("brxm") + "</personName>" +
                    "<sexCode>" + mxxx.Get("brxb") + "</sexCode>" +
                    "<birthday>" + mxxx.Get("brcsrq") + "</birthday>" +
                    "<idCard>" + mxxx.Get("sfzh") + "</idCard>" +
                    "<idType>01</idType>" +
                    "<cardNo>" + mxxx.Get("jzkh") + "</cardNo>" +
                    "<cardType>" + mxxx.Get("jzklx") + "</cardType>" +
                    "</patient></head>" +
                    "<body>" +
                    "<reserveNo>" + mxxx.Get("zzsqdh") + "</reserveNo>" +
                    "<departmentNo>" + mxxx.Get("ksdm") + "</departmentNo>" +
                    "<department>" + mxxx.Get("ksmc") + "</department>" +
                    "</body></request>";

                    var rtnstr = sendJK(str);
                    var xmldom = new XmlDocument();
                    var xmlOut = "";
                    xmlOut = rtnstr;
                    xmldom.LoadXml(xmlOut);
                    var errno = xmldom.GetElementsByTagName("code").Item(0).FirstChild.InnerText;
                    if (errno == "200")
                    {
                        var tran = DBVisitor.Connection.BeginTransaction();
                        try
                        {
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.AUTO00007, mxxx.Get("zzsqdh")), tran);
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                        }
                    }
                }
            }
        }

        public static void sendRYSJ()
        {
            var Brxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00001, " and brzt in (3,4) and ryfszt = 0 "));
            if (Brxx == null || Brxx.Count == 0)
            {
            }
            else
            {
                foreach (var mxxx in Brxx)
                {

                    var str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<request action= \"sendInHospitalFlag\" client=\"市二:192.168.1.10\" >" +
                    "<head><patient><mpiId></mpiId>" +
                    "<personName>" + mxxx.Get("brxm") + "</personName>" +
                    "<sexCode>" + mxxx.Get("brxb") + "</sexCode>" +
                    "<birthday>" + mxxx.Get("brcsrq") + "</birthday>" +
                    "<idCard>" + mxxx.Get("sfzh") + "</idCard>" +
                    "<idType>01</idType>" +
                    "<cardNo>" + mxxx.Get("jzkh") + "</cardNo>" +
                    "<cardType>" + mxxx.Get("jzklx") + "</cardType>" +
                    "</patient></head>" +
                    "<body>" +
                    "<hospitalCode>47011662233010511A1001</hospitalCode>" +
                    "<wardAreaCode>" + mxxx.Get("bqdm") + "</wardAreaCode>" +
                    "<wardArea>" + mxxx.Get("bqmc") + "</wardArea>" +
                    "<departmentCode>" + mxxx.Get("ksdm") + "</departmentCode>" +
                    "<department>" + mxxx.Get("ksmc") + "</department>" +
                    "<bedNo>" + mxxx.Get("curr_bed") + "</bedNo>" +
                    "<inHospitalTime>" + mxxx.Get("admiss_date") + "</inHospitalTime>" +
                    "<inHospitalReason></inHospitalReason>" +
                    "<doctorName>" + mxxx.Get("zzys") + "</doctorName>" +
                    "<doctorPhone>" + mxxx.Get("zzyslxdh") + "</doctorPhone>" +
                    "<reserveNo>" + mxxx.Get("zzsqdh") + "</reserveNo>" +
                    "<submitor>自动转发</submitor>" +
                    "<submitTime>" + System.DateTime.Now.ToString() + "</submitTime>" +
                    "<submitAgency>杭州市二医院</submitAgency></body></request>";

                    var rtnstr = sendJK(str);
                    var xmldom = new XmlDocument();
                    var xmlOut = "";
                    xmlOut = rtnstr;
                    xmldom.LoadXml(xmlOut);
                    var errno = xmldom.GetElementsByTagName("code").Item(0).FirstChild.InnerText;
                    if (errno == "200")
                    {
                        var tran = DBVisitor.Connection.BeginTransaction();
                        try
                        {
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.AUTO00007, mxxx.Get("zzsqdh")), tran);
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                        }
                    }
                }
            }
        }

        public static void sendLYSJ()
        {
            var Brxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00001, " and brzt in (4) and lyfszt = 0 "));
            if (Brxx == null || Brxx.Count == 0)
            {
            }
            else
            {
                foreach (var mxxx in Brxx)
                {
                    var str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<request action= \"sendLeaveHospitalReport\" client=\"市二:192.168.1.10\" >" +
                    "<head><patient><mpiId></mpiId>" +
                    "<personName>" + mxxx.Get("brxm") + "</personName>" +
                    "<sexCode>" + mxxx.Get("brxb") + "</sexCode>" +
                    "<birthday>" + mxxx.Get("brcsrq") + "</birthday>" +
                    "<idCard>" + mxxx.Get("sfzh") + "</idCard>" +
                    "<idType>01</idType>" +
                    "<cardNo>" + mxxx.Get("jzkh") + "</cardNo>" +
                    "<cardType>" + mxxx.Get("jzklx") + "</cardType>" +
                    "</patient></head>" +
                    "<body>" +
                    "<yybm>47011662233010511A1001</yybm>" +
                    "<zzdh>" + mxxx.Get("zzsqdh") + "</zzdh>" +
                    "<lylx>" + mxxx.Get("ywlx") + "</lylx>" +
                    "<zljg>" + mxxx.Get("cyzt") + "</zljg>" +
                    "<cyxj> </cyxj>" +
                    "<lyrq>" + mxxx.Get("out_date") + "</lyrq>" +
                    "<zzys>" + mxxx.Get("zzys") + "</zzys>" +
                    "<tjys>自动转发</tjys>" +
                    "<tjsj>" + System.DateTime.Now.ToString() + "</tjsj>" +
                    "<tjjg>杭州市二医院</tjjg>";
                    var cfxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00014, mxxx.Get("jzxh")));
                    if (cfxx == null || cfxx.Count == 0)
                    {
                    }
                    else
                    {
                        foreach (var item in cfxx)
                        {
                            str += "<chufangxx>" +
                            "<cfid>" + item.Get("cfid") + "</cfid>" +
                            "<brxm>" + mxxx.Get("brxm") + "</brxm>" +
                            "<cflx>" + item.Get("cflx") + "</cflx>" +
                            "<cfks>" + item.Get("cfks") + "</cfks>" +
                            "<cfys>" + item.Get("cfys") + "</cfys>" +
                            "<cfrq>" + item.Get("cfrq") + "</cfrq>" +
                            "<bzxx>" + item.Get("bzxx") + "</bzxx>";
                            var cfmxxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00015, item.Get("cfid")));
                            if (cfmxxx == null || cfmxxx.Count == 0)
                            {
                            }
                            else
                            {
                                foreach (var itemmx in cfmxxx)
                                {
                                    str += "<chufangxxxx>" +
                                    "<cfid>" + item.Get("cfid") + "</cfid>" +
                                    "<yplx>" + itemmx.Get("yplx") + "</yplx>" +
                                    "<ypmc>" + itemmx.Get("ypmc") + "</ypmc>" +
                                    "<ypgg>" + itemmx.Get("ypgg") + "</ypgg>" +
                                    "<ycjl>" + itemmx.Get("ycjl") + "</ycjl>" +
                                    "<jldw>" + itemmx.Get("jldw") + "</jldw>" +
                                    "<yyts>" + itemmx.Get("yyts") + "</yyts>" +
                                    "<yypl>" + itemmx.Get("yypl") + "</yypl>" +
                                    "<ypcd>" + itemmx.Get("ypcd") + "</ypcd>" +
                                    "<yysl>" + itemmx.Get("ypsl") + "</yysl>" +
                                    "<ypyf>" + itemmx.Get("ypyf") + "</ypyf>" +
                                    "<kssj>" + itemmx.Get("kssj") + "</kssj>" +
                                    "<tzsj>" + itemmx.Get("tzsj") + "</tzsj>" +
                                    "<psjg>" + itemmx.Get("psjg") + "</psjg>" +
                                    "<zyts>" + itemmx.Get("zyts") + "</zyts>" +
                                    "<fyrq>" + itemmx.Get("fyrq") + "</fyrq>" +
                                    "</chufangxxxx>";
                                }
                            }
                            str += "</chufangxx>";
                        }
                    }
                    var jyxx = DBVisitor1.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00016, mxxx.Get("jzxh")));
                    if (jyxx == null || jyxx.Count == 0)
                    {
                    }
                    else
                    {
                        foreach (var item in jyxx)
                        {
                            str += "<jianyanxx>"+
                            "<jydh>" + item.Get("jydh") + "</jydh>" +
                            "<jylb>" + item.Get("jylb") + "</jylb>" +
                            "<jyxm>" + item.Get("jyxm") + "</jyxm>" +
                            "<kdrq>" + item.Get("kdrq") + "</kdrq>" +
                            "<kdys>" + item.Get("kdys") + "</kdys>" +
                            "<kdks>" + item.Get("kdks") + "</kdks>" +
                            "<jyff>" + item.Get("jyff") + "</jyff>" +
                            "<jyjg>" + item.Get("jyjg") + "</jyjg>" +
                            "<bzxx>" + item.Get("bzxx") + "</bzxx>";
                            var jymxxx = DBVisitor1.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00017, item.Get("jydh")));
                            if (jymxxx == null || jymxxx.Count == 0)
                            {
                            }
                            else
                            {
                                foreach (var itemmx in jymxxx)
                                {
                                    str += "<jianyanmxxx>" +
                                    "<jyxm>" + item.Get("jyxm") + "</jyxm>" +
                                    "<jyjgdx>" + item.Get("jyjgdx") + "</jyjgdx>" +
                                    "<jydh>" + item.Get("jydh") + "</jydh>" +
                                    "<jyjgsz>" + item.Get("jyjg") + "</jyjgsz>" +
                                    "<jldw>" + item.Get("jldw") + "</jldw>" +
                                    "<ckgz>" + item.Get("ckgz") + "</ckgz>" +
                                    "<ckdz>" + item.Get("ckdz") + "</ckdz>" +
                                    "<jyzb>" + item.Get("jyzb") + "</jyzb>" +
                                    "<bzxx>" + item.Get("bzxx") + "</bzxx>" +
                                    "</jianyanmxxx>";
                                }
                            }
                            str += "</jianyanxx>";
                        }
                    }
                    var jcxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(mxxx.Get("ywlx") == "1" ? SQ.AUTO00018 : SQ.AUTO00020, mxxx.Get("jzxh")));
                    if (jcxx == null || jcxx.Count == 0)
                    {
                    }
                    else
                    {
                        foreach (var item in jcxx)
                        {
                            str += "<jianchaxx>" +
                            "<jcdh>" + item.Get("jcdh") + "</jcdh>" +
                            "<yxbh>" + item.Get("yxbh") + "</yxbh>" +
                            "<jclb>" + item.Get("jclb") + "</jclb>" +
                            "<jcxm>" + item.Get("jcxm") + "</jcxm>" +
                            "<jcbw>" + item.Get("jcbw") + "</jcbw>" +
                            "<jcsqd>" + item.Get("jcsqd") + "</jcsqd>" +
                            "<jcbg>" + item.Get("jcbg") + "</jcbg>" +
                            "<yxxx>" + item.Get("yxxx") + "</yxxx>" +
                            "<jcjg>" + item.Get("jcjg") + "</jcjg>" +
                            "<bzxx>" + item.Get("bzxx") + "</bzxx>" +
                            "</jianchaxx>";
                        }
                    }
                    str += "</body></request>";

                    var rtnstr = sendJK(str);
                    var xmldom = new XmlDocument();
                    var xmlOut = "";
                    xmlOut = rtnstr;
                    xmldom.LoadXml(xmlOut);
                    var errno = xmldom.GetElementsByTagName("code").Item(0).FirstChild.InnerText;
                    if (errno == "200")
                    {
                        var tran = DBVisitor.Connection.BeginTransaction();
                        try
                        {
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.AUTO00007, mxxx.Get("zzsqdh")), tran);
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                        }
                    }
                }
            }
        }

        public static void sendCFTZ()
        {
            var Brxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00001, " and cftz in = 1 and cftzfszt = 0 "));
            if (Brxx == null || Brxx.Count == 0)
            {
            }
            else
            {
                foreach (var mxxx in Brxx)
                {
                    var str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<request action= \"sendInhospitalVisitNotice\" client=\"市二:192.168.1.10\" >" +
                    "<head><patient><mpiId></mpiId>" +
                    "<personName>" + mxxx.Get("brxm") + "</personName>" +
                    "<sexCode>" + mxxx.Get("brxb") + "</sexCode>" +
                    "<birthday>" + mxxx.Get("brcsrq") + "</birthday>" +
                    "<idCard>" + mxxx.Get("sfzh") + "</idCard>" +
                    "<idType>01</idType>" +
                    "<cardNo>" + mxxx.Get("jzkh") + "</cardNo>" +
                    "<cardType>" + mxxx.Get("jzklx") + "</cardType>" +
                    "</patient></head>" +
                    "<body>" +
                    "<reserveNo>" + mxxx.Get("zzsqdh") + "</reserveNo>" +
                    "<visitDate>" + mxxx.Get("ywlx") + "</visitDate>" +
                    "<submitor>自动转发</submitor>" +
                    "<submitTime>" + System.DateTime.Now.ToString() + "</submitTime>" +
                    "<submitAgency>杭州市二医院</submitAgency></body></request>";

                    var rtnstr = sendJK(str);
                    var xmldom = new XmlDocument();
                    var xmlOut = "";
                    xmlOut = rtnstr;
                    xmldom.LoadXml(xmlOut);
                    var errno = xmldom.GetElementsByTagName("code").Item(0).FirstChild.InnerText;
                    if (errno == "200")
                    {
                        var tran = DBVisitor.Connection.BeginTransaction();
                        try
                        {
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.AUTO00007, mxxx.Get("zzsqdh")), tran);
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                        }
                    }
                }
            }
        }

        public static void sendSHBTG()
        {
            var Brxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00001, " and brzt in (5) and shwtgfszt = 0 "));
            if (Brxx == null || Brxx.Count == 0)
            {
            }
            else
            {
                foreach (var mxxx in Brxx)
                {
                    var str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<request action= \"sendInHospitalConfirm\" client=\"市二:192.168.1.10\" >" +
                    "<head><patient><mpiId></mpiId>" +
                    "<personName>" + mxxx.Get("brxm") + "</personName>" +
                    "<sexCode>" + mxxx.Get("brxb") + "</sexCode>" +
                    "<birthday>" + mxxx.Get("brcsrq") + "</birthday>" +
                    "<idCard>" + mxxx.Get("sfzh") + "</idCard>" +
                    "<idType>01</idType>" +
                    "<cardNo>" + mxxx.Get("jzkh") + "</cardNo>" +
                    "<cardType>" + mxxx.Get("jzklx") + "</cardType>" +
                    "</patient></head>" +
                    "<body>" +
                    "<hospitalCode>47011662233010511A1001</hospitalCode>" +
                    "<confirmResult>2</confirmResult>" +
                    "<refuseReason></refuseReason>" +
                    "<wardAreaCode>" + mxxx.Get("bqdm") + "</wardAreaCode>" +
                    "<wardArea>" + mxxx.Get("bqmc") + "</wardArea>" +
                    "<departmentCode>" + mxxx.Get("ksdm") + "</departmentCode>" +
                    "<department>" + mxxx.Get("ksmc") + "</department>" +
                    "<bedNo>" + mxxx.Get("curr_bed") + "</bedNo>" +
                    "<doctorName>" + mxxx.Get("zzys") + "</doctorName>" +
                    "<doctorPhone>" + mxxx.Get("zzyslxdh") + "</doctorPhone>" +
                    "<submitor>自动转发</submitor>" +
                    "<submitTime>" + System.DateTime.Now.ToString() + "</submitTime>" +
                    "<submitAgency>杭州市二医院</submitAgency></body></request>";

                    var rtnstr = sendJK(str);
                    var xmldom = new XmlDocument();
                    var xmlOut = "";
                    xmlOut = rtnstr;
                    xmldom.LoadXml(xmlOut);
                    var errno = xmldom.GetElementsByTagName("code").Item(0).FirstChild.InnerText;
                    if (errno == "200")
                    {
                        var tran = DBVisitor.Connection.BeginTransaction();
                        try
                        {
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.AUTO00007, mxxx.Get("zzsqdh")), tran);
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                        }
                    }
                }
            }
        }

        public static void sendXZSQSJ()
        {
            var Brxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00001, " and xzzt = 1 and xzsqfszt = 0 "));
            if (Brxx == null || Brxx.Count == 0)
            {
            }
            else
            {
                foreach (var mxxx in Brxx)
                {
                    var str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<request action= \"sendExchangeReport\" client=\"市二:192.168.1.10\" >" +
                    "<head><patient><mpiId></mpiId>" +
                    "<personName>" + mxxx.Get("brxm") + "</personName>" +
                    "<sexCode>" + mxxx.Get("brxb") + "</sexCode>" +
                    "<birthday>" + mxxx.Get("brcsrq") + "</birthday>" +
                    "<idCard>" + mxxx.Get("sfzh") + "</idCard>" +
                    "<idType>01</idType>" +
                    "<cardNo>" + mxxx.Get("jzkh") + "</cardNo>" +
                    "<cardType>" + mxxx.Get("jzklx") + "</cardType>" +
                    "</patient></head>" +
                    "<body>" +
                    "<yybm>47011662233010511A1001</yybm>" +
                    "<zzdh>" + mxxx.Get("zzsqdh") + "</zzdh>" +
                    "<zzdh>" + mxxx.Get("zzsqdh") + "</zzdh>" +
                    "<zljg>" + mxxx.Get("cyzt") + "</zljg>" +
                    "<xzrq>" + mxxx.Get("xzsqrq") + "</xzrq>" +
                    "<kfnr>" + mxxx.Get("cyzt") + "</kfnr>" +
                    "<zzys>" + mxxx.Get("zzys") + "</zzys>" +
                    "<cyxj>" + mxxx.Get("xzzzyy") + "</cyxj>" +
                    "<tjys>自动转发</tjys>" +
                    "<tjsj>" + System.DateTime.Now.ToString() + "</tjsj>" +
                    "<tjjg>杭州市二医院</tjjg>";
                    var cfxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00014, mxxx.Get("jzxh")));
                    if (cfxx == null || cfxx.Count == 0)
                    {
                    }
                    else
                    {
                        foreach (var item in cfxx)
                        {
                            str += "<chufangxx>" +
                            "<cfid>" + item.Get("cfid") + "</cfid>" +
                            "<brxm>" + mxxx.Get("brxm") + "</brxm>" +
                            "<cflx>" + item.Get("cflx") + "</cflx>" +
                            "<cfks>" + item.Get("cfks") + "</cfks>" +
                            "<cfys>" + item.Get("cfys") + "</cfys>" +
                            "<cfrq>" + item.Get("cfrq") + "</cfrq>" +
                            "<bzxx>" + item.Get("bzxx") + "</bzxx>";
                            var cfmxxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00015, item.Get("cfid")));
                            if (cfmxxx == null || cfmxxx.Count == 0)
                            {
                            }
                            else
                            {
                                foreach (var itemmx in cfmxxx)
                                {
                                    str += "<chufangxxxx>" +
                                    "<cfid>" + item.Get("cfid") + "</cfid>" +
                                    "<yplx>" + itemmx.Get("yplx") + "</yplx>" +
                                    "<ypmc>" + itemmx.Get("ypmc") + "</ypmc>" +
                                    "<ypgg>" + itemmx.Get("ypgg") + "</ypgg>" +
                                    "<ycjl>" + itemmx.Get("ycjl") + "</ycjl>" +
                                    "<jldw>" + itemmx.Get("jldw") + "</jldw>" +
                                    "<yyts>" + itemmx.Get("yyts") + "</yyts>" +
                                    "<yypl>" + itemmx.Get("yypl") + "</yypl>" +
                                    "<ypcd>" + itemmx.Get("ypcd") + "</ypcd>" +
                                    "<yysl>" + itemmx.Get("ypsl") + "</yysl>" +
                                    "<ypyf>" + itemmx.Get("ypyf") + "</ypyf>" +
                                    "<kssj>" + itemmx.Get("kssj") + "</kssj>" +
                                    "<tzsj>" + itemmx.Get("tzsj") + "</tzsj>" +
                                    "<psjg>" + itemmx.Get("psjg") + "</psjg>" +
                                    "<zyts>" + itemmx.Get("zyts") + "</zyts>" +
                                    "<fyrq>" + itemmx.Get("fyrq") + "</fyrq>" +
                                    "</chufangxxxx>";
                                }
                            }
                            str += "</chufangxx>";
                        }
                    }
                    var jyxx = DBVisitor1.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00016, mxxx.Get("jzxh")));
                    if (jyxx == null || jyxx.Count == 0)
                    {
                    }
                    else
                    {
                        foreach (var item in jyxx)
                        {
                            str += "<jianyanxx>"+
                            "<jydh>" + item.Get("jydh") + "</jydh>" +
                            "<jylb>" + item.Get("jylb") + "</jylb>" +
                            "<jyxm>" + item.Get("jyxm") + "</jyxm>" +
                            "<kdrq>" + item.Get("kdrq") + "</kdrq>" +
                            "<kdys>" + item.Get("kdys") + "</kdys>" +
                            "<kdks>" + item.Get("kdks") + "</kdks>" +
                            "<jyff>" + item.Get("jyff") + "</jyff>" +
                            "<jyjg>" + item.Get("jyjg") + "</jyjg>" +
                            "<bzxx>" + item.Get("bzxx") + "</bzxx>";
                            var jymxxx = DBVisitor1.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00017, item.Get("jydh")));
                            if (jymxxx == null || jymxxx.Count == 0)
                            {
                            }
                            else
                            {
                                foreach (var itemmx in jymxxx)
                                {
                                    str += "<jianyanmxxx>" +
                                    "<jyxm>" + item.Get("jyxm") + "</jyxm>" +
                                    "<jyjgdx>" + item.Get("jyjgdx") + "</jyjgdx>" +
                                    "<jydh>" + item.Get("jydh") + "</jydh>" +
                                    "<jyjgsz>" + item.Get("jyjg") + "</jyjgsz>" +
                                    "<jldw>" + item.Get("jldw") + "</jldw>" +
                                    "<ckgz>" + item.Get("ckgz") + "</ckgz>" +
                                    "<ckdz>" + item.Get("ckdz") + "</ckdz>" +
                                    "<jyzb>" + item.Get("jyzb") + "</jyzb>" +
                                    "<bzxx>" + item.Get("bzxx") + "</bzxx>" +
                                    "</jianyanmxx>";
                                }
                            }
                            str += "</jianyanxx>";
                        }
                    }
                    var jcxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00020, mxxx.Get("jzxh")));
                    if (jcxx == null || jcxx.Count == 0)
                    {
                    }
                    else
                    {
                        foreach (var item in jcxx)
                        {
                            str += "<jianchaxx>" +
                            "<jcdh>" + item.Get("jcdh") + "</jcdh>" +
                            "<yxbh>" + item.Get("yxbh") + "</yxbh>" +
                            "<jclb>" + item.Get("jclb") + "</jclb>" +
                            "<jcxm>" + item.Get("jcxm") + "</jcxm>" +
                            "<jcbw>" + item.Get("jcbw") + "</jcbw>" +
                            "<jcsqd>" + item.Get("jcsqd") + "</jcsqd>" +
                            "<jcbg>" + item.Get("jcbg") + "</jcbg>" +
                            "<yxxx>" + item.Get("yxxx") + "</yxxx>" +
                            "<jcjg>" + item.Get("jcjg") + "</jcjg>" +
                            "<bzxx>" + item.Get("bzxx") + "</bzxx>" +
                            "</jianchaxx>";
                        }
                    }
                    var zyyzxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00019, mxxx.Get("jzxh")));
                    if (zyyzxx == null || zyyzxx.Count == 0)
                    {
                    }
                    else
                    {
                        str += "<zyyzjl>";
                        foreach (var item in zyyzxx)
                        {
                            str += "<zyyzmx>" +
                            "<yzxm>" + item.Get("yzxh") + "</yzxm>" +
                            "<zyh>" + mxxx.Get("bah") + "</zyh>" +
                            "<ypmc>" + item.Get("ypmc") + "</ypmc>" +
                            "<ypcd>" + item.Get("ypcd") + "</ypcd>" +
                            "<yfgg>" + item.Get("yfgg") + "</yfgg>" +
                            "<yfdw>" + item.Get("yfdw") + "</yfdw>" +
                            "<kssj>" + item.Get("kssj") + "</kssj>" +
                            "<ycsl>" + item.Get("ycsl") + "</ycsl>" +
                            "<ytcs>" + item.Get("ytcs") + "</ytcs>" +
                            "<fysl>" + item.Get("fysl") + "</fysl>" +
                            "<sypc>" + item.Get("sypc") + "</sypc>" +
                            "<yzlx>" + item.Get("yzlx") + "</yzlx>" +
                            "<yyts>" + item.Get("yyts") + "</yyts>" +
                            "</zyyzmx>";
                        }
                        str += "</zyyzjl>";
                    }
                    str += "</body></request>";

                    var rtnstr = sendJK(str);
                    var xmldom = new XmlDocument();
                    var xmlOut = "";
                    xmlOut = rtnstr;
                    xmldom.LoadXml(xmlOut);
                    var errno = xmldom.GetElementsByTagName("code").Item(0).FirstChild.InnerText;
                    if (errno == "200")
                    {
                        var tran = DBVisitor.Connection.BeginTransaction();
                        try
                        {
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.AUTO00007, mxxx.Get("zzsqdh")), tran);
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                        }
                    }
                }
            }
        }

        public static void sendJCFL()
        {
            var JCFLtable = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00009));

            if (JCFLtable == null || JCFLtable.Count == 0)
            {
            }
            else
            {
                var xmlin = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xmlin += "<request action=\"sendCheckClassify\" client=\"市二:192.168.1.10\">";
                xmlin += "<head>";
                xmlin += "<patient>";
                xmlin += "<mpiId></mpiId>";
                xmlin += "</patient>";
                xmlin += "</head>";
                xmlin += "<body>";

                foreach (var mxxx in JCFLtable)
                {
                    xmlin += "<item>";
                    xmlin += "<classifyCode>" + mxxx.Get("id") + "</classifyCode>";
                    xmlin += "<classifyName>" + mxxx.Get("ylflmc") + "</classifyName>";
                    xmlin += "<departmentCode>" + mxxx.Get("zxkdm") + "</departmentCode>";
                    xmlin += "<inUseFlag>" + mxxx.Get("del_flag") + "</inUseFlag>";
                    xmlin += "<telephone>" + mxxx.Get("lxdh") + "</telephone>";
                    xmlin += "<departmentLocation>" + mxxx.Get("ksdz") + "</departmentLocation>";
                    xmlin += "<hospitalCode>47011662233010511A1001</hospitalCode>";
                    xmlin += "</item>";
                }
                xmlin += "</body>";
                xmlin += "</request>";
                var rtnstr = sendJK(xmlin);
            }
        }

        public static void sendJCXM()
        {
            var JCXMtable = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00010));
            if (JCXMtable == null || JCXMtable.Count == 0)
            {
            }
            else
            {
                var xmlin = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xmlin += "<request action=\"sendCheckItem\" client=\"市二:192.168.1.10\">";
                xmlin += "<head>";
                xmlin += "<patient>";
                xmlin += "<mpiId></mpiId>";
                xmlin += "</patient>";
                xmlin += "</head>";
                xmlin += "<body>";

                foreach (var mxxx in JCXMtable)
                {
                    xmlin += "<item>";
                    xmlin += "<itemCode>" + mxxx.Get("jcxmdm") + "</itemCode>";
                    xmlin += "<itemName>" + mxxx.Get("jcxmmc") + "</itemName>";
                    xmlin += "<needReserve>1</needReserve>";
                    xmlin += "<classifyCode>" + mxxx.Get("JCXMLX") + "</classifyCode>";
                    xmlin += "<partCode>" + mxxx.Get("JCXMBWDM") + "</partCode>";
                    xmlin += "<directionCode>" + mxxx.Get("JCXMFX") + "</directionCode>";
                    xmlin += "<hospitalCode>47011662233010511A1001</hospitalCode>";
                    xmlin += "<announcements>" + mxxx.Get("JCXMBZ") + "</announcements>";
                    xmlin += "</item>";
                }
                xmlin += "</body>";
                xmlin += "</request>";
                var rtnstr = sendJK(xmlin);
            }
        }

        public static void sendJCFX()
        {
            var JCFXtable = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00011));
            if (JCFXtable == null || JCFXtable.Count == 0)
            {
            }
            else
            {
                var xmlin = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xmlin += "<request action=\"sendCheckDirection\" client=\"市二:192.168.1.10\">";
                xmlin += "<head>";
                xmlin += "<patient>";
                xmlin += "<mpiId></mpiId>";
                xmlin += "</patient>";
                xmlin += "</head>";
                xmlin += "<body>";

                foreach (var mxxx in JCFXtable)
                {
                    xmlin += "<item>";
                    xmlin += "<directionCode>" + mxxx.Get("id") + "</directionCode>";
                    xmlin += "<directionParentCode>" + mxxx.Get("IDF") + "</directionParentCode>";
                    xmlin += "<directionName>" + mxxx.Get("fxmc") + "</directionName>";
                    xmlin += "<inUseFlag>" + mxxx.Get("del_flag") + "</inUseFlag>";
                    xmlin += "<frequency>" + mxxx.Get("xmcs") + "</frequency>";
                    xmlin += "<hospitalCode>47011662233010511A1001</hospitalCode>";
                    xmlin += "</item>";
                }
                xmlin += "</body>";
                xmlin += "</request>";
                var rtnstr = sendJK(xmlin);
            }
        }

        public static void sendJCBW()
        {
            var JCBWtable = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.AUTO00012));
            if (JCBWtable == null || JCBWtable.Count == 0)
            {
            }
            else
            {
                var xmlin = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xmlin += "<request action=\"sendCheckPart\" client=\"市二:192.168.1.10\">";
                xmlin += "<head>";
                xmlin += "<patient>";
                xmlin += "<mpiId></mpiId>";
                xmlin += "</patient>";
                xmlin += "</head>";
                xmlin += "<body>";

                foreach (var mxxx in JCBWtable)
                {
                    xmlin += "<item>";
                    xmlin += "<partCode>" + mxxx.Get("id") + "</partCode>";//检查部位代码
                    xmlin += "<partName>" + mxxx.Get("bwmc") + "</partName>";//检查部位名称
                    xmlin += "<inUseFlag>" + mxxx.Get("del_flag") + "</inUseFlag>";
                    xmlin += "<classifyCode>" + mxxx.Get("flbm") + "</classifyCode>";//检查类型代码
                    xmlin += "<departmentCode>" + mxxx.Get("zxksdm") + "</departmentCode>";//执行科室代码
                    xmlin += "<hospitalCode>47011662233010511A1001</hospitalCode>";
                    xmlin += "</item>";
                }
                xmlin += "</body>";
                xmlin += "</request>";
                var rtnstr = sendJK(xmlin);
            }
        }

        /// <summary>
        /// 发送接口数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string sendJK(string request)
        {
            var rtnstr = "";
            using (var channelFactory = new DR.drServicePortTypeClient())
            {
                rtnstr = channelFactory.execute(request);
            }
            return rtnstr;
        }
    }
}
