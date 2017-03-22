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
    public class SHEBEIYYQX : IMessage<SHEBEIYYQX_IN, SHEBEIYYQX_OUT>
    {
        public override void ProcessMessage()
        {
            //取得预约信息
            OutObject = new SHEBEIYYQX_OUT();
            if (InObject.YUYUESQDBH == null || InObject.YUYUESQDBH == "")
            {
                throw new Exception(string.Format("预约申请单编号为空！"));
            }
            var listyyxx = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00004, InObject.YUYUESQDBH.ToString()));
            if (listyyxx == null)
            {
                OutObject.OUTMSG.ERRNO = "-2";
                OutObject.OUTMSG.ERRMSG = string.Format("找不到预约信息:申请单编号[{0}]", InObject.YUYUESQDBH.ToString());
                return;
                //throw new Exception(string.Format("找不到预约信息:申请单编号[{0}]", InObject.YUYUESQDBH.ToString()));
            }
            if (listyyxx.Items.Count == 0)
            {
                OutObject.OUTMSG.ERRNO = "-2";
                OutObject.OUTMSG.ERRMSG = string.Format("找不到预约信息:申请单编号[{0}]", InObject.YUYUESQDBH.ToString());
                return;
                //throw new Exception(string.Format("找不到预约信息:申请单编号[{0}]", InObject.YUYUESQDBH.ToString()));
            }

            if (System.Configuration.ConfigurationManager.AppSettings["JianChaJKMS"] == "1")
            {
                var resource = new HISYY_Cancel();
                resource.RequestNo = listyyxx.Items["YYH"].ToString();
                resource.YYH = "";
                resource.JCH = "";
                string url = System.Configuration.ConfigurationManager.AppSettings["LAIDAURL"];
                string xml = XMLHandle.EntitytoXML<HISYY_Cancel>(resource);
                HISYY_Cancel_Result result = XMLHandle.XMLtoEntity<HISYY_Cancel_Result>(WSServer.Call<HISYY_GetResource>(url, xml).ToString());
                if (result.Success == "False")
                {
                    throw new Exception("取消预约失败,错误原因：" + result.Message);
                }
                var tran = DBVisitor.Connection.BeginTransaction();
                try
                {
                    DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00005, InObject.YUYUESQDBH.ToString(), 9), tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            else
            {


                var listyyhxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.FSD00014, listyyxx.Items["JCSBDM"].ToString(), listyyxx.Items["JCRQ"].ToString(), listyyxx.Items["JCSJ"].ToString()));
                foreach (var item in listyyhxx)
                {
                    var listyyh = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00010, item.Get("yyhxx").ToString(), listyyxx.Items["YYH"].ToString()));
                    var zyyyys = int.Parse(item.Get("zyyyys"));
                    var mzyyys = int.Parse(item.Get("mzyyys"));
                    var sqyyys = int.Parse(item.Get("sqyyys"));
                    var yyys = int.Parse(item.Get("yyys"));
                    var yyly = listyyxx.Items["YYLY"].ToString();
                    if (yyly == "3")
                    {
                        --sqyyys;
                    }
                    else if (yyly == "2")
                    {
                        --zyyyys;
                    }
                    else if (yyly == "1")
                    {
                        --mzyyys;
                    }
                    else
                    {
                        if (listyyxx.Items["BRLX"].ToString() == "2")
                        {
                            --zyyyys;
                        }
                    }

                    //if (listyyxx.Items["BRLX"].ToString() == "2")
                    //{
                    //    --zyyyys; 
                    //}
                    if (listyyh == null)
                        continue;
                    if (listyyh.Items.Count > 0)
                    {
                        var tran = DBVisitor.Connection.BeginTransaction();
                        try
                        {
                            //更新预约信息状态为取消
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00005, InObject.YUYUESQDBH.ToString(), 9), tran);
                            //更新预约号状态
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00009, item.Get("yyhxx").ToString(), listyyxx.Items["YYH"], 0), tran);
                            //更新预约排班表
                            if (yyly == "3")
                            {
                                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00021, item.Get("yyhxx").ToString(), int.Parse(item.Get("yyys")) - 1, sqyyys), tran);
                            }
                            else if (yyly == "2")
                            {
                                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00008, item.Get("yyhxx").ToString(), int.Parse(item.Get("yyys")) - 1, zyyyys), tran);
                            }
                            else if (yyly == "1")
                            {
                                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00020, item.Get("yyhxx").ToString(), int.Parse(item.Get("yyys")) - 1, mzyyys), tran);
                            }
                            else
                            {
                                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00008, item.Get("yyhxx").ToString(), int.Parse(item.Get("yyys")) - 1, zyyyys), tran);
                            }
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }
    }
}
