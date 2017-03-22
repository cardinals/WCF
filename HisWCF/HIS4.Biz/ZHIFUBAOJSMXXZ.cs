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
using Common.Alipay;
using System.Net;
using System.IO;
using System.Xml;
using HIS4.Schemas.DATAENTITY;


namespace HIS4.Biz
{
    /// <summary>
    /// 支付宝结算明细下载
    /// </summary>
    public class ZHIFUBAOJSMXXZ : IMessage<ZHIFUBAOJSMXXZ_IN, ZHIFUBAOJSMXXZ_OUT>
    {
        public override void ProcessMessage()
        {
            #region
            //本地下载
            if (InObject.XIAZAILX == "1")
            {
                if (string.IsNullOrEmpty(InObject.BASEINFO.FENYUANDM))
                {
                    throw new Exception("分院代码不能为空");
                }
                OutObject = new ZHIFUBAOJSMXXZ_OUT();
                //每页固定查询50条
                int star = 1 + (int.Parse(InObject.PAGENO) - 1) * 50;
                int end = 50 + (int.Parse(InObject.PAGENO) - 1) * 50;
                //分页数据
                var Listxx = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00016, InObject.RIQI, end, star, InObject.BASEINFO.FENYUANDM));
                //数据总量
                var TotalTb = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00017, InObject.RIQI, InObject.BASEINFO.FENYUANDM));
                OutObject.PAGENO = InObject.PAGENO;
                OutObject.PAGESIZE = "50";
                ///判断是否还有数据没下载
                if (TotalTb.Rows.Count <= (int.Parse(InObject.PAGENO) - 1) * 3 + Listxx.Rows.Count)
                {
                    OutObject.HASNEXTPAGE = "F";
                }
                else
                {
                    OutObject.HASNEXTPAGE = "T";
                }
                if (Listxx.Rows.Count > 0)
                {
                    foreach (DataRow dr in Listxx.Rows)
                    {
                        ZHIFUBAOMX zfbdzmx = new ZHIFUBAOMX();
                        zfbdzmx.MERCHANTOUTORDERNO = dr["IOUTTRADENO"].ToString();
                        zfbdzmx.TOTALFEE = dr["ITOTALFEE"].ToString();
                        if (dr["JIESUANZT"].ToString() == "1")
                        {
                            zfbdzmx.TRANSCODEMSG = "收费";
                        }
                        else if (dr["JIESUANZT"].ToString() == "3")
                        {
                            zfbdzmx.TRANSCODEMSG = "退费";
                        }
                        zfbdzmx.TRANSDATE = dr["QINGQIUSJ"].ToString();
                        zfbdzmx.CZYDM = dr["CAOZUOYDM"].ToString();
                        zfbdzmx.CZYXM = dr["CAOZUOYXM"].ToString();

                        zfbdzmx.TRADENO = dr["TRADENO"].ToString();
                        OutObject.ZHIFUBAOXZMX.Add(zfbdzmx);
                    }
                }

            }
            #endregion
            #region 下载支付宝
            //下载支付宝
            else if (InObject.XIAZAILX == "2")
            {
                if (string.IsNullOrEmpty(InObject.BASEINFO.FENYUANDM))
                {
                    throw new Exception("分院代码不能为空");
                }
                DataTable dt = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00006, InObject.BASEINFO.FENYUANDM));
                if (dt.Rows.Count <= 0)
                {
                    throw new Exception("查询不到医院代码为【" + InObject.BASEINFO.FENYUANDM + "】的医院信息");
                }
                //先删除这一天的下载记录
                DBVisitor.ExecuteBool(SqlLoad.GetFormat(SQ.HIS00020, InObject.RIQI));
                //页号
                string page_no = InObject.PAGENO;
                //必填，必须是正整数
                //账务查询开始时间
                string gmt_start_time = DateTime.Parse(InObject.RIQI).ToString("yyyy-MM-dd 00:00:00");// WIDgmt_start_time.Text.Trim();
                //格式为：yyyy-MM-ddHH:mm:ss
                //账务查询结束时间
                string gmt_end_time = DateTime.Parse(InObject.RIQI).ToString("yyyy-MM-dd 23:59:59"); ;
                //格式为：yyyy-MM-ddHH:mm:ss

                ////////////////////////////////////////////////////////////////////////////////////////////////

                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", dt.Rows[0]["PARTNER"].ToString());// Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "account.page.query");
                sParaTemp.Add("page_no", page_no);
                sParaTemp.Add("gmt_start_time", gmt_start_time);
                sParaTemp.Add("gmt_end_time", gmt_end_time);
                sParaTemp.Add("key", dt.Rows[0]["KEY"].ToString());
                //sParaTemp.Add("iw_account_log_id", InObject.IWACCOUNTLOGID);
                //sParaTemp.Add("trade_no", InObject.TRADENO);
                //sParaTemp.Add("merchant_out_order_no", InObject.MERCHANTOUTORDERNO);
                //sParaTemp.Add("deposit_bank_no", InObject.DEPOSITBANKNO);
                //sParaTemp.Add("page_size", InObject.PAGESIZE);

                //建立请求
                string sHtmlText = Submit.BuildRequest(sParaTemp);

                //请在这里加上商户的业务逻辑程序代码

                #region 支付宝错误码字典
                Dictionary<string, string> ErrDic = new Dictionary<string, string>();
                ErrDic.Add("REQUIRED_DATE", "起始和结束时间不能为空");
                ErrDic.Add("ILLEGAL_DATE_FORMAT", "起始和结束时间格式不正确");
                ErrDic.Add("ILLEGAL_DATE_TOO_LONG", "起始和结束时间间隔超过最大间隔");
                ErrDic.Add("START_DATE_AFTER_NOW", "起始时间大于当前时间");
                ErrDic.Add("START_DATE_AFTER_END_DATE", "起始时间大于结束时间");
                ErrDic.Add("ILLEGAL_PAGE_NO", "当前页码必须为数据且必须大于0");
                ErrDic.Add("START_DATE_OUT_OF_RANGE", "查询时间超出范围");
                ErrDic.Add("ILLEGAL_PAGE_SIZE", "分页大小必须为数字且大于0");
                ErrDic.Add("ILLEGAL_ACCOUNT_LOG_ID", "账务流水必须为数字且大于0");
                ErrDic.Add("TOO_MANY_QUERY", "当前查询量太多");
                ErrDic.Add("ACCOUNT_NOT_EXIST", "要查询的用户不存在");
                ErrDic.Add("ACCESS_ACCOUNT_DENIED", "无权查询该账户的账务明细");
                ErrDic.Add("SYSTEM_BUSY", "系统繁忙");
                ErrDic.Add("ILLEGAL_SIGN", "签名不正确");
                ErrDic.Add("ILLEGAL_ARGUMENT", "参数不正确");
                ErrDic.Add("ILLEGAL_SERVICE", "非法服务名称");
                ErrDic.Add("ILLEGAL_USER", "用户ID不正确");
                ErrDic.Add("ILLEGAL_PARTNER", "合作伙伴信息不正确");
                ErrDic.Add("ILLEGAL_EXTERFACE", "接口配置不正确");
                ErrDic.Add("ILLEGAL_PARTNER_EXTERFACE", "合作伙伴接口信息不正确");
                ErrDic.Add("ILLEGAL_SECURITY_PROFILE", "未找到匹配的密钥配置");
                ErrDic.Add("ILLEGAL_SIGN_TYPE", "签名类型不正确");
                ErrDic.Add("ILLEGAL_CHARSET", "字符集不合法");
                ErrDic.Add("ILLEGAL_CLIENT_IP", "客户端IP地址无权访问服务");
                ErrDic.Add("HAS_NO_PRIVILEGE", "未开通此接口权限");
                ErrDic.Add("USER_DATA_MIGRATE_ERROR", "用户的数据未迁移或者迁移状态未完成");
                //系统错误 ，联系支付宝技术支持处理
                ErrDic.Add("SYSTEM_ERROR", "支付宝系统错误");
                ErrDic.Add("SESSION_TIMEOUT", "session超时");
                ErrDic.Add("ILLEGAL_TARGET_SERVICE", "错误的target_service");
                ErrDic.Add("ILLEGAL_ACCESS_SWITCH_SYSTEM", "partner不允许访问该类型的系统");
                ErrDic.Add("ILLEGAL_SWITCH_SYSTEM", "切换系统异常");
                ErrDic.Add("ILLEGAL_ENCODING", "不支持该编码类型");
                ErrDic.Add("EXTERFACE_IS_CLOSED", "接口已关闭");
                #endregion

                #region 分析结算出差
                OutObject = new ZHIFUBAOJSMXXZ_OUT();
                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.LoadXml(sHtmlText);
                    XmlNodeList nodeList = xmlDoc.SelectSingleNode("alipay").ChildNodes;
                    foreach (XmlNode xn in nodeList)
                    {
                        //判断是否异常,如果返回异常 则抛出
                        if (xn.Name == "is_success" && xn.InnerText == "F")
                        {
                            foreach (XmlNode xnerr in nodeList)
                            {
                                if (xnerr.Name == "error")
                                {
                                    foreach (var errmsg in ErrDic)
                                    {
                                        if (errmsg.Key == xnerr.InnerText.ToUpper())
                                            throw new Exception(errmsg.Key + "|" + errmsg.Value);
                                    }
                                }

                            }
                        }


                        if (xn.Name == "response")
                        {
                            XmlElement xe = (XmlElement)xn;
                            XmlNodeList subList = xe.ChildNodes;
                            foreach (XmlNode xmlNode in subList)
                            {
                                if (xmlNode.Name == "account_page_query_result")
                                {
                                    XmlElement xemx = (XmlElement)xmlNode;
                                    XmlNodeList submxList = xemx.ChildNodes;
                                    string text = "";
                                    foreach (XmlNode mxnode in submxList)
                                    {
                                        if (mxnode.Name == "has_next_page")
                                        {
                                            OutObject.HASNEXTPAGE = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "page_no")
                                        {
                                            OutObject.PAGENO = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "page_size")
                                        {
                                            OutObject.PAGESIZE = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "account_log_list")
                                        {
                                            XmlElement mx = (XmlElement)mxnode;
                                            XmlNodeList mxList = mx.ChildNodes;
                                            foreach (XmlNode nodemx in mxList)
                                            {
                                                XmlElement mx1 = (XmlElement)nodemx;
                                                XmlNodeList mxList1 = mx1.ChildNodes;
                                                ZHIFUBAOMX zfbdzmx = new ZHIFUBAOMX();

                                                //明细列表赋值
                                                foreach (XmlNode nodemx1 in mxList1)
                                                {
                                                    //BALANCE	余额
                                                    if (nodemx1.Name == "balance")
                                                        zfbdzmx.BALANCE = nodemx1.InnerText;
                                                    //INCOME	收入金额                                               
                                                    if (nodemx1.Name == "income")
                                                    {
                                                        if (double.Parse(nodemx1.InnerText) != 0)
                                                        {
                                                            // zfbdzmx.INCOME = nodemx1.InnerText;
                                                            zfbdzmx.TOTALFEE = nodemx1.InnerText;
                                                        }
                                                    }
                                                    //OUTCOME	支出金额
                                                    if (nodemx1.Name == "outcome")
                                                    {
                                                        // zfbdzmx.OUTCOME = nodemx1.InnerText;
                                                        if (double.Parse(nodemx1.InnerText) != 0)
                                                        {
                                                            // zfbdzmx.INCOME = nodemx1.InnerText;
                                                            zfbdzmx.TOTALFEE = "-" + nodemx1.InnerText;
                                                        }
                                                    }
                                                    ////BANKACCOUNTNAME	银行帐号名称
                                                    //if (nodemx1.Name == "bank_account_name")
                                                    //    zfbdzmx.BANKACCOUNTNAME = nodemx1.InnerText;
                                                    ////BANKACCOUNTNO	银行帐号
                                                    //if (nodemx1.Name == "bank_account_no")
                                                    //    zfbdzmx.BANKACCOUNTNO = nodemx1.InnerText;
                                                    ////BANKNAME	银行名称
                                                    //if (nodemx1.Name == "bank_name")
                                                    //    zfbdzmx.BANKNAME = nodemx1.InnerText;
                                                    // //BUYERACCOUNT	买家支付宝人民币资金账户
                                                    if (nodemx1.Name == "buyer_account")
                                                        zfbdzmx.BUYERACCOUNT = nodemx1.InnerText;
                                                    ////CURRENCY	货币代码
                                                    //if (nodemx1.Name == "currency")
                                                    //    zfbdzmx.CURRENCY = nodemx1.InnerText;
                                                    ////GOODSTITLE	商品名称
                                                    //if (nodemx1.Name == "goods_title")
                                                    //    zfbdzmx.GOODSTITLE = nodemx1.InnerText;
                                                    ////IWACCOUNTLOGID	财务序列号                                                  
                                                    //if (nodemx1.Name == "iw_account_log_id")
                                                    //    zfbdzmx.IWACCOUNTLOGID = nodemx1.InnerText;
                                                    ////MEMO	备注
                                                    //if (nodemx1.Name == "memo")
                                                    //    zfbdzmx.MEMO = nodemx1.InnerText;
                                                    //MERCHANTOUTORDERNO	商户订单号
                                                    if (nodemx1.Name == "merchant_out_order_no")
                                                        zfbdzmx.MERCHANTOUTORDERNO = nodemx1.InnerText;
                                                    ////OUTHERACCOUNTEMAIL	财务对方邮箱
                                                    //if (nodemx1.Name == "other_account_email")
                                                    //    zfbdzmx.OTHERACCOUNTFULLNAME = nodemx1.InnerText;
                                                    ////OTHERACCOUNTFULLNAME	财务对方名称
                                                    //if (nodemx1.Name == "other_account_fullname")
                                                    //    zfbdzmx.OTHERACCOUNTFULLNAME = nodemx1.InnerText;
                                                    ////OTHERUSERID	财务对方支付宝用户号
                                                    //if (nodemx1.Name == "other_user_id")
                                                    //    zfbdzmx.OTHERUSERID = nodemx1.InnerText;
                                                    //PARTNERID	合作者身份ID
                                                    if (nodemx1.Name == "partner_id")
                                                        zfbdzmx.PARTNERID = nodemx1.InnerText;
                                                    ////SELLERACCOUNT 卖家支付宝人民币资金账户
                                                    //if (nodemx1.Name == "seller_account")
                                                    //    zfbdzmx.SELLERACCOUNT = nodemx1.InnerText;
                                                    //SELLERFULLNAME	卖家姓名
                                                    if (nodemx1.Name == "seller_fullname")
                                                        zfbdzmx.SELLERFULLNAME = nodemx1.InnerText;
                                                    ////SERVICFEE	交易服务费
                                                    //if (nodemx1.Name == "service_fee")
                                                    //    zfbdzmx.SERVICFEE = nodemx1.InnerText;
                                                    ////SERVICEFEERATIO	交易服务费率
                                                    //if (nodemx1.Name == "service_fee_ratio")
                                                    //    zfbdzmx.SERVICEFEERATIO = nodemx1.InnerText;
                                                    //TOTALFEE	交易总金额
                                                    //if (nodemx1.Name == "total_fee")
                                                    //    zfbdzmx.TOTALFEE = nodemx1.InnerText;
                                                    //TRADENO	支付宝交易号
                                                    if (nodemx1.Name == "trade_no")
                                                        zfbdzmx.TRADENO = nodemx1.InnerText;
                                                    ////TRANSACCOUNT	财务本方支付宝人民币资金账户
                                                    //if (nodemx1.Name == "trans_account")
                                                    //    zfbdzmx.TRANSACCOUNT = nodemx1.InnerText;
                                                    //TRANSCODEMSG	业务类型
                                                    if (nodemx1.Name == "trans_code_msg")
                                                        zfbdzmx.TRANSCODEMSG = nodemx1.InnerText;
                                                    //TRANSDATE	交易时间
                                                    if (nodemx1.Name == "trans_date")
                                                        zfbdzmx.TRANSDATE = nodemx1.InnerText;
                                                    ////TRANSOUTERORDERNO	订单号
                                                    //if (nodemx1.Name == "trans_out_order_no")
                                                    //    zfbdzmx.TRANSOUTERORDERNO = nodemx1.InnerText;
                                                    ////DEPOSITBANKNO	网银充值流水号
                                                    //if (nodemx1.Name == "deposit_bank_no")
                                                    //    zfbdzmx.DEPOSITBANKNO = nodemx1.InnerText;
                                                    ////TRADEREFUNDAMOUNT	累积退款金额
                                                    //if (nodemx1.Name == "trade_refund_amount")
                                                    //    zfbdzmx.TRADEREFUNDAMOUNT = nodemx1.InnerText;

                                                }
                                                if (string.IsNullOrEmpty(zfbdzmx.TOTALFEE))
                                                {
                                                    zfbdzmx.TOTALFEE = "0";
                                                }
                                                if (double.Parse(zfbdzmx.TOTALFEE) < 0)
                                                {
                                                    zfbdzmx.MERCHANTOUTORDERNO = "T" + zfbdzmx.MERCHANTOUTORDERNO;
                                                }
                                                //   OutObject.ZHIFUBAOMX.Add(zfbdzmx);
                                                string sfzh = "";
                                                if (string.IsNullOrEmpty(zfbdzmx.BUYERACCOUNT) && zfbdzmx.BUYERACCOUNT.Length > 4)
                                                {
                                                    DataTable dtqyxx = DBVisitor.ExecuteTable(string.Format("select * from gy_bangdinggx where BINRENSB='{0}'", zfbdzmx.BUYERACCOUNT.Substring(0, zfbdzmx.BUYERACCOUNT.Length - 4)));
                                                    if (dtqyxx.Rows.Count > 0)
                                                    {
                                                        sfzh = dtqyxx.Rows[0]["SHENFENZH"].ToString();
                                                    }
                                                }
                                                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00018,
                                                                              zfbdzmx.MERCHANTOUTORDERNO,//IOUTTRADENO	VARCHAR2(50)	N			商户订单号
                                                                              zfbdzmx.TOTALFEE,//ITOTALFEE	VARCHAR2(100)	N			总金额
                                                                              "",                                                    //JIESUANZT	NUMBER(4)	Y			状态  0支付  1退费
                                                                              zfbdzmx.TRANSDATE,//JIESUANSJ	DATE	Y			结算时间
                                                                              zfbdzmx.BUYERACCOUNT,//IBUYERID	VARCHAR2(50)	Y			买家支付宝用户号
                                                                              sfzh,//IBUYEREMAIL	VARCHAR2(50)	Y			买家支付宝帐号
                                                                              "",//IAGREENNO	VARCHAR2(50)	Y			协议号
                                                                              zfbdzmx.TRADENO,//TRADENO	VARCHAR2(50)	Y			支付宝交易号
                                                                               InObject.BASEINFO.FENYUANDM,  //FENYUANDM	VARCHAR2(50)	Y			分院代码
                                                                              ""//SHOUKUANZH	VARCHAR2(50)	Y			收款帐号
                                                                              ));
                                            }
                                        }
                                    }
                                }

                            }

                        }
                    }


                }
                catch (Exception exp)
                {
                    throw new Exception(exp.Message.ToString());
                }
                #endregion
            #endregion
            }
            else
            {
                throw new Exception("下载类型不正确");
            }
        }

    }
}

