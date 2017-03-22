using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Data.Common;
using System.Data.OracleClient;
using HIS4.Schemas;
using System.Configuration;
using log4net;

namespace HIS4.Biz
{
    public class GUAHAOYY : IMessage<GUAHAOYY_IN,GUAHAOYY_OUT>
    {
        ILog log = log4net.LogManager.GetLogger("SqlInfo");
        public override void ProcessMessage()
        {
            OutObject = new GUAHAOYY_OUT();
            string yuyueLx = InObject.YUYUELX;
            string jiuzhenkLx = InObject.JIUZHENKLX;//就诊卡类型
            string jiuzhenKh = InObject.JIUZHENKH;//就诊卡号
            string zhengjianLx = InObject.ZHENGJIANLX;//证件类型
            string zhengjianHm = InObject.ZHENGJIANHM;//证件号码
            string xingMing = InObject.XINGMING;//姓名
            string yizhoupbId = InObject.YIZHOUPBID;//一周排班id;
            string dangtianpbId = InObject.DANGTIANPBID;//当天排班id
            string riQi = InObject.RIQI;//日期
            string guahaoBc = InObject.GUAHAOBC;//挂号班次
            string guahaoLb = InObject.GUAHAOLB;//挂号类别
            string keshiDm = InObject.KESHIDM;//科室代码
            string yishengDm = InObject.YISHENGDM;//医生代码
            string guahaoXh = InObject.GUAHAOXH;//挂号序号
            string yuyueLy = InObject.YUYUELY;//预约来源
            string lianxiDh = InObject.LIANXIDH;//手机号码
            string bingrenId = InObject.BINGRENID; //病人ID
            string xingBie = InObject.XINGBIE;//性别
            string FenYuanDM = InObject.BASEINFO.FENYUANDM;//分院代码
            string beiZhu = InObject.BEIZHU;//备注

            string czygh = ConfigurationManager.AppSettings["JRCaoZuoYGH"];//接入操作员工号
            string yuYueCZFS = ConfigurationManager.AppSettings["GUAHAOYYCZFS"];//挂号预约处理方式
            if (!string.IsNullOrEmpty(yuYueCZFS) && yuYueCZFS == "1")
            {
                #region 嘉兴悦城业务处理
                #region 基本入参判断
                //if (string.IsNullOrEmpty(zhengjianHm)) {
                //    throw new Exception("证件号码不能为空！");
                //}
                if (string.IsNullOrEmpty(beiZhu)) {
                    beiZhu = string.Empty;
                }

                if (string.IsNullOrEmpty(xingMing)) {
                    throw new Exception("姓名不能为空！");
                }

                if (string.IsNullOrEmpty(xingBie)) {
                    throw new Exception("性别不能为空！");
                }

                if (string.IsNullOrEmpty(lianxiDh)) {
                    throw new Exception("联系电话不能为空！");
                }

                if (string.IsNullOrEmpty(guahaoBc)) {
                    throw new Exception("挂号班次不能为空！请传入1 上午，2 下午！");
                }
                else if (guahaoBc != "1" && guahaoBc != "2") {
                    throw new Exception("挂号班次不能为空！请传入1 上午，2 下午！");
                }

                if (string.IsNullOrEmpty(xingBie)) {
                    xingBie = "0";
                }

                if (string.IsNullOrEmpty(yuyueLx)) {
                    yuyueLx = ConfigurationManager.AppSettings["GuaHaoYYLX"];//预约类型
                }
                #endregion

                string sXingBie = string.Empty;
                switch (xingBie) { 
                    case "0":
                        sXingBie = "未知";
                        break;
                    case "1":
                        sXingBie = "男";
                        break;
                    case"2":
                        sXingBie = "女";
                        break;
                    default:
                        sXingBie = "其它";
                        break;
                }

                string chuShenRQ = "null";
                if (!string.IsNullOrEmpty(zhengjianHm))
                {
                    chuShenRQ = string.Format("to_date('{0}','yyyy-mm-dd')", zhengjianHm.Substring(6, 8).Insert(4, "-").Insert(7, "-"));
                }
              
                string houzhensj = "select kaishisj,jieshusj from v_mz_houzhensj where paibanid = '{0}' and (shangxiawbz = '{1}') and (qishighxh>= '{2}' and jieshughxh <= '{2}')";
                DataTable dtHouZhensj = DBVisitor.ExecuteTable(string.Format(houzhensj, dangtianpbId, guahaoBc == "1" ? "0" : "1", guahaoXh));
                string kaishisj = string.Empty;
                string jieshusj = string.Empty;
                if (dtHouZhensj != null && dtHouZhensj.Rows.Count > 0)
                {
                    kaishisj = dtHouZhensj.Rows[0]["kaishisj"].ToString();
                    jieshusj = dtHouZhensj.Rows[0]["jieshusj"].ToString();
                }

                string PaiBanHYXX = @"select * from v_mz_guahaoyyxh_qbhy where paibanid = '{0}' and guahaoyyxh = '{1}' and shangxiawbz = '{2}'and yuanquid = '{3}' ";

                DataTable dtPBHYXX = DBVisitor.ExecuteTable(string.Format(PaiBanHYXX, dangtianpbId, guahaoXh, guahaoBc == "1" ? "0" : "1", FenYuanDM));
                if (dtPBHYXX == null || dtPBHYXX.Rows.Count <= 0)
                {
                    throw new Exception("未找到有效的号源信息！");
                }

                //号源占用判断
                DataTable dtYYYXX = DBVisitor.ExecuteTable(string.Format("select * from v_mz_guahaoyyxh_yshyh where paibanid = '{0}' and guahaoyyxh = '{1}' and shangxiawbz = '{2}' and yuanquid = '{3}' ", dangtianpbId, guahaoXh, ((guahaoBc == "1") ? 0 : 1), FenYuanDM));
                if (dtYYYXX.Rows.Count > 0)
                {
                    throw new Exception("该时间段已被预约，请选择其它时间段！");
                }

                string quHaoMM = DBVisitor.ExecuteScalar("select LPAD(seq_gy_jiuzhenyy.nextval,10,'0') from dual").ToString();

                string bingrenid = DBVisitor.ExecuteScalar(string.Format("select nvl(bingrenid,'') from gy_bingrenxx where xingming = '{0}' and xingbie = '{1}' and  jiatingdh = '{2}' ",xingMing,sXingBie,lianxiDh)).ToString();

                string insertYYXX = @"insert into gy_jiuzhenyy (jiuzhenyyid,yuyuesj,yisehngid,keshiid,xingming,
                                            shuruma1,shuruma2,shuruma3,xingbie,chushengrq,dianhua,
                                            yuyuerq,jiuzhenbz,jiluren,jieshusj,zuofeibz,yuyuely,beizhu,bingrenid,chuzhenbz )
                                            values( '{0}','{1}','{2}','{3}','{4}',
                                            fun_gy_getshuruma1('{4}',50),fun_gy_getshuruma2('{4}',50),fun_gy_getshuruma3('{4}',50),'{5}',{6},'{7}',
                                            to_date('{8}','yyyy-mm-dd'),'0','{9}','{10}','0','{11}','{12}','{13}',{14} )";
                DBVisitor.ExecuteNonQuery(string.Format(insertYYXX, quHaoMM, kaishisj, yishengDm, keshiDm,
                    xingMing, sXingBie, chuShenRQ, lianxiDh, riQi, InObject.BASEINFO.CAOZUOYDM, jieshusj, yuyueLx,beiZhu,
                    bingrenid,string.IsNullOrEmpty(bingrenid)?1:0));
                OutObject.GUAHAOXH = guahaoXh;
                OutObject.JIUZHENSJ = Unity.getJiuZhenSJD(yishengDm, keshiDm.ToString(), guahaoXh, yuyueLx, ((guahaoBc == "1") ? 0 : 1), guahaoLb, riQi);
                OutObject.QUHAOMM = quHaoMM;
                #endregion
            }
            else
            {
                #region 标准his4业务处理
                
                #region 基本入参判断
                if (string.IsNullOrEmpty(InObject.BASEINFO.CAOZUOYDM))
                {
                    throw new Exception("非法操作员，无法进行预约操作！");
                }
                if (!string.IsNullOrEmpty(czygh) && !czygh.Contains("^" + InObject.BASEINFO.CAOZUOYDM + "^"))
                {
                    throw new Exception("非法操作员，无法进行预约操作！");
                }


                //预约类型
                if (string.IsNullOrEmpty(yuyueLx))
                {
                    yuyueLx = ConfigurationManager.AppSettings["GuaHaoYYLX"];//预约类型
                }

                //性别
                if (string.IsNullOrEmpty(xingBie))
                {
                    xingBie = "未知";
                }
                else if (xingBie == "1")
                {
                    xingBie = "男";
                }
                else if (xingBie == "2")
                {
                    xingBie = "女";
                }
                else if (xingBie == "9")
                {
                    xingBie = "其他";
                }
                else
                {
                    xingBie = "";
                }

                if (!string.IsNullOrEmpty(zhengjianHm)) {
                    zhengjianHm = zhengjianHm.ToUpper();
                }

                //姓名
                if (string.IsNullOrEmpty(xingMing))
                {
                    throw new Exception("姓名不能为空！");
                }
                //就诊卡号
                if (string.IsNullOrEmpty(jiuzhenKh) && string.IsNullOrEmpty(zhengjianHm) && string.IsNullOrEmpty(bingrenId))
                {
                    throw new Exception("就诊卡号和证件号码不能同时为空！");
                }

                //排班id
                if (string.IsNullOrEmpty(dangtianpbId))
                {
                    throw new Exception("排班信息获取失败，请重新尝试挂号！");
                }
                //预约日期
                if (string.IsNullOrEmpty(riQi))
                {
                    throw new Exception("预约日期获取失败，请重新尝试挂号！");
                }
                //挂号班次
                if (string.IsNullOrEmpty(guahaoBc))
                {
                    throw new Exception("挂号班次获取失败，请重新尝试挂号！");
                }
                //挂号类别
                if (string.IsNullOrEmpty(guahaoLb))
                {
                    throw new Exception("挂号类别获取失败，请重新尝试挂号！");
                }
                //科室代码
                if (string.IsNullOrEmpty(keshiDm))
                {
                    throw new Exception("科室信息获取失败，请重新尝试挂号！");
                }
                //挂号序号
                if (string.IsNullOrEmpty(guahaoXh))
                {
                    throw new Exception("预约挂号序号获取失败，请重新尝试挂号！");
                }
                //联系电话
                if (string.IsNullOrEmpty(lianxiDh))
                {
                    throw new Exception("联系电话获取失败，请重新尝试挂号！");
                }

                if (string.IsNullOrEmpty(yishengDm))
                {
                    yishengDm = "*";
                }

                if (string.IsNullOrEmpty(yuyueLy))
                {
                    yuyueLy = ConfigurationManager.AppSettings["GuaHaoYYLY"];
                    if (string.IsNullOrEmpty(yuyueLy))
                    {
                        yuyueLy = "7";
                    }
                }
                #endregion



                string chushengrq = "";
                string xuexing = "";
                string hunyin = "";
                string zhiye = "";
                string guoji = "";
                string minzu = "";
                string jiwangshi = "";
                string guominshi = "";
                string jiatingdz = "";
                string lianxirdh = "";
                string lianxiren = "";
                string dianhua = "";
                string houzhenkssj = "";
                string houzhenjssj = "";
                if (!string.IsNullOrEmpty(jiuzhenKh) || !string.IsNullOrEmpty(zhengjianHm) || !string.IsNullOrEmpty(bingrenId))
                {
                    //获取病人个人信息
                    StringBuilder sbSql = new StringBuilder("select a.jiuzhenkh,a.shenfenzh,xingbie, to_char(a.chushengrq,'yyyy-mm-dd') chushengrq,a.bingrenid, a.xuexing,a.hunyin,");
                    sbSql.Append(" a.zhiye,a.guoji,a.minzu,a.jiwangshi,a.guominshi,a.jiatingdz,a.lianxirdh,a.lianxiren,a.dianhua,a.lianxidh ");
                    sbSql.Append("from gy_v_bingrenxx a where ((a.jiuzhenkh is not null and  a.jiuzhenkh = '" + jiuzhenKh + "') or (a.yibaokh is not null and   a.yibaokh = '" + jiuzhenKh + "') or  (a.bingrenid is not null and a.bingrenid ='" + bingrenId + "') or (a.shenfenzh is not null and a.shenfenzh = '" + zhengjianHm + "'))   order by a.yibaokh asc, a.xiugaisj desc ");
                    DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
                    if (dt.Rows.Count > 0)
                    {
                        jiuzhenKh = dt.Rows[0]["jiuzhenkh"].ToString();
                        bingrenId = dt.Rows[0]["bingrenid"].ToString();
                        zhengjianHm = dt.Rows[0]["shenfenzh"].ToString();
                        chushengrq = dt.Rows[0]["chushengrq"].ToString();
                        xuexing = dt.Rows[0]["xuexing"].ToString();
                        hunyin = dt.Rows[0]["hunyin"].ToString();
                        zhiye = dt.Rows[0]["zhiye"].ToString();
                        guoji = dt.Rows[0]["guoji"].ToString();
                        minzu = dt.Rows[0]["minzu"].ToString();
                        jiwangshi = dt.Rows[0]["jiwangshi"].ToString();
                        guominshi = dt.Rows[0]["guominshi"].ToString();
                        jiatingdz = dt.Rows[0]["jiatingdz"].ToString();
                        lianxirdh = dt.Rows[0]["lianxirdh"].ToString();
                        lianxiren = dt.Rows[0]["lianxiren"].ToString();
                        dianhua = dt.Rows[0]["dianhua"].ToString();
                        xingBie = dt.Rows[0]["xingbie"].ToString();
                        if (string.IsNullOrEmpty(lianxiDh))
                        {
                            lianxiDh = string.IsNullOrEmpty(dianhua) ? dt.Rows[0]["lianxidh"].ToString() : dianhua;
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(zhengjianHm))
                        {
                            throw new Exception("证件号码不能为空！");
                        }
                    }
                }

                if(bingrenId == "null" || bingrenId == "Null" || bingrenId =="NULL"){
                   bingrenId = string.Empty;
                }

                #region 科室可预约年龄判断
                if (!Unity.shiFouKeGHNianLin(keshiDm, zhengjianHm))
                {
                    throw new Exception("年龄条件不符合，不能在本科室挂号/预约！");
                }
                #endregion
                #region 科室可预约性别判断
                if (!Unity.shiFouKeGHXingBie(keshiDm, zhengjianHm))
                {
                    throw new Exception("性别条件不符合，不能在本科室挂号/预约！");
                }
                #endregion

                #region 获取候诊时间
                string guaHaoHYMS = ConfigurationManager.AppSettings["GuaHaoYMS"];//挂号号源模式
                if (!string.IsNullOrEmpty(guaHaoHYMS) && guaHaoHYMS == "1")
                {

                }
                else
                {
                    OutObject.JIUZHENSJ = Unity.getJiuZhenSJD(yishengDm, keshiDm.ToString(), guahaoXh, yuyueLx, ((guahaoBc == "1") ? 0 : 1), guahaoLb, riQi);
                }
                if (OutObject.JIUZHENSJ.Contains("-")) {
                    houzhenkssj = OutObject.JIUZHENSJ.Split('-')[0];
                    houzhenjssj = OutObject.JIUZHENSJ.Split('-')[1];
                }
                #endregion


                //拼装交易字符串（2014-07-08,参考来源：his4.GHPKG.P_GH_BOOK）
                string jiaoyiZfc = bingrenId;//外部病人ID为空
                jiaoyiZfc += "|" + jiuzhenKh;//就诊卡号
                jiaoyiZfc += "|";//医保卡号
                jiaoyiZfc += "|" + xingMing;//姓名
                jiaoyiZfc += "|" + xingBie;//性别
                jiaoyiZfc += "|" + zhengjianHm;//证件号码
                jiaoyiZfc += "|" + chushengrq;//出身日期
                jiaoyiZfc += "|" + xuexing;//血型
                jiaoyiZfc += "|" + hunyin.Replace('|', '-');//婚姻状况
                jiaoyiZfc += "|" + zhiye;//职业
                jiaoyiZfc += "|" + guoji;//国籍
                jiaoyiZfc += "|" + minzu;//民族
                jiaoyiZfc += "|" + jiwangshi;//既往史
                jiaoyiZfc += "|" + guominshi;//过敏史
                jiaoyiZfc += "|" + jiatingdz;//家庭地址为空
                jiaoyiZfc += "|" + lianxiDh;//联系电话
                jiaoyiZfc += "|" + lianxiren;//联系人
                jiaoyiZfc += "|" + lianxiDh;//联系电话
                jiaoyiZfc += "|" + (guahaoBc == "1" ? "S" : "X") + dangtianpbId;//排版ID
                jiaoyiZfc += "|2";//排班模式
                jiaoyiZfc += "|" + riQi;//就诊日期
                jiaoyiZfc += "|" + yuyueLx;//预约类型
                jiaoyiZfc += "|" + yuyueLy;//记录来源
                jiaoyiZfc += "|" + guahaoXh;//预约挂号就诊序号
                jiaoyiZfc += "|";
                jiaoyiZfc += "|";
                jiaoyiZfc += "|" + InObject.BASEINFO.CAOZUOYDM;
                jiaoyiZfc += "|" + String.Empty;//yuyuehao 取号密码字段
                jiaoyiZfc += "|" + lianxiDh;//家庭电话
                jiaoyiZfc += "|" ;//单位电话
                jiaoyiZfc += "|" + houzhenkssj;//候诊开始时间
                jiaoyiZfc += "|" + houzhenjssj;//候诊结束时间

                //提交预约号-------------------------------------------------------------------------------------------
                OracleParameter[] paramJiaoYi = new OracleParameter[3];
                paramJiaoYi[0] = new OracleParameter("PRM_JIAOYIZFC", OracleType.VarChar);
                paramJiaoYi[0].Value = jiaoyiZfc;
                paramJiaoYi[0].Direction = ParameterDirection.Input;
                paramJiaoYi[1] = new OracleParameter("PRM_APPCODE", OracleType.Number);
                paramJiaoYi[1].Value = null;
                paramJiaoYi[1].Direction = ParameterDirection.Output;
                paramJiaoYi[2] = new OracleParameter("PRM_DATABUFFER", OracleType.VarChar);
                paramJiaoYi[2].Value = null;
                paramJiaoYi[2].Size = 2000;
                paramJiaoYi[2].Direction = ParameterDirection.Output;

                log.InfoFormat("{0}", "执行存储过程：PKG_MZ_YUYUE.PRC_GY_TIJIAOYUYUEXX \r\nPRM_JIAOYIZFC：" + jiaoyiZfc + "\r\n");

                string returnValue = string.Empty;
                DbTransaction transaction = null;
                DbConnection conn = DBVisitor.Connection;
                try
                {
                    transaction = conn.BeginTransaction();
                    DBVisitor.ExecuteProcedure("PKG_MZ_YUYUE.PRC_GY_TIJIAOYUYUEXX", paramJiaoYi, transaction);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                        conn.Close();
                    }
                    throw new Exception(ex.Message);
                }
                //--------------------------------------------------------------------------------------------------  
                //String filePath = System.Environment.CurrentDirectory; //取当前系统路径
                string errMsg = string.Empty;
                returnValue = paramJiaoYi[2].Value.ToString();
                if (!returnValue.Contains("|"))
                {
                    transaction.Rollback();
                    conn.Close();
                    throw new Exception(returnValue);
                }

                if (returnValue.Length > 3 && returnValue.Substring(0, 3).ToUpper() == "ERR")//获取信息错误
                {
                    errMsg = returnValue.Substring(returnValue.IndexOf('|') + 1);
                    transaction.Rollback();
                    conn.Close();
                    throw new Exception(errMsg);
                }
                else
                {
                    string yuYueHao = string.Empty;
                    if (returnValue.Contains("|"))
                    {


                        yuYueHao = returnValue.Split('|')[1];
                        //此处需要处理返回成功的xml
                        OutObject.QUHAOMM = yuYueHao;
                        OutObject.GUAHAOXH = guahaoXh;

                        transaction.Commit();
                        conn.Close();

                    }
                    else
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw new Exception("存储过程返回值格式有误!");
                    }
                }
            #endregion
            }
        }
    }
}
